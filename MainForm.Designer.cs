namespace SchoolSystem
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region كود المصمم

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.tsmiDashboard = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStudents = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStudentsManage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStudentsEnroll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStudentsClasses = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTeachers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTeachersManage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTeachersAttendance = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTeachersPayroll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAcademic = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSubjects = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClasses = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTimetable = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGrades = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAttendance = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFinancial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFees = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVouchers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExpenses = new System.Windows.Forms.ToolStripMenuItem();
            this.تعريفرسومالصفوفToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReports = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAuditLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new Krypton.Toolkit.KryptonPanel();
            this.lblDateTime = new Krypton.Toolkit.KryptonLabel();
            this.lblUsername = new Krypton.Toolkit.KryptonLabel();
            this.lblSystemTitle = new Krypton.Toolkit.KryptonLabel();
            this.panelContent = new Krypton.Toolkit.KryptonPanel();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.lblDBStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOnlineUsers = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);

            this.menuStripMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();

            // menuStripMain
            this.menuStripMain.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.menuStripMain.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.menuStripMain.ForeColor = System.Drawing.Color.White;
            this.menuStripMain.ImageScalingSize = new System.Drawing.Size(16, 16);
            this.menuStripMain.CanOverflow = true;
            this.menuStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.menuStripMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStripMain.AutoSize = false;
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiDashboard,
                this.tsmiStudents,
                this.tsmiTeachers,
                this.tsmiAcademic,
                this.tsmiAttendance,
                this.tsmiFinancial,
                this.tsmiTransport,
                this.tsmiLibrary,
                this.tsmiUsers,
                this.tsmiReports,
                this.tsmiAuditLogs,
                this.tsmiSettings,
                this.tsmiLogout
            });
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.menuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStripMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStripMain.Size = new System.Drawing.Size(1200, 41);
            this.menuStripMain.TabIndex = 0;

            // tsmiAuditLogs
            this.tsmiAuditLogs.Name = "tsmiAuditLogs";
            this.tsmiAuditLogs.Visible = true;
            this.tsmiAuditLogs.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsmiAuditLogs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmiAuditLogs.AutoSize = true;
            this.tsmiAuditLogs.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tsmiAuditLogs.Size = new System.Drawing.Size(110, 29);
            this.tsmiAuditLogs.Text = "سجل الأنشطة";
            this.tsmiAuditLogs.Click += new System.EventHandler(this.tsmiAuditLogs_Click);

            // tsmiSettings
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Visible = true;
            this.tsmiSettings.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsmiSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmiSettings.AutoSize = true;
            this.tsmiSettings.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tsmiSettings.Size = new System.Drawing.Size(90, 29);
            this.tsmiSettings.Text = "الإعدادات";
            this.tsmiSettings.Click += new System.EventHandler(this.tsmiSettings_Click);

            // tsmiDashboard
            this.tsmiDashboard.Name = "tsmiDashboard";
            this.tsmiDashboard.Size = new System.Drawing.Size(106, 29);
            this.tsmiDashboard.Text = "📊 الرئيسية";
            this.tsmiDashboard.Click += new System.EventHandler(this.tsmiDashboard_Click);

            // tsmiStudents
            this.tsmiStudents.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiStudentsManage,
                this.tsmiStudentsEnroll,
                this.tsmiStudentsClasses
            });
            this.tsmiStudents.Name = "tsmiStudents";
            this.tsmiStudents.Size = new System.Drawing.Size(90, 29);
            this.tsmiStudents.Text = "👨‍🎓 الطلاب";

            // tsmiStudentsManage
            this.tsmiStudentsManage.Name = "tsmiStudentsManage";
            this.tsmiStudentsManage.Size = new System.Drawing.Size(220, 28);
            this.tsmiStudentsManage.Text = "إدارة الطلاب";
            this.tsmiStudentsManage.Click += new System.EventHandler(this.tsmiStudentsManage_Click);

            // tsmiStudentsEnroll
            this.tsmiStudentsEnroll.Name = "tsmiStudentsEnroll";
            this.tsmiStudentsEnroll.Size = new System.Drawing.Size(220, 28);
            this.tsmiStudentsEnroll.Text = "التسجيل والقبول";
            this.tsmiStudentsEnroll.Click += new System.EventHandler(this.tsmiStudentsEnroll_Click);

            // tsmiStudentsClasses
            this.tsmiStudentsClasses.Name = "tsmiStudentsClasses";
            this.tsmiStudentsClasses.Size = new System.Drawing.Size(220, 28);
            this.tsmiStudentsClasses.Text = "توزيع الفصول";
            this.tsmiStudentsClasses.Click += new System.EventHandler(this.tsmiStudentsClasses_Click);

            // tsmiTeachers
            this.tsmiTeachers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiTeachersManage,
                this.tsmiTeachersAttendance,
                this.tsmiTeachersPayroll
            });
            this.tsmiTeachers.Name = "tsmiTeachers";
            this.tsmiTeachers.Size = new System.Drawing.Size(107, 29);
            this.tsmiTeachers.Text = "👨‍🏫 المعلمين";

            // tsmiTeachersManage
            this.tsmiTeachersManage.Name = "tsmiTeachersManage";
            this.tsmiTeachersManage.Size = new System.Drawing.Size(220, 28);
            this.tsmiTeachersManage.Text = "إدارة المعلمين";
            this.tsmiTeachersManage.Click += new System.EventHandler(this.tsmiTeachersManage_Click);

            // tsmiTeachersAttendance
            this.tsmiTeachersAttendance.Name = "tsmiTeachersAttendance";
            this.tsmiTeachersAttendance.Size = new System.Drawing.Size(220, 28);
            this.tsmiTeachersAttendance.Text = "الحضور والانصراف";
            this.tsmiTeachersAttendance.Click += new System.EventHandler(this.tsmiTeachersAttendance_Click);

            // tsmiTeachersPayroll
            this.tsmiTeachersPayroll.Name = "tsmiTeachersPayroll";
            this.tsmiTeachersPayroll.Size = new System.Drawing.Size(220, 28);
            this.tsmiTeachersPayroll.Text = "العقود والرواتب";
            this.tsmiTeachersPayroll.Click += new System.EventHandler(this.tsmiTeachersPayroll_Click);

            // tsmiAcademic
            this.tsmiAcademic.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiSubjects,
                this.tsmiClasses,
                this.tsmiTimetable,
                this.tsmiGrades
            });
            this.tsmiAcademic.Name = "tsmiAcademic";
            this.tsmiAcademic.Size = new System.Drawing.Size(111, 29);
            this.tsmiAcademic.Text = "📚 الأكاديمي";

            // tsmiSubjects
            this.tsmiSubjects.Name = "tsmiSubjects";
            this.tsmiSubjects.Size = new System.Drawing.Size(220, 28);
            this.tsmiSubjects.Text = "المواد الدراسية";
            this.tsmiSubjects.Click += new System.EventHandler(this.tsmiSubjects_Click);

            // tsmiClasses
            this.tsmiClasses.Name = "tsmiClasses";
            this.tsmiClasses.Size = new System.Drawing.Size(220, 28);
            this.tsmiClasses.Text = "الفصول والقاعات";
            this.tsmiClasses.Click += new System.EventHandler(this.tsmiClasses_Click);

            // tsmiTimetable
            this.tsmiTimetable.Name = "tsmiTimetable";
            this.tsmiTimetable.Size = new System.Drawing.Size(220, 28);
            this.tsmiTimetable.Text = "الجداول الدراسية";
            this.tsmiTimetable.Click += new System.EventHandler(this.tsmiTimetable_Click);

            // tsmiGrades
            this.tsmiGrades.Name = "tsmiGrades";
            this.tsmiGrades.Size = new System.Drawing.Size(220, 28);
            this.tsmiGrades.Text = "إدخال الدرجات";
            this.tsmiGrades.Click += new System.EventHandler(this.tsmiGrades_Click);

            // tsmiAttendance
            this.tsmiAttendance.Name = "tsmiAttendance";
            this.tsmiAttendance.Size = new System.Drawing.Size(93, 29);
            this.tsmiAttendance.Text = "📝 الحضور";
            this.tsmiAttendance.Click += new System.EventHandler(this.tsmiAttendance_Click);

            // tsmiFinancial
            this.tsmiFinancial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiFees,
                this.tsmiVouchers,
                this.tsmiExpenses,
                this.تعريفرسومالصفوفToolStripMenuItem
            });
            this.tsmiFinancial.Name = "tsmiFinancial";
            this.tsmiFinancial.Size = new System.Drawing.Size(88, 29);
            this.tsmiFinancial.Text = "💰 المالية";

            // tsmiFees
            this.tsmiFees.Name = "tsmiFees";
            this.tsmiFees.Size = new System.Drawing.Size(240, 28);
            this.tsmiFees.Text = "الرسوم الدراسية";
            this.tsmiFees.Click += new System.EventHandler(this.tsmiFees_Click);

            // tsmiVouchers
            this.tsmiVouchers.Name = "tsmiVouchers";
            this.tsmiVouchers.Size = new System.Drawing.Size(240, 28);
            this.tsmiVouchers.Text = "السندات";
            this.tsmiVouchers.Click += new System.EventHandler(this.tsmiVouchers_Click);

            // tsmiExpenses
            this.tsmiExpenses.Name = "tsmiExpenses";
            this.tsmiExpenses.Size = new System.Drawing.Size(240, 28);
            this.tsmiExpenses.Text = "المصروفات";
            this.tsmiExpenses.Click += new System.EventHandler(this.tsmiExpenses_Click);

            // تعريفرسومالصفوفToolStripMenuItem
            this.تعريفرسومالصفوفToolStripMenuItem.Name = "تعريفرسومالصفوفToolStripMenuItem";
            this.تعريفرسومالصفوفToolStripMenuItem.Size = new System.Drawing.Size(240, 28);
            this.تعريفرسومالصفوفToolStripMenuItem.Text = "تعريف رسوم الصفوف";
            this.تعريفرسومالصفوفToolStripMenuItem.Click += new System.EventHandler(this.تعريفرسومالصفوفToolStripMenuItem_Click);

            // tsmiTransport
            this.tsmiTransport.Name = "tsmiTransport";
            this.tsmiTransport.Size = new System.Drawing.Size(81, 29);
            this.tsmiTransport.Text = "🚌 النقل";
            this.tsmiTransport.Click += new System.EventHandler(this.tsmiTransport_Click);

            // tsmiLibrary
            this.tsmiLibrary.Name = "tsmiLibrary";
            this.tsmiLibrary.Size = new System.Drawing.Size(95, 29);
            this.tsmiLibrary.Text = "📖 المكتبة";
            this.tsmiLibrary.Click += new System.EventHandler(this.tsmiLibrary_Click);

            // tsmiUsers
            this.tsmiUsers.Name = "tsmiUsers";
            this.tsmiUsers.Size = new System.Drawing.Size(135, 29);
            this.tsmiUsers.Text = "👤 المستخدمين";
            this.tsmiUsers.Click += new System.EventHandler(this.tsmiUsers_Click);

            // tsmiReports
            this.tsmiReports.Name = "tsmiReports";
            this.tsmiReports.Size = new System.Drawing.Size(90, 29);
            this.tsmiReports.Text = "📋 التقارير";
            this.tsmiReports.Click += new System.EventHandler(this.tsmiReports_Click);

            // tsmiLogout
            this.tsmiLogout.Name = "tsmiLogout";
            this.tsmiLogout.Visible = true;
            this.tsmiLogout.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsmiLogout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmiLogout.AutoSize = true;
            this.tsmiLogout.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tsmiLogout.Size = new System.Drawing.Size(75, 29);
            this.tsmiLogout.Text = "🚪 خروج";
            this.tsmiLogout.Click += new System.EventHandler(this.tsmiLogout_Click);

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.lblDateTime);
            this.panelTop.Controls.Add(this.lblUsername);
            this.panelTop.Controls.Add(this.lblSystemTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 41);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 76);
            this.panelTop.TabIndex = 1;

            // lblDateTime
            this.lblDateTime.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
            this.lblDateTime.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblDateTime.Location = new System.Drawing.Point(20, 26);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblDateTime.Size = new System.Drawing.Size(350, 26);
            this.lblDateTime.TabIndex = 0;
            this.lblDateTime.Text = "الخميس, 08/07/2026  10:30:45";

            // lblUsername
            this.lblUsername.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblUsername.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblUsername.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.lblUsername.Location = new System.Drawing.Point(488, 21);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.lblUsername.Size = new System.Drawing.Size(225, 34);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "👤 مدير النظام";

            // lblSystemTitle
            this.lblSystemTitle.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.lblSystemTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblSystemTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblSystemTitle.Location = new System.Drawing.Point(820, 18);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Size = new System.Drawing.Size(350, 40);
            this.lblSystemTitle.TabIndex = 2;
            this.lblSystemTitle.Text = "نظام إدارة المدرسة";

            // panelContent
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 117);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(18);
            this.panelContent.Size = new System.Drawing.Size(1200, 552);
            this.panelContent.TabIndex = 2;

            // statusStripMain
            this.statusStripMain.BackColor = System.Drawing.Color.White;
            this.statusStripMain.Font = new System.Drawing.Font("Tahoma", 9F);
            this.statusStripMain.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblDBStatus,
                this.lblOnlineUsers
            });
            this.statusStripMain.Location = new System.Drawing.Point(0, 669);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.statusStripMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStripMain.Size = new System.Drawing.Size(1200, 26);
            this.statusStripMain.TabIndex = 3;

            // lblDBStatus
            this.lblDBStatus.ForeColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.lblDBStatus.Name = "lblDBStatus";
            this.lblDBStatus.Size = new System.Drawing.Size(169, 20);
            this.lblDBStatus.Text = "🟢 متصل بقاعدة البيانات";

            // lblOnlineUsers
            this.lblOnlineUsers.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblOnlineUsers.Name = "lblOnlineUsers";
            this.lblOnlineUsers.Size = new System.Drawing.Size(159, 20);
            this.lblOnlineUsers.Text = "المستخدمين المتصلين: 1";

            // timerClock
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(1200, 695);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.menuStripMain);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.MainMenuStrip = this.menuStripMain;
            this.MinimumSize = new System.Drawing.Size(1100, 650);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "نظام إدارة المدرسة";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiDashboard;
        private System.Windows.Forms.ToolStripMenuItem tsmiStudents;
        private System.Windows.Forms.ToolStripMenuItem tsmiStudentsManage;
        private System.Windows.Forms.ToolStripMenuItem tsmiStudentsEnroll;
        private System.Windows.Forms.ToolStripMenuItem tsmiStudentsClasses;
        private System.Windows.Forms.ToolStripMenuItem tsmiTeachers;
        private System.Windows.Forms.ToolStripMenuItem tsmiTeachersManage;
        private System.Windows.Forms.ToolStripMenuItem tsmiTeachersAttendance;
        private System.Windows.Forms.ToolStripMenuItem tsmiTeachersPayroll;
        private System.Windows.Forms.ToolStripMenuItem tsmiAcademic;
        private System.Windows.Forms.ToolStripMenuItem tsmiSubjects;
        private System.Windows.Forms.ToolStripMenuItem tsmiClasses;
        private System.Windows.Forms.ToolStripMenuItem tsmiTimetable;
        private System.Windows.Forms.ToolStripMenuItem tsmiGrades;
        private System.Windows.Forms.ToolStripMenuItem tsmiAttendance;
        private System.Windows.Forms.ToolStripMenuItem tsmiFinancial;
        private System.Windows.Forms.ToolStripMenuItem tsmiFees;
        private System.Windows.Forms.ToolStripMenuItem tsmiVouchers;
        private System.Windows.Forms.ToolStripMenuItem tsmiExpenses;
        private System.Windows.Forms.ToolStripMenuItem تعريفرسومالصفوفToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiTransport;
        private System.Windows.Forms.ToolStripMenuItem tsmiLibrary;
        private System.Windows.Forms.ToolStripMenuItem tsmiUsers;
        private System.Windows.Forms.ToolStripMenuItem tsmiReports;
        private System.Windows.Forms.ToolStripMenuItem tsmiAuditLogs;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogout;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;

        private Krypton.Toolkit.KryptonPanel panelTop;
        private Krypton.Toolkit.KryptonLabel lblSystemTitle;
        private Krypton.Toolkit.KryptonLabel lblUsername;
        private Krypton.Toolkit.KryptonLabel lblDateTime;

        private Krypton.Toolkit.KryptonPanel panelContent;

        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel lblDBStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblOnlineUsers;

        private System.Windows.Forms.Timer timerClock;
    }
}
