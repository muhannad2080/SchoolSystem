namespace SchoolSystem.UI
{
    partial class DashboardHome
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutSummary;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.Label lblDashboardSubtitle;
        private System.Windows.Forms.Label lblActiveAcademicYear;
        private System.Windows.Forms.Label lblLastUpdated;

        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.FlowLayoutPanel panelCards;

        private System.Windows.Forms.Panel cardStudents;
        private System.Windows.Forms.Panel cardStudentsBar;
        private System.Windows.Forms.Label lblStudentsTitle;
        private System.Windows.Forms.Label lblStudentsValue;

        private System.Windows.Forms.Panel cardTeachers;
        private System.Windows.Forms.Panel cardTeachersBar;
        private System.Windows.Forms.Label lblTeachersTitle;
        private System.Windows.Forms.Label lblTeachersValue;

        private System.Windows.Forms.Panel cardSubjects;
        private System.Windows.Forms.Panel cardSubjectsBar;
        private System.Windows.Forms.Label lblSubjectsTitle;
        private System.Windows.Forms.Label lblSubjectsValue;

        private System.Windows.Forms.Panel cardClasses;
        private System.Windows.Forms.Panel cardClassesBar;
        private System.Windows.Forms.Label lblClassesTitle;
        private System.Windows.Forms.Label lblClassesValue;

        private System.Windows.Forms.Panel cardFees;
        private System.Windows.Forms.Panel cardFeesBar;
        private System.Windows.Forms.Label lblFeesTitle;
        private System.Windows.Forms.Label lblFeesValue;

        private System.Windows.Forms.TableLayoutPanel tableLayoutLower;
        private System.Windows.Forms.Panel panelAlerts;
        private System.Windows.Forms.Label lblAlertsTitle;
        private System.Windows.Forms.Label lblPendingFees;
        private System.Windows.Forms.Label lblTodayAbsence;
        private System.Windows.Forms.Label lblOperationalStatus;

        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.Label lblChartTitle;
        private System.Windows.Forms.Panel panelChartBody;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region كود المصمم

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.panelSummary =
                new System.Windows.Forms.Panel();

            this.tableLayoutSummary =
                new System.Windows.Forms.TableLayoutPanel();

            this.lblDashboardTitle =
                new System.Windows.Forms.Label();

            this.lblDashboardSubtitle =
                new System.Windows.Forms.Label();

            this.lblActiveAcademicYear =
                new System.Windows.Forms.Label();

            this.lblLastUpdated =
                new System.Windows.Forms.Label();

            this.tableLayoutMain =
                new System.Windows.Forms.TableLayoutPanel();

            this.panelCards =
                new System.Windows.Forms.FlowLayoutPanel();

            this.cardStudents =
                new System.Windows.Forms.Panel();

            this.cardStudentsBar =
                new System.Windows.Forms.Panel();

            this.lblStudentsTitle =
                new System.Windows.Forms.Label();

            this.lblStudentsValue =
                new System.Windows.Forms.Label();

            this.cardTeachers =
                new System.Windows.Forms.Panel();

            this.cardTeachersBar =
                new System.Windows.Forms.Panel();

            this.lblTeachersTitle =
                new System.Windows.Forms.Label();

            this.lblTeachersValue =
                new System.Windows.Forms.Label();

            this.cardSubjects =
                new System.Windows.Forms.Panel();

            this.cardSubjectsBar =
                new System.Windows.Forms.Panel();

            this.lblSubjectsTitle =
                new System.Windows.Forms.Label();

            this.lblSubjectsValue =
                new System.Windows.Forms.Label();

            this.cardClasses =
                new System.Windows.Forms.Panel();

            this.cardClassesBar =
                new System.Windows.Forms.Panel();

            this.lblClassesTitle =
                new System.Windows.Forms.Label();

            this.lblClassesValue =
                new System.Windows.Forms.Label();

            this.cardFees =
                new System.Windows.Forms.Panel();

            this.cardFeesBar =
                new System.Windows.Forms.Panel();

            this.lblFeesTitle =
                new System.Windows.Forms.Label();

            this.lblFeesValue =
                new System.Windows.Forms.Label();

            this.tableLayoutLower =
                new System.Windows.Forms.TableLayoutPanel();

            this.panelAlerts =
                new System.Windows.Forms.Panel();

            this.lblAlertsTitle =
                new System.Windows.Forms.Label();

            this.lblPendingFees =
                new System.Windows.Forms.Label();

            this.lblTodayAbsence =
                new System.Windows.Forms.Label();

            this.lblOperationalStatus =
                new System.Windows.Forms.Label();

            this.panelChart =
                new System.Windows.Forms.Panel();

            this.lblChartTitle =
                new System.Windows.Forms.Label();

            this.panelChartBody =
                new System.Windows.Forms.Panel();

            this.panelSummary.SuspendLayout();
            this.tableLayoutSummary.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.panelCards.SuspendLayout();
            this.cardStudents.SuspendLayout();
            this.cardTeachers.SuspendLayout();
            this.cardSubjects.SuspendLayout();
            this.cardClasses.SuspendLayout();
            this.cardFees.SuspendLayout();
            this.tableLayoutLower.SuspendLayout();
            this.panelAlerts.SuspendLayout();
            this.panelChart.SuspendLayout();
            this.SuspendLayout();

            // panelSummary
            this.panelSummary.BackColor =
                System.Drawing.Color.White;

            this.panelSummary.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.panelSummary.Location =
                new System.Drawing.Point(0, 0);

            this.panelSummary.Name =
                "panelSummary";

            this.panelSummary.Padding =
                new System.Windows.Forms.Padding(
                    20, 14, 20, 10);

            this.panelSummary.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.panelSummary.Size =
                new System.Drawing.Size(1100, 104);

            this.panelSummary.TabIndex = 0;

            this.panelSummary.Controls.Add(
                this.tableLayoutSummary);

            // tableLayoutSummary
            this.tableLayoutSummary.ColumnCount = 2;

            this.tableLayoutSummary.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(
                    System.Windows.Forms.SizeType.Percent,
                    68F));

            this.tableLayoutSummary.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(
                    System.Windows.Forms.SizeType.Percent,
                    32F));

            this.tableLayoutSummary.Controls.Add(
                this.lblDashboardTitle,
                0,
                0);

            this.tableLayoutSummary.Controls.Add(
                this.lblDashboardSubtitle,
                0,
                1);

            this.tableLayoutSummary.Controls.Add(
                this.lblActiveAcademicYear,
                1,
                0);

            this.tableLayoutSummary.Controls.Add(
                this.lblLastUpdated,
                1,
                1);

            this.tableLayoutSummary.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.tableLayoutSummary.Location =
                new System.Drawing.Point(20, 14);

            this.tableLayoutSummary.Margin =
                new System.Windows.Forms.Padding(0);

            this.tableLayoutSummary.Name =
                "tableLayoutSummary";

            this.tableLayoutSummary.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.tableLayoutSummary.RowCount = 2;

            this.tableLayoutSummary.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent,
                    55F));

            this.tableLayoutSummary.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent,
                    45F));

            this.tableLayoutSummary.Size =
                new System.Drawing.Size(1060, 80);

            this.tableLayoutSummary.TabIndex = 0;

            // lblDashboardTitle
            this.lblDashboardTitle.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.lblDashboardTitle.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    18F,
                    System.Drawing.FontStyle.Bold);

            this.lblDashboardTitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    30, 41, 59);

            this.lblDashboardTitle.Location =
                new System.Drawing.Point(3, 0);

            this.lblDashboardTitle.Name =
                "lblDashboardTitle";

            this.lblDashboardTitle.Padding =
                new System.Windows.Forms.Padding(
                    4, 0, 10, 0);

            this.lblDashboardTitle.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.lblDashboardTitle.Size =
                new System.Drawing.Size(717, 44);

            this.lblDashboardTitle.TabIndex = 0;

            this.lblDashboardTitle.Text =
                "لوحة التحكم";

            this.lblDashboardTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleRight;

            // lblDashboardSubtitle
            this.lblDashboardSubtitle.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.lblDashboardSubtitle.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    9.5F,
                    System.Drawing.FontStyle.Regular);

            this.lblDashboardSubtitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    71, 85, 105);

            this.lblDashboardSubtitle.Location =
                new System.Drawing.Point(3, 44);

            this.lblDashboardSubtitle.Name =
                "lblDashboardSubtitle";

            this.lblDashboardSubtitle.Padding =
                new System.Windows.Forms.Padding(
                    4, 0, 10, 0);

            this.lblDashboardSubtitle.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.lblDashboardSubtitle.Size =
                new System.Drawing.Size(717, 36);

            this.lblDashboardSubtitle.TabIndex = 1;

            this.lblDashboardSubtitle.Text =
                "ملخص حي لمؤشرات المدرسة وحالتها التشغيلية";

            this.lblDashboardSubtitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleRight;

            // lblActiveAcademicYear
            this.lblActiveAcademicYear.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.lblActiveAcademicYear.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblActiveAcademicYear.ForeColor =
                System.Drawing.Color.FromArgb(
                    15, 118, 110);

            this.lblActiveAcademicYear.Location =
                new System.Drawing.Point(724, 0);

            this.lblActiveAcademicYear.Name =
                "lblActiveAcademicYear";

            this.lblActiveAcademicYear.Padding =
                new System.Windows.Forms.Padding(
                    6, 0, 6, 0);

            this.lblActiveAcademicYear.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.lblActiveAcademicYear.Size =
                new System.Drawing.Size(333, 44);

            this.lblActiveAcademicYear.TabIndex = 2;

            this.lblActiveAcademicYear.Text =
                "العام الدراسي: جاري التحميل...";

            this.lblActiveAcademicYear.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            // lblLastUpdated
            this.lblLastUpdated.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.lblLastUpdated.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    8.5F,
                    System.Drawing.FontStyle.Regular);

            this.lblLastUpdated.ForeColor =
                System.Drawing.Color.FromArgb(
                    100, 116, 139);

            this.lblLastUpdated.Location =
                new System.Drawing.Point(724, 44);

            this.lblLastUpdated.Name =
                "lblLastUpdated";

            this.lblLastUpdated.Padding =
                new System.Windows.Forms.Padding(
                    6, 0, 6, 0);

            this.lblLastUpdated.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.lblLastUpdated.Size =
                new System.Drawing.Size(333, 36);

            this.lblLastUpdated.TabIndex = 3;

            this.lblLastUpdated.Text =
                "آخر تحديث تلقائي: جاري التحميل...";

            this.lblLastUpdated.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            // tableLayoutMain
            this.tableLayoutMain.ColumnCount = 1;

            this.tableLayoutMain.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(
                    System.Windows.Forms.SizeType.Percent,
                    100F));

            this.tableLayoutMain.Controls.Add(
                this.panelCards,
                0,
                0);

            this.tableLayoutMain.Controls.Add(
                this.tableLayoutLower,
                0,
                1);

            this.tableLayoutMain.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.tableLayoutMain.Location =
                new System.Drawing.Point(0, 104);

            this.tableLayoutMain.Margin =
                new System.Windows.Forms.Padding(0);

            this.tableLayoutMain.Name =
                "tableLayoutMain";

            this.tableLayoutMain.Padding =
                new System.Windows.Forms.Padding(
                    14, 10, 14, 14);

            this.tableLayoutMain.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.tableLayoutMain.RowCount = 2;

            this.tableLayoutMain.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Absolute,
                    142F));

            this.tableLayoutMain.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent,
                    100F));

            this.tableLayoutMain.Size =
                new System.Drawing.Size(1100, 556);

            this.tableLayoutMain.TabIndex = 1;

            // panelCards
            this.panelCards.AutoScroll = true;

            this.panelCards.BackColor =
                System.Drawing.Color.Transparent;

            this.panelCards.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.panelCards.FlowDirection =
                System.Windows.Forms.FlowDirection.RightToLeft;

            this.panelCards.Location =
                new System.Drawing.Point(14, 114);

            this.panelCards.Margin =
                new System.Windows.Forms.Padding(0);

            this.panelCards.Name =
                "panelCards";

            this.panelCards.Padding =
                new System.Windows.Forms.Padding(2);

            this.panelCards.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.panelCards.Size =
                new System.Drawing.Size(1072, 142);

            this.panelCards.TabIndex = 0;

            this.panelCards.WrapContents = true;

            this.panelCards.Controls.Add(
                this.cardStudents);

            this.panelCards.Controls.Add(
                this.cardTeachers);

            this.panelCards.Controls.Add(
                this.cardSubjects);

            this.panelCards.Controls.Add(
                this.cardClasses);

            this.panelCards.Controls.Add(
                this.cardFees);

            // cardStudents
            ConfigureCardDesigner(
                this.cardStudents,
                "cardStudents",
                190,
                122);

            this.cardStudents.Controls.Add(
                this.lblStudentsValue);

            this.cardStudents.Controls.Add(
                this.lblStudentsTitle);

            this.cardStudents.Controls.Add(
                this.cardStudentsBar);

            ConfigureCardBarDesigner(
                this.cardStudentsBar,
                "cardStudentsBar");

            ConfigureCardTitleDesigner(
                this.lblStudentsTitle,
                "lblStudentsTitle",
                "إجمالي الطلاب");

            ConfigureCardValueDesigner(
                this.lblStudentsValue,
                "lblStudentsValue",
                "0");

            // cardTeachers
            ConfigureCardDesigner(
                this.cardTeachers,
                "cardTeachers",
                190,
                122);

            this.cardTeachers.Controls.Add(
                this.lblTeachersValue);

            this.cardTeachers.Controls.Add(
                this.lblTeachersTitle);

            this.cardTeachers.Controls.Add(
                this.cardTeachersBar);

            ConfigureCardBarDesigner(
                this.cardTeachersBar,
                "cardTeachersBar");

            ConfigureCardTitleDesigner(
                this.lblTeachersTitle,
                "lblTeachersTitle",
                "المعلمون");

            ConfigureCardValueDesigner(
                this.lblTeachersValue,
                "lblTeachersValue",
                "0");

            // cardSubjects
            ConfigureCardDesigner(
                this.cardSubjects,
                "cardSubjects",
                190,
                122);

            this.cardSubjects.Controls.Add(
                this.lblSubjectsValue);

            this.cardSubjects.Controls.Add(
                this.lblSubjectsTitle);

            this.cardSubjects.Controls.Add(
                this.cardSubjectsBar);

            ConfigureCardBarDesigner(
                this.cardSubjectsBar,
                "cardSubjectsBar");

            ConfigureCardTitleDesigner(
                this.lblSubjectsTitle,
                "lblSubjectsTitle",
                "المواد الدراسية");

            ConfigureCardValueDesigner(
                this.lblSubjectsValue,
                "lblSubjectsValue",
                "0");

            // cardClasses
            ConfigureCardDesigner(
                this.cardClasses,
                "cardClasses",
                190,
                122);

            this.cardClasses.Controls.Add(
                this.lblClassesValue);

            this.cardClasses.Controls.Add(
                this.lblClassesTitle);

            this.cardClasses.Controls.Add(
                this.cardClassesBar);

            ConfigureCardBarDesigner(
                this.cardClassesBar,
                "cardClassesBar");

            ConfigureCardTitleDesigner(
                this.lblClassesTitle,
                "lblClassesTitle",
                "الفصول الدراسية");

            ConfigureCardValueDesigner(
                this.lblClassesValue,
                "lblClassesValue",
                "0");

            // cardFees
            ConfigureCardDesigner(
                this.cardFees,
                "cardFees",
                190,
                122);

            this.cardFees.Controls.Add(
                this.lblFeesValue);

            this.cardFees.Controls.Add(
                this.lblFeesTitle);

            this.cardFees.Controls.Add(
                this.cardFeesBar);

            ConfigureCardBarDesigner(
                this.cardFeesBar,
                "cardFeesBar");

            ConfigureCardTitleDesigner(
                this.lblFeesTitle,
                "lblFeesTitle",
                "إجمالي الرسوم المتبقية");

            ConfigureCardValueDesigner(
                this.lblFeesValue,
                "lblFeesValue",
                "0");

            this.lblFeesValue.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    16F,
                    System.Drawing.FontStyle.Bold);

            // tableLayoutLower
            this.tableLayoutLower.ColumnCount = 2;

            this.tableLayoutLower.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(
                    System.Windows.Forms.SizeType.Percent,
                    34F));

            this.tableLayoutLower.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(
                    System.Windows.Forms.SizeType.Percent,
                    66F));

            this.tableLayoutLower.Controls.Add(
                this.panelAlerts,
                0,
                0);

            this.tableLayoutLower.Controls.Add(
                this.panelChart,
                1,
                0);

            this.tableLayoutLower.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.tableLayoutLower.Location =
                new System.Drawing.Point(14, 256);

            this.tableLayoutLower.Margin =
                new System.Windows.Forms.Padding(0);

            this.tableLayoutLower.Name =
                "tableLayoutLower";

            this.tableLayoutLower.Padding =
                new System.Windows.Forms.Padding(0);

            this.tableLayoutLower.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.tableLayoutLower.RowCount = 1;

            this.tableLayoutLower.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent,
                    100F));

            this.tableLayoutLower.Size =
                new System.Drawing.Size(1072, 390);

            this.tableLayoutLower.TabIndex = 1;

            // panelAlerts
            this.panelAlerts.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.panelAlerts.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.panelAlerts.Location =
                new System.Drawing.Point(0, 0);

            this.panelAlerts.Margin =
                new System.Windows.Forms.Padding(
                    0, 0, 8, 0);

            this.panelAlerts.Name =
                "panelAlerts";

            this.panelAlerts.Padding =
                new System.Windows.Forms.Padding(
                    12);

            this.panelAlerts.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.panelAlerts.Size =
                new System.Drawing.Size(364, 390);

            this.panelAlerts.TabIndex = 0;

            this.panelAlerts.Controls.Add(
                this.lblOperationalStatus);

            this.panelAlerts.Controls.Add(
                this.lblTodayAbsence);

            this.panelAlerts.Controls.Add(
                this.lblPendingFees);

            this.panelAlerts.Controls.Add(
                this.lblAlertsTitle);

            // lblAlertsTitle
            this.lblAlertsTitle.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.lblAlertsTitle.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblAlertsTitle.Location =
                new System.Drawing.Point(12, 12);

            this.lblAlertsTitle.Name =
                "lblAlertsTitle";

            this.lblAlertsTitle.Padding =
                new System.Windows.Forms.Padding(
                    10, 8, 10, 8);

            this.lblAlertsTitle.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.lblAlertsTitle.Size =
                new System.Drawing.Size(338, 44);

            this.lblAlertsTitle.TabIndex = 0;

            this.lblAlertsTitle.Text =
                "المتابعة والتنبيهات";

            this.lblAlertsTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleRight;

            // lblPendingFees
            ConfigureStatusLabelDesigner(
                this.lblPendingFees,
                "lblPendingFees",
                "بنود الرسوم غير المسددة: جاري التحميل...",
                1);

            // lblTodayAbsence
            ConfigureStatusLabelDesigner(
                this.lblTodayAbsence,
                "lblTodayAbsence",
                "غياب اليوم: جاري التحميل...",
                2);

            // lblOperationalStatus
            ConfigureOperationalLabelDesigner(
                this.lblOperationalStatus);

            // panelChart
            this.panelChart.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.panelChart.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.panelChart.Location =
                new System.Drawing.Point(364, 0);

            this.panelChart.Margin =
                new System.Windows.Forms.Padding(
                    8, 0, 0, 0);

            this.panelChart.Name =
                "panelChart";

            this.panelChart.Padding =
                new System.Windows.Forms.Padding(
                    12);

            this.panelChart.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.panelChart.Size =
                new System.Drawing.Size(708, 390);

            this.panelChart.TabIndex = 1;

            this.panelChart.Controls.Add(
                this.panelChartBody);

            this.panelChart.Controls.Add(
                this.lblChartTitle);

            // lblChartTitle
            this.lblChartTitle.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.lblChartTitle.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblChartTitle.Location =
                new System.Drawing.Point(12, 12);

            this.lblChartTitle.Name =
                "lblChartTitle";

            this.lblChartTitle.Padding =
                new System.Windows.Forms.Padding(
                    10, 8, 10, 8);

            this.lblChartTitle.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.lblChartTitle.Size =
                new System.Drawing.Size(682, 44);

            this.lblChartTitle.TabIndex = 0;

            this.lblChartTitle.Text =
                "توزيع الطلاب حسب الفصل";

            this.lblChartTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleRight;

            // panelChartBody
            this.panelChartBody.AutoScroll = true;
            this.panelChartBody.BackColor =
                System.Drawing.Color.White;

            this.panelChartBody.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.panelChartBody.Location =
                new System.Drawing.Point(12, 56);

            this.panelChartBody.Name =
                "panelChartBody";

            this.panelChartBody.Padding =
                new System.Windows.Forms.Padding(
                    8, 12, 8, 8);

            this.panelChartBody.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.panelChartBody.Size =
                new System.Drawing.Size(682, 320);

            this.panelChartBody.TabIndex = 1;

            // DashboardHome
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(7F, 15F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(
                    248, 250, 252);

            this.Controls.Add(
                this.tableLayoutMain);

            this.Controls.Add(
                this.panelSummary);

            this.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    9F);

            this.Margin =
                new System.Windows.Forms.Padding(0);

            this.Name =
                "DashboardHome";

            this.Padding =
                new System.Windows.Forms.Padding(0);

            this.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            this.Size =
                new System.Drawing.Size(
                    1100,
                    660);

            this.panelSummary.ResumeLayout(false);
            this.tableLayoutSummary.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelCards.ResumeLayout(false);
            this.cardStudents.ResumeLayout(false);
            this.cardTeachers.ResumeLayout(false);
            this.cardSubjects.ResumeLayout(false);
            this.cardClasses.ResumeLayout(false);
            this.cardFees.ResumeLayout(false);
            this.tableLayoutLower.ResumeLayout(false);
            this.panelAlerts.ResumeLayout(false);
            this.panelChart.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private static void ConfigureCardDesigner(
            System.Windows.Forms.Panel card,
            string name,
            int width,
            int height)
        {
            card.Margin =
                new System.Windows.Forms.Padding(6);

            card.Name = name;

            card.Size =
                new System.Drawing.Size(
                    width,
                    height);

            card.TabIndex = 0;
        }

        private static void ConfigureCardBarDesigner(
            System.Windows.Forms.Panel bar,
            string name)
        {
            bar.Dock =
                System.Windows.Forms.DockStyle.Top;

            bar.Name = name;

            bar.Size =
                new System.Drawing.Size(190, 5);

            bar.TabIndex = 0;
        }

        private static void ConfigureCardTitleDesigner(
            System.Windows.Forms.Label label,
            string name,
            string text)
        {
            label.Dock =
                System.Windows.Forms.DockStyle.Top;

            label.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    9.5F,
                    System.Drawing.FontStyle.Bold);

            label.Location =
                new System.Drawing.Point(0, 5);

            label.Name = name;

            label.Padding =
                new System.Windows.Forms.Padding(
                    8, 8, 8, 2);

            label.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            label.Size =
                new System.Drawing.Size(
                    190,
                    36);

            label.TabIndex = 1;

            label.Text = text;

            label.TextAlign =
                System.Drawing.ContentAlignment.MiddleCenter;
        }

        private static void ConfigureCardValueDesigner(
            System.Windows.Forms.Label label,
            string name,
            string text)
        {
            label.Dock =
                System.Windows.Forms.DockStyle.Fill;

            label.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    22F,
                    System.Drawing.FontStyle.Bold);

            label.Location =
                new System.Drawing.Point(
                    0,
                    41);

            label.Name = name;

            label.Padding =
                new System.Windows.Forms.Padding(
                    8, 0, 8, 8);

            label.RightToLeft =
                System.Windows.Forms.RightToLeft.No;

            label.Size =
                new System.Drawing.Size(
                    190,
                    81);

            label.TabIndex = 2;

            label.Text = text;

            label.TextAlign =
                System.Drawing.ContentAlignment.MiddleCenter;
        }

        private static void ConfigureStatusLabelDesigner(
            System.Windows.Forms.Label label,
            string name,
            string text,
            int row)
        {
            label.Dock =
                System.Windows.Forms.DockStyle.Top;

            label.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    9F,
                    System.Drawing.FontStyle.Bold);

            label.Margin =
                new System.Windows.Forms.Padding(
                    0, 8, 0, 0);

            label.Name = name;

            label.Padding =
                new System.Windows.Forms.Padding(
                    10, 8, 10, 8);

            label.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            label.Size =
                new System.Drawing.Size(
                    338,
                    54);

            label.TabIndex = row;

            label.Text = text;

            label.TextAlign =
                System.Drawing.ContentAlignment.MiddleRight;

            label.AutoEllipsis = true;
        }

        private static void ConfigureOperationalLabelDesigner(
            System.Windows.Forms.Label label)
        {
            label.Dock =
                System.Windows.Forms.DockStyle.Fill;

            label.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    9F,
                    System.Drawing.FontStyle.Bold);

            label.Margin =
                new System.Windows.Forms.Padding(
                    0, 8, 0, 0);

            label.Name =
                "lblOperationalStatus";

            label.Padding =
                new System.Windows.Forms.Padding(
                    10, 10, 10, 10);

            label.RightToLeft =
                System.Windows.Forms.RightToLeft.Yes;

            label.Size =
                new System.Drawing.Size(
                    338,
                    196);

            label.TabIndex = 3;

            label.Text =
                "حالة التشغيل: جاري التحميل...";

            label.TextAlign =
                System.Drawing.ContentAlignment.TopRight;

            label.AutoEllipsis = true;
        }

        #endregion
    }
}
