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
        private readonly Button btnRefreshDashboard = new Button();

        public DashboardHome()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            UIHelper.ApplyTheme(this);
            this.Dock = DockStyle.Fill;
            ConfigureDashboardActions();
            this.Load += DashboardHome_Load;
        }

        private void ConfigureDashboardActions()
        {
            btnRefreshDashboard.Text = "تحديث البيانات";
            btnRefreshDashboard.Name = "btnRefreshDashboard";
            btnRefreshDashboard.Size = new Size(125, 34);
            btnRefreshDashboard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshDashboard.Location = new Point(Math.Max(12, Width - btnRefreshDashboard.Width - 18), 16);
            btnRefreshDashboard.BackColor = UIHelper.InfoColor;
            btnRefreshDashboard.ForeColor = Color.White;
            btnRefreshDashboard.FlatStyle = FlatStyle.Flat;
            btnRefreshDashboard.FlatAppearance.BorderSize = 0;
            btnRefreshDashboard.Font = new Font(UIHelper.FontFamily, UIHelper.CaptionFontSize, FontStyle.Bold);
            btnRefreshDashboard.Click += async (sender, e) => await ReloadDashboardAsync();
            Controls.Add(btnRefreshDashboard);
            btnRefreshDashboard.BringToFront();
            Resize += (sender, e) => btnRefreshDashboard.Location = new Point(Math.Max(12, Width - btnRefreshDashboard.Width - 18), 16);
        }

        private async void DashboardHome_Load(object sender, EventArgs e)
        {
            await ReloadDashboardAsync();
        }

        private async Task ReloadDashboardAsync()
        {
            try
            {
                btnRefreshDashboard.Enabled = false;
                Cursor = Cursors.WaitCursor;
                await LoadStatisticsAsync();
                LoadChart();
                await LoadAlertsAsync();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعذر تحديث لوحة التحكم: ", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnRefreshDashboard.Enabled = true;
            }
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                panelCards.Controls.Clear();

                int studentCount = await Task.Run(() => dashboardService.GetStudentCount());
                int teacherCount = await Task.Run(() => dashboardService.GetTeacherCount());
                int subjectCount = await Task.Run(() => dashboardService.GetSubjectCount());
                int classCount = await Task.Run(() => dashboardService.GetClassCount());
                decimal pendingFeesTotal = await Task.Run(() => dashboardService.GetPendingFeesTotal());

                CreateCard("الطلاب", studentCount.ToString(), UIHelper.InfoColor, 0);
                CreateCard("المعلمون", teacherCount.ToString(), UIHelper.SuccessColor, 1);
                CreateCard("المواد", subjectCount.ToString(), UIHelper.AccentColor, 2);
                CreateCard("الفصول", classCount.ToString(), UIHelper.WarningColor, 3);
                CreateCard("الرسوم المتبقية", pendingFeesTotal.ToString("N2"), UIHelper.DangerColor, 4);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("خطأ في تحميل الإحصائيات: ", ex);
            }
        }

        private void CreateCard(string title, string value, Color accentColor, int columnIndex)
        {
            Panel card = new Panel
            {
                BackColor = UIHelper.SurfaceElevatedColor,
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
                Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize, FontStyle.Bold),
                ForeColor = UIHelper.MutedTextColor,
                Location = new Point(10, 15),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font(UIHelper.FontFamily, UIHelper.TitleFontSize + 6F, FontStyle.Bold),
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
                    Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize),
                    ForeColor = UIHelper.MutedTextColor,
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
                    Font = new Font(UIHelper.FontFamily, UIHelper.CaptionFontSize),
                    ForeColor = UIHelper.MutedTextColor,
                    Location = new Point(10, y),
                    Size = new Size(120, 20)
                };

                int barWidth = Math.Min(count * 4, maxWidth - 170);
                Panel bar = new Panel
                {
                    BackColor = UIHelper.InfoColor,
                    Location = new Point(140, y + 2),
                    Size = new Size(barWidth, 18)
                };

                Label lblCount = new Label
                {
                    Text = count.ToString(),
                    Font = new Font(UIHelper.FontFamily, UIHelper.CaptionFontSize, FontStyle.Bold),
                    ForeColor = UIHelper.TextColor,
                    Location = new Point(145 + barWidth, y),
                    AutoSize = true
                };

                panelChart.Controls.Add(lblClass);
                panelChart.Controls.Add(bar);
                panelChart.Controls.Add(lblCount);

                y += barHeight;
            }
        }

        private async Task LoadAlertsAsync()
        {
            try
            {
                for (int i = panelAlerts.Controls.Count - 1; i >= 0; i--)
                {
                    Control control = panelAlerts.Controls[i];

                    if (control != lblAlertsTitle &&
                        control != lblPendingFees &&
                        control != lblTodayAbsence)
                    {
                        panelAlerts.Controls.RemoveAt(i);
                        control.Dispose();
                    }
                }

                lblPendingFees.Text = "الرسوم غير المدفوعة: جاري التحميل...";
                lblTodayAbsence.Text = "غياب اليوم: جاري التحميل...";
                panelAlerts.PerformLayout();

                int pendingFees = await Task.Run(() => dashboardService.GetPendingFeesCount());
                int todayAbsence = await Task.Run(() => dashboardService.GetTodayAbsenceCount());

                lblPendingFees.Text = $"رسوم غير مدفوعة: {pendingFees}";
                lblTodayAbsence.Text = $"غياب اليوم: {todayAbsence}";
            }
            catch (Exception ex)
            {
                lblPendingFees.Text = "الرسوم غير المدفوعة: تعذر التحميل";
                lblTodayAbsence.Text = "غياب اليوم: تعذر التحميل";
                UIHelper.ShowException("تعذر تحميل تنبيهات لوحة التحكم: ", ex);
            }
        }
    }
}