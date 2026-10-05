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
            this.cardStudents.BackColor = System.Drawing.Color.White;
            this.cardStudents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardStudents.Margin = new System.Windows.Forms.Padding(6);
            this.cardStudents.Name = "cardStudents";
            this.cardStudents.Size = new System.Drawing.Size(190, 122);
            this.cardStudents.TabIndex = 0;

            this.cardStudents.Controls.Add(
                this.lblStudentsValue);

            this.cardStudents.Controls.Add(
                this.lblStudentsTitle);

            this.cardStudents.Controls.Add(
                this.cardStudentsBar);

            this.cardStudentsBar.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.cardStudentsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardStudentsBar.Name = "cardStudentsBar";
            this.cardStudentsBar.Size = new System.Drawing.Size(190, 5);
            this.cardStudentsBar.TabIndex = 0;

            this.lblStudentsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStudentsTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStudentsTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblStudentsTitle.Location = new System.Drawing.Point(0, 5);
            this.lblStudentsTitle.Name = "lblStudentsTitle";
            this.lblStudentsTitle.Padding = new System.Windows.Forms.Padding(8, 8, 8, 2);
            this.lblStudentsTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblStudentsTitle.Size = new System.Drawing.Size(190, 36);
            this.lblStudentsTitle.TabIndex = 1;
            this.lblStudentsTitle.Text = "إجمالي الطلاب";
            this.lblStudentsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblStudentsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStudentsValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblStudentsValue.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.lblStudentsValue.Location = new System.Drawing.Point(0, 41);
            this.lblStudentsValue.Name = "lblStudentsValue";
            this.lblStudentsValue.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.lblStudentsValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStudentsValue.Size = new System.Drawing.Size(190, 81);
            this.lblStudentsValue.TabIndex = 2;
            this.lblStudentsValue.Text = "0";
            this.lblStudentsValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // cardTeachers
            this.cardTeachers.BackColor = System.Drawing.Color.White;
            this.cardTeachers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTeachers.Margin = new System.Windows.Forms.Padding(6);
            this.cardTeachers.Name = "cardTeachers";
            this.cardTeachers.Size = new System.Drawing.Size(190, 122);
            this.cardTeachers.TabIndex = 0;

            this.cardTeachers.Controls.Add(
                this.lblTeachersValue);

            this.cardTeachers.Controls.Add(
                this.lblTeachersTitle);

            this.cardTeachers.Controls.Add(
                this.cardTeachersBar);

            this.cardTeachersBar.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.cardTeachersBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardTeachersBar.Name = "cardTeachersBar";
            this.cardTeachersBar.Size = new System.Drawing.Size(190, 5);
            this.cardTeachersBar.TabIndex = 0;

            this.lblTeachersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTeachersTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTeachersTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblTeachersTitle.Location = new System.Drawing.Point(0, 5);
            this.lblTeachersTitle.Name = "lblTeachersTitle";
            this.lblTeachersTitle.Padding = new System.Windows.Forms.Padding(8, 8, 8, 2);
            this.lblTeachersTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTeachersTitle.Size = new System.Drawing.Size(190, 36);
            this.lblTeachersTitle.TabIndex = 1;
            this.lblTeachersTitle.Text = "المعلمون";
            this.lblTeachersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblTeachersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTeachersValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblTeachersValue.ForeColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.lblTeachersValue.Location = new System.Drawing.Point(0, 41);
            this.lblTeachersValue.Name = "lblTeachersValue";
            this.lblTeachersValue.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.lblTeachersValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTeachersValue.Size = new System.Drawing.Size(190, 81);
            this.lblTeachersValue.TabIndex = 2;
            this.lblTeachersValue.Text = "0";
            this.lblTeachersValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // cardSubjects
            this.cardSubjects.BackColor = System.Drawing.Color.White;
            this.cardSubjects.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardSubjects.Margin = new System.Windows.Forms.Padding(6);
            this.cardSubjects.Name = "cardSubjects";
            this.cardSubjects.Size = new System.Drawing.Size(190, 122);
            this.cardSubjects.TabIndex = 0;

            this.cardSubjects.Controls.Add(
                this.lblSubjectsValue);

            this.cardSubjects.Controls.Add(
                this.lblSubjectsTitle);

            this.cardSubjects.Controls.Add(
                this.cardSubjectsBar);

            this.cardSubjectsBar.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.cardSubjectsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardSubjectsBar.Name = "cardSubjectsBar";
            this.cardSubjectsBar.Size = new System.Drawing.Size(190, 5);
            this.cardSubjectsBar.TabIndex = 0;

            this.lblSubjectsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubjectsTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblSubjectsTitle.Location = new System.Drawing.Point(0, 5);
            this.lblSubjectsTitle.Name = "lblSubjectsTitle";
            this.lblSubjectsTitle.Padding = new System.Windows.Forms.Padding(8, 8, 8, 2);
            this.lblSubjectsTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSubjectsTitle.Size = new System.Drawing.Size(190, 36);
            this.lblSubjectsTitle.TabIndex = 1;
            this.lblSubjectsTitle.Text = "المواد الدراسية";
            this.lblSubjectsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblSubjectsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubjectsValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsValue.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.lblSubjectsValue.Location = new System.Drawing.Point(0, 41);
            this.lblSubjectsValue.Name = "lblSubjectsValue";
            this.lblSubjectsValue.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.lblSubjectsValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblSubjectsValue.Size = new System.Drawing.Size(190, 81);
            this.lblSubjectsValue.TabIndex = 2;
            this.lblSubjectsValue.Text = "0";
            this.lblSubjectsValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // cardClasses
            this.cardClasses.BackColor = System.Drawing.Color.White;
            this.cardClasses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardClasses.Margin = new System.Windows.Forms.Padding(6);
            this.cardClasses.Name = "cardClasses";
            this.cardClasses.Size = new System.Drawing.Size(190, 122);
            this.cardClasses.TabIndex = 0;

            this.cardClasses.Controls.Add(
                this.lblClassesValue);

            this.cardClasses.Controls.Add(
                this.lblClassesTitle);

            this.cardClasses.Controls.Add(
                this.cardClassesBar);

            this.cardClassesBar.BackColor = System.Drawing.Color.FromArgb(217, 119, 6);
            this.cardClassesBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardClassesBar.Name = "cardClassesBar";
            this.cardClassesBar.Size = new System.Drawing.Size(190, 5);
            this.cardClassesBar.TabIndex = 0;

            this.lblClassesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblClassesTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblClassesTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblClassesTitle.Location = new System.Drawing.Point(0, 5);
            this.lblClassesTitle.Name = "lblClassesTitle";
            this.lblClassesTitle.Padding = new System.Windows.Forms.Padding(8, 8, 8, 2);
            this.lblClassesTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClassesTitle.Size = new System.Drawing.Size(190, 36);
            this.lblClassesTitle.TabIndex = 1;
            this.lblClassesTitle.Text = "الفصول الدراسية";
            this.lblClassesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblClassesValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassesValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblClassesValue.ForeColor = System.Drawing.Color.FromArgb(217, 119, 6);
            this.lblClassesValue.Location = new System.Drawing.Point(0, 41);
            this.lblClassesValue.Name = "lblClassesValue";
            this.lblClassesValue.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.lblClassesValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblClassesValue.Size = new System.Drawing.Size(190, 81);
            this.lblClassesValue.TabIndex = 2;
            this.lblClassesValue.Text = "0";
            this.lblClassesValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // cardFees
            this.cardFees.BackColor = System.Drawing.Color.White;
            this.cardFees.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardFees.Margin = new System.Windows.Forms.Padding(6);
            this.cardFees.Name = "cardFees";
            this.cardFees.Size = new System.Drawing.Size(190, 122);
            this.cardFees.TabIndex = 0;

            this.cardFees.Controls.Add(
                this.lblFeesValue);

            this.cardFees.Controls.Add(
                this.lblFeesTitle);

            this.cardFees.Controls.Add(
                this.cardFeesBar);

            this.cardFeesBar.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.cardFeesBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardFeesBar.Name = "cardFeesBar";
            this.cardFeesBar.Size = new System.Drawing.Size(190, 5);
            this.cardFeesBar.TabIndex = 0;

            this.lblFeesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFeesTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFeesTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblFeesTitle.Location = new System.Drawing.Point(0, 5);
            this.lblFeesTitle.Name = "lblFeesTitle";
            this.lblFeesTitle.Padding = new System.Windows.Forms.Padding(8, 8, 8, 2);
            this.lblFeesTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFeesTitle.Size = new System.Drawing.Size(190, 36);
            this.lblFeesTitle.TabIndex = 1;
            this.lblFeesTitle.Text = "إجمالي الرسوم المتبقية";
            this.lblFeesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblFeesValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeesValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblFeesValue.ForeColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.lblFeesValue.Location = new System.Drawing.Point(0, 41);
            this.lblFeesValue.Name = "lblFeesValue";
            this.lblFeesValue.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.lblFeesValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFeesValue.Size = new System.Drawing.Size(190, 81);
            this.lblFeesValue.TabIndex = 2;
            this.lblFeesValue.Text = "0";
            this.lblFeesValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

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
            this.lblPendingFees.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPendingFees.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPendingFees.ForeColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.lblPendingFees.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblPendingFees.Name = "lblPendingFees";
            this.lblPendingFees.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblPendingFees.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPendingFees.Size = new System.Drawing.Size(338, 54);
            this.lblPendingFees.TabIndex = 1;
            this.lblPendingFees.Text = "بنود الرسوم غير المسددة: جاري التحميل...";
            this.lblPendingFees.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPendingFees.AutoEllipsis = true;

            // lblTodayAbsence
            this.lblTodayAbsence.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTodayAbsence.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTodayAbsence.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.lblTodayAbsence.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblTodayAbsence.Name = "lblTodayAbsence";
            this.lblTodayAbsence.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblTodayAbsence.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTodayAbsence.Size = new System.Drawing.Size(338, 54);
            this.lblTodayAbsence.TabIndex = 2;
            this.lblTodayAbsence.Text = "غياب اليوم: جاري التحميل...";
            this.lblTodayAbsence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTodayAbsence.AutoEllipsis = true;

            // lblOperationalStatus
            this.lblOperationalStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOperationalStatus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblOperationalStatus.ForeColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.lblOperationalStatus.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblOperationalStatus.Name = "lblOperationalStatus";
            this.lblOperationalStatus.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.lblOperationalStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblOperationalStatus.Size = new System.Drawing.Size(338, 196);
            this.lblOperationalStatus.TabIndex = 3;
            this.lblOperationalStatus.Text = "حالة التشغيل: جاري التحميل...";
            this.lblOperationalStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOperationalStatus.AutoEllipsis = false;

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

        #endregion
    }
}
