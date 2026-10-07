using SchoolSystem.UI;
using SchoolSystem.Security;
using SchoolSystem.Services;
using SchoolSystem.Helpers;

using System;
using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Models;

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

        private PictureBox schoolLogoBox;
        private Image schoolLogoImage;

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
            InitializeSchoolBranding();

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

            lblSystemTitle.Visible =
                true;

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
            InitializeSchoolBranding();
        }

        private void InitializeSchoolBranding()
        {
            EnsureSchoolLogoControl();

            SchoolSettings settings = SchoolSettingsService.Current();
            if (settings == null)
                return;

            string schoolName = !string.IsNullOrWhiteSpace(settings.SchoolNameAr)
                ? settings.SchoolNameAr.Trim()
                : (!string.IsNullOrWhiteSpace(settings.SchoolNameEn)
                    ? settings.SchoolNameEn.Trim()
                    : "نظام إدارة المدرسة");

            lblSystemTitle.Text = schoolName;
            lblSystemTitle.TextAlign = ContentAlignment.MiddleRight;
            lblSystemTitle.Font = new Font("Tahoma", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblSystemTitle.ForeColor = Color.FromArgb(22, 50, 79);

            lblSystemSubtitle.Text = BuildSchoolHeaderSubtitle(settings);
            lblSystemSubtitle.TextAlign = ContentAlignment.MiddleRight;
            lblSystemSubtitle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblSystemSubtitle.ForeColor = Color.FromArgb(71, 85, 105);

            LoadSchoolLogo(settings);
        }

        private static string BuildSchoolHeaderSubtitle(SchoolSettings settings)
        {
            var parts = new System.Collections.Generic.List<string>();

            if (!string.IsNullOrWhiteSpace(settings.OrganizationNameAr))
                parts.Add(settings.OrganizationNameAr.Trim());

            if (!string.IsNullOrWhiteSpace(settings.AddressAr))
                parts.Add(settings.AddressAr.Trim());

            if (!string.IsNullOrWhiteSpace(settings.Phone))
                parts.Add("هاتف: " + settings.Phone.Trim());

            if (!string.IsNullOrWhiteSpace(settings.Email))
                parts.Add(settings.Email.Trim());

            string result = string.Join("   •   ", parts);
            return string.IsNullOrWhiteSpace(result) ? "نظام إدارة المدرسة" : result;
        }

        private void EnsureSchoolLogoControl()
        {
            if (schoolLogoBox != null && !schoolLogoBox.IsDisposed)
                return;

            schoolLogoBox = new PictureBox
            {
                Name = "schoolLogoBox",
                Size = new Size(76, 76),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TabStop = false
            };

            panelTop.Controls.Add(schoolLogoBox);
            schoolLogoBox.BringToFront();

            panelTop.Resize += delegate
            {
                schoolLogoBox.Left = panelTop.ClientSize.Width - schoolLogoBox.Width - 18;
                schoolLogoBox.Top = Math.Max(6, (panelTop.ClientSize.Height - schoolLogoBox.Height) / 2);
            };

            schoolLogoBox.Left = panelTop.ClientSize.Width - schoolLogoBox.Width - 18;
            schoolLogoBox.Top = Math.Max(6, (panelTop.ClientSize.Height - schoolLogoBox.Height) / 2);
        }

        private void LoadSchoolLogo(SchoolSettings settings)
        {
            if (schoolLogoBox == null)
                return;

            if (schoolLogoImage != null)
            {
                schoolLogoBox.Image = null;
                schoolLogoImage.Dispose();
                schoolLogoImage = null;
            }

            if (settings.Logo == null || settings.Logo.Length == 0)
            {
                schoolLogoBox.Visible = false;
                tblHeader.Padding = new Padding(0);
                return;
            }

            try
            {
                using (var stream = new System.IO.MemoryStream(settings.Logo))
                using (Image source = Image.FromStream(stream))
                {
                    schoolLogoImage = new Bitmap(source);
                }

                schoolLogoBox.Image = schoolLogoImage;
                schoolLogoBox.Visible = true;
                tblHeader.Padding = new Padding(0, 0, 92, 0);
            }
            catch (Exception ex)
            {
                schoolLogoBox.Visible = false;
                tblHeader.Padding = new Padding(0);
                ApplicationLogger.LogException("تحميل شعار المدرسة في الترويسة", ex);
            }
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

            PermissionUiHelper.Apply(uc, module);
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

            PermissionUiHelper.Apply(form, module);
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

            UIHelper.ShowWarning(message);

            return false;
        }

        private bool EnsurePermission(
            string permission)
        {
            if (Has(permission))
                return true;

            UIHelper.ShowWarning("ليس لديك صلاحية للوصول إلى هذه الشاشة.");

            return false;
        }

        private bool EnsureAnyPermission(
            params string[] permissions)
        {
            if (HasAny(permissions))
                return true;

            UIHelper.ShowWarning("ليس لديك صلاحية للوصول إلى هذه الشاشة.");

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
                tsmiSections,
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
                    "TeacherContracts",
                    "TeacherContracts.View",
                    "TeacherContracts.Manage");

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

            tsmiSections.Visible =
                CanOpen(
                    "Sections",
                    "Sections.View",
                    PermissionKeys.SectionsManage);

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
                CanOpen("TeacherContracts") ||
                CanOpen("Payroll");

            tsmiAcademic.Visible =
                CanOpen("Subjects") ||
                CanOpen("Classes") ||
                CanOpen("Sections") ||
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
                "TeacherContracts",
                "ليس لديك صلاحية عرض عقود المعلمين.",
                new PayrollForm(),
                "TeacherContracts.View",
                "TeacherContracts.Manage");
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

        private void tsmiSections_Click(object sender, EventArgs e)
        {
            LoadUserControl(
                "Sections",
                "ليس لديك صلاحية عرض الشعب الدراسية.",
                new SchoolSectionsForm(),
                "Sections.View",
                PermissionKeys.SectionsManage);
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

            if (schoolLogoImage != null)
            {
                schoolLogoBox.Image = null;
                schoolLogoImage.Dispose();
                schoolLogoImage = null;
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
            bool confirmed =
                UIHelper.ShowConfirmation("هل تريد تسجيل الخروج؟", "تسجيل الخروج");

            if (!confirmed)
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
