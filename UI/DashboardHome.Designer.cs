
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
            this.pnlTitleYear = new System.Windows.Forms.TableLayoutPanel();
            this.lblDashboardTitle = new System.Windows.Forms.Label();
            this.lblActiveAcademicYear = new System.Windows.Forms.Label();
            this.btnRefreshDashboard = new System.Windows.Forms.Button();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelCards = new System.Windows.Forms.TableLayoutPanel();
            this.cardStudents = new System.Windows.Forms.Panel();
            this.lblStudentsValue = new System.Windows.Forms.Label();
            this.lblStudentsTitle = new System.Windows.Forms.Label();
            this.cardStudentsBar = new System.Windows.Forms.Panel();
            this.cardTeachers = new System.Windows.Forms.Panel();
            this.lblTeachersValue = new System.Windows.Forms.Label();
            this.lblTeachersTitle = new System.Windows.Forms.Label();
            this.cardTeachersBar = new System.Windows.Forms.Panel();
            this.cardSubjects = new System.Windows.Forms.Panel();
            this.lblSubjectsValue = new System.Windows.Forms.Label();
            this.lblSubjectsTitle = new System.Windows.Forms.Label();
            this.cardSubjectsBar = new System.Windows.Forms.Panel();
            this.cardClasses = new System.Windows.Forms.Panel();
            this.lblClassesValue = new System.Windows.Forms.Label();
            this.lblClassesTitle = new System.Windows.Forms.Label();
            this.cardClassesBar = new System.Windows.Forms.Panel();
            this.cardFees = new System.Windows.Forms.Panel();
            this.lblFeesValue = new System.Windows.Forms.Label();
            this.lblFeesTitle = new System.Windows.Forms.Label();
            this.cardFeesBar = new System.Windows.Forms.Panel();
            this.panelChart = new Krypton.Toolkit.KryptonPanel();
            this.panelChartBody = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.panelAlerts = new System.Windows.Forms.Panel();
            this.lblOperationalStatus = new System.Windows.Forms.Label();
            this.lblTodayAbsence = new System.Windows.Forms.Label();
            this.lblPendingFees = new System.Windows.Forms.Label();
            this.lblAlertsTitle = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.tlpHeader.SuspendLayout();
            this.pnlTitleYear.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.panelCards.SuspendLayout();
            this.cardStudents.SuspendLayout();
            this.cardTeachers.SuspendLayout();
            this.cardSubjects.SuspendLayout();
            this.cardClasses.SuspendLayout();
            this.cardFees.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelChart)).BeginInit();
            this.panelChart.SuspendLayout();
            this.panelAlerts.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.tlpHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
            this.panelHeader.Size = new System.Drawing.Size(950, 95);
            this.panelHeader.TabIndex = 0;
            // 
            // tlpHeader
            // 
            this.tlpHeader.ColumnCount = 2;
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tlpHeader.Controls.Add(this.pnlTitleYear, 0, 0);
            this.tlpHeader.Controls.Add(this.btnRefreshDashboard, 1, 0);
            this.tlpHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHeader.Location = new System.Drawing.Point(14, 8);
            this.tlpHeader.Name = "tlpHeader";
            this.tlpHeader.RowCount = 1;
            this.tlpHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHeader.Size = new System.Drawing.Size(922, 79);
            this.tlpHeader.TabIndex = 0;
            // 
            // pnlTitleYear
            // 
            this.pnlTitleYear.ColumnCount = 1;
            this.pnlTitleYear.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlTitleYear.Controls.Add(this.lblDashboardTitle, 0, 0);
            this.pnlTitleYear.Controls.Add(this.lblActiveAcademicYear, 0, 1);
            this.pnlTitleYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitleYear.Location = new System.Drawing.Point(193, 3);
            this.pnlTitleYear.Name = "pnlTitleYear";
            this.pnlTitleYear.RowCount = 2;
            this.pnlTitleYear.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTitleYear.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.pnlTitleYear.Size = new System.Drawing.Size(726, 73);
            this.pnlTitleYear.TabIndex = 0;
            // 
            // lblDashboardTitle
            // 
            this.lblDashboardTitle.AutoSize = true;
            this.lblDashboardTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDashboardTitle.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblDashboardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblDashboardTitle.Location = new System.Drawing.Point(3, 0);
            this.lblDashboardTitle.Name = "lblDashboardTitle";
            this.lblDashboardTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblDashboardTitle.Size = new System.Drawing.Size(720, 26);
            this.lblDashboardTitle.TabIndex = 0;
            this.lblDashboardTitle.Text = "لوحة المعلومات";
            this.lblDashboardTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblActiveAcademicYear
            // 
            this.lblActiveAcademicYear.AutoSize = true;
            this.lblActiveAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblActiveAcademicYear.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblActiveAcademicYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblActiveAcademicYear.Location = new System.Drawing.Point(3, 26);
            this.lblActiveAcademicYear.Name = "lblActiveAcademicYear";
            this.lblActiveAcademicYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblActiveAcademicYear.Size = new System.Drawing.Size(720, 47);
            this.lblActiveAcademicYear.TabIndex = 1;
            this.lblActiveAcademicYear.Text = "العام النشط: جاري التحميل...";
            this.lblActiveAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRefreshDashboard
            // 
            this.btnRefreshDashboard.AccessibleDescription = "إعادة تحميل مؤشرات لوحة التحكم والبيانات الحالية";
            this.btnRefreshDashboard.AccessibleName = "تحديث بيانات لوحة التحكم";
            this.btnRefreshDashboard.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRefreshDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnRefreshDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshDashboard.FlatAppearance.BorderSize = 0;
            this.btnRefreshDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshDashboard.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefreshDashboard.ForeColor = System.Drawing.Color.White;
            this.btnRefreshDashboard.Location = new System.Drawing.Point(7, 20);
            this.btnRefreshDashboard.MinimumSize = new System.Drawing.Size(180, 38);
            this.btnRefreshDashboard.Name = "btnRefreshDashboard";
            this.btnRefreshDashboard.Size = new System.Drawing.Size(180, 38);
            this.btnRefreshDashboard.TabIndex = 1;
            this.btnRefreshDashboard.Text = "تحديث البيانات";
            this.btnRefreshDashboard.UseVisualStyleBackColor = false;
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tableLayoutMain.Controls.Add(this.panelCards, 0, 0);
            this.tableLayoutMain.Controls.Add(this.panelChart, 0, 1);
            this.tableLayoutMain.Controls.Add(this.panelAlerts, 1, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 95);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.Padding = new System.Windows.Forms.Padding(12);
            this.tableLayoutMain.RowCount = 2;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(950, 465);
            this.tableLayoutMain.TabIndex = 1;
            // 
            // panelCards
            // 
            this.panelCards.ColumnCount = 5;
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.Controls.Add(this.cardStudents, 0, 0);
            this.panelCards.Controls.Add(this.cardTeachers, 1, 0);
            this.panelCards.Controls.Add(this.cardSubjects, 2, 0);
            this.panelCards.Controls.Add(this.cardClasses, 3, 0);
            this.panelCards.Controls.Add(this.cardFees, 4, 0);
            this.panelCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCards.Location = new System.Drawing.Point(364, 12);
            this.panelCards.Margin = new System.Windows.Forms.Padding(0);
            this.panelCards.Name = "panelCards";
            this.panelCards.RowCount = 1;
            this.panelCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelCards.Size = new System.Drawing.Size(574, 132);
            this.panelCards.TabIndex = 0;
            // 
            // cardStudents
            // 
            this.cardStudents.BackColor = System.Drawing.Color.White;
            this.cardStudents.Controls.Add(this.lblStudentsValue);
            this.cardStudents.Controls.Add(this.lblStudentsTitle);
            this.cardStudents.Controls.Add(this.cardStudentsBar);
            this.cardStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardStudents.Location = new System.Drawing.Point(464, 4);
            this.cardStudents.Margin = new System.Windows.Forms.Padding(4);
            this.cardStudents.Name = "cardStudents";
            this.cardStudents.Size = new System.Drawing.Size(106, 124);
            this.cardStudents.TabIndex = 0;
            // 
            // lblStudentsValue
            // 
            this.lblStudentsValue.AutoSize = true;
            this.lblStudentsValue.Font = new System.Drawing.Font("Tahoma", 19F, System.Drawing.FontStyle.Bold);
            this.lblStudentsValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblStudentsValue.Location = new System.Drawing.Point(8, 46);
            this.lblStudentsValue.Name = "lblStudentsValue";
            this.lblStudentsValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblStudentsValue.Size = new System.Drawing.Size(45, 39);
            this.lblStudentsValue.TabIndex = 2;
            this.lblStudentsValue.Text = "--";
            // 
            // lblStudentsTitle
            // 
            this.lblStudentsTitle.AutoSize = true;
            this.lblStudentsTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStudentsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblStudentsTitle.Location = new System.Drawing.Point(8, 18);
            this.lblStudentsTitle.Name = "lblStudentsTitle";
            this.lblStudentsTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblStudentsTitle.Size = new System.Drawing.Size(59, 19);
            this.lblStudentsTitle.TabIndex = 1;
            this.lblStudentsTitle.Text = "الطلاب";
            // 
            // cardStudentsBar
            // 
            this.cardStudentsBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.cardStudentsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardStudentsBar.Location = new System.Drawing.Point(0, 0);
            this.cardStudentsBar.Name = "cardStudentsBar";
            this.cardStudentsBar.Size = new System.Drawing.Size(106, 5);
            this.cardStudentsBar.TabIndex = 0;
            // 
            // cardTeachers
            // 
            this.cardTeachers.BackColor = System.Drawing.Color.White;
            this.cardTeachers.Controls.Add(this.lblTeachersValue);
            this.cardTeachers.Controls.Add(this.lblTeachersTitle);
            this.cardTeachers.Controls.Add(this.cardTeachersBar);
            this.cardTeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTeachers.Location = new System.Drawing.Point(350, 4);
            this.cardTeachers.Margin = new System.Windows.Forms.Padding(4);
            this.cardTeachers.Name = "cardTeachers";
            this.cardTeachers.Size = new System.Drawing.Size(106, 124);
            this.cardTeachers.TabIndex = 1;
            // 
            // lblTeachersValue
            // 
            this.lblTeachersValue.AutoSize = true;
            this.lblTeachersValue.Font = new System.Drawing.Font("Tahoma", 19F, System.Drawing.FontStyle.Bold);
            this.lblTeachersValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblTeachersValue.Location = new System.Drawing.Point(8, 46);
            this.lblTeachersValue.Name = "lblTeachersValue";
            this.lblTeachersValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTeachersValue.Size = new System.Drawing.Size(45, 39);
            this.lblTeachersValue.TabIndex = 2;
            this.lblTeachersValue.Text = "--";
            // 
            // lblTeachersTitle
            // 
            this.lblTeachersTitle.AutoSize = true;
            this.lblTeachersTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTeachersTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblTeachersTitle.Location = new System.Drawing.Point(8, 18);
            this.lblTeachersTitle.Name = "lblTeachersTitle";
            this.lblTeachersTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTeachersTitle.Size = new System.Drawing.Size(76, 19);
            this.lblTeachersTitle.TabIndex = 1;
            this.lblTeachersTitle.Text = "المعلمون";
            // 
            // cardTeachersBar
            // 
            this.cardTeachersBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.cardTeachersBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardTeachersBar.Location = new System.Drawing.Point(0, 0);
            this.cardTeachersBar.Name = "cardTeachersBar";
            this.cardTeachersBar.Size = new System.Drawing.Size(106, 5);
            this.cardTeachersBar.TabIndex = 0;
            // 
            // cardSubjects
            // 
            this.cardSubjects.BackColor = System.Drawing.Color.White;
            this.cardSubjects.Controls.Add(this.lblSubjectsValue);
            this.cardSubjects.Controls.Add(this.lblSubjectsTitle);
            this.cardSubjects.Controls.Add(this.cardSubjectsBar);
            this.cardSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardSubjects.Location = new System.Drawing.Point(236, 4);
            this.cardSubjects.Margin = new System.Windows.Forms.Padding(4);
            this.cardSubjects.Name = "cardSubjects";
            this.cardSubjects.Size = new System.Drawing.Size(106, 124);
            this.cardSubjects.TabIndex = 2;
            // 
            // lblSubjectsValue
            // 
            this.lblSubjectsValue.AutoSize = true;
            this.lblSubjectsValue.Font = new System.Drawing.Font("Tahoma", 19F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblSubjectsValue.Location = new System.Drawing.Point(8, 46);
            this.lblSubjectsValue.Name = "lblSubjectsValue";
            this.lblSubjectsValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSubjectsValue.Size = new System.Drawing.Size(45, 39);
            this.lblSubjectsValue.TabIndex = 2;
            this.lblSubjectsValue.Text = "--";
            // 
            // lblSubjectsTitle
            // 
            this.lblSubjectsTitle.AutoSize = true;
            this.lblSubjectsTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSubjectsTitle.Location = new System.Drawing.Point(8, 18);
            this.lblSubjectsTitle.Name = "lblSubjectsTitle";
            this.lblSubjectsTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSubjectsTitle.Size = new System.Drawing.Size(51, 19);
            this.lblSubjectsTitle.TabIndex = 1;
            this.lblSubjectsTitle.Text = "المواد";
            // 
            // cardSubjectsBar
            // 
            this.cardSubjectsBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.cardSubjectsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardSubjectsBar.Location = new System.Drawing.Point(0, 0);
            this.cardSubjectsBar.Name = "cardSubjectsBar";
            this.cardSubjectsBar.Size = new System.Drawing.Size(106, 5);
            this.cardSubjectsBar.TabIndex = 0;
            // 
            // cardClasses
            // 
            this.cardClasses.BackColor = System.Drawing.Color.White;
            this.cardClasses.Controls.Add(this.lblClassesValue);
            this.cardClasses.Controls.Add(this.lblClassesTitle);
            this.cardClasses.Controls.Add(this.cardClassesBar);
            this.cardClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardClasses.Location = new System.Drawing.Point(122, 4);
            this.cardClasses.Margin = new System.Windows.Forms.Padding(4);
            this.cardClasses.Name = "cardClasses";
            this.cardClasses.Size = new System.Drawing.Size(106, 124);
            this.cardClasses.TabIndex = 3;
            // 
            // lblClassesValue
            // 
            this.lblClassesValue.AutoSize = true;
            this.lblClassesValue.Font = new System.Drawing.Font("Tahoma", 19F, System.Drawing.FontStyle.Bold);
            this.lblClassesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblClassesValue.Location = new System.Drawing.Point(8, 46);
            this.lblClassesValue.Name = "lblClassesValue";
            this.lblClassesValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClassesValue.Size = new System.Drawing.Size(45, 39);
            this.lblClassesValue.TabIndex = 2;
            this.lblClassesValue.Text = "--";
            // 
            // lblClassesTitle
            // 
            this.lblClassesTitle.AutoSize = true;
            this.lblClassesTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblClassesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblClassesTitle.Location = new System.Drawing.Point(8, 18);
            this.lblClassesTitle.Name = "lblClassesTitle";
            this.lblClassesTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClassesTitle.Size = new System.Drawing.Size(64, 19);
            this.lblClassesTitle.TabIndex = 1;
            this.lblClassesTitle.Text = "الفصول";
            // 
            // cardClassesBar
            // 
            this.cardClassesBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.cardClassesBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardClassesBar.Location = new System.Drawing.Point(0, 0);
            this.cardClassesBar.Name = "cardClassesBar";
            this.cardClassesBar.Size = new System.Drawing.Size(106, 5);
            this.cardClassesBar.TabIndex = 0;
            // 
            // cardFees
            // 
            this.cardFees.BackColor = System.Drawing.Color.White;
            this.cardFees.Controls.Add(this.lblFeesValue);
            this.cardFees.Controls.Add(this.lblFeesTitle);
            this.cardFees.Controls.Add(this.cardFeesBar);
            this.cardFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardFees.Location = new System.Drawing.Point(4, 4);
            this.cardFees.Margin = new System.Windows.Forms.Padding(4);
            this.cardFees.Name = "cardFees";
            this.cardFees.Size = new System.Drawing.Size(110, 124);
            this.cardFees.TabIndex = 4;
            // 
            // lblFeesValue
            // 
            this.lblFeesValue.AutoSize = true;
            this.lblFeesValue.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblFeesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblFeesValue.Location = new System.Drawing.Point(8, 48);
            this.lblFeesValue.Name = "lblFeesValue";
            this.lblFeesValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFeesValue.Size = new System.Drawing.Size(35, 30);
            this.lblFeesValue.TabIndex = 2;
            this.lblFeesValue.Text = "--";
            // 
            // lblFeesTitle
            // 
            this.lblFeesTitle.AutoSize = true;
            this.lblFeesTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFeesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblFeesTitle.Location = new System.Drawing.Point(8, 18);
            this.lblFeesTitle.Name = "lblFeesTitle";
            this.lblFeesTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFeesTitle.Size = new System.Drawing.Size(127, 19);
            this.lblFeesTitle.TabIndex = 1;
            this.lblFeesTitle.Text = "الرسوم المتبقية";
            // 
            // cardFeesBar
            // 
            this.cardFeesBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.cardFeesBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardFeesBar.Location = new System.Drawing.Point(0, 0);
            this.cardFeesBar.Name = "cardFeesBar";
            this.cardFeesBar.Size = new System.Drawing.Size(110, 5);
            this.cardFeesBar.TabIndex = 0;
            // 
            // panelChart
            // 
            this.panelChart.Controls.Add(this.panelChartBody);
            this.panelChart.Controls.Add(this.lblChartTitle);
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.Location = new System.Drawing.Point(367, 147);
            this.panelChart.Name = "panelChart";
            this.panelChart.Padding = new System.Windows.Forms.Padding(10);
            this.panelChart.Size = new System.Drawing.Size(568, 303);
            this.panelChart.TabIndex = 1;
            // 
            // panelChartBody
            // 
            this.panelChartBody.AutoScroll = true;
            this.panelChartBody.BackColor = System.Drawing.Color.White;
            this.panelChartBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChartBody.Location = new System.Drawing.Point(10, 44);
            this.panelChartBody.Name = "panelChartBody";
            this.panelChartBody.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.panelChartBody.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelChartBody.Size = new System.Drawing.Size(548, 249);
            this.panelChartBody.TabIndex = 1;
            // 
            // lblChartTitle
            // 
            this.lblChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblChartTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblChartTitle.Location = new System.Drawing.Point(10, 10);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Padding = new System.Windows.Forms.Padding(2, 2, 2, 6);
            this.lblChartTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblChartTitle.Size = new System.Drawing.Size(548, 34);
            this.lblChartTitle.TabIndex = 0;
            this.lblChartTitle.Text = "توزيع الطلاب في الفصول";
            this.lblChartTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelAlerts
            // 
            this.panelAlerts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(237)))));
            this.panelAlerts.Controls.Add(this.lblOperationalStatus);
            this.panelAlerts.Controls.Add(this.lblTodayAbsence);
            this.panelAlerts.Controls.Add(this.lblPendingFees);
            this.panelAlerts.Controls.Add(this.lblAlertsTitle);
            this.panelAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAlerts.Location = new System.Drawing.Point(15, 15);
            this.panelAlerts.Name = "panelAlerts";
            this.panelAlerts.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutMain.SetRowSpan(this.panelAlerts, 2);
            this.panelAlerts.Size = new System.Drawing.Size(346, 435);
            this.panelAlerts.TabIndex = 2;
            // 
            // lblOperationalStatus
            // 
            this.lblOperationalStatus.BackColor = System.Drawing.Color.White;
            this.lblOperationalStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOperationalStatus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblOperationalStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblOperationalStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOperationalStatus.Location = new System.Drawing.Point(10, 110);
            this.lblOperationalStatus.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.lblOperationalStatus.Name = "lblOperationalStatus";
            this.lblOperationalStatus.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.lblOperationalStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblOperationalStatus.Size = new System.Drawing.Size(326, 56);
            this.lblOperationalStatus.TabIndex = 3;
            this.lblOperationalStatus.Text = "حالة التشغيل: جاري التحميل...";
            // 
            // lblTodayAbsence
            // 
            this.lblTodayAbsence.BackColor = System.Drawing.Color.White;
            this.lblTodayAbsence.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTodayAbsence.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblTodayAbsence.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.lblTodayAbsence.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTodayAbsence.Location = new System.Drawing.Point(10, 78);
            this.lblTodayAbsence.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.lblTodayAbsence.Name = "lblTodayAbsence";
            this.lblTodayAbsence.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.lblTodayAbsence.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTodayAbsence.Size = new System.Drawing.Size(326, 32);
            this.lblTodayAbsence.TabIndex = 2;
            this.lblTodayAbsence.Text = "غياب اليوم: جاري التحميل...";
            this.lblTodayAbsence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPendingFees
            // 
            this.lblPendingFees.BackColor = System.Drawing.Color.White;
            this.lblPendingFees.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPendingFees.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblPendingFees.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(65)))), ((int)(((byte)(12)))));
            this.lblPendingFees.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPendingFees.Location = new System.Drawing.Point(10, 46);
            this.lblPendingFees.Margin = new System.Windows.Forms.Padding(3);
            this.lblPendingFees.Name = "lblPendingFees";
            this.lblPendingFees.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.lblPendingFees.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPendingFees.Size = new System.Drawing.Size(326, 32);
            this.lblPendingFees.TabIndex = 1;
            this.lblPendingFees.Text = "الرسوم غير المدفوعة: جاري التحميل...";
            this.lblPendingFees.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAlertsTitle
            // 
            this.lblAlertsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAlertsTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblAlertsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblAlertsTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAlertsTitle.Location = new System.Drawing.Point(10, 10);
            this.lblAlertsTitle.Name = "lblAlertsTitle";
            this.lblAlertsTitle.Padding = new System.Windows.Forms.Padding(6, 2, 6, 6);
            this.lblAlertsTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAlertsTitle.Size = new System.Drawing.Size(326, 36);
            this.lblAlertsTitle.TabIndex = 0;
            this.lblAlertsTitle.Text = "التنبيهات";
            this.lblAlertsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DashboardHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.Controls.Add(this.tableLayoutMain);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "DashboardHome";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(950, 560);
            this.panelHeader.ResumeLayout(false);
            this.tlpHeader.ResumeLayout(false);
            this.pnlTitleYear.ResumeLayout(false);
            this.pnlTitleYear.PerformLayout();
            this.tableLayoutMain.ResumeLayout(false);
            this.panelCards.ResumeLayout(false);
            this.cardStudents.ResumeLayout(false);
            this.cardStudents.PerformLayout();
            this.cardTeachers.ResumeLayout(false);
            this.cardTeachers.PerformLayout();
            this.cardSubjects.ResumeLayout(false);
            this.cardSubjects.PerformLayout();
            this.cardClasses.ResumeLayout(false);
            this.cardClasses.PerformLayout();
            this.cardFees.ResumeLayout(false);
            this.cardFees.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelChart)).EndInit();
            this.panelChart.ResumeLayout(false);
            this.panelAlerts.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion


        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.TableLayoutPanel pnlTitleYear;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.Label lblActiveAcademicYear;
        private System.Windows.Forms.Button btnRefreshDashboard;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.TableLayoutPanel panelCards;
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
        private Krypton.Toolkit.KryptonPanel panelChart;
        private System.Windows.Forms.Label lblChartTitle;
        private System.Windows.Forms.Panel panelChartBody;
        private System.Windows.Forms.Panel panelAlerts;
        private System.Windows.Forms.Label lblAlertsTitle;
        private System.Windows.Forms.Label lblPendingFees;
        private System.Windows.Forms.Label lblTodayAbsence;
        private System.Windows.Forms.Label lblOperationalStatus;
    }
}
