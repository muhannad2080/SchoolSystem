// LoginForm.Designer.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolSystem.UI
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #endregion

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
            this.lblUserLabel = new System.Windows.Forms.Label();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserIcon = new System.Windows.Forms.Label();
            this.lblPasswordLabel = new System.Windows.Forms.Label();
            this.pnlPassword = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnTogglePassword = new System.Windows.Forms.Button();
            this.lblPasswordIcon = new System.Windows.Forms.Label();
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.bottomActions = new System.Windows.Forms.TableLayoutPanel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.sepBottom = new System.Windows.Forms.Panel();
            this.lblFooter = new System.Windows.Forms.Label();
            this.panelBackground.SuspendLayout();
            this.layoutRoot.SuspendLayout();
            this.panelCard.SuspendLayout();
            this.cardLayout.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlPassword.SuspendLayout();
            this.pnlLogin.SuspendLayout();
            this.bottomActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelBackground.Controls.Add(this.layoutRoot);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(1000, 700);
            this.panelBackground.TabIndex = 0;
            // 
            // layoutRoot
            // 
            this.layoutRoot.BackColor = System.Drawing.Color.Transparent;
            this.layoutRoot.ColumnCount = 3;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.layoutRoot.Controls.Add(this.panelCard, 1, 1);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.RowCount = 3;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.layoutRoot.Size = new System.Drawing.Size(1000, 700);
            this.layoutRoot.TabIndex = 0;
            // 
            // panelCard
            // 
            this.panelCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelCard.BackColor = System.Drawing.Color.White;
            this.panelCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCard.Controls.Add(this.cardLayout);
            this.panelCard.Location = new System.Drawing.Point(220, 100);
            this.panelCard.Margin = new System.Windows.Forms.Padding(0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(36);
            this.panelCard.Size = new System.Drawing.Size(560, 500);
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
            this.cardLayout.Controls.Add(this.lblUserLabel, 0, 4);
            this.cardLayout.Controls.Add(this.pnlUser, 0, 5);
            this.cardLayout.Controls.Add(this.lblPasswordLabel, 0, 7);
            this.cardLayout.Controls.Add(this.pnlPassword, 0, 8);
            this.cardLayout.Controls.Add(this.pnlLogin, 0, 10);
            this.cardLayout.Controls.Add(this.bottomActions, 0, 12);
            this.cardLayout.Controls.Add(this.sepBottom, 0, 13);
            this.cardLayout.Controls.Add(this.lblFooter, 0, 14);
            this.cardLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardLayout.Location = new System.Drawing.Point(36, 36);
            this.cardLayout.Margin = new System.Windows.Forms.Padding(0);
            this.cardLayout.Name = "cardLayout";
            this.cardLayout.RowCount = 15;
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cardLayout.Size = new System.Drawing.Size(486, 426);
            this.cardLayout.TabIndex = 0;
            // 
            // lblIcon
            // 
            this.lblIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 28F);
            this.lblIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.lblIcon.Location = new System.Drawing.Point(3, 0);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(480, 60);
            this.lblIcon.TabIndex = 0;
            this.lblIcon.Text = "";
            this.lblIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.lblTitle.Location = new System.Drawing.Point(3, 60);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(480, 42);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "نظام إدارة المدرسة";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(118)))), ((int)(((byte)(133)))));
            this.lblSubtitle.Location = new System.Drawing.Point(3, 102);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(480, 30);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "أدخل بيانات الدخول للوصول إلى لوحة التحكم";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sepTop
            // 
            this.sepTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
            this.sepTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sepTop.Location = new System.Drawing.Point(0, 147);
            this.sepTop.Margin = new System.Windows.Forms.Padding(0);
            this.sepTop.Name = "sepTop";
            this.sepTop.Size = new System.Drawing.Size(486, 1);
            this.sepTop.TabIndex = 3;
            // 
            // lblUserLabel
            // 
            this.lblUserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUserLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(58)))), ((int)(((byte)(72)))));
            this.lblUserLabel.Location = new System.Drawing.Point(3, 148);
            this.lblUserLabel.Name = "lblUserLabel";
            this.lblUserLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblUserLabel.Size = new System.Drawing.Size(480, 24);
            this.lblUserLabel.TabIndex = 4;
            this.lblUserLabel.Text = "اسم المستخدم";
            this.lblUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlUser
            // 
            this.pnlUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUser.Controls.Add(this.txtUserName);
            this.pnlUser.Controls.Add(this.lblUserIcon);
            this.pnlUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUser.Location = new System.Drawing.Point(0, 172);
            this.pnlUser.Margin = new System.Windows.Forms.Padding(0);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.pnlUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlUser.Size = new System.Drawing.Size(486, 48);
            this.pnlUser.TabIndex = 5;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.txtUserName.Location = new System.Drawing.Point(10, 4);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(0);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtUserName.Size = new System.Drawing.Size(432, 25);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Text = "اسم المستخدم";
            this.txtUserName.Enter += new System.EventHandler(this.txtUserName_Enter);
            this.txtUserName.Leave += new System.EventHandler(this.txtUserName_Leave);
            // 
            // lblUserIcon
            // 
            this.lblUserIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblUserIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F);
            this.lblUserIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.lblUserIcon.Location = new System.Drawing.Point(442, 4);
            this.lblUserIcon.Name = "lblUserIcon";
            this.lblUserIcon.Size = new System.Drawing.Size(32, 38);
            this.lblUserIcon.TabIndex = 0;
            this.lblUserIcon.Text = "";
            this.lblUserIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPasswordLabel
            // 
            this.lblPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPasswordLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(58)))), ((int)(((byte)(72)))));
            this.lblPasswordLabel.Location = new System.Drawing.Point(3, 232);
            this.lblPasswordLabel.Name = "lblPasswordLabel";
            this.lblPasswordLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPasswordLabel.Size = new System.Drawing.Size(480, 24);
            this.lblPasswordLabel.TabIndex = 6;
            this.lblPasswordLabel.Text = "كلمة المرور";
            this.lblPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlPassword
            // 
            this.pnlPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPassword.Controls.Add(this.txtPassword);
            this.pnlPassword.Controls.Add(this.btnTogglePassword);
            this.pnlPassword.Controls.Add(this.lblPasswordIcon);
            this.pnlPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPassword.Location = new System.Drawing.Point(0, 256);
            this.pnlPassword.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPassword.Name = "pnlPassword";
            this.pnlPassword.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.pnlPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlPassword.Size = new System.Drawing.Size(486, 48);
            this.pnlPassword.TabIndex = 7;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.txtPassword.Location = new System.Drawing.Point(48, 4);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(0);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtPassword.Size = new System.Drawing.Size(394, 25);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "كلمة المرور";
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
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
            this.btnTogglePassword.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F);
            this.btnTogglePassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(103)))), ((int)(((byte)(118)))));
            this.btnTogglePassword.Location = new System.Drawing.Point(10, 4);
            this.btnTogglePassword.Margin = new System.Windows.Forms.Padding(0);
            this.btnTogglePassword.Name = "btnTogglePassword";
            this.btnTogglePassword.Size = new System.Drawing.Size(38, 38);
            this.btnTogglePassword.TabIndex = 2;
            this.btnTogglePassword.Text = "";
            this.btnTogglePassword.UseVisualStyleBackColor = false;
            this.btnTogglePassword.Click += new System.EventHandler(this.btnTogglePassword_Click);
            // 
            // lblPasswordIcon
            // 
            this.lblPasswordIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPasswordIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 16F);
            this.lblPasswordIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.lblPasswordIcon.Location = new System.Drawing.Point(442, 4);
            this.lblPasswordIcon.Name = "lblPasswordIcon";
            this.lblPasswordIcon.Size = new System.Drawing.Size(32, 38);
            this.lblPasswordIcon.TabIndex = 0;
            this.lblPasswordIcon.Text = "";
            this.lblPasswordIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLogin
            // 
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 320);
            this.pnlLogin.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(486, 52);
            this.pnlLogin.TabIndex = 8;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(138)))), ((int)(((byte)(109)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(111)))), ((int)(((byte)(88)))));
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(157)))), ((int)(((byte)(123)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(0, 0);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(486, 52);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "   تسجيل الدخول";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // bottomActions
            // 
            this.bottomActions.ColumnCount = 2;
            this.bottomActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomActions.Controls.Add(this.btnClear, 1, 0);
            this.bottomActions.Controls.Add(this.btnExit, 0, 0);
            this.bottomActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomActions.Location = new System.Drawing.Point(0, 384);
            this.bottomActions.Margin = new System.Windows.Forms.Padding(0);
            this.bottomActions.Name = "bottomActions";
            this.bottomActions.RowCount = 1;
            this.bottomActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomActions.Size = new System.Drawing.Size(486, 38);
            this.bottomActions.TabIndex = 9;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(230)))), ((int)(((byte)(235)))));
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(91)))), ((int)(((byte)(105)))));
            this.btnClear.Location = new System.Drawing.Point(0, 3);
            this.btnClear.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(110, 32);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "   مسح";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(230)))), ((int)(((byte)(235)))));
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(91)))), ((int)(((byte)(105)))));
            this.btnExit.Location = new System.Drawing.Point(376, 3);
            this.btnExit.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(110, 32);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "   إغلاق";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // sepBottom
            // 
            this.sepBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
            this.sepBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.sepBottom.Location = new System.Drawing.Point(0, 422);
            this.sepBottom.Margin = new System.Windows.Forms.Padding(0);
            this.sepBottom.Name = "sepBottom";
            this.sepBottom.Size = new System.Drawing.Size(486, 1);
            this.sepBottom.TabIndex = 10;
            // 
            // lblFooter
            // 
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(153)))), ((int)(((byte)(164)))));
            this.lblFooter.Location = new System.Drawing.Point(3, 436);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(480, 1);
            this.lblFooter.TabIndex = 11;
            this.lblFooter.Text = "يرجى التأكد من صحة بيانات الدخول قبل المتابعة";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(900, 640);
            this.Name = "LoginForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تسجيل الدخول";
            this.panelBackground.ResumeLayout(false);
            this.layoutRoot.ResumeLayout(false);
            this.panelCard.ResumeLayout(false);
            this.cardLayout.ResumeLayout(false);
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlPassword.ResumeLayout(false);
            this.pnlPassword.PerformLayout();
            this.pnlLogin.ResumeLayout(false);
            this.bottomActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // ============================================================
        // Controls
        // ============================================================

        private Panel panelBackground;
        private TableLayoutPanel layoutRoot;
        private Panel panelCard;
        private TableLayoutPanel cardLayout;

        private Label lblIcon;
        private Label lblTitle;
        private Label lblSubtitle;

        private Label lblUserLabel;
        private Label lblUserIcon;
        private TextBox txtUserName;
        private Panel pnlUser;

        private Label lblPasswordLabel;
        private Label lblPasswordIcon;
        private TextBox txtPassword;
        private Button btnTogglePassword;
        private Panel pnlPassword;

        private Panel pnlLogin;
        private Button btnLogin;

        private TableLayoutPanel bottomActions;
        private Button btnClear;
        private Button btnExit;

        private Panel sepTop;
        private Panel sepBottom;
        private Label lblFooter;
    }
}
