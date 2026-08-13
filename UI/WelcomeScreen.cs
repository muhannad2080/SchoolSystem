using System;
using System.Windows.Forms;
using SchoolSystem.UI; // لاحظ أننا نستخدم نفس الـ namespace

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
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            MainForm.Instance?.LoadFormInPanel(new StudentsForm());
        }

        private void btnTeachers_Click(object sender, EventArgs e)
        {
            // الحل الصحيح: استدعاء الدالة مباشرة دون محاولة تخزين قيمتها لأنها void
            MainForm.Instance?.LoadUserControl(new TeachersForm());
        }

        private void btnFinance_Click(object sender, EventArgs e)
        {
            MainForm.Instance?.LoadUserControl(new FeesForm());
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            MainForm.Instance?.LoadUserControl(new DailyAttendanceForm());
        }
    }
}