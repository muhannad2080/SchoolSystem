namespace SchoolSystem.UI
{
    partial class ReportCenterForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportCenterForm));
            this.panelTitle = new Krypton.Toolkit.KryptonPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxFilters = new System.Windows.Forms.GroupBox();
            this.tableLayoutFilters = new System.Windows.Forms.TableLayoutPanel();
            this.lblReportType = new System.Windows.Forms.Label();
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.lblAcademicYear = new System.Windows.Forms.Label();
            this.txtAcademicYear = new System.Windows.Forms.TextBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblSection = new System.Windows.Forms.Label();
            this.cmbSection = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblSummary = new System.Windows.Forms.Label();
            this.panelActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dataGridViewReport = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.groupBoxFilters.SuspendLayout();
            this.tableLayoutFilters.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReport)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1200, 60);
            this.panelTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1200, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "مركز التقارير المدرسية";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainContainer
            // 
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.groupBoxFilters, 0, 0);
            this.mainContainer.Controls.Add(this.panelSummary, 0, 1);
            this.mainContainer.Controls.Add(this.panelActions, 0, 2);
            this.mainContainer.Controls.Add(this.dataGridViewReport, 0, 3);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 4);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 60);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.RowCount = 5;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainContainer.Size = new System.Drawing.Size(1200, 690);
            this.mainContainer.TabIndex = 1;
            // 
            // groupBoxFilters
            // 
            this.groupBoxFilters.BackColor = System.Drawing.Color.White;
            this.groupBoxFilters.Controls.Add(this.tableLayoutFilters);
            this.groupBoxFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFilters.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxFilters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxFilters.Location = new System.Drawing.Point(15, 13);
            this.groupBoxFilters.Name = "groupBoxFilters";
            this.groupBoxFilters.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxFilters.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxFilters.Size = new System.Drawing.Size(1170, 144);
            this.groupBoxFilters.TabIndex = 0;
            this.groupBoxFilters.TabStop = false;
            this.groupBoxFilters.Text = "خيارات التقرير والفلاتر";
            // 
            // tableLayoutFilters
            // 
            this.tableLayoutFilters.ColumnCount = 8;
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFilters.Controls.Add(this.lblReportType, 0, 0);
            this.tableLayoutFilters.Controls.Add(this.cmbReportType, 1, 0);
            this.tableLayoutFilters.Controls.Add(this.lblAcademicYear, 2, 0);
            this.tableLayoutFilters.Controls.Add(this.txtAcademicYear, 3, 0);
            this.tableLayoutFilters.Controls.Add(this.lblClass, 4, 0);
            this.tableLayoutFilters.Controls.Add(this.cmbClass, 5, 0);
            this.tableLayoutFilters.Controls.Add(this.lblSection, 6, 0);
            this.tableLayoutFilters.Controls.Add(this.cmbSection, 7, 0);
            this.tableLayoutFilters.Controls.Add(this.lblStatus, 0, 1);
            this.tableLayoutFilters.Controls.Add(this.cmbStatus, 1, 1);
            this.tableLayoutFilters.Controls.Add(this.lblFromDate, 2, 1);
            this.tableLayoutFilters.Controls.Add(this.dtpFromDate, 3, 1);
            this.tableLayoutFilters.Controls.Add(this.lblToDate, 4, 1);
            this.tableLayoutFilters.Controls.Add(this.dtpToDate, 5, 1);
            this.tableLayoutFilters.Controls.Add(this.lblSearch, 6, 1);
            this.tableLayoutFilters.Controls.Add(this.txtSearch, 7, 1);
            this.tableLayoutFilters.Controls.Add(this.btnLoad, 6, 2);
            this.tableLayoutFilters.Controls.Add(this.btnRefresh, 7, 2);
            this.tableLayoutFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFilters.Location = new System.Drawing.Point(12, 29);
            this.tableLayoutFilters.Name = "tableLayoutFilters";
            this.tableLayoutFilters.RowCount = 3;
            this.tableLayoutFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.tableLayoutFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.tableLayoutFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.tableLayoutFilters.Size = new System.Drawing.Size(1146, 105);
            this.tableLayoutFilters.TabIndex = 0;
            // 
            // lblReportType
            // 
            this.lblReportType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReportType.Location = new System.Drawing.Point(1039, 0);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(104, 35);
            this.lblReportType.TabIndex = 0;
            this.lblReportType.Text = "نوع التقرير:";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbReportType
            // 
            this.cmbReportType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.Location = new System.Drawing.Point(863, 3);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(170, 29);
            this.cmbReportType.TabIndex = 1;
            // 
            // lblAcademicYear
            // 
            this.lblAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicYear.Location = new System.Drawing.Point(753, 0);
            this.lblAcademicYear.Name = "lblAcademicYear";
            this.lblAcademicYear.Size = new System.Drawing.Size(104, 35);
            this.lblAcademicYear.TabIndex = 2;
            this.lblAcademicYear.Text = "العام الدراسي:";
            this.lblAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAcademicYear
            // 
            this.txtAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAcademicYear.Location = new System.Drawing.Point(577, 3);
            this.txtAcademicYear.Name = "txtAcademicYear";
            this.txtAcademicYear.Size = new System.Drawing.Size(170, 28);
            this.txtAcademicYear.TabIndex = 3;
            // 
            // lblClass
            // 
            this.lblClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClass.Location = new System.Drawing.Point(467, 0);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(104, 35);
            this.lblClass.TabIndex = 4;
            this.lblClass.Text = "الصف:";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbClass
            // 
            this.cmbClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.Location = new System.Drawing.Point(291, 3);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(170, 29);
            this.cmbClass.TabIndex = 5;
            // 
            // lblSection
            // 
            this.lblSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSection.Location = new System.Drawing.Point(181, 0);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(104, 35);
            this.lblSection.TabIndex = 6;
            this.lblSection.Text = "الشعبة:";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSection
            // 
            this.cmbSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSection.Location = new System.Drawing.Point(3, 3);
            this.cmbSection.Name = "cmbSection";
            this.cmbSection.Size = new System.Drawing.Size(172, 29);
            this.cmbSection.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(1039, 35);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(104, 35);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "الحالة:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Location = new System.Drawing.Point(863, 38);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(170, 29);
            this.cmbStatus.TabIndex = 9;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFromDate.Location = new System.Drawing.Point(753, 35);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(104, 35);
            this.lblFromDate.TabIndex = 10;
            this.lblFromDate.Text = "من تاريخ:";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(577, 38);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(170, 28);
            this.dtpFromDate.TabIndex = 11;
            // 
            // lblToDate
            // 
            this.lblToDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblToDate.Location = new System.Drawing.Point(467, 35);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(104, 35);
            this.lblToDate.TabIndex = 12;
            this.lblToDate.Text = "إلى تاريخ:";
            this.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(291, 38);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(170, 28);
            this.dtpToDate.TabIndex = 13;
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSearch.Location = new System.Drawing.Point(181, 35);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(104, 35);
            this.lblSearch.TabIndex = 14;
            this.lblSearch.Text = "بحث:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(3, 38);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(172, 28);
            this.txtSearch.TabIndex = 15;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(181, 73);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(104, 29);
            this.btnLoad.TabIndex = 16;
            this.btnLoad.Text = "تحميل التقرير";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(3, 73);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(172, 29);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.White;
            this.panelSummary.Controls.Add(this.lblSummary);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSummary.Location = new System.Drawing.Point(15, 163);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1170, 36);
            this.panelSummary.TabIndex = 1;
            // 
            // lblSummary
            // 
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblSummary.Location = new System.Drawing.Point(0, 0);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(1170, 36);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "ملخص التقرير: لا توجد بيانات محملة.";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelActions
            // 
            this.panelActions.BackColor = System.Drawing.Color.Transparent;
            this.panelActions.Controls.Add(this.btnExportExcel);
            this.panelActions.Controls.Add(this.btnExportCsv);
            this.panelActions.Controls.Add(this.btnExportPDF);
            this.panelActions.Controls.Add(this.btnPrint);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelActions.Location = new System.Drawing.Point(15, 205);
            this.panelActions.Name = "panelActions";
            this.panelActions.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelActions.Size = new System.Drawing.Size(1170, 46);
            this.panelActions.TabIndex = 2;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(3, 11);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(125, 36);
            this.btnExportExcel.TabIndex = 0;
            this.btnExportExcel.Text = "تصدير Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(184)))), ((int)(((byte)(166)))));
            this.btnExportCsv.FlatAppearance.BorderSize = 0;
            this.btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCsv.ForeColor = System.Drawing.Color.White;
            this.btnExportCsv.Location = new System.Drawing.Point(134, 11);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(125, 36);
            this.btnExportCsv.TabIndex = 1;
            this.btnExportCsv.Text = "تصدير CSV";
            this.btnExportCsv.UseVisualStyleBackColor = false;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnExportPDF.FlatAppearance.BorderSize = 0;
            this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportPDF.Location = new System.Drawing.Point(265, 11);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(125, 36);
            this.btnExportPDF.TabIndex = 2;
            this.btnExportPDF.Text = "تصدير PDF";
            this.btnExportPDF.UseVisualStyleBackColor = false;
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(396, 11);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(125, 36);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "طباعة";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dataGridViewReport
            // 
            this.dataGridViewReport.AllowUserToAddRows = false;
            this.dataGridViewReport.AllowUserToDeleteRows = false;
            this.dataGridViewReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewReport.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewReport.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewReport.ColumnHeadersHeight = 42;
            this.dataGridViewReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewReport.EnableHeadersVisualStyles = false;
            this.dataGridViewReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewReport.Location = new System.Drawing.Point(15, 257);
            this.dataGridViewReport.MultiSelect = false;
            this.dataGridViewReport.Name = "dataGridViewReport";
            this.dataGridViewReport.ReadOnly = true;
            this.dataGridViewReport.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewReport.RowHeadersVisible = false;
            this.dataGridViewReport.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.dataGridViewReport.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewReport.RowTemplate.Height = 34;
            this.dataGridViewReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewReport.Size = new System.Drawing.Size(1170, 388);
            this.dataGridViewReport.TabIndex = 3;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(15, 651);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1170, 26);
            this.panelBottom.TabIndex = 4;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(1170, 26);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد السجلات: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument_PrintPage);
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Document = this.printDocument;
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // ReportCenterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "ReportCenterForm";
            this.Text = "مركز التقارير";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1200, 750);
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.groupBoxFilters.ResumeLayout(false);
            this.tableLayoutFilters.ResumeLayout(false);
            this.tableLayoutFilters.PerformLayout();
            this.panelSummary.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReport)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel mainContainer;

        private System.Windows.Forms.GroupBox groupBoxFilters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFilters;

        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.Label lblAcademicYear;
        private System.Windows.Forms.TextBox txtAcademicYear;
        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox cmbClass;
        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.ComboBox cmbSection;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblSummary;

        private System.Windows.Forms.FlowLayoutPanel panelActions;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Button btnPrint;

        private System.Windows.Forms.DataGridView dataGridViewReport;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;

        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
    }
}
