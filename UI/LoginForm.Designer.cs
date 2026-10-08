// LoginForm.Designer.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Helpers;

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
            this.panelBackground = new System.Windows.Forms.Panel();
            this.btnWindowClose = new System.Windows.Forms.Button();
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
            this.lblDevelopers = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
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
            this.panelBackground.BackColor = UIHelper.BackgroundColor;
            this.panelBackground.Controls.Add(this.btnWindowClose);
            this.panelBackground.Controls.Add(this.layoutRoot);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Margin = new System.Windows.Forms.Padding(0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(1180, 720);
            this.panelBackground.TabIndex = 0;
            // 
            // btnWindowClose
            // 
            this.btnWindowClose.BackColor = System.Drawing.Color.Transparent;
            this.btnWindowClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWindowClose.FlatAppearance.BorderSize = 0;
            this.btnWindowClose.FlatAppearance.MouseDownBackColor = UIHelper.LoginDangerPressedColor;
            this.btnWindowClose.FlatAppearance.MouseOverBackColor = UIHelper.LoginDangerHoverColor;
            this.btnWindowClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowClose.Font = new System.Drawing.Font(UIHelper.FontFamily, 12F, System.Drawing.FontStyle.Bold);
            this.btnWindowClose.ForeColor = UIHelper.LoginSecondaryTextColor;
            this.btnWindowClose.Location = new System.Drawing.Point(16, 16);
            this.btnWindowClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnWindowClose.Name = "btnWindowClose";
            this.btnWindowClose.Size = new System.Drawing.Size(36, 36);
            this.btnWindowClose.TabIndex = 1;
            this.btnWindowClose.Text = "✕";
            this.toolTip.SetToolTip(this.btnWindowClose, "إغلاق النافذة");
            this.btnWindowClose.UseVisualStyleBackColor = false;
            this.btnWindowClose.Click += new System.EventHandler(this.btnWindowClose_Click);
            // 
            // layoutRoot
            // 
            this.layoutRoot.ColumnCount = 2;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.layoutRoot.Controls.Add(this.panelBrand, 0, 0);
            this.layoutRoot.Controls.Add(this.panelLoginArea, 1, 0);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.layoutRoot.RowCount = 1;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Size = new System.Drawing.Size(1180, 720);
            this.layoutRoot.TabIndex = 0;
            // 
            // panelBrand
            // 
            this.panelBrand.BackColor = UIHelper.PrimaryDarkColor;
            this.panelBrand.Controls.Add(this.brandLayout);
            this.panelBrand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBrand.Location = new System.Drawing.Point(685, 0);
            this.panelBrand.Margin = new System.Windows.Forms.Padding(0);
            this.panelBrand.Name = "panelBrand";
            this.panelBrand.Padding = new System.Windows.Forms.Padding(48);
            this.panelBrand.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelBrand.Size = new System.Drawing.Size(495, 720);
            this.panelBrand.TabIndex = 0;
            // 
            // brandLayout
            // 
            this.brandLayout.BackColor = System.Drawing.Color.Transparent;
            this.brandLayout.ColumnCount = 1;
            this.brandLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandLayout.Controls.Add(this.brandTop, 0, 0);
            this.brandLayout.Controls.Add(this.brandFeatureLayout, 0, 1);
            this.brandLayout.Controls.Add(this.lblBrandFooter, 0, 2);
            this.brandLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brandLayout.Location = new System.Drawing.Point(48, 48);
            this.brandLayout.Margin = new System.Windows.Forms.Padding(0);
            this.brandLayout.Name = "brandLayout";
            this.brandLayout.RowCount = 3;
            this.brandLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.brandLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.brandLayout.Size = new System.Drawing.Size(399, 624);
            this.brandLayout.TabIndex = 0;
            // 
            // brandTop
            // 
            this.brandTop.BackColor = System.Drawing.Color.Transparent;
            this.brandTop.ColumnCount = 1;
            this.brandTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandTop.Controls.Add(this.lblBrandMark, 0, 0);
            this.brandTop.Controls.Add(this.lblBrandName, 0, 1);
            this.brandTop.Controls.Add(this.lblBrandTagline, 0, 2);
            this.brandTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brandTop.Location = new System.Drawing.Point(0, 0);
            this.brandTop.Margin = new System.Windows.Forms.Padding(0);
            this.brandTop.Name = "brandTop";
            this.brandTop.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.brandTop.RowCount = 3;
            this.brandTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.brandTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.brandTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandTop.Size = new System.Drawing.Size(399, 210);
            this.brandTop.TabIndex = 0;
            // 
            // lblBrandMark
            // 
            this.lblBrandMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBrandMark.BackColor = UIHelper.BrandColor;
            this.lblBrandMark.Font = new System.Drawing.Font(UIHelper.FontFamily, 24F, System.Drawing.FontStyle.Bold);
            this.lblBrandMark.ForeColor = UIHelper.SurfaceColor;
            this.lblBrandMark.Location = new System.Drawing.Point(0, 0);
            this.lblBrandMark.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.lblBrandMark.Name = "lblBrandMark";
            this.lblBrandMark.Size = new System.Drawing.Size(72, 56);
            this.lblBrandMark.TabIndex = 0;
            this.lblBrandMark.Text = "م";
            this.lblBrandMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBrandName
            // 
            this.lblBrandName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrandName.Font = new System.Drawing.Font(UIHelper.FontFamily, 18F, System.Drawing.FontStyle.Bold);
            this.lblBrandName.ForeColor = System.Drawing.Color.White;
            this.lblBrandName.Location = new System.Drawing.Point(0, 72);
            this.lblBrandName.Margin = new System.Windows.Forms.Padding(0);
            this.lblBrandName.Name = "lblBrandName";
            this.lblBrandName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblBrandName.Size = new System.Drawing.Size(399, 48);
            this.lblBrandName.TabIndex = 1;
            this.lblBrandName.Text = "مدرسة النور الأهلية";
            this.lblBrandName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrandTagline
            // 
            this.lblBrandTagline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrandTagline.Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F);
            this.lblBrandTagline.ForeColor = UIHelper.BrandMutedTextColor;
            this.lblBrandTagline.Location = new System.Drawing.Point(0, 120);
            this.lblBrandTagline.Margin = new System.Windows.Forms.Padding(0);
            this.lblBrandTagline.Name = "lblBrandTagline";
            this.lblBrandTagline.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblBrandTagline.Size = new System.Drawing.Size(399, 90);
            this.lblBrandTagline.TabIndex = 2;
            this.lblBrandTagline.Text = "صنعاء - حي الجراف الغربي - بالقرب من ملعب الثورة\r\nهاتف: 772542145";
            this.lblBrandTagline.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // brandFeatureLayout
            // 
            this.brandFeatureLayout.BackColor = System.Drawing.Color.Transparent;
            this.brandFeatureLayout.ColumnCount = 1;
            this.brandFeatureLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.brandFeatureLayout.Controls.Add(this.lblFeatureTitle, 0, 0);
            this.brandFeatureLayout.Controls.Add(this.lblFeature1, 0, 1);
            this.brandFeatureLayout.Controls.Add(this.lblFeature2, 0, 2);
            this.brandFeatureLayout.Controls.Add(this.lblFeature3, 0, 3);
            this.brandFeatureLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brandFeatureLayout.Location = new System.Drawing.Point(0, 218);
            this.brandFeatureLayout.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.brandFeatureLayout.Name = "brandFeatureLayout";
            this.brandFeatureLayout.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.brandFeatureLayout.RowCount = 4;
            this.brandFeatureLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.brandFeatureLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.brandFeatureLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.brandFeatureLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.brandFeatureLayout.Size = new System.Drawing.Size(399, 374);
            this.brandFeatureLayout.TabIndex = 1;
            // 
            // lblFeatureTitle
            // 
            this.lblFeatureTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeatureTitle.Font = new System.Drawing.Font(UIHelper.FontFamily, 11F, System.Drawing.FontStyle.Bold);
            this.lblFeatureTitle.ForeColor = UIHelper.BrandHighlightColor;
            this.lblFeatureTitle.Location = new System.Drawing.Point(0, 0);
            this.lblFeatureTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblFeatureTitle.Name = "lblFeatureTitle";
            this.lblFeatureTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFeatureTitle.Size = new System.Drawing.Size(399, 40);
            this.lblFeatureTitle.TabIndex = 0;
            this.lblFeatureTitle.Text = "إدارة مدرسية ذكية في منصة واحدة";
            this.lblFeatureTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFeature1
            // 
            this.lblFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeature1.Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F);
            this.lblFeature1.ForeColor = UIHelper.BrandFeatureTextColor;
            this.lblFeature1.Location = new System.Drawing.Point(0, 40);
            this.lblFeature1.Margin = new System.Windows.Forms.Padding(0);
            this.lblFeature1.Name = "lblFeature1";
            this.lblFeature1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFeature1.Size = new System.Drawing.Size(399, 38);
            this.lblFeature1.TabIndex = 1;
            this.lblFeature1.Text = "✓   بيانات الطلاب والتسجيلات مترابطة ومنظمة";
            this.lblFeature1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFeature2
            // 
            this.lblFeature2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeature2.Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F);
            this.lblFeature2.ForeColor = UIHelper.BrandFeatureTextColor;
            this.lblFeature2.Location = new System.Drawing.Point(0, 78);
            this.lblFeature2.Margin = new System.Windows.Forms.Padding(0);
            this.lblFeature2.Name = "lblFeature2";
            this.lblFeature2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFeature2.Size = new System.Drawing.Size(399, 38);
            this.lblFeature2.TabIndex = 2;
            this.lblFeature2.Text = "✓   الأكاديميات والرسوم والحضور في مسار واحد";
            this.lblFeature2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFeature3
            // 
            this.lblFeature3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeature3.Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F);
            this.lblFeature3.ForeColor = UIHelper.BrandFeatureTextColor;
            this.lblFeature3.Location = new System.Drawing.Point(0, 116);
            this.lblFeature3.Margin = new System.Windows.Forms.Padding(0);
            this.lblFeature3.Name = "lblFeature3";
            this.lblFeature3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFeature3.Size = new System.Drawing.Size(399, 258);
            this.lblFeature3.TabIndex = 3;
            this.lblFeature3.Text = "✓   أدوار وصلاحيات وسجل تدقيق للعمليات";
            this.lblFeature3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrandFooter
            // 
            this.lblBrandFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrandFooter.Font = new System.Drawing.Font(UIHelper.FontFamily, 8.5F);
            this.lblBrandFooter.ForeColor = UIHelper.TextDisabledColor;
            this.lblBrandFooter.Location = new System.Drawing.Point(0, 592);
            this.lblBrandFooter.Margin = new System.Windows.Forms.Padding(0);
            this.lblBrandFooter.Name = "lblBrandFooter";
            this.lblBrandFooter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblBrandFooter.Size = new System.Drawing.Size(399, 32);
            this.lblBrandFooter.TabIndex = 2;
            this.lblBrandFooter.Text = "SchoolSystem • نظام إدارة المدرسة";
            this.lblBrandFooter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelLoginArea
            // 
            this.panelLoginArea.BackColor = UIHelper.BackgroundColor;
            this.panelLoginArea.Controls.Add(this.loginAreaLayout);
            this.panelLoginArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLoginArea.Location = new System.Drawing.Point(0, 0);
            this.panelLoginArea.Margin = new System.Windows.Forms.Padding(0);
            this.panelLoginArea.Name = "panelLoginArea";
            this.panelLoginArea.Padding = new System.Windows.Forms.Padding(32, 24, 32, 24);
            this.panelLoginArea.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelLoginArea.Size = new System.Drawing.Size(685, 720);
            this.panelLoginArea.TabIndex = 1;
            // 
            // loginAreaLayout
            // 
            this.loginAreaLayout.BackColor = System.Drawing.Color.Transparent;
            this.loginAreaLayout.ColumnCount = 1;
            this.loginAreaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.loginAreaLayout.Controls.Add(this.loginCard, 0, 0);
            this.loginAreaLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginAreaLayout.Location = new System.Drawing.Point(32, 24);
            this.loginAreaLayout.Margin = new System.Windows.Forms.Padding(0);
            this.loginAreaLayout.Name = "loginAreaLayout";
            this.loginAreaLayout.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.loginAreaLayout.RowCount = 1;
            this.loginAreaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.loginAreaLayout.Size = new System.Drawing.Size(621, 672);
            this.loginAreaLayout.TabIndex = 0;
            // 
            // loginCard
            // 
            this.loginCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loginCard.BackColor = System.Drawing.Color.White;
            this.loginCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginCard.Controls.Add(this.cardLayout);
            this.loginCard.Location = new System.Drawing.Point(81, 36);
            this.loginCard.Margin = new System.Windows.Forms.Padding(0);
            this.loginCard.Name = "loginCard";
            this.loginCard.Padding = new System.Windows.Forms.Padding(32, 28, 32, 24);
            this.loginCard.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.loginCard.Size = new System.Drawing.Size(460, 600);
            this.loginCard.TabIndex = 1;
            // 
            // cardLayout
            // 
            this.cardLayout.BackColor = System.Drawing.Color.White;
            this.cardLayout.ColumnCount = 1;
            this.cardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cardLayout.Controls.Add(this.lblWelcome, 0, 0);
            this.cardLayout.Controls.Add(this.lblTitle, 0, 1);
            this.cardLayout.Controls.Add(this.lblSubtitle, 0, 2);
            this.cardLayout.Controls.Add(this.sepTop, 0, 3);
            this.cardLayout.Controls.Add(this.lblUserLabel, 0, 4);
            this.cardLayout.Controls.Add(this.pnlUser, 0, 5);
            this.cardLayout.Controls.Add(this.lblPasswordLabel, 0, 6);
            this.cardLayout.Controls.Add(this.pnlPassword, 0, 7);
            this.cardLayout.Controls.Add(this.pnlLogin, 0, 8);
            this.cardLayout.Controls.Add(this.bottomActions, 0, 9);
            this.cardLayout.Controls.Add(this.lblStatus, 0, 10);
            this.cardLayout.Controls.Add(this.sepBottom, 0, 11);
            this.cardLayout.Controls.Add(this.lblFooter, 0, 12);
            this.cardLayout.Controls.Add(this.lblDevelopers, 0, 13);
            this.cardLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardLayout.Location = new System.Drawing.Point(32, 28);
            this.cardLayout.Margin = new System.Windows.Forms.Padding(0);
            this.cardLayout.Name = "cardLayout";
            this.cardLayout.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cardLayout.RowCount = 14;
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.Size = new System.Drawing.Size(394, 546);
            this.cardLayout.TabIndex = 0;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome.Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = UIHelper.BrandColor;
            this.lblWelcome.Location = new System.Drawing.Point(0, 0);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblWelcome.Size = new System.Drawing.Size(394, 24);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "مرحبًا بك في SchoolSystem";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font(UIHelper.FontFamily, 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = UIHelper.PrimaryDarkColor;
            this.lblTitle.Location = new System.Drawing.Point(0, 24);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTitle.Size = new System.Drawing.Size(394, 42);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "تسجيل الدخول";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtitle.Font = new System.Drawing.Font(UIHelper.FontFamily, 9F);
            this.lblSubtitle.ForeColor = UIHelper.LoginSecondaryTextColor;
            this.lblSubtitle.Location = new System.Drawing.Point(0, 66);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSubtitle.Size = new System.Drawing.Size(394, 40);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "أدخل بيانات حسابك المصرّح له بالوصول إلى نظام إدارة المدرسة";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sepTop
            // 
            this.sepTop.BackColor = UIHelper.BrandFeatureTextColor;
            this.sepTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.sepTop.Location = new System.Drawing.Point(0, 112);
            this.sepTop.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.sepTop.Name = "sepTop";
            this.sepTop.Size = new System.Drawing.Size(394, 1);
            this.sepTop.TabIndex = 3;
            // 
            // lblUserLabel
            // 
            this.lblUserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserLabel.Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUserLabel.ForeColor = UIHelper.PrimaryColor;
            this.lblUserLabel.Location = new System.Drawing.Point(0, 122);
            this.lblUserLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lblUserLabel.Name = "lblUserLabel";
            this.lblUserLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUserLabel.Size = new System.Drawing.Size(394, 24);
            this.lblUserLabel.TabIndex = 4;
            this.lblUserLabel.Text = "اسم المستخدم";
            this.lblUserLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // pnlUser
            // 
            this.pnlUser.BackColor = UIHelper.BackgroundColor;
            this.pnlUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUser.Controls.Add(this.txtUserName);
            this.pnlUser.Controls.Add(this.lblUserIcon);
            this.pnlUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUser.Location = new System.Drawing.Point(0, 148);
            this.pnlUser.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlUser.Size = new System.Drawing.Size(394, 44);
            this.pnlUser.TabIndex = 5;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = UIHelper.BackgroundColor;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserName.Font = new System.Drawing.Font(UIHelper.FontFamily, 10F);
            this.txtUserName.ForeColor = UIHelper.TextDisabledColor;
            this.txtUserName.Location = new System.Drawing.Point(10, 8);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(0);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtUserName.Size = new System.Drawing.Size(340, 23);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.Text = "اسم المستخدم";
            this.toolTip.SetToolTip(this.txtUserName, "أدخل اسم المستخدم المسجل في النظام");
            this.txtUserName.Enter += new System.EventHandler(this.txtUserName_Enter);
            this.txtUserName.Leave += new System.EventHandler(this.txtUserName_Leave);
            // 
            // lblUserIcon
            // 
            this.lblUserIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblUserIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 13F);
            this.lblUserIcon.ForeColor = UIHelper.BrandColor;
            this.lblUserIcon.Location = new System.Drawing.Point(350, 8);
            this.lblUserIcon.Margin = new System.Windows.Forms.Padding(0);
            this.lblUserIcon.Name = "lblUserIcon";
            this.lblUserIcon.Size = new System.Drawing.Size(32, 26);
            this.lblUserIcon.TabIndex = 1;
            this.lblUserIcon.Text = "";
            this.lblUserIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPasswordLabel
            // 
            this.lblPasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPasswordLabel.Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPasswordLabel.ForeColor = UIHelper.PrimaryColor;
            this.lblPasswordLabel.Location = new System.Drawing.Point(0, 194);
            this.lblPasswordLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lblPasswordLabel.Name = "lblPasswordLabel";
            this.lblPasswordLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPasswordLabel.Size = new System.Drawing.Size(394, 24);
            this.lblPasswordLabel.TabIndex = 6;
            this.lblPasswordLabel.Text = "كلمة المرور";
            this.lblPasswordLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // pnlPassword
            // 
            this.pnlPassword.BackColor = UIHelper.BackgroundColor;
            this.pnlPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPassword.Controls.Add(this.txtPassword);
            this.pnlPassword.Controls.Add(this.btnTogglePassword);
            this.pnlPassword.Controls.Add(this.lblPasswordIcon);
            this.pnlPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPassword.Location = new System.Drawing.Point(0, 220);
            this.pnlPassword.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.pnlPassword.Name = "pnlPassword";
            this.pnlPassword.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlPassword.Size = new System.Drawing.Size(394, 44);
            this.pnlPassword.TabIndex = 7;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = UIHelper.BackgroundColor;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Font = new System.Drawing.Font(UIHelper.FontFamily, 10F);
            this.txtPassword.ForeColor = UIHelper.TextDisabledColor;
            this.txtPassword.Location = new System.Drawing.Point(91, 8);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(0);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtPassword.Size = new System.Drawing.Size(259, 23);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "كلمة المرور";
            this.toolTip.SetToolTip(this.txtPassword, "أدخل كلمة المرور الخاصة بحسابك");
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            // 
            // btnTogglePassword
            // 
            this.btnTogglePassword.BackColor = System.Drawing.Color.Transparent;
            this.btnTogglePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTogglePassword.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTogglePassword.FlatAppearance.BorderSize = 0;
            this.btnTogglePassword.FlatAppearance.MouseDownBackColor = UIHelper.BrandFeatureTextColor;
            this.btnTogglePassword.FlatAppearance.MouseOverBackColor = UIHelper.SurfaceSecondaryColor;
            this.btnTogglePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTogglePassword.Font = new System.Drawing.Font(UIHelper.FontFamily, 8.5F, System.Drawing.FontStyle.Bold);
            this.btnTogglePassword.ForeColor = UIHelper.SecondaryColor;
            this.btnTogglePassword.Location = new System.Drawing.Point(10, 8);
            this.btnTogglePassword.Margin = new System.Windows.Forms.Padding(0);
            this.btnTogglePassword.Name = "btnTogglePassword";
            this.btnTogglePassword.Size = new System.Drawing.Size(81, 26);
            this.btnTogglePassword.TabIndex = 2;
            this.btnTogglePassword.Text = "إظهار";
            this.toolTip.SetToolTip(this.btnTogglePassword, "إظهار أو إخفاء كلمة المرور");
            this.btnTogglePassword.UseVisualStyleBackColor = false;
            this.btnTogglePassword.Click += new System.EventHandler(this.btnTogglePassword_Click);
            // 
            // lblPasswordIcon
            // 
            this.lblPasswordIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPasswordIcon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 13F);
            this.lblPasswordIcon.ForeColor = UIHelper.BrandColor;
            this.lblPasswordIcon.Location = new System.Drawing.Point(350, 8);
            this.lblPasswordIcon.Margin = new System.Windows.Forms.Padding(0);
            this.lblPasswordIcon.Name = "lblPasswordIcon";
            this.lblPasswordIcon.Size = new System.Drawing.Size(32, 26);
            this.lblPasswordIcon.TabIndex = 0;
            this.lblPasswordIcon.Text = "";
            this.lblPasswordIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLogin
            // 
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 266);
            this.pnlLogin.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Padding = new System.Windows.Forms.Padding(0, 10, 0, 2);
            this.pnlLogin.Size = new System.Drawing.Size(394, 56);
            this.pnlLogin.TabIndex = 8;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = UIHelper.BrandColor;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseDownBackColor = UIHelper.AccentColor;
            this.btnLogin.FlatAppearance.MouseOverBackColor = UIHelper.BrandHoverColor;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font(UIHelper.FontFamily, 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(0, 10);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnLogin.Size = new System.Drawing.Size(394, 44);
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
            this.bottomActions.Controls.Add(this.btnClear, 0, 0);
            this.bottomActions.Controls.Add(this.btnExit, 1, 0);
            this.bottomActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomActions.Location = new System.Drawing.Point(0, 326);
            this.bottomActions.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.bottomActions.Name = "bottomActions";
            this.bottomActions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bottomActions.RowCount = 1;
            this.bottomActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomActions.Size = new System.Drawing.Size(394, 36);
            this.bottomActions.TabIndex = 9;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.FlatAppearance.BorderColor = UIHelper.BrandMutedTextColor;
            this.btnClear.FlatAppearance.MouseDownBackColor = UIHelper.SurfaceSecondaryColor;
            this.btnClear.FlatAppearance.MouseOverBackColor = UIHelper.BackgroundColor;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font(UIHelper.FontFamily, 9F);
            this.btnClear.ForeColor = UIHelper.SecondaryColor;
            this.btnClear.Location = new System.Drawing.Point(197, 1);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.btnClear.Name = "btnClear";
            this.btnClear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnClear.Size = new System.Drawing.Size(193, 34);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "مسح الحقول";
            this.toolTip.SetToolTip(this.btnClear, "مسح بيانات الدخول وإعادة الحقول إلى حالتها الافتراضية");
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.FlatAppearance.BorderColor = UIHelper.BrandMutedTextColor;
            this.btnExit.FlatAppearance.MouseDownBackColor = UIHelper.LoginDangerPressedColor;
            this.btnExit.FlatAppearance.MouseOverBackColor = UIHelper.LoginDangerHoverColor;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font(UIHelper.FontFamily, 9F);
            this.btnExit.ForeColor = UIHelper.LoginDangerTextColor;
            this.btnExit.Location = new System.Drawing.Point(4, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(0, 1, 4, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnExit.Size = new System.Drawing.Size(193, 34);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "خروج";
            this.toolTip.SetToolTip(this.btnExit, "إغلاق تطبيق نظام إدارة المدرسة");
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font(UIHelper.FontFamily, 8.5F);
            this.lblStatus.ForeColor = UIHelper.LoginSecondaryTextColor;
            this.lblStatus.Location = new System.Drawing.Point(0, 362);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblStatus.Size = new System.Drawing.Size(394, 28);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "الحالة: جاهز لتسجيل الدخول";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sepBottom
            // 
            this.sepBottom.BackColor = UIHelper.BrandFeatureTextColor;
            this.sepBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.sepBottom.Location = new System.Drawing.Point(0, 394);
            this.sepBottom.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.sepBottom.Name = "sepBottom";
            this.sepBottom.Size = new System.Drawing.Size(394, 1);
            this.sepBottom.TabIndex = 11;
            // 
            // lblFooter
            // 
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooter.Font = new System.Drawing.Font(UIHelper.FontFamily, 8F);
            this.lblFooter.ForeColor = UIHelper.TextDisabledColor;
            this.lblFooter.Location = new System.Drawing.Point(0, 402);
            this.lblFooter.Margin = new System.Windows.Forms.Padding(0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFooter.Size = new System.Drawing.Size(394, 24);
            this.lblFooter.TabIndex = 12;
            this.lblFooter.Text = "الوصول إلى النظام متاح للحسابات المصرح لها فقط";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDevelopers
            // 
            this.lblDevelopers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDevelopers.Font = new System.Drawing.Font(UIHelper.FontFamily, 8F);
            this.lblDevelopers.ForeColor = UIHelper.TextDisabledColor;
            this.lblDevelopers.Location = new System.Drawing.Point(0, 426);
            this.lblDevelopers.Margin = new System.Windows.Forms.Padding(0);
            this.lblDevelopers.Name = "lblDevelopers";
            this.lblDevelopers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblDevelopers.Size = new System.Drawing.Size(394, 120);
            this.lblDevelopers.TabIndex = 13;
            this.lblDevelopers.Text = UIHelper.LoginDeveloperCredits;
            this.lblDevelopers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 400;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = UIHelper.BackgroundColor;
            this.ClientSize = new System.Drawing.Size(1180, 720);
            this.Controls.Add(this.panelBackground);
            this.Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1040, 680);
            this.Name = "LoginForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = UIHelper.LoginWindowTitle;
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
