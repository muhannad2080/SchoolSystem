using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SchoolSystem.SetupBootstrapper
{
    internal static class Program
    {
        private const string ProductName = "نظام إدارة المدارس - SchoolSystem";
        private const string InstallDirectoryName = "SchoolSystem";
        private const string UninstallRegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\SchoolSystem";

        [STAThread]
        private static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (args.Any(a => string.Equals(a, "/uninstall", StringComparison.OrdinalIgnoreCase)))
                    return Uninstall();

                return Install();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "تعذر إكمال العملية.\n\n" + ex.Message,
                    ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return 1;
            }
        }

        private static int Install()
        {
            EnsureFramework();

            Process[] running = Process.GetProcessesByName("SchoolSystem");
            if (running.Length > 0)
            {
                MessageBox.Show(
                    "أغلق نظام إدارة المدارس المفتوح حاليًا ثم أعد تشغيل برنامج التثبيت.",
                    ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return 2;
            }

            string installPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                InstallDirectoryName);

            string tempRoot = Path.Combine(
                Path.GetTempPath(),
                "SchoolSystemSetup",
                Guid.NewGuid().ToString("N"));

            string payloadZip = Path.Combine(tempRoot, "Payload.zip");
            string extractedRoot = Path.Combine(tempRoot, "Payload");

            try
            {
                Directory.CreateDirectory(tempRoot);
                ExtractEmbeddedPayload(payloadZip);
                Directory.CreateDirectory(extractedRoot);
                ZipFile.ExtractToDirectory(payloadZip, extractedRoot);

                string installedExe = Path.Combine(installPath, "SchoolSystem.exe");
                if (Directory.Exists(installPath))
                    Directory.Delete(installPath, true);

                Directory.CreateDirectory(installPath);
                CopyDirectory(extractedRoot, installPath);

                if (!File.Exists(installedExe))
                    throw new FileNotFoundException("لم يتم العثور على SchoolSystem.exe بعد فك حزمة التثبيت.");

                string setupCopy = Path.Combine(installPath, "SchoolSystem-Setup.exe");
                string currentSetup = Assembly.GetExecutingAssembly().Location;
                if (!string.IsNullOrWhiteSpace(currentSetup) && File.Exists(currentSetup))
                    File.Copy(currentSetup, setupCopy, true);

                CreateShortcuts(installedExe, installPath);
                RegisterUninstall(setupCopy);

                MessageBox.Show(
                    "اكتمل تثبيت " + ProductName + " بنجاح.\n\n" +
                    "تم تثبيت البرنامج وقاعدة البيانات الأولية وملفات الترحيل داخل الحزمة.\n" +
                    "عند التشغيل الأول سيتعرف النظام تلقائيًا على SchoolDB الموجودة، أو ينشئها ويطبق المهاجرات إذا لم تكن موجودة.\n\n" +
                    "سيتم تشغيل النظام الآن.",
                    ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Process.Start(new ProcessStartInfo
                {
                    FileName = installedExe,
                    WorkingDirectory = installPath,
                    UseShellExecute = true
                });

                return 0;
            }
            finally
            {
                try
                {
                    if (Directory.Exists(tempRoot))
                        Directory.Delete(tempRoot, true);
                }
                catch
                {
                    // ملفات التثبيت المؤقتة ليست جزءًا من بيانات النظام.
                }
            }
        }

        private static int Uninstall()
        {
            string installPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                InstallDirectoryName);

            if (Process.GetProcessesByName("SchoolSystem").Length > 0)
            {
                MessageBox.Show(
                    "أغلق النظام قبل إلغاء التثبيت.",
                    ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return 2;
            }

            string cleanupScript = Path.Combine(
                Path.GetTempPath(),
                "SchoolSystem-Uninstall-" + Guid.NewGuid().ToString("N") + ".cmd");

            string escapedInstall = installPath.Replace("%", "%%");
            File.WriteAllText(
                cleanupScript,
                "@echo off\r\n" +
                "timeout /t 2 /nobreak >nul\r\n" +
                "rmdir /s /q \"" + escapedInstall + "\"\r\n" +
                "reg delete \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\SchoolSystem\" /f >nul 2>&1\r\n" +
                "del \"" + cleanupScript.Replace("%", "%%") + "\" >nul 2>&1\r\n",
                Encoding.ASCII);

            Process.Start(new ProcessStartInfo
            {
                FileName = cleanupScript,
                WorkingDirectory = Path.GetDirectoryName(cleanupScript),
                UseShellExecute = true,
                Verb = "runas"
            });

            return 0;
        }

        private static void EnsureFramework()
        {
            const string releaseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full";
            object release = Microsoft.Win32.Registry.GetValue(releaseKey, "Release", 0);
            int value;
            if (release == null || !int.TryParse(release.ToString(), out value) || value < 461808)
                throw new InvalidOperationException(
                    ".NET Framework 4.7.2 أو أحدث مطلوب قبل تثبيت SchoolSystem.");
        }

        private static void ExtractEmbeddedPayload(string destinationZip)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream source = assembly.GetManifestResourceStream("Payload.zip"))
            {
                if (source == null)
                    throw new InvalidOperationException("ملف بيانات التثبيت المضمّن غير موجود داخل Setup.exe.");

                using (FileStream target = File.Create(destinationZip))
                    source.CopyTo(target);
            }
        }

        private static void CopyDirectory(string source, string destination)
        {
            foreach (string directory in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                string relative = directory.Substring(source.Length)
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                Directory.CreateDirectory(Path.Combine(destination, relative));
            }

            foreach (string file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
            {
                string relative = file.Substring(source.Length)
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                string target = Path.Combine(destination, relative);
                string targetDirectory = Path.GetDirectoryName(target);
                if (!string.IsNullOrWhiteSpace(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                File.Copy(file, target, true);
            }
        }

        private static void CreateShortcuts(string exePath, string installPath)
        {
            string programs = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms),
                ProductName);

            Directory.CreateDirectory(programs);

            CreateShortcut(
                Path.Combine(programs, "SchoolSystem.lnk"),
                exePath,
                installPath);

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            if (string.IsNullOrWhiteSpace(desktop))
                desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            CreateShortcut(
                Path.Combine(desktop, ProductName + ".lnk"),
                exePath,
                installPath);
        }

        private static void CreateShortcut(string shortcutPath, string targetPath, string workingDirectory)
        {
            Type shellType = Type.GetTypeFromProgID("WScript.Shell");
            if (shellType == null)
                return;

            object shell = Activator.CreateInstance(shellType);
            object shortcut = shellType.InvokeMember(
                "CreateShortcut",
                BindingFlags.InvokeMethod,
                null,
                shell,
                new object[] { shortcutPath });

            Type shortcutType = shortcut.GetType();
            shortcutType.InvokeMember("TargetPath", BindingFlags.SetProperty, null, shortcut, new object[] { targetPath });
            shortcutType.InvokeMember("WorkingDirectory", BindingFlags.SetProperty, null, shortcut, new object[] { workingDirectory });
            shortcutType.InvokeMember("IconLocation", BindingFlags.SetProperty, null, shortcut, new object[] { targetPath + ",0" });
            shortcutType.InvokeMember("Description", BindingFlags.SetProperty, null, shortcut, new object[] { ProductName });
            shortcutType.InvokeMember("Save", BindingFlags.InvokeMethod, null, shortcut, null);
        }

        private static void RegisterUninstall(string setupPath)
        {
            using (Microsoft.Win32.RegistryKey root =
                   Microsoft.Win32.Registry.LocalMachine.CreateSubKey(UninstallRegistryPath))
            {
                if (root == null)
                    return;

                root.SetValue("DisplayName", ProductName);
                root.SetValue("DisplayVersion", "1.0.0");
                root.SetValue("Publisher", "SchoolSystem");
                root.SetValue("InstallLocation",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), InstallDirectoryName));
                root.SetValue("UninstallString", "\"" + setupPath + "\" /uninstall");
                root.SetValue("NoModify", 1, Microsoft.Win32.RegistryValueKind.DWord);
                root.SetValue("NoRepair", 1, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }
    }
}
