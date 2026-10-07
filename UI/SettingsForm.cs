using System;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using SchoolSystem.DataAccess;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Security;
using SchoolSystem.Services;


namespace SchoolSystem.UI
{

    public partial class SettingsForm : UserControl
    {
        // تعريف الدالة SchoolSettingsService لتنفيذ العملية المرتبطة بها.
        private readonly SchoolSettingsService settingsService = new SchoolSettingsService();

        private SchoolSettings settings;

        private byte[] pendingLogo;

        private string pendingLogoFileName;

        private ApplicationSettingsData applicationSettings;
        // تعريف الدالة DatabaseBackupService لتنفيذ العملية المرتبطة بها.
        private readonly DatabaseBackupService backupService = new DatabaseBackupService();

        // تعريف الدالة SettingsForm لتنفيذ العملية المرتبطة بها.
        public SettingsForm()
        {

            InitializeComponent();

            if (LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;


            saveButton.Click += SaveButton_Click;

            chooseLogoButton.Click += ChooseLogoButton_Click;

            removeLogoButton.Click += RemoveLogoButton_Click;

            testConnectionButton.Click += TestConnectionButton_Click;

            useSqlAuthCheckBox.CheckedChanged += delegate { ToggleSqlAuthFields(); };

            chooseBackupDirectoryButton.Click += ChooseBackupDirectoryButton_Click;

            createBackupButton.Click += CreateBackupButton_Click;

            chooseRestoreFileButton.Click += ChooseRestoreFileButton_Click;

            restoreButton.Click += RestoreButton_Click;

            emailNotificationsCheckBox.CheckedChanged += delegate { ToggleEmailFields(); };
            Load += SettingsForm_Load;

            // تقييد حقل منفذ SMTP: أرقام فقط وبحد أقصى 5 خانات.
            smtpPortTextBox.MaxLength = 5;
            smtpPortTextBox.KeyPress -= SmtpPort_KeyPress;
            smtpPortTextBox.KeyPress += SmtpPort_KeyPress;

            UIHelper.ApplyStyle(this);
            UIHelper.StylePrimaryButton(saveButton);
            UIHelper.StylePrimaryButton(chooseLogoButton);
            UIHelper.StyleButton(removeLogoButton, UIHelper.WarningColor);
            UIHelper.StylePrimaryButton(testConnectionButton);
            UIHelper.StylePrimaryButton(createBackupButton);
            UIHelper.StyleButton(restoreButton, UIHelper.WarningColor);
        }

        // تعريف الدالة SmtpPort_KeyPress للسماح بالأرقام فقط في منفذ SMTP.
        private void SmtpPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e == null)
                return;

            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // تعريف الدالة SettingsForm_Load لتنفيذ العملية المرتبطة بها.
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                ApplyAccessRules();


                settings = settingsService.Load();

                applicationSettings = ApplicationSettingsService.Load();
                BindSettings();
                BindApplicationSettings();
                statusLabel.Text = "بيانات المدرسة جاهزة للتعديل.";
            }

            catch (Exception ex)
            {
                statusLabel.Text = "تعذر تحميل بيانات المدرسة.";
                UIHelper.ShowException("تحميل إعدادات المدرسة", ex);
            }
        }

        // تعريف الدالة ApplyAccessRules لتنفيذ العملية المرتبطة بها.
        private void ApplyAccessRules()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            bool canView = CurrentUser.CanView("Settings") || CurrentUser.CanAccessModule("Settings");
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            bool canEdit = CurrentUser.CanEdit("Settings") || CurrentUser.HasPermission(PermissionKeys.SettingsManage);
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            bool canManageDatabase = CurrentUser.HasAny(PermissionKeys.SettingsEdit, PermissionKeys.SettingsManage);

            saveButton.Enabled = canEdit;
            chooseLogoButton.Enabled = canEdit;
            removeLogoButton.Enabled = canEdit;
            serverInstanceTextBox.Enabled = canEdit;
            databaseNameTextBox.Enabled = canEdit;
            useSqlAuthCheckBox.Enabled = canEdit;
            sqlUserTextBox.Enabled = canEdit && useSqlAuthCheckBox.Checked;
            sqlPasswordTextBox.Enabled = canEdit && useSqlAuthCheckBox.Checked;
            backupDirectoryTextBox.Enabled = canEdit;
            restoreDatabaseTextBox.Enabled = canManageDatabase;
            restoreFileTextBox.Enabled = canManageDatabase;
            chooseBackupDirectoryButton.Enabled = canEdit;
            testConnectionButton.Enabled = canView;
            createBackupButton.Enabled = canManageDatabase;
            chooseRestoreFileButton.Enabled = canManageDatabase;
            replaceExistingCheckBox.Enabled = canManageDatabase;
            restoreButton.Enabled = canManageDatabase;
            languageComboBox.Enabled = canEdit;
            currencyTextBox.Enabled = canEdit;
            dateFormatTextBox.Enabled = canEdit;
            receiptPrefixTextBox.Enabled = canEdit;
            paymentPrefixTextBox.Enabled = canEdit;
            emailNotificationsCheckBox.Enabled = canEdit;
            smtpHostTextBox.Enabled = canEdit;
            smtpPortTextBox.Enabled = canEdit;
            smtpUsernameTextBox.Enabled = canEdit;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            smtpPasswordTextBox.Enabled = canEdit;
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            sslCheckBox.Enabled = canEdit;
        }

        // تعريف الدالة ToggleEmailFields لتنفيذ العملية المرتبطة بها.
        private void ToggleEmailFields()
        {
            bool enabled = emailNotificationsCheckBox.Checked && emailNotificationsCheckBox.Enabled;
            smtpHostTextBox.Enabled = enabled;
            smtpPortTextBox.Enabled = enabled;
            smtpUsernameTextBox.Enabled = enabled;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            smtpPasswordTextBox.Enabled = enabled;
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            sslCheckBox.Enabled = enabled;
        }

        // تعريف الدالة ToggleSqlAuthFields لتنفيذ العملية المرتبطة بها.
        private void ToggleSqlAuthFields()
        {
            if (useSqlAuthCheckBox == null)
                return;
            bool enabled = useSqlAuthCheckBox.Checked && useSqlAuthCheckBox.Enabled;
            sqlUserLabel.Enabled = enabled;
            sqlUserTextBox.Enabled = enabled;
            sqlPasswordLabel.Enabled = enabled;
            sqlPasswordTextBox.Enabled = enabled;
        }

        // تعريف الدالة ParsePort لتنفيذ العملية المرتبطة بها.
        private static int ParsePort(string value)
        {
            int port;
            if (!int.TryParse(value, out port) || port < 1 || port > 65535)

                throw new ArgumentException("منفذ SMTP يجب أن يكون رقماً بين 1 و65535.");

            return port;
        }

        // تعريف الدالة BindApplicationSettings لتنفيذ العملية المرتبطة بها.
        private void BindApplicationSettings()
        {
            serverInstanceTextBox.Text = applicationSettings.ServerInstance ?? string.Empty;
            databaseNameTextBox.Text = applicationSettings.DatabaseName ?? string.Empty;
            useSqlAuthCheckBox.Checked = applicationSettings.UseSqlAuthentication;
            sqlUserTextBox.Text = applicationSettings.SqlUser ?? string.Empty;
            sqlPasswordTextBox.Text = ApplicationSettingsService.GetSqlPassword(applicationSettings);
            backupDirectoryTextBox.Text = applicationSettings.BackupDirectory ?? string.Empty;
            restoreDatabaseTextBox.Text = applicationSettings.DatabaseName ?? string.Empty;
            languageComboBox.Text = applicationSettings.Language ?? "ar";
            currencyTextBox.Text = applicationSettings.CurrencySymbol ?? "ر.ي";
            dateFormatTextBox.Text = applicationSettings.DateFormat ?? "yyyy/MM/dd";
            receiptPrefixTextBox.Text = applicationSettings.ReceiptPrefix ?? "REC";
            paymentPrefixTextBox.Text = applicationSettings.PaymentPrefix ?? "PAY";
            emailNotificationsCheckBox.Checked = applicationSettings.EnableEmailNotifications;
            smtpHostTextBox.Text = applicationSettings.SmtpHost ?? string.Empty;
            smtpPortTextBox.Text = applicationSettings.SmtpPort.ToString();
            smtpUsernameTextBox.Text = applicationSettings.SmtpUsername ?? string.Empty;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            smtpPasswordTextBox.Text = applicationSettings.SmtpPassword ?? string.Empty;
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            sslCheckBox.Checked = applicationSettings.EnableSsl;
            replaceExistingCheckBox.Checked = false;
            ToggleEmailFields();
        }

        // تعريف الدالة SaveApplicationSettings لتنفيذ العملية المرتبطة بها.
        private void SaveApplicationSettings()
        {
            ApplicationSettingsData updated = new ApplicationSettingsData
            {
                ServerInstance = serverInstanceTextBox.Text.Trim(),
                DatabaseName = databaseNameTextBox.Text.Trim(),
                UseSqlAuthentication = useSqlAuthCheckBox.Checked,
                SqlUser = useSqlAuthCheckBox.Checked ? sqlUserTextBox.Text.Trim() : string.Empty,
                BackupDirectory = backupDirectoryTextBox.Text.Trim(),
                Language = languageComboBox.Text.Trim(),
                CurrencySymbol = currencyTextBox.Text.Trim(),
                DateFormat = dateFormatTextBox.Text.Trim(),
                ReceiptPrefix = receiptPrefixTextBox.Text.Trim(),
                PaymentPrefix = paymentPrefixTextBox.Text.Trim(),
                EnableEmailNotifications = emailNotificationsCheckBox.Checked,
                SmtpHost = smtpHostTextBox.Text.Trim(),
                SmtpPort = ParsePort(smtpPortTextBox.Text),
                SmtpUsername = smtpUsernameTextBox.Text.Trim(),
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                SmtpPassword = smtpPasswordTextBox.Text,
                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                EnableSsl = sslCheckBox.Checked

            };

            if (useSqlAuthCheckBox.Checked)
                ApplicationSettingsService.SetSqlPassword(updated, sqlPasswordTextBox.Text);

            ApplicationSettingsService.Save(updated);
            applicationSettings = updated;
            DbConnection.Reload();
        }

        // تعريف الدالة BindSettings لتنفيذ العملية المرتبطة بها.
        private void BindSettings()
        {
            schoolNameArTextBox.Text = settings.SchoolNameAr ?? string.Empty;
            schoolNameEnTextBox.Text = settings.SchoolNameEn ?? string.Empty;
            organizationNameArTextBox.Text = settings.OrganizationNameAr ?? string.Empty;
            organizationNameEnTextBox.Text = settings.OrganizationNameEn ?? string.Empty;
            addressArTextBox.Text = settings.AddressAr ?? string.Empty;
            addressEnTextBox.Text = settings.AddressEn ?? string.Empty;
            phoneTextBox.Text = settings.Phone ?? string.Empty;
            additionalPhoneTextBox.Text = settings.AdditionalPhone ?? string.Empty;
            emailTextBox.Text = settings.Email ?? string.Empty;
            websiteTextBox.Text = settings.Website ?? string.Empty;
            principalNameArTextBox.Text = settings.PrincipalNameAr ?? string.Empty;
            principalNameEnTextBox.Text = settings.PrincipalNameEn ?? string.Empty;
            licenseNumberTextBox.Text = settings.LicenseNumber ?? string.Empty;
            pendingLogo = settings.Logo == null ? null : (byte[])settings.Logo.Clone();
            pendingLogoFileName = settings.LogoFileName;
            ShowLogo(pendingLogo);
        }

        // تعريف الدالة ChooseLogoButton_Click لتنفيذ العملية المرتبطة بها.
        private void ChooseLogoButton_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.CanAccessModule("Settings"))
            {
                UIHelper.ShowWarning("لا تملك صلاحية تعديل بيانات المدرسة.");

                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "اختيار شعار المدرسة";
                dialog.Filter = "صور الشعار (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                dialog.Multiselect = false;
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                FileInfo file = new FileInfo(dialog.FileName);
                if (file.Length <= 0 || file.Length > 5 * 1024 * 1024)
                {
                    UIHelper.ShowWarning("حجم الشعار يجب أن يكون أكبر من صفر وألا يتجاوز 5 ميجابايت.");

                    return;
                }

                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    using (Image image = Image.FromFile(dialog.FileName))
                    {
                        pendingLogo = File.ReadAllBytes(dialog.FileName);
                        pendingLogoFileName = file.Name;
                    }
                    ShowLogo(pendingLogo);
                    statusLabel.Text = "تمت معاينة الشعار. اضغط حفظ التغييرات لتثبيته.";
                }

                catch (Exception ex)
                {
                    ApplicationLogger.LogException("شاشة الإعدادات: اختيار شعار المدرسة", ex);
                    pendingLogo = null;
                    pendingLogoFileName = null;
                    UIHelper.ShowWarning("الملف المختار ليس صورة صالحة.");
                }
            }
        }

        // تعريف الدالة RemoveLogoButton_Click لتنفيذ العملية المرتبطة بها.
        private void RemoveLogoButton_Click(object sender, EventArgs e)
        {
            pendingLogo = null;
            pendingLogoFileName = null;
            ShowLogo(null);
            statusLabel.Text = "تمت إزالة الشعار من المعاينة. اضغط حفظ التغييرات لتأكيد الحذف.";
        }

        // تعريف الدالة ShowLogo لتنفيذ العملية المرتبطة بها.
        private void ShowLogo(byte[] bytes)
        {
            if (logoPictureBox.Image != null)
            {
                Image old = logoPictureBox.Image;
                logoPictureBox.Image = null;

                old.Dispose();
            }

            if (bytes == null || bytes.Length == 0)
            {
                logoPictureBox.BackColor = Color.FromArgb(241, 245, 249);
                logoPlaceholderLabel.Visible = true;
                return;
            }

            using (MemoryStream stream = new MemoryStream(bytes))
            using (Image image = Image.FromStream(stream))
                logoPictureBox.Image = new Bitmap(image);
            logoPictureBox.BackColor = Color.White;
            logoPlaceholderLabel.Visible = false;
        }

        // تعريف الدالة TestConnectionButton_Click لتنفيذ العملية المرتبطة بها.
        private void TestConnectionButton_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                ApplicationSettingsData preview = applicationSettings ?? new ApplicationSettingsData();
                preview.ServerInstance = serverInstanceTextBox.Text.Trim();
                preview.DatabaseName = databaseNameTextBox.Text.Trim();
                preview.UseSqlAuthentication = useSqlAuthCheckBox.Checked;
                preview.SqlUser = sqlUserTextBox.Text.Trim();
                if (useSqlAuthCheckBox.Checked)
                    ApplicationSettingsService.SetSqlPassword(preview, sqlPasswordTextBox.Text);

                string connection = SchoolSystem.DataAccess.DbConnection.BuildEffectiveConnectionString(preview, 15);
                using (var con = new System.Data.SqlClient.SqlConnection(connection))
                {
                    con.Open();
                }
                systemStatusLabel.Text = "تم الاتصال بقاعدة البيانات بنجاح.";
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("اختبار اتصال قاعدة البيانات", ex);
            }
        }

        // تعريف الدالة ChooseBackupDirectoryButton_Click لتنفيذ العملية المرتبطة بها.
        private void ChooseBackupDirectoryButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "اختر مجلداً خارج مجلد البرنامج لحفظ النسخ الاحتياطية";
                dialog.SelectedPath = backupDirectoryTextBox.Text;
                if (dialog.ShowDialog() == DialogResult.OK)
                    backupDirectoryTextBox.Text = dialog.SelectedPath;
            }
        }

        // تعريف الدالة CreateBackupButton_Click لتنفيذ العملية المرتبطة بها.
        private void CreateBackupButton_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                SaveApplicationSettings();
                string path = backupService.Backup(applicationSettings.ServerInstance, applicationSettings.DatabaseName, applicationSettings.BackupDirectory);
                systemStatusLabel.Text = "تم إنشاء النسخة الاحتياطية: " + path;
                UIHelper.ShowInformation("تم إنشاء النسخة الاحتياطية بنجاح.");
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("إنشاء نسخة احتياطية", ex);
            }
        }

        // تعريف الدالة ChooseRestoreFileButton_Click لتنفيذ العملية المرتبطة بها.
        private void ChooseRestoreFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "اختيار ملف النسخة الاحتياطية";
                dialog.Filter = "SQL Backup (*.bak)|*.bak|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                    restoreFileTextBox.Text = dialog.FileName;
            }
        }

        // تعريف الدالة RestoreButton_Click لتنفيذ العملية المرتبطة بها.
        private void RestoreButton_Click(object sender, EventArgs e)
        {

            if (!UIHelper.ShowConfirmation("استعادة قاعدة البيانات قد تستبدل البيانات الحالية. هل تريد المتابعة؟", "تأكيد الاستعادة"))

                return;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                SaveApplicationSettings();
                backupService.Restore(applicationSettings.ServerInstance, restoreFileTextBox.Text, restoreDatabaseTextBox.Text, replaceExistingCheckBox.Checked);
                DbConnection.Reload();
                systemStatusLabel.Text = "تمت استعادة قاعدة البيانات بنجاح. أعد تشغيل النظام قبل متابعة العمل.";
                UIHelper.ShowInformation("تمت الاستعادة. يوصى بإعادة تشغيل البرنامج.");
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("استعادة قاعدة البيانات", ex);
            }
        }

        // تعريف الدالة SaveButton_Click لتنفيذ العملية المرتبطة بها.
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.CanAccessModule("Settings"))
            {
                UIHelper.ShowWarning("لا تملك صلاحية حفظ إعدادات المدرسة.");

                return;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                SchoolSettings updated = new SchoolSettings
                {
                    SchoolSettingsID = settings == null ? 1 : settings.SchoolSettingsID,
                    SchoolNameAr = schoolNameArTextBox.Text.Trim(),
                    SchoolNameEn = schoolNameEnTextBox.Text.Trim(),
                    OrganizationNameAr = NullIfEmpty(organizationNameArTextBox.Text),
                    OrganizationNameEn = NullIfEmpty(organizationNameEnTextBox.Text),
                    AddressAr = NullIfEmpty(addressArTextBox.Text),
                    AddressEn = NullIfEmpty(addressEnTextBox.Text),
                    Phone = NullIfEmpty(phoneTextBox.Text),
                    AdditionalPhone = NullIfEmpty(additionalPhoneTextBox.Text),
                    Email = NullIfEmpty(emailTextBox.Text),
                    Website = NullIfEmpty(websiteTextBox.Text),
                    PrincipalNameAr = NullIfEmpty(principalNameArTextBox.Text),
                    PrincipalNameEn = NullIfEmpty(principalNameEnTextBox.Text),
                    LicenseNumber = NullIfEmpty(licenseNumberTextBox.Text),
                    Logo = pendingLogo,
                    LogoFileName = pendingLogoFileName,
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    UpdatedByUserID = CurrentUser.User == null ? (int?)null : CurrentUser.User.UserID

                };

                SchoolSettingsService.Validate(updated);

                settingsService.Save(updated);
                SaveApplicationSettings();
                settings = updated;
                statusLabel.Text = "تم حفظ بيانات المدرسة وتحديث الشعار بنجاح.";

                new AuditLogService().Record("تعديل بيانات المدرسة", "SchoolSettings", "1", "تحديث هوية المدرسة والشعار");
                DataRefreshNotifier.Notify("All", this);
                UIHelper.ShowInformation("تم حفظ بيانات المدرسة بنجاح.");
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("حفظ بيانات المدرسة", ex);
            }
        }

        // تعريف الدالة NullIfEmpty لتنفيذ العملية المرتبطة بها.
        private static string NullIfEmpty(string value)
        {

            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
