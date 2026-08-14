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
            // actionsPanel
            actionsPanel.BackColor = Color.FromArgb(248, 250, 252);
            actionsPanel.ColumnCount = 4;
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            actionsPanel.Controls.Add(testConnectionButton, 0, 0);
            actionsPanel.Controls.Add(saveButton, 1, 0);
            actionsPanel.Controls.Add(backupButton, 2, 0);
            actionsPanel.Controls.Add(restoreButton, 3, 0);
            actionsPanel.Controls.Add(replaceExistingCheckBox, 0, 1);
            actionsPanel.SetColumnSpan(replaceExistingCheckBox, 4);
            actionsPanel.Dock = DockStyle.Top;
            actionsPanel.Padding = new Padding(18, 14, 18, 10);
            actionsPanel.RowCount = 2;
            actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            actionsPanel.Height = 102;
            // buttons
            testConnectionButton.Dock = DockStyle.Fill;
            testConnectionButton.Margin = new Padding(4);
            testConnectionButton.Text = "اختبار الاتصال";
            saveButton.Dock = DockStyle.Fill;
            saveButton.Margin = new Padding(4);
            saveButton.Text = "حفظ الإعدادات";
            backupButton.Dock = DockStyle.Fill;
            backupButton.Margin = new Padding(4);
            backupButton.Text = "إنشاء نسخة الآن";
            restoreButton.Dock = DockStyle.Fill;
            restoreButton.Margin = new Padding(4);
            restoreButton.Text = "استعادة نسخة";
            replaceExistingCheckBox.AutoSize = true;
            replaceExistingCheckBox.Anchor = AnchorStyles.Right;
            replaceExistingCheckBox.Text = "السماح باستبدال قاعدة البيانات الحالية (للمدير فقط)";
            replaceExistingCheckBox.RightToLeft = RightToLeft.Yes;
            // contentPanel
            contentPanel.AutoScroll = true;
            contentPanel.BackColor = Color.FromArgb(248, 250, 252);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(0);
            contentPanel.Controls.Add(actionsPanel);
            contentPanel.Controls.Add(fieldsTable);

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
            Controls.Add(contentPanel);
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
            contentPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
