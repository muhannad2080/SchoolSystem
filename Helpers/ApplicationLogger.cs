using System;
using System.IO;

namespace SchoolSystem.Helpers
{
    /// <summary>
    /// يسجل الأخطاء محليًا دون السماح لفشل التسجيل بإيقاف التطبيق.
    /// </summary>
    public static class ApplicationLogger
    {
        private static readonly object SyncRoot = new object();

        public static string LogDirectory
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SchoolSystem",
                    "Logs");
            }
        }

        public static void LogException(string operation, Exception exception, string fileName)
        {
            if (exception == null)
                return;

            try
            {
                string safeOperation = string.IsNullOrWhiteSpace(operation) ? "عملية غير محددة" : operation.Trim();
                string safeFileName = string.IsNullOrWhiteSpace(fileName) ? "application-errors.log" : fileName.Trim();
                Directory.CreateDirectory(LogDirectory);
                string path = Path.Combine(LogDirectory, safeFileName);
                string entry = string.Format(
                    "[{0:yyyy-MM-dd HH:mm:ss}] {1} | {2}{3}",
                    DateTime.Now,
                    safeOperation,
                    exception,
                    Environment.NewLine);

                lock (SyncRoot)
                {
                    File.AppendAllText(path, entry);
                }
            }
            catch
            {
                // لا نسمح لفشل التسجيل أن يتسبب في خطأ إضافي.
            }
        }

        public static void LogException(string operation, Exception exception)
        {
            LogException(operation, exception, "application-errors.log");
        }
    }
}
