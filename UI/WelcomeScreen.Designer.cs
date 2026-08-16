namespace SchoolSystem.UI
{
    partial class WelcomeScreen
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region كود المصمم

        private void InitializeComponent()
        {
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblSystemName = new System.Windows.Forms.Label();
            this.lblTagline = new System.Windows.Forms.Label();
            this.flowQuickLinks = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnTeachers = new System.Windows.Forms.Button();
            this.btnFinance = new System.Windows.Forms.Button();
            this.btnAttendance = new System.Windows.Forms.Button();

            this.panelBackground.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.flowQuickLinks.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(33, 42, 57);
            this.panelBackground.Controls.Add(this.panelCenter);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(950, 560);
            this.panelBackground.TabIndex = 0;

            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.lblIcon);
            this.panelCenter.Controls.Add(this.lblSystemName);
            this.panelCenter.Controls.Add(this.lblTagline);
            this.panelCenter.Controls.Add(this.flowQuickLinks);
            this.panelCenter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelCenter.Location = new System.Drawing.Point(220, 100);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(760, 360);
            this.panelCenter.TabIndex = 0;

            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = false;
            this.lblIcon.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblIcon.Font = new System.Drawing.Font("Tahoma", 54F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIcon.ForeColor = System.Drawing.Color.White;
            this.lblIcon.Location = new System.Drawing.Point(0, 0);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(760, 92);
            this.lblIcon.TabIndex = 0;
            this.lblIcon.Text = "🏫";

            // 
            // lblSystemName
            // 
            this.lblSystemName.AutoSize = false;
            this.lblSystemName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSystemName.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold);
            this.lblSystemName.ForeColor = System.Drawing.Color.White;
            this.lblSystemName.Location = new System.Drawing.Point(0, 92);
            this.lblSystemName.Name = "lblSystemName";
            this.lblSystemName.Size = new System.Drawing.Size(760, 56);
            this.lblSystemName.TabIndex = 1;
            this.lblSystemName.Text = "نظام إدارة المدرسة";
            this.lblSystemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblTagline
            // 
            this.lblTagline.AutoSize = false;
            this.lblTagline.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTagline.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblTagline.ForeColor = System.Drawing.Color.LightGray;
            this.lblTagline.Location = new System.Drawing.Point(0, 148);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(760, 34);
            this.lblTagline.TabIndex = 2;
            this.lblTagline.Text = "الحل المتكامل لإدارة المؤسسات التعليمية";
            this.lblTagline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // flowQuickLinks
            // 
            this.flowQuickLinks.Controls.Add(this.btnStudents);
            this.flowQuickLinks.Controls.Add(this.btnTeachers);
            this.flowQuickLinks.Controls.Add(this.btnFinance);
            this.flowQuickLinks.Controls.Add(this.btnAttendance);
            this.flowQuickLinks.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowQuickLinks.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowQuickLinks.Location = new System.Drawing.Point(0, 302);
            this.flowQuickLinks.Name = "flowQuickLinks";
            this.flowQuickLinks.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.flowQuickLinks.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowQuickLinks.Size = new System.Drawing.Size(760, 58);
            this.flowQuickLinks.WrapContents = false;
            this.flowQuickLinks.TabIndex = 3;

            // 
            // btnStudents
            // 
            this.btnStudents.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnStudents.FlatAppearance.BorderSize = 0;
            this.btnStudents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudents.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnStudents.ForeColor = System.Drawing.Color.White;
            this.btnStudents.Location = new System.Drawing.Point(3, 10);
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnStudents.Size = new System.Drawing.Size(164, 44);
            this.btnStudents.TabIndex = 0;
            this.btnStudents.Text = "👨‍🎓 الطلاب";
            this.btnStudents.UseVisualStyleBackColor = false;
            this.btnStudents.Click += new System.EventHandler(this.btnStudents_Click);

            // 
            // btnTeachers
            // 
            this.btnTeachers.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnTeachers.FlatAppearance.BorderSize = 0;
            this.btnTeachers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeachers.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnTeachers.ForeColor = System.Drawing.Color.White;
            this.btnTeachers.Location = new System.Drawing.Point(129, 10);
            this.btnTeachers.Name = "btnTeachers";
            this.btnTeachers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnTeachers.Size = new System.Drawing.Size(164, 44);
            this.btnTeachers.TabIndex = 1;
            this.btnTeachers.Text = "👨‍🏫 المعلمين";
            this.btnTeachers.UseVisualStyleBackColor = false;
            this.btnTeachers.Click += new System.EventHandler(this.btnTeachers_Click);

            // 
            // btnFinance
            // 
            this.btnFinance.BackColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.btnFinance.FlatAppearance.BorderSize = 0;
            this.btnFinance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnFinance.ForeColor = System.Drawing.Color.White;
            this.btnFinance.Location = new System.Drawing.Point(255, 10);
            this.btnFinance.Name = "btnFinance";
            this.btnFinance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnFinance.Size = new System.Drawing.Size(164, 44);
            this.btnFinance.TabIndex = 2;
            this.btnFinance.Text = "💰 المالية";
            this.btnFinance.UseVisualStyleBackColor = false;
            this.btnFinance.Click += new System.EventHandler(this.btnFinance_Click);

            // 
            // btnAttendance
            // 
            this.btnAttendance.BackColor = System.Drawing.Color.FromArgb(142, 68, 173);
            this.btnAttendance.FlatAppearance.BorderSize = 0;
            this.btnAttendance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAttendance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAttendance.ForeColor = System.Drawing.Color.White;
            this.btnAttendance.Location = new System.Drawing.Point(381, 10);
            this.btnAttendance.Name = "btnAttendance";
            this.btnAttendance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnAttendance.Size = new System.Drawing.Size(164, 44);
            this.btnAttendance.TabIndex = 3;
            this.btnAttendance.Text = "📝 الحضور";
            this.btnAttendance.UseVisualStyleBackColor = false;
            this.btnAttendance.Click += new System.EventHandler(this.btnAttendance_Click);

            // 
            // WelcomeScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBackground);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "WelcomeScreen";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(950, 560);
            this.panelBackground.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.panelCenter.PerformLayout();
            this.flowQuickLinks.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblSystemName;
        private System.Windows.Forms.Label lblTagline;
        private System.Windows.Forms.FlowLayoutPanel flowQuickLinks;
        private System.Windows.Forms.Button btnStudents;
        private System.Windows.Forms.Button btnTeachers;
        private System.Windows.Forms.Button btnFinance;
        private System.Windows.Forms.Button btnAttendance;
    }
}