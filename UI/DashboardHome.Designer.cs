
namespace SchoolSystem.UI
{
    partial class DashboardHome
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region كود المصمم

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.tlpHeader = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTitle = new System.Windows.Forms.TableLayoutPanel();
            this.lblDashboardTitle = new System.Windows.Forms.Label();
            this.lblDashboardSubtitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.pnlMeta = new System.Windows.Forms.TableLayoutPanel();
            this.lblActiveAcademicYear = new System.Windows.Forms.Label();
            this.lblCurrentDateTime = new System.Windows.Forms.Label();
            this.lblLastUpdated = new System.Windows.Forms.Label();
            this.btnRefreshDashboard = new System.Windows.Forms.Button();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardStudents = new System.Windows.Forms.Panel();
            this.cardStudentsBar = new System.Windows.Forms.Panel();
            this.lblStudentsTitle = new System.Windows.Forms.Label();
            this.lblStudentsValue = new System.Windows.Forms.Label();
            this.cardTeachers = new System.Windows.Forms.Panel();
            this.cardTeachersBar = new System.Windows.Forms.Panel();
            this.lblTeachersTitle = new System.Windows.Forms.Label();
            this.lblTeachersValue = new System.Windows.Forms.Label();
            this.cardSubjects = new System.Windows.Forms.Panel();
            this.cardSubjectsBar = new System.Windows.Forms.Panel();
            this.lblSubjectsTitle = new System.Windows.Forms.Label();
            this.lblSubjectsValue = new System.Windows.Forms.Label();
            this.cardClasses = new System.Windows.Forms.Panel();
            this.cardClassesBar = new System.Windows.Forms.Panel();
            this.lblClassesTitle = new System.Windows.Forms.Label();
            this.lblClassesValue = new System.Windows.Forms.Label();
            this.cardFees = new System.Windows.Forms.Panel();
            this.cardFeesBar = new System.Windows.Forms.Panel();
            this.lblFeesTitle = new System.Windows.Forms.Label();
            this.lblFeesValue = new System.Windows.Forms.Label();
            this.tableLayoutLower = new System.Windows.Forms.TableLayoutPanel();
            this.panelAlerts = new System.Windows.Forms.Panel();
            this.lblAlertsTitle = new System.Windows.Forms.Label();
            this.lblPendingFees = new System.Windows.Forms.Label();
            this.lblTodayAbsence = new System.Windows.Forms.Label();
            this.lblOperationalStatus = new System.Windows.Forms.Label();
            this.panelChart = new Krypton.Toolkit.KryptonPanel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.panelChartBody = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.tlpHeader.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlMeta.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.panelCards.SuspendLayout();
            this.cardStudents.SuspendLayout();
            this.cardTeachers.SuspendLayout();
            this.cardSubjects.SuspendLayout();
            this.cardClasses.SuspendLayout();
            this.cardFees.SuspendLayout();
            this.tableLayoutLower.SuspendLayout();
            this.panelAlerts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelChart)).BeginInit();
            this.panelChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.tlpHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.panelHeader.Size = new System.Drawing.Size(1100, 116);
            this.panelHeader.TabIndex = 0;
            // 
            // tlpHeader
            // 
            this.tlpHeader.ColumnCount = 3;
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 265F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tlpHeader.Controls.Add(this.pnlTitle, 0, 0);
            this.tlpHeader.Controls.Add(this.pnlMeta, 1, 0);
            this.tlpHeader.Controls.Add(this.btnRefreshDashboard, 2, 0);
            this.tlpHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHeader.Location = new System.Drawing.Point(18, 12);
            this.tlpHeader.Name = "tlpHeader";
            this.tlpHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpHeader.RowCount = 1;
            this.tlpHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHeader.Size = new System.Drawing.Size(1064, 92);
            this.tlpHeader.TabIndex = 0;
            // 
            // pnlTitle
            // 
            this.pnlTitle.ColumnCount = 1;
            this.pnlTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlTitle.Controls.Add(this.lblDashboardTitle, 0, 0);
            this.pnlTitle.Controls.Add(this.lblDashboardSubtitle, 0, 1);
            this.pnlTitle.Controls.Add(this.lblWelcome, 0, 2);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(420, 3);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlTitle.RowCount = 3;
            this.pnlTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.pnlTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.pnlTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlTitle.Size = new System.Drawing.Size(641, 86);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblDashboardTitle
            // 
            this.lblDashboardTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDashboardTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblDashboardTitle.Location = new System.Drawing.Point(3, 0);
            this.lblDashboardTitle.Name = "lblDashboardTitle";
            this.lblDashboardTitle.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.lblDashboardTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblDashboardTitle.Size = new System.Drawing.Size(635, 31);
            this.lblDashboardTitle.TabIndex = 0;
            this.lblDashboardTitle.Text = "لوحة التحكم";
            this.lblDashboardTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDashboardSubtitle
            // 
            this.lblDashboardSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDashboardSubtitle.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblDashboardSubtitle.Location = new System.Drawing.Point(3, 31);
            this.lblDashboardSubtitle.Name = "lblDashboardSubtitle";
            this.lblDashboardSubtitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblDashboardSubtitle.Size = new System.Drawing.Size(635, 24);
            this.lblDashboardSubtitle.TabIndex = 1;
            this.lblDashboardSubtitle.Text = "نظرة سريعة على مؤشرات المدرسة والعمليات اليومية";
            this.lblDashboardSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(3, 55);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblWelcome.Size = new System.Drawing.Size(635, 31);
            this.lblWelcome.TabIndex = 2;
            this.lblWelcome.Text = "مرحبًا بك";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlMeta
            // 
            this.pnlMeta.ColumnCount = 1;
            this.pnlMeta.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMeta.Controls.Add(this.lblActiveAcademicYear, 0, 0);
            this.pnlMeta.Controls.Add(this.lblCurrentDateTime, 0, 1);
            this.pnlMeta.Controls.Add(this.lblLastUpdated, 0, 2);
            this.pnlMeta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMeta.Location = new System.Drawing.Point(155, 3);
            this.pnlMeta.Name = "pnlMeta";
            this.pnlMeta.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlMeta.RowCount = 3;
            this.pnlMeta.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.pnlMeta.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.pnlMeta.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.pnlMeta.Size = new System.Drawing.Size(265, 86);
            this.pnlMeta.TabIndex = 1;
            // 
            // lblActiveAcademicYear
            // 
            this.lblActiveAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblActiveAcademicYear.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblActiveAcademicYear.Location = new System.Drawing.Point(3, 0);
            this.lblActiveAcademicYear.Name = "lblActiveAcademicYear";
            this.lblActiveAcademicYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblActiveAcademicYear.Size = new System.Drawing.Size(259, 29);
            this.lblActiveAcademicYear.TabIndex = 0;
            this.lblActiveAcademicYear.Text = "العام الدراسي: جاري التحميل...";
            this.lblActiveAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentDateTime
            // 
            this.lblCurrentDateTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentDateTime.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCurrentDateTime.Location = new System.Drawing.Point(3, 29);
            this.lblCurrentDateTime.Name = "lblCurrentDateTime";
            this.lblCurrentDateTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblCurrentDateTime.Size = new System.Drawing.Size(259, 28);
            this.lblCurrentDateTime.TabIndex = 1;
            this.lblCurrentDateTime.Text = "التاريخ والوقت: --";
            this.lblCurrentDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLastUpdated.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblLastUpdated.Location = new System.Drawing.Point(3, 57);
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblLastUpdated.Size = new System.Drawing.Size(259, 29);
            this.lblLastUpdated.TabIndex = 2;
            this.lblLastUpdated.Text = "آخر تحديث: --";
            this.lblLastUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRefreshDashboard
            // 
            this.btnRefreshDashboard.AccessibleDescription = "إعادة تحميل مؤشرات لوحة التحكم والبيانات الحالية";
            this.btnRefreshDashboard.AccessibleName = "تحديث بيانات لوحة التحكم";
            this.btnRefreshDashboard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRefreshDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshDashboard.FlatAppearance.BorderSize = 0;
            this.btnRefreshDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshDashboard.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefreshDashboard.Location = new System.Drawing.Point(10, 24);
            this.btnRefreshDashboard.MinimumSize = new System.Drawing.Size(135, 44);
            this.btnRefreshDashboard.Name = "btnRefreshDashboard";
            this.btnRefreshDashboard.Size = new System.Drawing.Size(135, 44);
            this.btnRefreshDashboard.TabIndex = 2;
            this.btnRefreshDashboard.Text = "تحديث البيانات";
            this.btnRefreshDashboard.UseVisualStyleBackColor = false;
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 1;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Controls.Add(this.panelCards, 0, 0);
            this.tableLayoutMain.Controls.Add(this.tableLayoutLower, 0, 1);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 116);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.Padding = new System.Windows.Forms.Padding(14, 12, 14, 14);
            this.tableLayoutMain.RowCount = 2;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(1100, 544);
            this.tableLayoutMain.TabIndex = 1;
            // 
            // panelCards
            // 
            this.panelCards.AutoScroll = false;
            this.panelCards.BackColor = System.Drawing.Color.Transparent;
            this.panelCards.Controls.Add(this.cardStudents);
            this.panelCards.Controls.Add(this.cardTeachers);
            this.panelCards.Controls.Add(this.cardSubjects);
            this.panelCards.Controls.Add(this.cardClasses);
            this.panelCards.Controls.Add(this.cardFees);
            this.panelCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCards.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelCards.Location = new System.Drawing.Point(14, 12);
            this.panelCards.Margin = new System.Windows.Forms.Padding(0);
            this.panelCards.Name = "panelCards";
            this.panelCards.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelCards.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelCards.Size = new System.Drawing.Size(1072, 132);
            this.panelCards.TabIndex = 0;
            this.panelCards.WrapContents = true;
            // 
            // cardStudents
            // 
            this.cardStudents.Controls.Add(this.lblStudentsValue);
            this.cardStudents.Controls.Add(this.lblStudentsTitle);
            this.cardStudents.Controls.Add(this.cardStudentsBar);
            this.cardStudents.Margin = new System.Windows.Forms.Padding(6);
            this.cardStudents.Name = "cardStudents";
            this.cardStudents.Size = new System.Drawing.Size(190, 116);
            this.cardStudents.TabIndex = 0;
            // 
            // cardStudentsBar
            // 
            this.cardStudentsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardStudentsBar.Location = new System.Drawing.Point(0, 0);
            this.cardStudentsBar.Name = "cardStudentsBar";
            this.cardStudentsBar.Size = new System.Drawing.Size(190, 5);
            this.cardStudentsBar.TabIndex = 0;
            // 
            // lblStudentsTitle
            // 
            this.lblStudentsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStudentsTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStudentsTitle.Location = new System.Drawing.Point(0, 5);
            this.lblStudentsTitle.Name = "lblStudentsTitle";
            this.lblStudentsTitle.Padding = new System.Windows.Forms.Padding(12, 8, 12, 2);
            this.lblStudentsTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblStudentsTitle.Size = new System.Drawing.Size(190, 35);
            this.lblStudentsTitle.TabIndex = 1;
            this.lblStudentsTitle.Text = "الطلاب";
            this.lblStudentsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStudentsValue
            // 
            this.lblStudentsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStudentsValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblStudentsValue.Location = new System.Drawing.Point(0, 40);
            this.lblStudentsValue.Name = "lblStudentsValue";
            this.lblStudentsValue.Padding = new System.Windows.Forms.Padding(12, 0, 12, 10);
            this.lblStudentsValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStudentsValue.Size = new System.Drawing.Size(190, 76);
            this.lblStudentsValue.TabIndex = 2;
            this.lblStudentsValue.Text = "--";
            this.lblStudentsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cardTeachers
            // 
            this.cardTeachers.Controls.Add(this.lblTeachersValue);
            this.cardTeachers.Controls.Add(this.lblTeachersTitle);
            this.cardTeachers.Controls.Add(this.cardTeachersBar);
            this.cardTeachers.Margin = new System.Windows.Forms.Padding(6);
            this.cardTeachers.Name = "cardTeachers";
            this.cardTeachers.Size = new System.Drawing.Size(190, 116);
            this.cardTeachers.TabIndex = 1;
            // 
            // cardTeachersBar
            // 
            this.cardTeachersBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardTeachersBar.Location = new System.Drawing.Point(0, 0);
            this.cardTeachersBar.Name = "cardTeachersBar";
            this.cardTeachersBar.Size = new System.Drawing.Size(190, 5);
            this.cardTeachersBar.TabIndex = 0;
            // 
            // lblTeachersTitle
            // 
            this.lblTeachersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTeachersTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTeachersTitle.Location = new System.Drawing.Point(0, 5);
            this.lblTeachersTitle.Name = "lblTeachersTitle";
            this.lblTeachersTitle.Padding = new System.Windows.Forms.Padding(12, 8, 12, 2);
            this.lblTeachersTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTeachersTitle.Size = new System.Drawing.Size(190, 35);
            this.lblTeachersTitle.TabIndex = 1;
            this.lblTeachersTitle.Text = "المعلمون";
            this.lblTeachersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTeachersValue
            // 
            this.lblTeachersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTeachersValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblTeachersValue.Location = new System.Drawing.Point(0, 40);
            this.lblTeachersValue.Name = "lblTeachersValue";
            this.lblTeachersValue.Padding = new System.Windows.Forms.Padding(12, 0, 12, 10);
            this.lblTeachersValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTeachersValue.Size = new System.Drawing.Size(190, 76);
            this.lblTeachersValue.TabIndex = 2;
            this.lblTeachersValue.Text = "--";
            this.lblTeachersValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cardSubjects
            // 
            this.cardSubjects.Controls.Add(this.lblSubjectsValue);
            this.cardSubjects.Controls.Add(this.lblSubjectsTitle);
            this.cardSubjects.Controls.Add(this.cardSubjectsBar);
            this.cardSubjects.Margin = new System.Windows.Forms.Padding(6);
            this.cardSubjects.Name = "cardSubjects";
            this.cardSubjects.Size = new System.Drawing.Size(190, 116);
            this.cardSubjects.TabIndex = 2;
            // 
            // cardSubjectsBar
            // 
            this.cardSubjectsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardSubjectsBar.Location = new System.Drawing.Point(0, 0);
            this.cardSubjectsBar.Name = "cardSubjectsBar";
            this.cardSubjectsBar.Size = new System.Drawing.Size(190, 5);
            this.cardSubjectsBar.TabIndex = 0;
            // 
            // lblSubjectsTitle
            // 
            this.lblSubjectsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubjectsTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsTitle.Location = new System.Drawing.Point(0, 5);
            this.lblSubjectsTitle.Name = "lblSubjectsTitle";
            this.lblSubjectsTitle.Padding = new System.Windows.Forms.Padding(12, 8, 12, 2);
            this.lblSubjectsTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSubjectsTitle.Size = new System.Drawing.Size(190, 35);
            this.lblSubjectsTitle.TabIndex = 1;
            this.lblSubjectsTitle.Text = "المواد الدراسية";
            this.lblSubjectsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubjectsValue
            // 
            this.lblSubjectsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubjectsValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsValue.Location = new System.Drawing.Point(0, 40);
            this.lblSubjectsValue.Name = "lblSubjectsValue";
            this.lblSubjectsValue.Padding = new System.Windows.Forms.Padding(12, 0, 12, 10);
            this.lblSubjectsValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblSubjectsValue.Size = new System.Drawing.Size(190, 76);
            this.lblSubjectsValue.TabIndex = 2;
            this.lblSubjectsValue.Text = "--";
            this.lblSubjectsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cardClasses
            // 
            this.cardClasses.Controls.Add(this.lblClassesValue);
            this.cardClasses.Controls.Add(this.lblClassesTitle);
            this.cardClasses.Controls.Add(this.cardClassesBar);
            this.cardClasses.Margin = new System.Windows.Forms.Padding(6);
            this.cardClasses.Name = "cardClasses";
            this.cardClasses.Size = new System.Drawing.Size(190, 116);
            this.cardClasses.TabIndex = 3;
            // 
            // cardClassesBar
            // 
            this.cardClassesBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardClassesBar.Location = new System.Drawing.Point(0, 0);
            this.cardClassesBar.Name = "cardClassesBar";
            this.cardClassesBar.Size = new System.Drawing.Size(190, 5);
            this.cardClassesBar.TabIndex = 0;
            // 
            // lblClassesTitle
            // 
            this.lblClassesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblClassesTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblClassesTitle.Location = new System.Drawing.Point(0, 5);
            this.lblClassesTitle.Name = "lblClassesTitle";
            this.lblClassesTitle.Padding = new System.Windows.Forms.Padding(12, 8, 12, 2);
            this.lblClassesTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClassesTitle.Size = new System.Drawing.Size(190, 35);
            this.lblClassesTitle.TabIndex = 1;
            this.lblClassesTitle.Text = "الفصول";
            this.lblClassesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblClassesValue
            // 
            this.lblClassesValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassesValue.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblClassesValue.Location = new System.Drawing.Point(0, 40);
            this.lblClassesValue.Name = "lblClassesValue";
            this.lblClassesValue.Padding = new System.Windows.Forms.Padding(12, 0, 12, 10);
            this.lblClassesValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblClassesValue.Size = new System.Drawing.Size(190, 76);
            this.lblClassesValue.TabIndex = 2;
            this.lblClassesValue.Text = "--";
            this.lblClassesValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cardFees
            // 
            this.cardFees.Controls.Add(this.lblFeesValue);
            this.cardFees.Controls.Add(this.lblFeesTitle);
            this.cardFees.Controls.Add(this.cardFeesBar);
            this.cardFees.Margin = new System.Windows.Forms.Padding(6);
            this.cardFees.Name = "cardFees";
            this.cardFees.Size = new System.Drawing.Size(190, 116);
            this.cardFees.TabIndex = 4;
            // 
            // cardFeesBar
            // 
            this.cardFeesBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardFeesBar.Location = new System.Drawing.Point(0, 0);
            this.cardFeesBar.Name = "cardFeesBar";
            this.cardFeesBar.Size = new System.Drawing.Size(190, 5);
            this.cardFeesBar.TabIndex = 0;
            // 
            // lblFeesTitle
            // 
            this.lblFeesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFeesTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFeesTitle.Location = new System.Drawing.Point(0, 5);
            this.lblFeesTitle.Name = "lblFeesTitle";
            this.lblFeesTitle.Padding = new System.Windows.Forms.Padding(12, 8, 12, 2);
            this.lblFeesTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFeesTitle.Size = new System.Drawing.Size(190, 35);
            this.lblFeesTitle.TabIndex = 1;
            this.lblFeesTitle.Text = "الرسوم المتبقية";
            this.lblFeesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFeesValue
            // 
            this.lblFeesValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeesValue.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblFeesValue.Location = new System.Drawing.Point(0, 40);
            this.lblFeesValue.Name = "lblFeesValue";
            this.lblFeesValue.Padding = new System.Windows.Forms.Padding(12, 0, 12, 10);
            this.lblFeesValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFeesValue.Size = new System.Drawing.Size(190, 76);
            this.lblFeesValue.TabIndex = 2;
            this.lblFeesValue.Text = "--";
            this.lblFeesValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutLower
            // 
            this.tableLayoutLower.ColumnCount = 2;
            this.tableLayoutLower.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tableLayoutLower.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69F));
            this.tableLayoutLower.Controls.Add(this.panelAlerts, 0, 0);
            this.tableLayoutLower.Controls.Add(this.panelChart, 1, 0);
            this.tableLayoutLower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutLower.Location = new System.Drawing.Point(14, 144);
            this.tableLayoutLower.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutLower.Name = "tableLayoutLower";
            this.tableLayoutLower.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutLower.RowCount = 1;
            this.tableLayoutLower.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutLower.Size = new System.Drawing.Size(1072, 386);
            this.tableLayoutLower.TabIndex = 1;
            // 
            // panelAlerts
            // 
            this.panelAlerts.Controls.Add(this.lblOperationalStatus);
            this.panelAlerts.Controls.Add(this.lblTodayAbsence);
            this.panelAlerts.Controls.Add(this.lblPendingFees);
            this.panelAlerts.Controls.Add(this.lblAlertsTitle);
            this.panelAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAlerts.Location = new System.Drawing.Point(750, 4);
            this.panelAlerts.Margin = new System.Windows.Forms.Padding(4);
            this.panelAlerts.Name = "panelAlerts";
            this.panelAlerts.Padding = new System.Windows.Forms.Padding(12);
            this.panelAlerts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelAlerts.Size = new System.Drawing.Size(318, 378);
            this.panelAlerts.TabIndex = 0;
            // 
            // lblAlertsTitle
            // 
            this.lblAlertsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAlertsTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblAlertsTitle.Location = new System.Drawing.Point(12, 12);
            this.lblAlertsTitle.Name = "lblAlertsTitle";
            this.lblAlertsTitle.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblAlertsTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAlertsTitle.Size = new System.Drawing.Size(294, 42);
            this.lblAlertsTitle.TabIndex = 0;
            this.lblAlertsTitle.Text = "المتابعة والتنبيهات";
            this.lblAlertsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPendingFees
            // 
            this.lblPendingFees.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPendingFees.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPendingFees.Location = new System.Drawing.Point(12, 54);
            this.lblPendingFees.Name = "lblPendingFees";
            this.lblPendingFees.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.lblPendingFees.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPendingFees.Size = new System.Drawing.Size(294, 48);
            this.lblPendingFees.TabIndex = 1;
            this.lblPendingFees.Text = "بنود الرسوم غير المسددة: جاري التحميل...";
            this.lblPendingFees.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTodayAbsence
            // 
            this.lblTodayAbsence.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTodayAbsence.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTodayAbsence.Location = new System.Drawing.Point(12, 102);
            this.lblTodayAbsence.Name = "lblTodayAbsence";
            this.lblTodayAbsence.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.lblTodayAbsence.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTodayAbsence.Size = new System.Drawing.Size(294, 48);
            this.lblTodayAbsence.TabIndex = 2;
            this.lblTodayAbsence.Text = "غياب اليوم: جاري التحميل...";
            this.lblTodayAbsence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOperationalStatus
            // 
            this.lblOperationalStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOperationalStatus.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblOperationalStatus.Location = new System.Drawing.Point(12, 150);
            this.lblOperationalStatus.Name = "lblOperationalStatus";
            this.lblOperationalStatus.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblOperationalStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblOperationalStatus.Size = new System.Drawing.Size(294, 92);
            this.lblOperationalStatus.TabIndex = 3;
            this.lblOperationalStatus.Text = "حالة التشغيل: جاري التحميل...";
            this.lblOperationalStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelChart
            // 
            this.panelChart.Controls.Add(this.panelChartBody);
            this.panelChart.Controls.Add(this.lblChartTitle);
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.Location = new System.Drawing.Point(4, 4);
            this.panelChart.Margin = new System.Windows.Forms.Padding(4);
            this.panelChart.Name = "panelChart";
            this.panelChart.Padding = new System.Windows.Forms.Padding(14);
            this.panelChart.Size = new System.Drawing.Size(742, 378);
            this.panelChart.TabIndex = 1;
            // 
            // lblChartTitle
            // 
            this.lblChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.Location = new System.Drawing.Point(14, 14);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Padding = new System.Windows.Forms.Padding(4, 0, 4, 6);
            this.lblChartTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblChartTitle.Size = new System.Drawing.Size(714, 40);
            this.lblChartTitle.TabIndex = 0;
            this.lblChartTitle.Text = "توزيع الطلاب حسب الفصل";
            this.lblChartTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelChartBody
            // 
            this.panelChartBody.AutoScroll = true;
            this.panelChartBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChartBody.Location = new System.Drawing.Point(14, 54);
            this.panelChartBody.Name = "panelChartBody";
            this.panelChartBody.Padding = new System.Windows.Forms.Padding(8);
            this.panelChartBody.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelChartBody.Size = new System.Drawing.Size(714, 310);
            this.panelChartBody.TabIndex = 1;
            // 
            // DashboardHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.tableLayoutMain);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.MinimumSize = new System.Drawing.Size(820, 520);
            this.Name = "DashboardHome";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1100, 660);
            this.panelHeader.ResumeLayout(false);
            this.tlpHeader.ResumeLayout(false);
            this.pnlTitle.ResumeLayout(false);
            this.pnlMeta.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelCards.ResumeLayout(false);
            this.cardStudents.ResumeLayout(false);
            this.cardTeachers.ResumeLayout(false);
            this.cardSubjects.ResumeLayout(false);
            this.cardClasses.ResumeLayout(false);
            this.cardFees.ResumeLayout(false);
            this.tableLayoutLower.ResumeLayout(false);
            this.panelAlerts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelChart)).EndInit();
            this.panelChart.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.TableLayoutPanel pnlTitle;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.Label lblDashboardSubtitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TableLayoutPanel pnlMeta;
        private System.Windows.Forms.Label lblActiveAcademicYear;
        private System.Windows.Forms.Label lblCurrentDateTime;
        private System.Windows.Forms.Label lblLastUpdated;
        private System.Windows.Forms.Button btnRefreshDashboard;
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
        private Krypton.Toolkit.KryptonPanel panelChart;
        private System.Windows.Forms.Label lblChartTitle;
        private System.Windows.Forms.Panel panelChartBody;
    }
}
