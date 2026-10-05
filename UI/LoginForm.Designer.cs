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
            // Initialize the WinForms component container before creating component-bound controls.
            this.components = new System.ComponentModel.Container();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.layoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.panelBrand = new System.Windows.Forms.Panel();
            this.brandLayout = new System.Windows.Forms.TableLayoutPanel();
            this.brandTop = new System.Windows.Forms.TableLayoutPanel();
            this.lblBrandMark = new System.Windows.Forms.Label();
            this.lblBrandName = new System.Windows.Forms.Label();
            this.lblBrandTagline = new System.Windows.Forms.Label();
            this.brandFeatureLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblFeatureTitle = new System.Windows.Forms.Label();
            this.lblFeature1 = new System.Windows.Forms.Label();
            this.lblFeature2 = new System.Windows.Forms.Label();
            this.lblFeature3 = new System.Windows.Forms.Label();
            this.lblBrandFooter = new System.Windows.Forms.Label();
            this.panelLoginArea = new System.Windows.Forms.Panel();
            this.loginAreaLayout = new System.Windows.Forms.TableLayoutPanel();
            this.loginCard = new System.Windows.Forms.Panel();
            this.cardLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblWelcome = new System.Windows.Forms.Label();
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.sepBottom = new System.Windows.Forms.Panel();
            this.lblFooter = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblDevelopers = new System.Windows.Forms.Label();
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
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.panelBackground.Controls.Add(this.layoutRoot);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Margin = new System.Windows.Forms.Padding(0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(1120, 720);
            this.panelBackground.TabIndex = 0;
            // 
            // layoutRoot
            // 
            this.layoutRoot.ColumnCount = 2;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57F));
            this.layoutRoot.Controls.Add(this.panelBrand, 0, 0);
            this.layoutRoot.Controls.Add(this.panelLoginArea, 1, 0);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.RowCount = 1;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Size = new System.Drawing.Size(1120, 720);
            this.layoutRoot.TabIndex = 0;
            // 
            // panelBrand
            // 
            this.panelBrand.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.panelBrand.Controls.Add(this.brandLayout);
            this.panelBrand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBrand.Location = new System.Drawing.Point(0, 0);
            this.panelBrand.Margin = new System.Windows.Forms.Padding(0);
            this.panelBrand.Name = "panelBrand";
            this.panelBrand.Padding = new System.Windows.Forms.Padding(54, 48, 54, 40);
            this.panelBrand.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelBrand.Size = new System.Drawing.Size(481, 720);
            this.panelBrand.TabIndex = 0;
            // 
            // brandLayout
            // 
            this.brandLayout.ColumnCount = 1;
            this.brandLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandLayout.Controls.Add(this.brandTop, 0, 0);
            this.brandLayout.Controls.Add(this.brandFeatureLayout, 0, 2);
            this.brandLayout.Controls.Add(this.lblBrandFooter, 0, 4);
            this.brandLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brandLayout.Location = new System.Drawing.Point(54, 48);
            this.brandLayout.Margin = new System.Windows.Forms.Padding(0);
            this.brandLayout.Name = "brandLayout";
            this.brandLayout.RowCount = 5;
            this.brandLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.brandLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.brandLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 238F));
            this.brandLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.brandLayout.Size = new System.Drawing.Size(373, 632);
            this.brandLayout.TabIndex = 0;
            // 
            // brandTop
            // 
            this.brandTop.ColumnCount = 1;
            this.brandTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandTop.Controls.Add(this.lblBrandMark, 0, 0);
            this.brandTop.Controls.Add(this.lblBrandName, 0, 1);
            this.brandTop.Controls.Add(this.lblBrandTagline, 0, 2);
            this.brandTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brandTop.Location = new System.Drawing.Point(0, 0);
            this.brandTop.Margin = new System.Windows.Forms.Padding(0);
            this.brandTop.Name = "brandTop";
            this.brandTop.RowCount = 3;
            this.brandTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.brandTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.brandTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandTop.Size = new System.Drawing.Size(373, 196);
            this.brandTop.TabIndex = 0;
            // 
            // lblBrandMark
            // 
            this.lblBrandMark.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBrandMark.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.lblBrandMark.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Bold);
            this.lblBrandMark.ForeColor = System.Drawing.Color.White;
            this.lblBrandMark.Location = new System.Drawing.Point(285, 4);
            this.lblBrandMark.Margin = new System.Windows.Forms.Padding(0);
            this.lblBrandMark.Name = "lblBrandMark";
            this.lblBrandMark.Size = new System.Drawing.Size(88, 80);
            this.lblBrandMark.TabIndex = 0;
            this.lblBrandMark.Text = "م";
            this.lblBrandMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBrandName
            // 
            this.lblBrandName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrandName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblBrandName.ForeColor = System.Drawing.Color.White;
            this.lblBrandName.Location = new System.Drawing.Point(0, 88);
            this.lblBrandName.Margin = new System.Windows.Forms.Padding(0);
            this.lblBrandName.Name = "lblBrandName";
            this.lblBrandName.Size = new System.Drawing.Size(373, 48);
            this.lblBrandName.TabIndex = 1;
            this.lblBrandName.Text = "نظام إدارة المدرسة";
            this.lblBrandName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrandTagline
            // 
            this.lblBrandTagline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrandTagline.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.lblBrandTagline.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblBrandTagline.Location = new System.Drawing.Point(0, 136);
            this.lblBrandTagline.Margin = new System.Windows.Forms.Padding(0);
            this.lblBrandTagline.Name = "lblBrandTagline";
            this.lblBrandTagline.Size = new System.Drawing.Size(373, 60);
            this.lblBrandTagline.TabIndex = 2;
            this.lblBrandTagline.Text = "إدارة موحدة للطلاب، المعلمين، الحضور، الرسوم والعمليات المدرسية";
            this.lblBrandTagline.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // brandFeatureLayout
            // 
            this.brandFeatureLayout.ColumnCount = 1;
            this.brandFeatureLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandFeatureLayout.Controls.Add(this.lblFeatureTitle, 0, 0);
            this.brandFeatureLayout.Controls.Add(this.lblFeature1, 0, 1);
            this.brandFeatureLayout.Controls.Add(this.lblFeature2, 0, 2);
            this.brandFeatureLayout.Controls.Add(this.lblFeature3, 0, 3);
            this.brandFeatureLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brandFeatureLayout.Location = new System.Drawing.Point(0, 230);
            this.brandFeatureLayout.Margin = new System.Windows.Forms.Padding(0);
            this.brandFeatureLayout.Name = "brandFeatureLayout";
            this.brandFeatureLayout.RowCount = 4;
            this.brandFeatureLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.brandFeatureLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.brandFeatureLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.brandFeatureLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.brandFeatureLayout.Size = new System.Drawing.Size(373, 238);
            this.brandFeatureLayout.TabIndex = 1;
            // 
            // lblFeatureTitle
            // 
            this.lblFeatureTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeatureTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblFeatureTitle.ForeColor = System.Drawing.Color.FromArgb(153, 246, 228);
            this.lblFeatureTitle.Location = new System.Drawing.Point(0, 0);
            this.lblFeatureTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblFeatureTitle.Name = "lblFeatureTitle";
            this.lblFeatureTitle.Size = new System.Drawing.Size(373, 44);
            this.lblFeatureTitle.TabIndex = 0;
            this.lblFeatureTitle.Text = "منصة عمل واحدة";
            this.lblFeatureTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFeature1
            // 
            this.lblFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeature1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblFeature1.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.lblFeature1.Location = new System.Drawing.Point(0, 44);
            this.lblFeature1.Margin = new System.Windows.Forms.Padding(0);
            this.lblFeature1.Name = "lblFeature1";
            this.lblFeature1.Size = new System.Drawing.Size(373, 64);
            this.lblFeature1.TabIndex = 1;
            this.lblFeature1.Text = "✓  بيانات مدرسية مترابطة في مكان واحد";
            this.lblFeature1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFeature2
            // 
            this.lblFeature2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeature2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblFeature2.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.lblFeature2.Location = new System.Drawing.Point(0, 108);
            this.lblFeature2.Margin = new System.Windows.Forms.Padding(0);
            this.lblFeature2.Name = "lblFeature2";
            this.lblFeature2.Size = new System.Drawing.Size(373, 64);
            this.lblFeature2.TabIndex = 2;
            this.lblFeature2.Text = "✓  متابعة العمليات المالية والأكاديمية بسهولة";
            this.lblFeature2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFeature3
            // 
            this.lblFeature3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeature3.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblFeature3.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.lblFeature3.Location = new System.Drawing.Point(0, 172);
            this.lblFeature3.Margin = new System.Windows.Forms.Padding(0);
            this.lblFeature3.Name = "lblFeature3";
            this.lblFeature3.Size = new System.Drawing.Size(373, 64);
            this.lblFeature3.TabIndex = 3;
            this.lblFeature3.Text = "✓  صلاحيات مستخدمين وسجل للعمليات";
            this.lblFeature3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrandFooter
            // 
            this.lblBrandFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrandFooter.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblBrandFooter.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblBrandFooter.Location = new System.Drawing.Point(0, 598);
            this.lblBrandFooter.Margin = new System.Windows.Forms.Padding(0);
            this.lblBrandFooter.Name = "lblBrandFooter";
            this.lblBrandFooter.Size = new System.Drawing.Size(373, 34);
            this.lblBrandFooter.TabIndex = 2;
            this.lblBrandFooter.Text = "بوابة تشغيل يومية لإدارة المدرسة";
            this.lblBrandFooter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelLoginArea
            // 
            this.panelLoginArea.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.panelLoginArea.Controls.Add(this.loginAreaLayout);
            this.panelLoginArea.Controls.Add(this.lblDevelopers);
            this.panelLoginArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLoginArea.Location = new System.Drawing.Point(481, 0);
            this.panelLoginArea.Margin = new System.Windows.Forms.Padding(0);
            this.panelLoginArea.Name = "panelLoginArea";
            this.panelLoginArea.Padding = new System.Windows.Forms.Padding(28);
            this.panelLoginArea.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelLoginArea.Size = new System.Drawing.Size(639, 720);
            this.panelLoginArea.TabIndex = 1;
            // 
            // loginAreaLayout
            // 
            this.loginAreaLayout.ColumnCount = 3;
            this.loginAreaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.loginAreaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 92F));
            this.loginAreaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.loginAreaLayout.Controls.Add(this.loginCard, 1, 1);
            this.loginAreaLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginAreaLayout.Location = new System.Drawing.Point(28, 28);
            this.loginAreaLayout.Margin = new System.Windows.Forms.Padding(0);
            this.loginAreaLayout.Name = "loginAreaLayout";
            this.loginAreaLayout.RowCount = 3;
            this.loginAreaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.loginAreaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84F));
            this.loginAreaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.loginAreaLayout.Size = new System.Drawing.Size(583, 664);
            this.loginAreaLayout.TabIndex = 0;
            // 
            // loginCard
            // 
            this.loginCard.BackColor = System.Drawing.Color.White;
            this.loginCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginCard.Controls.Add(this.cardLayout);
            this.loginCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginCard.Location = new System.Drawing.Point(22, 56);
            this.loginCard.Margin = new System.Windows.Forms.Padding(0);
            this.loginCard.Name = "loginCard";
            this.loginCard.Padding = new System.Windows.Forms.Padding(42, 34, 42, 30);
            this.loginCard.Size = new System.Drawing.Size(536, 558);
            this.loginCard.TabIndex = 0;
            // 
            // cardLayout
            // 
            this.cardLayout.ColumnCount = 1;
            this.cardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cardLayout.Controls.Add(this.lblWelcome, 0, 0);
            this.cardLayout.Controls.Add(this.lblTitle, 0, 1);
            this.cardLayout.Controls.Add(this.lblSubtitle, 0, 2);
            this.cardLayout.Controls.Add(this.sepTop, 0, 3);
            this.cardLayout.Controls.Add(this.lblUserLabel, 0, 4);
            this.cardLayout.Controls.Add(this.pnlUser, 0, 5);
            this.cardLayout.Controls.Add(this.lblPasswordLabel, 0, 7);
            this.cardLayout.Controls.Add(this.pnlPassword, 0, 8);
            this.cardLayout.Controls.Add(this.pnlLogin, 0, 10);
            this.cardLayout.Controls.Add(this.bottomActions, 0, 12);
            this.cardLayout.Controls.Add(this.lblStatus, 0, 13);
            this.cardLayout.Controls.Add(this.sepBottom, 0, 14);
            this.cardLayout.Controls.Add(this.lblFooter, 0, 15);
            this.cardLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardLayout.Location = new System.Drawing.Point(42, 34);
            this.cardLayout.Margin = new System.Windows.Forms.Padding(0);
            this.cardLayout.Name = "cardLayout";
            this.cardLayout.RowCount = 16;
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cardLayout.Size = new System.Drawing.Size(383, 434);
            this.cardLayout.TabIndex = 0;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.lblWelcome.Location = new System.Drawing.Point(3, 0);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(377, 36);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "مرحبًا بك";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblTitle.Location = new System.Drawing.Point(3, 36);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(377, 42);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "تسجيل الدخول";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtitle.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblSubtitle.Location = new System.Drawing.Point(3, 78);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(377, 32);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "أدخل بيانات حسابك للمتابعة إلى النظام";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sepTop
            // 
            this.sepTop.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.sepTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sepTop.Location = new System.Drawing.Point(0, 97);
            this.sepTop.Margin = new System.Windows.Forms.Padding(0);
            this.sepTop.Name = "sepTop";
            this.sepTop.Size = new System.Drawing.Size(383, 1);
            this.sepTop.TabIndex = 3;
            // 
            // lblUserLabel
            // 
            this.lblUserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserLabel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUserLabel.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblUserLabel.Location = new System.Drawing.Point(3, 130);
            this.lblUserLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblUserLabel.Name = "lblUserLabel";
            this.lblUserLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblUserLabel.Size = new System.Drawing.Size(377, 24);
            this.lblUserLabel.TabIndex = 4;
            this.lblUserLabel.Text = "اسم المستخدم";
            this.lblUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlUser
            // 
            this.pnlUser.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUser.Controls.Add(this.txtUserName);
            this.pnlUser.Controls.Add(this.lblUserIcon);
            this.pnlUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUser.Location = new System.Drawing.Point(0, 154);
            this.pnlUser.Margin = new System.Windows.Forms.Padding(0);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.pnlUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlUser.Size = new System.Drawing.Size(383, 48);
            this.pnlUser.TabIndex = 5;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserName.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtUserName.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.txtUserName.Location = new System.Drawing.Point(12, 5);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(0);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtUserName.Size = new System.Drawing.Size(321, 26);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Text = "اسم المستخدم";
            this.txtUserName.Enter += new System.EventHandler(this.txtUserName_Enter);
            this.txtUserName.Leave += new System.EventHandler(this.txtUserName_Leave);
            // 
            // lblUserIcon
            // 
            this.lblUserIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblUserIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 14F);
            this.lblUserIcon.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.lblUserIcon.Location = new System.Drawing.Point(333, 5);
            this.lblUserIcon.Margin = new System.Windows.Forms.Padding(0);
            this.lblUserIcon.Name = "lblUserIcon";
            this.lblUserIcon.Size = new System.Drawing.Size(36, 36);
            this.lblUserIcon.TabIndex = 0;
            this.lblUserIcon.Text = "";
            this.lblUserIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPasswordLabel
            // 
            this.lblPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPasswordLabel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPasswordLabel.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblPasswordLabel.Location = new System.Drawing.Point(3, 216);
            this.lblPasswordLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblPasswordLabel.Name = "lblPasswordLabel";
            this.lblPasswordLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPasswordLabel.Size = new System.Drawing.Size(377, 24);
            this.lblPasswordLabel.TabIndex = 6;
            this.lblPasswordLabel.Text = "كلمة المرور";
            this.lblPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlPassword
            // 
            this.pnlPassword.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPassword.Controls.Add(this.txtPassword);
            this.pnlPassword.Controls.Add(this.btnTogglePassword);
            this.pnlPassword.Controls.Add(this.lblPasswordIcon);
            this.pnlPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPassword.Location = new System.Drawing.Point(0, 240);
            this.pnlPassword.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPassword.Name = "pnlPassword";
            this.pnlPassword.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.pnlPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlPassword.Size = new System.Drawing.Size(383, 48);
            this.pnlPassword.TabIndex = 7;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.txtPassword.Location = new System.Drawing.Point(52, 5);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(0);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtPassword.Size = new System.Drawing.Size(281, 26);
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
            this.btnTogglePassword.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.btnTogglePassword.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnTogglePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTogglePassword.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnTogglePassword.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnTogglePassword.Location = new System.Drawing.Point(12, 5);
            this.btnTogglePassword.Margin = new System.Windows.Forms.Padding(0);
            this.btnTogglePassword.Name = "btnTogglePassword";
            this.btnTogglePassword.Size = new System.Drawing.Size(40, 36);
            this.btnTogglePassword.TabIndex = 2;
            this.btnTogglePassword.Text = "إظهار";
            this.btnTogglePassword.UseVisualStyleBackColor = false;
            this.btnTogglePassword.Click += new System.EventHandler(this.btnTogglePassword_Click);
            // 
            // lblPasswordIcon
            // 
            this.lblPasswordIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPasswordIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 14F);
            this.lblPasswordIcon.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.lblPasswordIcon.Location = new System.Drawing.Point(333, 5);
            this.lblPasswordIcon.Margin = new System.Windows.Forms.Padding(0);
            this.lblPasswordIcon.Name = "lblPasswordIcon";
            this.lblPasswordIcon.Size = new System.Drawing.Size(36, 36);
            this.lblPasswordIcon.TabIndex = 0;
            this.lblPasswordIcon.Text = "";
            this.lblPasswordIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLogin
            // 
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 306);
            this.pnlLogin.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Padding = new System.Windows.Forms.Padding(0);
            this.pnlLogin.Size = new System.Drawing.Size(383, 52);
            this.pnlLogin.TabIndex = 8;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(13, 91, 85);
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(17, 138, 129);
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(0, 0);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(383, 52);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "تسجيل الدخول";
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
            this.bottomActions.Location = new System.Drawing.Point(0, 372);
            this.bottomActions.Margin = new System.Windows.Forms.Padding(0);
            this.bottomActions.Name = "bottomActions";
            this.bottomActions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bottomActions.RowCount = 1;
            this.bottomActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomActions.Size = new System.Drawing.Size(383, 34);
            this.bottomActions.TabIndex = 9;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnClear.Location = new System.Drawing.Point(5, 1);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(92, 32);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "مسح الحقول";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnExit.Location = new System.Drawing.Point(286, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(92, 32);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "إغلاق";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblStatus.Location = new System.Drawing.Point(3, 406);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(377, 22);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "الحالة: جاهز لتسجيل الدخول";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sepBottom
            // 
            this.sepBottom.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.sepBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.sepBottom.Location = new System.Drawing.Point(0, 428);
            this.sepBottom.Margin = new System.Windows.Forms.Padding(0);
            this.sepBottom.Name = "sepBottom";
            this.sepBottom.Size = new System.Drawing.Size(383, 1);
            this.sepBottom.TabIndex = 11;
            // 
            // lblFooter
            // 
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooter.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblFooter.Location = new System.Drawing.Point(3, 442);
            this.lblFooter.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(377, 1);
            this.lblFooter.TabIndex = 12;
            this.lblFooter.Text = "الوصول إلى النظام متاح للحسابات المصرح لها فقط";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDevelopers
            // 
            this.lblDevelopers.BackColor = System.Drawing.Color.Transparent;
            this.lblDevelopers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDevelopers.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblDevelopers.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblDevelopers.Location = new System.Drawing.Point(28, 654);
            this.lblDevelopers.Margin = new System.Windows.Forms.Padding(0);
            this.lblDevelopers.Name = "lblDevelopers";
            this.lblDevelopers.Padding = new System.Windows.Forms.Padding(8, 2, 8, 4);
            this.lblDevelopers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblDevelopers.Size = new System.Drawing.Size(583, 38);
            this.lblDevelopers.TabIndex = 1;
            this.lblDevelopers.Text = "المطورون: المهندس مهند علي علي علي سعد • عبد الرحمن سعد علي سعد • سامي الحاج";
            this.lblDevelopers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 400;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            this.toolTip.SetToolTip(this.txtUserName, "أدخل اسم المستخدم المسجل في النظام");
            this.toolTip.SetToolTip(this.txtPassword, "أدخل كلمة المرور كما هي دون تعديل المسافات");
            this.toolTip.SetToolTip(this.btnTogglePassword, "إظهار أو إخفاء كلمة المرور");
            this.toolTip.SetToolTip(this.btnClear, "مسح بيانات الدخول وإعادة الحقول إلى حالتها الافتراضية");
            this.toolTip.SetToolTip(this.btnExit, "إغلاق نظام إدارة المدرسة");
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(1120, 720);
            this.Controls.Add(this.panelBackground);
            this.Font = new System.Drawing.Font("Tahoma", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(900, 640);
            this.Name = "LoginForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
