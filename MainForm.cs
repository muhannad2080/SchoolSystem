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

            Instance = this;

            ApplyModernMenuStyle();

            timerClock.Start();

            LoadWelcomeScreen();

            ApplyCurrentUserPermissions();
        }

        private void ApplyModernMenuStyle()
        {
            Color mainColor = Color.FromArgb(30, 41, 59);
            Color accentColor = Color.FromArgb(15, 118, 110);
            Color contentBack = Color.FromArgb(248, 250, 252);
            menuStripMain.BackColor = mainColor;
            menuStripMain.ForeColor = Color.White;
            menuStripMain.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            menuStripMain.Padding = new Padding(8, 6, 8, 6);
            menuStripMain.RenderMode = ToolStripRenderMode.Professional;

            foreach (ToolStripItem item in menuStripMain.Items)
            {
                item.BackColor = mainColor;
                item.ForeColor = Color.White;
                item.Margin = new Padding(2, 0, 2, 0);
                item.Padding = new Padding(8, 0, 8, 0);

                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                    StyleDropDownItems(menuItem);
            }

            panelTop.BackColor = Color.White;
            panelTop.Height = 76;

            lblSystemTitle.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblSystemTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblSystemTitle.Text = "نظام إدارة المدرسة";
            lblSystemTitle.TextAlign = ContentAlignment.MiddleRight;

            lblUsername.BackColor = Color.FromArgb(241, 245, 249);
            lblUsername.ForeColor = accentColor;
            lblUsername.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblUsername.TextAlign = ContentAlignment.MiddleCenter;
            lblUsername.Padding = new Padding(12, 0, 12, 0);

            lblUsername.Text = (lblUsername.Text ?? string.Empty).Replace("👤 ", string.Empty).Trim();

            lblDateTime.ForeColor = Color.FromArgb(100, 116, 139);
            lblDateTime.Font = new Font("Tahoma", 10F);
            lblDateTime.TextAlign = ContentAlignment.MiddleLeft;

            panelContent.BackColor = contentBack;
            panelContent.Padding = new Padding(18);

            statusStripMain.BackColor = Color.White;
            statusStripMain.ForeColor = Color.FromArgb(71, 85, 105);
            statusStripMain.Font = new Font("Tahoma", 9F);
            statusStripMain.Padding = new Padding(12, 0, 12, 0);

            lblDBStatus.ForeColor = Color.FromArgb(22, 163, 74);
            lblOnlineUsers.ForeColor = Color.FromArgb(71, 85, 105);

            this.BackColor = contentBack;
            this.MinimumSize = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "نظام إدارة المدرسة";
        }

        private void StyleDropDownItems(ToolStripMenuItem parent)
        {
            parent.DropDown.BackColor = Color.White;
            parent.DropDown.RightToLeft = RightToLeft.Yes;
            parent.DropDown.Padding = new Padding(4);

            foreach (ToolStripItem item in parent.DropDownItems)
            {
                item.BackColor = Color.White;
                item.ForeColor = Color.FromArgb(30, 41, 59);
                item.Font = new Font("Tahoma", 10F);
                item.Padding = new Padding(8, 4, 8, 4);

                ToolStripMenuItem child = item as ToolStripMenuItem;
                if (child != null)
                    StyleDropDownItems(child);
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy  HH:mm:ss");
        }

        private void LoadWelcomeScreen()
        {
            ClearContentPanel();

            var welcome = new WelcomeScreen();
            welcome.SystemName = " فريق خليها على الله";
            welcome.Dock = DockStyle.Fill;

            panelContent.Controls.Add(welcome);
        }

        public void LoadUserControl(UserControl uc)
        {
            try
            {
                if (uc == null)
                    throw new ArgumentNullException("uc");

                ClearContentPanel();
                uc.Dock = DockStyle.Fill;

                panelContent.Controls.Add(uc);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل الواجهة", ex);
                ShowLoadError("تعذر تحميل الواجهة. حاول مرة أخرى أو تواصل مع مسؤول النظام.");
            }
        }

        public void LoadFormInPanel(Form form)
        {
            try
            {
                if (form == null)
                    throw new ArgumentNullException("form");

                ClearContentPanel();
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;

                panelContent.Controls.Add(form);
                form.Show();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل الواجهة", ex);
                ShowLoadError("تعذر تحميل الواجهة. حاول مرة أخرى أو تواصل مع مسؤول النظام.");
            }
        }

        private void ClearContentPanel()
        {
            Control[] controls = new Control[panelContent.Controls.Count];
            panelContent.Controls.CopyTo(controls, 0);
            panelContent.Controls.Clear();

            foreach (Control control in controls)
            {
                try
                {
                    control.Dispose();
                }
                catch (Exception ex)
                {
                    UIHelper.LogException("تحرير موارد الواجهة", ex);
                }
            }
        }

        private void ShowLoadError(string message)
        {
            panelContent.Controls.Clear();

            Label errorLabel = new Label
            {
                Text = message,
                Font = new Font("Tahoma", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 28, 28),
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

        private void ApplyCurrentUserPermissions()
        {
            if (!CurrentUser.IsLoggedIn)
                return;

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

            tsmiTransport.Visible = Has(PermissionKeys.TransportManage);
            tsmiLibrary.Visible = Has(PermissionKeys.LibraryManage);

            tsmiUsers.Visible = Has(PermissionKeys.UsersManage);
            tsmiReports.Visible = Has(PermissionKeys.ReportsView);

            تعريفرسومالصفوفToolStripMenuItem.Visible = Has(PermissionKeys.FeesManage);
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
                panelContent.Controls.Clear();

                var dashboard = new DashboardHome();
                dashboard.Dock = DockStyle.Fill;

                panelContent.Controls.Add(dashboard);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل لوحة التحكم", ex);
                ShowLoadError("تعذر تحميل لوحة التحكم. حاول مرة أخرى أو تواصل مع مسؤول النظام.");
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

        private void تعريفرسومالصفوفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission(PermissionKeys.FeesManage))
                return;

            LoadUserControl(new FeePlansForm());
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

            Hide();

            using (LoginForm loginForm = new LoginForm())
            {
                DialogResult loginResult = loginForm.ShowDialog();

                if (loginResult == DialogResult.OK)
                {
                    MainForm newMain = new MainForm();
                    newMain.Show();

                    Close();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
