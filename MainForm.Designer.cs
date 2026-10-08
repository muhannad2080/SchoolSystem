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
            this.tsmiSections = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTimetable = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAttendanceGrades = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAttendance = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGrades = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFinancial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFees = new System.Windows.Forms.ToolStripMenuItem();
            this.تعريفرسومالصفوفToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVouchers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExpenses = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFinancialPayroll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServices = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAnnualClosing = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAuditLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReports = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new System.Windows.Forms.Panel();
            this.tblHeader = new System.Windows.Forms.TableLayoutPanel();
            this.tblUserInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.tblTitle = new System.Windows.Forms.TableLayoutPanel();
            this.lblSystemTitle = new System.Windows.Forms.Label();
            this.lblSystemSubtitle = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelScreenHeader = new System.Windows.Forms.Panel();
            this.lblScreenTitle = new System.Windows.Forms.Label();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.lblDBStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOnlineUsers = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.menuStripMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.tblHeader.SuspendLayout();
            this.tblUserInfo.SuspendLayout();
            this.tblTitle.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.menuStripMain.CanOverflow = true;
            this.menuStripMain.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStripMain.ForeColor = System.Drawing.Color.White;
            this.menuStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDashboard,
            this.tsmiStudents,
            this.tsmiTeachers,
            this.tsmiAcademic,
            this.tsmiAttendanceGrades,
            this.tsmiFinancial,
            this.tsmiServices,
            this.tsmiAdmin,
            this.tsmiReports,
            this.tsmiLogout});
            this.menuStripMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.menuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStripMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStripMain.Size = new System.Drawing.Size(1280, 33);
            this.menuStripMain.TabIndex = 0;
            // 
            // tsmiDashboard
            // 
            this.tsmiDashboard.Name = "tsmiDashboard";
            this.tsmiDashboard.Size = new System.Drawing.Size(86, 24);
            this.tsmiDashboard.Text = "الرئيسية";
            this.tsmiDashboard.Click += new System.EventHandler(this.tsmiDashboard_Click);
            // 
            // tsmiStudents
            // 
            this.tsmiStudents.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStudentsManage,
            this.tsmiStudentsEnroll,
            this.tsmiStudentsClasses});
            this.tsmiStudents.Name = "tsmiStudents";
            this.tsmiStudents.Size = new System.Drawing.Size(73, 24);
            this.tsmiStudents.Text = "الطلاب";
            // 
            // tsmiStudentsManage
            // 
            this.tsmiStudentsManage.Name = "tsmiStudentsManage";
            this.tsmiStudentsManage.Size = new System.Drawing.Size(214, 26);
            this.tsmiStudentsManage.Text = "إدارة الطلاب";
            this.tsmiStudentsManage.Click += new System.EventHandler(this.tsmiStudentsManage_Click);
            // 
            // tsmiStudentsEnroll
            // 
            this.tsmiStudentsEnroll.Name = "tsmiStudentsEnroll";
            this.tsmiStudentsEnroll.Size = new System.Drawing.Size(214, 26);
            this.tsmiStudentsEnroll.Text = "التسجيل والقبول";
            this.tsmiStudentsEnroll.Click += new System.EventHandler(this.tsmiStudentsEnroll_Click);
            // 
            // tsmiStudentsClasses
            // 
            this.tsmiStudentsClasses.Name = "tsmiStudentsClasses";
            this.tsmiStudentsClasses.Size = new System.Drawing.Size(214, 26);
            this.tsmiStudentsClasses.Text = "توزيع الفصول";
            this.tsmiStudentsClasses.Click += new System.EventHandler(this.tsmiStudentsClasses_Click);
            // 
            // tsmiTeachers
            // 
            this.tsmiTeachers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTeachersManage,
            this.tsmiTeachersAttendance,
            this.tsmiTeachersPayroll});
            this.tsmiTeachers.Name = "tsmiTeachers";
            this.tsmiTeachers.Size = new System.Drawing.Size(90, 24);
            this.tsmiTeachers.Text = "المعلمون";
            // 
            // tsmiTeachersManage
            // 
            this.tsmiTeachersManage.Name = "tsmiTeachersManage";
            this.tsmiTeachersManage.Size = new System.Drawing.Size(222, 26);
            this.tsmiTeachersManage.Text = "إدارة المعلمين";
            this.tsmiTeachersManage.Click += new System.EventHandler(this.tsmiTeachersManage_Click);
            // 
            // tsmiTeachersAttendance
            // 
            this.tsmiTeachersAttendance.Name = "tsmiTeachersAttendance";
            this.tsmiTeachersAttendance.Size = new System.Drawing.Size(222, 26);
            this.tsmiTeachersAttendance.Text = "الحضور والانصراف";
            this.tsmiTeachersAttendance.Click += new System.EventHandler(this.tsmiTeachersAttendance_Click);
            // 
            // tsmiTeachersPayroll
            // 
            this.tsmiTeachersPayroll.Name = "tsmiTeachersPayroll";
            this.tsmiTeachersPayroll.Size = new System.Drawing.Size(222, 26);
            this.tsmiTeachersPayroll.Text = "العقود الوظيفية";
            this.tsmiTeachersPayroll.Click += new System.EventHandler(this.tsmiTeachersPayroll_Click);
            // 
            // tsmiAcademic
            // 
            this.tsmiAcademic.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSubjects,
            this.tsmiClasses,
            this.tsmiSections,
            this.tsmiTimetable});
            this.tsmiAcademic.Name = "tsmiAcademic";
            this.tsmiAcademic.Size = new System.Drawing.Size(147, 24);
            this.tsmiAcademic.Text = "الهيكل الأكاديمي";
            // 
            // tsmiSubjects
            // 
            this.tsmiSubjects.Name = "tsmiSubjects";
            this.tsmiSubjects.Size = new System.Drawing.Size(215, 26);
            this.tsmiSubjects.Text = "المواد الدراسية";
            this.tsmiSubjects.Click += new System.EventHandler(this.tsmiSubjects_Click);
            // 
            // tsmiClasses
            // 
            this.tsmiClasses.Name = "tsmiClasses";
            this.tsmiClasses.Size = new System.Drawing.Size(215, 26);
            this.tsmiClasses.Text = "الفصول والقاعات";
            this.tsmiClasses.Click += new System.EventHandler(this.tsmiClasses_Click);
            // 
            // tsmiSections
            // 
            this.tsmiSections.Name = "tsmiSections";
            this.tsmiSections.Size = new System.Drawing.Size(215, 26);
            this.tsmiSections.Text = "الشعب الدراسية";
            this.tsmiSections.Click += new System.EventHandler(this.tsmiSections_Click);
            // 
            // tsmiTimetable
            // 
            this.tsmiTimetable.Name = "tsmiTimetable";
            this.tsmiTimetable.Size = new System.Drawing.Size(215, 26);
            this.tsmiTimetable.Text = "الجداول الدراسية";
            this.tsmiTimetable.Click += new System.EventHandler(this.tsmiTimetable_Click);
            // 
            // tsmiAttendanceGrades
            // 
            this.tsmiAttendanceGrades.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAttendance,
            this.tsmiGrades});
            this.tsmiAttendanceGrades.Name = "tsmiAttendanceGrades";
            this.tsmiAttendanceGrades.Size = new System.Drawing.Size(146, 24);
            this.tsmiAttendanceGrades.Text = "الحضور والدرجات";
            // 
            // tsmiAttendance
            // 
            this.tsmiAttendance.Name = "tsmiAttendance";
            this.tsmiAttendance.Size = new System.Drawing.Size(196, 26);
            this.tsmiAttendance.Text = "حضور الطلاب";
            this.tsmiAttendance.Click += new System.EventHandler(this.tsmiAttendance_Click);
            // 
            // tsmiGrades
            // 
            this.tsmiGrades.Name = "tsmiGrades";
            this.tsmiGrades.Size = new System.Drawing.Size(196, 26);
            this.tsmiGrades.Text = "إدخال الدرجات";
            this.tsmiGrades.Click += new System.EventHandler(this.tsmiGrades_Click);
            // 
            // tsmiFinancial
            // 
            this.tsmiFinancial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFees,
            this.تعريفرسومالصفوفToolStripMenuItem,
            this.tsmiVouchers,
            this.tsmiExpenses,
            this.tsmiFinancialPayroll});
            this.tsmiFinancial.Name = "tsmiFinancial";
            this.tsmiFinancial.Size = new System.Drawing.Size(70, 24);
            this.tsmiFinancial.Text = "المالية";
            // 
            // tsmiFees
            // 
            this.tsmiFees.Name = "tsmiFees";
            this.tsmiFees.Size = new System.Drawing.Size(216, 26);
            this.tsmiFees.Text = "الرسوم الدراسية";
            this.tsmiFees.Click += new System.EventHandler(this.tsmiFees_Click);
            // 
            // تعريفرسومالصفوفToolStripMenuItem
            // 
            this.تعريفرسومالصفوفToolStripMenuItem.Name = "تعريفرسومالصفوفToolStripMenuItem";
            this.تعريفرسومالصفوفToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.تعريفرسومالصفوفToolStripMenuItem.Text = "خطط الرسوم";
            this.تعريفرسومالصفوفToolStripMenuItem.Click += new System.EventHandler(this.تعريفرسومالصفوفToolStripMenuItem_Click);
            // 
            // tsmiVouchers
            // 
            this.tsmiVouchers.Name = "tsmiVouchers";
            this.tsmiVouchers.Size = new System.Drawing.Size(216, 26);
            this.tsmiVouchers.Text = "السندات";
            this.tsmiVouchers.Click += new System.EventHandler(this.tsmiVouchers_Click);
            // 
            // tsmiExpenses
            // 
            this.tsmiExpenses.Name = "tsmiExpenses";
            this.tsmiExpenses.Size = new System.Drawing.Size(216, 26);
            this.tsmiExpenses.Text = "المصروفات";
            this.tsmiExpenses.Click += new System.EventHandler(this.tsmiExpenses_Click);
            // 
            // tsmiFinancialPayroll
            // 
            this.tsmiFinancialPayroll.Name = "tsmiFinancialPayroll";
            this.tsmiFinancialPayroll.Size = new System.Drawing.Size(216, 26);
            this.tsmiFinancialPayroll.Text = "الرواتب الشهرية";
            this.tsmiFinancialPayroll.Click += new System.EventHandler(this.tsmiFinancialPayroll_Click);
            // 
            // tsmiServices
            // 
            this.tsmiServices.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTransport,
            this.tsmiLibrary});
            this.tsmiServices.Name = "tsmiServices";
            this.tsmiServices.Size = new System.Drawing.Size(83, 24);
            this.tsmiServices.Text = "الخدمات";
            // 
            // tsmiTransport
            // 
            this.tsmiTransport.Name = "tsmiTransport";
            this.tsmiTransport.Size = new System.Drawing.Size(206, 26);
            this.tsmiTransport.Text = "النقل المدرسي";
            this.tsmiTransport.Click += new System.EventHandler(this.tsmiTransport_Click);
            // 
            // tsmiLibrary
            // 
            this.tsmiLibrary.Name = "tsmiLibrary";
            this.tsmiLibrary.Size = new System.Drawing.Size(206, 26);
            this.tsmiLibrary.Text = "المكتبة";
            this.tsmiLibrary.Click += new System.EventHandler(this.tsmiLibrary_Click);
            // 
            // tsmiAdmin
            // 
            this.tsmiAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAnnualClosing,
            this.tsmiUsers,
            this.tsmiAuditLogs,
            this.tsmiSettings});
            this.tsmiAdmin.Name = "tsmiAdmin";
            this.tsmiAdmin.Size = new System.Drawing.Size(67, 24);
            this.tsmiAdmin.Text = "الإدارة";
            // 
            // tsmiAnnualClosing
            // 
            this.tsmiAnnualClosing.Name = "tsmiAnnualClosing";
            this.tsmiAnnualClosing.Size = new System.Drawing.Size(276, 26);
            this.tsmiAnnualClosing.Text = "الإغلاق السنوي والترحيل";
            this.tsmiAnnualClosing.Click += new System.EventHandler(this.tsmiAnnualClosing_Click);
            // 
            // tsmiUsers
            // 
            this.tsmiUsers.Name = "tsmiUsers";
            this.tsmiUsers.Size = new System.Drawing.Size(276, 26);
            this.tsmiUsers.Text = "المستخدمون";
            this.tsmiUsers.Click += new System.EventHandler(this.tsmiUsers_Click);
            // 
            // tsmiAuditLogs
            // 
            this.tsmiAuditLogs.Name = "tsmiAuditLogs";
            this.tsmiAuditLogs.Size = new System.Drawing.Size(276, 26);
            this.tsmiAuditLogs.Text = "سجل الأنشطة";
            this.tsmiAuditLogs.Click += new System.EventHandler(this.tsmiAuditLogs_Click);
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(276, 26);
            this.tsmiSettings.Text = "الإعدادات";
            this.tsmiSettings.Click += new System.EventHandler(this.tsmiSettings_Click);
            // 
            // tsmiReports
            // 
            this.tsmiReports.Name = "tsmiReports";
            this.tsmiReports.Size = new System.Drawing.Size(72, 24);
            this.tsmiReports.Text = "التقارير";
            this.tsmiReports.Click += new System.EventHandler(this.tsmiReports_Click);
            // 
            // tsmiLogout
            // 
            this.tsmiLogout.Name = "tsmiLogout";
            this.tsmiLogout.Size = new System.Drawing.Size(130, 24);
            this.tsmiLogout.Text = "تسجيل الخروج";
            this.tsmiLogout.Click += new System.EventHandler(this.tsmiLogout_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(255)))));
            this.panelTop.Controls.Add(this.tblHeader);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 33);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(18, 8, 18, 8);
            this.panelTop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panelTop.Size = new System.Drawing.Size(1280, 96);
            this.panelTop.TabIndex = 1;
            // 
            // tblHeader
            // 
            this.tblHeader.ColumnCount = 2;
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.tblHeader.Controls.Add(this.tblUserInfo, 0, 0);
            this.tblHeader.Controls.Add(this.tblTitle, 1, 0);
            this.tblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblHeader.Location = new System.Drawing.Point(18, 6);
            this.tblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tblHeader.Name = "tblHeader";
            this.tblHeader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tblHeader.RowCount = 1;
            this.tblHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHeader.Size = new System.Drawing.Size(1244, 71);
            this.tblHeader.TabIndex = 0;
            // 
            // tblUserInfo
            // 
            this.tblUserInfo.ColumnCount = 1;
            this.tblUserInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblUserInfo.Controls.Add(this.lblUsername, 0, 0);
            this.tblUserInfo.Controls.Add(this.lblUserRole, 0, 1);
            this.tblUserInfo.Controls.Add(this.lblDateTime, 0, 2);
            this.tblUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblUserInfo.Location = new System.Drawing.Point(0, 0);
            this.tblUserInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tblUserInfo.Name = "tblUserInfo";
            this.tblUserInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tblUserInfo.RowCount = 3;
            this.tblUserInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tblUserInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tblUserInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tblUserInfo.Size = new System.Drawing.Size(522, 71);
            this.tblUserInfo.TabIndex = 1;
            // 
            // lblUsername
            // 
            this.lblUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsername.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblUsername.Location = new System.Drawing.Point(3, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(516, 26);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "المستخدم الحالي";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUserRole
            // 
            this.lblUserRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserRole.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblUserRole.Location = new System.Drawing.Point(3, 26);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(516, 19);
            this.lblUserRole.TabIndex = 1;
            this.lblUserRole.Text = "الدور: مدير النظام";
            this.lblUserRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDateTime
            // 
            this.lblDateTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDateTime.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblDateTime.Location = new System.Drawing.Point(3, 45);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(516, 26);
            this.lblDateTime.TabIndex = 2;
            this.lblDateTime.Text = "التاريخ والوقت";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tblTitle
            // 
            this.tblTitle.ColumnCount = 1;
            this.tblTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTitle.Controls.Add(this.lblSystemTitle, 0, 0);
            this.tblTitle.Controls.Add(this.lblSystemSubtitle, 0, 1);
            this.tblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTitle.Location = new System.Drawing.Point(522, 0);
            this.tblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.tblTitle.Name = "tblTitle";
            this.tblTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tblTitle.RowCount = 2;
            this.tblTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57F));
            this.tblTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.tblTitle.Size = new System.Drawing.Size(722, 71);
            this.tblTitle.TabIndex = 0;
            // 
            // lblSystemTitle
            // 
            this.lblSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSystemTitle.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(50)))), ((int)(((byte)(79)))));
            this.lblSystemTitle.Location = new System.Drawing.Point(8, 0);
            this.lblSystemTitle.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Size = new System.Drawing.Size(706, 40);
            this.lblSystemTitle.TabIndex = 0;
            this.lblSystemTitle.Text = "نظام إدارة المدرسة";
            this.lblSystemTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSystemSubtitle
            // 
            this.lblSystemSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSystemSubtitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSystemSubtitle.Location = new System.Drawing.Point(8, 40);
            this.lblSystemSubtitle.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblSystemSubtitle.Name = "lblSystemSubtitle";
            this.lblSystemSubtitle.Size = new System.Drawing.Size(706, 31);
            this.lblSystemSubtitle.TabIndex = 1;
            this.lblSystemSubtitle.Text = "بيانات المدرسة والهوية المؤسسية";
            this.lblSystemSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 116);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(18);
            this.panelContent.Size = new System.Drawing.Size(1280, 660);
            this.panelContent.TabIndex = 2;
            // 
            // panelScreenHeader
            // 
            this.panelScreenHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.panelScreenHeader.Controls.Add(this.lblScreenTitle);
            this.panelScreenHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScreenHeader.Location = new System.Drawing.Point(0, 116);
            this.panelScreenHeader.Name = "panelScreenHeader";
            this.panelScreenHeader.Padding = new System.Windows.Forms.Padding(18, 0, 18, 0);
            this.panelScreenHeader.Size = new System.Drawing.Size(1280, 54);
            this.panelScreenHeader.TabIndex = 3;
            this.panelScreenHeader.Visible = false;
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScreenTitle.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle.Location = new System.Drawing.Point(18, 0);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblScreenTitle.Size = new System.Drawing.Size(1244, 54);
            this.lblScreenTitle.TabIndex = 0;
            this.lblScreenTitle.Text = "الشاشة الحالية";
            this.lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStripMain
            // 
            this.statusStripMain.BackColor = System.Drawing.Color.White;
            this.statusStripMain.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStripMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDBStatus,
            this.lblOnlineUsers,
            this.lblStatusUser});
            this.statusStripMain.Location = new System.Drawing.Point(0, 776);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            this.statusStripMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStripMain.Size = new System.Drawing.Size(1280, 24);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 3;
            // 
            // lblDBStatus
            // 
            this.lblDBStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblDBStatus.Name = "lblDBStatus";
            this.lblDBStatus.Size = new System.Drawing.Size(160, 18);
            this.lblDBStatus.Text = "🟢 متصل بقاعدة البيانات";
            // 
            // lblOnlineUsers
            // 
            this.lblOnlineUsers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblOnlineUsers.Name = "lblOnlineUsers";
            this.lblOnlineUsers.Size = new System.Drawing.Size(937, 18);
            this.lblOnlineUsers.Spring = true;
            this.lblOnlineUsers.Text = "المستخدمين المتصلين: 1";
            // 
            // lblStatusUser
            // 
            this.lblStatusUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblStatusUser.Name = "lblStatusUser";
            this.lblStatusUser.Size = new System.Drawing.Size(151, 18);
            this.lblStatusUser.Text = "المستخدم: مدير النظام";
            // 
            // timerClock
            // 
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelScreenHeader);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.menuStripMain);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStripMain;
            this.MinimumSize = new System.Drawing.Size(1100, 720);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "نظام إدارة المدرسة";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.tblHeader.ResumeLayout(false);
            this.tblUserInfo.ResumeLayout(false);
            this.tblTitle.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // ================================================================
        // MENU
        // ================================================================

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
        private System.Windows.Forms.ToolStripMenuItem tsmiSections;
        private System.Windows.Forms.ToolStripMenuItem tsmiTimetable;

        private System.Windows.Forms.ToolStripMenuItem tsmiAttendanceGrades;
        private System.Windows.Forms.ToolStripMenuItem tsmiAttendance;
        private System.Windows.Forms.ToolStripMenuItem tsmiGrades;

        private System.Windows.Forms.ToolStripMenuItem tsmiFinancial;
        private System.Windows.Forms.ToolStripMenuItem tsmiFees;
        private System.Windows.Forms.ToolStripMenuItem تعريفرسومالصفوفToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiVouchers;
        private System.Windows.Forms.ToolStripMenuItem tsmiExpenses;
        private System.Windows.Forms.ToolStripMenuItem tsmiFinancialPayroll;

        private System.Windows.Forms.ToolStripMenuItem tsmiServices;
        private System.Windows.Forms.ToolStripMenuItem tsmiTransport;
        private System.Windows.Forms.ToolStripMenuItem tsmiLibrary;

        private System.Windows.Forms.ToolStripMenuItem tsmiAdmin;
        private System.Windows.Forms.ToolStripMenuItem tsmiAnnualClosing;
        private System.Windows.Forms.ToolStripMenuItem tsmiUsers;
        private System.Windows.Forms.ToolStripMenuItem tsmiAuditLogs;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;

        private System.Windows.Forms.ToolStripMenuItem tsmiReports;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogout;

        // ================================================================
        // HEADER
        // ================================================================

        private System.Windows.Forms.Panel panelTop;

        private System.Windows.Forms.TableLayoutPanel tblHeader;

        private System.Windows.Forms.TableLayoutPanel tblTitle;

        private System.Windows.Forms.Label lblSystemTitle;

        private System.Windows.Forms.Label lblSystemSubtitle;

        private System.Windows.Forms.TableLayoutPanel tblUserInfo;

        private System.Windows.Forms.Label lblUsername;

        private System.Windows.Forms.Label lblUserRole;

        private System.Windows.Forms.Label lblDateTime;

        // ================================================================
        // CONTENT
        // ================================================================

        private System.Windows.Forms.Panel panelContent;

        private System.Windows.Forms.Panel panelScreenHeader;

        private System.Windows.Forms.Label lblScreenTitle;

        // ================================================================
        // STATUS
        // ================================================================

        private System.Windows.Forms.StatusStrip statusStripMain;

        private System.Windows.Forms.ToolStripStatusLabel lblDBStatus;

        private System.Windows.Forms.ToolStripStatusLabel lblOnlineUsers;

        private System.Windows.Forms.ToolStripStatusLabel lblStatusUser;

        private System.Windows.Forms.Timer timerClock;
    }
}
