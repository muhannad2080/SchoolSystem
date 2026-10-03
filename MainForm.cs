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
        // ================================================================
        // STATIC INSTANCE
        // ================================================================

        public static MainForm Instance { get; private set; }

        // ================================================================
        // SERVICES
        // ================================================================

        private readonly AuditLogService auditLogService =
            new AuditLogService();

        // ================================================================
        // DYNAMIC MENU ITEM
        // ================================================================


        // ================================================================
        // CONSTRUCTOR
        // ================================================================

        public MainForm()
        {
            InitializeComponent();


            FormClosed +=
                MainForm_FormClosed;

            Instance = this;

            UpdateCurrentUserLabel();

            timerClock.Start();

            LoadWelcomeScreen();

            ApplyCurrentUserPermissions();
        }

        // ================================================================
        // HEADER VISIBILITY
        //
        // الصفحة الرئيسية:
        //     Header ظاهر.
        //
        // أي شاشة فرعية:
        //     Header مخفي بالكامل.
        //
        // لا يتم إنشاء Header آخر للشاشة الفرعية.
        // ================================================================

        private void ShowMainHeader()
        {
            if (IsDisposed || Disposing)
                return;

            panelTop.Visible = true;

            // استعادة حالة العنوان الرئيسية فقط.
            lblSystemTitle.Text =
                "نظام إدارة المدرسة";

            lblSystemSubtitle.Visible =
                true;

            lblSystemSubtitle.Text =
                "الحل المتكامل لإدارة المؤسسات التعليمية";

            lblUserRole.Visible =
                true;

            lblDateTime.Visible =
                true;

            UpdateCurrentUserLabel();

            // التأكد من إعادة ترتيب Dock بعد إظهار الرأس.
            panelContent.BringToFront();

            panelTop.BringToFront();

            statusStripMain.BringToFront();
        }

        private void HideMainHeader()
        {
            if (IsDisposed || Disposing)
                return;

            // إخفاء الرأس بالكامل.
            //
            // panelContent يستخدم DockStyle.Fill ولذلك سيأخذ
            // المساحة التي كانت محجوزة للرأس تلقائيًا.

            panelTop.Visible = false;

            // لا نحتاج اسم النظام أو اسم الشاشة أو أي نص إضافي
            // في الجزء العلوي عند فتح فرع.

            lblSystemTitle.Text =
                string.Empty;

            lblSystemSubtitle.Text =
                string.Empty;

            lblUserRole.Text =
                string.Empty;

            // إخفاء العناصر النصية نفسها لتجنب حجز مساحة داخلية.
            lblSystemTitle.Visible =
                false;

            lblSystemSubtitle.Visible =
                false;

            lblUserRole.Visible =
                false;
        }

        // ================================================================
        // ANNUAL CLOSING
        // ================================================================

        private void tsmiAnnualClosing_Click(
            object sender,
            EventArgs e)
        {
            LoadFormInPanel(
                "AnnualClosing",
                "ليس لديك صلاحية عرض الإغلاق السنوي والترحيل.",
                new AnnualClosingForm(),
                PermissionKeys.AnnualClosingView,
                PermissionKeys.AnnualClosingManage);
        }

        // ================================================================
        // CURRENT USER
        // ================================================================

        private void UpdateCurrentUserLabel()
        {
            if (!CurrentUser.IsLoggedIn ||
                CurrentUser.User == null)
            {
                lblUsername.Text =
                    "زائر";

                lblUserRole.Text =
                    string.Empty;

                lblStatusUser.Text =
                    "غير مسجل";

                return;
            }

            string displayName =
                CurrentUser.User.FullName;

            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName =
                    CurrentUser.User.UserName;
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName =
                    "مستخدم";
            }

            lblUsername.Text =
                "👤 " + displayName;

            lblStatusUser.Text =
                "المستخدم: " + displayName;

            string role =
                CurrentUser.User.RoleName;

            lblUserRole.Text =
                string.IsNullOrWhiteSpace(role)
                    ? string.Empty
                    : "الدور: " + role;
        }

        // ================================================================
        // CLOCK
        // ================================================================

        private void timerClock_Tick(
            object sender,
            EventArgs e)
        {
            lblDateTime.Text =
                FormatNow();
        }

        private static string FormatNow()
        {
            return DateTime.Now.ToString(
                "dddd، dd/MM/yyyy  HH:mm:ss");
        }

        // ================================================================
        // WELCOME SCREEN
        // ================================================================

        private void LoadWelcomeScreen()
        {
            ClearPanelContent();

            // الرئيسية هي الحالة الوحيدة التي يظهر فيها الرأس.
            ShowMainHeader();

            var welcome =
                new WelcomeScreen();

            welcome.SystemName =
                "أهلاً بك في نظام إدارة المدرسة";

            welcome.Dock =
                DockStyle.Fill;

            panelContent.Controls.Add(
                welcome);

            welcome.BringToFront();
        }

        // ================================================================
        // PUBLIC WELCOME SCREEN
        // ================================================================

        public void ShowWelcomeScreen()
        {
            if (IsDisposed ||
                Disposing)
            {
                return;
            }

            LoadWelcomeScreen();
        }

        // ================================================================
        // CLEAR CONTENT
        // ================================================================

        private void ClearPanelContent()
        {
            while (panelContent.Controls.Count > 0)
            {
                Control existing =
                    panelContent.Controls[0];

                panelContent.Controls.RemoveAt(0);

                existing.Dispose();
            }
        }

        // ================================================================
        // LOAD USER CONTROL
        //
        // الشاشة الفرعية تستحوذ على كل المساحة المتاحة.
        // ================================================================

        public void LoadUserControl(
            UserControl uc)
        {
            try
            {
                if (uc == null)
                {
                    ShowLoadError(
                        "تعذر تحميل الواجهة المطلوبة.");

                    return;
                }

                ClearPanelContent();

                // إخفاء الرأس بالكامل.
                HideMainHeader();

                // جعل UserControl يستخدم كل مساحة panelContent.
                uc.Dock =
                    DockStyle.Fill;

                panelContent.Controls.Add(
                    uc);

                uc.BringToFront();

                ApplicationLogger.LogUsage("الشاشة الرئيسية", "فتح واجهة", uc.GetType().Name);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "تحميل الواجهة",
                    ex);

                ShowLoadError(
                    "تعذر تحميل الواجهة. تم تسجيل التفاصيل ويمكنك المحاولة مرة أخرى.");
            }
        }

        // ================================================================
        // LOAD FORM INSIDE PANEL
        //
        // الشاشة الفرعية تستحوذ على كل المساحة المتاحة.
        // ================================================================

        public void LoadFormInPanel(
            Form form)
        {
            try
            {
                if (form == null)
                {
                    ShowLoadError(
                        "تعذر تحميل الواجهة المطلوبة.");

                    return;
                }

                ClearPanelContent();

                // إخفاء الـHeader بالكامل.
                HideMainHeader();

                form.TopLevel =
                    false;

                form.FormBorderStyle =
                    FormBorderStyle.None;

                form.Dock =
                    DockStyle.Fill;

                panelContent.Controls.Add(
                    form);

                form.Show();

                form.BringToFront();

                ApplicationLogger.LogUsage("الشاشة الرئيسية", "فتح شاشة", form.GetType().Name);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "تحميل الواجهة",
                    ex);

                ShowLoadError(
                    "تعذر تحميل الواجهة. تم تسجيل التفاصيل ويمكنك المحاولة مرة أخرى.");
            }
        }

        // ================================================================
        // PROTECTED USER CONTROL LOADING
        // ================================================================

        public void LoadUserControl(
            string module,
            string message,
            UserControl uc,
            params string[] legacyPermissions)
        {
            if (!EnsureModule(
                    module,
                    message,
                    legacyPermissions))
            {
                uc?.Dispose();

                return;
            }

            LoadUserControl(uc);
        }

        // ================================================================
        // PROTECTED FORM LOADING
        // ================================================================

        public void LoadFormInPanel(
            string module,
            string message,
            Form form,
            params string[] legacyPermissions)
        {
            if (!EnsureModule(
                    module,
                    message,
                    legacyPermissions))
            {
                form?.Dispose();

                return;
            }

            LoadFormInPanel(form);
        }

        // ================================================================
        // LOAD ERROR
        // ================================================================

        private void ShowLoadError(
            string message)
        {
            ClearPanelContent();

            HideMainHeader();

            Label errorLabel =
                new Label
                {
                    Text =
                        message,

                    TextAlign =
                        ContentAlignment.MiddleCenter,

                    Dock =
                        DockStyle.Fill,

                    AutoSize =
                        false
                };

            panelContent.Controls.Add(
                errorLabel);

            errorLabel.BringToFront();
        }

        // ================================================================
        // PERMISSIONS
        // ================================================================

        private bool Has(
            string permission)
        {
            return CurrentUser.HasPermission(
                permission);
        }

        private bool HasAny(
            params string[] permissions)
        {
            return CurrentUser.HasAny(
                permissions);
        }

        private bool CanOpen(
            string module,
            params string[] legacyPermissions)
        {
            if (!AuthorizationService.CanAccessScreen(
                    module))
            {
                return false;
            }

            // مفاتيح التوافق القديمة كانت تُمرَّر دون تقييم؛ تُطبَّق الآن كشرط إضافي فعلي.
            // كل قائمة استدعاء تبدأ بمفتاح View للوحدة نفسها، فلا يُضيَّق وصول
            // من يملك صلاحية الشاشة فعلاً، بينما تُحجب الوحدات غير المصرح بها.
            if (legacyPermissions == null ||
                legacyPermissions.Length == 0)
            {
                return true;
            }

            return CurrentUser.HasAny(
                legacyPermissions);
        }

        private bool EnsureModule(
            string module,
            string message,
            params string[] legacyPermissions)
        {
            if (CanOpen(
                    module,
                    legacyPermissions))
            {
                return true;
            }

            MessageBox.Show(
                message,
                "صلاحية غير كافية",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign |
                MessageBoxOptions.RtlReading);

            return false;
        }

        private bool EnsurePermission(
            string permission)
        {
            if (Has(permission))
                return true;

            MessageBox.Show(
                "ليس لديك صلاحية للوصول إلى هذه الشاشة.",
                "صلاحية غير كافية",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign |
                MessageBoxOptions.RtlReading);

            return false;
        }

        private bool EnsureAnyPermission(
            params string[] permissions)
        {
            if (HasAny(permissions))
                return true;

            MessageBox.Show(
                "ليس لديك صلاحية للوصول إلى هذه الشاشة.",
                "صلاحية غير كافية",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign |
                MessageBoxOptions.RtlReading);

            return false;
        }

        // ================================================================
        // SESSION
        // ================================================================

        public void RefreshCurrentUserSession()
        {
            UpdateCurrentUserLabel();

            ApplyCurrentUserPermissions();
        }

        // ================================================================
        // MENU PERMISSIONS
        // ================================================================

        private void ApplyCurrentUserPermissions()
        {
            if (IsDesignTime())
            {
                SetMenuItemsVisible(true);

                return;
            }

            if (!CurrentUser.IsLoggedIn)
            {
                // جلسة غير مسجلة: لا تُعرض أي قوائم؛ الحماية لا تعتمد على الإخفاء وحده
                // (كل فتح شاشة يمر عبر EnsureModule) لكن لا سبب لعرض وحدات لمصرّح بها أصلاً.
                SetMenuItemsVisible(false);

                return;
            }

            ToolStripMenuItem[] permissionItems =
            {
                tsmiDashboard,

                tsmiStudents,
                tsmiStudentsManage,
                tsmiStudentsEnroll,
                tsmiStudentsClasses,

                tsmiTeachers,
                tsmiTeachersManage,
                tsmiTeachersAttendance,
                tsmiTeachersPayroll,

                tsmiAcademic,
                tsmiSubjects,
                tsmiClasses,
                tsmiTimetable,

                tsmiAttendanceGrades,
                tsmiGrades,
                tsmiAttendance,

                tsmiFinancial,
                tsmiFees,
                tsmiVouchers,
                tsmiExpenses,
                تعريفرسومالصفوفToolStripMenuItem,
                tsmiFinancialPayroll,

                tsmiServices,
                tsmiTransport,
                tsmiLibrary,

                tsmiAdmin,
                tsmiUsers,
                tsmiReports,
                tsmiAuditLogs,
                tsmiSettings,
                tsmiAnnualClosing
            };

            foreach (ToolStripMenuItem item
                     in permissionItems)
            {
                item.Visible =
                    false;
            }

            tsmiDashboard.Visible =
                CanOpen(
                    "Dashboard",
                    PermissionKeys.DashboardView);

            tsmiStudentsManage.Visible =
                CanOpen(
                    "Students",
                    PermissionKeys.StudentsView,
                    PermissionKeys.StudentsManage);

            tsmiStudentsEnroll.Visible =
                CanOpen(
                    "Enrollment",
                    "Enrollment.View",
                    PermissionKeys.EnrollmentManage);

            tsmiStudentsClasses.Visible =
                CanOpen(
                    "ClassAssignment",
                    PermissionKeys.ClassAssignmentView,
                    PermissionKeys.ClassAssignmentManage);

            tsmiTeachersManage.Visible =
                CanOpen(
                    "Teachers",
                    "Teachers.View",
                    PermissionKeys.TeachersManage);

            tsmiTeachersAttendance.Visible =
                CanOpen(
                    "StaffAttendance",
                    "StaffAttendance.View",
                    PermissionKeys.StaffAttendanceManage)

                ||

                CanOpen(
                    "TeacherAttendance",
                    "TeacherAttendance.View",
                    "TeacherAttendance.Manage");

            tsmiTeachersPayroll.Visible =
                CanOpen(
                    "Payroll",
                    "Payroll.View",
                    PermissionKeys.PayrollManage);

            tsmiSubjects.Visible =
                CanOpen(
                    "Subjects",
                    "Subjects.View",
                    PermissionKeys.SubjectsManage);

            tsmiClasses.Visible =
                CanOpen(
                    "Classes",
                    "Classes.View",
                    PermissionKeys.ClassesManage);

            tsmiTimetable.Visible =
                CanOpen(
                    "Timetable",
                    "Timetable.View",
                    PermissionKeys.TimetableManage);

            tsmiGrades.Visible =
                CanOpen(
                    "Grades",
                    "Grades.View",
                    PermissionKeys.GradesManage);

            tsmiAttendance.Visible =
                CanOpen(
                    "Attendance",
                    "Attendance.View",
                    PermissionKeys.AttendanceManage);

            tsmiFees.Visible =
                CanOpen(
                    "Fees",
                    "Fees.View",
                    PermissionKeys.FeesManage);

            tsmiVouchers.Visible =
                CanOpen(
                    "Vouchers",
                    "Vouchers.View",
                    PermissionKeys.VouchersManage);

            tsmiExpenses.Visible =
                CanOpen(
                    "Expenses",
                    "Expenses.View",
                    PermissionKeys.ExpensesManage);

            tsmiFinancialPayroll.Visible =
                CanOpen(
                    "Payroll",
                    "Payroll.View",
                    PermissionKeys.PayrollManage);

            تعريفرسومالصفوفToolStripMenuItem.Visible =
                CanOpen(
                    "FeePlans",
                    "FeePlans.View",
                    PermissionKeys.FeesManage);

            tsmiTransport.Visible =
                CanOpen(
                    "Transport",
                    "Transport.View",
                    PermissionKeys.TransportManage);

            tsmiLibrary.Visible =
                CanOpen(
                    "Library",
                    "Library.View",
                    PermissionKeys.LibraryManage);

            tsmiUsers.Visible =
                CanOpen(
                    "Users",
                    PermissionKeys.UsersView,
                    PermissionKeys.UsersManage);

            tsmiReports.Visible =
                CanOpen(
                    "Reports",
                    PermissionKeys.ReportsView);

            tsmiAuditLogs.Visible =
                CanOpen(
                    "AuditLogs",
                    PermissionKeys.AuditLogsView);

            tsmiSettings.Visible =
                CanOpen(
                    "Settings",
                    PermissionKeys.SettingsView,
                    PermissionKeys.SettingsManage);

            if (tsmiAnnualClosing != null)
            {
                tsmiAnnualClosing.Visible =
                    CanOpen(
                        "AnnualClosing",
                        PermissionKeys.AnnualClosingView,
                        PermissionKeys.AnnualClosingManage);
            }

            tsmiStudents.Visible =
                CanOpen("Students") ||
                CanOpen("Enrollment") ||
                CanOpen("ClassAssignment");

            tsmiTeachers.Visible =
                CanOpen("Teachers") ||
                CanOpen("StaffAttendance") ||
                CanOpen("TeacherAttendance") ||
                CanOpen("Payroll");

            tsmiAcademic.Visible =
                CanOpen("Subjects") ||
                CanOpen("Classes") ||
                CanOpen("Timetable");

            tsmiAttendanceGrades.Visible =
                CanOpen("Grades") ||
                CanOpen("Attendance");

            tsmiFinancial.Visible =
                CanOpen("Fees") ||
                CanOpen("FeePlans") ||
                CanOpen("Vouchers") ||
                CanOpen("Expenses") ||
                CanOpen("Payroll");

            tsmiServices.Visible =
                CanOpen("Transport") ||
                CanOpen("Library");

            tsmiAdmin.Visible =
                CanOpen("Users") ||
                CanOpen("AuditLogs") ||
                CanOpen("Settings") ||
                CanOpen("AnnualClosing");

            tsmiLogout.Visible = true;
        }

        // ================================================================
        // DESIGN TIME
        // ================================================================

        private bool IsDesignTime()
        {
            if (System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return true;
            }

            if (Site != null &&
                Site.DesignMode)
            {
                return true;
            }

            return GetService(
                       typeof(
                           System.ComponentModel.Design.IDesignerHost))
                   != null;
        }

        // ================================================================
        // MENU VISIBILITY
        // ================================================================

        private void SetMenuItemsVisible(
            bool visible)
        {
            ToolStripMenuItem[] items =
            {
                tsmiDashboard,

                tsmiStudents,
                tsmiStudentsManage,
                tsmiStudentsEnroll,
                tsmiStudentsClasses,

                tsmiTeachers,
                tsmiTeachersManage,
                tsmiTeachersAttendance,
                tsmiTeachersPayroll,

                tsmiAcademic,
                tsmiSubjects,
                tsmiClasses,
                tsmiTimetable,

                tsmiAttendanceGrades,
                tsmiGrades,
                tsmiAttendance,

                tsmiFinancial,
                tsmiFees,
                tsmiVouchers,
                tsmiExpenses,
                تعريفرسومالصفوفToolStripMenuItem,
                tsmiFinancialPayroll,

                tsmiServices,
                tsmiTransport,
                tsmiLibrary,

                tsmiAdmin,
                tsmiUsers,
                tsmiReports,
                tsmiAuditLogs,
                tsmiSettings,

                tsmiLogout
            };

            foreach (ToolStripMenuItem item
                     in items)
            {
                item.Visible =
                    visible;
            }

            if (tsmiAnnualClosing != null)
            {
                tsmiAnnualClosing.Visible =
                    visible;
            }
        }

        // ================================================================
        // DASHBOARD
        // ================================================================

        private void tsmiDashboard_Click(
            object sender,
            EventArgs e)
        {
            if (!EnsurePermission(
                    PermissionKeys.DashboardView))
            {
                return;
            }

            LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                ClearPanelContent();

                ShowMainHeader();

                var dashboard =
                    new DashboardHome();

                dashboard.Dock =
                    DockStyle.Fill;

                panelContent.Controls.Add(
                    dashboard);

                dashboard.BringToFront();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "تحميل لوحة التحكم",
                    ex);

                ShowLoadError(
                    "تعذر تحميل لوحة التحكم. تم تسجيل التفاصيل ويمكنك المحاولة مرة أخرى.");
            }
        }

        // ================================================================
        // STUDENTS
        // ================================================================

        private void tsmiStudentsManage_Click(
            object sender,
            EventArgs e)
        {
            LoadFormInPanel(
                "Students",
                "ليس لديك صلاحية عرض الطلاب.",
                new StudentsForm(),
                PermissionKeys.StudentsView,
                PermissionKeys.StudentsManage);
        }

        private void tsmiStudentsEnroll_Click(
            object sender,
            EventArgs e)
        {
            LoadFormInPanel(
                "Enrollment",
                "ليس لديك صلاحية عرض القبول والتسجيل.",
                new SchoolSystem.UI.EnrollmentForm(),
                "Enrollment.View",
                PermissionKeys.EnrollmentManage);
        }

        private void tsmiStudentsClasses_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                UserControl assignmentForm =
                    new SchoolSystem.UI.Students.ClassAssignmentForm();

                LoadUserControl(
                    "ClassAssignment",
                    "ليس لديك صلاحية عرض توزيع الطلاب.",
                    assignmentForm,
                    PermissionKeys.ClassAssignmentView,
                    PermissionKeys.ClassAssignmentManage);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فتح واجهة توزيع الفصول",
                    ex);
            }
        }

        // ================================================================
        // TEACHERS
        // ================================================================

        private void tsmiTeachersManage_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Teachers",
                "ليس لديك صلاحية عرض المعلمين.",
                new TeachersForm(),
                "Teachers.View",
                PermissionKeys.TeachersManage);
        }

        private void tsmiTeachersAttendance_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "StaffAttendance",
                "ليس لديك صلاحية عرض حضور الموظفين.",
                new StaffAttendanceForm(),
                "StaffAttendance.View",
                PermissionKeys.StaffAttendanceManage);
        }

        private void tsmiFinancialPayroll_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Payroll",
                "ليس لديك صلاحية عرض الرواتب.",
                new PayrollRecordsForm(),
                "Payroll.View",
                PermissionKeys.PayrollManage);
        }

        private void tsmiTeachersPayroll_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Payroll",
                "ليس لديك صلاحية عرض الرواتب.",
                new PayrollForm(),
                "Payroll.View",
                PermissionKeys.PayrollManage);
        }

        // ================================================================
        // ACADEMIC
        // ================================================================

        private void tsmiSubjects_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Subjects",
                "ليس لديك صلاحية عرض المواد.",
                new SubjectsForm(),
                "Subjects.View",
                PermissionKeys.SubjectsManage);
        }

        private void tsmiClasses_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Classes",
                "ليس لديك صلاحية عرض الفصول الدراسية.",
                new ClassesForm(),
                "Classes.View",
                PermissionKeys.ClassesManage);
        }

        private void tsmiTimetable_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Timetable",
                "ليس لديك صلاحية عرض الجدول الدراسي.",
                new TimetableForm(),
                "Timetable.View",
                PermissionKeys.TimetableManage);
        }

        // ================================================================
        // ATTENDANCE / GRADES
        // ================================================================

        private void tsmiGrades_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Grades",
                "ليس لديك صلاحية عرض الدرجات.",
                new GradeEntryForm(),
                "Grades.View",
                PermissionKeys.GradesManage);
        }

        private void tsmiAttendance_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Attendance",
                "ليس لديك صلاحية عرض الحضور.",
                new DailyAttendanceForm(),
                "Attendance.View",
                PermissionKeys.AttendanceManage);
        }

        // ================================================================
        // FINANCIAL
        // ================================================================

        private void tsmiFees_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Fees",
                "ليس لديك صلاحية عرض الرسوم.",
                new FeesForm(),
                "Fees.View",
                PermissionKeys.FeesManage);
        }

        private void tsmiVouchers_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Vouchers",
                "ليس لديك صلاحية عرض السندات.",
                new VouchersForm(),
                "Vouchers.View",
                PermissionKeys.VouchersManage);
        }

        private void tsmiExpenses_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Expenses",
                "ليس لديك صلاحية عرض المصروفات.",
                new ExpensesForm(),
                "Expenses.View",
                PermissionKeys.ExpensesManage);
        }

        private void تعريفرسومالصفوفToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "FeePlans",
                "ليس لديك صلاحية عرض خطط الرسوم.",
                new FeePlansForm(),
                "FeePlans.View",
                PermissionKeys.FeesManage);
        }

        // ================================================================
        // SERVICES
        // ================================================================

        private void tsmiTransport_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Transport",
                "ليس لديك صلاحية عرض النقل.",
                new TransportForm(),
                "Transport.View",
                PermissionKeys.TransportManage);
        }

        private void tsmiLibrary_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Library",
                "ليس لديك صلاحية عرض المكتبة.",
                new LibraryForm(),
                "Library.View",
                PermissionKeys.LibraryManage);
        }

        // ================================================================
        // ADMINISTRATION
        // ================================================================

        private void tsmiUsers_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Users",
                "ليس لديك صلاحية عرض المستخدمين.",
                new UsersForm(),
                PermissionKeys.UsersView,
                PermissionKeys.UsersManage);
        }

        private void tsmiReports_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Reports",
                "ليس لديك صلاحية عرض التقارير.",
                new ReportCenterForm(),
                PermissionKeys.ReportsView);
        }

        private void tsmiSettings_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "Settings",
                "ليس لديك صلاحية عرض الإعدادات.",
                new SettingsForm(),
                PermissionKeys.SettingsView,
                PermissionKeys.SettingsManage);
        }

        private void tsmiAuditLogs_Click(
            object sender,
            EventArgs e)
        {
            LoadUserControl(
                "AuditLogs",
                "ليس لديك صلاحية عرض سجل التدقيق.",
                new AuditLogForm(),
                PermissionKeys.AuditLogsView);
        }

        // ================================================================
        // FORM CLOSED
        // ================================================================

        private void MainForm_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            if (CurrentUser.IsLoggedIn &&
                CurrentUser.User != null)
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
                    ApplicationLogger.LogException(
                        "إغلاق التطبيق في سجل التدقيق",
                        auditException);
                }
            }

            CurrentUser.Clear();

            Instance =
                null;

            Application.Exit();
        }

        // ================================================================
        // LOGOUT
        // ================================================================

        private void tsmiLogout_Click(
            object sender,
            EventArgs e)
        {
            DialogResult result =
                MessageBox.Show(
                    "هل تريد تسجيل الخروج؟",
                    "تسجيل الخروج",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.RightAlign |
                    MessageBoxOptions.RtlReading);

            if (result !=
                DialogResult.Yes)
            {
                return;
            }

            ApplicationLogger.LogUsage("الشاشة الرئيسية", "تسجيل الخروج", "المستخدم أنهى الجلسة");

            try
            {
                if (CurrentUser.IsLoggedIn &&
                    CurrentUser.User != null)
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
                ApplicationLogger.LogException(
                    "تسجيل الخروج في سجل التدقيق",
                    auditException);
            }

            CurrentUser.Clear();

            RefreshCurrentUserSession();

            LoadWelcomeScreen();

            Hide();

            // بعد تأكيد تسجيل الخروج يعود النظام دائماً إلى شاشة تسجيل الدخول ولا يغلق
            // التطبيق. لا يتم إغلاق النظام إلا عبر زر "خروج" في شاشة الدخول صراحةً.
            while (true)
            {
                using (LoginForm loginForm =
                       new LoginForm())
                {
                    DialogResult loginResult =
                        loginForm.ShowDialog();

                    if (loginResult ==
                        DialogResult.OK)
                    {
                        Show();

                        WindowState =
                            FormWindowState.Normal;

                        RefreshCurrentUserSession();

                        LoadWelcomeScreen();

                        break;
                    }

                    // إلغاء إعادة تسجيل الدخول لا يغلق النظام ولا يُبقي واجهة بلا
                    // مستخدم؛ بل يعود إلى شاشة الدخول من جديد لانتظار تسجيل الدخول.
                    // خروج النظام يتم فقط عبر زر "خروج" (Application.Exit).
                    if (!loginForm.AppShouldExit)
                        continue;

                    Close();

                    break;
                }
            }
        }
    }
}
