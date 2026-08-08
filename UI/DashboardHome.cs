using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class DashboardHome : UserControl
    {
        private readonly DashboardService dashboardService = new DashboardService();

        public DashboardHome()
        {
            InitializeComponent();
            UIHelper.ApplyTheme(this);
            this.Dock = DockStyle.Fill;
            this.Load += DashboardHome_Load;
        }

        private async void DashboardHome_Load(object sender, EventArgs e)
        {
            await LoadStatisticsAsync();
            LoadChart();
            LoadAlerts();
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                panelCards.Controls.Clear();

                int studentCount = await Task.Run(() => dashboardService.GetStudentCount());
                int teacherCount = await Task.Run(() => dashboardService.GetTeacherCount());
                int subjectCount = await Task.Run(() => dashboardService.GetSubjectCount());
                int classCount = await Task.Run(() => dashboardService.GetClassCount());

                CreateCard("👨‍🎓  الطلاب", studentCount.ToString(), Color.FromArgb(41, 128, 185), 0);
                CreateCard("👨‍🏫  المعلمين", teacherCount.ToString(), Color.FromArgb(39, 174, 96), 1);
                CreateCard("📚  المواد", subjectCount.ToString(), Color.FromArgb(142, 68, 173), 2);
                CreateCard("🏫  الفصول", classCount.ToString(), Color.FromArgb(230, 126, 34), 3);
                CreateCard("💰  الرسوم", "0", Color.FromArgb(192, 57, 43), 4); // سنحدثه لاحقاً

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("خطأ في تحميل الإحصائيات: " + ex.Message);
            }
        }

        private void CreateCard(string title, string value, Color accentColor, int columnIndex)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(8),
                Dock = DockStyle.Fill
            };

            // شريط لوني علوي
            Panel colorBar = new Panel
            {
                Height = 5,
                BackColor = accentColor,
                Dock = DockStyle.Top
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Tahoma", 10, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Location = new Point(10, 15),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Tahoma", 22, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(10, 45),
                AutoSize = true
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            card.Controls.Add(colorBar);

            panelCards.Controls.Add(card, columnIndex, 0);
        }

        private void LoadChart()
        {
            for (int i = panelChart.Controls.Count - 1; i >= 0; i--)
            {
                Control control = panelChart.Controls[i];

                if (control != lblChartTitle)
                {
                    panelChart.Controls.RemoveAt(i);
                    control.Dispose();
                }
            }

            DataTable dt = dashboardService.GetStudentsPerClass();
            if (dt == null || dt.Rows.Count == 0)
            {
                Label lblNoData = new Label
                {
                    Text = "لا توجد بيانات للعرض",
                    Font = new Font("Tahoma", 10),
                    ForeColor = Color.Gray,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                panelChart.Controls.Add(lblNoData);
                return;
            }

            int barHeight = 35;
            int y = 40;
            int maxWidth = panelChart.Width - 30;

            foreach (DataRow row in dt.Rows)
            {
                string className = row["ClassName"].ToString();
                int count = Convert.ToInt32(row["StudentCount"]);

                Label lblClass = new Label
                {
                    Text = className,
                    Font = new Font("Tahoma", 9),
                    ForeColor = Color.DimGray,
                    Location = new Point(10, y),
                    Size = new Size(120, 20)
                };

                int barWidth = Math.Min(count * 4, maxWidth - 170);
                Panel bar = new Panel
                {
                    BackColor = Color.FromArgb(52, 152, 219),
                    Location = new Point(140, y + 2),
                    Size = new Size(barWidth, 18)
                };

                Label lblCount = new Label
                {
                    Text = count.ToString(),
                    Font = new Font("Tahoma", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(33, 42, 57),
                    Location = new Point(145 + barWidth, y),
                    AutoSize = true
                };

                panelChart.Controls.Add(lblClass);
                panelChart.Controls.Add(bar);
                panelChart.Controls.Add(lblCount);

                y += barHeight;
            }
        }

        private async void LoadAlerts()
        {
            try
            {
                for (int i = panelAlerts.Controls.Count - 1; i >= 0; i--)
                {
                    Control control = panelAlerts.Controls[i];

                    if (control != lblAlertsTitle)
                    {
                        panelAlerts.Controls.RemoveAt(i);
                        control.Dispose();
                    }
                }

                int pendingFees = await Task.Run(() => dashboardService.GetPendingFeesCount());
                int todayAbsence = await Task.Run(() => dashboardService.GetTodayAbsenceCount());

                lblPendingFees.Text = $"⚠️  رسوم غير مدفوعة: {pendingFees}";
                lblTodayAbsence.Text = $"📅  غياب اليوم: {todayAbsence}";
            }
            catch
            {
                lblPendingFees.Text = "⚠️  رسوم غير مدفوعة: --";
                lblTodayAbsence.Text = "📅  غياب اليوم: --";
            }
        }
    }
}