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
        private TableLayoutPanel fieldsTable;
        private Label serverLabel;
        private Label databaseLabel;
        private Label backupDirectoryLabel;
        private TextBox serverTextBox;
        private TextBox databaseTextBox;
        private TextBox backupDirectoryTextBox;
        private Button browseButton;
        private TableLayoutPanel actionsPanel;
        private Button testConnectionButton;
        private Button saveButton;
        private Button backupButton;
        private Button restoreButton;
        private CheckBox replaceExistingCheckBox;
        private Panel contentPanel;
        private Panel footerPanel;
        private Label statusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            headerPanel = new Panel();
            titleLabel = new Label();
            subtitleLabel = new Label();
            fieldsTable = new TableLayoutPanel();
            serverLabel = new Label();
            databaseLabel = new Label();
            backupDirectoryLabel = new Label();
            serverTextBox = new TextBox();
            databaseTextBox = new TextBox();
            backupDirectoryTextBox = new TextBox();
            browseButton = new Button();
            actionsPanel = new TableLayoutPanel();
            testConnectionButton = new Button();
            saveButton = new Button();
            backupButton = new Button();
            restoreButton = new Button();
            replaceExistingCheckBox = new CheckBox();
            contentPanel = new Panel();
            footerPanel = new Panel();
            statusLabel = new Label();
            headerPanel.SuspendLayout();
            fieldsTable.SuspendLayout();
            actionsPanel.SuspendLayout();
            footerPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            SuspendLayout();
            // headerPanel
            headerPanel.BackColor = Color.White;
            headerPanel.Controls.Add(subtitleLabel);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 86;
            headerPanel.Padding = new Padding(18, 10, 18, 8);
            // titleLabel
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(15, 23, 42);
            titleLabel.Height = 34;
            titleLabel.Text = "الإعدادات والنسخ الاحتياطي";
            titleLabel.RightToLeft = RightToLeft.Yes;
            titleLabel.TextAlign = ContentAlignment.MiddleRight;
            // subtitleLabel
            subtitleLabel.Dock = DockStyle.Fill;
            subtitleLabel.Font = new Font("Tahoma", 9F);
            subtitleLabel.ForeColor = Color.FromArgb(71, 85, 105);
            subtitleLabel.Text = "إدارة اتصال خادم قاعدة البيانات وحماية بيانات النظام";
            subtitleLabel.RightToLeft = RightToLeft.Yes;
            subtitleLabel.TextAlign = ContentAlignment.MiddleRight;
            // fieldsTable
            fieldsTable.BackColor = Color.White;
            fieldsTable.AutoSize = false;
            fieldsTable.Margin = new Padding(0);
            fieldsTable.MinimumSize = new Size(520, 216);
            fieldsTable.RightToLeft = RightToLeft.Yes;
            fieldsTable.ColumnCount = 3;
            fieldsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            fieldsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            fieldsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            fieldsTable.Controls.Add(serverLabel, 0, 0);
            fieldsTable.Controls.Add(serverTextBox, 1, 0);
            fieldsTable.Controls.Add(databaseLabel, 0, 1);
            fieldsTable.Controls.Add(databaseTextBox, 1, 1);
            fieldsTable.Controls.Add(backupDirectoryLabel, 0, 2);
            fieldsTable.Controls.Add(backupDirectoryTextBox, 1, 2);
            fieldsTable.Controls.Add(browseButton, 2, 2);
            fieldsTable.Dock = DockStyle.Top;
            fieldsTable.Padding = new Padding(24, 18, 24, 18);
            fieldsTable.RowCount = 3;
            fieldsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            fieldsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            fieldsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            fieldsTable.Height = 216;
            // labels and fields
            serverLabel.Text = "خادم SQL Server";
            databaseLabel.Text = "قاعدة البيانات";
            backupDirectoryLabel.Text = "مجلد النسخ الاحتياطي";
            serverLabel.Dock = DockStyle.Fill;
            serverLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            serverLabel.ForeColor = Color.FromArgb(55, 65, 81);
            serverLabel.AutoSize = false;
            serverLabel.Margin = new Padding(4);
            serverLabel.TextAlign = ContentAlignment.MiddleRight;
            serverLabel.RightToLeft = RightToLeft.Yes;
            databaseLabel.Dock = DockStyle.Fill;
            databaseLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            databaseLabel.ForeColor = Color.FromArgb(55, 65, 81);
            databaseLabel.AutoSize = false;
            databaseLabel.Margin = new Padding(4);
            databaseLabel.TextAlign = ContentAlignment.MiddleRight;
            databaseLabel.RightToLeft = RightToLeft.Yes;
            backupDirectoryLabel.Dock = DockStyle.Fill;
            backupDirectoryLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            backupDirectoryLabel.ForeColor = Color.FromArgb(55, 65, 81);
            backupDirectoryLabel.AutoSize = false;
            backupDirectoryLabel.Margin = new Padding(4);
            backupDirectoryLabel.TextAlign = ContentAlignment.MiddleRight;
            backupDirectoryLabel.RightToLeft = RightToLeft.Yes;
            serverTextBox.RightToLeft = RightToLeft.Yes;
            serverTextBox.TextAlign = HorizontalAlignment.Right;
            serverTextBox.Dock = DockStyle.Fill;
            serverTextBox.Font = new Font("Tahoma", 10F);
            serverTextBox.Margin = new Padding(8, 7, 8, 7);
            databaseTextBox.RightToLeft = RightToLeft.Yes;
            databaseTextBox.TextAlign = HorizontalAlignment.Right;
            databaseTextBox.Dock = DockStyle.Fill;
            databaseTextBox.Font = new Font("Tahoma", 10F);
            databaseTextBox.Margin = new Padding(8, 7, 8, 7);
            backupDirectoryTextBox.RightToLeft = RightToLeft.Yes;
            backupDirectoryTextBox.TextAlign = HorizontalAlignment.Right;
            backupDirectoryTextBox.Dock = DockStyle.Fill;
            backupDirectoryTextBox.Font = new Font("Tahoma", 10F);
            backupDirectoryTextBox.Margin = new Padding(8, 7, 8, 7);
            browseButton.AutoSize = false;
            browseButton.Dock = DockStyle.Fill;
            browseButton.Text = "اختيار مجلد";
            browseButton.RightToLeft = RightToLeft.Yes;
            // actionsPanel
            actionsPanel.BackColor = Color.FromArgb(248, 250, 252);
            actionsPanel.AutoSize = false;
            actionsPanel.Margin = new Padding(0);
            actionsPanel.MinimumSize = new Size(520, 178);
            actionsPanel.RightToLeft = RightToLeft.Yes;
            actionsPanel.ColumnCount = 2;
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actionsPanel.Controls.Add(testConnectionButton, 0, 0);
            actionsPanel.Controls.Add(saveButton, 1, 0);
            actionsPanel.Controls.Add(backupButton, 0, 1);
            actionsPanel.Controls.Add(restoreButton, 1, 1);
            actionsPanel.Controls.Add(replaceExistingCheckBox, 0, 2);
            actionsPanel.SetColumnSpan(replaceExistingCheckBox, 2);
            actionsPanel.Dock = DockStyle.Top;
            actionsPanel.Padding = new Padding(24, 14, 24, 14);
            actionsPanel.RowCount = 3;
            actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            actionsPanel.Height = 178;
            // buttons
            testConnectionButton.Dock = DockStyle.Fill;
            testConnectionButton.Margin = new Padding(4);
            testConnectionButton.Text = "اختبار الاتصال";
            testConnectionButton.RightToLeft = RightToLeft.Yes;
            saveButton.Dock = DockStyle.Fill;
            saveButton.Margin = new Padding(4);
            saveButton.Text = "حفظ الإعدادات";
            saveButton.RightToLeft = RightToLeft.Yes;
            backupButton.Dock = DockStyle.Fill;
            backupButton.Margin = new Padding(4);
            backupButton.Text = "إنشاء نسخة الآن";
            backupButton.RightToLeft = RightToLeft.Yes;
            restoreButton.Dock = DockStyle.Fill;
            restoreButton.Margin = new Padding(4);
            restoreButton.Text = "استعادة نسخة";
            restoreButton.RightToLeft = RightToLeft.Yes;
            replaceExistingCheckBox.AutoSize = false;
            replaceExistingCheckBox.Dock = DockStyle.Fill;
            replaceExistingCheckBox.Anchor = AnchorStyles.Right;
            replaceExistingCheckBox.Margin = new Padding(6, 4, 6, 4);
            replaceExistingCheckBox.Text = "السماح باستبدال قاعدة البيانات الحالية (للمدير فقط)";
            replaceExistingCheckBox.RightToLeft = RightToLeft.Yes;
            // contentPanel
            contentPanel.AutoScroll = true;
            contentPanel.RightToLeft = RightToLeft.Yes;
            contentPanel.BackColor = Color.FromArgb(248, 250, 252);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(0, 0, 0, 12);
            contentPanel.Controls.Add(actionsPanel);
            contentPanel.Controls.Add(fieldsTable);

            // footerPanel
            footerPanel.BackColor = Color.White;
            footerPanel.RightToLeft = RightToLeft.Yes;
            footerPanel.Controls.Add(statusLabel);
            footerPanel.Dock = DockStyle.Bottom;
            footerPanel.Height = 46;
            footerPanel.Padding = new Padding(18, 4, 18, 4);
            statusLabel.Dock = DockStyle.Fill;
            statusLabel.Font = new Font("Tahoma", 9F);
            statusLabel.ForeColor = Color.FromArgb(71, 85, 105);
            statusLabel.RightToLeft = RightToLeft.Yes;
            statusLabel.TextAlign = ContentAlignment.MiddleRight;
            // SettingsForm
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            Controls.Add(contentPanel);
            Controls.Add(footerPanel);
            Controls.Add(headerPanel);
            Dock = DockStyle.Fill;
            Name = "SettingsForm";
            Text = "إعدادات النظام والنسخ الاحتياطي";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1120, 650);
            headerPanel.ResumeLayout(false);
            fieldsTable.ResumeLayout(false);
            fieldsTable.PerformLayout();
            actionsPanel.ResumeLayout(false);
            actionsPanel.PerformLayout();
            footerPanel.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
