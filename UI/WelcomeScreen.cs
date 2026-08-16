using System;
using System.Drawing;
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
            ConfigureResponsiveLayout();
            Resize += (sender, e) => CenterContent();
            CenterContent();
        }

        private void ConfigureResponsiveLayout()
        {
            RightToLeft = RightToLeft.Yes;
            panelBackground.Dock = DockStyle.Fill;
            panelCenter.Anchor = AnchorStyles.None;
            panelCenter.Size = new Size(760, 360);
            panelCenter.BackColor = Color.Transparent;

            lblIcon.AutoSize = false;
            lblIcon.Dock = DockStyle.Top;
            lblIcon.Height = 92;
            lblIcon.Margin = Padding.Empty;
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;

            lblSystemName.AutoSize = false;
            lblSystemName.Dock = DockStyle.Top;
            lblSystemName.Height = 56;
            lblSystemName.Margin = Padding.Empty;
            lblSystemName.TextAlign = ContentAlignment.MiddleCenter;

            lblTagline.AutoSize = false;
            lblTagline.Dock = DockStyle.Top;
            lblTagline.Height = 34;
            lblTagline.Margin = Padding.Empty;
            lblTagline.TextAlign = ContentAlignment.MiddleCenter;

            flowQuickLinks.Dock = DockStyle.Bottom;
            flowQuickLinks.Height = 58;
            flowQuickLinks.Width = panelCenter.ClientSize.Width;
            flowQuickLinks.Margin = Padding.Empty;
            flowQuickLinks.Padding = new Padding(0, 6, 0, 6);
            flowQuickLinks.FlowDirection = FlowDirection.RightToLeft;
            flowQuickLinks.WrapContents = false;
            flowQuickLinks.AutoScroll = false;
            flowQuickLinks.RightToLeft = RightToLeft.Yes;

            Button[] buttons = { btnStudents, btnTeachers, btnFinance, btnAttendance };
            foreach (Button button in buttons)
            {
                button.AutoSize = false;
                button.Size = new Size(164, 44);
                button.Margin = new Padding(4, 0, 4, 0);
                button.Anchor = AnchorStyles.None;
                button.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void CenterContent()
        {
            if (panelBackground == null || panelCenter == null)
                return;

            panelCenter.Left = Math.Max(0, (panelBackground.ClientSize.Width - panelCenter.Width) / 2);
            panelCenter.Top = Math.Max(0, (panelBackground.ClientSize.Height - panelCenter.Height) / 2);
            flowQuickLinks.Width = panelCenter.ClientSize.Width;
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