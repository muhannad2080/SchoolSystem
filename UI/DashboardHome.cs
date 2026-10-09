using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Security;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    /// <summary>
    /// لوحة التحكم الرئيسية.
    /// هذه الواجهة تعرض محتوى لوحة التحكم فقط؛
    /// رأس النظام واسم المستخدم والتاريخ والوقت مسؤول عنها MainForm.
    /// جميع المؤشرات تُحمّل تلقائيًا من قاعدة البيانات عند فتح الواجهة،
    /// ثم تُحدّث دوريًا دون زر يدوي.
    /// </summary>
    public partial class DashboardHome : UserControl
    {
        private readonly DashboardService dashboardService =
            new DashboardService();

        private DataTable studentsPerClassTable;
        private Timer dashboardAutoRefreshTimer;
        private FlowLayoutPanel quickActionsPanel;
        private bool isDashboardLoading;

        public DashboardHome()
        {
            InitializeComponent();

            RightToLeft = RightToLeft.Yes;
            BackColor = UIHelper.BackgroundColor;

            UIHelper.ApplyKryptonTheme();
            UIHelper.ApplyResponsiveLayout(this);
            UIHelper.ApplyFieldValidation(this);

            ConfigureDashboardVisuals();
            ConfigureArabicDashboardLayout();
            ConfigureQuickActions();
            ConfigureDashboardSurface();

            panelCards.Resize += (sender, e) => ResizeDashboardCards();
            panelChartBody.Resize += (sender, e) =>
            {
                if (studentsPerClassTable != null)
                    RenderChart();
            };

            Load += DashboardHome_Load;
            Disposed += DashboardHome_Disposed;
            DataRefreshNotifier.DataChanged += DashboardHome_DataChanged;

            ResizeDashboardCards();
        }

        /// <summary>
        /// توحيد النصوص واتجاه العرض داخل لوحة التحكم.
        /// </summary>
        private void ConfigureArabicDashboardLayout()
        {
            // اسم المدرسة يأتي من الإعدادات، لكن تعذّر قراءة الإعدادات لا يجب أن يمنع فتح لوحة التحكم.
            SchoolSettings branding = null;
            try
            {
                branding = SchoolSettingsService.Current();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("قراءة هوية المدرسة في لوحة التحكم", ex);
            }

            string schoolName =
                branding != null && !string.IsNullOrWhiteSpace(branding.SchoolNameAr)
                    ? branding.SchoolNameAr.Trim()
                    : "المدرسة";

            string organization =
                branding != null && !string.IsNullOrWhiteSpace(branding.OrganizationNameAr)
                    ? branding.OrganizationNameAr.Trim()
                    : string.Empty;

            lblDashboardTitle.AutoEllipsis = true;
            lblDashboardSubtitle.AutoEllipsis = true;
            lblDashboardTitle.Text = "لوحة تحكم " + schoolName;

            lblDashboardSubtitle.Text =
                string.IsNullOrWhiteSpace(organization)
                    ? "مركز الإدارة لمتابعة الطلاب والهيئة التعليمية والرسوم والحالة التشغيلية"
                    : organization + "  •  مركز الإدارة لمتابعة الطلاب والهيئة التعليمية والرسوم والحالة التشغيلية";
            lblActiveAcademicYear.Text =
                "العام الدراسي: جاري التحميل...";
            lblLastUpdated.Text =
                "آخر تحديث تلقائي: جاري التحميل...";

            lblStudentsTitle.Text = "إجمالي الطلاب";
            lblTeachersTitle.Text = "المعلمون";
            lblSubjectsTitle.Text = "المواد الدراسية";
            lblClassesTitle.Text = "الفصول الدراسية";
            lblFeesTitle.Text = "إجمالي الرسوم المتبقية";

            lblAlertsTitle.Text = "المتابعة والتنبيهات";
            lblChartTitle.Text = "توزيع الطلاب حسب الفصل";

            Control[] arabicControls =
            {
                panelSummary,
                tableLayoutSummary,
                tableLayoutMain,
                panelCards,
                tableLayoutLower,
                panelAlerts,
                panelChart,
                panelChartBody
            };

            foreach (Control control in arabicControls)
            {
                if (control != null)
                    control.RightToLeft = RightToLeft.Yes;
            }

            Label[] arabicLabels =
            {
                lblDashboardTitle,
                lblDashboardSubtitle,
                lblActiveAcademicYear,
                lblLastUpdated,
                lblStudentsTitle,
                lblTeachersTitle,
                lblSubjectsTitle,
                lblClassesTitle,
                lblFeesTitle,
                lblAlertsTitle,
                lblPendingFees,
                lblTodayAbsence,
                lblOperationalStatus,
                lblChartTitle
            };

            foreach (Label label in arabicLabels)
            {
                if (label == null)
                    continue;

                label.RightToLeft = RightToLeft.Yes;
                label.TextAlign = ContentAlignment.MiddleRight;
            }

            Label[] numericLabels =
            {
                lblStudentsValue,
                lblTeachersValue,
                lblSubjectsValue,
                lblClassesValue,
                lblFeesValue
            };

            foreach (Label label in numericLabels)
            {
                if (label == null)
                    continue;

                label.RightToLeft = RightToLeft.No;
                label.TextAlign = ContentAlignment.MiddleCenter;
            }

            lblDashboardTitle.TextAlign =
                ContentAlignment.MiddleCenter;

            lblDashboardSubtitle.TextAlign =
                ContentAlignment.MiddleRight;

            lblActiveAcademicYear.TextAlign =
                ContentAlignment.MiddleLeft;

            lblLastUpdated.TextAlign =
                ContentAlignment.MiddleLeft;

            lblOperationalStatus.AutoEllipsis = false;
            panelChartBody.BackColor = UIHelper.SurfaceColor;
            panelChartBody.AutoScroll = true;
        }

        private void ConfigureQuickActions()
        {
            if (quickActionsPanel != null && !quickActionsPanel.IsDisposed)
                return;

            panelSummary.Height = 154;
            panelSummary.Padding = new Padding(18, 10, 18, 6);

            quickActionsPanel = new FlowLayoutPanel
            {
                Name = "quickActionsPanel",
                Dock = DockStyle.Bottom,
                Height = 46,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.RightToLeft,
                RightToLeft = RightToLeft.Yes,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 0),
                Margin = new Padding(0)
            };

            Label title = new Label
            {
                Name = "lblQuickActionsTitle",
                Text = "إجراءات سريعة",
                AutoSize = false,
                Width = 108,
                Height = 40,
                Margin = new Padding(0, 0, 8, 0),
                Padding = new Padding(0, 0, 8, 0),
                TextAlign = ContentAlignment.MiddleRight,
                RightToLeft = RightToLeft.Yes,
                ForeColor = UIHelper.TextPrimaryColor,
                Font = new Font(UIHelper.FontFamily, 9.5F, FontStyle.Bold)
            };
            quickActionsPanel.Controls.Add(title);

            AddQuickAction(
                "إضافة طالب",
                "Students",
                "Students.Add",
                PermissionKeys.StudentsManage,
                UIHelper.InfoColor,
                delegate
                {
                    MainForm.Instance?.LoadFormInPanel(
                        "Students",
                        "ليس لديك صلاحية عرض الطلاب.",
                        new StudentsForm(),
                        PermissionKeys.StudentsView,
                        PermissionKeys.StudentsManage);
                });

            AddQuickAction(
                "قبول وتسجيل",
                "Enrollment",
                "Enrollment.Add",
                PermissionKeys.EnrollmentManage,
                UIHelper.SuccessColor,
                delegate
                {
                    MainForm.Instance?.LoadFormInPanel(
                        "Enrollment",
                        "ليس لديك صلاحية عرض القبول والتسجيل.",
                        new EnrollmentForm(),
                        "Enrollment.View",
                        PermissionKeys.EnrollmentManage);
                });

            AddQuickAction(
                "الرسوم",
                "Fees",
                "Fees.View",
                PermissionKeys.FeesManage,
                UIHelper.WarningColor,
                delegate
                {
                    MainForm.Instance?.LoadUserControl(
                        "Fees",
                        "ليس لديك صلاحية عرض الرسوم.",
                        new FeesForm(),
                        "Fees.View",
                        PermissionKeys.FeesManage);
                });

            AddQuickAction(
                "خطط الرسوم",
                "FeePlans",
                "FeePlans.View",
                "FeePlans.Manage",
                UIHelper.AccentColor,
                delegate
                {
                    MainForm.Instance?.LoadUserControl(
                        "FeePlans",
                        "ليس لديك صلاحية عرض خطط الرسوم.",
                        new FeePlansForm(),
                        "FeePlans.View",
                        "FeePlans.Manage");
                });

            AddQuickAction(
                "سند قبض",
                "Vouchers",
                "Vouchers.Add",
                PermissionKeys.VouchersManage,
                UIHelper.DangerColor,
                delegate
                {
                    MainForm.Instance?.LoadUserControl(
                        "Vouchers",
                        "ليس لديك صلاحية عرض السندات.",
                        new VouchersForm(),
                        "Vouchers.View",
                        PermissionKeys.VouchersManage);
                });

            AddQuickAction(
                "التقارير",
                "Reports",
                PermissionKeys.ReportsView,
                string.Empty,
                UIHelper.SecondaryColor,
                delegate
                {
                    MainForm.Instance?.LoadUserControl(
                        "Reports",
                        "ليس لديك صلاحية عرض التقارير.",
                        new ReportCenterForm(),
                        PermissionKeys.ReportsView);
                });

            Button refresh = CreateQuickActionButton(
                "تحديث الآن",
                UIHelper.NeutralColor,
                116);
            refresh.Click += async delegate
            {
                await ReloadDashboardAsync();
            };
            quickActionsPanel.Controls.Add(refresh);

            panelSummary.Controls.Add(quickActionsPanel);
            quickActionsPanel.BringToFront();
        }

        private void AddQuickAction(
            string text,
            string module,
            string permission,
            string fallbackPermission,
            Color accent,
            Action action)
        {
            bool allowed =
                CurrentUser.IsAdmin() ||
                CurrentUser.HasPermission(permission) ||
                (!string.IsNullOrWhiteSpace(fallbackPermission) &&
                 CurrentUser.HasPermission(fallbackPermission));

            if (!allowed)
                return;

            Button button = CreateQuickActionButton(
                text,
                accent,
                text.Length >= 10 ? 122 : 106);

            button.AccessibleName = text;
            button.AccessibleDescription = "إجراء سريع لوحدة " + module;
            button.Click += delegate
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("فتح " + text, ex);
                }
            };

            quickActionsPanel.Controls.Add(button);
        }

        private static Button CreateQuickActionButton(
            string text,
            Color accent,
            int width)
        {
            Button button = new Button
            {
                Text = text,
                Width = width,
                Height = 40,
                Margin = new Padding(4, 0, 4, 0),
                Padding = new Padding(10, 0, 10, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = accent,
                Font = new Font(UIHelper.FontFamily, 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = true,
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 250, 252);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(241, 245, 249);

            return button;
        }

        private void ConfigureDashboardSurface()
        {
            Panel[] panels = { panelSummary, panelAlerts, panelChart };
            foreach (Panel panel in panels)
            {
                if (panel == null)
                    continue;
                panel.BorderStyle = BorderStyle.None;
                panel.Paint += DashboardPanel_Paint;
            }

            Panel[] cards =
            {
                cardStudents,
                cardTeachers,
                cardSubjects,
                cardClasses,
                cardFees
            };

            foreach (Panel card in cards)
            {
                if (card == null)
                    continue;
                card.BorderStyle = BorderStyle.None;
                card.Paint += DashboardCard_Paint;
            }

            lblOperationalStatus.Padding = new Padding(12, 8, 12, 8);
            lblOperationalStatus.Font =
                new Font(UIHelper.FontFamily, 9F, FontStyle.Bold);

            lblPendingFees.Padding = new Padding(12, 8, 12, 8);
            lblTodayAbsence.Padding = new Padding(12, 8, 12, 8);
            lblAlertsTitle.Height = 42;
            lblChartTitle.Height = 42;
            panelChartBody.Padding = new Padding(10, 8, 10, 8);
        }

        private static void DashboardPanel_Paint(
            object sender,
            PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds =
                new Rectangle(0, 0,
                    Math.Max(0, panel.Width - 1),
                    Math.Max(0, panel.Height - 1));

            using (GraphicsPath path = CreateRoundedPath(bounds, 12))
            using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1F))
                e.Graphics.DrawPath(pen, path);
        }

        private static void DashboardCard_Paint(
            object sender,
            PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds =
                new Rectangle(0, 0,
                    Math.Max(0, card.Width - 1),
                    Math.Max(0, card.Height - 1));

            using (GraphicsPath path = CreateRoundedPath(bounds, 14))
            using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1F))
                e.Graphics.DrawPath(pen, path);
        }

        private static GraphicsPath CreateRoundedPath(
            Rectangle bounds,
            int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            if (bounds.Width < diameter || bounds.Height < diameter)
            {
                path.AddRectangle(bounds);
                return path;
            }

            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ConfigureDashboardVisuals()
        {
            BackColor = UIHelper.BackgroundColor;

            panelSummary.BackColor = UIHelper.SurfaceColor;

            lblDashboardTitle.ForeColor =
                UIHelper.TextPrimaryColor;

            lblDashboardSubtitle.ForeColor =
                UIHelper.TextSecondaryColor;

            lblActiveAcademicYear.ForeColor =
                UIHelper.PrimaryColor;

            lblLastUpdated.ForeColor =
                UIHelper.MutedTextColor;

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

            panelChart.BackColor =
                UIHelper.SurfaceColor;

            panelAlerts.BackColor =
                UIHelper.SurfaceColor;

            lblChartTitle.ForeColor =
                UIHelper.TextPrimaryColor;

            lblAlertsTitle.ForeColor =
                UIHelper.TextPrimaryColor;

            lblAlertsTitle.BackColor =
                UIHelper.SurfaceSecondaryColor;

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
            card.BackColor =
                UIHelper.SurfaceColor;

            card.BorderStyle =
                BorderStyle.FixedSingle;

            accentBar.BackColor =
                accent;

            title.ForeColor =
                UIHelper.TextSecondaryColor;

            value.ForeColor =
                accent;
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
            if (panelCards == null ||
                panelCards.IsDisposed)
            {
                return;
            }

            int width =
                Math.Max(320, panelCards.ClientSize.Width - 4);

            int columns =
                width >= 1050
                    ? 5
                    : width >= 780
                        ? 3
                        : 2;

            int horizontalMargins =
                columns * 12;

            int cardWidth =
                Math.Max(
                    150,
                    (width - horizontalMargins) /
                    columns);

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

        private void DashboardHome_Load(
            object sender,
            EventArgs e)
        {
            _ = ReloadDashboardAsync();

            // قد يعاد تحميل UserControl أكثر من مرة أثناء التنقل؛
            // لا ننشئ مؤقتًا جديدًا في كل مرة حتى لا تتكرر الاستعلامات الدورية.
            if (dashboardAutoRefreshTimer != null)
                return;

            dashboardAutoRefreshTimer =
                new Timer
                {
                    Interval = 60000
                };

            dashboardAutoRefreshTimer.Tick +=
                async (timerSender, timerEventArgs) =>
                {
                    await ReloadDashboardAsync();
                };

            dashboardAutoRefreshTimer.Start();
        }

        private void DashboardHome_Disposed(
            object sender,
            EventArgs e)
        {
            DataRefreshNotifier.DataChanged -= DashboardHome_DataChanged;

            if (dashboardAutoRefreshTimer == null)
                return;

            dashboardAutoRefreshTimer.Stop();
            dashboardAutoRefreshTimer.Dispose();
            dashboardAutoRefreshTimer = null;
        }

        private async void DashboardHome_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            // إشعارات التغيير قد تصل من شاشة أو خدمة تعمل على خيط آخر.
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(
                        () => DashboardHome_DataChanged(sender, e)));
                }
                catch (InvalidOperationException)
                {
                    // أُغلقت لوحة التحكم قبل تنفيذ التحديث.
                }

                return;
            }

            if (ReferenceEquals(sender, this) || isDashboardLoading)
                return;

            // لوحة التحكم لا تعرض بيانات محلية؛ كل مؤشر يعاد قراءته من قاعدة البيانات
            // بعد أي تغيير من أي وحدة حتى تبقى المؤشرات والمخطط حيّة.
            await ReloadDashboardAsync();
        }

        private async Task ReloadDashboardAsync()
        {
            if (isDashboardLoading ||
                IsDisposed ||
                Disposing)
            {
                return;
            }

            isDashboardLoading = true;

            try
            {
                await Task.WhenAll(
                    LoadActiveYearAsync(),
                    LoadStatisticsAsync(),
                    LoadChartAsync(),
                    LoadAlertsAsync());

                lblLastUpdated.Text =
                    "آخر تحديث تلقائي: " +
                    DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "تحديث لوحة التحكم",
                    ex);

                lblLastUpdated.Text =
                    "آخر تحديث تلقائي: تعذر اكتمال التحميل";
            }
            finally
            {
                isDashboardLoading = false;
            }
        }

        private async Task LoadActiveYearAsync()
        {
            string year =
                await RunSafeQueryAsync(
                    () => new AnnualClosingService()
                        .GetActiveAcademicYear(),
                    "تحميل العام الدراسي للوحة التحكم",
                    string.Empty);

            lblActiveAcademicYear.Text =
                string.IsNullOrWhiteSpace(year)
                    ? "العام الدراسي: غير محدد"
                    : "العام الدراسي: " +
                      year.Trim().Replace('-', '/');
        }

        private async Task LoadStatisticsAsync()
        {
            lblStudentsValue.Text = "--";
            lblTeachersValue.Text = "--";
            lblSubjectsValue.Text = "--";
            lblClassesValue.Text = "--";
            lblFeesValue.Text = "--";

            Task<int> studentsTask =
                RunSafeQueryAsync(
                    () => dashboardService.GetStudentCount(),
                    "تحميل عدد الطلاب للوحة التحكم",
                    0);

            Task<int> teachersTask =
                RunSafeQueryAsync(
                    () => dashboardService.GetTeacherCount(),
                    "تحميل عدد المعلمين للوحة التحكم",
                    0);

            Task<int> subjectsTask =
                RunSafeQueryAsync(
                    () => dashboardService.GetSubjectCount(),
                    "تحميل عدد المواد للوحة التحكم",
                    0);

            Task<int> classesTask =
                RunSafeQueryAsync(
                    () => dashboardService.GetClassCount(),
                    "تحميل عدد الفصول للوحة التحكم",
                    0);

            Task<decimal> feesTask =
                RunSafeQueryAsync(
                    () => dashboardService.GetPendingFeesTotal(),
                    "تحميل إجمالي الرسوم المتبقية للوحة التحكم",
                    0m);

            await Task.WhenAll(
                studentsTask,
                teachersTask,
                subjectsTask,
                classesTask,
                feesTask);

            lblStudentsValue.Text =
                studentsTask.Result.ToString("N0");

            lblTeachersValue.Text =
                teachersTask.Result.ToString("N0");

            lblSubjectsValue.Text =
                subjectsTask.Result.ToString("N0");

            lblClassesValue.Text =
                classesTask.Result.ToString("N0");

            lblFeesValue.Text =
                feesTask.Result.ToString("N2");
        }

        private async Task LoadChartAsync()
        {
            DataTable table =
                await RunSafeQueryAsync(
                    () => dashboardService.GetStudentsPerClass(),
                    "تحميل توزيع الطلاب حسب الفصل للوحة التحكم",
                    null);

            studentsPerClassTable = table;
            RenderChart();
        }

        private void RenderChart()
        {
            if (panelChartBody == null ||
                panelChartBody.IsDisposed)
            {
                return;
            }

            panelChartBody.SuspendLayout();

            try
            {
                for (int i = panelChartBody.Controls.Count - 1;
                     i >= 0;
                     i--)
                {
                    Control existing =
                        panelChartBody.Controls[i];

                    panelChartBody.Controls.RemoveAt(i);
                    existing.Dispose();
                }

                if (studentsPerClassTable == null ||
                    studentsPerClassTable.Rows.Count == 0)
                {
                    panelChartBody.Controls.Add(
                        CreateChartMessage(
                            "لا توجد بيانات فعلية للعرض في المخطط."));

                    return;
                }

                long maxCount = 1;

                foreach (DataRow row
                         in studentsPerClassTable.Rows)
                {
                    long count =
                        row["StudentCount"] ==
                        DBNull.Value
                            ? 0
                            : Convert.ToInt64(
                                row["StudentCount"]);

                    if (count > maxCount)
                        maxCount = count;
                }

                FlowLayoutPanel rows =
                    new FlowLayoutPanel
                    {
                        Dock = DockStyle.Top,
                        AutoSize = true,
                        WrapContents = false,
                        FlowDirection =
                            FlowDirection.TopDown,
                        RightToLeft = RightToLeft.Yes,
                        Padding = new Padding(0),
                        Margin = new Padding(0),
                        Width =
                            Math.Max(
                                240,
                                panelChartBody.ClientSize.Width - 28)
                    };

                foreach (DataRow row
                         in studentsPerClassTable.Rows)
                {
                    string className =
                        row["ClassName"] ==
                        DBNull.Value
                            ? "غير مسمى"
                            : row["ClassName"].ToString();

                    int count =
                        row["StudentCount"] ==
                        DBNull.Value
                            ? 0
                            : Convert.ToInt32(
                                row["StudentCount"]);

                    rows.Controls.Add(
                        CreateChartRow(
                            className,
                            count,
                            maxCount,
                            rows.Width));
                }

                panelChartBody.Controls.Add(rows);
            }
            finally
            {
                panelChartBody.ResumeLayout();
            }
        }

        private static Label CreateChartMessage(
            string message)
        {
            return new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                RightToLeft = RightToLeft.Yes,
                ForeColor = UIHelper.MutedTextColor,
                Font =
                    new Font(
                        UIHelper.FontFamily,
                        UIHelper.BodyFontSize,
                        FontStyle.Bold)
            };
        }

        private static TableLayoutPanel CreateChartRow(
            string className,
            int count,
            long maxCount,
            int availableWidth)
        {
            TableLayoutPanel row =
                new TableLayoutPanel
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
                new ColumnStyle(
                    SizeType.Percent,
                    29F));

            row.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    56F));

            row.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    15F));

            Label classLabel =
                new Label
                {
                    Text = className,
                    Dock = DockStyle.Fill,
                    AutoEllipsis = true,
                    TextAlign =
                        ContentAlignment.MiddleRight,
                    RightToLeft =
                        RightToLeft.Yes,
                    ForeColor =
                        UIHelper.TextSecondaryColor,
                    Font =
                        new Font(
                            UIHelper.FontFamily,
                            UIHelper.BodyFontSize,
                            FontStyle.Bold),
                    Padding =
                        new Padding(0, 0, 8, 0)
                };

            Label countLabel =
                new Label
                {
                    Text =
                        count.ToString("N0"),
                    Dock = DockStyle.Fill,
                    TextAlign =
                        ContentAlignment.MiddleCenter,
                    RightToLeft =
                        RightToLeft.No,
                    ForeColor =
                        UIHelper.TextPrimaryColor,
                    Font =
                        new Font(
                            UIHelper.FontFamily,
                            UIHelper.BodyFontSize,
                            FontStyle.Bold)
                };

            Panel track =
                new Panel
                {
                    Dock = DockStyle.Fill,
                    Margin =
                        new Padding(8, 9, 8, 9),
                    BackColor =
                        UIHelper.SurfaceSecondaryColor
                };

            Panel bar =
                new Panel
                {
                    Dock = DockStyle.Right,
                    BackColor =
                        UIHelper.InfoColor,
                    Width =
                        Math.Max(
                            4,
                            (int)Math.Round(
                                (double)count /
                                Math.Max(
                                    1,
                                    maxCount) *
                                Math.Max(
                                    8,
                                    (availableWidth *
                                     0.56) - 16)))
                };

            track.Controls.Add(bar);
            row.Controls.Add(classLabel, 0, 0);
            row.Controls.Add(track, 1, 0);
            row.Controls.Add(countLabel, 2, 0);

            return row;
        }

        private async Task LoadAlertsAsync()
        {
            lblPendingFees.Text =
                "بنود الرسوم غير المسددة: جاري التحميل...";

            lblTodayAbsence.Text =
                "غياب اليوم: جاري التحميل...";

            lblOperationalStatus.Text =
                "حالة التشغيل: جاري التحميل...";

            Task<int> pendingFeesTask =
                RunSafeQueryAsync(
                    () => dashboardService.GetPendingFeesCount(),
                    "تحميل عدد بنود الرسوم المتبقية للوحة التحكم",
                    0);

            Task<int> absenceTask =
                RunSafeQueryAsync(
                    () => dashboardService.GetTodayAbsenceCount(),
                    "تحميل غياب اليوم للوحة التحكم",
                    0);

            Task<DataTable> operationalTask =
                RunSafeQueryAsync(
                    () => dashboardService.GetOperationalStatus(),
                    "تحميل حالة التشغيل للوحة التحكم",
                    null);

            await Task.WhenAll(
                pendingFeesTask,
                absenceTask,
                operationalTask);

            lblPendingFees.Text =
                "بنود الرسوم غير المسددة: " +
                pendingFeesTask.Result.ToString("N0");

            lblTodayAbsence.Text =
                "غياب اليوم: " +
                absenceTask.Result.ToString("N0") +
                " طالب";

            RenderOperationalStatus(
                operationalTask.Result);
        }

        private void RenderOperationalStatus(
            DataTable table)
        {
            if (table == null ||
                table.Rows.Count == 0)
            {
                lblOperationalStatus.Text =
                    "حالة التشغيل:" +
                    Environment.NewLine +
                    "لا توجد بيانات تشغيلية متاحة حاليًا.";

                return;
            }

            DataRow row =
                table.Rows[0];

            string backupFile =
                row["LastBackupFile"] ==
                DBNull.Value
                    ? string.Empty
                    : row["LastBackupFile"].ToString();

            string backupDate =
                row["LastBackupDate"] ==
                DBNull.Value
                    ? "غير متوفر"
                    : Convert
                        .ToDateTime(
                            row["LastBackupDate"])
                        .ToString(
                            "yyyy/MM/dd HH:mm");

            string closingYear =
                row["LastClosingYear"] ==
                DBNull.Value
                    ? "غير متوفر"
                    : row["LastClosingYear"].ToString();

            string closingStatus =
                row["LastClosingStatus"] ==
                DBNull.Value
                    ? "غير متوفر"
                    : row["LastClosingStatus"].ToString();

            lblOperationalStatus.Text =
                "حالة التشغيل:" +
                Environment.NewLine +
                "آخر نسخة احتياطية: " +
                (string.IsNullOrWhiteSpace(
                    backupFile)
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
                ApplicationLogger.LogException(
                    context,
                    ex);

                return fallback;
            }
        }
    }
}