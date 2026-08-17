using SchoolSystem.UI;
using SchoolSystem.Security;
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
            menuStripMain.Padding = new Padding(8, 6, 8, 6);
            menuStripMain.CanOverflow = true;
            menuStripMain.AutoSize = false;
            menuStripMain.Renderer = new CustomMenuRenderer();

            // عناصر حيوية لا يمكن إخفاؤها في زر الفائض عند تصغير النافذة.
            tsmiDashboard.Overflow = ToolStripItemOverflow.Never;
            tsmiLogout.Overflow = ToolStripItemOverflow.Never;

            foreach (ToolStripItem item in menuStripMain.Items)
            {
                item.BackColor = mainColor;
                item.ForeColor = Color.White;
                item.Margin = new Padding(2, 0, 2, 0);
                item.Padding = new Padding(4, 0, 4, 0);

                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                    StyleDropDownItems(menuItem);
            }

            // تسجيل الخروج منفصل بصريًا في أقصى الجهة اليسرى.
            // في شريط قوائم RTL يقصد ToolStripItemAlignment.Right الطرف الأيسر فيزيائيًا.
            tsmiLogout.Alignment = ToolStripItemAlignment.Right;

            // الترويسة: اسم النظام من اليمين وبيانات المستخدم والوقت من اليسار.
            panelTop.BackColor = UIHelper.SurfaceElevatedColor;
            panelTop.Height = 80;

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
            panelContent.Padding = new Padding(18);

            statusStripMain.BackColor = UIHelper.SurfaceElevatedColor;
            statusStripMain.ForeColor = UIHelper.MutedTextColor;
            statusStripMain.Font = new Font(UIHelper.FontFamily, UIHelper.CaptionFontSize);
            statusStripMain.Padding = new Padding(12, 0, 12, 0);

            lblDBStatus.ForeColor = UIHelper.SuccessColor;
            lblOnlineUsers.ForeColor = UIHelper.MutedTextColor;
            lblStatusUser.ForeColor = UIHelper.TextColor;
            lblStatusUser.Font = new Font(UIHelper.FontFamily, UIHelper.CaptionFontSize, FontStyle.Bold);

            this.BackColor = contentBack;
            this.MinimumSize = new Size(1024, 700);
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

                ToolStripMenuItem child = item as ToolStripMenuItem;
                if (child != null)
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

            tsmiDashboard.Visible = Has(PermissionKeys.DashboardView);

            tsmiStudentsManage.Visible = HasAny(PermissionKeys.StudentsView, PermissionKeys.StudentsManage);
            tsmiStudentsEnroll.Visible = Has(PermissionKeys.EnrollmentManage);
            tsmiStudentsClasses.Visible = Has(PermissionKeys.ClassAssignmentManage);

            tsmiTeachersManage.Visible = Has(PermissionKeys.TeachersManage);
            tsmiTeachersAttendance.Visible = Has(PermissionKeys.StaffAttendanceManage);
            tsmiTeachersPayroll.Visible = Has(PermissionKeys.PayrollManage);

            tsmiSubjects.Visible = Has(PermissionKeys.SubjectsManage);
            tsmiClasses.Visible = Has(PermissionKeys.ClassesManage);
            tsmiTimetable.Visible = Has(PermissionKeys.TimetableManage);

            tsmiGrades.Visible = Has(PermissionKeys.GradesManage);
            tsmiAttendance.Visible = Has(PermissionKeys.AttendanceManage);

            tsmiFees.Visible = Has(PermissionKeys.FeesManage);
            tsmiVouchers.Visible = Has(PermissionKeys.VouchersManage);
            tsmiExpenses.Visible = Has(PermissionKeys.ExpensesManage);
            tsmiFinancialPayroll.Visible = Has(PermissionKeys.PayrollManage);

            tsmiTransport.Visible = Has(PermissionKeys.TransportManage);
            tsmiLibrary.Visible = Has(PermissionKeys.LibraryManage);

            tsmiUsers.Visible = Has(PermissionKeys.UsersManage);
            tsmiReports.Visible = Has(PermissionKeys.ReportsView);
            tsmiAuditLogs.Visible = Has(PermissionKeys.AuditLogsView);
            tsmiSettings.Visible = Has(PermissionKeys.SettingsManage);
            تعريفرسومالصفوفToolStripMenuItem.Visible = Has(PermissionKeys.FeesManage);

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
            if (!EnsureAnyPermission(PermissionKeys.StudentsView, PermissionKeys.StudentsManage))
                return;

            LoadFormInPanel(new StudentsForm());
        }

        private void tsmiStudentsEnroll_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.EnrollmentManage))
                return;

            LoadFormInPanel(new SchoolSystem.UI.EnrollmentForm());
        }

        private void tsmiStudentsClasses_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.ClassAssignmentManage))
                return;

            LoadUserControl(new SchoolSystem.UI.Students.ClassAssignmentForm());
        }

        private void tsmiTeachersManage_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.TeachersManage))
                return;

            LoadUserControl(new TeachersForm());
        }

        private void tsmiTeachersAttendance_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.StaffAttendanceManage))
                return;

            LoadUserControl(new StaffAttendanceForm());
        }

        private void tsmiTeachersPayroll_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.PayrollManage))
                return;

            LoadUserControl(new PayrollForm());
        }

        private void tsmiSubjects_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.SubjectsManage))
                return;

            LoadUserControl(new SubjectsForm());
        }

        private void tsmiClasses_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.ClassesManage))
                return;

            LoadUserControl(new ClassesForm());
        }

        private void tsmiTimetable_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.TimetableManage))
                return;

            LoadUserControl(new TimetableForm());
        }

        private void tsmiGrades_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.GradesManage))
                return;

            LoadUserControl(new GradeEntryForm());
        }

        private void tsmiAttendance_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.AttendanceManage))
                return;

            LoadUserControl(new DailyAttendanceForm());
        }

        private void tsmiFees_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.FeesManage))
                return;

            LoadUserControl(new FeesForm());
        }

        private void tsmiVouchers_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.VouchersManage))
                return;

            LoadUserControl(new VouchersForm());
        }

        private void tsmiExpenses_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.ExpensesManage))
                return;

            LoadUserControl(new ExpensesForm());
        }

        private void tsmiTransport_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.TransportManage))
                return;

            LoadUserControl(new TransportForm());
        }

        private void tsmiLibrary_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.LibraryManage))
                return;

            LoadUserControl(new LibraryForm());
        }

        private void tsmiUsers_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.UsersManage))
                return;

            LoadUserControl(new UsersForm());
        }

        private void tsmiReports_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.ReportsView))
                return;

            LoadUserControl(new ReportCenterForm());
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.SettingsManage))
                return;

            LoadUserControl(new SettingsForm());
        }

        private void tsmiAuditLogs_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.AuditLogsView))
                return;

            LoadUserControl(new AuditLogForm());
        }

        private void تعريفرسومالصفوفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.FeesManage))
                return;

            LoadUserControl(new FeePlansForm());
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // لا تترك جلسة صالحة عند إغلاق النافذة مباشرة.
            CurrentUser.Clear();
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
