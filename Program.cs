using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SchoolSystem.UI;

namespace SchoolSystem
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += OnUiThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
            }
        }

        private static void OnUiThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);
            MessageBox.Show(
                "حدث خطأ غير متوقع أثناء تنفيذ العملية. تم تسجيل الخطأ ويمكن متابعة العمل.",
                "خطأ في النظام",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                LogException(exception);
            }
        }

        private static void LogException(Exception exception)
        {
            try
            {
                string directory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SchoolSystem",
                    "Logs");
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, "application-errors.log");
                File.AppendAllText(
                    path,
                    string.Format(
                        "[{0:yyyy-MM-dd HH:mm:ss}] {1}{2}",
                        DateTime.Now,
                        exception,
                        Environment.NewLine));
            }
            catch
            {
                // لا نسمح لفشل التسجيل بأن يتسبب في خطأ إضافي.
            }
        }
    }
}
