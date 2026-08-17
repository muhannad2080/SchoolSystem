using System;
using System.Windows.Forms;
using SchoolSystem.UI; // لاحظ أننا نستخدم نفس الـ namespace
using SchoolSystem.Security;

namespace SchoolSystem.UI
{
    public partial class WelcomeScreen : UserControl
    {
        public string SystemName
        {
            get => lblSystemName.Text;
            set => lblSystemName.Text = value;
        }

        public WelcomeScreen()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            this.Dock = DockStyle.Fill;
            ApplyPermissionVisibility();
        }

        private void ApplyPermissionVisibility()
        {
            // هذه الأزرار اختصارات وليست قائمة ثابتة؛ يجب ألا تظهر لأي مستخدم
            // إلا إذا كان يملك صلاحية فعلية للوحدة المرتبطة بها.
            btnStudents.Visible = CurrentUser.CanAccessModule("Students");
            btnTeachers.Visible = CurrentUser.CanAccessModule("Teachers");
            btnFinance.Visible = CurrentUser.CanAccessModule("Fees")
                || CurrentUser.CanAccessModule("Vouchers")
                || CurrentUser.CanAccessModule("Expenses");
            btnAttendance.Visible = CurrentUser.CanAccessModule("Attendance");
        }

        private bool CanOpen(string module, string message)
        {
            if (CurrentUser.CanAccessModule(module))
                return true;

            MessageBox.Show(message, "صلاحية غير كافية", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            return false;
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            if (CanOpen("Students", "ليس لديك صلاحية عرض الطلاب."))
                MainForm.Instance?.LoadFormInPanel(new StudentsForm());
        }

        private void btnTeachers_Click(object sender, EventArgs e)
        {
            if (CanOpen("Teachers", "ليس لديك صلاحية عرض المعلمين."))
                MainForm.Instance?.LoadUserControl(new TeachersForm());
        }

        private void btnFinance_Click(object sender, EventArgs e)
        {
            if (CurrentUser.CanAccessModule("Fees"))
            {
                MainForm.Instance?.LoadUserControl(new FeesForm());
                return;
            }

            if (CurrentUser.CanAccessModule("Vouchers"))
            {
                MainForm.Instance?.LoadUserControl(new VouchersForm());
                return;
            }

            if (CurrentUser.CanAccessModule("Expenses"))
            {
                MainForm.Instance?.LoadUserControl(new ExpensesForm());
                return;
            }

            MessageBox.Show("ليس لديك صلاحية عرض الوحدة المالية.", "صلاحية غير كافية", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            if (CanOpen("Attendance", "ليس لديك صلاحية عرض الحضور."))
                MainForm.Instance?.LoadUserControl(new DailyAttendanceForm());
        }
    }
}