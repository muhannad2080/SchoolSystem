using System;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using SchoolSystem.DataAccess;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
        /// <summary>
        /// نافذة إعداد قاعدة البيانات التي تظهر أول مرة عند عدم وجود قاعدة البيانات.
        /// تتيح تحديد الخادم واسم قاعدة البيانات ونوع المصادقة ومسار النسخ الاحتياطية.
        /// اختبار الاتصال يفحص خادم SQL Server فقط (متوفر أم لا)، وإن توفر يُفعَّل
        /// زر "إنشاء قاعدة البيانات" الذي ينشئ قاعدة البيانات كاملة.
        /// </summary>
        public partial class DatabaseSetupForm : Form
        {
        private bool serverTested;

        /// <summary>
        /// كلمة مرور مدير النظام التي تم توليدها أثناء تجهيز قاعدة تثبيت جديدة.
        /// تبقى في ذاكرة العملية فقط حتى يعرضها Program مرة واحدة بعد إغلاق شاشة الإعداد.
        /// </summary>
        public static string LastFreshInstallAdminPassword { get; private set; }

        public DatabaseSetupForm()
        {
            LastFreshInstallAdminPassword = null;

            InitializeComponent();
            UIHelper.ApplyStyle(this);
            LoadCurrentSettings();
            ApplyAuthState(true);
            WireFieldChanged();
            btnCreate.Enabled = false;
            SetStatus(
                "يرجى اختبار الاتصال بالخادم أولاً. إن توفر الخادم سيُفعَّل زر إنشاء قاعدة البيانات.",
                UIHelper.MutedTextColor);
        }

            public void SetStartupHint(string message)
            {
                SetStatus(string.IsNullOrWhiteSpace(message)
                ? "يرجى اختبار الاتصال بالخادم أولاً. إن توفر الخادم سيُفعَّل زر إنشاء قاعدة البيانات."
                : message,
                UIHelper.WarningColor);
            }

        /// <summary>يربط أحداث تغيير الحقول بحيث يُعاد تعطيل زر الإنشاء عند أي تعديل.</summary>
        private void WireFieldChanged()
        {
            txtServer.TextChanged += (s, e) => OnConnectionFieldsChanged();
            txtDatabase.TextChanged += (s, e) => OnConnectionFieldsChanged();
            rbWindows.CheckedChanged += (s, e) => OnConnectionFieldsChanged();
            rbSql.CheckedChanged += (s, e) => OnConnectionFieldsChanged();
            txtSqlUser.TextChanged += (s, e) => OnConnectionFieldsChanged();
            txtSqlPass.TextChanged += (s, e) => OnConnectionFieldsChanged();
            txtBackupDir.TextChanged += (s, e) => OnConnectionFieldsChanged();
        }

        /// <summary>عند تعديل أي حقل اتصال، نلغي تفعيل زر الإنشاء حتى يُعاد الاختبار.</summary>
        private void OnConnectionFieldsChanged()
        {
            serverTested = false;
            btnCreate.Enabled = false;
        }

        /// <summary>لفّ سلسلة اتصال اختبارية وتخزين قيم المدخلات الحالية.</summary>
        private ApplicationSettingsData BuildDraft()
        {
            return new ApplicationSettingsData
            {
                ServerInstance = txtServer.Text.Trim(),
                DatabaseName = txtDatabase.Text.Trim(),
                UseSqlAuthentication = rbSql.Checked,
                SqlUser = rbSql.Checked ? txtSqlUser.Text.Trim() : string.Empty,
                BackupDirectory = txtBackupDir.Text.Trim()
            };
        }

        private void LoadCurrentSettings()
        {
            try
            {
                ApplicationSettingsData current = ApplicationSettingsService.Load();
                txtServer.Text = current.ServerInstance ?? ".";
                txtDatabase.Text = current.DatabaseName ?? "SchoolDB";
                if (current.UseSqlAuthentication)
                {
                    rbSql.Checked = true;
                    txtSqlUser.Text = current.SqlUser ?? string.Empty;
                    txtSqlPass.Text = ApplicationSettingsService.GetSqlPassword(current);
                }
                else
                {
                    rbWindows.Checked = true;
                }
                txtBackupDir.Text = current.BackupDirectory ?? Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SchoolSystem", "Backups");
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة إعداد قاعدة البيانات: تحميل الإعدادات الحالية", ex);
                txtServer.Text = ".";
                txtDatabase.Text = "SchoolDB";
                rbWindows.Checked = true;
            }
        }

        private void ApplyAuthState(bool initial)
        {
            bool sql = rbSql.Checked;
            lblSqlUser.Enabled = sql;
            pnlSqlUser.Enabled = sql;
            lblSqlPass.Enabled = sql;
            pnlSqlPass.Enabled = sql;
            btnTogglePassword.Enabled = sql;
            if (initial && !sql)
            {
                txtSqlUser.Clear();
                txtSqlPass.Clear();
            }
        }

        private void Auth_CheckedChanged(object sender, EventArgs e)
        {
            ApplyAuthState(false);
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            if (txtSqlPass.UseSystemPasswordChar)
            {
                txtSqlPass.UseSystemPasswordChar = false;
                btnTogglePassword.Text = "\uE8F1";
            }
            else
            {
                txtSqlPass.UseSystemPasswordChar = true;
                btnTogglePassword.Text = "\uE891";
            }
            txtSqlPass.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnBrowseBackup_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "اختر مجلداً لحفظ النسخ الاحتياطية لقاعدة البيانات";
                dialog.SelectedPath = txtBackupDir.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    txtBackupDir.Text = dialog.SelectedPath;
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                UIHelper.ShowWarning("يرجى تحديد اسم خادم SQL Server.");
                txtServer.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtDatabase.Text))
            {
                UIHelper.ShowWarning("يرجى إدخال اسم قاعدة البيانات.");
                txtDatabase.Focus();
                return false;
            }
            if (rbSql.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtSqlUser.Text))
                {
                    UIHelper.ShowWarning("يرجى إدخال اسم مستخدم SQL.");
                    txtSqlUser.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtSqlPass.Text))
                {
                    UIHelper.ShowWarning("يرجى إدخال كلمة مرور مستخدم SQL.");
                    txtSqlPass.Focus();
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(txtBackupDir.Text) || !Path.IsPathRooted(txtBackupDir.Text.Trim()))
            {
                UIHelper.ShowWarning("مسار النسخ الاحتياطية يجب أن يكون مساراً كاملاً صالحاً.");
                txtBackupDir.Focus();
                return false;
            }
            return true;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                UIHelper.ShowWarning("يرجى تحديد اسم خادم SQL Server.");
                txtServer.Focus();
                return;
            }

            SetBusy(true);
            try
            {
                // اختبار الاتصال بالخادم فقط: نتصل بقاعدة master (الخادم نفسه)،
                // دون استخدام اسم قاعدة البيانات، حتى نتحقق من توفر الخادم فقط.
                string server = txtServer.Text.Trim();
                bool useSql = rbSql.Checked;
                string sqlUser = useSql ? txtSqlUser.Text.Trim() : null;
                string sqlPass = useSql ? txtSqlPass.Text : null;

                System.Data.SqlClient.SqlConnectionStringBuilder b =
                    new System.Data.SqlClient.SqlConnectionStringBuilder
                    {
                        DataSource = server,
                        InitialCatalog = "master",
                        TrustServerCertificate = true,
                        ConnectTimeout = 15
                    };
                if (useSql)
                {
                    b.IntegratedSecurity = false;
                    b.UserID = sqlUser ?? string.Empty;
                    b.Password = sqlPass ?? string.Empty;
                }
                else
                {
                    b.IntegratedSecurity = true;
                }

                // فتح الاتصال على خيط عمل حتى لا تتجمد شاشة الإعداد أثناء مهلة الاتصال.
                string connectionString = b.ConnectionString;
                await Task.Run(() =>
                {
                    using (var con = new System.Data.SqlClient.SqlConnection(connectionString))
                    {
                        con.Open();
                    }
                });

                // الخادم متوفر وناجح: تفعيل زر إنشاء قاعدة البيانات.
                serverTested = true;
                SetStatus(
                    "تم الاتصال بخادم SQL Server بنجاح: الخادم متوفر. يمكنك الآن إنشاء قاعدة البيانات.",
                    UIHelper.SuccessColor);
                btnCreate.Enabled = true;
            }
            catch (Exception ex)
            {
                // الخادم غير متوفر أو لا يمكن الوصول إليه.
                ApplicationLogger.LogException("شاشة إعداد قاعدة البيانات: اختبار الاتصال بالخادم", ex);
                serverTested = false;
                btnCreate.Enabled = false;
                SetStatus(
                    "خادم SQL Server غير متوفر في الجهاز. تحقق من اسم الخادم أو من تشغيل خدمة SQL Server. («" +
                    SafeMessage(ex) + "»)",
                    UIHelper.DangerColor);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!serverTested)
            {
                SetStatus(
                    "يرجى اختبار الاتصال بالخادم أولاً. إن توفر الخادم سيُفعَّل زر الإنشاء.",
                    UIHelper.WarningColor);
                return;
            }

            SetBusy(true);
            try
            {
                if (!ValidateInputs())
                    return;

                ApplicationSettingsData updated = BuildDraft();
                if (rbSql.Checked)
                    ApplicationSettingsService.SetSqlPassword(updated, txtSqlPass.Text);

                ApplicationSettingsService.Save(updated);
                DbConnection.Reload();

                string databaseFolder = ResolveDatabaseFolder();

                ApplicationLogger.Log(
                    "DatabaseMigration",
                    "بدء تجهيز قاعدة البيانات بشكل غير هدّام على " +
                    updated.ServerInstance + " وقاعدة " + updated.DatabaseName + ".");

                string migrationResult = DatabaseMigrationService.Apply(
                    updated.ServerInstance,
                    updated.DatabaseName,
                    updated.UseSqlAuthentication,
                    updated.SqlUser,
                    ApplicationSettingsService.GetSqlPassword(updated),
                    databaseFolder);

                ApplicationLogger.Log("DatabaseMigration", migrationResult);

                if (DatabaseMigrationService.DatabaseCreatedInLastRun &&
                    !DatabaseMigrationService.DatabaseRestoredInLastRun)
                {
                    DatabaseMigrationService.ApplyFreshInstallationReferenceData(
                        updated.ServerInstance,
                        updated.DatabaseName,
                        updated.UseSqlAuthentication,
                        updated.SqlUser,
                        ApplicationSettingsService.GetSqlPassword(updated),
                        databaseFolder);

                    UserService userService = new UserService();
                    userService.InitializeFreshInstallation(
                        out string initialAdminPassword);

                    LastFreshInstallAdminPassword = initialAdminPassword;

                    ApplicationLogger.Log(
                        "DatabaseMigration",
                        "تم إكمال تهيئة التثبيت الأول: البيانات المرجعية وحساب مدير النظام.");
                }

                HealthCheckResult health = DatabaseHealthCheckService.Check(
                    updated.ServerInstance,
                    updated.DatabaseName,
                    updated.UseSqlAuthentication,
                    updated.SqlUser,
                    ApplicationSettingsService.GetSqlPassword(updated));

                ApplicationLogger.Log("DatabaseHealthCheck", health.Summarize());

                if (!health.StructuralHealthy)
                    throw new InvalidOperationException(health.Summarize());

                SetStatus(
                    "تم تجهيز قاعدة البيانات والبيانات المرجعية بنجاح دون حذف بيانات قاعدة موجودة.",
                    UIHelper.SuccessColor);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("فشل تجهيز قاعدة البيانات", ex);
                SetStatus(
                    "تعذر تجهيز قاعدة البيانات: " + SafeMessage(ex),
                    UIHelper.DangerColor);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy)
        {
            btnTest.Enabled = !busy;
            btnCancel.Enabled = !busy;
            btnCreate.Enabled = !busy && serverTested;

            // لا تسمح بتعديل بيانات الاتصال أثناء اختبارها أو أثناء الإنشاء،
            // حتى لا تصبح نتيجة الاختبار مرتبطة بقيم تختلف عن القيم المعروضة.
            txtServer.Enabled = !busy;
            txtDatabase.Enabled = !busy;
            rbWindows.Enabled = !busy;
            rbSql.Enabled = !busy;
            txtBackupDir.Enabled = !busy;
            btnBrowseBackup.Enabled = !busy;

            bool sqlAuthenticationEnabled = !busy && rbSql.Checked;
            lblSqlUser.Enabled = sqlAuthenticationEnabled;
            pnlSqlUser.Enabled = sqlAuthenticationEnabled;
            txtSqlUser.Enabled = sqlAuthenticationEnabled;
            lblSqlPass.Enabled = sqlAuthenticationEnabled;
            pnlSqlPass.Enabled = sqlAuthenticationEnabled;
            txtSqlPass.Enabled = sqlAuthenticationEnabled;
            btnTogglePassword.Enabled = sqlAuthenticationEnabled;

            UseWaitCursor = busy;
        }

        private void SetStatus(string text, Color color)
        {
            lblStatus.Text = text ?? string.Empty;
            lblStatus.ForeColor = color;
        }

        private static string SafeMessage(Exception ex)
        {
            if (ex == null)
                return string.Empty;
            return string.IsNullOrWhiteSpace(ex.Message) ? ex.ToString() : ex.Message;
        }

        private static string ResolveDatabaseFolder()
        {
            string deployed = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
            if (IsUsableDatabaseFolder(deployed))
                return deployed;

            string current = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
            while (current != null)
            {
                string candidate = Path.Combine(current, "Database");
                if (IsUsableDatabaseFolder(candidate))
                    return candidate;
                string parent = Directory.GetParent(current) != null ? Directory.GetParent(current).FullName : null;
                if (string.Equals(parent, current, StringComparison.OrdinalIgnoreCase))
                    break;
                current = parent;
            }

            throw new DirectoryNotFoundException(
                "لم يتم العثور على مجلد Database يحتوي سكربت الإنشاء (databaseschool.sql) أو مجلد initial بجوار التطبيق أو ضمن مسار المشروع.");
        }

        /// <summary>مجلد قاعدة بيانات قابل للاستخدام: يحتوي مجلد initial أو السكربت الكامل databaseschool.sql.</summary>
        private static bool IsUsableDatabaseFolder(string databaseFolder)
        {
            if (string.IsNullOrWhiteSpace(databaseFolder) || !Directory.Exists(databaseFolder))
                return false;
            if (Directory.Exists(Path.Combine(databaseFolder, "initial")))
                return true;
            return File.Exists(Path.Combine(databaseFolder, DatabaseMigrationService.FullSchemaScriptFileName));
        }

        private static string FindBackupFile(string databaseFolder)
        {
            if (string.IsNullOrWhiteSpace(databaseFolder) || !Directory.Exists(databaseFolder))
                return string.Empty;

            return Directory.GetFiles(databaseFolder, "*.bak", SearchOption.TopDirectoryOnly)
                .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault() ?? string.Empty;
        }

        private static bool DatabaseExists(string serverInstance, string databaseName, bool useSqlAuthentication, string sqlUser, string sqlPassword)
        {
            string connectionString = BuildConnectionString(serverInstance, "master", useSqlAuthentication, sqlUser, sqlPassword);
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("SELECT CASE WHEN DB_ID(@db) IS NOT NULL THEN 1 ELSE 0 END;", connection))
            {
                command.CommandTimeout = 30;
                command.Parameters.Add("@db", System.Data.SqlDbType.NVarChar, 128).Value = databaseName ?? string.Empty;
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) == 1;
            }
        }

        private static string BuildConnectionString(string serverInstance, string databaseName, bool useSqlAuthentication, string sqlUser, string sqlPassword)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = serverInstance == null ? string.Empty : serverInstance.Trim(),
                InitialCatalog = databaseName == null ? string.Empty : databaseName.Trim(),
                TrustServerCertificate = true,
                ConnectTimeout = 15
            };

            if (useSqlAuthentication)
            {
                builder.IntegratedSecurity = false;
                builder.UserID = sqlUser == null ? string.Empty : sqlUser.Trim();
                builder.Password = sqlPassword ?? string.Empty;
            }
            else
            {
                builder.IntegratedSecurity = true;
            }

            return builder.ConnectionString;
        }
    }
}