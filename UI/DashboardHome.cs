using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    /// <summary>
    /// لوحة المعلومات.
    /// التصميم الثابت للواجهة موجود بالكامل في DashboardHome.Designer.cs.
    /// هذا الملف مسؤول عن الأحداث وتحميل البيانات ومعالجتها فقط.
    /// </summary>
    public partial class DashboardHome : UserControl
    {
        private readonly DashboardService dashboardService = new DashboardService();

        private bool isDashboardLoading = false;

        public DashboardHome()
        {
            InitializeComponent();

            UIHelper.ApplyStyle(this);
            Dock = DockStyle.Fill;

            // لا يتم تطبيق أي Theme أو Style هنا حتى لا يتعارض
            // مع التصميم المعرفة خصائصه في ملف Designer.

            btnRefreshDashboard.Click += async (sender, e) => await ReloadDashboardAsync();

            this.Load += DashboardHome_Load;
        }

        private async void DashboardHome_Load(object sender, EventArgs e)
        {
            await ReloadDashboardAsync();
        }

        private async Task ReloadDashboardAsync()
        {
            if (isDashboardLoading)
                return;

            isDashboardLoading = true;

            try
            {
                btnRefreshDashboard.Enabled = false;
                Cursor = Cursors.WaitCursor;

                await LoadActiveYearAsync();
                await LoadStatisticsAsync();
                await LoadChartAsync();
                await LoadAlertsAsync();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعذر تحديث لوحة التحكم", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnRefreshDashboard.Enabled = true;
                isDashboardLoading = false;
            }
        }

        private async Task LoadActiveYearAsync()
        {
            try
            {
                string year = await Task.Run(
                    () => new AnnualClosingService().GetActiveAcademicYear());

                lblActiveAcademicYear.Text =
                    string.IsNullOrWhiteSpace(year)
                        ? "العام النشط: غير محدد"
                        : "العام النشط: " + year;
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "تحميل العام الدراسي النشط للوحة المعلومات",
                    ex);

                lblActiveAcademicYear.Text =
                    "العام النشط: تعذر التحميل";
            }
        }

        private async Task LoadStatisticsAsync()
        {
            // قيم انتظار فقط إلى حين وصول البيانات الحقيقية.
            lblStudentsValue.Text = "--";
            lblTeachersValue.Text = "--";
            lblSubjectsValue.Text = "--";
            lblClassesValue.Text = "--";
            lblFeesValue.Text = "--";

            try
            {
                int studentCount =
                    await Task.Run(() => dashboardService.GetStudentCount());

                int teacherCount =
                    await Task.Run(() => dashboardService.GetTeacherCount());

                int subjectCount =
                    await Task.Run(() => dashboardService.GetSubjectCount());

                int classCount =
                    await Task.Run(() => dashboardService.GetClassCount());

                decimal pendingFeesTotal =
                    await Task.Run(() => dashboardService.GetPendingFeesTotal());

                lblStudentsValue.Text = studentCount.ToString();
                lblTeachersValue.Text = teacherCount.ToString();
                lblSubjectsValue.Text = subjectCount.ToString();
                lblClassesValue.Text = classCount.ToString();
                lblFeesValue.Text = pendingFeesTotal.ToString("N2");
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "تحميل إحصائيات لوحة المعلومات",
                    ex);

                lblStudentsValue.Text = "خطأ";
                lblTeachersValue.Text = "خطأ";
                lblSubjectsValue.Text = "خطأ";
                lblClassesValue.Text = "خطأ";
                lblFeesValue.Text = "خطأ";
            }
        }

        /// <summary>
        /// تحميل مخطط الطلاب حسب الصف.
        /// عناصر المخطط نفسها تنشأ ديناميكيًا لأنها تعتمد على
        /// عدد الصفوف وبيانات قاعدة البيانات.
        /// </summary>
        private async Task LoadChartAsync()
        {
            panelChartBody.SuspendLayout();

            try
            {
                for (int i = panelChartBody.Controls.Count - 1; i >= 0; i--)
                {
                    Control existing = panelChartBody.Controls[i];

                    panelChartBody.Controls.RemoveAt(i);
                    existing.Dispose();
                }

                Label loading = new Label
                {
                    Text = "جاري تحميل المخطط...",
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    RightToLeft = RightToLeft.Yes
                };

                panelChartBody.Controls.Add(loading);
            }
            finally
            {
                panelChartBody.ResumeLayout();
            }

            DataTable dt;

            try
            {
                dt = await Task.Run(
                    () => dashboardService.GetStudentsPerClass());
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "تحميل مخطط الطلاب حسب الصف",
                    ex);

                panelChartBody.SuspendLayout();

                for (int i = panelChartBody.Controls.Count - 1; i >= 0; i--)
                {
                    Control existing = panelChartBody.Controls[i];

                    panelChartBody.Controls.RemoveAt(i);
                    existing.Dispose();
                }

                Label errorLabel = new Label
                {
                    Text = "تعذر تحميل بيانات المخطط",
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    RightToLeft = RightToLeft.Yes
                };

                panelChartBody.Controls.Add(errorLabel);

                panelChartBody.ResumeLayout();

                return;
            }

            panelChartBody.SuspendLayout();

            for (int i = panelChartBody.Controls.Count - 1; i >= 0; i--)
            {
                Control existing = panelChartBody.Controls[i];

                panelChartBody.Controls.RemoveAt(i);
                existing.Dispose();
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                Label lblNoData = new Label
                {
                    Text = "لا توجد بيانات للعرض",
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    RightToLeft = RightToLeft.Yes
                };

                panelChartBody.Controls.Add(lblNoData);
                panelChartBody.ResumeLayout();

                return;
            }

            long maxCount = 1;

            foreach (DataRow row in dt.Rows)
            {
                long count = Convert.ToInt64(row["StudentCount"]);

                if (count > maxCount)
                    maxCount = count;
            }

            int rowHeight = 34;
            int y = 8;

            int availableWidth =
                Math.Max(160, panelChartBody.ClientSize.Width - 120);

            foreach (DataRow row in dt.Rows)
            {
                string className =
                    row["ClassName"] == DBNull.Value
                        ? string.Empty
                        : row["ClassName"].ToString();

                int count =
                    row["StudentCount"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(row["StudentCount"]);

                Label lblClass = new Label
                {
                    Text = className,
                    Location = new System.Drawing.Point(8, y + 2),
                    Size = new System.Drawing.Size(110, 20),
                    AutoEllipsis = true,
                    TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                    RightToLeft = RightToLeft.Yes
                };

                int barWidth =
                    (int)Math.Max(
                        6,
                        (double)count / maxCount * availableWidth);

                Panel bar = new Panel
                {
                    Location = new System.Drawing.Point(124, y + 5),
                    Size = new System.Drawing.Size(barWidth, 16)
                };

                Label lblCount = new Label
                {
                    Text = count.ToString(),
                    Location = new System.Drawing.Point(
                        128 + barWidth,
                        y + 2),
                    AutoSize = true,
                    RightToLeft = RightToLeft.No
                };

                panelChartBody.Controls.Add(lblClass);
                panelChartBody.Controls.Add(bar);
                panelChartBody.Controls.Add(lblCount);

                y += rowHeight;
            }

            panelChartBody.ResumeLayout();
        }

        private async Task LoadAlertsAsync()
        {
            lblPendingFees.Text =
                "الرسوم غير المدفوعة: جاري التحميل...";

            lblTodayAbsence.Text =
                "غياب اليوم: جاري التحميل...";

            try
            {
                int pendingFees =
                    await Task.Run(
                        () => dashboardService.GetPendingFeesCount());

                int todayAbsence =
                    await Task.Run(
                        () => dashboardService.GetTodayAbsenceCount());

                lblPendingFees.Text =
                    $"رسوم غير مدفوعة: {pendingFees}";

                lblTodayAbsence.Text =
                    $"غياب اليوم: {todayAbsence}";

                await LoadOperationalStatusAsync();
            }
            catch (Exception ex)
            {
                lblPendingFees.Text =
                    "الرسوم غير المدفوعة: تعذر التحميل";

                lblTodayAbsence.Text =
                    "غياب اليوم: تعذر التحميل";

                lblOperationalStatus.Text =
                    "حالة التشغيل: تعذر التحميل";

                ApplicationLogger.LogException(
                    "تحميل تنبيهات لوحة المعلومات",
                    ex);
            }
        }

        private async Task LoadOperationalStatusAsync()
        {
            try
            {
                DataTable table =
                    await Task.Run(
                        () => dashboardService.GetOperationalStatus());

                if (table == null || table.Rows.Count == 0)
                {
                    lblOperationalStatus.Text =
                        "حالة التشغيل: لم تُطبّق بنية المتابعة بعد";

                    return;
                }

                DataRow row = table.Rows[0];

                string backupFile =
                    row["LastBackupFile"] == DBNull.Value
                        ? string.Empty
                        : row["LastBackupFile"].ToString();

                string backupDate =
                    row["LastBackupDate"] == DBNull.Value
                        ? "غير متوفر"
                        : Convert
                            .ToDateTime(row["LastBackupDate"])
                            .ToString("yyyy-MM-dd HH:mm");

                string closingYear =
                    row["LastClosingYear"] == DBNull.Value
                        ? "غير متوفر"
                        : row["LastClosingYear"].ToString();

                string closingStatus =
                    row["LastClosingStatus"] == DBNull.Value
                        ? "غير متوفر"
                        : row["LastClosingStatus"].ToString();

                lblOperationalStatus.Text =
                    "آخر نسخة احتياطية: "
                    + (string.IsNullOrWhiteSpace(backupFile)
                        ? "غير متوفرة"
                        : backupDate)
                    + Environment.NewLine
                    + "آخر إغلاق سنوي: "
                    + closingYear
                    + " / "
                    + closingStatus;
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "تحميل حالة التشغيل للوحة المعلومات",
                    ex);

                lblOperationalStatus.Text =
                    "حالة التشغيل: تعذر تحميل النسخة أو الإغلاق";
            }
        }
    }
}
