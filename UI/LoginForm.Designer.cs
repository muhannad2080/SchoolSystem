// LoginForm.Designer.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolSystem.UI
{
    partial class LoginForm
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
            this.components = new System.ComponentModel.Container();

            this.panelBackground = new Panel();
            this.layoutRoot = new TableLayoutPanel();

            this.panelBrand = new Panel();
            this.brandLayout = new TableLayoutPanel();
            this.brandTop = new TableLayoutPanel();
            this.lblBrandMark = new Label();
            this.lblBrandName = new Label();
            this.lblBrandTagline = new Label();
            this.brandFeatureLayout = new TableLayoutPanel();
            this.lblFeatureTitle = new Label();
            this.lblFeature1 = new Label();
            this.lblFeature2 = new Label();
            this.lblFeature3 = new Label();
            this.lblBrandFooter = new Label();

            this.panelLoginArea = new Panel();
            this.loginAreaLayout = new TableLayoutPanel();
            this.btnWindowClose = new Button();
            this.loginCard = new Panel();
            this.cardLayout = new TableLayoutPanel();

            this.lblWelcome = new Label();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.sepTop = new Panel();

            this.lblUserLabel = new Label();
            this.pnlUser = new Panel();
            this.txtUserName = new TextBox();
            this.lblUserIcon = new Label();

            this.lblPasswordLabel = new Label();
            this.pnlPassword = new Panel();
            this.txtPassword = new TextBox();
            this.btnTogglePassword = new Button();
            this.lblPasswordIcon = new Label();

            this.pnlLogin = new Panel();
            this.btnLogin = new Button();

            this.bottomActions = new TableLayoutPanel();
            this.btnClear = new Button();
            this.btnExit = new Button();
            this.lblStatus = new Label();
            this.sepBottom = new Panel();
            this.lblFooter = new Label();
            this.lblDevelopers = new Label();

            this.toolTip = new ToolTip(this.components);

            this.panelBackground.SuspendLayout();
            this.layoutRoot.SuspendLayout();
            this.panelBrand.SuspendLayout();
            this.brandLayout.SuspendLayout();
            this.brandTop.SuspendLayout();
            this.brandFeatureLayout.SuspendLayout();
            this.panelLoginArea.SuspendLayout();
            this.loginAreaLayout.SuspendLayout();
            this.loginCard.SuspendLayout();
            this.cardLayout.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlPassword.SuspendLayout();
            this.pnlLogin.SuspendLayout();
            this.bottomActions.SuspendLayout();
            this.SuspendLayout();

            // panelBackground
            this.panelBackground.BackColor = Color.FromArgb(248, 250, 252);
            this.panelBackground.Controls.Add(this.layoutRoot);
            this.panelBackground.Controls.Add(this.btnWindowClose);
            this.panelBackground.Dock = DockStyle.Fill;
            this.panelBackground.Location = new Point(0, 0);
            this.panelBackground.Margin = new Padding(0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new Size(1180, 720);
            this.panelBackground.TabIndex = 0;

            // layoutRoot
            this.layoutRoot.ColumnCount = 2;
            this.layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            this.layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            this.layoutRoot.Controls.Add(this.panelBrand, 0, 0);
            this.layoutRoot.Controls.Add(this.panelLoginArea, 1, 0);
            this.layoutRoot.Dock = DockStyle.Fill;
            this.layoutRoot.Location = new Point(0, 0);
            this.layoutRoot.Margin = new Padding(0);
            this.layoutRoot.Name = "layoutRoot";
            // ترتيب الأعمدة عربي RTL: الهوية البصرية في اليمين، ونموذج الدخول في اليسار.
            this.layoutRoot.RightToLeft = RightToLeft.Yes;
            this.layoutRoot.RowCount = 1;
            this.layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.layoutRoot.Size = new Size(1180, 720);
            this.layoutRoot.TabIndex = 0;

            // panelBrand
            this.panelBrand.BackColor = Color.FromArgb(15, 23, 42);
            this.panelBrand.Controls.Add(this.brandLayout);
            this.panelBrand.Dock = DockStyle.Fill;
            this.panelBrand.Location = new Point(0, 0);
            this.panelBrand.Margin = new Padding(0);
            this.panelBrand.Name = "panelBrand";
            this.panelBrand.Padding = new Padding(54, 42, 54, 32);
            this.panelBrand.RightToLeft = RightToLeft.Yes;
            this.panelBrand.Size = new Size(531, 720);
            this.panelBrand.TabIndex = 0;

            // brandLayout
            this.brandLayout.ColumnCount = 1;
            this.brandLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.brandLayout.BackColor = Color.Transparent;
            this.brandLayout.Controls.Add(this.brandTop, 0, 0);
            this.brandLayout.Controls.Add(this.brandFeatureLayout, 0, 2);
            this.brandLayout.Controls.Add(this.lblBrandFooter, 0, 4);
            this.brandLayout.Dock = DockStyle.Fill;
            this.brandLayout.Location = new Point(54, 42);
            this.brandLayout.Margin = new Padding(0);
            this.brandLayout.Name = "brandLayout";
            this.brandLayout.RowCount = 5;
            this.brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            this.brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            this.brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 206F));
            this.brandLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            this.brandLayout.Size = new Size(423, 646);
            this.brandLayout.TabIndex = 0;

            // brandTop
            this.brandTop.ColumnCount = 1;
            this.brandTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.brandTop.BackColor = Color.Transparent;
            this.brandTop.Controls.Add(this.lblBrandMark, 0, 0);
            this.brandTop.Controls.Add(this.lblBrandName, 0, 1);
            this.brandTop.Controls.Add(this.lblBrandTagline, 0, 2);
            this.brandTop.Dock = DockStyle.Fill;
            this.brandTop.Location = new Point(0, 0);
            this.brandTop.Margin = new Padding(0);
            this.brandTop.Name = "brandTop";
            this.brandTop.RightToLeft = RightToLeft.Yes;
            this.brandTop.RowCount = 3;
            this.brandTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            this.brandTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            this.brandTop.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.brandTop.Size = new Size(415, 204);
            this.brandTop.TabIndex = 0;

            // lblBrandMark
            this.lblBrandMark.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblBrandMark.BackColor = Color.FromArgb(13, 148, 136);
            this.lblBrandMark.Font = new Font("Tahoma", 30F, FontStyle.Bold);
            this.lblBrandMark.ForeColor = Color.White;
            this.lblBrandMark.Location = new Point(327, 0);
            this.lblBrandMark.Margin = new Padding(0, 0, 0, 14);
            this.lblBrandMark.Name = "lblBrandMark";
            this.lblBrandMark.RightToLeft = RightToLeft.Yes;
            this.lblBrandMark.Size = new Size(96, 96);
            this.lblBrandMark.TabIndex = 0;
            this.lblBrandMark.Text = "م";
            this.lblBrandMark.TextAlign = ContentAlignment.MiddleCenter;

            // lblBrandName
            this.lblBrandName.Dock = DockStyle.Fill;
            this.lblBrandName.Font = new Font("Tahoma", 19F, FontStyle.Bold);
            this.lblBrandName.ForeColor = Color.White;
            this.lblBrandName.Location = new Point(0, 96);
            this.lblBrandName.Margin = new Padding(0);
            this.lblBrandName.Name = "lblBrandName";
            this.lblBrandName.RightToLeft = RightToLeft.Yes;
            this.lblBrandName.Size = new Size(423, 56);
            this.lblBrandName.TabIndex = 1;
            this.lblBrandName.Text = "نظام إدارة المدرسة";
            this.lblBrandName.TextAlign = ContentAlignment.MiddleRight;

            // lblBrandTagline
            this.lblBrandTagline.Dock = DockStyle.Fill;
            this.lblBrandTagline.Font = new Font("Tahoma", 10.5F);
            this.lblBrandTagline.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblBrandTagline.Location = new Point(0, 152);
            this.lblBrandTagline.Margin = new Padding(0);
            this.lblBrandTagline.Name = "lblBrandTagline";
            this.lblBrandTagline.RightToLeft = RightToLeft.Yes;
            this.lblBrandTagline.Size = new Size(423, 68);
            this.lblBrandTagline.TabIndex = 2;
            this.lblBrandTagline.Text = "منصة مدرسية موحّدة لإدارة الطلاب والمعلمين والحضور والرسوم والعمليات اليومية";
            this.lblBrandTagline.TextAlign = ContentAlignment.TopRight;

            // brandFeatureLayout
            this.brandFeatureLayout.ColumnCount = 1;
            this.brandFeatureLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.brandFeatureLayout.BackColor = Color.Transparent;
            this.brandFeatureLayout.Controls.Add(this.lblFeatureTitle, 0, 0);
            this.brandFeatureLayout.Controls.Add(this.lblFeature1, 0, 1);
            this.brandFeatureLayout.Controls.Add(this.lblFeature2, 0, 2);
            this.brandFeatureLayout.Controls.Add(this.lblFeature3, 0, 3);
            this.brandFeatureLayout.Dock = DockStyle.Fill;
            this.brandFeatureLayout.Location = new Point(0, 238);
            this.brandFeatureLayout.Margin = new Padding(0);
            this.brandFeatureLayout.Name = "brandFeatureLayout";
            this.brandFeatureLayout.RightToLeft = RightToLeft.Yes;
            this.brandFeatureLayout.RowCount = 4;
            this.brandFeatureLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            this.brandFeatureLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            this.brandFeatureLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            this.brandFeatureLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            this.brandFeatureLayout.Size = new Size(423, 206);
            this.brandFeatureLayout.TabIndex = 1;

            // feature title
            this.lblFeatureTitle.Dock = DockStyle.Fill;
            this.lblFeatureTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            this.lblFeatureTitle.ForeColor = Color.FromArgb(94, 234, 212);
            this.lblFeatureTitle.Location = new Point(0, 0);
            this.lblFeatureTitle.Margin = new Padding(0, 0, 0, 8);
            this.lblFeatureTitle.Name = "lblFeatureTitle";
            this.lblFeatureTitle.RightToLeft = RightToLeft.Yes;
            this.lblFeatureTitle.Size = new Size(415, 44);
            this.lblFeatureTitle.TabIndex = 0;
            this.lblFeatureTitle.Text = "إدارة مدرسية واضحة ومتكاملة";
            this.lblFeatureTitle.TextAlign = ContentAlignment.MiddleRight;

            // feature 1
            this.lblFeature1.Dock = DockStyle.Fill;
            this.lblFeature1.Font = new Font("Tahoma", 10F);
            this.lblFeature1.ForeColor = Color.FromArgb(226, 232, 240);
            this.lblFeature1.Location = new Point(0, 44);
            this.lblFeature1.Margin = new Padding(0);
            this.lblFeature1.Name = "lblFeature1";
            this.lblFeature1.RightToLeft = RightToLeft.Yes;
            this.lblFeature1.Size = new Size(415, 48);
            this.lblFeature1.TabIndex = 1;
            this.lblFeature1.Text = "✓  بيانات مترابطة وسهلة الوصول";
            this.lblFeature1.TextAlign = ContentAlignment.MiddleRight;

            // feature 2
            this.lblFeature2.Dock = DockStyle.Fill;
            this.lblFeature2.Font = new Font("Tahoma", 10F);
            this.lblFeature2.ForeColor = Color.FromArgb(226, 232, 240);
            this.lblFeature2.Location = new Point(0, 92);
            this.lblFeature2.Margin = new Padding(0);
            this.lblFeature2.Name = "lblFeature2";
            this.lblFeature2.RightToLeft = RightToLeft.Yes;
            this.lblFeature2.Size = new Size(415, 48);
            this.lblFeature2.TabIndex = 2;
            this.lblFeature2.Text = "✓  متابعة أكاديمية ومالية في واجهة واحدة";
            this.lblFeature2.TextAlign = ContentAlignment.MiddleRight;

            // feature 3
            this.lblFeature3.Dock = DockStyle.Fill;
            this.lblFeature3.Font = new Font("Tahoma", 10F);
            this.lblFeature3.ForeColor = Color.FromArgb(226, 232, 240);
            this.lblFeature3.Location = new Point(0, 140);
            this.lblFeature3.Margin = new Padding(0);
            this.lblFeature3.Name = "lblFeature3";
            this.lblFeature3.RightToLeft = RightToLeft.Yes;
            this.lblFeature3.Size = new Size(415, 48);
            this.lblFeature3.TabIndex = 3;
            this.lblFeature3.Text = "✓  صلاحيات مستخدمين وسجل للعمليات";
            this.lblFeature3.TextAlign = ContentAlignment.MiddleRight;

            // brand footer
            this.lblBrandFooter.Dock = DockStyle.Fill;
            this.lblBrandFooter.Font = new Font("Tahoma", 8.5F);
            this.lblBrandFooter.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblBrandFooter.Location = new Point(0, 594);
            this.lblBrandFooter.Margin = new Padding(0);
            this.lblBrandFooter.Name = "lblBrandFooter";
            this.lblBrandFooter.RightToLeft = RightToLeft.Yes;
            this.lblBrandFooter.Size = new Size(415, 42);
            this.lblBrandFooter.TabIndex = 2;
            this.lblBrandFooter.Text = "بوابة تشغيل يومية لإدارة المدرسة";
            this.lblBrandFooter.TextAlign = ContentAlignment.MiddleRight;

            // panelLoginArea
            this.panelLoginArea.BackColor = Color.FromArgb(248, 250, 252);
            this.panelLoginArea.Controls.Add(this.loginAreaLayout);
            this.panelLoginArea.Dock = DockStyle.Fill;
            this.panelLoginArea.Location = new Point(531, 0);
            this.panelLoginArea.Margin = new Padding(0);
            this.panelLoginArea.Name = "panelLoginArea";
            this.panelLoginArea.Padding = new Padding(40, 22, 40, 28);
            this.panelLoginArea.RightToLeft = RightToLeft.Yes;
            this.panelLoginArea.Size = new Size(649, 720);
            this.panelLoginArea.TabIndex = 1;

            // loginAreaLayout
            this.loginAreaLayout.ColumnCount = 1;
            this.loginAreaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.loginAreaLayout.RowCount = 1;
            this.loginAreaLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.loginAreaLayout.BackColor = Color.Transparent;
            this.loginAreaLayout.Controls.Add(this.loginCard, 0, 0);
            this.loginAreaLayout.Dock = DockStyle.Fill;
            this.loginAreaLayout.Location = new Point(48, 24);
            this.loginAreaLayout.Margin = new Padding(0);
            this.loginAreaLayout.Name = "loginAreaLayout";
            this.loginAreaLayout.RightToLeft = RightToLeft.Yes;
            this.loginAreaLayout.Size = new Size(553, 666);
            this.loginAreaLayout.TabIndex = 0;

            // btnWindowClose
            this.btnWindowClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnWindowClose.BackColor = Color.Transparent;
            this.btnWindowClose.Cursor = Cursors.Hand;
            this.btnWindowClose.FlatAppearance.BorderSize = 0;
            this.btnWindowClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            this.btnWindowClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            this.btnWindowClose.FlatStyle = FlatStyle.Flat;
            this.btnWindowClose.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            this.btnWindowClose.ForeColor = Color.FromArgb(100, 116, 139);
            this.btnWindowClose.Location = new Point(1128, 18);
            this.btnWindowClose.Margin = new Padding(0);
            this.btnWindowClose.Name = "btnWindowClose";
            this.btnWindowClose.RightToLeft = RightToLeft.No;
            this.btnWindowClose.Size = new Size(40, 36);
            this.btnWindowClose.TabIndex = 6;
            this.btnWindowClose.Text = "×";
            this.btnWindowClose.UseVisualStyleBackColor = false;
            this.btnWindowClose.Click += new EventHandler(this.btnWindowClose_Click);

            // loginCard
            this.loginCard.Anchor = AnchorStyles.None;
            this.loginCard.BackColor = Color.White;
            this.loginCard.BorderStyle = BorderStyle.FixedSingle;
            this.loginCard.Controls.Add(this.cardLayout);
            this.loginCard.Location = new Point(32, 38);
            this.loginCard.Margin = new Padding(0);
            this.loginCard.Name = "loginCard";
            this.loginCard.Padding = new Padding(32, 28, 32, 22);
            this.loginCard.RightToLeft = RightToLeft.Yes;
            this.loginCard.Size = new Size(480, 590);
            this.loginCard.TabIndex = 1;

            // cardLayout
            this.cardLayout.ColumnCount = 1;
            this.cardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.cardLayout.RowCount = 16;
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 14F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 14F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 2F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            this.cardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

            this.cardLayout.BackColor = Color.White;
            this.cardLayout.Controls.Add(this.lblWelcome, 0, 0);
            this.cardLayout.Controls.Add(this.lblTitle, 0, 1);
            this.cardLayout.Controls.Add(this.lblSubtitle, 0, 2);
            this.cardLayout.Controls.Add(this.sepTop, 0, 3);
            this.cardLayout.Controls.Add(this.lblUserLabel, 0, 4);
            this.cardLayout.Controls.Add(this.pnlUser, 0, 5);
            this.cardLayout.Controls.Add(this.lblPasswordLabel, 0, 7);
            this.cardLayout.Controls.Add(this.pnlPassword, 0, 8);
            this.cardLayout.Controls.Add(this.pnlLogin, 0, 10);
            this.cardLayout.Controls.Add(this.bottomActions, 0, 11);
            this.cardLayout.Controls.Add(this.lblStatus, 0, 12);
            this.cardLayout.Controls.Add(this.sepBottom, 0, 13);
            this.cardLayout.Controls.Add(this.lblFooter, 0, 14);
            this.cardLayout.Controls.Add(this.lblDevelopers, 0, 15);
            this.cardLayout.Dock = DockStyle.Fill;
            this.cardLayout.Location = new Point(30, 24);
            this.cardLayout.Margin = new Padding(0);
            this.cardLayout.Name = "cardLayout";
            this.cardLayout.RightToLeft = RightToLeft.Yes;
            this.cardLayout.Size = new Size(398, 546);
            this.cardLayout.TabIndex = 0;

            // lblWelcome
            this.lblWelcome.Dock = DockStyle.Fill;
            this.lblWelcome.Font = new Font("Tahoma", 10.5F, FontStyle.Bold);
            this.lblWelcome.ForeColor = Color.FromArgb(13, 148, 136);
            this.lblWelcome.Location = new Point(3, 0);
            this.lblWelcome.Margin = new Padding(3, 0, 3, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.RightToLeft = RightToLeft.Yes;
            this.lblWelcome.Size = new Size(392, 38);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "مرحبًا بك";
            this.lblWelcome.TextAlign = ContentAlignment.BottomRight;

            // lblTitle
            this.lblTitle.Dock = DockStyle.Fill;
            this.lblTitle.Font = new Font("Tahoma", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblTitle.Location = new Point(3, 38);
            this.lblTitle.Margin = new Padding(3, 0, 3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.RightToLeft = RightToLeft.Yes;
            this.lblTitle.Size = new Size(392, 40);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "تسجيل الدخول إلى النظام";
            this.lblTitle.TextAlign = ContentAlignment.MiddleRight;

            // lblSubtitle
            this.lblSubtitle.Dock = DockStyle.Fill;
            this.lblSubtitle.Font = new Font("Tahoma", 9.75F);
            this.lblSubtitle.ForeColor = Color.FromArgb(71, 85, 105);
            this.lblSubtitle.Location = new Point(3, 78);
            this.lblSubtitle.Margin = new Padding(3, 0, 3, 0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.RightToLeft = RightToLeft.Yes;
            this.lblSubtitle.Size = new Size(392, 50);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "أدخل بيانات حسابك المصرّح له بالوصول إلى نظام إدارة المدرسة";
            this.lblSubtitle.TextAlign = ContentAlignment.MiddleRight;

            // sepTop
            this.sepTop.BackColor = Color.FromArgb(226, 232, 240);
            this.sepTop.Dock = DockStyle.Bottom;
            this.sepTop.Location = new Point(0, 79);
            this.sepTop.Margin = new Padding(0);
            this.sepTop.Name = "sepTop";
            this.sepTop.Size = new Size(398, 1);
            this.sepTop.TabIndex = 3;

            // lblUserLabel
            this.lblUserLabel.Dock = DockStyle.Fill;
            this.lblUserLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            this.lblUserLabel.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblUserLabel.Location = new Point(3, 144);
            this.lblUserLabel.Margin = new Padding(3, 0, 3, 0);
            this.lblUserLabel.Name = "lblUserLabel";
            this.lblUserLabel.RightToLeft = RightToLeft.Yes;
            this.lblUserLabel.Size = new Size(392, 24);
            this.lblUserLabel.TabIndex = 4;
            this.lblUserLabel.Text = "اسم المستخدم";
            this.lblUserLabel.TextAlign = ContentAlignment.MiddleRight;

            // pnlUser
            this.pnlUser.BackColor = Color.FromArgb(248, 250, 252);
            this.pnlUser.BorderStyle = BorderStyle.FixedSingle;
            this.pnlUser.Controls.Add(this.txtUserName);
            this.pnlUser.Controls.Add(this.lblUserIcon);
            this.pnlUser.Dock = DockStyle.Fill;
            this.pnlUser.Location = new Point(0, 168);
            this.pnlUser.Margin = new Padding(0);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Padding = new Padding(12, 5, 12, 5);
            this.pnlUser.RightToLeft = RightToLeft.Yes;
            this.pnlUser.Size = new Size(416, 52);
            this.pnlUser.TabIndex = 5;

            // txtUserName
            this.txtUserName.BackColor = Color.FromArgb(248, 250, 252);
            this.txtUserName.BorderStyle = BorderStyle.None;
            this.txtUserName.Dock = DockStyle.Fill;
            this.txtUserName.Font = new Font("Tahoma", 10.5F);
            this.txtUserName.ForeColor = Color.FromArgb(148, 163, 184);
            this.txtUserName.Location = new Point(12, 5);
            this.txtUserName.Margin = new Padding(0);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.RightToLeft = RightToLeft.Yes;
            this.txtUserName.Size = new Size(334, 26);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.Text = "اسم المستخدم";
            this.txtUserName.TextAlign = HorizontalAlignment.Right;
            this.txtUserName.Enter += new EventHandler(this.txtUserName_Enter);
            this.txtUserName.Leave += new EventHandler(this.txtUserName_Leave);

            // lblUserIcon
            this.lblUserIcon.Dock = DockStyle.Right;
            this.lblUserIcon.Font = new Font("Segoe MDL2 Assets", 14F);
            this.lblUserIcon.ForeColor = Color.FromArgb(13, 148, 136);
            this.lblUserIcon.Location = new Point(346, 5);
            this.lblUserIcon.Margin = new Padding(0);
            this.lblUserIcon.Name = "lblUserIcon";
            this.lblUserIcon.Size = new Size(38, 36);
            this.lblUserIcon.TabIndex = 1;
            this.lblUserIcon.Text = "";
            this.lblUserIcon.TextAlign = ContentAlignment.MiddleCenter;

            // lblPasswordLabel
            this.lblPasswordLabel.Dock = DockStyle.Fill;
            this.lblPasswordLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            this.lblPasswordLabel.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblPasswordLabel.Location = new Point(3, 232);
            this.lblPasswordLabel.Margin = new Padding(3, 0, 3, 0);
            this.lblPasswordLabel.Name = "lblPasswordLabel";
            this.lblPasswordLabel.RightToLeft = RightToLeft.Yes;
            this.lblPasswordLabel.Size = new Size(392, 24);
            this.lblPasswordLabel.TabIndex = 6;
            this.lblPasswordLabel.Text = "كلمة المرور";
            this.lblPasswordLabel.TextAlign = ContentAlignment.MiddleRight;

            // pnlPassword
            this.pnlPassword.BackColor = Color.FromArgb(248, 250, 252);
            this.pnlPassword.BorderStyle = BorderStyle.FixedSingle;
            this.pnlPassword.Controls.Add(this.txtPassword);
            this.pnlPassword.Controls.Add(this.btnTogglePassword);
            this.pnlPassword.Controls.Add(this.lblPasswordIcon);
            this.pnlPassword.Dock = DockStyle.Fill;
            this.pnlPassword.Location = new Point(0, 256);
            this.pnlPassword.Margin = new Padding(0);
            this.pnlPassword.Name = "pnlPassword";
            this.pnlPassword.Padding = new Padding(12, 5, 12, 5);
            this.pnlPassword.RightToLeft = RightToLeft.Yes;
            this.pnlPassword.Size = new Size(416, 52);
            this.pnlPassword.TabIndex = 7;

            // txtPassword
            this.txtPassword.BackColor = Color.FromArgb(248, 250, 252);
            this.txtPassword.BorderStyle = BorderStyle.None;
            this.txtPassword.Dock = DockStyle.Fill;
            this.txtPassword.Font = new Font("Tahoma", 10.5F);
            this.txtPassword.ForeColor = Color.FromArgb(148, 163, 184);
            this.txtPassword.Location = new Point(52, 5);
            this.txtPassword.Margin = new Padding(0);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.RightToLeft = RightToLeft.Yes;
            this.txtPassword.Size = new Size(294, 26);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "كلمة المرور";
            this.txtPassword.TextAlign = HorizontalAlignment.Right;
            this.txtPassword.Enter += new EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new EventHandler(this.txtPassword_Leave);

            // btnTogglePassword
            this.btnTogglePassword.BackColor = Color.Transparent;
            this.btnTogglePassword.Cursor = Cursors.Hand;
            this.btnTogglePassword.Dock = DockStyle.Left;
            this.btnTogglePassword.FlatAppearance.BorderSize = 0;
            this.btnTogglePassword.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 232, 240);
            this.btnTogglePassword.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
            this.btnTogglePassword.FlatStyle = FlatStyle.Flat;
            this.btnTogglePassword.Font = new Font("Tahoma", 8.5F, FontStyle.Bold);
            this.btnTogglePassword.ForeColor = Color.FromArgb(71, 85, 105);
            this.btnTogglePassword.Location = new Point(12, 5);
            this.btnTogglePassword.Margin = new Padding(0);
            this.btnTogglePassword.Name = "btnTogglePassword";
            this.btnTogglePassword.RightToLeft = RightToLeft.Yes;
            this.btnTogglePassword.Size = new Size(92, 36);
            this.btnTogglePassword.TabIndex = 2;
            this.btnTogglePassword.Text = "إظهار";
            this.btnTogglePassword.UseVisualStyleBackColor = false;
            this.btnTogglePassword.Click += new EventHandler(this.btnTogglePassword_Click);

            // lblPasswordIcon
            this.lblPasswordIcon.Dock = DockStyle.Right;
            this.lblPasswordIcon.Font = new Font("Segoe MDL2 Assets", 14F);
            this.lblPasswordIcon.ForeColor = Color.FromArgb(13, 148, 136);
            this.lblPasswordIcon.Location = new Point(346, 5);
            this.lblPasswordIcon.Margin = new Padding(0);
            this.lblPasswordIcon.Name = "lblPasswordIcon";
            this.lblPasswordIcon.Size = new Size(38, 36);
            this.lblPasswordIcon.TabIndex = 0;
            this.lblPasswordIcon.Text = "";
            this.lblPasswordIcon.TextAlign = ContentAlignment.MiddleCenter;

            // pnlLogin
            this.pnlLogin.Dock = DockStyle.Fill;
            this.pnlLogin.Location = new Point(0, 320);
            this.pnlLogin.Margin = new Padding(0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Padding = new Padding(0, 2, 0, 0);
            this.pnlLogin.Size = new Size(416, 54);
            this.pnlLogin.TabIndex = 8;
            this.pnlLogin.Controls.Add(this.btnLogin);

            // btnLogin
            this.btnLogin.BackColor = Color.FromArgb(13, 148, 136);
            this.btnLogin.Cursor = Cursors.Hand;
            this.btnLogin.Dock = DockStyle.Fill;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(15, 118, 110);
            this.btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 184, 166);
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.Location = new Point(0, 2);
            this.btnLogin.Margin = new Padding(0);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.RightToLeft = RightToLeft.Yes;
            this.btnLogin.Size = new Size(416, 52);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "تسجيل الدخول";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);

            // bottomActions
            this.bottomActions.ColumnCount = 2;
            this.bottomActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.bottomActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.bottomActions.Controls.Add(this.btnClear, 0, 0);
            this.bottomActions.Controls.Add(this.btnExit, 1, 0);
            this.bottomActions.Dock = DockStyle.Fill;
            this.bottomActions.Location = new Point(0, 372);
            this.bottomActions.Margin = new Padding(0);
            this.bottomActions.Name = "bottomActions";
            this.bottomActions.RightToLeft = RightToLeft.Yes;
            this.bottomActions.RowCount = 1;
            this.bottomActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.bottomActions.Size = new Size(416, 42);
            this.bottomActions.TabIndex = 9;

            // btnClear
            this.btnClear.Anchor = AnchorStyles.Left;
            this.btnClear.BackColor = Color.White;
            this.btnClear.Cursor = Cursors.Hand;
            this.btnClear.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            this.btnClear.FlatAppearance.BorderSize = 1;
            this.btnClear.FlatAppearance.MouseDownBackColor = Color.FromArgb(241, 245, 249);
            this.btnClear.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 250, 252);
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Tahoma", 9F);
            this.btnClear.ForeColor = Color.FromArgb(71, 85, 105);
            this.btnClear.Location = new Point(6, 4);
            this.btnClear.Margin = new Padding(0, 0, 6, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.RightToLeft = RightToLeft.Yes;
            this.btnClear.Size = new Size(112, 36);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "مسح الحقول";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            // btnExit
            this.btnExit.Anchor = AnchorStyles.Right;
            this.btnExit.BackColor = Color.White;
            this.btnExit.Cursor = Cursors.Hand;
            this.btnExit.DialogResult = DialogResult.None;
            this.btnExit.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            this.btnExit.FlatAppearance.BorderSize = 1;
            this.btnExit.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            this.btnExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            this.btnExit.FlatStyle = FlatStyle.Flat;
            this.btnExit.Font = new Font("Tahoma", 9F);
            this.btnExit.ForeColor = Color.FromArgb(185, 28, 28);
            this.btnExit.Location = new Point(284, 4);
            this.btnExit.Margin = new Padding(6, 0, 0, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.RightToLeft = RightToLeft.Yes;
            this.btnExit.Size = new Size(112, 36);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "خروج";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new EventHandler(this.btnExit_Click);

            // lblStatus
            this.lblStatus.Dock = DockStyle.Fill;
            this.lblStatus.Font = new Font("Tahoma", 8.5F);
            this.lblStatus.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblStatus.Location = new Point(3, 412);
            this.lblStatus.Margin = new Padding(3, 0, 3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeft = RightToLeft.Yes;
            this.lblStatus.Size = new Size(392, 26);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "الحالة: جاهز لتسجيل الدخول";
            this.lblStatus.TextAlign = ContentAlignment.MiddleRight;

            // sepBottom
            this.sepBottom.BackColor = Color.FromArgb(226, 232, 240);
            this.sepBottom.Dock = DockStyle.Fill;
            this.sepBottom.Location = new Point(0, 438);
            this.sepBottom.Margin = new Padding(0);
            this.sepBottom.Name = "sepBottom";
            this.sepBottom.Size = new Size(398, 1);
            this.sepBottom.TabIndex = 11;

            // lblFooter
            this.lblFooter.Dock = DockStyle.Fill;
            this.lblFooter.Font = new Font("Tahoma", 8F);
            this.lblFooter.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblFooter.Location = new Point(3, 439);
            this.lblFooter.Margin = new Padding(3, 0, 3, 0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.RightToLeft = RightToLeft.Yes;
            this.lblFooter.Size = new Size(392, 30);
            this.lblFooter.TabIndex = 12;
            this.lblFooter.Text = "الوصول إلى النظام متاح للحسابات المصرّح لها فقط";
            this.lblFooter.TextAlign = ContentAlignment.MiddleRight;

            // lblDevelopers
            this.lblDevelopers.Dock = DockStyle.Fill;
            this.lblDevelopers.Font = new Font("Tahoma", 8F, FontStyle.Regular);
            this.lblDevelopers.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblDevelopers.Location = new Point(3, 469);
            this.lblDevelopers.Margin = new Padding(3, 0, 3, 0);
            this.lblDevelopers.Name = "lblDevelopers";
            this.lblDevelopers.RightToLeft = RightToLeft.Yes;
            this.lblDevelopers.Size = new Size(392, 40);
            this.lblDevelopers.AutoSize = false;
            this.lblDevelopers.TabIndex = 13;
            this.lblDevelopers.Text = "المطورون\r\nالمهندس مهند علي علي علي سعد • عبد الرحمن سعد علي سعد • سامي الحاج";
            this.lblDevelopers.TextAlign = ContentAlignment.MiddleCenter;

            // ToolTip
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 400;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            this.toolTip.SetToolTip(this.txtUserName, "أدخل اسم المستخدم المسجل في النظام");
            this.toolTip.SetToolTip(this.txtPassword, "أدخل كلمة المرور كما هي دون تعديل المسافات");
            this.toolTip.SetToolTip(this.btnTogglePassword, "إظهار أو إخفاء كلمة المرور");
            this.toolTip.SetToolTip(this.btnClear, "مسح بيانات الدخول وإعادة الحقول إلى حالتها الافتراضية");
            this.toolTip.SetToolTip(this.btnExit, "إغلاق نظام إدارة المدرسة");
            this.toolTip.SetToolTip(this.btnWindowClose, "إغلاق نافذة تسجيل الدخول");

            // LoginForm
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new SizeF(7F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.ClientSize = new Size(1180, 720);
            this.Controls.Add(this.panelBackground);
            this.Font = new Font("Tahoma", 10F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(1040, 680);
            this.Name = "LoginForm";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "تسجيل الدخول | نظام إدارة المدرسة";

            this.panelBackground.ResumeLayout(false);
            this.layoutRoot.ResumeLayout(false);
            this.panelBrand.ResumeLayout(false);
            this.brandLayout.ResumeLayout(false);
            this.brandTop.ResumeLayout(false);
            this.brandFeatureLayout.ResumeLayout(false);
            this.panelLoginArea.ResumeLayout(false);
            this.loginAreaLayout.ResumeLayout(false);
            this.loginCard.ResumeLayout(false);
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

        private Panel panelBackground;
        private TableLayoutPanel layoutRoot;

        private Panel panelBrand;
        private TableLayoutPanel brandLayout;
        private TableLayoutPanel brandTop;
        private Label lblBrandMark;
        private Label lblBrandName;
        private Label lblBrandTagline;
        private TableLayoutPanel brandFeatureLayout;
        private Label lblFeatureTitle;
        private Label lblFeature1;
        private Label lblFeature2;
        private Label lblFeature3;
        private Label lblBrandFooter;

        private Panel panelLoginArea;
        private TableLayoutPanel loginAreaLayout;
        private Button btnWindowClose;
        private Panel loginCard;
        private TableLayoutPanel cardLayout;

        private Label lblWelcome;
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
        private Label lblStatus;

        private Panel sepTop;
        private Panel sepBottom;
        private Label lblFooter;
        private Label lblDevelopers;
        private ToolTip toolTip;
    }
}