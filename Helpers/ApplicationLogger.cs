using System;
using System.IO;
using System.Reflection;


namespace SchoolSystem.Helpers
{
    /// <summary>
    /// يسجل الأخطاء والرسائل التشخيصية محليًا في ملف موحّد داخل مجلد المشروع
    /// مع تدوير تلقائي للملف عند تجاوز الحد. فشل التسجيل نفسه لا يوقف التطبيق؛
    /// عند تعذر مجلد المشروع تتم محاولة مجلد التطبيق ثم مجلد مؤقت بديل
    /// قبل الاستسلام الصامت.
    /// </summary>

    public static class ApplicationLogger
    {
        /// <summary>الحد الأقصى لحجم ملف السجل الواحد قبل تدويره إلى .old.</summary>
        private const long MaxLogSizeBytes = 10 * 1024 * 1024;

        private static readonly object SyncRoot = new object();

        private static string _resolvedDirectory;

        /// <summary>
        /// مجلد السجلات: يُحل تلقائياً للوصول إلى مجلد المشروع.
        /// البحث يبدأ من مجلد التنفيذ ويرفع درجة حتى يجد الملف SchoolSystem.csproj
        /// أو ملف الحل SchoolSystem.sln (مجلد جذر المشروع).
        /// </summary>
        public static string LogDirectory
        {
            get
            {
                if (_resolvedDirectory != null)
                    return _resolvedDirectory;

                _resolvedDirectory = ResolveProjectLogDirectory();

                return _resolvedDirectory;
            }
        }

        /// <summary>إعادة حل مجلد السجلات (تُستدعى عند تغيير مجلد العمل).</summary>
        public static void ResetDirectory()
        {
            _resolvedDirectory = null;
        }

        /// <summary>
        /// يحاول تحديد مجلد جذر المشروع (الذي يحتوي فيه SchoolSystem.csproj
        /// أو SchoolSystem.sln) ثم ينشئ بداخله مجلد `Logs`. إن تعذر ذلك يتحول إلى
        /// مجلد بيانات التطبيق المحلي، ثم أخيراً إلى المجلد المؤقت.
        /// </summary>
        private static string ResolveProjectLogDirectory()
        {
            // 1) محاولة إيجاد مجلد المشروع (حيث يوجد .csproj أو .sln).
            string currentDir = AppDomain.CurrentDomain.BaseDirectory ?? AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            string projectRoot = FindProjectRoot(currentDir);

            if (projectRoot != null)
            {
                try
                {
                    string logsInProject = Path.Combine(projectRoot, "Logs");
                    Directory.CreateDirectory(logsInProject);
                    return logsInProject;
                }
                catch
                {
                    // قد يكون مجلد المشروع للقراءة فقط (مثلاً أثناء التثبيت)؛ ننتقل للبديل.
                }
            }

            // 2) بديل: مجلد التطبيق المحلي (LocalApplicationData/SchoolSystem/Logs).
            try
            {
                string localData = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SchoolSystem",
                    "Logs");
                Directory.CreateDirectory(localData);
                return localData;
            }
            catch
            {
                // 3) بديل أخير: المجلد المؤقت.
            }

            return Path.Combine(Path.GetTempPath(), "SchoolSystem");
        }

        /// <summary>
        /// يرتقي في مجلدات الأبوة حتى يعثر على مجلد يحتوي على SchoolSystem.csproj
        /// أو SchoolSystem.sln (مجلد جذر المشروع). يعيد null إن لم يوجد.
        /// </summary>
        private static string FindProjectRoot(string startDir)
        {
            DirectoryInfo dir;
            try
            {
                dir = new DirectoryInfo(startDir);
            }
            catch
            {
                return null;
            }

            for (int i = 0; i < 12 && dir != null; i++)
            {
                try
                {
                    if (File.Exists(Path.Combine(dir.FullName, "SchoolSystem.csproj")) ||
                        File.Exists(Path.Combine(dir.FullName, "SchoolSystem.sln")))
                    {
                        return dir.FullName;
                    }
                }
                catch
                {
                    // نتجاهل أي خطأ في مجلدات القراءة.
                }

                dir = dir.Parent;
            }

            return null;
        }

        /// <summary>مسار ملف السجل الموحّد.</summary>
        public static string LogFilePath
        {
            get { return Path.Combine(LogDirectory, "application-errors.log"); }
        }

        /// <summary>محتويات ملف السجل الحالية (تُعرض للمستخدم عند الطلب).</summary>
        public static string ReadLogTail(int maxChars = 4000)
        {
            try
            {
                if (!File.Exists(LogFilePath))
                    return string.Empty;

                using (var fs = new FileStream(LogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs))
                {
                    fs.Seek(0, SeekOrigin.End);
                    long length = fs.Length;
                    long start = Math.Max(0, length - maxChars);
                    fs.Seek(start, SeekOrigin.Begin);

                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>مسجل رسالة تشخيصية (بدون استثناء) في السجل الموحد.</summary>
        public static void Log(string operation, string message)
        {
            Write(operation, message ?? string.Empty);
        }

        public static void LogException(string operation, Exception exception, string fileName)
        {
            if (exception == null)
                return;

            Write(operation, exception.ToString());
        }

        public static void LogException(string operation, Exception exception)
        {
            if (exception == null)
                return;

            Write(operation, exception.ToString());
        }

        /// <summary>يسجل رسالة استخدام (نشاط مستخدم) لمساعدتنا في تتبع سلوك النظام.</summary>
        public static void LogUsage(string screen, string action, string details)
        {
            string operation = string.IsNullOrWhiteSpace(screen) ? "واجهة" : screen.Trim();
            if (!string.IsNullOrWhiteSpace(action))
                operation += " > " + action.Trim();

            Write(operation, "استخدام: " + (details ?? string.Empty));
        }

        /// <summary>يسجل حدثاً محتملاً (تحذير) دون إيقاف التطبيق.</summary>
        public static void LogWarning(string operation, string message)
        {
            Write("[تحذير] " + operation, message ?? string.Empty);
        }

        private static void Write(string operation, string details)
        {
            try
            {
                string safeOperation = string.IsNullOrWhiteSpace(operation) ? "عملية غير محددة" : operation.Trim();
                string entry = string.Format(
                    "[{0:yyyy-MM-dd HH:mm:ss}] {1} | {2}{3}",
                    DateTime.Now,
                    safeOperation,
                    details,
                    Environment.NewLine);

                lock (SyncRoot)
                {
                    string path = LogFilePath;
                    Directory.CreateDirectory(LogDirectory);

                    RotateIfNeeded(path);

                    File.AppendAllText(path, entry);
                }
            }
            catch (Exception primaryFailure)
            {
                // المجلد الافتراضي غير متاح (صلاحيات/ملف شخصي موجّه)؛ نحاول مجلدًا مؤقتًا بديلًا
                // حتى لا تضيع تشخيصات الأعطال الحرجة.
                try
                {
                    string fallbackPath = Path.Combine(Path.GetTempPath(), "SchoolSystem-application-errors.log");
                    string entry = string.Format(
                        "[{0:yyyy-MM-dd HH:mm:ss}] {1} | {2}{3}",
                        DateTime.Now,
                        operation ?? "عملية غير محددة",
                        details,
                        Environment.NewLine);
                    File.AppendAllText(fallbackPath, entry);
                }
                catch
                {
                    // لا يوجد أي مخرج متاح؛ لا يمكن فعل شيء دون إيقاف التطبيق بسبب فشل تسجيل.
                    _ = primaryFailure;
                }
            }
        }

        private static void RotateIfNeeded(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return;

                var info = new FileInfo(path);
                if (info.Length < MaxLogSizeBytes)
                    return;

                string rotated = Path.ChangeExtension(path, ".old.log");

                if (File.Exists(rotated))
                    File.Delete(rotated);

                File.Move(path, rotated);
            }
            catch
            {
                // التدوير تحسين غير حرج؛ لا يمنع كتابة السجل.
            }
        }
    }
}
