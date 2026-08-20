using SchoolSystem.UI;
using SchoolSystem.Security;
using SchoolSystem.Services;
using SchoolSystem.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.UI.Students;

namespace SchoolSystem
{
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; private set; }
        private readonly AuditLogService auditLogService = new AuditLogService();

        public MainForm()
        {
            InitializeComponent();
            FormClosed += MainForm_FormClosed;
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);

            Instance = this;

            ApplyModernMenuStyle();

            UpdateCurrentUserLabel();

            timerClock.Start();

            LoadWelcomeScreen();

            ApplyCurrentUserPermissions();
        }

        private void ApplyModernMenuStyle()
        {
            Color mainColor = UIHelper.PrimaryColor;
            Color accentColor = UIHelper.AccentColor;
            Color contentBack = UIHelper.BackgroundColor;

            menuStripMain.BackColor = mainColor;
            menuStripMain.ForeColor = Color.White;
            menuStripMain.Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize, FontStyle.Bold);
            menuStripMain.Padding = new Padding(14, 6, 14, 6);
            menuStripMain.CanOverflow = true;
            menuStripMain.AutoSize = false;
            menuStripMain.GripStyle = ToolStripGripStyle.Hidden;
            menuStripMain.ShowItemToolTips = true;
            menuStripMain.Renderer = new CustomMenuRenderer();

            // عناصر حيوية لا يمكن إخفاؤها في زر الفائض عند تصغير النافذة.
            tsmiDashboard.Overflow = ToolStripItemOverflow.Never;
            tsmiLogout.Overflow = ToolStripItemOverflow.Never;

            foreach (ToolStripItem item in menuStripMain.Items)
            {
                item.BackColor = mainColor;
                item.ForeColor = Color.White;
                item.Margin = new Padding(3, 0, 3, 0);
                item.Padding = new Padding(9, 0, 9, 0);
                item.AutoSize = true;
                item.Height = 34;

                if (item is ToolStripMenuItem menuItem)
                    StyleDropDownItems(menuItem);
            }

            // يبقى تسجيل الخروج منفصلًا في الطرف المقابل للقائمة مع هامش واضح.
            // يتم استخدام Alignment بدل Location حتى يحافظ MenuStrip على التخطيط عند تغيير الحجم.
            tsmiLogout.Alignment = ToolStripItemAlignment.Left;
            tsmiLogout.Margin = new Padding(18, 0, 3, 0);
            tsmiLogout.Padding = new Padding(12, 0, 12, 0);
            tsmiLogout.ToolTipText = "إنهاء الجلسة الحالية والعودة إلى شاشة الدخول";

            // الترويسة: اسم النظام من اليمين وبيانات المستخدم والوقت من اليسار.
            panelTop.BackColor = UIHelper.SurfaceElevatedColor;
            panelTop.Height = 92;

            lblSystemTitle.Font = new Font(UIHelper.FontFamily, UIHelper.TitleFontSize + 4F, FontStyle.Bold);
            lblSystemTitle.ForeColor = UIHelper.PrimaryColor;
            lblSystemTitle.Text = "نظام إدارة المدرسة";
            lblSystemTitle.Margin = new Padding(3, 0, 12, 0);

            lblSystemSubtitle.Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize);
            lblSystemSubtitle.ForeColor = UIHelper.MutedTextColor;
            lblSystemSubtitle.Text = "الحل المتكامل لإدارة المؤسسات التعليمية";
            lblSystemSubtitle.Margin = new Padding(3, 0, 12, 0);

            lblUsername.ForeColor = accentColor;
            lblUsername.Font = new Font(UIHelper.FontFamily, UIHelper.TitleFontSize, FontStyle.Bold);
            lblUsername.Margin = new Padding(3, 0, 3, 0);

            lblUserRole.ForeColor = UIHelper.MutedTextColor;
            lblUserRole.Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize);
            lblUserRole.Margin = new Padding(3, 0, 3, 0);

            lblDateTime.ForeColor = UIHelper.MutedTextColor;
            lblDateTime.Font = new Font(UIHelper.FontFamily, UIHelper.CaptionFontSize);
            lblDateTime.Margin = new Padding(3, 0, 3, 0);

            panelContent.BackColor = contentBack;
            panelContent.Padding = new Padding(20);

            statusStripMain.BackColor = UIHelper.SurfaceElevatedColor;
            statusStripMain.ForeColor = UIHelper.MutedTextColor;
            statusStripMain.Font = new Font(UIHelper.FontFamily, UIHelper.CaptionFontSize);
            statusStripMain.Padding = new Padding(16, 0, 16, 0);

            lblDBStatus.ForeColor = UIHelper.SuccessColor;
            lblOnlineUsers.ForeColor = UIHelper.MutedTextColor;
            lblStatusUser.ForeColor = UIHelper.TextColor;
            lblStatusUser.Font = new Font(UIHelper.FontFamily, UIHelper.CaptionFontSize, FontStyle.Bold);

            this.BackColor = contentBack;
            this.MinimumSize = new Size(1100, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "نظام إدارة المدرسة";

            lblDateTime.Text = FormatNow();
        }

        private void UpdateCurrentUserLabel()
        {
            if (!CurrentUser.IsLoggedIn || CurrentUser.User == null)
            {
                lblUsername.Text = "زائر";
                lblUserRole.Text = string.Empty;
                lblStatusUser.Text = "غير مسجل";
                return;
            }

            string displayName = CurrentUser.User.FullName;

            if (string.IsNullOrWhiteSpace(displayName))
                displayName = CurrentUser.User.UserName;

            if (string.IsNullOrWhiteSpace(displayName))
                displayName = "مستخدم";

            lblUsername.Text = "👤 " + displayName;
            lblStatusUser.Text = "المستخدم: " + displayName;

            string role = CurrentUser.User.RoleName;
            lblUserRole.Text = string.IsNullOrWhiteSpace(role)
                ? string.Empty
                : "الدور: " + role;
        }

        private void StyleDropDownItems(ToolStripMenuItem parent)
        {
            parent.DropDown.BackColor = UIHelper.SurfaceElevatedColor;
            parent.DropDown.RightToLeft = RightToLeft.Yes;
            parent.DropDown.Padding = new Padding(4);

            foreach (ToolStripItem item in parent.DropDownItems)
            {
                item.BackColor = UIHelper.SurfaceElevatedColor;
                item.ForeColor = UIHelper.TextColor;
                item.Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize);
                item.Padding = new Padding(8, 4, 8, 4);

                if (item is ToolStripMenuItem child)
                    StyleDropDownItems(child);
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = FormatNow();
        }

        private static string FormatNow()
        {
            return DateTime.Now.ToString("dddd، dd/MM/yyyy  HH:mm:ss");
        }

        private void LoadWelcomeScreen()
        {
            ClearPanelContent();

            var welcome = new WelcomeScreen();
            welcome.SystemName = "أهلاً بك في نظام إدارة المدرسة";
            welcome.Dock = DockStyle.Fill;

            panelContent.Controls.Add(welcome);
        }

        private void ClearPanelContent()
        {
            while (panelContent.Controls.Count > 0)
            {
                Control existing = panelContent.Controls[0];
                panelContent.Controls.RemoveAt(0);
                existing.Dispose();
            }
        }

        public void LoadUserControl(UserControl uc)
        {
            try
            {
                UIHelper.ApplyStyle(uc);
                ClearPanelContent();

                uc.Dock = DockStyle.Fill;

                panelContent.Controls.Add(uc);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل الواجهة", ex);
                ShowLoadError("تعذر تحميل الواجهة. تم تسجيل التفاصيل ويمكنك المحاولة مرة أخرى.");
            }
        }

        public void LoadFormInPanel(Form form)
        {
            try
            {
                UIHelper.ApplyStyle(form);
                ClearPanelContent();

                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;

                panelContent.Controls.Add(form);
                form.Show();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل الواجهة", ex);
                ShowLoadError("تعذر تحميل الواجهة. تم تسجيل التفاصيل ويمكنك المحاولة مرة أخرى.");
            }
        }

        // تحميل محمي بالصلاحية: يمنع الوصول المباشر للشاشة لمن لا يملك أي صلاحية للوحدة،
        // دون الاعتماد على إخفاء الأزرار فقط.
        public void LoadUserControl(string module, string message, UserControl uc, params string[] legacyPermissions)
        {
            if (!EnsureModule(module, message, legacyPermissions))
            {
                uc?.Dispose();
                return;
            }
            LoadUserControl(uc);
        }

        public void LoadFormInPanel(string module, string message, Form form, params string[] legacyPermissions)
        {
            if (!EnsureModule(module, message, legacyPermissions))
            {
                form?.Dispose();
                return;
            }
            LoadFormInPanel(form);
        }

        private void ShowLoadError(string message)
        {
            ClearPanelContent();

            Label errorLabel = new Label
            {
                Text = message,
                Font = new Font(UIHelper.FontFamily, UIHelper.HeadingFontSize, FontStyle.Bold),
                ForeColor = UIHelper.DangerColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            panelContent.Controls.Add(errorLabel);
        }

        private bool Has(string permission)
        {
            return CurrentUser.HasPermission(permission);
        }

        private bool HasAny(params string[] permissions)
        {
            return CurrentUser.HasAny(permissions);
        }

        private bool CanOpen(string module, params string[] legacyPermissions)
        {
            // إظهار الوحدة عند امتلاك أي صلاحية صحيحة لها؛ فالصلاحيات الإجرائية
            // مثل Add/Edit/Delete لا ينبغي أن تجعل الشاشة تختفي إذا غاب View بالخطأ.
            return CurrentUser.CanAccessModule(module)
                || CurrentUser.CanView(module)
                || HasAny(legacyPermissions);
        }

        private bool EnsureModule(string module, string message, params string[] legacyPermissions)
        {
            if (CanOpen(module, legacyPermissions))
                return true;

            MessageBox.Show(message, "صلاحية غير كافية", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            return false;
        }

        private bool EnsurePermission(string permission)
        {
            if (Has(permission))
                return true;

            MessageBox.Show(
                "ليس لديك صلاحية للوصول إلى هذه الشاشة.",
                "صلاحية غير كافية",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            return false;
        }

        private bool EnsureAnyPermission(params string[] permissions)
        {
            if (HasAny(permissions))
                return true;

            MessageBox.Show(
                "ليس لديك صلاحية للوصول إلى هذه الشاشة.",
                "صلاحية غير كافية",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            return false;
        }

        public void RefreshCurrentUserSession()
        {
            UpdateCurrentUserLabel();
            ApplyCurrentUserPermissions();
        }

        private void ApplyCurrentUserPermissions()
        {
            if (IsDesignTime())
            {
                SetMenuItemsVisible(true);
                return;
            }

            if (!CurrentUser.IsLoggedIn)
            {
                // لا نترك القائمة مخفية أثناء انتقال الجلسة أو داخل المصمم.
                SetMenuItemsVisible(true);
                return;
            }

            ToolStripMenuItem[] permissionItems =
            {
                tsmiDashboard, tsmiStudents, tsmiStudentsManage, tsmiStudentsEnroll,
                tsmiStudentsClasses, tsmiTeachers, tsmiTeachersManage, tsmiTeachersAttendance,
                tsmiTeachersPayroll, tsmiAcademic, tsmiSubjects, tsmiClasses, tsmiTimetable,
                tsmiAttendanceGrades, tsmiGrades, tsmiAttendance, tsmiFinancial, tsmiFees, tsmiVouchers,
                tsmiExpenses, تعريفرسومالصفوفToolStripMenuItem, tsmiFinancialPayroll,
                tsmiServices, tsmiTransport, tsmiLibrary,
                tsmiAdmin, tsmiUsers, tsmiReports, tsmiAuditLogs, tsmiSettings
            };

            foreach (ToolStripMenuItem item in permissionItems)
                item.Visible = false;

            // مدير النظام يرى كامل كتالوج الواجهات. يبقى EnsurePermission
            // داخل كل معالج وخدمة هو الحاجز الأمني الفعلي عند الفتح والتنفيذ.
            if (CurrentUser.IsAdmin())
            {
                tsmiDashboard.Visible = true;
                tsmiStudents.Visible = true;
                tsmiStudentsManage.Visible = true;
                tsmiStudentsEnroll.Visible = true;
                tsmiStudentsClasses.Visible = true;
                tsmiTeachers.Visible = true;
                tsmiTeachersManage.Visible = true;
                tsmiTeachersAttendance.Visible = true;
                tsmiTeachersPayroll.Visible = true;
                tsmiAcademic.Visible = true;
                tsmiSubjects.Visible = true;
                tsmiClasses.Visible = true;
                tsmiTimetable.Visible = true;
                tsmiAttendanceGrades.Visible = true;
                tsmiGrades.Visible = true;
                tsmiAttendance.Visible = true;
                tsmiFinancial.Visible = true;
                tsmiFees.Visible = true;
                tsmiVouchers.Visible = true;
                tsmiExpenses.Visible = true;
                تعريفرسومالصفوفToolStripMenuItem.Visible = true;
                tsmiFinancialPayroll.Visible = true;
                tsmiServices.Visible = true;
                tsmiTransport.Visible = true;
                tsmiLibrary.Visible = true;
                tsmiAdmin.Visible = true;
                tsmiUsers.Visible = true;
                tsmiReports.Visible = true;
                tsmiAuditLogs.Visible = true;
                tsmiSettings.Visible = true;
                return;
            }

            tsmiDashboard.Visible = CanOpen("Dashboard", PermissionKeys.DashboardView);

            // الطلاب: نفحص Students.View أو Students.Manage أو أي صلاحية للوحدة
            tsmiStudentsManage.Visible = CanOpen("Students", PermissionKeys.StudentsView, PermissionKeys.StudentsManage);
            // التسجيل: نفحص Enrollment.View أو Enrollment.Manage
            tsmiStudentsEnroll.Visible = CanOpen("Enrollment", "Enrollment.View", PermissionKeys.EnrollmentManage);
            // توزيع الفصول
            tsmiStudentsClasses.Visible = CanOpen("ClassAssignment", PermissionKeys.ClassAssignmentView, PermissionKeys.ClassAssignmentManage);

            // المعلمون
            tsmiTeachersManage.Visible = CanOpen("Teachers", "Teachers.View", PermissionKeys.TeachersManage);
            // حضور المعلمين: يدعم كلاً من StaffAttendance و TeacherAttendance
            tsmiTeachersAttendance.Visible = CanOpen("StaffAttendance", "StaffAttendance.View", PermissionKeys.StaffAttendanceManage)
                                          || CanOpen("TeacherAttendance", "TeacherAttendance.View", "TeacherAttendance.Manage");
            tsmiTeachersPayroll.Visible = CanOpen("Payroll", "Payroll.View", PermissionKeys.PayrollManage);

            // الأكاديمي
            tsmiSubjects.Visible = CanOpen("Subjects", "Subjects.View", PermissionKeys.SubjectsManage);
            tsmiClasses.Visible = CanOpen("Classes", "Classes.View", PermissionKeys.ClassesManage);
            tsmiTimetable.Visible = CanOpen("Timetable", "Timetable.View", PermissionKeys.TimetableManage);

            // الدرجات والحضور
            tsmiGrades.Visible = CanOpen("Grades", "Grades.View", PermissionKeys.GradesManage);
            tsmiAttendance.Visible = CanOpen("Attendance", "Attendance.View", PermissionKeys.AttendanceManage);

            // المالي
            tsmiFees.Visible = CanOpen("Fees", "Fees.View", PermissionKeys.FeesManage);
            tsmiVouchers.Visible = CanOpen("Vouchers", "Vouchers.View", PermissionKeys.VouchersManage);
            tsmiExpenses.Visible = CanOpen("Expenses", "Expenses.View", PermissionKeys.ExpensesManage);
            tsmiFinancialPayroll.Visible = CanOpen("Payroll", "Payroll.View", PermissionKeys.PayrollManage);
            // خطط الرسوم: تُفتح بصلاحية FeePlans أو Fees.Manage
            تعريفرسومالصفوفToolStripMenuItem.Visible = CanOpen("FeePlans", "FeePlans.View", PermissionKeys.FeesManage);

            // الخدمات
            tsmiTransport.Visible = CanOpen("Transport", "Transport.View", PermissionKeys.TransportManage);
            tsmiLibrary.Visible = CanOpen("Library", "Library.View", PermissionKeys.LibraryManage);

            // الإدارة
            tsmiUsers.Visible = CanOpen("Users", PermissionKeys.UsersView, PermissionKeys.UsersManage);
            tsmiReports.Visible = CanOpen("Reports", PermissionKeys.ReportsView);
            tsmiAuditLogs.Visible = CanOpen("AuditLogs", PermissionKeys.AuditLogsView);
            tsmiSettings.Visible = CanOpen("Settings", PermissionKeys.SettingsView, PermissionKeys.SettingsManage);

            // إخفاء مجموعات القوائم التي لا تحتوي على أي خيار مسموح للمستخدم.
            tsmiStudents.Visible = tsmiStudentsManage.Visible || tsmiStudentsEnroll.Visible || tsmiStudentsClasses.Visible;
            tsmiTeachers.Visible = tsmiTeachersManage.Visible || tsmiTeachersAttendance.Visible || tsmiTeachersPayroll.Visible;
            tsmiAcademic.Visible = tsmiSubjects.Visible || tsmiClasses.Visible || tsmiTimetable.Visible;
            tsmiAttendanceGrades.Visible = tsmiGrades.Visible || tsmiAttendance.Visible;
            tsmiFinancial.Visible = tsmiFees.Visible || tsmiVouchers.Visible || tsmiExpenses.Visible || تعريفرسومالصفوفToolStripMenuItem.Visible || tsmiFinancialPayroll.Visible;
            tsmiServices.Visible = tsmiTransport.Visible || tsmiLibrary.Visible;
            tsmiAdmin.Visible = tsmiUsers.Visible || tsmiAuditLogs.Visible || tsmiSettings.Visible;
        }

        private bool IsDesignTime()
        {
            if (System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime)
                return true;

            if (Site != null && Site.DesignMode)
                return true;

            return GetService(typeof(System.ComponentModel.Design.IDesignerHost)) != null;
        }

        private void SetMenuItemsVisible(bool visible)
        {
            ToolStripMenuItem[] items =
            {
                tsmiDashboard, tsmiStudents, tsmiStudentsManage, tsmiStudentsEnroll,
                tsmiStudentsClasses, tsmiTeachers, tsmiTeachersManage, tsmiTeachersAttendance,
                tsmiTeachersPayroll, tsmiAcademic, tsmiSubjects, tsmiClasses, tsmiTimetable,
                tsmiAttendanceGrades, tsmiGrades, tsmiAttendance, tsmiFinancial, tsmiFees, tsmiVouchers,
                tsmiExpenses, تعريفرسومالصفوفToolStripMenuItem, tsmiFinancialPayroll,
                tsmiServices, tsmiTransport, tsmiLibrary,
                tsmiAdmin, tsmiUsers, tsmiReports, tsmiAuditLogs, tsmiSettings, tsmiLogout
            };

            foreach (ToolStripMenuItem item in items)
                item.Visible = visible;
        }

        private void tsmiDashboard_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.DashboardView))
                return;

            LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                ClearPanelContent();

                var dashboard = new DashboardHome();
                dashboard.Dock = DockStyle.Fill;

                panelContent.Controls.Add(dashboard);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل لوحة التحكم", ex);
                ShowLoadError("تعذر تحميل لوحة التحكم. تم تسجيل التفاصيل ويمكنك المحاولة مرة أخرى.");
            }
        }

        private void tsmiStudentsManage_Click(object sender, EventArgs e)
        {
            LoadFormInPanel("Students", "ليس لديك صلاحية عرض الطلاب.", new StudentsForm(), PermissionKeys.StudentsView, PermissionKeys.StudentsManage);
        }

        private void tsmiStudentsEnroll_Click(object sender, EventArgs e)
        {
            LoadFormInPanel("Enrollment", "ليس لديك صلاحية عرض القبول والتسجيل.", new SchoolSystem.UI.EnrollmentForm(), "Enrollment.View", PermissionKeys.EnrollmentManage);
        }

        private void tsmiStudentsClasses_Click(object sender, EventArgs e)
        {
            try
            {
                // إنشاء UserControl داخل الحماية حتى تظهر أخطاء المُنشئ/Designer
                // للمستخدم بصورة آمنة بدل أن تتسرب إلى معالج WinForms العام.
                UserControl assignmentForm = new SchoolSystem.UI.Students.ClassAssignmentForm();
                LoadUserControl("ClassAssignment", "ليس لديك صلاحية عرض توزيع الطلاب.", assignmentForm, PermissionKeys.ClassAssignmentView, PermissionKeys.ClassAssignmentManage);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فتح واجهة توزيع الفصول", ex);
            }
        }

        private void tsmiTeachersManage_Click(object sender, EventArgs e)
        {
            LoadUserControl("Teachers", "ليس لديك صلاحية عرض المعلمين.", new TeachersForm(), "Teachers.View", PermissionKeys.TeachersManage);
        }

        private void tsmiTeachersAttendance_Click(object sender, EventArgs e)
        {
            LoadUserControl("StaffAttendance", "ليس لديك صلاحية عرض حضور الموظفين.", new StaffAttendanceForm(), "StaffAttendance.View", PermissionKeys.StaffAttendanceManage);
        }

        private void tsmiTeachersPayroll_Click(object sender, EventArgs e)
        {
            LoadUserControl("Payroll", "ليس لديك صلاحية عرض الرواتب.", new PayrollForm(), "Payroll.View", PermissionKeys.PayrollManage);
        }

        private void tsmiSubjects_Click(object sender, EventArgs e)
        {
            LoadUserControl("Subjects", "ليس لديك صلاحية عرض المواد.", new SubjectsForm(), "Subjects.View", PermissionKeys.SubjectsManage);
        }

        private void tsmiClasses_Click(object sender, EventArgs e)
        {
            LoadUserControl("Classes", "ليس لديك صلاحية عرض الفصول الدراسية.", new ClassesForm(), "Classes.View", PermissionKeys.ClassesManage);
        }

        private void tsmiTimetable_Click(object sender, EventArgs e)
        {
            LoadUserControl("Timetable", "ليس لديك صلاحية عرض الجدول الدراسي.", new TimetableForm(), "Timetable.View", PermissionKeys.TimetableManage);
        }

        private void tsmiGrades_Click(object sender, EventArgs e)
        {
            LoadUserControl("Grades", "ليس لديك صلاحية عرض الدرجات.", new GradeEntryForm(), "Grades.View", PermissionKeys.GradesManage);
        }

        private void tsmiAttendance_Click(object sender, EventArgs e)
        {
            LoadUserControl("Attendance", "ليس لديك صلاحية عرض الحضور.", new DailyAttendanceForm(), "Attendance.View", PermissionKeys.AttendanceManage);
        }

        private void tsmiFees_Click(object sender, EventArgs e)
        {
            LoadUserControl("Fees", "ليس لديك صلاحية عرض الرسوم.", new FeesForm(), "Fees.View", PermissionKeys.FeesManage);
        }

        private void tsmiVouchers_Click(object sender, EventArgs e)
        {
            LoadUserControl("Vouchers", "ليس لديك صلاحية عرض السندات.", new VouchersForm(), "Vouchers.View", PermissionKeys.VouchersManage);
        }

        private void tsmiExpenses_Click(object sender, EventArgs e)
        {
            LoadUserControl("Expenses", "ليس لديك صلاحية عرض المصروفات.", new ExpensesForm(), "Expenses.View", PermissionKeys.ExpensesManage);
        }

        private void tsmiTransport_Click(object sender, EventArgs e)
        {
            LoadUserControl("Transport", "ليس لديك صلاحية عرض النقل.", new TransportForm(), "Transport.View", PermissionKeys.TransportManage);
        }

        private void tsmiLibrary_Click(object sender, EventArgs e)
        {
            LoadUserControl("Library", "ليس لديك صلاحية عرض المكتبة.", new LibraryForm(), "Library.View", PermissionKeys.LibraryManage);
        }

        private void tsmiUsers_Click(object sender, EventArgs e)
        {
            LoadUserControl("Users", "ليس لديك صلاحية عرض المستخدمين.", new UsersForm(), PermissionKeys.UsersView, PermissionKeys.UsersManage);
        }

        private void tsmiReports_Click(object sender, EventArgs e)
        {
            LoadUserControl("Reports", "ليس لديك صلاحية عرض التقارير.", new ReportCenterForm(), PermissionKeys.ReportsView);
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            LoadUserControl("Settings", "ليس لديك صلاحية عرض الإعدادات.", new SettingsForm(), PermissionKeys.SettingsView, PermissionKeys.SettingsManage);
        }

        private void tsmiAuditLogs_Click(object sender, EventArgs e)
        {
            LoadUserControl("AuditLogs", "ليس لديك صلاحية عرض سجل التدقيق.", new AuditLogForm(), PermissionKeys.AuditLogsView);
        }

        private void تعريفرسومالصفوفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUserControl("FeePlans", "ليس لديك صلاحية عرض خطط الرسوم.", new FeePlansForm(), "FeePlans.View", PermissionKeys.FeesManage);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CurrentUser.IsLoggedIn && CurrentUser.User != null)
            {
                try
                {
                    auditLogService.Record(
                        "إغلاق التطبيق",
                        "User",
                        CurrentUser.User.UserID.ToString(),
                        "تم إغلاق التطبيق مع إنهاء جلسة المستخدم");
                }
                catch (Exception auditException)
                {
                    ApplicationLogger.LogException("إغلاق التطبيق في سجل التدقيق", auditException);
                }
            }

            // لا تترك جلسة صالحة عند إغلاق النافذة مباشرة.
            CurrentUser.Clear();
            Instance = null;
            Application.Exit();
        }

        private void tsmiLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "هل تريد تسجيل الخروج؟",
                "تسجيل الخروج",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (result != DialogResult.Yes)
                return;

            try
            {
                if (CurrentUser.IsLoggedIn && CurrentUser.User != null)
                {
                    auditLogService.Record(
                        "تسجيل الخروج",
                        "User",
                        CurrentUser.User.UserID.ToString(),
                        "تم إنهاء جلسة المستخدم بواسطة تسجيل الخروج");
                }
            }
            catch (Exception auditException)
            {
                ApplicationLogger.LogException("تسجيل الخروج في سجل التدقيق", auditException);
            }

            CurrentUser.Clear();
            RefreshCurrentUserSession();
            LoadWelcomeScreen();
            Hide();

            using (LoginForm loginForm = new LoginForm())
            {
                DialogResult loginResult = loginForm.ShowDialog();

                if (loginResult == DialogResult.OK)
                {
                    // حافظ على MainForm الأصلي لأنه مالك Application.Run.
                    // إغلاقه أثناء إنشاء نافذة جديدة ينهي حلقة الرسائل ويوقف التطبيق.
                    Show();
                    WindowState = FormWindowState.Normal;
                    RefreshCurrentUserSession();
                    LoadWelcomeScreen();
                }
                else
                {
                    // لا نغلق MainForm لأن Application.Run يعتمد عليه.
                    // يمكن للمستخدم إعادة محاولة تسجيل الدخول من نفس الجلسة.
                    Show();
                    WindowState = FormWindowState.Normal;
                    SetMenuItemsVisible(true);
                    UpdateCurrentUserLabel();
                }
            }
        }
    }
}
