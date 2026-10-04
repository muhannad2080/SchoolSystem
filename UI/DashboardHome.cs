using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Security;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    /// <summary>
    /// لوحة التحكم الرئيسية للنظام.
    /// تعرض مؤشرات حقيقية من قاعدة البيانات وتعيد ترتيب العرض
    /// تلقائيًا عند تغير حجم المساحة المتاحة.
    /// </summary>
    public partial class DashboardHome : UserControl
    {
        private readonly DashboardService dashboardService = new DashboardService();

        private DataTable studentsPerClassTable;
        private Timer dashboardClockTimer;
        private bool isDashboardLoading;

        public DashboardHome()
        {
            InitializeComponent();

            UIHelper.ApplyKryptonTheme();
            UIHelper.ApplyResponsiveLayout(this);
            UIHelper.ApplyFieldValidation(this);
            UIHelper.StyleButton(btnRefreshDashboard, UIHelper.InfoColor);

            ConfigureDashboardVisuals();
            ConfigureArabicDashboardLayout();

            btnRefreshDashboard.Click += async (sender, e) => await ReloadDashboardAsync();
            panelCards.Resize += (sender, e) => ResizeDashboardCards();
            panelChartBody.Resize += (sender, e) => { if (studentsPerClassTable != null) RenderChart(); };
            Load += DashboardHome_Load;
            Disposed += DashboardHome_Disposed;

            ResizeDashboardCards();
        }

        /// <summary>
        /// يوحّد اتجاه النصوص وتسميات لوحة التحكم لتكون عربية وواضحة ومتناسقة.
        /// </summary>
        private void ConfigureArabicDashboardLayout()
        {
            RightToLeft = RightToLeft.Yes;

            lblDashboardTitle.Text = "لوحة التحكم";
            lblDashboardSubtitle.Text = "ملخص حي لمؤشرات المدرسة وحالتها التشغيلية";
            btnRefreshDashboard.Text = "تحديث البيانات";

            lblStudentsTitle.Text = "إجمالي الطلاب";
            lblTeachersTitle.Text = "المعلمون";
            lblSubjectsTitle.Text = "المواد الدراسية";
            lblClassesTitle.Text = "الفصول الدراسية";
            lblFeesTitle.Text = "إجمالي الرسوم المتبقية";

            lblAlertsTitle.Text = "المتابعة والتنبيهات";
            lblChartTitle.Text = "توزيع الطلاب حسب الفصل";

            Control[] arabicControls =
            {
                panelHeader, tlpHeader, pnlTitle, pnlMeta,
                tableLayoutMain, tableLayoutLower, panelAlerts,
                panelChart, panelChartBody
            };

            foreach (Control control in arabicControls)
            {
                if (control != null)
                    control.RightToLeft = RightToLeft.Yes;
            }

            Label[] arabicLabels =
            {
                lblDashboardTitle, lblDashboardSubtitle, lblWelcome,
                lblActiveAcademicYear, lblCurrentDateTime, lblLastUpdated,
                lblStudentsTitle, lblTeachersTitle, lblSubjectsTitle,
                lblClassesTitle, lblFeesTitle, lblAlertsTitle,
                lblPendingFees, lblTodayAbsence, lblOperationalStatus,
                lblChartTitle
            };

            foreach (Label label in arabicLabels)
            {
                label.RightToLeft = RightToLeft.Yes;
                label.TextAlign = ContentAlignment.MiddleRight;
            }

            Label[] numericLabels =
            {
                lblStudentsValue, lblTeachersValue,
                lblSubjectsValue, lblClassesValue, lblFeesValue
            };

            foreach (Label label in numericLabels)
            {
                label.RightToLeft = RightToLeft.No;
                label.TextAlign = ContentAlignment.MiddleRight;
            }

            lblOperationalStatus.AutoEllipsis = false;
            panelChartBody.BackColor = UIHelper.SurfaceColor;
            panelChartBody.AutoScroll = true;
        }

        private void ConfigureDashboardVisuals()
        {
            BackColor = UIHelper.BackgroundColor;

            panelHeader.BackColor = UIHelper.PrimaryDarkColor;

            lblDashboardTitle.ForeColor = Color.White;
            lblDashboardSubtitle.ForeColor = UIHelper.PrimaryLightColor;
            lblWelcome.ForeColor = UIHelper.PrimaryLightColor;

            lblActiveAcademicYear.ForeColor = Color.White;
            lblCurrentDateTime.ForeColor = UIHelper.PrimaryLightColor;
            lblLastUpdated.ForeColor = UIHelper.PrimaryLightColor;

            UIHelper.StyleButton(btnRefreshDashboard, UIHelper.AccentColor);
            btnRefreshDashboard.ForeColor = Color.White;

            ConfigureCard(
                cardStudents,
                cardStudentsBar,
                lblStudentsTitle,
                lblStudentsValue,
                UIHelper.InfoColor);

            ConfigureCard(
                cardTeachers,
                cardTeachersBar,
                lblTeachersTitle,
                lblTeachersValue,
                UIHelper.SuccessColor);

            ConfigureCard(
                cardSubjects,
                cardSubjectsBar,
                lblSubjectsTitle,
                lblSubjectsValue,
                UIHelper.AccentColor);

            ConfigureCard(
                cardClasses,
                cardClassesBar,
                lblClassesTitle,
                lblClassesValue,
                UIHelper.WarningColor);

            ConfigureCard(
                cardFees,
                cardFeesBar,
                lblFeesTitle,
                lblFeesValue,
                UIHelper.DangerColor);

            panelChart.BackColor = UIHelper.SurfaceColor;
            panelAlerts.BackColor = UIHelper.SurfaceColor;

            lblChartTitle.ForeColor = UIHelper.TextPrimaryColor;
            lblAlertsTitle.ForeColor = UIHelper.TextPrimaryColor;
            lblAlertsTitle.BackColor = UIHelper.SurfaceSecondaryColor;

            StyleStatusLabel(
                lblPendingFees,
                UIHelper.SurfaceSecondaryColor,
                UIHelper.WarningColor);

            StyleStatusLabel(
                lblTodayAbsence,
                UIHelper.SurfaceSecondaryColor,
                UIHelper.DangerColor);

            StyleStatusLabel(
                lblOperationalStatus,
                UIHelper.SurfaceSecondaryColor,
                UIHelper.SecondaryColor);
        }

        private static void ConfigureCard(
            Panel card,
            Panel accentBar,
            Label title,
            Label value,
            Color accent)
        {
            card.BackColor = UIHelper.SurfaceColor;
            card.BorderStyle = BorderStyle.FixedSingle;

            accentBar.BackColor = accent;

            title.ForeColor = UIHelper.TextSecondaryColor;
            value.ForeColor = accent;
        }

        private static void StyleStatusLabel(
            Label label,
            Color background,
            Color foreground)
        {
            label.BackColor = background;
            label.ForeColor = foreground;
            label.BorderStyle = BorderStyle.FixedSingle;
        }

        private void ResizeDashboardCards()
        {
            if (panelCards == null || panelCards.IsDisposed)
                return;

            int width = Math.Max(320, panelCards.ClientSize.Width - 4);

            int columns =
                width >= 960
                    ? 5
                    : width >= 700
                        ? 3
                        : 2;

            int horizontalMargins = columns * 12;
            int cardWidth = Math.Max(
                150,
                (width - horizontalMargins) / columns);

            Control[] cards =
            {
                cardStudents,
                cardTeachers,
                cardSubjects,
                cardClasses,
                cardFees
            };

            foreach (Control card in cards)
                card.Width = cardWidth;
        }

        private void DashboardHome_Load(object sender, EventArgs e)
        {
            UpdateHeaderContext();

            dashboardClockTimer = new Timer
            {
                Interval = 30000
            };

            dashboardClockTimer.Tick += (timerSender, timerEventArgs) =>
            {
                UpdateHeaderContext();
            };

            dashboardClockTimer.Start();

            _ = ReloadDashboardAsync();
        }

        private void DashboardHome_Disposed(object sender, EventArgs e)
        {
            if (dashboardClockTimer == null)
                return;

            dashboardClockTimer.Stop();
            dashboardClockTimer.Dispose();
            dashboardClockTimer = null;
        }

        private void UpdateHeaderContext()
        {
            string displayName = "المستخدم";

            if (CurrentUser.User != null)
            {
                displayName = CurrentUser.User.FullName;

                if (string.IsNullOrWhiteSpace(displayName))
                    displayName = CurrentUser.User.UserName;
            }

            if (string.IsNullOrWhiteSpace(displayName))
                displayName = "المستخدم";

            lblWelcome.Text = "مرحبًا، " + displayName;
            lblCurrentDateTime.Text =
                "التاريخ والوقت: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        }

        private async Task ReloadDashboardAsync()
        {
            if (isDashboardLoading)
                return;

            isDashboardLoading = true;

            try
            {
                btnRefreshDashboard.Enabled = false;
                btnRefreshDashboard.Text = "جاري التحديث...";
                Cursor = Cursors.WaitCursor;

                await Task.WhenAll(
                    LoadActiveYearAsync(),
                    LoadStatisticsAsync(),
                    LoadChartAsync(),
                    LoadAlertsAsync());

                lblLastUpdated.Text =
                    "آخر تحديث: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعذر تحديث لوحة التحكم", ex);

                lblLastUpdated.Text =
                    "آخر تحديث: تعذر الاكتمال";
            }
            finally
            {
                Cursor = Cursors.Default;
                btnRefreshDashboard.Text = "تحديث البيانات";
                btnRefreshDashboard.Enabled = true;
                isDashboardLoading = false;
            }
        }

        private async Task LoadActiveYearAsync()
        {
            string year = await RunSafeQueryAsync(
                () => new AnnualClosingService().GetActiveAcademicYear(),
                "تحميل العام الدراسي النشط للوحة التحكم",
                string.Empty);

            lblActiveAcademicYear.Text =
                string.IsNullOrWhiteSpace(year)
                    ? "العام الدراسي: غير محدد"
                    : "العام الدراسي: " + year.Trim().Replace('-', '/');
        }

        private async Task LoadStatisticsAsync()
        {
            lblStudentsValue.Text = "--";
            lblTeachersValue.Text = "--";
            lblSubjectsValue.Text = "--";
            lblClassesValue.Text = "--";
            lblFeesValue.Text = "--";

            Task<int> studentsTask = RunSafeQueryAsync(
                () => dashboardService.GetStudentCount(),
                "تحميل عدد الطلاب للوحة التحكم",
                0);

            Task<int> teachersTask = RunSafeQueryAsync(
                () => dashboardService.GetTeacherCount(),
                "تحميل عدد المعلمين للوحة التحكم",
                0);

            Task<int> subjectsTask = RunSafeQueryAsync(
                () => dashboardService.GetSubjectCount(),
                "تحميل عدد المواد للوحة التحكم",
                0);

            Task<int> classesTask = RunSafeQueryAsync(
                () => dashboardService.GetClassCount(),
                "تحميل عدد الفصول للوحة التحكم",
                0);

            Task<decimal> feesTask = RunSafeQueryAsync(
                () => dashboardService.GetPendingFeesTotal(),
                "تحميل إجمالي الرسوم المتبقية للوحة التحكم",
                0m);

            await Task.WhenAll(
                studentsTask,
                teachersTask,
                subjectsTask,
                classesTask,
                feesTask);

            lblStudentsValue.Text = studentsTask.Result.ToString("N0");
            lblTeachersValue.Text = teachersTask.Result.ToString("N0");
            lblSubjectsValue.Text = subjectsTask.Result.ToString("N0");
            lblClassesValue.Text = classesTask.Result.ToString("N0");
            lblFeesValue.Text = feesTask.Result.ToString("N2");
        }

        private async Task LoadChartAsync()
        {
            DataTable table = await RunSafeQueryAsync(
                () => dashboardService.GetStudentsPerClass(),
                "تحميل توزيع الطلاب حسب الفصل للوحة التحكم",
                null);

            studentsPerClassTable = table;
            RenderChart();
        }

        private void RenderChart()
        {
            if (panelChartBody == null || panelChartBody.IsDisposed)
                return;

            panelChartBody.SuspendLayout();

            try
            {
                for (int i = panelChartBody.Controls.Count - 1; i >= 0; i--)
                {
                    Control existing = panelChartBody.Controls[i];
                    panelChartBody.Controls.RemoveAt(i);
                    existing.Dispose();
                }

                if (studentsPerClassTable == null ||
                    studentsPerClassTable.Rows.Count == 0)
                {
                    Label noData = CreateChartMessage("لا توجد بيانات فعلية للعرض في المخطط.");
                    panelChartBody.Controls.Add(noData);
                    return;
                }

                long maxCount = 1;

                foreach (DataRow row in studentsPerClassTable.Rows)
                {
                    long count = row["StudentCount"] == DBNull.Value
                        ? 0
                        : Convert.ToInt64(row["StudentCount"]);

                    if (count > maxCount)
                        maxCount = count;
                }

                FlowLayoutPanel rows = new FlowLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    WrapContents = false,
                    FlowDirection = FlowDirection.TopDown,
                    RightToLeft = RightToLeft.Yes,
                    Padding = new Padding(0),
                    Margin = new Padding(0),
                    Width = Math.Max(240, panelChartBody.ClientSize.Width - 28)
                };

                foreach (DataRow row in studentsPerClassTable.Rows)
                {
                    string className = row["ClassName"] == DBNull.Value
                        ? "غير مسمى"
                        : row["ClassName"].ToString();

                    int count = row["StudentCount"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(row["StudentCount"]);

                    TableLayoutPanel chartRow = CreateChartRow(
                        className,
                        count,
                        maxCount,
                        rows.Width);

                    rows.Controls.Add(chartRow);
                }

                panelChartBody.Controls.Add(rows);
            }
            finally
            {
                panelChartBody.ResumeLayout();
            }
        }

        private static Label CreateChartMessage(string message)
        {
            return new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                RightToLeft = RightToLeft.Yes,
                ForeColor = UIHelper.MutedTextColor,
                Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize, FontStyle.Bold)
            };
        }

        private static TableLayoutPanel CreateChartRow(
            string className,
            int count,
            long maxCount,
            int availableWidth)
        {
            TableLayoutPanel row = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 1,
                Width = Math.Max(240, availableWidth),
                Height = 42,
                Margin = new Padding(0, 0, 0, 6),
                Padding = new Padding(0),
                RightToLeft = RightToLeft.Yes
            };

            row.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 29F));

            row.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 56F));

            row.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 15F));

            Label classLabel = new Label
            {
                Text = className,
                Dock = DockStyle.Fill,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleRight,
                RightToLeft = RightToLeft.Yes,
                ForeColor = UIHelper.TextSecondaryColor,
                Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize, FontStyle.Bold),
                Padding = new Padding(0, 0, 8, 0)
            };

            Label countLabel = new Label
            {
                Text = count.ToString("N0"),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                RightToLeft = RightToLeft.No,
                ForeColor = UIHelper.TextPrimaryColor,
                Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize, FontStyle.Bold)
            };

            Panel track = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 9, 8, 9),
                BackColor = UIHelper.SurfaceSecondaryColor
            };

            Panel bar = new Panel
            {
                Dock = DockStyle.Right,
                BackColor = UIHelper.InfoColor,
                Width = Math.Max(
                    4,
                    (int)Math.Round(
                        (double)count /
                        Math.Max(1, maxCount) *
                        Math.Max(8, (availableWidth * 0.56) - 16)))
            };

            track.Controls.Add(bar);
            row.Controls.Add(classLabel, 0, 0);
            row.Controls.Add(track, 1, 0);
            row.Controls.Add(countLabel, 2, 0);

            return row;
        }

        private async Task LoadAlertsAsync()
        {
            lblPendingFees.Text = "بنود الرسوم غير المسددة: جاري التحميل...";
            lblTodayAbsence.Text = "غياب اليوم: جاري التحميل...";
            lblOperationalStatus.Text = "حالة التشغيل: جاري التحميل...";

            Task<int> pendingFeesTask = RunSafeQueryAsync(
                () => dashboardService.GetPendingFeesCount(),
                "تحميل عدد بنود الرسوم المتبقية للوحة التحكم",
                0);

            Task<int> absenceTask = RunSafeQueryAsync(
                () => dashboardService.GetTodayAbsenceCount(),
                "تحميل غياب اليوم للوحة التحكم",
                0);

            Task<DataTable> operationalTask = RunSafeQueryAsync(
                () => dashboardService.GetOperationalStatus(),
                "تحميل حالة التشغيل للوحة التحكم",
                null);

            await Task.WhenAll(
                pendingFeesTask,
                absenceTask,
                operationalTask);

            lblPendingFees.Text =
                "بنود الرسوم غير المسددة: " + pendingFeesTask.Result.ToString("N0");

            lblTodayAbsence.Text =
                "غياب اليوم: " + absenceTask.Result.ToString("N0") + " طالب";

            RenderOperationalStatus(operationalTask.Result);
        }

        private void RenderOperationalStatus(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
            {
                lblOperationalStatus.Text =
                    "حالة التشغيل:" + Environment.NewLine +
                    "لا توجد بيانات تشغيلية متاحة حاليًا.";

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
                        .ToString("yyyy/MM/dd HH:mm");

            string closingYear =
                row["LastClosingYear"] == DBNull.Value
                    ? "غير متوفر"
                    : row["LastClosingYear"].ToString();

            string closingStatus =
                row["LastClosingStatus"] == DBNull.Value
                    ? "غير متوفر"
                    : row["LastClosingStatus"].ToString();

            lblOperationalStatus.Text =
                "حالة التشغيل:" + Environment.NewLine +
                "آخر نسخة احتياطية: " +
                (string.IsNullOrWhiteSpace(backupFile)
                    ? "غير متوفرة"
                    : backupDate) +
                Environment.NewLine +
                "آخر إغلاق سنوي: " +
                closingYear +
                " / " +
                closingStatus;
        }

        private static async Task<T> RunSafeQueryAsync<T>(
            Func<T> query,
            string context,
            T fallback)
        {
            try
            {
                return await Task.Run(query);
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(context, ex);
                return fallback;
            }
        }
    }
}
