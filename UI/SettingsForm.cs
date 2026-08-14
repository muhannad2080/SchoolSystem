using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Security;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class SettingsForm : UserControl
    {
        private readonly DatabaseBackupService backupService = new DatabaseBackupService();
        private ApplicationSettingsData settings;

        public SettingsForm()
        {
            InitializeComponent();
            UIHelper.ApplyStyle(this);
            settings = ApplicationSettingsService.Load();
            serverTextBox.Text = settings.ServerInstance;
            databaseTextBox.Text = settings.DatabaseName;
            backupDirectoryTextBox.Text = settings.BackupDirectory;
            statusLabel.Text = "جاهز";
            UIHelper.StyleTextBox(serverTextBox);
            UIHelper.StyleTextBox(databaseTextBox);
            UIHelper.StyleTextBox(backupDirectoryTextBox);
            UIHelper.StylePrimaryButton(testConnectionButton);
            UIHelper.StylePrimaryButton(saveButton);
            UIHelper.StyleButton(backupButton, UIHelper.SuccessColor);
            UIHelper.StyleButton(restoreButton, UIHelper.WarningColor);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "اختر مجلد النسخ الاحتياطية خارج مجلد البرنامج";
                dialog.SelectedPath = backupDirectoryTextBox.Text;
                if (dialog.ShowDialog() == DialogResult.OK)
                    backupDirectoryTextBox.Text = dialog.SelectedPath;
            }
        }

        private async void TestConnectionButton_Click(object sender, EventArgs e)
        {
            try
            {
                SetBusy(true, "جارٍ اختبار الاتصال...");
                await Task.Run(() => backupService.TestConnection(serverTextBox.Text, databaseTextBox.Text));
                SetBusy(false, "تم الاتصال بخادم SQL Server بنجاح.");
                UIHelper.ShowInformation("تم الاتصال بخادم SQL Server وقاعدة البيانات بنجاح.");
            }
            catch (Exception ex)
            {
                SetBusy(false, "فشل اختبار الاتصال.");
                UIHelper.ShowException("اختبار الاتصال", ex);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();
                settings = new ApplicationSettingsData
                {
                    ServerInstance = serverTextBox.Text.Trim(),
                    DatabaseName = databaseTextBox.Text.Trim(),
                    BackupDirectory = backupDirectoryTextBox.Text.Trim()
                };
                ApplicationSettingsService.Save(settings);
                statusLabel.Text = "تم حفظ الإعدادات محلياً.";
                UIHelper.ShowInformation("تم حفظ إعدادات SQL Server والنسخ الاحتياطي.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حفظ الإعدادات", ex);
            }
        }

        private async void BackupButton_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin())
            {
                UIHelper.ShowWarning("إنشاء النسخ الاحتياطية متاح لمدير النظام فقط.");
                return;
            }

            try
            {
                ValidateFields();
                SaveSettingsSilently();
                SetBusy(true, "جارٍ إنشاء النسخة الاحتياطية...");
                string file = await Task.Run(() => backupService.Backup(
                    settings.ServerInstance, settings.DatabaseName, settings.BackupDirectory));
                SetBusy(false, "تم إنشاء النسخة الاحتياطية بنجاح.");
                new AuditLogService().Record("إنشاء نسخة احتياطية", "Database", settings.DatabaseName, "الملف: " + file);
                UIHelper.ShowInformation("تم إنشاء النسخة الاحتياطية بنجاح:\n" + file);
            }
            catch (Exception ex)
            {
                SetBusy(false, "فشل إنشاء النسخة الاحتياطية.");
                UIHelper.ShowException("النسخ الاحتياطي", ex);
            }
        }

        private async void RestoreButton_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin())
            {
                UIHelper.ShowWarning("استعادة قواعد البيانات متاحة لمدير النظام فقط.");
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "SQL Backup (*.bak)|*.bak|All files (*.*)|*.*";
                dialog.Title = "اختر ملف النسخة الاحتياطية";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                bool replace = replaceExistingCheckBox.Checked;
                string message = replace
                    ? "سيتم استبدال قاعدة البيانات الهدف بعد إيقاف الاتصالات الحالية. هل تريد المتابعة؟"
                    : "ستتم الاستعادة إلى قاعدة البيانات الهدف إذا لم تكن موجودة. هل تريد المتابعة؟";
                if (MessageBox.Show(message, "تأكيد الاستعادة", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) != DialogResult.Yes)
                    return;

                try
                {
                    ValidateFields();
                    SaveSettingsSilently();
                    SetBusy(true, "جارٍ استعادة قاعدة البيانات...");
                    await Task.Run(() => backupService.Restore(
                        settings.ServerInstance, dialog.FileName, settings.DatabaseName, replace));
                    SetBusy(false, "تمت استعادة قاعدة البيانات بنجاح.");
                    new AuditLogService().Record("استعادة قاعدة بيانات", "Database", settings.DatabaseName, "الملف: " + dialog.FileName);
                    UIHelper.ShowInformation("تمت استعادة قاعدة البيانات بنجاح.");
                }
                catch (Exception ex)
                {
                    SetBusy(false, "فشلت استعادة قاعدة البيانات.");
                    UIHelper.ShowException("استعادة قاعدة البيانات", ex);
                }
            }
        }

        private void ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(serverTextBox.Text))
                throw new InvalidOperationException("أدخل اسم خادم SQL Server.");
            if (string.IsNullOrWhiteSpace(databaseTextBox.Text))
                throw new InvalidOperationException("أدخل اسم قاعدة البيانات.");
            if (string.IsNullOrWhiteSpace(backupDirectoryTextBox.Text))
                throw new InvalidOperationException("اختر مجلد النسخ الاحتياطية.");
        }

        private void SaveSettingsSilently()
        {
            settings = new ApplicationSettingsData
            {
                ServerInstance = serverTextBox.Text.Trim(),
                DatabaseName = databaseTextBox.Text.Trim(),
                BackupDirectory = backupDirectoryTextBox.Text.Trim()
            };
            ApplicationSettingsService.Save(settings);
        }

        private void SetBusy(bool busy, string message)
        {
            statusLabel.Text = message;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            testConnectionButton.Enabled = !busy;
            saveButton.Enabled = !busy;
            backupButton.Enabled = !busy;
            restoreButton.Enabled = !busy;
            browseButton.Enabled = !busy;
            serverTextBox.Enabled = !busy;
            databaseTextBox.Enabled = !busy;
            backupDirectoryTextBox.Enabled = !busy;
            replaceExistingCheckBox.Enabled = !busy;
        }
    }
}
