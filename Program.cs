using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SchoolSystem.DataAccess;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.UI;


namespace SchoolSystem
{

    internal static class Program
    {
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

                MessageBox.Show(
                    "لا يمكن تشغيل النظام لأن قاعدة البيانات غير جاهزة أو غير قابلة للاتصال.\n\n" + ex.Message,
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

        // تعريف الدالة CheckStartupState لتنفيذ العملية المرتبطة بها.
        private static StartupCheckResult CheckStartupState()
        {
            ApplicationSettingsData config = ApplicationSettingsService.Load();
            bool useSql = config.UseSqlAuthentication;
            string sqlUser = config.SqlUser;
            string sqlPassword = ApplicationSettingsService.GetSqlPassword(config);
            string effectiveConnection = DbConnection.BuildEffectiveConnectionString(config, 15);
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(effectiveConnection);

            string server = builder.DataSource ?? string.Empty;
            string database = builder.InitialCatalog ?? string.Empty;
            useSql = !builder.IntegratedSecurity;
            sqlUser = builder.UserID;
            sqlPassword = builder.Password;

            string masterConnection = BuildConnectionString(server, "master", useSql, sqlUser, sqlPassword, 15);

            Exception masterFailure;
            if (!CanOpenConnection(masterConnection, out masterFailure))
            {
                StartupState state = ClassifyMasterFailure(masterFailure);
                return new StartupCheckResult
                {
                    State = state,
                    Message = BuildMasterFailureMessage(state, server, masterFailure),
                    ShowSetupForm = state == StartupState.ServerUnavailable
                };
            }

            if (!DatabaseExists(masterConnection, database))
            {
                return new StartupCheckResult
                {
                    State = StartupState.DatabaseMissing,
                    Message = "لم يتم العثور على قاعدة البيانات " + database + ". يمكنك إنشاء قاعدة البيانات أو استيراد النسخة الاحتياطية الأولية.",
                    ShowSetupForm = true
                };
            }

            Exception databaseFailure;
            if (!CanOpenConnection(effectiveConnection, out databaseFailure))
            {
                return new StartupCheckResult
                {
                    State = ClassifyDatabaseFailure(databaseFailure),
                    Message = BuildDatabaseFailureMessage(database, databaseFailure),
                    ShowSetupForm = false
                };
            }

            TryApplyDatabaseMigrations(server, database, useSqlAuth, sqlUser, sqlPassword);
            HealthCheckResult health = DatabaseHealthCheckService.Check(server, database, useSql, sqlUser, sqlPassword);
            if (!health.StructuralHealthy)
            {
                return new StartupCheckResult
                {
                    State = StartupState.SchemaInvalid,
                    Message = "قاعدة البيانات موجودة ولكن تحتاج إلى إصلاح/ترقية.\n\n" + health.Summarize(),
                    ShowSetupForm = false
                };
            }

            return new StartupCheckResult
            {
                State = StartupState.Ready,
                Message = string.Empty,
                ShowSetupForm = false
            };
        }

        /// <summary>
        /// يطبّق ترحيلات قاعدة البيانات المعلّقة قبل فحص الصحة النهائي.
        /// أي خطأ في الترحيل يُسجّل ثم يتولى فحص الصحة تحديد ما إذا كانت القاعدة ما زالت غير جاهزة.
        /// </summary>
        private static void TryApplyDatabaseMigrations(
            string server,
            string database,
            bool useSqlAuth,
            string sqlUser,
            string sqlPassword)
        {
            try
            {
                string databaseFolder = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Database");

                if (!Directory.Exists(databaseFolder))
                {
                    ApplicationLogger.Log(
                        "DatabaseMigration",
                        "لم يتم العثور على مجلد Database بجوار التطبيق؛ تم تخطي الترحيلات التلقائية.");
                    return;
                }

                string result = DatabaseMigrationService.Apply(
                    server,
                    database,
                    useSqlAuth,
                    sqlUser,
                    sqlPassword,
                    databaseFolder);

                ApplicationLogger.Log("DatabaseMigration", result);
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "التحديث التلقائي لقاعدة البيانات عند بدء التشغيل",
                    ex);
            }
        }

        private static bool CanOpenConnection(string connectionString, out Exception failure)
        {
            failure = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                failure = ex;
                return false;
            }
        }

        private static bool DatabaseExists(string masterConnection, string database)
        {
            using (SqlConnection connection = new SqlConnection(masterConnection))
            using (SqlCommand command = new SqlCommand("SELECT CASE WHEN DB_ID(@db) IS NOT NULL THEN 1 ELSE 0 END;", connection))
            {
                command.CommandTimeout = 30;
                command.Parameters.Add("@db", System.Data.SqlDbType.NVarChar, 128).Value = database ?? string.Empty;
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) == 1;
            }
        }

        private static string BuildConnectionString(string server, string database,
            bool useSqlAuth, string sqlUser, string sqlPassword, int timeout)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = server ?? string.Empty,
                InitialCatalog = database ?? string.Empty,
                TrustServerCertificate = true,
                ConnectTimeout = timeout > 0 ? timeout : 15
            };

            if (useSqlAuth)
            {
                builder.IntegratedSecurity = false;
                builder.UserID = sqlUser ?? string.Empty;
                builder.Password = sqlPassword ?? string.Empty;
            }
            else
            {
                builder.IntegratedSecurity = true;
            }

            return builder.ConnectionString;
        }

        private static StartupState ClassifyMasterFailure(Exception exception)
        {
            SqlException sql = exception as SqlException;
            if (sql != null)
            {
                if (sql.Number == 18456)
                    return StartupState.AuthenticationFailed;
                if (sql.Number == 53 || sql.Number == 40 || sql.Number == 2 || sql.Number == 121 || sql.Number == 10054 || sql.Number == 10060 || sql.Number == 10061)
                    return StartupState.ServerUnavailable;
                if (sql.Number == 4060 || sql.Number == 4064 || sql.Number == 5120 || sql.Number == 942 || sql.Number == 927 || sql.Number == 229)
                    return StartupState.DatabaseConnectionFailed;
            }

            return StartupState.UnknownFailure;
        }

        private static StartupState ClassifyDatabaseFailure(Exception exception)
        {
            SqlException sql = exception as SqlException;
            if (sql != null)
            {
                if (sql.Number == 18456)
                    return StartupState.AuthenticationFailed;
                if (sql.Number == 4060 || sql.Number == 4064 || sql.Number == 5120 || sql.Number == 942 || sql.Number == 927 || sql.Number == 229)
                    return StartupState.DatabaseConnectionFailed;
                if (sql.Number == 53 || sql.Number == 40 || sql.Number == 2 || sql.Number == 121 || sql.Number == 10054 || sql.Number == 10060 || sql.Number == 10061)
                    return StartupState.ServerUnavailable;
            }

            return StartupState.DatabaseConnectionFailed;
        }

        private static string BuildMasterFailureMessage(StartupState state, string server, Exception exception)
        {
            string details = SafeExceptionMessage(exception);

            switch (state)
            {
                case StartupState.AuthenticationFailed:
                    return "فشلت مصادقة الاتصال بخادم SQL Server. تحقق من اسم المستخدم وكلمة المرور والصلاحيات.\n\n" + details;
                case StartupState.DatabaseConnectionFailed:
                    return "تم الوصول إلى SQL Server لكن تعذر فتح قاعدة البيانات أو الوصول إلى master.\n\n" + details;
                case StartupState.ServerUnavailable:
                    return "خادم SQL Server غير متاح أو خدمة SQL Server متوقفة. تحقق من اسم الخادم أو تشغيل الخدمة.\n\n" + details;
                default:
                    return "تعذر الوصول إلى SQL Server على " + server + ".\n\n" + details;
            }
        }

        private static string BuildDatabaseFailureMessage(string database, Exception exception)
        {
            return "تم العثور على قاعدة البيانات " + database + " ولكن تعذر الاتصال بها.\n\n" + SafeExceptionMessage(exception);
        }

        private static string SafeExceptionMessage(Exception exception)
        {
            if (exception == null)
                return string.Empty;
            return string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
        }

        // تعريف الدالة OnUiThreadException لتنفيذ العملية المرتبطة بها.
        private static void OnUiThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);

            MessageBox.Show(

                "حدث خطأ غير متوقع أثناء تنفيذ العملية. تم تسجيل الخطأ ويمكن متابعة العمل.",
                "خطأ في النظام",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        // تعريف الدالة OnUnhandledException لتنفيذ العملية المرتبطة بها.
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                LogException(exception);
            }
        }

        // تعريف الدالة LogException لتنفيذ العملية المرتبطة بها.
        private static void LogException(Exception exception)
        {
            ApplicationLogger.LogException("خطأ عام في التطبيق", exception);
        }
    }
}
