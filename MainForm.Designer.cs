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
            this.tsmiUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAuditLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReports = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new System.Windows.Forms.Panel();
            this.tblHeader = new System.Windows.Forms.TableLayoutPanel();
            this.tblTitle = new System.Windows.Forms.TableLayoutPanel();
            this.lblSystemTitle = new System.Windows.Forms.Label();
            this.lblSystemSubtitle = new System.Windows.Forms.Label();
            this.tblUserInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.lblDBStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOnlineUsers = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.menuStripMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.tblHeader.SuspendLayout();
            this.tblTitle.SuspendLayout();
            this.tblUserInfo.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.menuStripMain.CanOverflow = true;
            this.menuStripMain.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
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
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.menuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStripMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStripMain.Size = new System.Drawing.Size(1280, 33);
            this.menuStripMain.TabIndex = 0;
            // 
            // tsmiDashboard
            // 
            this.tsmiDashboard.Name = "tsmiDashboard";
            this.tsmiDashboard.Size = new System.Drawing.Size(110, 24);
            this.tsmiDashboard.Text = "📊 الرئيسية";
            this.tsmiDashboard.Click += new System.EventHandler(this.tsmiDashboard_Click);
            // 
            // tsmiStudents
            // 
            this.tsmiStudents.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStudentsManage,
            this.tsmiStudentsEnroll,
            this.tsmiStudentsClasses});
            this.tsmiStudents.Name = "tsmiStudents";
            this.tsmiStudents.Size = new System.Drawing.Size(95, 24);
            this.tsmiStudents.Text = "👨‍🎓 الطلاب";
            // 
            // tsmiStudentsManage
            // 
            this.tsmiStudentsManage.Name = "tsmiStudentsManage";
            this.tsmiStudentsManage.Size = new System.Drawing.Size(224, 26);
            this.tsmiStudentsManage.Text = "إدارة الطلاب";
            this.tsmiStudentsManage.Click += new System.EventHandler(this.tsmiStudentsManage_Click);
            // 
            // tsmiStudentsEnroll
            // 
            this.tsmiStudentsEnroll.Name = "tsmiStudentsEnroll";
            this.tsmiStudentsEnroll.Size = new System.Drawing.Size(224, 26);
            this.tsmiStudentsEnroll.Text = "التسجيل والقبول";
            this.tsmiStudentsEnroll.Click += new System.EventHandler(this.tsmiStudentsEnroll_Click);
            // 
            // tsmiStudentsClasses
            // 
            this.tsmiStudentsClasses.Name = "tsmiStudentsClasses";
            this.tsmiStudentsClasses.Size = new System.Drawing.Size(224, 26);
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
            this.tsmiTeachers.Size = new System.Drawing.Size(115, 24);
            this.tsmiTeachers.Text = "👨‍🏫 المعلمون";
            // 
            // tsmiTeachersManage
            // 
            this.tsmiTeachersManage.Name = "tsmiTeachersManage";
            this.tsmiTeachersManage.Size = new System.Drawing.Size(224, 26);
            this.tsmiTeachersManage.Text = "إدارة المعلمين";
            this.tsmiTeachersManage.Click += new System.EventHandler(this.tsmiTeachersManage_Click);
            // 
            // tsmiTeachersAttendance
            // 
            this.tsmiTeachersAttendance.Name = "tsmiTeachersAttendance";
            this.tsmiTeachersAttendance.Size = new System.Drawing.Size(224, 26);
            this.tsmiTeachersAttendance.Text = "الحضور والانصراف";
            this.tsmiTeachersAttendance.Click += new System.EventHandler(this.tsmiTeachersAttendance_Click);
            // 
            // tsmiTeachersPayroll
            // 
            this.tsmiTeachersPayroll.Name = "tsmiTeachersPayroll";
            this.tsmiTeachersPayroll.Size = new System.Drawing.Size(224, 26);
            this.tsmiTeachersPayroll.Text = "العقود والرواتب";
            this.tsmiTeachersPayroll.Click += new System.EventHandler(this.tsmiTeachersPayroll_Click);
            // 
            // tsmiAcademic
            // 
            this.tsmiAcademic.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSubjects,
            this.tsmiClasses,
            this.tsmiTimetable});
            this.tsmiAcademic.Name = "tsmiAcademic";
            this.tsmiAcademic.Size = new System.Drawing.Size(171, 24);
            this.tsmiAcademic.Text = "📚 الهيكل الأكاديمي";
            // 
            // tsmiSubjects
            // 
            this.tsmiSubjects.Name = "tsmiSubjects";
            this.tsmiSubjects.Size = new System.Drawing.Size(224, 26);
            this.tsmiSubjects.Text = "المواد الدراسية";
            this.tsmiSubjects.Click += new System.EventHandler(this.tsmiSubjects_Click);
            // 
            // tsmiClasses
            // 
            this.tsmiClasses.Name = "tsmiClasses";
            this.tsmiClasses.Size = new System.Drawing.Size(224, 26);
            this.tsmiClasses.Text = "الفصول والقاعات";
            this.tsmiClasses.Click += new System.EventHandler(this.tsmiClasses_Click);
            // 
            // tsmiTimetable
            // 
            this.tsmiTimetable.Name = "tsmiTimetable";
            this.tsmiTimetable.Size = new System.Drawing.Size(224, 26);
            this.tsmiTimetable.Text = "الجداول الدراسية";
            this.tsmiTimetable.Click += new System.EventHandler(this.tsmiTimetable_Click);
            // 
            // tsmiAttendanceGrades
            // 
            this.tsmiAttendanceGrades.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAttendance,
            this.tsmiGrades});
            this.tsmiAttendanceGrades.Name = "tsmiAttendanceGrades";
            this.tsmiAttendanceGrades.Size = new System.Drawing.Size(171, 24);
            this.tsmiAttendanceGrades.Text = "📝 الحضور والدرجات";
            // 
            // tsmiAttendance
            // 
            this.tsmiAttendance.Name = "tsmiAttendance";
            this.tsmiAttendance.Size = new System.Drawing.Size(224, 26);
            this.tsmiAttendance.Text = "حضور الطلاب";
            this.tsmiAttendance.Click += new System.EventHandler(this.tsmiAttendance_Click);
            // 
            // tsmiGrades
            // 
            this.tsmiGrades.Name = "tsmiGrades";
            this.tsmiGrades.Size = new System.Drawing.Size(224, 26);
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
            this.tsmiFinancial.Size = new System.Drawing.Size(92, 24);
            this.tsmiFinancial.Text = "💰 المالية";
            // 
            // tsmiFees
            // 
            this.tsmiFees.Name = "tsmiFees";
            this.tsmiFees.Size = new System.Drawing.Size(224, 26);
            this.tsmiFees.Text = "الرسوم الدراسية";
            this.tsmiFees.Click += new System.EventHandler(this.tsmiFees_Click);
            // 
            // تعريفرسومالصفوفToolStripMenuItem
            // 
            this.تعريفرسومالصفوفToolStripMenuItem.Name = "تعريفرسومالصفوفToolStripMenuItem";
            this.تعريفرسومالصفوفToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.تعريفرسومالصفوفToolStripMenuItem.Text = "خطط الرسوم";
            this.تعريفرسومالصفوفToolStripMenuItem.Click += new System.EventHandler(this.تعريفرسومالصفوفToolStripMenuItem_Click);
            // 
            // tsmiVouchers
            // 
            this.tsmiVouchers.Name = "tsmiVouchers";
            this.tsmiVouchers.Size = new System.Drawing.Size(224, 26);
            this.tsmiVouchers.Text = "السندات";
            this.tsmiVouchers.Click += new System.EventHandler(this.tsmiVouchers_Click);
            // 
            // tsmiExpenses
            // 
            this.tsmiExpenses.Name = "tsmiExpenses";
            this.tsmiExpenses.Size = new System.Drawing.Size(224, 26);
            this.tsmiExpenses.Text = "المصروفات";
            this.tsmiExpenses.Click += new System.EventHandler(this.tsmiExpenses_Click);
            // 
            // tsmiFinancialPayroll
            // 
            this.tsmiFinancialPayroll.Name = "tsmiFinancialPayroll";
            this.tsmiFinancialPayroll.Size = new System.Drawing.Size(224, 26);
            this.tsmiFinancialPayroll.Text = "الرواتب";
            this.tsmiFinancialPayroll.Click += new System.EventHandler(this.tsmiTeachersPayroll_Click);
            // 
            // tsmiServices
            // 
            this.tsmiServices.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTransport,
            this.tsmiLibrary});
            this.tsmiServices.Name = "tsmiServices";
            this.tsmiServices.Size = new System.Drawing.Size(108, 24);
            this.tsmiServices.Text = "🚌 الخدمات";
            // 
            // tsmiTransport
            // 
            this.tsmiTransport.Name = "tsmiTransport";
            this.tsmiTransport.Size = new System.Drawing.Size(224, 26);
            this.tsmiTransport.Text = "النقل المدرسي";
            this.tsmiTransport.Click += new System.EventHandler(this.tsmiTransport_Click);
            // 
            // tsmiLibrary
            // 
            this.tsmiLibrary.Name = "tsmiLibrary";
            this.tsmiLibrary.Size = new System.Drawing.Size(224, 26);
            this.tsmiLibrary.Text = "المكتبة";
            this.tsmiLibrary.Click += new System.EventHandler(this.tsmiLibrary_Click);
            // 
            // tsmiAdmin
            // 
            this.tsmiAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiUsers,
            this.tsmiAuditLogs,
            this.tsmiSettings});
            this.tsmiAdmin.Name = "tsmiAdmin";
            this.tsmiAdmin.Size = new System.Drawing.Size(92, 24);
            this.tsmiAdmin.Text = "⚙️ الإدارة";
            // 
            // tsmiUsers
            // 
            this.tsmiUsers.Name = "tsmiUsers";
            this.tsmiUsers.Size = new System.Drawing.Size(203, 26);
            this.tsmiUsers.Text = "المستخدمون";
            this.tsmiUsers.Click += new System.EventHandler(this.tsmiUsers_Click);
            // 
            // tsmiAuditLogs
            // 
            this.tsmiAuditLogs.Name = "tsmiAuditLogs";
            this.tsmiAuditLogs.Size = new System.Drawing.Size(203, 26);
            this.tsmiAuditLogs.Text = "سجل الأنشطة";
            this.tsmiAuditLogs.Click += new System.EventHandler(this.tsmiAuditLogs_Click);
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(203, 26);
            this.tsmiSettings.Text = "الإعدادات";
            this.tsmiSettings.Click += new System.EventHandler(this.tsmiSettings_Click);
            // 
            // tsmiReports
            // 
            this.tsmiReports.Name = "tsmiReports";
            this.tsmiReports.Size = new System.Drawing.Size(94, 24);
            this.tsmiReports.Text = "📋 التقارير";
            this.tsmiReports.Click += new System.EventHandler(this.tsmiReports_Click);
            // 
            // tsmiLogout
            // 
            this.tsmiLogout.Name = "tsmiLogout";
            this.tsmiLogout.Size = new System.Drawing.Size(83, 23);
            this.tsmiLogout.Text = "🚪 خروج";
            this.tsmiLogout.Click += new System.EventHandler(this.tsmiLogout_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.tblHeader);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 33);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(14, 6, 14, 6);
            this.panelTop.Size = new System.Drawing.Size(1280, 80);
            this.panelTop.TabIndex = 1;
            // 
            // tblHeader
            // 
            this.tblHeader.ColumnCount = 3;
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tblHeader.Controls.Add(this.tblTitle, 0, 0);
            this.tblHeader.Controls.Add(this.tblUserInfo, 2, 0);
            this.tblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblHeader.Location = new System.Drawing.Point(14, 6);
            this.tblHeader.Name = "tblHeader";
            this.tblHeader.RowCount = 1;
            this.tblHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHeader.Size = new System.Drawing.Size(1252, 68);
            this.tblHeader.TabIndex = 0;
            // 
            // tblTitle
            // 
            this.tblTitle.ColumnCount = 1;
            this.tblTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTitle.Controls.Add(this.lblSystemTitle, 0, 0);
            this.tblTitle.Controls.Add(this.lblSystemSubtitle, 0, 1);
            this.tblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTitle.Location = new System.Drawing.Point(689, 0);
            this.tblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.tblTitle.Name = "tblTitle";
            this.tblTitle.RowCount = 2;
            this.tblTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTitle.Size = new System.Drawing.Size(563, 68);
            this.tblTitle.TabIndex = 0;
            // 
            // lblSystemTitle
            // 
            this.lblSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSystemTitle.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblSystemTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblSystemTitle.Location = new System.Drawing.Point(12, 3);
            this.lblSystemTitle.Margin = new System.Windows.Forms.Padding(3, 3, 12, 0);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Size = new System.Drawing.Size(548, 31);
            this.lblSystemTitle.TabIndex = 0;
            this.lblSystemTitle.Text = "نظام إدارة المدرسة";
            this.lblSystemTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSystemSubtitle
            // 
            this.lblSystemSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSystemSubtitle.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblSystemSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSystemSubtitle.Location = new System.Drawing.Point(12, 37);
            this.lblSystemSubtitle.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.lblSystemSubtitle.Name = "lblSystemSubtitle";
            this.lblSystemSubtitle.Size = new System.Drawing.Size(548, 28);
            this.lblSystemSubtitle.TabIndex = 1;
            this.lblSystemSubtitle.Text = "الحل المتكامل لإدارة المؤسسات التعليمية";
            this.lblSystemSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.tblUserInfo.RowCount = 3;
            this.tblUserInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblUserInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblUserInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tblUserInfo.Size = new System.Drawing.Size(564, 68);
            this.tblUserInfo.TabIndex = 1;
            // 
            // lblUsername
            // 
            this.lblUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsername.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblUsername.Location = new System.Drawing.Point(3, 3);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(558, 25);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "👤 مدير النظام";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUserRole
            // 
            this.lblUserRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserRole.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblUserRole.Location = new System.Drawing.Point(3, 31);
            this.lblUserRole.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(558, 19);
            this.lblUserRole.TabIndex = 1;
            this.lblUserRole.Text = "الدور: مدير النظام";
            this.lblUserRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDateTime
            // 
            this.lblDateTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDateTime.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblDateTime.Location = new System.Drawing.Point(3, 53);
            this.lblDateTime.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(558, 15);
            this.lblDateTime.TabIndex = 2;
            this.lblDateTime.Text = "الخميس, 08/07/2026  10:30:45";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 113);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(18);
            this.panelContent.Size = new System.Drawing.Size(1280, 663);
            this.panelContent.TabIndex = 2;
            // 
            // statusStripMain
            // 
            this.statusStripMain.BackColor = System.Drawing.Color.White;
            this.statusStripMain.Font = new System.Drawing.Font("Tahoma", 9F);
            this.statusStripMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDBStatus,
            this.lblOnlineUsers,
            this.lblStatusUser});
            this.statusStripMain.Location = new System.Drawing.Point(0, 776);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.statusStripMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStripMain.Size = new System.Drawing.Size(1280, 24);
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
            this.lblOnlineUsers.Size = new System.Drawing.Size(945, 18);
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
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.menuStripMain);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.MainMenuStrip = this.menuStripMain;
            this.MinimumSize = new System.Drawing.Size(1024, 700);
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
            this.tblTitle.ResumeLayout(false);
            this.tblUserInfo.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem tsmiAttendanceGrades;
        private System.Windows.Forms.ToolStripMenuItem tsmiAttendance;
        private System.Windows.Forms.ToolStripMenuItem tsmiGrades;
        private System.Windows.Forms.ToolStripMenuItem tsmiFinancial;
        private System.Windows.Forms.ToolStripMenuItem tsmiFees;
        private System.Windows.Forms.ToolStripMenuItem tsmiVouchers;
        private System.Windows.Forms.ToolStripMenuItem tsmiExpenses;
        private System.Windows.Forms.ToolStripMenuItem تعريفرسومالصفوفToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiFinancialPayroll;
        private System.Windows.Forms.ToolStripMenuItem tsmiServices;
        private System.Windows.Forms.ToolStripMenuItem tsmiTransport;
        private System.Windows.Forms.ToolStripMenuItem tsmiLibrary;
        private System.Windows.Forms.ToolStripMenuItem tsmiAdmin;
        private System.Windows.Forms.ToolStripMenuItem tsmiUsers;
        private System.Windows.Forms.ToolStripMenuItem tsmiAuditLogs;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiReports;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogout;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.TableLayoutPanel tblTitle;
        private System.Windows.Forms.Label lblSystemTitle;
        private System.Windows.Forms.Label lblSystemSubtitle;
        private System.Windows.Forms.TableLayoutPanel tblUserInfo;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Label lblDateTime;

        private System.Windows.Forms.Panel panelContent;

        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel lblDBStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblOnlineUsers;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusUser;

        private System.Windows.Forms.Timer timerClock;
    }
}
