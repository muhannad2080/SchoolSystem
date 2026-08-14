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
        private Panel actionsPanel;
        private Button testConnectionButton;
        private Button saveButton;
        private Button backupButton;
        private Button restoreButton;
        private CheckBox replaceExistingCheckBox;
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
            actionsPanel = new Panel();
            testConnectionButton = new Button();
            saveButton = new Button();
            backupButton = new Button();
            restoreButton = new Button();
            replaceExistingCheckBox = new CheckBox();
            footerPanel = new Panel();
            statusLabel = new Label();
            headerPanel.SuspendLayout();
            fieldsTable.SuspendLayout();
            actionsPanel.SuspendLayout();
            footerPanel.SuspendLayout();
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
            titleLabel.TextAlign = ContentAlignment.MiddleRight;
            // subtitleLabel
            subtitleLabel.Dock = DockStyle.Fill;
            subtitleLabel.Font = new Font("Tahoma", 9F);
            subtitleLabel.ForeColor = Color.FromArgb(71, 85, 105);
            subtitleLabel.Text = "إدارة اتصال SQL Server وحماية بيانات النظام";
            subtitleLabel.TextAlign = ContentAlignment.MiddleRight;
            // fieldsTable
            fieldsTable.BackColor = Color.White;
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
            fieldsTable.Padding = new Padding(18, 22, 18, 10);
            fieldsTable.RowCount = 3;
            fieldsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            fieldsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            fieldsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            fieldsTable.Height = 178;
            // labels and fields
            serverLabel.Text = "خادم SQL Server";
            databaseLabel.Text = "قاعدة البيانات";
            backupDirectoryLabel.Text = "مجلد النسخ الاحتياطي";
            serverLabel.Dock = DockStyle.Fill;
            serverLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            serverLabel.ForeColor = Color.FromArgb(55, 65, 81);
            serverLabel.TextAlign = ContentAlignment.MiddleRight;
            databaseLabel.Dock = DockStyle.Fill;
            databaseLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            databaseLabel.ForeColor = Color.FromArgb(55, 65, 81);
            databaseLabel.TextAlign = ContentAlignment.MiddleRight;
            backupDirectoryLabel.Dock = DockStyle.Fill;
            backupDirectoryLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            backupDirectoryLabel.ForeColor = Color.FromArgb(55, 65, 81);
            backupDirectoryLabel.TextAlign = ContentAlignment.MiddleRight;
            serverTextBox.Dock = DockStyle.Fill;
            serverTextBox.Font = new Font("Tahoma", 10F);
            serverTextBox.Margin = new Padding(8, 7, 8, 7);
            databaseTextBox.Dock = DockStyle.Fill;
            databaseTextBox.Font = new Font("Tahoma", 10F);
            databaseTextBox.Margin = new Padding(8, 7, 8, 7);
            backupDirectoryTextBox.Dock = DockStyle.Fill;
            backupDirectoryTextBox.Font = new Font("Tahoma", 10F);
            backupDirectoryTextBox.Margin = new Padding(8, 7, 8, 7);
            browseButton.Dock = DockStyle.Fill;
            browseButton.Text = "اختيار مجلد";
            browseButton.Click += BrowseButton_Click;
            // actionsPanel
            actionsPanel.BackColor = Color.FromArgb(248, 250, 252);
            actionsPanel.Controls.Add(replaceExistingCheckBox);
            actionsPanel.Controls.Add(restoreButton);
            actionsPanel.Controls.Add(backupButton);
            actionsPanel.Controls.Add(saveButton);
            actionsPanel.Controls.Add(testConnectionButton);
            actionsPanel.Dock = DockStyle.Top;
            actionsPanel.Height = 130;
            actionsPanel.Padding = new Padding(18, 18, 18, 12);
            // buttons
            testConnectionButton.Text = "اختبار الاتصال";
            testConnectionButton.Width = 150;
            testConnectionButton.Location = new Point(18, 18);
            testConnectionButton.Click += TestConnectionButton_Click;
            saveButton.Text = "حفظ الإعدادات";
            saveButton.Width = 150;
            saveButton.Location = new Point(178, 18);
            saveButton.Click += SaveButton_Click;
            backupButton.Text = "إنشاء نسخة الآن";
            backupButton.Width = 160;
            backupButton.Location = new Point(338, 18);
            backupButton.Click += BackupButton_Click;
            restoreButton.Text = "استعادة نسخة";
            restoreButton.Width = 150;
            restoreButton.Location = new Point(508, 18);
            restoreButton.Click += RestoreButton_Click;
            replaceExistingCheckBox.AutoSize = true;
            replaceExistingCheckBox.Location = new Point(18, 76);
            replaceExistingCheckBox.Text = "السماح باستبدال قاعدة البيانات الحالية (للمدير فقط)";
            replaceExistingCheckBox.RightToLeft = RightToLeft.Yes;
            // footerPanel
            footerPanel.BackColor = Color.White;
            footerPanel.Controls.Add(statusLabel);
            footerPanel.Dock = DockStyle.Bottom;
            footerPanel.Height = 46;
            footerPanel.Padding = new Padding(18, 4, 18, 4);
            statusLabel.Dock = DockStyle.Fill;
            statusLabel.Font = new Font("Tahoma", 9F);
            statusLabel.ForeColor = Color.FromArgb(71, 85, 105);
            statusLabel.TextAlign = ContentAlignment.MiddleRight;
            // SettingsForm
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            Controls.Add(actionsPanel);
            Controls.Add(fieldsTable);
            Controls.Add(footerPanel);
            Controls.Add(headerPanel);
            Dock = DockStyle.Fill;
            Name = "SettingsForm";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1120, 650);
            headerPanel.ResumeLayout(false);
            fieldsTable.ResumeLayout(false);
            fieldsTable.PerformLayout();
            actionsPanel.ResumeLayout(false);
            actionsPanel.PerformLayout();
            footerPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
