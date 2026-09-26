using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolSystem.UI
{
    partial class WelcomeScreen
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (pictureWelcomeHero != null && pictureWelcomeHero.Image != null)
                {
                    Image image = pictureWelcomeHero.Image;
                    pictureWelcomeHero.Image = null;
                    image.Dispose();
                }

                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region كود المصمم

        private void InitializeComponent()
        {
            this.components = new Container();
            this.panelBackground = new Panel();
            this.contentLayout = new TableLayoutPanel();
            this.pictureWelcomeHero = new PictureBox();
            this.infoPanel = new Panel();
            this.lblSystemName = new Label();
            this.lblTagline = new Label();
            this.quickLinksPanel = new Panel();
            this.flowQuickLinks = new FlowLayoutPanel();
            this.btnStudents = new Button();
            this.btnTeachers = new Button();
            this.btnFinance = new Button();
            this.btnAttendance = new Button();

            this.panelBackground.SuspendLayout();
            this.contentLayout.SuspendLayout();
            ((ISupportInitialize)(this.pictureWelcomeHero)).BeginInit();
            this.infoPanel.SuspendLayout();
            this.quickLinksPanel.SuspendLayout();
            this.flowQuickLinks.SuspendLayout();
            this.SuspendLayout();


            this.panelBackground.BackColor = Color.FromArgb(22, 34, 48);
            this.panelBackground.Controls.Add(this.contentLayout);
            this.panelBackground.Dock = DockStyle.Fill;
            this.panelBackground.Location = new Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Padding = new Padding(18, 16, 18, 14);
            this.panelBackground.Size = new Size(950, 560);
            this.panelBackground.TabIndex = 0;

            // contentLayout يفصل الصورة عن معلومات الترحيب والأزرار لمنع أي طبقة من حجب الصورة.
            this.contentLayout.BackColor = Color.FromArgb(22, 34, 48);
            this.contentLayout.ColumnCount = 1;
            this.contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.contentLayout.Controls.Add(this.pictureWelcomeHero, 0, 0);
            this.contentLayout.Controls.Add(this.infoPanel, 0, 1);
            this.contentLayout.Controls.Add(this.quickLinksPanel, 0, 2);
            this.contentLayout.Dock = DockStyle.Fill;
            this.contentLayout.Location = new Point(18, 16);
            this.contentLayout.Name = "contentLayout";
            this.contentLayout.RowCount = 3;
            this.contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            this.contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 14F));
            this.contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 16F));
            this.contentLayout.Size = new Size(914, 530);
            this.contentLayout.TabIndex = 0;


            this.pictureWelcomeHero.BackColor = Color.FromArgb(22, 34, 48);
            this.pictureWelcomeHero.Dock = DockStyle.Fill;
            this.pictureWelcomeHero.Location = new Point(3, 3);
            this.pictureWelcomeHero.Margin = new Padding(3);
            this.pictureWelcomeHero.Name = "pictureWelcomeHero";
            this.pictureWelcomeHero.Size = new Size(908, 365);
            this.pictureWelcomeHero.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureWelcomeHero.TabIndex = 0;
            this.pictureWelcomeHero.TabStop = false;


            this.infoPanel.BackColor = Color.FromArgb(248, 250, 252);
            this.infoPanel.Controls.Add(this.lblTagline);
            this.infoPanel.Controls.Add(this.lblSystemName);
            this.infoPanel.Dock = DockStyle.Fill;
            this.infoPanel.Location = new Point(3, 374);
            this.infoPanel.Margin = new Padding(3, 3, 3, 0);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new Padding(10, 2, 10, 0);
            this.infoPanel.Size = new Size(908, 68);
            this.infoPanel.TabIndex = 1;


            this.lblSystemName.AutoSize = false;
            this.lblSystemName.Dock = DockStyle.Top;
            this.lblSystemName.Font = new Font("Tahoma", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemName.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblSystemName.Location = new Point(10, 2);
            this.lblSystemName.Name = "lblSystemName";
            this.lblSystemName.Size = new Size(888, 38);
            this.lblSystemName.TabIndex = 0;
            this.lblSystemName.Text = "نظام إدارة المدرسة";
            this.lblSystemName.TextAlign = ContentAlignment.MiddleCenter;


            this.lblTagline.AutoSize = false;
            this.lblTagline.Dock = DockStyle.Fill;
            this.lblTagline.Font = new Font("Tahoma", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.lblTagline.ForeColor = Color.FromArgb(71, 85, 105);
            this.lblTagline.Location = new Point(10, 40);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new Size(888, 28);
            this.lblTagline.TabIndex = 1;
            this.lblTagline.Text = "الحل المتكامل لإدارة المؤسسات التعليمية";
            this.lblTagline.TextAlign = ContentAlignment.TopCenter;


            this.quickLinksPanel.BackColor = Color.FromArgb(248, 250, 252);
            this.quickLinksPanel.Controls.Add(this.flowQuickLinks);
            this.quickLinksPanel.Dock = DockStyle.Fill;
            this.quickLinksPanel.Location = new Point(3, 442);
            this.quickLinksPanel.Margin = new Padding(3, 0, 3, 3);
            this.quickLinksPanel.Name = "quickLinksPanel";
            this.quickLinksPanel.Padding = new Padding(0, 6, 0, 4);
            this.quickLinksPanel.Size = new Size(908, 85);
            this.quickLinksPanel.TabIndex = 2;

            // flowQuickLinks يرتب الأزرار في صف منفصل مع تمرير عند ضيق العرض بدل إخفائها.
            this.flowQuickLinks.AutoScroll = true;
            this.flowQuickLinks.BackColor = Color.FromArgb(248, 250, 252);
            this.flowQuickLinks.Controls.Add(this.btnStudents);
            this.flowQuickLinks.Controls.Add(this.btnTeachers);
            this.flowQuickLinks.Controls.Add(this.btnFinance);
            this.flowQuickLinks.Controls.Add(this.btnAttendance);
            this.flowQuickLinks.Dock = DockStyle.Fill;
            // RightToLeft.Yes يعكس الترتيب؛ استخدام LeftToRight معه يمنع القلب المزدوج.
            this.flowQuickLinks.FlowDirection = FlowDirection.LeftToRight;
            this.flowQuickLinks.Location = new Point(0, 6);
            this.flowQuickLinks.Name = "flowQuickLinks";
            this.flowQuickLinks.Padding = new Padding(8, 3, 8, 3);
            this.flowQuickLinks.RightToLeft = RightToLeft.Yes;
            this.flowQuickLinks.Size = new Size(908, 75);
            this.flowQuickLinks.TabIndex = 0;
            this.flowQuickLinks.WrapContents = true;

            // btnStudents يفتح شاشة إدارة الطلاب عند امتلاك الصلاحية.
            this.btnStudents.BackColor = Color.FromArgb(41, 128, 185);
            this.btnStudents.FlatAppearance.BorderSize = 0;
            this.btnStudents.FlatAppearance.MouseDownBackColor = Color.FromArgb(31, 98, 145);
            this.btnStudents.FlatAppearance.MouseOverBackColor = Color.FromArgb(52, 145, 205);
            this.btnStudents.FlatStyle = FlatStyle.Flat;
            this.btnStudents.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.btnStudents.ForeColor = Color.White;
            this.btnStudents.Margin = new Padding(6, 5, 6, 5);
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Size = new Size(145, 48);
            this.btnStudents.TabIndex = 0;
            this.btnStudents.Text = "الطلاب";
            this.btnStudents.UseVisualStyleBackColor = false;
            this.btnStudents.Click += new EventHandler(this.btnStudents_Click);

            // btnTeachers يفتح شاشة إدارة المعلمين عند امتلاك الصلاحية.
            this.btnTeachers.BackColor = Color.FromArgb(39, 174, 96);
            this.btnTeachers.FlatAppearance.BorderSize = 0;
            this.btnTeachers.FlatAppearance.MouseDownBackColor = Color.FromArgb(28, 128, 70);
            this.btnTeachers.FlatAppearance.MouseOverBackColor = Color.FromArgb(53, 194, 111);
            this.btnTeachers.FlatStyle = FlatStyle.Flat;
            this.btnTeachers.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.btnTeachers.ForeColor = Color.White;
            this.btnTeachers.Margin = new Padding(6, 5, 6, 5);
            this.btnTeachers.Name = "btnTeachers";
            this.btnTeachers.Size = new Size(145, 48);
            this.btnTeachers.TabIndex = 1;
            this.btnTeachers.Text = "المعلمون";
            this.btnTeachers.UseVisualStyleBackColor = false;
            this.btnTeachers.Click += new EventHandler(this.btnTeachers_Click);

            // btnFinance يفتح أول وحدة مالية يملك المستخدم صلاحيتها.
            this.btnFinance.BackColor = Color.FromArgb(230, 126, 34);
            this.btnFinance.FlatAppearance.BorderSize = 0;
            this.btnFinance.FlatAppearance.MouseDownBackColor = Color.FromArgb(171, 88, 22);
            this.btnFinance.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 145, 52);
            this.btnFinance.FlatStyle = FlatStyle.Flat;
            this.btnFinance.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.btnFinance.ForeColor = Color.White;
            this.btnFinance.Margin = new Padding(6, 5, 6, 5);
            this.btnFinance.Name = "btnFinance";
            this.btnFinance.Size = new Size(145, 48);
            this.btnFinance.TabIndex = 2;
            this.btnFinance.Text = "المالية";
            this.btnFinance.UseVisualStyleBackColor = false;
            this.btnFinance.Click += new EventHandler(this.btnFinance_Click);

            // btnAttendance يفتح شاشة الحضور والغياب عند امتلاك الصلاحية.
            this.btnAttendance.BackColor = Color.FromArgb(142, 68, 173);
            this.btnAttendance.FlatAppearance.BorderSize = 0;
            this.btnAttendance.FlatAppearance.MouseDownBackColor = Color.FromArgb(103, 48, 126);
            this.btnAttendance.FlatAppearance.MouseOverBackColor = Color.FromArgb(162, 85, 194);
            this.btnAttendance.FlatStyle = FlatStyle.Flat;
            this.btnAttendance.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.btnAttendance.ForeColor = Color.White;
            this.btnAttendance.Margin = new Padding(6, 5, 6, 5);
            this.btnAttendance.Name = "btnAttendance";
            this.btnAttendance.Size = new Size(145, 48);
            this.btnAttendance.TabIndex = 3;
            this.btnAttendance.Text = "الحضور والغياب";
            this.btnAttendance.UseVisualStyleBackColor = false;
            this.btnAttendance.Click += new EventHandler(this.btnAttendance_Click);


            this.AutoScaleDimensions = new SizeF(7F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(22, 34, 48);
            this.Controls.Add(this.panelBackground);
            this.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WelcomeScreen";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(950, 560);

            this.panelBackground.ResumeLayout(false);
            this.contentLayout.ResumeLayout(false);
            ((ISupportInitialize)(this.pictureWelcomeHero)).EndInit();
            this.infoPanel.ResumeLayout(false);
            this.quickLinksPanel.ResumeLayout(false);
            this.flowQuickLinks.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelBackground;
        private TableLayoutPanel contentLayout;
        private PictureBox pictureWelcomeHero;
        private Panel infoPanel;
        private Label lblSystemName;
        private Label lblTagline;
        private Panel quickLinksPanel;
        private FlowLayoutPanel flowQuickLinks;
        private Button btnStudents;
        private Button btnTeachers;
        private Button btnFinance;
        private Button btnAttendance;
    }
}
