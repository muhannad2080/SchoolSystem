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
            this.tsmiServices = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAuditLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReports = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new Krypton.Toolkit.KryptonPanel();
            this.tblHeader = new System.Windows.Forms.TableLayoutPanel();
            this.lblSystemTitle = new Krypton.Toolkit.KryptonLabel();
            this.flowHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDateTime = new Krypton.Toolkit.KryptonLabel();
            this.lblUsername = new Krypton.Toolkit.KryptonLabel();
            this.panelContent = new Krypton.Toolkit.KryptonPanel();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.lblDBStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOnlineUsers = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);

            this.menuStripMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.tblHeader.SuspendLayout();
            this.flowHeader.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();

            // menuStripMain
            this.menuStripMain.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.menuStripMain.CanOverflow = true;
            this.menuStripMain.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.menuStripMain.ForeColor = System.Drawing.Color.White;
            this.menuStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiDashboard,
                this.tsmiStudents,
                this.tsmiTeachers,
                this.tsmiAcademic,
                this.tsmiAttendance,
                this.tsmiFinancial,
                this.tsmiServices,
                this.tsmiAdmin,
                this.tsmiReports,
                this.tsmiLogout
            });
            this.menuStripMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.menuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStripMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStripMain.Size = new System.Drawing.Size(1200, 41);
            this.menuStripMain.TabIndex = 0;

            // tsmiDashboard
            this.tsmiDashboard.Name = "tsmiDashboard";
            this.tsmiDashboard.Size = new System.Drawing.Size(90, 26);
            this.tsmiDashboard.Text = "📊 الرئيسية";
            this.tsmiDashboard.Click += new System.EventHandler(this.tsmiDashboard_Click);

            // tsmiStudents
            this.tsmiStudents.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiStudentsManage,
                this.tsmiStudentsEnroll,
                this.tsmiStudentsClasses
            });
            this.tsmiStudents.Name = "tsmiStudents";
            this.tsmiStudents.Size = new System.Drawing.Size(82, 26);
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
            this.tsmiTeachers.Size = new System.Drawing.Size(95, 26);
            this.tsmiTeachers.Text = "👨‍🏫 المعلمون";

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
            this.tsmiAcademic.Size = new System.Drawing.Size(95, 26);
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
            this.tsmiAttendance.Size = new System.Drawing.Size(88, 26);
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
            this.tsmiFinancial.Size = new System.Drawing.Size(80, 26);
            this.tsmiFinancial.Text = "💰 المالية";

            // tsmiFees
            this.tsmiFees.Name = "tsmiFees";
            this.tsmiFees.Size = new System.Drawing.Size(220, 28);
            this.tsmiFees.Text = "الرسوم الدراسية";
            this.tsmiFees.Click += new System.EventHandler(this.tsmiFees_Click);

            // tsmiVouchers
            this.tsmiVouchers.Name = "tsmiVouchers";
            this.tsmiVouchers.Size = new System.Drawing.Size(220, 28);
            this.tsmiVouchers.Text = "السندات";
            this.tsmiVouchers.Click += new System.EventHandler(this.tsmiVouchers_Click);

            // tsmiExpenses
            this.tsmiExpenses.Name = "tsmiExpenses";
            this.tsmiExpenses.Size = new System.Drawing.Size(220, 28);
            this.tsmiExpenses.Text = "المصروفات";
            this.tsmiExpenses.Click += new System.EventHandler(this.tsmiExpenses_Click);

            // تعريفرسومالصفوفToolStripMenuItem
            this.تعريفرسومالصفوفToolStripMenuItem.Name = "تعريفرسومالصفوفToolStripMenuItem";
            this.تعريفرسومالصفوفToolStripMenuItem.Size = new System.Drawing.Size(220, 28);
            this.تعريفرسومالصفوفToolStripMenuItem.Text = "تعريف رسوم الصفوف";
            this.تعريفرسومالصفوفToolStripMenuItem.Click += new System.EventHandler(this.تعريفرسومالصفوفToolStripMenuItem_Click);

            // tsmiServices
            this.tsmiServices.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiTransport,
                this.tsmiLibrary
            });
            this.tsmiServices.Name = "tsmiServices";
            this.tsmiServices.Size = new System.Drawing.Size(82, 26);
            this.tsmiServices.Text = "🚌 الخدمات";

            // tsmiTransport
            this.tsmiTransport.Name = "tsmiTransport";
            this.tsmiTransport.Size = new System.Drawing.Size(220, 28);
            this.tsmiTransport.Text = "النقل المدرسي";
            this.tsmiTransport.Click += new System.EventHandler(this.tsmiTransport_Click);

            // tsmiLibrary
            this.tsmiLibrary.Name = "tsmiLibrary";
            this.tsmiLibrary.Size = new System.Drawing.Size(220, 28);
            this.tsmiLibrary.Text = "المكتبة";
            this.tsmiLibrary.Click += new System.EventHandler(this.tsmiLibrary_Click);

            // tsmiAdmin
            this.tsmiAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsmiUsers,
                this.tsmiAuditLogs,
                this.tsmiSettings
            });
            this.tsmiAdmin.Name = "tsmiAdmin";
            this.tsmiAdmin.Size = new System.Drawing.Size(82, 26);
            this.tsmiAdmin.Text = "⚙️ الإدارة";

            // tsmiUsers
            this.tsmiUsers.Name = "tsmiUsers";
            this.tsmiUsers.Size = new System.Drawing.Size(220, 28);
            this.tsmiUsers.Text = "المستخدمون";
            this.tsmiUsers.Click += new System.EventHandler(this.tsmiUsers_Click);

            // tsmiAuditLogs
            this.tsmiAuditLogs.Name = "tsmiAuditLogs";
            this.tsmiAuditLogs.Size = new System.Drawing.Size(220, 28);
            this.tsmiAuditLogs.Text = "سجل الأنشطة";
            this.tsmiAuditLogs.Click += new System.EventHandler(this.tsmiAuditLogs_Click);

            // tsmiSettings
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(220, 28);
            this.tsmiSettings.Text = "الإعدادات";
            this.tsmiSettings.Click += new System.EventHandler(this.tsmiSettings_Click);

            // tsmiReports
            this.tsmiReports.Name = "tsmiReports";
            this.tsmiReports.Size = new System.Drawing.Size(85, 26);
            this.tsmiReports.Text = "📋 التقارير";
            this.tsmiReports.Click += new System.EventHandler(this.tsmiReports_Click);

            // tsmiLogout
            this.tsmiLogout.Name = "tsmiLogout";
            this.tsmiLogout.Size = new System.Drawing.Size(70, 26);
            this.tsmiLogout.Text = "🚪 خروج";
            this.tsmiLogout.Click += new System.EventHandler(this.tsmiLogout_Click);

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.tblHeader);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 41);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.panelTop.Size = new System.Drawing.Size(1200, 64);
            this.panelTop.TabIndex = 1;

            // tblHeader
            this.tblHeader.ColumnCount = 3;
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tblHeader.Controls.Add(this.lblSystemTitle, 0, 0);
            this.tblHeader.Controls.Add(this.flowHeader, 2, 0);
            this.tblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblHeader.Location = new System.Drawing.Point(12, 0);
            this.tblHeader.Name = "tblHeader";
            this.tblHeader.RowCount = 1;
            this.tblHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHeader.Size = new System.Drawing.Size(1176, 64);
            this.tblHeader.TabIndex = 0;

            // lblSystemTitle
            this.lblSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSystemTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblSystemTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblSystemTitle.Location = new System.Drawing.Point(3, 0);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblSystemTitle.Size = new System.Drawing.Size(382, 64);
            this.lblSystemTitle.TabIndex = 0;
            this.lblSystemTitle.Text = "نظام إدارة المدرسة";

            // flowHeader
            this.flowHeader.Controls.Add(this.lblDateTime);
            this.flowHeader.Controls.Add(this.lblUsername);
            this.flowHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowHeader.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowHeader.Location = new System.Drawing.Point(791, 3);
            this.flowHeader.Name = "flowHeader";
            this.flowHeader.Padding = new System.Windows.Forms.Padding(0, 14, 0, 0);
            this.flowHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowHeader.Size = new System.Drawing.Size(382, 58);
            this.flowHeader.TabIndex = 1;

            // lblDateTime
            this.lblDateTime.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblDateTime.Location = new System.Drawing.Point(3, 17);
            this.lblDateTime.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(240, 24);
            this.lblDateTime.TabIndex = 0;
            this.lblDateTime.Text = "الخميس, 08/07/2026  10:30:45";

            // lblUsername
            this.lblUsername.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblUsername.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.lblUsername.Location = new System.Drawing.Point(258, 17);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(3);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Padding = new System.Windows.Forms.Padding(12, 2, 12, 2);
            this.lblUsername.Size = new System.Drawing.Size(121, 28);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "👤 مدير النظام";

            // panelContent
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 105);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(18);
            this.panelContent.Size = new System.Drawing.Size(1200, 564);
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
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "نظام إدارة المدرسة";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.flowHeader.ResumeLayout(false);
            this.flowHeader.PerformLayout();
            this.tblHeader.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem tsmiServices;
        private System.Windows.Forms.ToolStripMenuItem tsmiTransport;
        private System.Windows.Forms.ToolStripMenuItem tsmiLibrary;
        private System.Windows.Forms.ToolStripMenuItem tsmiAdmin;
        private System.Windows.Forms.ToolStripMenuItem tsmiUsers;
        private System.Windows.Forms.ToolStripMenuItem tsmiAuditLogs;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiReports;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogout;

        private Krypton.Toolkit.KryptonPanel panelTop;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private Krypton.Toolkit.KryptonLabel lblSystemTitle;
        private System.Windows.Forms.FlowLayoutPanel flowHeader;
        private Krypton.Toolkit.KryptonLabel lblDateTime;
        private Krypton.Toolkit.KryptonLabel lblUsername;

        private Krypton.Toolkit.KryptonPanel panelContent;

        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel lblDBStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblOnlineUsers;

        private System.Windows.Forms.Timer timerClock;
    }
}
