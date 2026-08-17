namespace SchoolSystem.UI
{
    partial class GradeEntryForm
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
            this.panelTitle = new Krypton.Toolkit.KryptonPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxFilters = new System.Windows.Forms.GroupBox();
            this.tableLayoutFilters = new System.Windows.Forms.TableLayoutPanel();
            this.lblAcademicYear = new System.Windows.Forms.Label();
            this.txtAcademicYear = new System.Windows.Forms.TextBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblSection = new System.Windows.Forms.Label();
            this.cmbSection = new System.Windows.Forms.ComboBox();
            this.lblSubject = new System.Windows.Forms.Label();
            this.cmbSubject = new System.Windows.Forms.ComboBox();
            this.lblTerm = new System.Windows.Forms.Label();
            this.cmbTerm = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.panelHint = new System.Windows.Forms.Panel();
            this.lblHint = new System.Windows.Forms.Label();
            this.dataGridViewGrades = new System.Windows.Forms.DataGridView();
            this.panelActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.btnDeleteGrade = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnIncomplete = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.groupBoxFilters.SuspendLayout();
            this.tableLayoutFilters.SuspendLayout();
            this.panelHint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrades)).BeginInit();
            this.panelActions.SuspendLayout();
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
            this.panelTitle.Size = new System.Drawing.Size(1180, 60);
            this.panelTitle.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1180, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "إدارة درجات الطلاب حسب مواد الصف";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // mainContainer
            //
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainContainer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.groupBoxFilters, 0, 0);
            this.mainContainer.Controls.Add(this.panelHint, 0, 1);
            this.mainContainer.Controls.Add(this.dataGridViewGrades, 0, 2);
            this.mainContainer.Controls.Add(this.panelActions, 0, 3);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 4);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 60);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.RowCount = 5;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainContainer.Size = new System.Drawing.Size(1180, 700);
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
            this.groupBoxFilters.Size = new System.Drawing.Size(1150, 139);
            this.groupBoxFilters.TabIndex = 0;
            this.groupBoxFilters.TabStop = false;
            this.groupBoxFilters.Text = "خيارات إدخال الدرجات";
            //
            // tableLayoutFilters
            //
            this.tableLayoutFilters.AutoSize = false;
            this.tableLayoutFilters.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutFilters.ColumnCount = 6;
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFilters.Controls.Add(this.lblAcademicYear, 0, 0);
            this.tableLayoutFilters.Controls.Add(this.txtAcademicYear, 1, 0);
            this.tableLayoutFilters.Controls.Add(this.lblClass, 2, 0);
            this.tableLayoutFilters.Controls.Add(this.cmbClass, 3, 0);
            this.tableLayoutFilters.Controls.Add(this.lblSection, 4, 0);
            this.tableLayoutFilters.Controls.Add(this.cmbSection, 5, 0);
            this.tableLayoutFilters.Controls.Add(this.lblSubject, 0, 1);
            this.tableLayoutFilters.Controls.Add(this.cmbSubject, 1, 1);
            this.tableLayoutFilters.Controls.Add(this.lblTerm, 2, 1);
            this.tableLayoutFilters.Controls.Add(this.cmbTerm, 3, 1);
            this.tableLayoutFilters.Controls.Add(this.lblSearch, 4, 1);
            this.tableLayoutFilters.Controls.Add(this.txtSearch, 5, 1);
            this.tableLayoutFilters.Controls.Add(this.btnLoad, 0, 2);
            this.tableLayoutFilters.SetColumnSpan(this.btnLoad, 6);
            this.tableLayoutFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFilters.Location = new System.Drawing.Point(12, 29);
            this.tableLayoutFilters.Name = "tableLayoutFilters";
            this.tableLayoutFilters.RowCount = 3;
            this.tableLayoutFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutFilters.Size = new System.Drawing.Size(1126, 144);
            this.tableLayoutFilters.TabIndex = 0;
            //
            // lblAcademicYear
            //
            this.lblAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicYear.Location = new System.Drawing.Point(1004, 0);
            this.lblAcademicYear.Name = "lblAcademicYear";
            this.lblAcademicYear.Size = new System.Drawing.Size(119, 33);
            this.lblAcademicYear.TabIndex = 0;
            this.lblAcademicYear.Text = "العام الدراسي:";
            this.lblAcademicYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // txtAcademicYear
            //
            this.txtAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAcademicYear.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtAcademicYear.Location = new System.Drawing.Point(754, 3);
            this.txtAcademicYear.Name = "txtAcademicYear";
            this.txtAcademicYear.Size = new System.Drawing.Size(244, 28);
            this.txtAcademicYear.TabIndex = 1;
            //
            // lblClass
            //
            this.lblClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClass.Location = new System.Drawing.Point(629, 0);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(119, 33);
            this.lblClass.TabIndex = 2;
            this.lblClass.Text = "الصف:";
            this.lblClass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // cmbClass
            //
            this.cmbClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Location = new System.Drawing.Point(379, 3);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(244, 29);
            this.cmbClass.TabIndex = 3;
            //
            // lblSection
            //
            this.lblSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSection.Location = new System.Drawing.Point(254, 0);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(119, 33);
            this.lblSection.TabIndex = 4;
            this.lblSection.Text = "الشعبة:";
            this.lblSection.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // cmbSection
            //
            this.cmbSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSection.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbSection.FormattingEnabled = true;
            this.cmbSection.Location = new System.Drawing.Point(3, 3);
            this.cmbSection.Name = "cmbSection";
            this.cmbSection.Size = new System.Drawing.Size(245, 29);
            this.cmbSection.TabIndex = 5;
            //
            // lblSubject
            //
            this.lblSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubject.Location = new System.Drawing.Point(1004, 33);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(119, 33);
            this.lblSubject.TabIndex = 6;
            this.lblSubject.Text = "المادة:";
            this.lblSubject.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSubject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // cmbSubject
            //
            this.cmbSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubject.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbSubject.FormattingEnabled = true;
            this.cmbSubject.Location = new System.Drawing.Point(754, 36);
            this.cmbSubject.Name = "cmbSubject";
            this.cmbSubject.Size = new System.Drawing.Size(244, 29);
            this.cmbSubject.TabIndex = 7;
            //
            // lblTerm
            //
            this.lblTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTerm.Location = new System.Drawing.Point(629, 33);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(119, 33);
            this.lblTerm.TabIndex = 8;
            this.lblTerm.Text = "الفصل:";
            this.lblTerm.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTerm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // cmbTerm
            //
            this.cmbTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTerm.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbTerm.FormattingEnabled = true;
            this.cmbTerm.Location = new System.Drawing.Point(379, 36);
            this.cmbTerm.Name = "cmbTerm";
            this.cmbTerm.Size = new System.Drawing.Size(244, 29);
            this.cmbTerm.TabIndex = 9;
            //
            // lblSearch
            //
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSearch.Location = new System.Drawing.Point(254, 33);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(119, 33);
            this.lblSearch.TabIndex = 10;
            this.lblSearch.Text = "بحث:";
            this.lblSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // txtSearch
            //
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(3, 36);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(245, 28);
            this.txtSearch.TabIndex = 11;
            //
            // btnLoad
            //
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoad.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(3, 69);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(245, 28);
            this.btnLoad.TabIndex = 12;
            this.btnLoad.Text = "تحميل طلاب الصف والمادة";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            //
            // panelHint
            //
            this.panelHint.BackColor = System.Drawing.Color.White;
            this.panelHint.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelHint.Controls.Add(this.lblHint);
            this.panelHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHint.Location = new System.Drawing.Point(15, 158);
            this.panelHint.Name = "panelHint";
            this.panelHint.Size = new System.Drawing.Size(1150, 36);
            this.panelHint.TabIndex = 1;
            //
            // lblHint
            //
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHint.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblHint.Location = new System.Drawing.Point(0, 0);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(1150, 36);
            this.lblHint.TabIndex = 0;
            this.lblHint.Text = "اختر الصف أولًا لتظهر المواد الخاصة به فقط، ثم اختر المادة والشعبة والفصل، وبعدها" +
    " أدخل الدرجات مباشرة في الجدول.";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // dataGridViewGrades
            //
            this.dataGridViewGrades.AllowUserToAddRows = false;
            this.dataGridViewGrades.AllowUserToDeleteRows = false;
            this.dataGridViewGrades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewGrades.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewGrades.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewGrades.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewGrades.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewGrades.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewGrades.ColumnHeadersHeight = 42;
            this.dataGridViewGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewGrades.EnableHeadersVisualStyles = false;
            this.dataGridViewGrades.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewGrades.Location = new System.Drawing.Point(15, 200);
            this.dataGridViewGrades.MultiSelect = false;
            this.dataGridViewGrades.Name = "dataGridViewGrades";
            this.dataGridViewGrades.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewGrades.RowHeadersVisible = false;
            this.dataGridViewGrades.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.dataGridViewGrades.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewGrades.RowTemplate.Height = 34;
            this.dataGridViewGrades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGrades.Size = new System.Drawing.Size(1150, 395);
            this.dataGridViewGrades.TabIndex = 2;
            this.dataGridViewGrades.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGrades_CellClick);
            this.dataGridViewGrades.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGrades_CellEndEdit);
            //
            // panelActions
            //
            this.panelActions.BackColor = System.Drawing.Color.Transparent;
            this.panelActions.Controls.Add(this.btnSaveAll);
            this.panelActions.Controls.Add(this.btnDeleteGrade);
            this.panelActions.Controls.Add(this.btnClear);
            this.panelActions.Controls.Add(this.btnRefresh);
            this.panelActions.Controls.Add(this.btnIncomplete);
            this.panelActions.Controls.Add(this.btnExportExcel);
            this.panelActions.Controls.Add(this.btnExportPdf);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelActions.Location = new System.Drawing.Point(15, 601);
            this.panelActions.Name = "panelActions";
            this.panelActions.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelActions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelActions.Size = new System.Drawing.Size(1150, 54);
            this.panelActions.TabIndex = 3;
            //
            // btnSaveAll
            //
            this.btnSaveAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnSaveAll.FlatAppearance.BorderSize = 0;
            this.btnSaveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveAll.ForeColor = System.Drawing.Color.White;
            this.btnSaveAll.Location = new System.Drawing.Point(3, 13);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(130, 36);
            this.btnSaveAll.TabIndex = 0;
            this.btnSaveAll.Text = "حفظ الدرجات";
            this.btnSaveAll.UseVisualStyleBackColor = false;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            //
            // btnDeleteGrade
            //
            this.btnDeleteGrade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDeleteGrade.FlatAppearance.BorderSize = 0;
            this.btnDeleteGrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteGrade.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteGrade.ForeColor = System.Drawing.Color.White;
            this.btnDeleteGrade.Location = new System.Drawing.Point(139, 13);
            this.btnDeleteGrade.Name = "btnDeleteGrade";
            this.btnDeleteGrade.Size = new System.Drawing.Size(120, 36);
            this.btnDeleteGrade.TabIndex = 1;
            this.btnDeleteGrade.Text = "حذف درجة";
            this.btnDeleteGrade.UseVisualStyleBackColor = false;
            this.btnDeleteGrade.Click += new System.EventHandler(this.btnDeleteGrade_Click);
            //
            // btnClear
            //
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(265, 13);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(110, 36);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "تفريغ";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(381, 13);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 36);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // btnIncomplete
            //
            this.btnIncomplete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.btnIncomplete.FlatAppearance.BorderSize = 0;
            this.btnIncomplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIncomplete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnIncomplete.ForeColor = System.Drawing.Color.White;
            this.btnIncomplete.Location = new System.Drawing.Point(497, 13);
            this.btnIncomplete.Name = "btnIncomplete";
            this.btnIncomplete.Size = new System.Drawing.Size(145, 36);
            this.btnIncomplete.TabIndex = 4;
            this.btnIncomplete.Text = "غير المكتملة";
            this.btnIncomplete.UseVisualStyleBackColor = false;
            this.btnIncomplete.Click += new System.EventHandler(this.btnIncomplete_Click);
            //
            // btnExportExcel
            //
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(648, 13);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(125, 36);
            this.btnExportExcel.TabIndex = 5;
            this.btnExportExcel.Text = "Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            //
            // btnExportPdf
            //
            this.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btnExportPdf.FlatAppearance.BorderSize = 0;
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPdf.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportPdf.Location = new System.Drawing.Point(779, 13);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(115, 36);
            this.btnExportPdf.TabIndex = 6;
            this.btnExportPdf.Text = "PDF";
            this.btnExportPdf.UseVisualStyleBackColor = false;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            //
            // panelBottom
            //
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(15, 661);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1150, 26);
            this.panelBottom.TabIndex = 4;
            //
            // lblRecordCount
            //
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(1150, 26);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد الطلاب: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // GradeEntryForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "GradeEntryForm";
            this.Text = "إدخال الدرجات";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1180, 760);
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.groupBoxFilters.ResumeLayout(false);
            this.tableLayoutFilters.ResumeLayout(false);
            this.tableLayoutFilters.PerformLayout();
            this.panelHint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrades)).EndInit();
            this.panelActions.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.TableLayoutPanel mainContainer;

        private System.Windows.Forms.GroupBox groupBoxFilters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFilters;

        private System.Windows.Forms.Label lblAcademicYear;
        private System.Windows.Forms.TextBox txtAcademicYear;

        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox cmbClass;

        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.ComboBox cmbSection;

        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.ComboBox cmbSubject;

        private System.Windows.Forms.Label lblTerm;
        private System.Windows.Forms.ComboBox cmbTerm;

        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;

        private System.Windows.Forms.Button btnLoad;

        private System.Windows.Forms.Panel panelHint;
        private System.Windows.Forms.Label lblHint;

        private System.Windows.Forms.DataGridView dataGridViewGrades;

        private System.Windows.Forms.FlowLayoutPanel panelActions;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.Button btnDeleteGrade;
        private System.Windows.Forms.Button btnClear;
                private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnIncomplete;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
