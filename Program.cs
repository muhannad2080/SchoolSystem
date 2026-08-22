using System;
using System.Threading;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
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

            try
            {
                StudentWorkflowSchemaService.EnsureReady();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("فشل تجهيز مخطط دورة الطالب", ex);
                MessageBox.Show(
                    "لا يمكن تشغيل النظام لأن مخطط قاعدة البيانات غير مكتمل أو غير قابل للاتصال.\n\n" + ex.Message +
                    "\n\nنفّذ ملفات الترحيل الموجودة في مجلد Databass ثم أعد تشغيل النظام.",
                    "قاعدة البيانات غير جاهزة",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

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
            ApplicationLogger.LogException("خطأ عام في التطبيق", exception);
        }
    }
}
