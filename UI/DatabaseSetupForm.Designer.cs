// DatabaseSetupForm.Designer.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolSystem.UI
{
    partial class DatabaseSetupForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelBackground = new System.Windows.Forms.Panel();
            this.layoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.panelCard = new System.Windows.Forms.Panel();
            this.cardLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.sepTop = new System.Windows.Forms.Panel();
            this.lblServerLabel = new System.Windows.Forms.Label();
            this.pnlServer = new System.Windows.Forms.Panel();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServerIcon = new System.Windows.Forms.Label();
            this.lblDbLabel = new System.Windows.Forms.Label();
            this.pnlDb = new System.Windows.Forms.Panel();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.lblDbIcon = new System.Windows.Forms.Label();
            this.lblAuthLabel = new System.Windows.Forms.Label();
            this.pnlAuth = new System.Windows.Forms.TableLayoutPanel();
            this.rbWindows = new System.Windows.Forms.RadioButton();
            this.rbSql = new System.Windows.Forms.RadioButton();
            this.lblSqlUser = new System.Windows.Forms.Label();
            this.pnlSqlUser = new System.Windows.Forms.Panel();
            this.txtSqlUser = new System.Windows.Forms.TextBox();
            this.lblSqlPass = new System.Windows.Forms.Label();
            this.pnlSqlPass = new System.Windows.Forms.Panel();
            this.txtSqlPass = new System.Windows.Forms.TextBox();
            this.btnTogglePassword = new System.Windows.Forms.Button();
            this.lblBackupDir = new System.Windows.Forms.Label();
            this.pnlBackupDir = new System.Windows.Forms.Panel();
            this.txtBackupDir = new System.Windows.Forms.TextBox();
            this.btnBrowseBackup = new System.Windows.Forms.Button();
            this.sepMid = new System.Windows.Forms.Panel();
            this.statusLine = new System.Windows.Forms.TableLayoutPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.actionsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.panelBackground.SuspendLayout();
            this.layoutRoot.SuspendLayout();
            this.panelCard.SuspendLayout();
            this.cardLayout.SuspendLayout();
            this.pnlServer.SuspendLayout();
            this.pnlDb.SuspendLayout();
            this.pnlAuth.SuspendLayout();
            this.pnlSqlUser.SuspendLayout();
            this.pnlSqlPass.SuspendLayout();
            this.pnlBackupDir.SuspendLayout();
            this.statusLine.SuspendLayout();
            this.actionsLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelBackground.Controls.Add(this.layoutRoot);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(1040, 860);
            this.panelBackground.TabIndex = 0;
            // 
            // layoutRoot
            // 
            this.layoutRoot.BackColor = System.Drawing.Color.Transparent;
            this.layoutRoot.ColumnCount = 3;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.layoutRoot.Controls.Add(this.panelCard, 1, 1);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.RowCount = 3;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84F));
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.layoutRoot.Size = new System.Drawing.Size(1040, 860);
            this.layoutRoot.TabIndex = 0;
            // 
            // panelCard
            // 
            this.panelCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelCard.BackColor = System.Drawing.Color.White;
            this.panelCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCard.Controls.Add(this.cardLayout);
            this.panelCard.Location = new System.Drawing.Point(221, 89);
            this.panelCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(28);
            this.panelCard.Size = new System.Drawing.Size(600, 680);
            this.panelCard.TabIndex = 1;
            // 
            // cardLayout
            // 
            this.cardLayout.ColumnCount = 1;
            this.cardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cardLayout.Controls.Add(this.lblIcon, 0, 0);
            this.cardLayout.Controls.Add(this.lblTitle, 0, 1);
            this.cardLayout.Controls.Add(this.lblSubtitle, 0, 2);
            this.cardLayout.Controls.Add(this.sepTop, 0, 3);
            this.cardLayout.Controls.Add(this.lblServerLabel, 0, 4);
            this.cardLayout.Controls.Add(this.pnlServer, 0, 5);
            this.cardLayout.Controls.Add(this.lblDbLabel, 0, 6);
            this.cardLayout.Controls.Add(this.pnlDb, 0, 7);
            this.cardLayout.Controls.Add(this.lblAuthLabel, 0, 8);
            this.cardLayout.Controls.Add(this.pnlAuth, 0, 9);
            this.cardLayout.Controls.Add(this.lblSqlUser, 0, 10);
            this.cardLayout.Controls.Add(this.pnlSqlUser, 0, 11);
            this.cardLayout.Controls.Add(this.lblSqlPass, 0, 12);
            this.cardLayout.Controls.Add(this.pnlSqlPass, 0, 13);
            this.cardLayout.Controls.Add(this.lblBackupDir, 0, 14);
            this.cardLayout.Controls.Add(this.pnlBackupDir, 0, 15);
            this.cardLayout.Controls.Add(this.sepMid, 0, 16);
            this.cardLayout.Controls.Add(this.statusLine, 0, 17);
            this.cardLayout.Controls.Add(this.actionsLayout, 0, 18);
            this.cardLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardLayout.Location = new System.Drawing.Point(28, 28);
            this.cardLayout.Margin = new System.Windows.Forms.Padding(0);
            this.cardLayout.Name = "cardLayout";
            this.cardLayout.RowCount = 19;
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.Size = new System.Drawing.Size(542, 622);
            this.cardLayout.TabIndex = 0;
            // 
            // lblIcon
            // 
            this.lblIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 26F);
            this.lblIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.lblIcon.Location = new System.Drawing.Point(3, 0);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(536, 54);
            this.lblIcon.TabIndex = 0;
            this.lblIcon.Text = "";
            this.lblIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.lblTitle.Location = new System.Drawing.Point(3, 54);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(536, 36);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "إعداد قاعدة البيانات";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(118)))), ((int)(((byte)(133)))));
            this.lblSubtitle.Location = new System.Drawing.Point(3, 90);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(536, 26);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "لم يتم العثور على قاعدة البيانات. حدّد الخادم وأكمل البيانات لإنشائها تلقائياً.";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sepTop
            // 
            this.sepTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
            this.sepTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sepTop.Location = new System.Drawing.Point(0, 129);
            this.sepTop.Margin = new System.Windows.Forms.Padding(0);
            this.sepTop.Name = "sepTop";
            this.sepTop.Size = new System.Drawing.Size(542, 1);
            this.sepTop.TabIndex = 3;
            // 
            // lblServerLabel
            // 
            this.lblServerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServerLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblServerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(58)))), ((int)(((byte)(72)))));
            this.lblServerLabel.Location = new System.Drawing.Point(3, 130);
            this.lblServerLabel.Name = "lblServerLabel";
            this.lblServerLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblServerLabel.Size = new System.Drawing.Size(536, 24);
            this.lblServerLabel.TabIndex = 4;
            this.lblServerLabel.Text = "خادم SQL Server (المثيل)";
            this.lblServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlServer
            // 
            this.pnlServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlServer.Controls.Add(this.txtServer);
            this.pnlServer.Controls.Add(this.lblServerIcon);
            this.pnlServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlServer.Location = new System.Drawing.Point(0, 154);
            this.pnlServer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlServer.Name = "pnlServer";
            this.pnlServer.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.pnlServer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlServer.Size = new System.Drawing.Size(542, 44);
            this.pnlServer.TabIndex = 5;
            // 
            // txtServer
            // 
            this.txtServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtServer.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.txtServer.Location = new System.Drawing.Point(12, 4);
            this.txtServer.Margin = new System.Windows.Forms.Padding(0);
            this.txtServer.Name = "txtServer";
            this.txtServer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtServer.Size = new System.Drawing.Size(486, 25);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = ".";
            // 
            // lblServerIcon
            // 
            this.lblServerIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblServerIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 15F);
            this.lblServerIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.lblServerIcon.Location = new System.Drawing.Point(498, 4);
            this.lblServerIcon.Name = "lblServerIcon";
            this.lblServerIcon.Size = new System.Drawing.Size(30, 34);
            this.lblServerIcon.TabIndex = 0;
            this.lblServerIcon.Text = "";
            this.lblServerIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDbLabel
            // 
            this.lblDbLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDbLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDbLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(58)))), ((int)(((byte)(72)))));
            this.lblDbLabel.Location = new System.Drawing.Point(3, 198);
            this.lblDbLabel.Name = "lblDbLabel";
            this.lblDbLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblDbLabel.Size = new System.Drawing.Size(536, 24);
            this.lblDbLabel.TabIndex = 6;
            this.lblDbLabel.Text = "اسم قاعدة البيانات";
            this.lblDbLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDb
            // 
            this.pnlDb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlDb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDb.Controls.Add(this.txtDatabase);
            this.pnlDb.Controls.Add(this.lblDbIcon);
            this.pnlDb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDb.Location = new System.Drawing.Point(0, 222);
            this.pnlDb.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDb.Name = "pnlDb";
            this.pnlDb.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.pnlDb.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlDb.Size = new System.Drawing.Size(542, 44);
            this.pnlDb.TabIndex = 7;
            // 
            // txtDatabase
            // 
            this.txtDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtDatabase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDatabase.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtDatabase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.txtDatabase.Location = new System.Drawing.Point(12, 4);
            this.txtDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtDatabase.Size = new System.Drawing.Size(486, 25);
            this.txtDatabase.TabIndex = 1;
            this.txtDatabase.Text = "SchoolDB";
            // 
            // lblDbIcon
            // 
            this.lblDbIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDbIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 15F);
            this.lblDbIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.lblDbIcon.Location = new System.Drawing.Point(498, 4);
            this.lblDbIcon.Name = "lblDbIcon";
            this.lblDbIcon.Size = new System.Drawing.Size(30, 34);
            this.lblDbIcon.TabIndex = 0;
            this.lblDbIcon.Text = "";
            this.lblDbIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAuthLabel
            // 
            this.lblAuthLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAuthLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAuthLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(58)))), ((int)(((byte)(72)))));
            this.lblAuthLabel.Location = new System.Drawing.Point(3, 266);
            this.lblAuthLabel.Name = "lblAuthLabel";
            this.lblAuthLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAuthLabel.Size = new System.Drawing.Size(536, 24);
            this.lblAuthLabel.TabIndex = 8;
            this.lblAuthLabel.Text = "نوع الاتصال (المصادقة)";
            this.lblAuthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlAuth
            // 
            this.pnlAuth.ColumnCount = 2;
            this.pnlAuth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlAuth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlAuth.Controls.Add(this.rbWindows, 0, 0);
            this.pnlAuth.Controls.Add(this.rbSql, 1, 0);
            this.pnlAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAuth.Location = new System.Drawing.Point(0, 290);
            this.pnlAuth.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAuth.Name = "pnlAuth";
            this.pnlAuth.RowCount = 1;
            this.pnlAuth.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlAuth.Size = new System.Drawing.Size(542, 34);
            this.pnlAuth.TabIndex = 9;
            // 
            // rbWindows
            // 
            this.rbWindows.AutoSize = true;
            this.rbWindows.Checked = true;
            this.rbWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbWindows.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbWindows.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.rbWindows.Location = new System.Drawing.Point(274, 3);
            this.rbWindows.Name = "rbWindows";
            this.rbWindows.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbWindows.Size = new System.Drawing.Size(265, 28);
            this.rbWindows.TabIndex = 0;
            this.rbWindows.TabStop = true;
            this.rbWindows.Text = "مصادقة Windows";
            this.rbWindows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbWindows.UseVisualStyleBackColor = true;
            this.rbWindows.CheckedChanged += new System.EventHandler(this.Auth_CheckedChanged);
            // 
            // rbSql
            // 
            this.rbSql.AutoSize = true;
            this.rbSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSql.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbSql.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.rbSql.Location = new System.Drawing.Point(3, 3);
            this.rbSql.Name = "rbSql";
            this.rbSql.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbSql.Size = new System.Drawing.Size(265, 28);
            this.rbSql.TabIndex = 1;
            this.rbSql.TabStop = true;
            this.rbSql.Text = "مصادقة SQL Server";
            this.rbSql.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbSql.UseVisualStyleBackColor = true;
            this.rbSql.CheckedChanged += new System.EventHandler(this.Auth_CheckedChanged);
            // 
            // lblSqlUser
            // 
            this.lblSqlUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSqlUser.Enabled = false;
            this.lblSqlUser.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSqlUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(58)))), ((int)(((byte)(72)))));
            this.lblSqlUser.Location = new System.Drawing.Point(3, 324);
            this.lblSqlUser.Name = "lblSqlUser";
            this.lblSqlUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSqlUser.Size = new System.Drawing.Size(536, 20);
            this.lblSqlUser.TabIndex = 10;
            this.lblSqlUser.Text = "مستخدم SQL";
            this.lblSqlUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSqlUser
            // 
            this.pnlSqlUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlSqlUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSqlUser.Controls.Add(this.txtSqlUser);
            this.pnlSqlUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSqlUser.Enabled = false;
            this.pnlSqlUser.Location = new System.Drawing.Point(0, 344);
            this.pnlSqlUser.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSqlUser.Name = "pnlSqlUser";
            this.pnlSqlUser.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.pnlSqlUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlSqlUser.Size = new System.Drawing.Size(542, 42);
            this.pnlSqlUser.TabIndex = 11;
            // 
            // txtSqlUser
            // 
            this.txtSqlUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtSqlUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSqlUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSqlUser.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSqlUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.txtSqlUser.Location = new System.Drawing.Point(12, 4);
            this.txtSqlUser.Margin = new System.Windows.Forms.Padding(0);
            this.txtSqlUser.MaxLength = 128;
            this.txtSqlUser.Name = "txtSqlUser";
            this.txtSqlUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSqlUser.Size = new System.Drawing.Size(516, 25);
            this.txtSqlUser.TabIndex = 1;
            // 
            // lblSqlPass
            // 
            this.lblSqlPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSqlPass.Enabled = false;
            this.lblSqlPass.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSqlPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(58)))), ((int)(((byte)(72)))));
            this.lblSqlPass.Location = new System.Drawing.Point(3, 386);
            this.lblSqlPass.Name = "lblSqlPass";
            this.lblSqlPass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSqlPass.Size = new System.Drawing.Size(536, 20);
            this.lblSqlPass.TabIndex = 12;
            this.lblSqlPass.Text = "كلمة مرور مستخدم SQL";
            this.lblSqlPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSqlPass
            // 
            this.pnlSqlPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlSqlPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSqlPass.Controls.Add(this.txtSqlPass);
            this.pnlSqlPass.Controls.Add(this.btnTogglePassword);
            this.pnlSqlPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSqlPass.Enabled = false;
            this.pnlSqlPass.Location = new System.Drawing.Point(0, 406);
            this.pnlSqlPass.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSqlPass.Name = "pnlSqlPass";
            this.pnlSqlPass.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.pnlSqlPass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlSqlPass.Size = new System.Drawing.Size(542, 42);
            this.pnlSqlPass.TabIndex = 13;
            // 
            // txtSqlPass
            // 
            this.txtSqlPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtSqlPass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSqlPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSqlPass.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSqlPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.txtSqlPass.Location = new System.Drawing.Point(50, 4);
            this.txtSqlPass.Margin = new System.Windows.Forms.Padding(0);
            this.txtSqlPass.MaxLength = 128;
            this.txtSqlPass.Name = "txtSqlPass";
            this.txtSqlPass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSqlPass.Size = new System.Drawing.Size(478, 25);
            this.txtSqlPass.TabIndex = 1;
            this.txtSqlPass.UseSystemPasswordChar = true;
            // 
            // btnTogglePassword
            // 
            this.btnTogglePassword.BackColor = System.Drawing.Color.Transparent;
            this.btnTogglePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTogglePassword.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTogglePassword.FlatAppearance.BorderSize = 0;
            this.btnTogglePassword.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.btnTogglePassword.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(243)))), ((int)(((byte)(245)))));
            this.btnTogglePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTogglePassword.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F);
            this.btnTogglePassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(103)))), ((int)(((byte)(118)))));
            this.btnTogglePassword.Location = new System.Drawing.Point(12, 4);
            this.btnTogglePassword.Margin = new System.Windows.Forms.Padding(0);
            this.btnTogglePassword.Name = "btnTogglePassword";
            this.btnTogglePassword.Size = new System.Drawing.Size(38, 32);
            this.btnTogglePassword.TabIndex = 2;
            this.btnTogglePassword.Text = "";
            this.btnTogglePassword.UseVisualStyleBackColor = false;
            this.btnTogglePassword.Click += new System.EventHandler(this.btnTogglePassword_Click);
            // 
            // lblBackupDir
            // 
            this.lblBackupDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackupDir.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBackupDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(58)))), ((int)(((byte)(72)))));
            this.lblBackupDir.Location = new System.Drawing.Point(3, 448);
            this.lblBackupDir.Name = "lblBackupDir";
            this.lblBackupDir.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblBackupDir.Size = new System.Drawing.Size(536, 20);
            this.lblBackupDir.TabIndex = 14;
            this.lblBackupDir.Text = "مجلد النسخ الاحتياطية";
            this.lblBackupDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBackupDir
            // 
            this.pnlBackupDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlBackupDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackupDir.Controls.Add(this.txtBackupDir);
            this.pnlBackupDir.Controls.Add(this.btnBrowseBackup);
            this.pnlBackupDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackupDir.Location = new System.Drawing.Point(0, 468);
            this.pnlBackupDir.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBackupDir.Name = "pnlBackupDir";
            this.pnlBackupDir.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.pnlBackupDir.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlBackupDir.Size = new System.Drawing.Size(542, 42);
            this.pnlBackupDir.TabIndex = 15;
            // 
            // txtBackupDir
            // 
            this.txtBackupDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtBackupDir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBackupDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBackupDir.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtBackupDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.txtBackupDir.Location = new System.Drawing.Point(54, 4);
            this.txtBackupDir.Margin = new System.Windows.Forms.Padding(0);
            this.txtBackupDir.Name = "txtBackupDir";
            this.txtBackupDir.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtBackupDir.Size = new System.Drawing.Size(474, 24);
            this.txtBackupDir.TabIndex = 1;
            // 
            // btnBrowseBackup
            // 
            this.btnBrowseBackup.BackColor = System.Drawing.Color.White;
            this.btnBrowseBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseBackup.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBrowseBackup.FlatAppearance.BorderSize = 0;
            this.btnBrowseBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseBackup.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F);
            this.btnBrowseBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.btnBrowseBackup.Location = new System.Drawing.Point(12, 4);
            this.btnBrowseBackup.Margin = new System.Windows.Forms.Padding(0);
            this.btnBrowseBackup.Name = "btnBrowseBackup";
            this.btnBrowseBackup.Size = new System.Drawing.Size(42, 32);
            this.btnBrowseBackup.TabIndex = 2;
            this.btnBrowseBackup.Text = "";
            this.btnBrowseBackup.UseVisualStyleBackColor = false;
            this.btnBrowseBackup.Click += new System.EventHandler(this.btnBrowseBackup_Click);
            // 
            // sepMid
            // 
            this.sepMid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
            this.sepMid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sepMid.Location = new System.Drawing.Point(0, 521);
            this.sepMid.Margin = new System.Windows.Forms.Padding(0);
            this.sepMid.Name = "sepMid";
            this.sepMid.Size = new System.Drawing.Size(542, 1);
            this.sepMid.TabIndex = 16;
            // 
            // statusLine
            // 
            this.statusLine.ColumnCount = 1;
            this.statusLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.statusLine.Controls.Add(this.lblStatus, 0, 0);
            this.statusLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusLine.Location = new System.Drawing.Point(0, 522);
            this.statusLine.Margin = new System.Windows.Forms.Padding(0);
            this.statusLine.Name = "statusLine";
            this.statusLine.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.statusLine.RowCount = 1;
            this.statusLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.statusLine.Size = new System.Drawing.Size(542, 36);
            this.statusLine.TabIndex = 17;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(103)))), ((int)(((byte)(118)))));
            this.lblStatus.Location = new System.Drawing.Point(3, 2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblStatus.Size = new System.Drawing.Size(536, 34);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "أدخل بيانات الاتصال ثم اضغط \"إنشاء قاعدة البيانات\". يمكنك اختبار الاتصال أولاً.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // actionsLayout
            // 
            this.actionsLayout.ColumnCount = 3;
            this.actionsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.actionsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.actionsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.actionsLayout.Controls.Add(this.btnCancel, 0, 0);
            this.actionsLayout.Controls.Add(this.btnTest, 1, 0);
            this.actionsLayout.Controls.Add(this.btnCreate, 2, 0);
            this.actionsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionsLayout.Location = new System.Drawing.Point(0, 558);
            this.actionsLayout.Margin = new System.Windows.Forms.Padding(0);
            this.actionsLayout.Name = "actionsLayout";
            this.actionsLayout.RowCount = 1;
            this.actionsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.actionsLayout.Size = new System.Drawing.Size(542, 64);
            this.actionsLayout.TabIndex = 18;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(230)))), ((int)(((byte)(235)))));
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(91)))), ((int)(((byte)(105)))));
            this.btnCancel.Location = new System.Drawing.Point(415, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 64);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "إلغاء";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(78)))), ((int)(((byte)(190)))));
            this.btnTest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(115)))), ((int)(((byte)(239)))));
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.Location = new System.Drawing.Point(196, 0);
            this.btnTest.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(206, 64);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "اختبار الاتصال";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.btnCreate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(111)))), ((int)(((byte)(88)))));
            this.btnCreate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(157)))), ((int)(((byte)(123)))));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.Location = new System.Drawing.Point(8, 0);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(2, 0, 6, 0);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(180, 64);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "إنشاء قاعدة البيانات";
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // DatabaseSetupForm
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1040, 860);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1040, 860);
            this.Name = "DatabaseSetupForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إعداد قاعدة البيانات";
            this.panelBackground.ResumeLayout(false);
            this.layoutRoot.ResumeLayout(false);
            this.panelCard.ResumeLayout(false);
            this.cardLayout.ResumeLayout(false);
            this.pnlServer.ResumeLayout(false);
            this.pnlServer.PerformLayout();
            this.pnlDb.ResumeLayout(false);
            this.pnlDb.PerformLayout();
            this.pnlAuth.ResumeLayout(false);
            this.pnlAuth.PerformLayout();
            this.pnlSqlUser.ResumeLayout(false);
            this.pnlSqlUser.PerformLayout();
            this.pnlSqlPass.ResumeLayout(false);
            this.pnlSqlPass.PerformLayout();
            this.pnlBackupDir.ResumeLayout(false);
            this.pnlBackupDir.PerformLayout();
            this.statusLine.ResumeLayout(false);
            this.actionsLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // Controls
        private Panel panelBackground;
        private TableLayoutPanel layoutRoot;
        private Panel panelCard;
        private TableLayoutPanel cardLayout;

        private Label lblIcon;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel sepTop;

        private Label lblServerLabel;
        private TextBox txtServer;
        private Panel pnlServer;
        private Label lblServerIcon;

        private Label lblDbLabel;
        private TextBox txtDatabase;
        private Panel pnlDb;
        private Label lblDbIcon;

        private Label lblAuthLabel;
        private TableLayoutPanel pnlAuth;
        private RadioButton rbWindows;
        private RadioButton rbSql;

        private Label lblSqlUser;
        private TextBox txtSqlUser;
        private Panel pnlSqlUser;

        private Label lblSqlPass;
        private TextBox txtSqlPass;
        private Button btnTogglePassword;
        private Panel pnlSqlPass;

        private Label lblBackupDir;
        private TextBox txtBackupDir;
        private Button btnBrowseBackup;
        private Panel pnlBackupDir;

        private Panel sepMid;
        private TableLayoutPanel statusLine;
        private Label lblStatus;

        private TableLayoutPanel actionsLayout;
        private Button btnTest;
        private Button btnCreate;
        private Button btnCancel;
    }
}
