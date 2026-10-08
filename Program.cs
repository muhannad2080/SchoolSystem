using System;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SchoolSystem.DataAccess;
using SchoolSystem.Helpers;
using SchoolSystem.Security;
using SchoolSystem.Services;
using SchoolSystem.UI;


namespace SchoolSystem
{

    internal static class Program
    {
        private static string freshInstallAdminPassword;

        private enum StartupState
        {
            Ready,
            ServerUnavailable,
            DatabaseMissing,
            DatabaseConnectionFailed,
            SchemaInvalid,
            AuthenticationFailed,
            UnknownFailure
        }

        private sealed class StartupCheckResult
        {
            public StartupState State { get; set; }
            public string Message { get; set; }
            public bool ShowSetupForm { get; set; }
        }

        [STAThread]
        // تعريف الدالة Main لتنفيذ العملية المرتبطة بها.
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += OnUiThreadException;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                StartupCheckResult startup = CheckStartupState();
                if (startup.State != StartupState.Ready)
                {
                    if (startup.ShowSetupForm)
                    {
                        using (DatabaseSetupForm setup = new DatabaseSetupForm())
                        {
                            setup.SetStartupHint(startup.Message);
                            if (setup.ShowDialog() != DialogResult.OK)
                                return;

                            if (!string.IsNullOrWhiteSpace(DatabaseSetupForm.LastFreshInstallAdminPassword))
                            {
                                freshInstallAdminPassword =
                                    DatabaseSetupForm.LastFreshInstallAdminPassword;
                                DatabaseSetupForm.LastFreshInstallAdminPassword = null;
                            }
                        }

                        startup = CheckStartupState();
                        if (startup.State != StartupState.Ready)
                        {
                            MessageBox.Show(
                                startup.Message,
                                "فشل تجهيز قاعدة البيانات",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            startup.Message,
                            "فشل تجهيز قاعدة البيانات",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationLogger.LogException("فشل تجهيز قاعدة البيانات", ex);

                string detail = SafeExceptionMessage(ex);
                string logHint = "سجل الأخطاء: " + ApplicationLogger.LogFilePath;

                MessageBox.Show(
                    "لا يمكن تشغيل النظام لأن قاعدة البيانات غير جاهزة أو غير قابلة للاتصال.\n\n" +
                    detail + "\n\n" + logHint,
                    "قاعدة البيانات غير جاهزة",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // دورة التطبيق: تسجيل دخول -> MainForm -> تسجيل خروج -> شاشة الدخول.
            // لا تغلق Application.Run عند تسجيل الخروج؛ نبدأ دورة مصادقة جديدة.
            if (!string.IsNullOrWhiteSpace(freshInstallAdminPassword))