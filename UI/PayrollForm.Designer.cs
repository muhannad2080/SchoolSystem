namespace SchoolSystem.UI
{
    partial class PayrollForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.panelFields = new System.Windows.Forms.Panel();
            this.groupBoxContract = new System.Windows.Forms.GroupBox();
            this.tableLayoutFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblTeacher = new System.Windows.Forms.Label();
            this.cmbTeacher = new System.Windows.Forms.ComboBox();
            this.lblContractNumber = new System.Windows.Forms.Label();
            this.txtContractNumber = new System.Windows.Forms.TextBox();
            this.lblContractType = new System.Windows.Forms.Label();
            this.cmbContractType = new System.Windows.Forms.ComboBox();
            this.lblContractStatus = new System.Windows.Forms.Label();
            this.cmbContractStatus = new System.Windows.Forms.ComboBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblBasicSalary = new System.Windows.Forms.Label();
            this.txtBasicSalary = new System.Windows.Forms.TextBox();
            this.lblHousing = new System.Windows.Forms.Label();
            this.txtHousing = new System.Windows.Forms.TextBox();
            this.lblTransport = new System.Windows.Forms.Label();
            this.txtTransport = new System.Windows.Forms.TextBox();
            this.lblOther = new System.Windows.Forms.Label();
            this.txtOther = new System.Windows.Forms.TextBox();
            this.lblDeductions = new System.Windows.Forms.Label();
            this.txtDeductions = new System.Windows.Forms.TextBox();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.lblNetSalary = new System.Windows.Forms.Label();
            this.txtNetSalary = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dataGridViewContracts = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.panelFields.SuspendLayout();
            this.groupBoxContract.SuspendLayout();
            this.tableLayoutFields.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContracts)).BeginInit();
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
            this.panelTitle.Size = new System.Drawing.Size(1100, 58);
            this.panelTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1100, 58);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "العقود والرواتب";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainContainer
            // 
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.panelFields, 0, 0);
            this.mainContainer.Controls.Add(this.panelButtons, 0, 1);
            this.mainContainer.Controls.Add(this.panelSearch, 0, 2);
            this.mainContainer.Controls.Add(this.dataGridViewContracts, 0, 3);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 4);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 58);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.RowCount = 5;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 265F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainContainer.Size = new System.Drawing.Size(1100, 642);
            this.mainContainer.TabIndex = 1;
            // 
            // panelFields
            // 
            this.panelFields.BackColor = System.Drawing.Color.Transparent;
            this.panelFields.Controls.Add(this.groupBoxContract);
            this.panelFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFields.Location = new System.Drawing.Point(15, 13);
            this.panelFields.Name = "panelFields";
            this.panelFields.Size = new System.Drawing.Size(1070, 259);
            this.panelFields.TabIndex = 0;
            // 
            // groupBoxContract
            // 
            this.groupBoxContract.BackColor = System.Drawing.Color.White;
            this.groupBoxContract.Controls.Add(this.tableLayoutFields);
            this.groupBoxContract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxContract.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxContract.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxContract.Location = new System.Drawing.Point(0, 0);
            this.groupBoxContract.Name = "groupBoxContract";
            this.groupBoxContract.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxContract.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxContract.Size = new System.Drawing.Size(1070, 259);
            this.groupBoxContract.TabIndex = 0;
            this.groupBoxContract.TabStop = false;
            this.groupBoxContract.Text = "بيانات العقد والراتب";
            // 
            // tableLayoutFields
            // 
            this.tableLayoutFields.ColumnCount = 6;
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFields.Controls.Add(this.lblTeacher, 0, 0);
            this.tableLayoutFields.Controls.Add(this.cmbTeacher, 1, 0);
            this.tableLayoutFields.Controls.Add(this.lblContractNumber, 2, 0);
            this.tableLayoutFields.Controls.Add(this.txtContractNumber, 3, 0);
            this.tableLayoutFields.Controls.Add(this.lblContractType, 4, 0);
            this.tableLayoutFields.Controls.Add(this.cmbContractType, 5, 0);
            this.tableLayoutFields.Controls.Add(this.lblContractStatus, 0, 1);
            this.tableLayoutFields.Controls.Add(this.cmbContractStatus, 1, 1);
            this.tableLayoutFields.Controls.Add(this.lblStartDate, 2, 1);
            this.tableLayoutFields.Controls.Add(this.dtpStartDate, 3, 1);
            this.tableLayoutFields.Controls.Add(this.lblEndDate, 4, 1);
            this.tableLayoutFields.Controls.Add(this.dtpEndDate, 5, 1);
            this.tableLayoutFields.Controls.Add(this.lblBasicSalary, 0, 2);
            this.tableLayoutFields.Controls.Add(this.txtBasicSalary, 1, 2);
            this.tableLayoutFields.Controls.Add(this.lblHousing, 2, 2);
            this.tableLayoutFields.Controls.Add(this.txtHousing, 3, 2);
            this.tableLayoutFields.Controls.Add(this.lblTransport, 4, 2);
            this.tableLayoutFields.Controls.Add(this.txtTransport, 5, 2);
            this.tableLayoutFields.Controls.Add(this.lblOther, 0, 3);
            this.tableLayoutFields.Controls.Add(this.txtOther, 1, 3);
            this.tableLayoutFields.Controls.Add(this.lblDeductions, 2, 3);
            this.tableLayoutFields.Controls.Add(this.txtDeductions, 3, 3);
            this.tableLayoutFields.Controls.Add(this.lblPaymentMethod, 4, 3);
            this.tableLayoutFields.Controls.Add(this.cmbPaymentMethod, 5, 3);
            this.tableLayoutFields.Controls.Add(this.lblTotal, 0, 4);
            this.tableLayoutFields.Controls.Add(this.txtTotal, 1, 4);
            this.tableLayoutFields.Controls.Add(this.lblNetSalary, 2, 4);
            this.tableLayoutFields.Controls.Add(this.txtNetSalary, 3, 4);
            this.tableLayoutFields.Controls.Add(this.lblNotes, 4, 4);
            this.tableLayoutFields.Controls.Add(this.txtNotes, 5, 4);
            this.tableLayoutFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFields.Location = new System.Drawing.Point(12, 29);
            this.tableLayoutFields.Name = "tableLayoutFields";
            this.tableLayoutFields.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.RowCount = 5;
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.Size = new System.Drawing.Size(1046, 220);
            this.tableLayoutFields.TabIndex = 0;
            // 
            // lblTeacher
            // 
            this.lblTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTeacher.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTeacher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblTeacher.Location = new System.Drawing.Point(925, 4);
            this.lblTeacher.Name = "lblTeacher";
            this.lblTeacher.Size = new System.Drawing.Size(114, 42);
            this.lblTeacher.TabIndex = 0;
            this.lblTeacher.Text = "المعلم:";
            this.lblTeacher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbTeacher
            // 
            this.cmbTeacher.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTeacher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeacher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTeacher.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.cmbTeacher.FormattingEnabled = true;
            this.cmbTeacher.Location = new System.Drawing.Point(700, 8);
            this.cmbTeacher.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTeacher.Name = "cmbTeacher";
            this.cmbTeacher.Size = new System.Drawing.Size(218, 27);
            this.cmbTeacher.TabIndex = 0;
            // 
            // lblContractNumber
            // 
            this.lblContractNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContractNumber.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblContractNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblContractNumber.Location = new System.Drawing.Point(579, 4);
            this.lblContractNumber.Name = "lblContractNumber";
            this.lblContractNumber.Size = new System.Drawing.Size(114, 42);
            this.lblContractNumber.TabIndex = 1;
            this.lblContractNumber.Text = "رقم العقد:";
            this.lblContractNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtContractNumber
            // 
            this.txtContractNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtContractNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContractNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContractNumber.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtContractNumber.Location = new System.Drawing.Point(354, 8);
            this.txtContractNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtContractNumber.Name = "txtContractNumber";
            this.txtContractNumber.Size = new System.Drawing.Size(218, 27);
            this.txtContractNumber.TabIndex = 1;
            // 
            // lblContractType
            // 
            this.lblContractType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContractType.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblContractType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblContractType.Location = new System.Drawing.Point(233, 4);
            this.lblContractType.Name = "lblContractType";
            this.lblContractType.Size = new System.Drawing.Size(114, 42);
            this.lblContractType.TabIndex = 2;
            this.lblContractType.Text = "نوع العقد:";
            this.lblContractType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbContractType
            // 
            this.cmbContractType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbContractType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbContractType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContractType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbContractType.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.cmbContractType.FormattingEnabled = true;
            this.cmbContractType.Items.AddRange(new object[] {
            "دوام كامل",
            "دوام جزئي",
            "بالساعة",
            "مؤقت",
            "تعاقد سنوي"});
            this.cmbContractType.Location = new System.Drawing.Point(8, 8);
            this.cmbContractType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbContractType.Name = "cmbContractType";
            this.cmbContractType.Size = new System.Drawing.Size(218, 27);
            this.cmbContractType.TabIndex = 2;
            // 
            // lblContractStatus
            // 
            this.lblContractStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContractStatus.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblContractStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblContractStatus.Location = new System.Drawing.Point(925, 46);
            this.lblContractStatus.Name = "lblContractStatus";
            this.lblContractStatus.Size = new System.Drawing.Size(114, 42);
            this.lblContractStatus.TabIndex = 3;
            this.lblContractStatus.Text = "حالة العقد:";
            this.lblContractStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbContractStatus
            // 
            this.cmbContractStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbContractStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbContractStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContractStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbContractStatus.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.cmbContractStatus.FormattingEnabled = true;
            this.cmbContractStatus.Items.AddRange(new object[] {
            "ساري",
            "منتهي",
            "موقوف",
            "ملغي"});
            this.cmbContractStatus.Location = new System.Drawing.Point(700, 50);
            this.cmbContractStatus.Margin = new System.Windows.Forms.Padding(4);
            this.cmbContractStatus.Name = "cmbContractStatus";
            this.cmbContractStatus.Size = new System.Drawing.Size(218, 27);
            this.cmbContractStatus.TabIndex = 3;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStartDate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStartDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblStartDate.Location = new System.Drawing.Point(579, 46);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(114, 42);
            this.lblStartDate.TabIndex = 4;
            this.lblStartDate.Text = "تاريخ البداية:";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "dd/MM/yyyy";
            this.dtpStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpStartDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(354, 50);
            this.dtpStartDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(218, 27);
            this.dtpStartDate.TabIndex = 4;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEndDate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEndDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblEndDate.Location = new System.Drawing.Point(233, 46);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(114, 42);
            this.lblEndDate.TabIndex = 5;
            this.lblEndDate.Text = "تاريخ النهاية:";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Checked = false;
            this.dtpEndDate.CustomFormat = "dd/MM/yyyy";
            this.dtpEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpEndDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(8, 50);
            this.dtpEndDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.Size = new System.Drawing.Size(218, 27);
            this.dtpEndDate.TabIndex = 5;
            // 
            // lblBasicSalary
            // 
            this.lblBasicSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBasicSalary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBasicSalary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblBasicSalary.Location = new System.Drawing.Point(925, 88);
            this.lblBasicSalary.Name = "lblBasicSalary";
            this.lblBasicSalary.Size = new System.Drawing.Size(114, 42);
            this.lblBasicSalary.TabIndex = 6;
            this.lblBasicSalary.Text = "الراتب الأساسي:";
            this.lblBasicSalary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBasicSalary
            // 
            this.txtBasicSalary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtBasicSalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBasicSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBasicSalary.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtBasicSalary.Location = new System.Drawing.Point(700, 92);
            this.txtBasicSalary.Margin = new System.Windows.Forms.Padding(4);
            this.txtBasicSalary.Name = "txtBasicSalary";
            this.txtBasicSalary.Size = new System.Drawing.Size(218, 27);
            this.txtBasicSalary.TabIndex = 6;
            this.txtBasicSalary.Text = "0";
            this.txtBasicSalary.TextChanged += new System.EventHandler(this.SalaryField_TextChanged);
            // 
            // lblHousing
            // 
            this.lblHousing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHousing.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblHousing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblHousing.Location = new System.Drawing.Point(579, 88);
            this.lblHousing.Name = "lblHousing";
            this.lblHousing.Size = new System.Drawing.Size(114, 42);
            this.lblHousing.TabIndex = 7;
            this.lblHousing.Text = "بدل السكن:";
            this.lblHousing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHousing
            // 
            this.txtHousing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtHousing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHousing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHousing.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtHousing.Location = new System.Drawing.Point(354, 92);
            this.txtHousing.Margin = new System.Windows.Forms.Padding(4);
            this.txtHousing.Name = "txtHousing";
            this.txtHousing.Size = new System.Drawing.Size(218, 27);
            this.txtHousing.TabIndex = 7;
            this.txtHousing.Text = "0";
            this.txtHousing.TextChanged += new System.EventHandler(this.SalaryField_TextChanged);
            // 
            // lblTransport
            // 
            this.lblTransport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransport.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTransport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblTransport.Location = new System.Drawing.Point(233, 88);
            this.lblTransport.Name = "lblTransport";
            this.lblTransport.Size = new System.Drawing.Size(114, 42);
            this.lblTransport.TabIndex = 8;
            this.lblTransport.Text = "بدل النقل:";
            this.lblTransport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTransport
            // 
            this.txtTransport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtTransport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTransport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTransport.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtTransport.Location = new System.Drawing.Point(8, 92);
            this.txtTransport.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransport.Name = "txtTransport";
            this.txtTransport.Size = new System.Drawing.Size(218, 27);
            this.txtTransport.TabIndex = 8;
            this.txtTransport.Text = "0";
            this.txtTransport.TextChanged += new System.EventHandler(this.SalaryField_TextChanged);
            // 
            // lblOther
            // 
            this.lblOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOther.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblOther.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblOther.Location = new System.Drawing.Point(925, 130);
            this.lblOther.Name = "lblOther";
            this.lblOther.Size = new System.Drawing.Size(114, 42);
            this.lblOther.TabIndex = 9;
            this.lblOther.Text = "بدلات أخرى:";
            this.lblOther.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOther
            // 
            this.txtOther.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtOther.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOther.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtOther.Location = new System.Drawing.Point(700, 134);
            this.txtOther.Margin = new System.Windows.Forms.Padding(4);
            this.txtOther.Name = "txtOther";
            this.txtOther.Size = new System.Drawing.Size(218, 27);
            this.txtOther.TabIndex = 9;
            this.txtOther.Text = "0";
            this.txtOther.TextChanged += new System.EventHandler(this.SalaryField_TextChanged);
            // 
            // lblDeductions
            // 
            this.lblDeductions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDeductions.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDeductions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.lblDeductions.Location = new System.Drawing.Point(579, 130);
            this.lblDeductions.Name = "lblDeductions";
            this.lblDeductions.Size = new System.Drawing.Size(114, 42);
            this.lblDeductions.TabIndex = 10;
            this.lblDeductions.Text = "الخصومات:";
            this.lblDeductions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDeductions
            // 
            this.txtDeductions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.txtDeductions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeductions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDeductions.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtDeductions.Location = new System.Drawing.Point(354, 134);
            this.txtDeductions.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeductions.Name = "txtDeductions";
            this.txtDeductions.Size = new System.Drawing.Size(218, 27);
            this.txtDeductions.TabIndex = 10;
            this.txtDeductions.Text = "0";
            this.txtDeductions.TextChanged += new System.EventHandler(this.SalaryField_TextChanged);
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPaymentMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblPaymentMethod.Location = new System.Drawing.Point(233, 130);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(114, 42);
            this.lblPaymentMethod.TabIndex = 11;
            this.lblPaymentMethod.Text = "طريقة الصرف:";
            this.lblPaymentMethod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPaymentMethod.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.cmbPaymentMethod.FormattingEnabled = true;
            this.cmbPaymentMethod.Items.AddRange(new object[] {
            "نقداً",
            "حوالة",
            "بنك",
            "محفظة إلكترونية"});
            this.cmbPaymentMethod.Location = new System.Drawing.Point(8, 134);
            this.cmbPaymentMethod.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(218, 27);
            this.cmbPaymentMethod.TabIndex = 11;
            // 
            // lblTotal
            // 
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblTotal.Location = new System.Drawing.Point(925, 172);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(114, 44);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "إجمالي المستحقات:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            this.txtTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTotal.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.txtTotal.Location = new System.Drawing.Point(700, 176);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(4);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(218, 27);
            this.txtTotal.TabIndex = 12;
            this.txtTotal.TabStop = false;
            this.txtTotal.Text = "0";
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNetSalary
            // 
            this.lblNetSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetSalary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNetSalary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblNetSalary.Location = new System.Drawing.Point(579, 172);
            this.lblNetSalary.Name = "lblNetSalary";
            this.lblNetSalary.Size = new System.Drawing.Size(114, 44);
            this.lblNetSalary.TabIndex = 13;
            this.lblNetSalary.Text = "صافي الراتب:";
            this.lblNetSalary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNetSalary
            // 
            this.txtNetSalary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.txtNetSalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNetSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNetSalary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtNetSalary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.txtNetSalary.Location = new System.Drawing.Point(354, 176);
            this.txtNetSalary.Margin = new System.Windows.Forms.Padding(4);
            this.txtNetSalary.Name = "txtNetSalary";
            this.txtNetSalary.ReadOnly = true;
            this.txtNetSalary.Size = new System.Drawing.Size(218, 27);
            this.txtNetSalary.TabIndex = 13;
            this.txtNetSalary.TabStop = false;
            this.txtNetSalary.Text = "0";
            this.txtNetSalary.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNotes
            // 
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblNotes.Location = new System.Drawing.Point(233, 172);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(114, 44);
            this.lblNotes.TabIndex = 14;
            this.lblNotes.Text = "ملاحظات:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtNotes.Location = new System.Drawing.Point(8, 176);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(218, 27);
            this.txtNotes.TabIndex = 14;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnUpdate);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnClear);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(15, 278);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelButtons.Size = new System.Drawing.Size(1070, 46);
            this.panelButtons.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(940, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 36);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "إضافة";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(814, 6);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 36);
            this.btnUpdate.TabIndex = 16;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(688, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 36);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(562, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 36);
            this.btnClear.TabIndex = 18;
            this.btnClear.Text = "تفريغ";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearch.Location = new System.Drawing.Point(15, 330);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.panelSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelSearch.Size = new System.Drawing.Size(1070, 42);
            this.panelSearch.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(10, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(930, 28);
            this.txtSearch.TabIndex = 19;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblSearch.Location = new System.Drawing.Point(940, 7);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(120, 28);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث سريع:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewContracts
            // 
            this.dataGridViewContracts.AllowUserToAddRows = false;
            this.dataGridViewContracts.AllowUserToDeleteRows = false;
            this.dataGridViewContracts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewContracts.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewContracts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewContracts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewContracts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewContracts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewContracts.ColumnHeadersHeight = 42;
            this.dataGridViewContracts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewContracts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewContracts.EnableHeadersVisualStyles = false;
            this.dataGridViewContracts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewContracts.Location = new System.Drawing.Point(15, 378);
            this.dataGridViewContracts.MultiSelect = false;
            this.dataGridViewContracts.Name = "dataGridViewContracts";
            this.dataGridViewContracts.ReadOnly = true;
            this.dataGridViewContracts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewContracts.RowHeadersVisible = false;
            this.dataGridViewContracts.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.dataGridViewContracts.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewContracts.RowTemplate.Height = 34;
            this.dataGridViewContracts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewContracts.Size = new System.Drawing.Size(1070, 219);
            this.dataGridViewContracts.TabIndex = 3;
            this.dataGridViewContracts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewContracts_CellClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(15, 603);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1070, 26);
            this.panelBottom.TabIndex = 4;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(1070, 26);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد العقود: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PayrollForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "PayrollForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1100, 700);
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.panelFields.ResumeLayout(false);
            this.groupBoxContract.ResumeLayout(false);
            this.tableLayoutFields.ResumeLayout(false);
            this.tableLayoutFields.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContracts)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel mainContainer;

        private System.Windows.Forms.Panel panelFields;
        private System.Windows.Forms.GroupBox groupBoxContract;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFields;

        private System.Windows.Forms.Label lblTeacher;
        private System.Windows.Forms.ComboBox cmbTeacher;

        private System.Windows.Forms.Label lblContractNumber;
        private System.Windows.Forms.TextBox txtContractNumber;

        private System.Windows.Forms.Label lblContractType;
        private System.Windows.Forms.ComboBox cmbContractType;

        private System.Windows.Forms.Label lblContractStatus;
        private System.Windows.Forms.ComboBox cmbContractStatus;

        private System.Windows.Forms.Label lblBasicSalary;
        private System.Windows.Forms.TextBox txtBasicSalary;

        private System.Windows.Forms.Label lblHousing;
        private System.Windows.Forms.TextBox txtHousing;

        private System.Windows.Forms.Label lblTransport;
        private System.Windows.Forms.TextBox txtTransport;

        private System.Windows.Forms.Label lblOther;
        private System.Windows.Forms.TextBox txtOther;

        private System.Windows.Forms.Label lblDeductions;
        private System.Windows.Forms.TextBox txtDeductions;

        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtTotal;

        private System.Windows.Forms.Label lblNetSalary;
        private System.Windows.Forms.TextBox txtNetSalary;

        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;

        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;

        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;

        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;

        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.DataGridView dataGridViewContracts;
        private System.Windows.Forms.Panel panelBottom;
    }
}
