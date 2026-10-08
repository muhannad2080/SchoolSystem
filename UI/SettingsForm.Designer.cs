using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace SchoolSystem.UI
{

    public partial class SettingsForm
    {

        private IContainer components = null;

        private Panel headerPanel;

        private Label titleLabel;

        private Label subtitleLabel;

        private Panel contentPanel;

        private GroupBox schoolDataGroup;

        private TableLayoutPanel fieldsTable;

        private GroupBox logoGroup;

        private Panel logoPreviewPanel;

        private PictureBox logoPictureBox;

        private Label logoPlaceholderLabel;

        private FlowLayoutPanel logoActionsPanel;

        private Button chooseLogoButton;

        private Button removeLogoButton;

        private Panel footerPanel;

        private Button saveButton;

        private Label statusLabel;

        private GroupBox systemGroup;

        private TableLayoutPanel systemTable;

        private TextBox serverInstanceTextBox;

        private TextBox databaseNameTextBox;

        private CheckBox useSqlAuthCheckBox;

        private Label sqlUserLabel;

        private TextBox sqlUserTextBox;

        private Label sqlPasswordLabel;

        private TextBox sqlPasswordTextBox;

        private TextBox backupDirectoryTextBox;

        private TextBox restoreFileTextBox;

        private TextBox restoreDatabaseTextBox;

        private Button testConnectionButton;

        private Button chooseBackupDirectoryButton;

        private Button createBackupButton;

        private Button chooseRestoreFileButton;

        private Button restoreButton;

        private CheckBox replaceExistingCheckBox;

        private Label systemStatusLabel;

        private GroupBox generalGroup;

        private TableLayoutPanel generalTable;

        private ComboBox languageComboBox;

        private TextBox currencyTextBox;

        private TextBox dateFormatTextBox;

        private TextBox receiptPrefixTextBox;

        private TextBox paymentPrefixTextBox;

        private GroupBox emailGroup;

        private TableLayoutPanel emailTable;

        private CheckBox emailNotificationsCheckBox;

        private TextBox smtpHostTextBox;

        private TextBox smtpPortTextBox;

        private TextBox smtpUsernameTextBox;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private TextBox smtpPasswordTextBox;

        private CheckBox sslCheckBox;

        private Label schoolNameArLabel;

        private Label schoolNameEnLabel;

        private Label organizationNameArLabel;

        private Label organizationNameEnLabel;

        private Label addressArLabel;

        private Label addressEnLabel;

        private Label phoneLabel;

        private Label additionalPhoneLabel;

        private Label emailLabel;

        private Label websiteLabel;

        private Label principalNameArLabel;

        private Label principalNameEnLabel;

        private Label licenseNumberLabel;

        private Label sqlServerLabel;

        private Label databaseLabel;

        private Label backupDirectoryLabel;

        private Label restoreFileLabel;

        private Label restoreDatabaseLabel;

        private Label languageLabel;

        private Label currencyLabel;

        private Label dateFormatLabel;

        private Label receiptPrefixLabel;

        private Label paymentPrefixLabel;

        private Label noteLabel;

        private Label notificationNoteLabel;

        private Label notificationHelpLabel;

        private Label smtpHostLabel;

        private Label smtpPortLabel;

        private Label smtpUsernameLabel;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private Label smtpPasswordLabel;

        private TextBox schoolNameArTextBox;

        private TextBox schoolNameEnTextBox;

        private TextBox organizationNameArTextBox;

        private TextBox organizationNameEnTextBox;

        private TextBox addressArTextBox;

        private TextBox addressEnTextBox;

        private TextBox phoneTextBox;

        private TextBox additionalPhoneTextBox;

        private TextBox emailTextBox;

        private TextBox websiteTextBox;

        private TextBox principalNameArTextBox;

        private TextBox principalNameEnTextBox;

        private TextBox licenseNumberTextBox;

        // تعريف الدالة Dispose لتنفيذ العملية المرتبطة بها.
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.emailGroup = new System.Windows.Forms.GroupBox();
            this.emailTable = new System.Windows.Forms.TableLayoutPanel();
            this.emailNotificationsCheckBox = new System.Windows.Forms.CheckBox();
            this.smtpHostLabel = new System.Windows.Forms.Label();
            this.smtpHostTextBox = new System.Windows.Forms.TextBox();
            this.smtpPortLabel = new System.Windows.Forms.Label();
            this.smtpPortTextBox = new System.Windows.Forms.TextBox();
            this.smtpUsernameLabel = new System.Windows.Forms.Label();
            this.smtpUsernameTextBox = new System.Windows.Forms.TextBox();
            this.smtpPasswordLabel = new System.Windows.Forms.Label();
            this.smtpPasswordTextBox = new System.Windows.Forms.TextBox();
            this.sslCheckBox = new System.Windows.Forms.CheckBox();
            this.notificationNoteLabel = new System.Windows.Forms.Label();
            this.generalGroup = new System.Windows.Forms.GroupBox();
            this.generalTable = new System.Windows.Forms.TableLayoutPanel();
            this.languageLabel = new System.Windows.Forms.Label();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.currencyLabel = new System.Windows.Forms.Label();
            this.currencyTextBox = new System.Windows.Forms.TextBox();
            this.dateFormatLabel = new System.Windows.Forms.Label();
            this.dateFormatTextBox = new System.Windows.Forms.TextBox();
            this.receiptPrefixLabel = new System.Windows.Forms.Label();
            this.receiptPrefixTextBox = new System.Windows.Forms.TextBox();
            this.paymentPrefixLabel = new System.Windows.Forms.Label();
            this.paymentPrefixTextBox = new System.Windows.Forms.TextBox();
            this.noteLabel = new System.Windows.Forms.Label();
            this.notificationHelpLabel = new System.Windows.Forms.Label();
            this.systemGroup = new System.Windows.Forms.GroupBox();
            this.systemTable = new System.Windows.Forms.TableLayoutPanel();
            this.sqlServerLabel = new System.Windows.Forms.Label();
            this.serverInstanceTextBox = new System.Windows.Forms.TextBox();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.databaseNameTextBox = new System.Windows.Forms.TextBox();
            this.useSqlAuthCheckBox = new System.Windows.Forms.CheckBox();
            this.sqlUserLabel = new System.Windows.Forms.Label();
            this.sqlUserTextBox = new System.Windows.Forms.TextBox();
            this.sqlPasswordLabel = new System.Windows.Forms.Label();
            this.sqlPasswordTextBox = new System.Windows.Forms.TextBox();
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.backupDirectoryLabel = new System.Windows.Forms.Label();
            this.backupDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.chooseBackupDirectoryButton = new System.Windows.Forms.Button();
            this.createBackupButton = new System.Windows.Forms.Button();
            this.restoreFileLabel = new System.Windows.Forms.Label();
            this.restoreFileTextBox = new System.Windows.Forms.TextBox();
            this.chooseRestoreFileButton = new System.Windows.Forms.Button();
            this.restoreDatabaseLabel = new System.Windows.Forms.Label();
            this.restoreDatabaseTextBox = new System.Windows.Forms.TextBox();
            this.replaceExistingCheckBox = new System.Windows.Forms.CheckBox();
            this.restoreButton = new System.Windows.Forms.Button();
            this.systemStatusLabel = new System.Windows.Forms.Label();
            this.logoGroup = new System.Windows.Forms.GroupBox();
            this.logoActionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.chooseLogoButton = new System.Windows.Forms.Button();
            this.removeLogoButton = new System.Windows.Forms.Button();
            this.logoPreviewPanel = new System.Windows.Forms.Panel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.logoPlaceholderLabel = new System.Windows.Forms.Label();
            this.schoolDataGroup = new System.Windows.Forms.GroupBox();
            this.fieldsTable = new System.Windows.Forms.TableLayoutPanel();
            this.schoolNameArLabel = new System.Windows.Forms.Label();
            this.schoolNameArTextBox = new System.Windows.Forms.TextBox();
            this.schoolNameEnLabel = new System.Windows.Forms.Label();
            this.schoolNameEnTextBox = new System.Windows.Forms.TextBox();
            this.organizationNameArLabel = new System.Windows.Forms.Label();
            this.organizationNameArTextBox = new System.Windows.Forms.TextBox();
            this.organizationNameEnLabel = new System.Windows.Forms.Label();
            this.organizationNameEnTextBox = new System.Windows.Forms.TextBox();
            this.addressArLabel = new System.Windows.Forms.Label();
            this.addressArTextBox = new System.Windows.Forms.TextBox();
            this.addressEnLabel = new System.Windows.Forms.Label();
            this.addressEnTextBox = new System.Windows.Forms.TextBox();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.phoneTextBox = new System.Windows.Forms.TextBox();
            this.additionalPhoneLabel = new System.Windows.Forms.Label();
            this.additionalPhoneTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.websiteLabel = new System.Windows.Forms.Label();
            this.websiteTextBox = new System.Windows.Forms.TextBox();
            this.principalNameArLabel = new System.Windows.Forms.Label();
            this.principalNameArTextBox = new System.Windows.Forms.TextBox();
            this.principalNameEnLabel = new System.Windows.Forms.Label();
            this.principalNameEnTextBox = new System.Windows.Forms.TextBox();
            this.licenseNumberLabel = new System.Windows.Forms.Label();
            this.licenseNumberTextBox = new System.Windows.Forms.TextBox();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.saveButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.emailGroup.SuspendLayout();
            this.emailTable.SuspendLayout();
            this.generalGroup.SuspendLayout();
            this.generalTable.SuspendLayout();
            this.systemGroup.SuspendLayout();
            this.systemTable.SuspendLayout();
            this.logoGroup.SuspendLayout();
            this.logoActionsPanel.SuspendLayout();
            this.logoPreviewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.schoolDataGroup.SuspendLayout();
            this.fieldsTable.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.headerPanel.Controls.Add(this.subtitleLabel);
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(22, 10, 22, 8);
            this.headerPanel.Size = new System.Drawing.Size(1299, 76);
            this.headerPanel.TabIndex = 2;
            // 
            // subtitleLabel
            // 
            this.subtitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subtitleLabel.Font = new System.Drawing.Font("Tahoma", 9F);
            this.subtitleLabel.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.subtitleLabel.Location = new System.Drawing.Point(22, 44);
            this.subtitleLabel.Name = "subtitleLabel";
            this.subtitleLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.subtitleLabel.Size = new System.Drawing.Size(1255, 26);
            this.subtitleLabel.TabIndex = 0;
            this.subtitleLabel.Text = "هوية المدرسة وبياناتها الرسمية المستخدمة في التقارير والطباعة والتصدير";
            this.subtitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleLabel
            // 
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(22, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.titleLabel.Size = new System.Drawing.Size(1255, 34);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "إعدادات المدرسة";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contentPanel
            // 
            this.contentPanel.AutoScroll = true;
            this.contentPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.contentPanel.Controls.Add(this.emailGroup);
            this.contentPanel.Controls.Add(this.generalGroup);
            this.contentPanel.Controls.Add(this.systemGroup);
            this.contentPanel.Controls.Add(this.logoGroup);
            this.contentPanel.Controls.Add(this.schoolDataGroup);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 78);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.contentPanel.Size = new System.Drawing.Size(1299, 391);
            this.contentPanel.TabIndex = 0;
            // 
            // emailGroup
            // 
            this.emailGroup.Controls.Add(this.emailTable);
            this.emailGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.emailGroup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.emailGroup.Location = new System.Drawing.Point(18, 1225);
            this.emailGroup.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.emailGroup.Name = "emailGroup";
            this.emailGroup.Padding = new System.Windows.Forms.Padding(12, 24, 12, 12);
            this.emailGroup.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.emailGroup.Size = new System.Drawing.Size(1242, 275);
            this.emailGroup.TabIndex = 0;
            this.emailGroup.TabStop = false;
            this.emailGroup.Text = "البريد الإلكتروني والتنبيهات";
            // 
            // emailTable
            // 
            this.emailTable.ColumnCount = 4;
            this.emailTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.emailTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.emailTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.emailTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.emailTable.Controls.Add(this.emailNotificationsCheckBox, 0, 0);
            this.emailTable.Controls.Add(this.smtpHostLabel, 0, 1);
            this.emailTable.Controls.Add(this.smtpHostTextBox, 1, 1);
            this.emailTable.Controls.Add(this.smtpPortLabel, 2, 1);
            this.emailTable.Controls.Add(this.smtpPortTextBox, 3, 1);
            this.emailTable.Controls.Add(this.smtpUsernameLabel, 0, 2);
            this.emailTable.Controls.Add(this.smtpUsernameTextBox, 1, 2);
            this.emailTable.Controls.Add(this.smtpPasswordLabel, 2, 2);
            this.emailTable.Controls.Add(this.smtpPasswordTextBox, 3, 2);
            this.emailTable.Controls.Add(this.sslCheckBox, 0, 3);
            this.emailTable.Controls.Add(this.notificationNoteLabel, 2, 3);
            this.emailTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailTable.Location = new System.Drawing.Point(12, 45);
            this.emailTable.Name = "emailTable";
            this.emailTable.Padding = new System.Windows.Forms.Padding(4);
            this.emailTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.emailTable.RowCount = 5;
            this.emailTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.emailTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.emailTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.emailTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.emailTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.emailTable.Size = new System.Drawing.Size(1218, 203);
            this.emailTable.TabIndex = 0;
            // 
            // emailNotificationsCheckBox
            // 
            this.emailNotificationsCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.emailNotificationsCheckBox.AutoSize = true;
            this.emailTable.SetColumnSpan(this.emailNotificationsCheckBox, 4);
            this.emailNotificationsCheckBox.Location = new System.Drawing.Point(1020, 12);
            this.emailNotificationsCheckBox.Name = "emailNotificationsCheckBox";
            this.emailNotificationsCheckBox.Size = new System.Drawing.Size(191, 25);
            this.emailNotificationsCheckBox.TabIndex = 0;
            this.emailNotificationsCheckBox.Text = "تفعيل تنبيهات البريد";
            // 
            // smtpHostLabel
            // 
            this.smtpHostLabel.Location = new System.Drawing.Point(1111, 46);
            this.smtpHostLabel.Name = "smtpHostLabel";
            this.smtpHostLabel.Size = new System.Drawing.Size(100, 23);
            this.smtpHostLabel.TabIndex = 1;
            this.smtpHostLabel.Text = "خادم SMTP";
            // 
            // smtpHostTextBox
            // 
            this.smtpHostTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.smtpHostTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.smtpHostTextBox.Location = new System.Drawing.Point(725, 52);
            this.smtpHostTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.smtpHostTextBox.Name = "smtpHostTextBox";
            this.smtpHostTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.smtpHostTextBox.Size = new System.Drawing.Size(333, 27);
            this.smtpHostTextBox.TabIndex = 2;
            // 
            // smtpPortLabel
            // 
            this.smtpPortLabel.Location = new System.Drawing.Point(616, 46);
            this.smtpPortLabel.Name = "smtpPortLabel";
            this.smtpPortLabel.Size = new System.Drawing.Size(100, 23);
            this.smtpPortLabel.TabIndex = 3;
            this.smtpPortLabel.Text = "منفذ SMTP";
            // 
            // smtpPortTextBox
            // 
            this.smtpPortTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.smtpPortTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.smtpPortTextBox.Location = new System.Drawing.Point(10, 52);
            this.smtpPortTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.smtpPortTextBox.Name = "smtpPortTextBox";
            this.smtpPortTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.smtpPortTextBox.Size = new System.Drawing.Size(553, 27);
            this.smtpPortTextBox.TabIndex = 4;
            // 
            // smtpUsernameLabel
            // 
            this.smtpUsernameLabel.Location = new System.Drawing.Point(1111, 88);
            this.smtpUsernameLabel.Name = "smtpUsernameLabel";
            this.smtpUsernameLabel.Size = new System.Drawing.Size(100, 23);
            this.smtpUsernameLabel.TabIndex = 5;
            this.smtpUsernameLabel.Text = "مستخدم SMTP";
            // 
            // smtpUsernameTextBox
            // 
            this.smtpUsernameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.smtpUsernameTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.smtpUsernameTextBox.Location = new System.Drawing.Point(725, 94);
            this.smtpUsernameTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.smtpUsernameTextBox.Name = "smtpUsernameTextBox";
            this.smtpUsernameTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.smtpUsernameTextBox.Size = new System.Drawing.Size(333, 27);
            this.smtpUsernameTextBox.TabIndex = 6;
            // 
            // smtpPasswordLabel
            // 
            this.smtpPasswordLabel.Location = new System.Drawing.Point(616, 88);
            this.smtpPasswordLabel.Name = "smtpPasswordLabel";
            this.smtpPasswordLabel.Size = new System.Drawing.Size(100, 23);
            this.smtpPasswordLabel.TabIndex = 7;
            this.smtpPasswordLabel.Text = "كلمة مرور SMTP";
            // 
            // smtpPasswordTextBox
            // 
            this.smtpPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.smtpPasswordTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.smtpPasswordTextBox.Location = new System.Drawing.Point(10, 94);
            this.smtpPasswordTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.smtpPasswordTextBox.Name = "smtpPasswordTextBox";
            this.smtpPasswordTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.smtpPasswordTextBox.Size = new System.Drawing.Size(553, 27);
            this.smtpPasswordTextBox.TabIndex = 8;
            this.smtpPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // sslCheckBox
            // 
            this.sslCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.sslCheckBox.AutoSize = true;
            this.emailTable.SetColumnSpan(this.sslCheckBox, 2);
            this.sslCheckBox.Location = new System.Drawing.Point(978, 138);
            this.sslCheckBox.Name = "sslCheckBox";
            this.sslCheckBox.Size = new System.Drawing.Size(233, 25);
            this.sslCheckBox.TabIndex = 9;
            this.sslCheckBox.Text = "استخدام اتصال SSL/TLS";
            // 
            // notificationNoteLabel
            // 
            this.notificationNoteLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notificationNoteLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.notificationNoteLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.notificationNoteLabel.Location = new System.Drawing.Point(573, 134);
            this.notificationNoteLabel.Margin = new System.Windows.Forms.Padding(4);
            this.notificationNoteLabel.Name = "notificationNoteLabel";
            this.notificationNoteLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.notificationNoteLabel.Size = new System.Drawing.Size(142, 34);
            this.notificationNoteLabel.TabIndex = 10;
            this.notificationNoteLabel.Text = "تنبيه";
            this.notificationNoteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // generalGroup
            // 
            this.generalGroup.Controls.Add(this.generalTable);
            this.generalGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.generalGroup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.generalGroup.Location = new System.Drawing.Point(18, 993);
            this.generalGroup.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.generalGroup.Name = "generalGroup";
            this.generalGroup.Padding = new System.Windows.Forms.Padding(12, 24, 12, 12);
            this.generalGroup.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.generalGroup.Size = new System.Drawing.Size(1242, 220);
            this.generalGroup.TabIndex = 1;
            this.generalGroup.TabStop = false;
            this.generalGroup.Text = "الإعدادات العامة والترقيم";
            // 
            // generalTable
            // 
            this.generalTable.ColumnCount = 4;
            this.generalTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.generalTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.generalTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.generalTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.generalTable.Controls.Add(this.languageLabel, 0, 0);
            this.generalTable.Controls.Add(this.languageComboBox, 1, 0);
            this.generalTable.Controls.Add(this.currencyLabel, 2, 0);
            this.generalTable.Controls.Add(this.currencyTextBox, 3, 0);
            this.generalTable.Controls.Add(this.dateFormatLabel, 0, 1);
            this.generalTable.Controls.Add(this.dateFormatTextBox, 1, 1);
            this.generalTable.Controls.Add(this.receiptPrefixLabel, 2, 1);
            this.generalTable.Controls.Add(this.receiptPrefixTextBox, 3, 1);
            this.generalTable.Controls.Add(this.paymentPrefixLabel, 0, 2);
            this.generalTable.Controls.Add(this.paymentPrefixTextBox, 1, 2);
            this.generalTable.Controls.Add(this.noteLabel, 2, 2);
            this.generalTable.Controls.Add(this.notificationHelpLabel, 3, 2);
            this.generalTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generalTable.Location = new System.Drawing.Point(12, 45);
            this.generalTable.Name = "generalTable";
            this.generalTable.Padding = new System.Windows.Forms.Padding(4);
            this.generalTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.generalTable.RowCount = 3;
            this.generalTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.generalTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.generalTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.generalTable.Size = new System.Drawing.Size(1218, 163);
            this.generalTable.TabIndex = 0;
            // 
            // languageLabel
            // 
            this.languageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.languageLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.languageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.languageLabel.Location = new System.Drawing.Point(1068, 8);
            this.languageLabel.Margin = new System.Windows.Forms.Padding(4);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.languageLabel.Size = new System.Drawing.Size(142, 36);
            this.languageLabel.TabIndex = 0;
            this.languageLabel.Text = "لغة النظام";
            this.languageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // languageComboBox
            // 
            this.languageComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageComboBox.Items.AddRange(new object[] {
            "ar",
            "en"});
            this.languageComboBox.Location = new System.Drawing.Point(722, 11);
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(339, 29);
            this.languageComboBox.TabIndex = 1;
            // 
            // currencyLabel
            // 
            this.currencyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currencyLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.currencyLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.currencyLabel.Location = new System.Drawing.Point(573, 8);
            this.currencyLabel.Margin = new System.Windows.Forms.Padding(4);
            this.currencyLabel.Name = "currencyLabel";
            this.currencyLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.currencyLabel.Size = new System.Drawing.Size(142, 36);
            this.currencyLabel.TabIndex = 2;
            this.currencyLabel.Text = "رمز العملة";
            this.currencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // currencyTextBox
            // 
            this.currencyTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currencyTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.currencyTextBox.Location = new System.Drawing.Point(10, 10);
            this.currencyTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.currencyTextBox.Name = "currencyTextBox";
            this.currencyTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.currencyTextBox.Size = new System.Drawing.Size(553, 27);
            this.currencyTextBox.TabIndex = 3;
            // 
            // dateFormatLabel
            // 
            this.dateFormatLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateFormatLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.dateFormatLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.dateFormatLabel.Location = new System.Drawing.Point(1068, 52);
            this.dateFormatLabel.Margin = new System.Windows.Forms.Padding(4);
            this.dateFormatLabel.Name = "dateFormatLabel";
            this.dateFormatLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dateFormatLabel.Size = new System.Drawing.Size(142, 36);
            this.dateFormatLabel.TabIndex = 4;
            this.dateFormatLabel.Text = "تنسيق التاريخ";
            this.dateFormatLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateFormatTextBox
            // 
            this.dateFormatTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateFormatTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dateFormatTextBox.Location = new System.Drawing.Point(725, 54);
            this.dateFormatTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.dateFormatTextBox.Name = "dateFormatTextBox";
            this.dateFormatTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dateFormatTextBox.Size = new System.Drawing.Size(333, 27);
            this.dateFormatTextBox.TabIndex = 5;
            // 
            // receiptPrefixLabel
            // 
            this.receiptPrefixLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.receiptPrefixLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.receiptPrefixLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.receiptPrefixLabel.Location = new System.Drawing.Point(573, 52);
            this.receiptPrefixLabel.Margin = new System.Windows.Forms.Padding(4);
            this.receiptPrefixLabel.Name = "receiptPrefixLabel";
            this.receiptPrefixLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.receiptPrefixLabel.Size = new System.Drawing.Size(142, 36);
            this.receiptPrefixLabel.TabIndex = 6;
            this.receiptPrefixLabel.Text = "بادئة سند القبض";
            this.receiptPrefixLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // receiptPrefixTextBox
            // 
            this.receiptPrefixTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.receiptPrefixTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.receiptPrefixTextBox.Location = new System.Drawing.Point(10, 54);
            this.receiptPrefixTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.receiptPrefixTextBox.Name = "receiptPrefixTextBox";
            this.receiptPrefixTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.receiptPrefixTextBox.Size = new System.Drawing.Size(553, 27);
            this.receiptPrefixTextBox.TabIndex = 7;
            // 
            // paymentPrefixLabel
            // 
            this.paymentPrefixLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentPrefixLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.paymentPrefixLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.paymentPrefixLabel.Location = new System.Drawing.Point(1068, 96);
            this.paymentPrefixLabel.Margin = new System.Windows.Forms.Padding(4);
            this.paymentPrefixLabel.Name = "paymentPrefixLabel";
            this.paymentPrefixLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.paymentPrefixLabel.Size = new System.Drawing.Size(142, 59);
            this.paymentPrefixLabel.TabIndex = 8;
            this.paymentPrefixLabel.Text = "بادئة سند الصرف";
            this.paymentPrefixLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // paymentPrefixTextBox
            // 
            this.paymentPrefixTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentPrefixTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.paymentPrefixTextBox.Location = new System.Drawing.Point(725, 98);
            this.paymentPrefixTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.paymentPrefixTextBox.Name = "paymentPrefixTextBox";
            this.paymentPrefixTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.paymentPrefixTextBox.Size = new System.Drawing.Size(333, 27);
            this.paymentPrefixTextBox.TabIndex = 9;
            // 
            // noteLabel
            // 
            this.noteLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noteLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.noteLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.noteLabel.Location = new System.Drawing.Point(573, 96);
            this.noteLabel.Margin = new System.Windows.Forms.Padding(4);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.noteLabel.Size = new System.Drawing.Size(142, 59);
            this.noteLabel.TabIndex = 10;
            this.noteLabel.Text = "ملاحظة";
            this.noteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // notificationHelpLabel
            // 
            this.notificationHelpLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notificationHelpLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.notificationHelpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.notificationHelpLabel.Location = new System.Drawing.Point(8, 96);
            this.notificationHelpLabel.Margin = new System.Windows.Forms.Padding(4);
            this.notificationHelpLabel.Name = "notificationHelpLabel";
            this.notificationHelpLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.notificationHelpLabel.Size = new System.Drawing.Size(557, 59);
            this.notificationHelpLabel.TabIndex = 11;
            this.notificationHelpLabel.Text = "الإعدادات تحفظ حالياً لاستخدامها مع خدمات التنبيه";
            this.notificationHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // systemGroup
            // 
            this.systemGroup.Controls.Add(this.systemTable);
            this.systemGroup.Controls.Add(this.systemStatusLabel);
            this.systemGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.systemGroup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.systemGroup.Location = new System.Drawing.Point(18, 584);
            this.systemGroup.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.systemGroup.Name = "systemGroup";
            this.systemGroup.Padding = new System.Windows.Forms.Padding(12, 24, 12, 12);
            this.systemGroup.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.systemGroup.Size = new System.Drawing.Size(1242, 409);
            this.systemGroup.TabIndex = 2;
            this.systemGroup.TabStop = false;
            this.systemGroup.Text = "الاتصال والنسخ الاحتياطي والاستعادة";
            // 
            // systemTable
            // 
            this.systemTable.ColumnCount = 4;
            this.systemTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.systemTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.systemTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.systemTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.systemTable.Controls.Add(this.sqlServerLabel, 0, 0);
            this.systemTable.Controls.Add(this.serverInstanceTextBox, 1, 0);
            this.systemTable.Controls.Add(this.databaseLabel, 2, 0);
            this.systemTable.Controls.Add(this.databaseNameTextBox, 3, 0);
            this.systemTable.Controls.Add(this.useSqlAuthCheckBox, 0, 1);
            this.systemTable.Controls.Add(this.sqlUserLabel, 0, 2);
            this.systemTable.Controls.Add(this.sqlUserTextBox, 1, 2);
            this.systemTable.Controls.Add(this.sqlPasswordLabel, 2, 2);
            this.systemTable.Controls.Add(this.sqlPasswordTextBox, 3, 2);
            this.systemTable.Controls.Add(this.testConnectionButton, 0, 3);
            this.systemTable.Controls.Add(this.backupDirectoryLabel, 0, 4);
            this.systemTable.Controls.Add(this.backupDirectoryTextBox, 1, 4);
            this.systemTable.Controls.Add(this.chooseBackupDirectoryButton, 2, 4);
            this.systemTable.Controls.Add(this.createBackupButton, 0, 5);
            this.systemTable.Controls.Add(this.restoreFileLabel, 0, 6);
            this.systemTable.Controls.Add(this.restoreFileTextBox, 1, 6);
            this.systemTable.Controls.Add(this.chooseRestoreFileButton, 2, 6);
            this.systemTable.Controls.Add(this.restoreDatabaseLabel, 0, 7);
            this.systemTable.Controls.Add(this.restoreDatabaseTextBox, 1, 7);
            this.systemTable.Controls.Add(this.replaceExistingCheckBox, 2, 7);
            this.systemTable.Controls.Add(this.restoreButton, 3, 7);
            this.systemTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.systemTable.Location = new System.Drawing.Point(12, 45);
            this.systemTable.Name = "systemTable";
            this.systemTable.Padding = new System.Windows.Forms.Padding(4);
            this.systemTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.systemTable.RowCount = 8;
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.systemTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.systemTable.Size = new System.Drawing.Size(1218, 299);
            this.systemTable.TabIndex = 0;
            // 
            // sqlServerLabel
            // 
            this.sqlServerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqlServerLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.sqlServerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.sqlServerLabel.Location = new System.Drawing.Point(1068, 8);
            this.sqlServerLabel.Margin = new System.Windows.Forms.Padding(4);
            this.sqlServerLabel.Name = "sqlServerLabel";
            this.sqlServerLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.sqlServerLabel.Size = new System.Drawing.Size(142, 32);
            this.sqlServerLabel.TabIndex = 0;
            this.sqlServerLabel.Text = "خادم SQL Server";
            this.sqlServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serverInstanceTextBox
            // 
            this.serverInstanceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverInstanceTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.serverInstanceTextBox.Location = new System.Drawing.Point(725, 10);
            this.serverInstanceTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.serverInstanceTextBox.Name = "serverInstanceTextBox";
            this.serverInstanceTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.serverInstanceTextBox.Size = new System.Drawing.Size(333, 27);
            this.serverInstanceTextBox.TabIndex = 1;
            // 
            // databaseLabel
            // 
            this.databaseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.databaseLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.databaseLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.databaseLabel.Location = new System.Drawing.Point(573, 8);
            this.databaseLabel.Margin = new System.Windows.Forms.Padding(4);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.databaseLabel.Size = new System.Drawing.Size(142, 32);
            this.databaseLabel.TabIndex = 2;
            this.databaseLabel.Text = "اسم قاعدة البيانات";
            this.databaseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // databaseNameTextBox
            // 
            this.databaseNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.databaseNameTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.databaseNameTextBox.Location = new System.Drawing.Point(10, 10);
            this.databaseNameTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.databaseNameTextBox.Name = "databaseNameTextBox";
            this.databaseNameTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.databaseNameTextBox.Size = new System.Drawing.Size(553, 27);
            this.databaseNameTextBox.TabIndex = 3;
            // 
            // useSqlAuthCheckBox
            // 
            this.useSqlAuthCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.systemTable.SetColumnSpan(this.useSqlAuthCheckBox, 4);
            this.useSqlAuthCheckBox.AutoSize = true;
            this.useSqlAuthCheckBox.Location = new System.Drawing.Point(978, 50);
            this.useSqlAuthCheckBox.Name = "useSqlAuthCheckBox";
            this.useSqlAuthCheckBox.Size = new System.Drawing.Size(197, 25);
            this.useSqlAuthCheckBox.TabIndex = 20;
            this.useSqlAuthCheckBox.Text = "مصادقة SQL (بدلاً من Windows)";
            this.useSqlAuthCheckBox.UseVisualStyleBackColor = true;
            // 
            // sqlUserLabel
            // 
            this.sqlUserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqlUserLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.sqlUserLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.sqlUserLabel.Location = new System.Drawing.Point(1068, 88);
            this.sqlUserLabel.Margin = new System.Windows.Forms.Padding(4);
            this.sqlUserLabel.Name = "sqlUserLabel";
            this.sqlUserLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.sqlUserLabel.Size = new System.Drawing.Size(142, 32);
            this.sqlUserLabel.TabIndex = 21;
            this.sqlUserLabel.Text = "مستخدم SQL";
            this.sqlUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sqlUserTextBox
            // 
            this.sqlUserTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqlUserTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.sqlUserTextBox.Location = new System.Drawing.Point(725, 90);
            this.sqlUserTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.sqlUserTextBox.Name = "sqlUserTextBox";
            this.sqlUserTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.sqlUserTextBox.Size = new System.Drawing.Size(333, 27);
            this.sqlUserTextBox.TabIndex = 22;
            // 
            // sqlPasswordLabel
            // 
            this.sqlPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqlPasswordLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.sqlPasswordLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.sqlPasswordLabel.Location = new System.Drawing.Point(573, 88);
            this.sqlPasswordLabel.Margin = new System.Windows.Forms.Padding(4);
            this.sqlPasswordLabel.Name = "sqlPasswordLabel";
            this.sqlPasswordLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.sqlPasswordLabel.Size = new System.Drawing.Size(142, 32);
            this.sqlPasswordLabel.TabIndex = 23;
            this.sqlPasswordLabel.Text = "كلمة مرور SQL";
            this.sqlPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sqlPasswordTextBox
            // 
            this.sqlPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqlPasswordTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.sqlPasswordTextBox.Location = new System.Drawing.Point(10, 90);
            this.sqlPasswordTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.sqlPasswordTextBox.Name = "sqlPasswordTextBox";
            this.sqlPasswordTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.sqlPasswordTextBox.Size = new System.Drawing.Size(553, 27);
            this.sqlPasswordTextBox.TabIndex = 24;
            this.sqlPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // testConnectionButton
            // 
            this.testConnectionButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.systemTable.SetColumnSpan(this.testConnectionButton, 4);
            this.testConnectionButton.Location = new System.Drawing.Point(7, 128);
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size(145, 32);
            this.testConnectionButton.TabIndex = 4;
            this.testConnectionButton.Text = "اختبار الاتصال";
            // 
            // backupDirectoryLabel
            // 
            this.backupDirectoryLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backupDirectoryLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.backupDirectoryLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.backupDirectoryLabel.Location = new System.Drawing.Point(1068, 88);
            this.backupDirectoryLabel.Margin = new System.Windows.Forms.Padding(4);
            this.backupDirectoryLabel.Name = "backupDirectoryLabel";
            this.backupDirectoryLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.backupDirectoryLabel.Size = new System.Drawing.Size(142, 32);
            this.backupDirectoryLabel.TabIndex = 5;
            this.backupDirectoryLabel.Text = "مجلد النسخ الاحتياطي";
            this.backupDirectoryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // backupDirectoryTextBox
            // 
            this.backupDirectoryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backupDirectoryTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.backupDirectoryTextBox.Location = new System.Drawing.Point(725, 90);
            this.backupDirectoryTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.backupDirectoryTextBox.Name = "backupDirectoryTextBox";
            this.backupDirectoryTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.backupDirectoryTextBox.Size = new System.Drawing.Size(333, 27);
            this.backupDirectoryTextBox.TabIndex = 6;
            // 
            // chooseBackupDirectoryButton
            // 
            this.chooseBackupDirectoryButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.systemTable.SetColumnSpan(this.chooseBackupDirectoryButton, 2);
            this.chooseBackupDirectoryButton.Location = new System.Drawing.Point(591, 88);
            this.chooseBackupDirectoryButton.Name = "chooseBackupDirectoryButton";
            this.chooseBackupDirectoryButton.Size = new System.Drawing.Size(125, 32);
            this.chooseBackupDirectoryButton.TabIndex = 7;
            this.chooseBackupDirectoryButton.Text = "اختيار المجلد";
            // 
            // createBackupButton
            // 
            this.createBackupButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.systemTable.SetColumnSpan(this.createBackupButton, 4);
            this.createBackupButton.Location = new System.Drawing.Point(7, 128);
            this.createBackupButton.Name = "createBackupButton";
            this.createBackupButton.Size = new System.Drawing.Size(190, 32);
            this.createBackupButton.TabIndex = 8;
            this.createBackupButton.Text = "إنشاء نسخة احتياطية الآن";
            // 
            // restoreFileLabel
            // 
            this.restoreFileLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restoreFileLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.restoreFileLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.restoreFileLabel.Location = new System.Drawing.Point(1068, 168);
            this.restoreFileLabel.Margin = new System.Windows.Forms.Padding(4);
            this.restoreFileLabel.Name = "restoreFileLabel";
            this.restoreFileLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.restoreFileLabel.Size = new System.Drawing.Size(142, 32);
            this.restoreFileLabel.TabIndex = 9;
            this.restoreFileLabel.Text = "ملف الاستعادة";
            this.restoreFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // restoreFileTextBox
            // 
            this.restoreFileTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restoreFileTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.restoreFileTextBox.Location = new System.Drawing.Point(725, 170);
            this.restoreFileTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.restoreFileTextBox.Name = "restoreFileTextBox";
            this.restoreFileTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.restoreFileTextBox.Size = new System.Drawing.Size(333, 27);
            this.restoreFileTextBox.TabIndex = 10;
            // 
            // chooseRestoreFileButton
            // 
            this.chooseRestoreFileButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.systemTable.SetColumnSpan(this.chooseRestoreFileButton, 2);
            this.chooseRestoreFileButton.Location = new System.Drawing.Point(591, 168);
            this.chooseRestoreFileButton.Name = "chooseRestoreFileButton";
            this.chooseRestoreFileButton.Size = new System.Drawing.Size(125, 32);
            this.chooseRestoreFileButton.TabIndex = 11;
            this.chooseRestoreFileButton.Text = "اختيار ملف .bak";
            // 
            // restoreDatabaseLabel
            // 
            this.restoreDatabaseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restoreDatabaseLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.restoreDatabaseLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.restoreDatabaseLabel.Location = new System.Drawing.Point(1068, 208);
            this.restoreDatabaseLabel.Margin = new System.Windows.Forms.Padding(4);
            this.restoreDatabaseLabel.Name = "restoreDatabaseLabel";
            this.restoreDatabaseLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.restoreDatabaseLabel.Size = new System.Drawing.Size(142, 32);
            this.restoreDatabaseLabel.TabIndex = 12;
            this.restoreDatabaseLabel.Text = "قاعدة الاستعادة";
            this.restoreDatabaseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // restoreDatabaseTextBox
            // 
            this.restoreDatabaseTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restoreDatabaseTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.restoreDatabaseTextBox.Location = new System.Drawing.Point(725, 210);
            this.restoreDatabaseTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.restoreDatabaseTextBox.Name = "restoreDatabaseTextBox";
            this.restoreDatabaseTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.restoreDatabaseTextBox.Size = new System.Drawing.Size(333, 27);
            this.restoreDatabaseTextBox.TabIndex = 13;
            // 
            // replaceExistingCheckBox
            // 
            this.replaceExistingCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.replaceExistingCheckBox.AutoSize = true;
            this.replaceExistingCheckBox.Location = new System.Drawing.Point(572, 211);
            this.replaceExistingCheckBox.Name = "replaceExistingCheckBox";
            this.replaceExistingCheckBox.Size = new System.Drawing.Size(144, 25);
            this.replaceExistingCheckBox.TabIndex = 14;
            this.replaceExistingCheckBox.Text = "استبدال الموجودة";
            // 
            // restoreButton
            // 
            this.restoreButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.restoreButton.Location = new System.Drawing.Point(396, 208);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(170, 32);
            this.restoreButton.TabIndex = 15;
            this.restoreButton.Text = "استعادة قاعدة البيانات";
            // 
            // systemStatusLabel
            // 
            this.systemStatusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.systemStatusLabel.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.systemStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.systemStatusLabel.Location = new System.Drawing.Point(12, 264);
            this.systemStatusLabel.Name = "systemStatusLabel";
            this.systemStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.systemStatusLabel.Size = new System.Drawing.Size(1218, 24);
            this.systemStatusLabel.TabIndex = 1;
            this.systemStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // logoGroup
            // 
            this.logoGroup.Controls.Add(this.logoActionsPanel);
            this.logoGroup.Controls.Add(this.logoPreviewPanel);
            this.logoGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.logoGroup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.logoGroup.Location = new System.Drawing.Point(18, 394);
            this.logoGroup.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.logoGroup.Name = "logoGroup";
            this.logoGroup.Padding = new System.Windows.Forms.Padding(12, 24, 12, 10);
            this.logoGroup.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.logoGroup.Size = new System.Drawing.Size(1242, 190);
            this.logoGroup.TabIndex = 3;
            this.logoGroup.TabStop = false;
            this.logoGroup.Text = "شعار المدرسة";
            // 
            // logoActionsPanel
            // 
            this.logoActionsPanel.Controls.Add(this.chooseLogoButton);
            this.logoActionsPanel.Controls.Add(this.removeLogoButton);
            this.logoActionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoActionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.logoActionsPanel.Location = new System.Drawing.Point(192, 45);
            this.logoActionsPanel.Name = "logoActionsPanel";
            this.logoActionsPanel.Padding = new System.Windows.Forms.Padding(22, 24, 22, 10);
            this.logoActionsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.logoActionsPanel.Size = new System.Drawing.Size(1038, 135);
            this.logoActionsPanel.TabIndex = 0;
            // 
            // chooseLogoButton
            // 
            this.chooseLogoButton.Location = new System.Drawing.Point(28, 30);
            this.chooseLogoButton.Margin = new System.Windows.Forms.Padding(6);
            this.chooseLogoButton.Name = "chooseLogoButton";
            this.chooseLogoButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chooseLogoButton.Size = new System.Drawing.Size(130, 38);
            this.chooseLogoButton.TabIndex = 0;
            this.chooseLogoButton.Text = "اختيار الشعار";
            // 
            // removeLogoButton
            // 
            this.removeLogoButton.Location = new System.Drawing.Point(170, 30);
            this.removeLogoButton.Margin = new System.Windows.Forms.Padding(6);
            this.removeLogoButton.Name = "removeLogoButton";
            this.removeLogoButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.removeLogoButton.Size = new System.Drawing.Size(115, 38);
            this.removeLogoButton.TabIndex = 1;
            this.removeLogoButton.Text = "حذف الشعار";
            // 
            // logoPreviewPanel
            // 
            this.logoPreviewPanel.BackColor = System.Drawing.Color.White;
            this.logoPreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logoPreviewPanel.Controls.Add(this.logoPictureBox);
            this.logoPreviewPanel.Controls.Add(this.logoPlaceholderLabel);
            this.logoPreviewPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.logoPreviewPanel.Location = new System.Drawing.Point(12, 45);
            this.logoPreviewPanel.Name = "logoPreviewPanel";
            this.logoPreviewPanel.Padding = new System.Windows.Forms.Padding(4);
            this.logoPreviewPanel.Size = new System.Drawing.Size(180, 135);
            this.logoPreviewPanel.TabIndex = 1;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPictureBox.Location = new System.Drawing.Point(4, 4);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(170, 125);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // logoPlaceholderLabel
            // 
            this.logoPlaceholderLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.logoPlaceholderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPlaceholderLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.logoPlaceholderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.logoPlaceholderLabel.Location = new System.Drawing.Point(4, 4);
            this.logoPlaceholderLabel.Name = "logoPlaceholderLabel";
            this.logoPlaceholderLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.logoPlaceholderLabel.Size = new System.Drawing.Size(170, 125);
            this.logoPlaceholderLabel.TabIndex = 1;
            this.logoPlaceholderLabel.Text = "معاينة الشعار";
            this.logoPlaceholderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // schoolDataGroup
            // 
            this.schoolDataGroup.Controls.Add(this.fieldsTable);
            this.schoolDataGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.schoolDataGroup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.schoolDataGroup.Location = new System.Drawing.Point(18, 14);
            this.schoolDataGroup.Name = "schoolDataGroup";
            this.schoolDataGroup.Padding = new System.Windows.Forms.Padding(12, 24, 12, 12);
            this.schoolDataGroup.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.schoolDataGroup.Size = new System.Drawing.Size(1242, 380);
            this.schoolDataGroup.TabIndex = 4;
            this.schoolDataGroup.TabStop = false;
            this.schoolDataGroup.Text = "بيانات المدرسة";
            // 
            // fieldsTable
            // 
            this.fieldsTable.ColumnCount = 4;
            this.fieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.fieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.fieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.fieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.fieldsTable.Controls.Add(this.schoolNameArLabel, 0, 0);
            this.fieldsTable.Controls.Add(this.schoolNameArTextBox, 1, 0);
            this.fieldsTable.Controls.Add(this.schoolNameEnLabel, 2, 0);
            this.fieldsTable.Controls.Add(this.schoolNameEnTextBox, 3, 0);
            this.fieldsTable.Controls.Add(this.organizationNameArLabel, 0, 1);
            this.fieldsTable.Controls.Add(this.organizationNameArTextBox, 1, 1);
            this.fieldsTable.Controls.Add(this.organizationNameEnLabel, 2, 1);
            this.fieldsTable.Controls.Add(this.organizationNameEnTextBox, 3, 1);
            this.fieldsTable.Controls.Add(this.addressArLabel, 0, 2);
            this.fieldsTable.Controls.Add(this.addressArTextBox, 1, 2);
            this.fieldsTable.Controls.Add(this.addressEnLabel, 2, 2);
            this.fieldsTable.Controls.Add(this.addressEnTextBox, 3, 2);
            this.fieldsTable.Controls.Add(this.phoneLabel, 0, 3);
            this.fieldsTable.Controls.Add(this.phoneTextBox, 1, 3);
            this.fieldsTable.Controls.Add(this.additionalPhoneLabel, 2, 3);
            this.fieldsTable.Controls.Add(this.additionalPhoneTextBox, 3, 3);
            this.fieldsTable.Controls.Add(this.emailLabel, 0, 4);
            this.fieldsTable.Controls.Add(this.emailTextBox, 1, 4);
            this.fieldsTable.Controls.Add(this.websiteLabel, 2, 4);
            this.fieldsTable.Controls.Add(this.websiteTextBox, 3, 4);
            this.fieldsTable.Controls.Add(this.principalNameArLabel, 0, 5);
            this.fieldsTable.Controls.Add(this.principalNameArTextBox, 1, 5);
            this.fieldsTable.Controls.Add(this.principalNameEnLabel, 2, 5);
            this.fieldsTable.Controls.Add(this.principalNameEnTextBox, 3, 5);
            this.fieldsTable.Controls.Add(this.licenseNumberLabel, 0, 6);
            this.fieldsTable.Controls.Add(this.licenseNumberTextBox, 1, 6);
            this.fieldsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldsTable.Location = new System.Drawing.Point(12, 45);
            this.fieldsTable.Name = "fieldsTable";
            this.fieldsTable.Padding = new System.Windows.Forms.Padding(4);
            this.fieldsTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fieldsTable.RowCount = 7;
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.fieldsTable.Size = new System.Drawing.Size(1218, 303);
            this.fieldsTable.TabIndex = 0;
            // 
            // schoolNameArLabel
            // 
            this.schoolNameArLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schoolNameArLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.schoolNameArLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.schoolNameArLabel.Location = new System.Drawing.Point(1076, 8);
            this.schoolNameArLabel.Margin = new System.Windows.Forms.Padding(4);
            this.schoolNameArLabel.Name = "schoolNameArLabel";
            this.schoolNameArLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.schoolNameArLabel.Size = new System.Drawing.Size(134, 37);
            this.schoolNameArLabel.TabIndex = 0;
            this.schoolNameArLabel.Text = "الاسم بالعربية *";
            this.schoolNameArLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // schoolNameArTextBox
            // 
            this.schoolNameArTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schoolNameArTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.schoolNameArTextBox.Location = new System.Drawing.Point(754, 10);
            this.schoolNameArTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.schoolNameArTextBox.Name = "schoolNameArTextBox";
            this.schoolNameArTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.schoolNameArTextBox.Size = new System.Drawing.Size(312, 27);
            this.schoolNameArTextBox.TabIndex = 1;
            this.schoolNameArTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // schoolNameEnLabel
            // 
            this.schoolNameEnLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schoolNameEnLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.schoolNameEnLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.schoolNameEnLabel.Location = new System.Drawing.Point(610, 8);
            this.schoolNameEnLabel.Margin = new System.Windows.Forms.Padding(4);
            this.schoolNameEnLabel.Name = "schoolNameEnLabel";
            this.schoolNameEnLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.schoolNameEnLabel.Size = new System.Drawing.Size(134, 37);
            this.schoolNameEnLabel.TabIndex = 2;
            this.schoolNameEnLabel.Text = "الاسم بالإنجليزية *";
            this.schoolNameEnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // schoolNameEnTextBox
            // 
            this.schoolNameEnTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schoolNameEnTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.schoolNameEnTextBox.Location = new System.Drawing.Point(10, 10);
            this.schoolNameEnTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.schoolNameEnTextBox.Name = "schoolNameEnTextBox";
            this.schoolNameEnTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.schoolNameEnTextBox.Size = new System.Drawing.Size(590, 27);
            this.schoolNameEnTextBox.TabIndex = 3;
            // 
            // organizationNameArLabel
            // 
            this.organizationNameArLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.organizationNameArLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.organizationNameArLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.organizationNameArLabel.Location = new System.Drawing.Point(1076, 53);
            this.organizationNameArLabel.Margin = new System.Windows.Forms.Padding(4);
            this.organizationNameArLabel.Name = "organizationNameArLabel";
            this.organizationNameArLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.organizationNameArLabel.Size = new System.Drawing.Size(134, 37);
            this.organizationNameArLabel.TabIndex = 4;
            this.organizationNameArLabel.Text = "الجهة بالعربية";
            this.organizationNameArLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // organizationNameArTextBox
            // 
            this.organizationNameArTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.organizationNameArTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.organizationNameArTextBox.Location = new System.Drawing.Point(754, 55);
            this.organizationNameArTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.organizationNameArTextBox.Name = "organizationNameArTextBox";
            this.organizationNameArTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.organizationNameArTextBox.Size = new System.Drawing.Size(312, 27);
            this.organizationNameArTextBox.TabIndex = 5;
            this.organizationNameArTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // organizationNameEnLabel
            // 
            this.organizationNameEnLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.organizationNameEnLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.organizationNameEnLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.organizationNameEnLabel.Location = new System.Drawing.Point(610, 53);
            this.organizationNameEnLabel.Margin = new System.Windows.Forms.Padding(4);
            this.organizationNameEnLabel.Name = "organizationNameEnLabel";
            this.organizationNameEnLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.organizationNameEnLabel.Size = new System.Drawing.Size(134, 37);
            this.organizationNameEnLabel.TabIndex = 6;
            this.organizationNameEnLabel.Text = "الجهة بالإنجليزية";
            this.organizationNameEnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // organizationNameEnTextBox
            // 
            this.organizationNameEnTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.organizationNameEnTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.organizationNameEnTextBox.Location = new System.Drawing.Point(10, 55);
            this.organizationNameEnTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.organizationNameEnTextBox.Name = "organizationNameEnTextBox";
            this.organizationNameEnTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.organizationNameEnTextBox.Size = new System.Drawing.Size(590, 27);
            this.organizationNameEnTextBox.TabIndex = 7;
            // 
            // addressArLabel
            // 
            this.addressArLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addressArLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.addressArLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.addressArLabel.Location = new System.Drawing.Point(1076, 98);
            this.addressArLabel.Margin = new System.Windows.Forms.Padding(4);
            this.addressArLabel.Name = "addressArLabel";
            this.addressArLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.addressArLabel.Size = new System.Drawing.Size(134, 37);
            this.addressArLabel.TabIndex = 8;
            this.addressArLabel.Text = "العنوان بالعربية";
            this.addressArLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // addressArTextBox
            // 
            this.addressArTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addressArTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.addressArTextBox.Location = new System.Drawing.Point(754, 100);
            this.addressArTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.addressArTextBox.Name = "addressArTextBox";
            this.addressArTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.addressArTextBox.Size = new System.Drawing.Size(312, 27);
            this.addressArTextBox.TabIndex = 9;
            this.addressArTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // addressEnLabel
            // 
            this.addressEnLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addressEnLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.addressEnLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.addressEnLabel.Location = new System.Drawing.Point(610, 98);
            this.addressEnLabel.Margin = new System.Windows.Forms.Padding(4);
            this.addressEnLabel.Name = "addressEnLabel";
            this.addressEnLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.addressEnLabel.Size = new System.Drawing.Size(134, 37);
            this.addressEnLabel.TabIndex = 10;
            this.addressEnLabel.Text = "العنوان بالإنجليزية";
            this.addressEnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // addressEnTextBox
            // 
            this.addressEnTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addressEnTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.addressEnTextBox.Location = new System.Drawing.Point(10, 100);
            this.addressEnTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.addressEnTextBox.Name = "addressEnTextBox";
            this.addressEnTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.addressEnTextBox.Size = new System.Drawing.Size(590, 27);
            this.addressEnTextBox.TabIndex = 11;
            // 
            // phoneLabel
            // 
            this.phoneLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phoneLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.phoneLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.phoneLabel.Location = new System.Drawing.Point(1076, 143);
            this.phoneLabel.Margin = new System.Windows.Forms.Padding(4);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.phoneLabel.Size = new System.Drawing.Size(134, 37);
            this.phoneLabel.TabIndex = 12;
            this.phoneLabel.Text = "الهاتف";
            this.phoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phoneTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.phoneTextBox.Location = new System.Drawing.Point(754, 145);
            this.phoneTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.phoneTextBox.Size = new System.Drawing.Size(312, 27);
            this.phoneTextBox.TabIndex = 13;
            // 
            // additionalPhoneLabel
            // 
            this.additionalPhoneLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.additionalPhoneLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.additionalPhoneLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.additionalPhoneLabel.Location = new System.Drawing.Point(610, 143);
            this.additionalPhoneLabel.Margin = new System.Windows.Forms.Padding(4);
            this.additionalPhoneLabel.Name = "additionalPhoneLabel";
            this.additionalPhoneLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.additionalPhoneLabel.Size = new System.Drawing.Size(134, 37);
            this.additionalPhoneLabel.TabIndex = 14;
            this.additionalPhoneLabel.Text = "هاتف إضافي";
            this.additionalPhoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // additionalPhoneTextBox
            // 
            this.additionalPhoneTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.additionalPhoneTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.additionalPhoneTextBox.Location = new System.Drawing.Point(10, 145);
            this.additionalPhoneTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.additionalPhoneTextBox.Name = "additionalPhoneTextBox";
            this.additionalPhoneTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.additionalPhoneTextBox.Size = new System.Drawing.Size(590, 27);
            this.additionalPhoneTextBox.TabIndex = 15;
            // 
            // emailLabel
            // 
            this.emailLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.emailLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.emailLabel.Location = new System.Drawing.Point(1076, 188);
            this.emailLabel.Margin = new System.Windows.Forms.Padding(4);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.emailLabel.Size = new System.Drawing.Size(134, 37);
            this.emailLabel.TabIndex = 16;
            this.emailLabel.Text = "البريد الإلكتروني";
            this.emailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.emailTextBox.Location = new System.Drawing.Point(754, 190);
            this.emailTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.emailTextBox.Size = new System.Drawing.Size(312, 27);
            this.emailTextBox.TabIndex = 17;
            // 
            // websiteLabel
            // 
            this.websiteLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.websiteLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.websiteLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.websiteLabel.Location = new System.Drawing.Point(610, 188);
            this.websiteLabel.Margin = new System.Windows.Forms.Padding(4);
            this.websiteLabel.Name = "websiteLabel";
            this.websiteLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.websiteLabel.Size = new System.Drawing.Size(134, 37);
            this.websiteLabel.TabIndex = 18;
            this.websiteLabel.Text = "الموقع الإلكتروني";
            this.websiteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // websiteTextBox
            // 
            this.websiteTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.websiteTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.websiteTextBox.Location = new System.Drawing.Point(10, 190);
            this.websiteTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.websiteTextBox.Name = "websiteTextBox";
            this.websiteTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.websiteTextBox.Size = new System.Drawing.Size(590, 27);
            this.websiteTextBox.TabIndex = 19;
            // 
            // principalNameArLabel
            // 
            this.principalNameArLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.principalNameArLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.principalNameArLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.principalNameArLabel.Location = new System.Drawing.Point(1076, 233);
            this.principalNameArLabel.Margin = new System.Windows.Forms.Padding(4);
            this.principalNameArLabel.Name = "principalNameArLabel";
            this.principalNameArLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.principalNameArLabel.Size = new System.Drawing.Size(134, 37);
            this.principalNameArLabel.TabIndex = 20;
            this.principalNameArLabel.Text = "اسم المدير بالعربية";
            this.principalNameArLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // principalNameArTextBox
            // 
            this.principalNameArTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.principalNameArTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.principalNameArTextBox.Location = new System.Drawing.Point(754, 235);
            this.principalNameArTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.principalNameArTextBox.Name = "principalNameArTextBox";
            this.principalNameArTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.principalNameArTextBox.Size = new System.Drawing.Size(312, 27);
            this.principalNameArTextBox.TabIndex = 21;
            this.principalNameArTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // principalNameEnLabel
            // 
            this.principalNameEnLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.principalNameEnLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.principalNameEnLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.principalNameEnLabel.Location = new System.Drawing.Point(610, 233);
            this.principalNameEnLabel.Margin = new System.Windows.Forms.Padding(4);
            this.principalNameEnLabel.Name = "principalNameEnLabel";
            this.principalNameEnLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.principalNameEnLabel.Size = new System.Drawing.Size(134, 37);
            this.principalNameEnLabel.TabIndex = 22;
            this.principalNameEnLabel.Text = "اسم المدير بالإنجليزية";
            this.principalNameEnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // principalNameEnTextBox
            // 
            this.principalNameEnTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.principalNameEnTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.principalNameEnTextBox.Location = new System.Drawing.Point(10, 235);
            this.principalNameEnTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.principalNameEnTextBox.Name = "principalNameEnTextBox";
            this.principalNameEnTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.principalNameEnTextBox.Size = new System.Drawing.Size(590, 27);
            this.principalNameEnTextBox.TabIndex = 23;
            // 
            // licenseNumberLabel
            // 
            this.licenseNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.licenseNumberLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.licenseNumberLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.licenseNumberLabel.Location = new System.Drawing.Point(1076, 278);
            this.licenseNumberLabel.Margin = new System.Windows.Forms.Padding(4);
            this.licenseNumberLabel.Name = "licenseNumberLabel";
            this.licenseNumberLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.licenseNumberLabel.Size = new System.Drawing.Size(134, 37);
            this.licenseNumberLabel.TabIndex = 24;
            this.licenseNumberLabel.Text = "رقم الترخيص / المنشأة";
            this.licenseNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // licenseNumberTextBox
            // 
            this.fieldsTable.SetColumnSpan(this.licenseNumberTextBox, 3);
            this.licenseNumberTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.licenseNumberTextBox.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.licenseNumberTextBox.Location = new System.Drawing.Point(10, 280);
            this.licenseNumberTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.licenseNumberTextBox.Name = "licenseNumberTextBox";
            this.licenseNumberTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.licenseNumberTextBox.Size = new System.Drawing.Size(1056, 27);
            this.licenseNumberTextBox.TabIndex = 25;
            // 
            // footerPanel
            // 
            this.footerPanel.BackColor = System.Drawing.Color.White;
            this.footerPanel.Controls.Add(this.saveButton);
            this.footerPanel.Controls.Add(this.statusLabel);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 469);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(18, 8, 18, 8);
            this.footerPanel.Size = new System.Drawing.Size(1299, 58);
            this.footerPanel.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.saveButton.Location = new System.Drawing.Point(1121, 8);
            this.saveButton.Margin = new System.Windows.Forms.Padding(6);
            this.saveButton.Name = "saveButton";
            this.saveButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.saveButton.Size = new System.Drawing.Size(160, 42);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "حفظ التغييرات";
            // 
            // statusLabel
            // 
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusLabel.Font = new System.Drawing.Font("Tahoma", 9F);
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.statusLabel.Location = new System.Drawing.Point(18, 8);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusLabel.Size = new System.Drawing.Size(1263, 42);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SettingsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1299, 527);
            this.headerPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            this.emailGroup.ResumeLayout(false);
            this.emailTable.ResumeLayout(false);
            this.emailTable.PerformLayout();
            this.generalGroup.ResumeLayout(false);
            this.generalTable.ResumeLayout(false);
            this.generalTable.PerformLayout();
            this.systemGroup.ResumeLayout(false);
            this.systemTable.ResumeLayout(false);
            this.systemTable.PerformLayout();
            this.logoGroup.ResumeLayout(false);
            this.logoActionsPanel.ResumeLayout(false);
            this.logoPreviewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.schoolDataGroup.ResumeLayout(false);
            this.fieldsTable.ResumeLayout(false);
            this.fieldsTable.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

    }
}
