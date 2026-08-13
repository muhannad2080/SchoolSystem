namespace SchoolSystem.UI
{
    partial class TeachersForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTitle = new Krypton.Toolkit.KryptonPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxPersonal = new System.Windows.Forms.GroupBox();
            this.layoutPersonal = new System.Windows.Forms.TableLayoutPanel();
            this.lblEmployeeNumber = new System.Windows.Forms.Label();
            this.txtEmployeeNumber = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.dtpBirthDate = new System.Windows.Forms.DateTimePicker();
            this.lblBirthPlace = new System.Windows.Forms.Label();
            this.txtBirthPlace = new System.Windows.Forms.TextBox();
            this.lblNationality = new System.Windows.Forms.Label();
            this.cmbNationality = new System.Windows.Forms.ComboBox();
            this.lblNationalID = new System.Windows.Forms.Label();
            this.txtNationalID = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblHireDate = new System.Windows.Forms.Label();
            this.dtpHireDate = new System.Windows.Forms.DateTimePicker();
            this.lblQualification = new System.Windows.Forms.Label();
            this.cmbQualification = new System.Windows.Forms.ComboBox();
            this.lblSpecialization = new System.Windows.Forms.Label();
            this.txtSpecialization = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.groupBoxFinancial = new System.Windows.Forms.GroupBox();
            this.layoutFinancial = new System.Windows.Forms.TableLayoutPanel();
            this.lblBasicSalary = new System.Windows.Forms.Label();
            this.nudBasicSalary = new System.Windows.Forms.NumericUpDown();
            this.lblTransportAllowance = new System.Windows.Forms.Label();
            this.nudTransportAllowance = new System.Windows.Forms.NumericUpDown();
            this.lblHousingAllowance = new System.Windows.Forms.Label();
            this.nudHousingAllowance = new System.Windows.Forms.NumericUpDown();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dataGridViewTeachers = new System.Windows.Forms.DataGridView();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.groupBoxPersonal.SuspendLayout();
            this.layoutPersonal.SuspendLayout();
            this.groupBoxFinancial.SuspendLayout();
            this.layoutFinancial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBasicSalary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransportAllowance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHousingAllowance)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1428, 56);
            this.panelTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1428, 56);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "إدارة شؤون المعلمين والموظفين";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainContainer
            // 
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.groupBoxPersonal, 0, 0);
            this.mainContainer.Controls.Add(this.groupBoxFinancial, 0, 1);
            this.mainContainer.Controls.Add(this.panelButtons, 0, 2);
            this.mainContainer.Controls.Add(this.panelSearch, 0, 3);
            this.mainContainer.Controls.Add(this.dataGridViewTeachers, 0, 4);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 56);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 12);
            this.mainContainer.RowCount = 5;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 205F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Size = new System.Drawing.Size(1428, 584);
            this.mainContainer.TabIndex = 1;
            // 
            // groupBoxPersonal
            // 
            this.groupBoxPersonal.BackColor = System.Drawing.Color.White;
            this.groupBoxPersonal.Controls.Add(this.layoutPersonal);
            this.groupBoxPersonal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPersonal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxPersonal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxPersonal.Location = new System.Drawing.Point(15, 13);
            this.groupBoxPersonal.Name = "groupBoxPersonal";
            this.groupBoxPersonal.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxPersonal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxPersonal.Size = new System.Drawing.Size(1398, 199);
            this.groupBoxPersonal.TabIndex = 0;
            this.groupBoxPersonal.TabStop = false;
            this.groupBoxPersonal.Text = "البيانات الشخصية والوظيفية";
            // 
            // layoutPersonal
            // 
            this.layoutPersonal.ColumnCount = 8;
            this.layoutPersonal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.layoutPersonal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutPersonal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.layoutPersonal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutPersonal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.layoutPersonal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutPersonal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.layoutPersonal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutPersonal.Controls.Add(this.lblEmployeeNumber, 0, 0);
            this.layoutPersonal.Controls.Add(this.txtEmployeeNumber, 1, 0);
            this.layoutPersonal.Controls.Add(this.lblFullName, 2, 0);
            this.layoutPersonal.Controls.Add(this.txtFullName, 3, 0);
            this.layoutPersonal.Controls.Add(this.lblGender, 6, 0);
            this.layoutPersonal.Controls.Add(this.cmbGender, 7, 0);
            this.layoutPersonal.Controls.Add(this.lblBirthDate, 0, 1);
            this.layoutPersonal.Controls.Add(this.dtpBirthDate, 1, 1);
            this.layoutPersonal.Controls.Add(this.lblBirthPlace, 2, 1);
            this.layoutPersonal.Controls.Add(this.txtBirthPlace, 3, 1);
            this.layoutPersonal.Controls.Add(this.lblNationality, 4, 1);
            this.layoutPersonal.Controls.Add(this.cmbNationality, 5, 1);
            this.layoutPersonal.Controls.Add(this.lblNationalID, 6, 1);
            this.layoutPersonal.Controls.Add(this.txtNationalID, 7, 1);
            this.layoutPersonal.Controls.Add(this.lblPhone, 0, 2);
            this.layoutPersonal.Controls.Add(this.txtPhone, 1, 2);
            this.layoutPersonal.Controls.Add(this.lblEmail, 2, 2);
            this.layoutPersonal.Controls.Add(this.txtEmail, 3, 2);
            this.layoutPersonal.Controls.Add(this.lblHireDate, 6, 2);
            this.layoutPersonal.Controls.Add(this.dtpHireDate, 7, 2);
            this.layoutPersonal.Controls.Add(this.lblQualification, 0, 3);
            this.layoutPersonal.Controls.Add(this.cmbQualification, 1, 3);
            this.layoutPersonal.Controls.Add(this.lblSpecialization, 2, 3);
            this.layoutPersonal.Controls.Add(this.txtSpecialization, 3, 3);
            this.layoutPersonal.Controls.Add(this.lblAddress, 4, 3);
            this.layoutPersonal.Controls.Add(this.txtAddress, 5, 3);
            this.layoutPersonal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPersonal.Location = new System.Drawing.Point(12, 31);
            this.layoutPersonal.Name = "layoutPersonal";
            this.layoutPersonal.Padding = new System.Windows.Forms.Padding(4);
            this.layoutPersonal.RowCount = 4;
            this.layoutPersonal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutPersonal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutPersonal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutPersonal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutPersonal.Size = new System.Drawing.Size(1374, 158);
            this.layoutPersonal.TabIndex = 0;
            // 
            // lblEmployeeNumber
            // 
            this.lblEmployeeNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEmployeeNumber.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEmployeeNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblEmployeeNumber.Location = new System.Drawing.Point(1268, 4);
            this.lblEmployeeNumber.Name = "lblEmployeeNumber";
            this.lblEmployeeNumber.Size = new System.Drawing.Size(99, 37);
            this.lblEmployeeNumber.TabIndex = 0;
            this.lblEmployeeNumber.Text = "رقم الموظف:";
            this.lblEmployeeNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEmployeeNumber
            // 
            this.txtEmployeeNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtEmployeeNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmployeeNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmployeeNumber.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtEmployeeNumber.Location = new System.Drawing.Point(1025, 8);
            this.txtEmployeeNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmployeeNumber.Name = "txtEmployeeNumber";
            this.txtEmployeeNumber.ReadOnly = true;
            this.txtEmployeeNumber.Size = new System.Drawing.Size(236, 29);
            this.txtEmployeeNumber.TabIndex = 1;
            // 
            // lblFullName
            // 
            this.lblFullName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblFullName.Location = new System.Drawing.Point(929, 4);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(89, 37);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "الاسم الكامل:";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFullName
            // 
            this.txtFullName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layoutPersonal.SetColumnSpan(this.txtFullName, 3);
            this.txtFullName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtFullName.Location = new System.Drawing.Point(347, 8);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(575, 29);
            this.txtFullName.TabIndex = 3;
            // 
            // lblGender
            // 
            this.lblGender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblGender.Location = new System.Drawing.Point(251, 4);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(89, 37);
            this.lblGender.TabIndex = 4;
            this.lblGender.Text = "الجنس:";
            this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbGender
            // 
            this.cmbGender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbGender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbGender.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            "ذكر",
            "أنثى"});
            this.cmbGender.Location = new System.Drawing.Point(8, 8);
            this.cmbGender.Margin = new System.Windows.Forms.Padding(4);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(236, 29);
            this.cmbGender.TabIndex = 5;
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBirthDate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBirthDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblBirthDate.Location = new System.Drawing.Point(1268, 41);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(99, 37);
            this.lblBirthDate.TabIndex = 6;
            this.lblBirthDate.Text = "تاريخ الميلاد:";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpBirthDate
            // 
            this.dtpBirthDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpBirthDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBirthDate.Location = new System.Drawing.Point(1025, 45);
            this.dtpBirthDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.Size = new System.Drawing.Size(236, 29);
            this.dtpBirthDate.TabIndex = 7;
            // 
            // lblBirthPlace
            // 
            this.lblBirthPlace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBirthPlace.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBirthPlace.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblBirthPlace.Location = new System.Drawing.Point(929, 41);
            this.lblBirthPlace.Name = "lblBirthPlace";
            this.lblBirthPlace.Size = new System.Drawing.Size(89, 37);
            this.lblBirthPlace.TabIndex = 8;
            this.lblBirthPlace.Text = "مكان الميلاد:";
            this.lblBirthPlace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBirthPlace
            // 
            this.txtBirthPlace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtBirthPlace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBirthPlace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBirthPlace.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtBirthPlace.Location = new System.Drawing.Point(686, 45);
            this.txtBirthPlace.Margin = new System.Windows.Forms.Padding(4);
            this.txtBirthPlace.Name = "txtBirthPlace";
            this.txtBirthPlace.Size = new System.Drawing.Size(236, 29);
            this.txtBirthPlace.TabIndex = 9;
            // 
            // lblNationality
            // 
            this.lblNationality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNationality.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNationality.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblNationality.Location = new System.Drawing.Point(590, 41);
            this.lblNationality.Name = "lblNationality";
            this.lblNationality.Size = new System.Drawing.Size(89, 37);
            this.lblNationality.TabIndex = 10;
            this.lblNationality.Text = "الجنسية:";
            this.lblNationality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbNationality
            // 
            this.cmbNationality.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbNationality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbNationality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNationality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbNationality.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbNationality.FormattingEnabled = true;
            this.cmbNationality.Location = new System.Drawing.Point(347, 45);
            this.cmbNationality.Margin = new System.Windows.Forms.Padding(4);
            this.cmbNationality.Name = "cmbNationality";
            this.cmbNationality.Size = new System.Drawing.Size(236, 29);
            this.cmbNationality.TabIndex = 11;
            // 
            // lblNationalID
            // 
            this.lblNationalID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNationalID.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNationalID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblNationalID.Location = new System.Drawing.Point(251, 41);
            this.lblNationalID.Name = "lblNationalID";
            this.lblNationalID.Size = new System.Drawing.Size(89, 37);
            this.lblNationalID.TabIndex = 12;
            this.lblNationalID.Text = "الرقم الوطني:";
            this.lblNationalID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNationalID
            // 
            this.txtNationalID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtNationalID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNationalID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNationalID.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNationalID.Location = new System.Drawing.Point(8, 45);
            this.txtNationalID.Margin = new System.Windows.Forms.Padding(4);
            this.txtNationalID.Name = "txtNationalID";
            this.txtNationalID.Size = new System.Drawing.Size(236, 29);
            this.txtNationalID.TabIndex = 13;
            // 
            // lblPhone
            // 
            this.lblPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblPhone.Location = new System.Drawing.Point(1268, 78);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(99, 37);
            this.lblPhone.TabIndex = 14;
            this.lblPhone.Text = "الهاتف:";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPhone.Location = new System.Drawing.Point(1025, 82);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(4);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(236, 29);
            this.txtPhone.TabIndex = 15;
            // 
            // lblEmail
            // 
            this.lblEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblEmail.Location = new System.Drawing.Point(929, 78);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(89, 37);
            this.lblEmail.TabIndex = 16;
            this.lblEmail.Text = "البريد:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layoutPersonal.SetColumnSpan(this.txtEmail, 3);
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtEmail.Location = new System.Drawing.Point(347, 82);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(575, 29);
            this.txtEmail.TabIndex = 17;
            // 
            // lblHireDate
            // 
            this.lblHireDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHireDate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblHireDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblHireDate.Location = new System.Drawing.Point(251, 78);
            this.lblHireDate.Name = "lblHireDate";
            this.lblHireDate.Size = new System.Drawing.Size(89, 37);
            this.lblHireDate.TabIndex = 24;
            this.lblHireDate.Text = "تاريخ التعيين:";
            this.lblHireDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpHireDate
            // 
            this.dtpHireDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpHireDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpHireDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHireDate.Location = new System.Drawing.Point(8, 82);
            this.dtpHireDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpHireDate.Name = "dtpHireDate";
            this.dtpHireDate.Size = new System.Drawing.Size(236, 29);
            this.dtpHireDate.TabIndex = 25;
            // 
            // lblQualification
            // 
            this.lblQualification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQualification.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblQualification.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblQualification.Location = new System.Drawing.Point(1268, 115);
            this.lblQualification.Name = "lblQualification";
            this.lblQualification.Size = new System.Drawing.Size(99, 39);
            this.lblQualification.TabIndex = 20;
            this.lblQualification.Text = "المؤهل:";
            this.lblQualification.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbQualification
            // 
            this.cmbQualification.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbQualification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbQualification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQualification.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbQualification.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbQualification.FormattingEnabled = true;
            this.cmbQualification.Location = new System.Drawing.Point(1025, 119);
            this.cmbQualification.Margin = new System.Windows.Forms.Padding(4);
            this.cmbQualification.Name = "cmbQualification";
            this.cmbQualification.Size = new System.Drawing.Size(236, 29);
            this.cmbQualification.TabIndex = 21;
            // 
            // lblSpecialization
            // 
            this.lblSpecialization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSpecialization.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSpecialization.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblSpecialization.Location = new System.Drawing.Point(929, 115);
            this.lblSpecialization.Name = "lblSpecialization";
            this.lblSpecialization.Size = new System.Drawing.Size(89, 39);
            this.lblSpecialization.TabIndex = 22;
            this.lblSpecialization.Text = "التخصص:";
            this.lblSpecialization.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSpecialization
            // 
            this.txtSpecialization.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtSpecialization.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSpecialization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSpecialization.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSpecialization.Location = new System.Drawing.Point(686, 119);
            this.txtSpecialization.Margin = new System.Windows.Forms.Padding(4);
            this.txtSpecialization.Name = "txtSpecialization";
            this.txtSpecialization.Size = new System.Drawing.Size(236, 29);
            this.txtSpecialization.TabIndex = 23;
            // 
            // lblAddress
            // 
            this.lblAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblAddress.Location = new System.Drawing.Point(590, 115);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(89, 39);
            this.lblAddress.TabIndex = 18;
            this.lblAddress.Text = "العنوان:";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAddress
            // 
            this.txtAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layoutPersonal.SetColumnSpan(this.txtAddress, 3);
            this.txtAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtAddress.Location = new System.Drawing.Point(8, 119);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(575, 29);
            this.txtAddress.TabIndex = 19;
            // 
            // groupBoxFinancial
            // 
            this.groupBoxFinancial.BackColor = System.Drawing.Color.White;
            this.groupBoxFinancial.Controls.Add(this.layoutFinancial);
            this.groupBoxFinancial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFinancial.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxFinancial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxFinancial.Location = new System.Drawing.Point(15, 218);
            this.groupBoxFinancial.Name = "groupBoxFinancial";
            this.groupBoxFinancial.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxFinancial.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxFinancial.Size = new System.Drawing.Size(1398, 99);
            this.groupBoxFinancial.TabIndex = 1;
            this.groupBoxFinancial.TabStop = false;
            this.groupBoxFinancial.Text = "البيانات المالية والإدارية";
            // 
            // layoutFinancial
            // 
            this.layoutFinancial.ColumnCount = 8;
            this.layoutFinancial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.layoutFinancial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutFinancial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.layoutFinancial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutFinancial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.layoutFinancial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutFinancial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.layoutFinancial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.layoutFinancial.Controls.Add(this.lblBasicSalary, 0, 0);
            this.layoutFinancial.Controls.Add(this.nudBasicSalary, 1, 0);
            this.layoutFinancial.Controls.Add(this.lblTransportAllowance, 2, 0);
            this.layoutFinancial.Controls.Add(this.nudTransportAllowance, 3, 0);
            this.layoutFinancial.Controls.Add(this.lblHousingAllowance, 4, 0);
            this.layoutFinancial.Controls.Add(this.nudHousingAllowance, 5, 0);
            this.layoutFinancial.Controls.Add(this.lblStatus, 6, 0);
            this.layoutFinancial.Controls.Add(this.cmbStatus, 7, 0);
            this.layoutFinancial.Controls.Add(this.lblNotes, 0, 1);
            this.layoutFinancial.Controls.Add(this.txtNotes, 1, 1);
            this.layoutFinancial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFinancial.Location = new System.Drawing.Point(12, 31);
            this.layoutFinancial.Name = "layoutFinancial";
            this.layoutFinancial.Padding = new System.Windows.Forms.Padding(4);
            this.layoutFinancial.RowCount = 2;
            this.layoutFinancial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutFinancial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutFinancial.Size = new System.Drawing.Size(1374, 58);
            this.layoutFinancial.TabIndex = 0;
            // 
            // lblBasicSalary
            // 
            this.lblBasicSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBasicSalary.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBasicSalary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblBasicSalary.Location = new System.Drawing.Point(1263, 4);
            this.lblBasicSalary.Name = "lblBasicSalary";
            this.lblBasicSalary.Size = new System.Drawing.Size(104, 25);
            this.lblBasicSalary.TabIndex = 0;
            this.lblBasicSalary.Text = "الراتب الأساسي:";
            this.lblBasicSalary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudBasicSalary
            // 
            this.nudBasicSalary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.nudBasicSalary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudBasicSalary.DecimalPlaces = 2;
            this.nudBasicSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudBasicSalary.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.nudBasicSalary.Location = new System.Drawing.Point(1073, 7);
            this.nudBasicSalary.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudBasicSalary.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudBasicSalary.Name = "nudBasicSalary";
            this.nudBasicSalary.Size = new System.Drawing.Size(183, 29);
            this.nudBasicSalary.TabIndex = 1;
            // 
            // lblTransportAllowance
            // 
            this.lblTransportAllowance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransportAllowance.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTransportAllowance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblTransportAllowance.Location = new System.Drawing.Point(977, 4);
            this.lblTransportAllowance.Name = "lblTransportAllowance";
            this.lblTransportAllowance.Size = new System.Drawing.Size(89, 25);
            this.lblTransportAllowance.TabIndex = 2;
            this.lblTransportAllowance.Text = "بدل النقل:";
            this.lblTransportAllowance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudTransportAllowance
            // 
            this.nudTransportAllowance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.nudTransportAllowance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudTransportAllowance.DecimalPlaces = 2;
            this.nudTransportAllowance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTransportAllowance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.nudTransportAllowance.Location = new System.Drawing.Point(787, 7);
            this.nudTransportAllowance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudTransportAllowance.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudTransportAllowance.Name = "nudTransportAllowance";
            this.nudTransportAllowance.Size = new System.Drawing.Size(183, 29);
            this.nudTransportAllowance.TabIndex = 3;
            // 
            // lblHousingAllowance
            // 
            this.lblHousingAllowance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHousingAllowance.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblHousingAllowance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblHousingAllowance.Location = new System.Drawing.Point(691, 4);
            this.lblHousingAllowance.Name = "lblHousingAllowance";
            this.lblHousingAllowance.Size = new System.Drawing.Size(89, 25);
            this.lblHousingAllowance.TabIndex = 4;
            this.lblHousingAllowance.Text = "بدل السكن:";
            this.lblHousingAllowance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudHousingAllowance
            // 
            this.nudHousingAllowance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.nudHousingAllowance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudHousingAllowance.DecimalPlaces = 2;
            this.nudHousingAllowance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudHousingAllowance.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.nudHousingAllowance.Location = new System.Drawing.Point(501, 7);
            this.nudHousingAllowance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudHousingAllowance.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudHousingAllowance.Name = "nudHousingAllowance";
            this.nudHousingAllowance.Size = new System.Drawing.Size(183, 29);
            this.nudHousingAllowance.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblStatus.Location = new System.Drawing.Point(390, 4);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(104, 25);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "الحالة:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "نشط",
            "إجازة",
            "مستقيل"});
            this.cmbStatus.Location = new System.Drawing.Point(8, 7);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(375, 29);
            this.cmbStatus.TabIndex = 7;
            // 
            // lblNotes
            // 
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblNotes.Location = new System.Drawing.Point(1263, 29);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(104, 25);
            this.lblNotes.TabIndex = 8;
            this.lblNotes.Text = "ملاحظات:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layoutFinancial.SetColumnSpan(this.txtNotes, 7);
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNotes.Location = new System.Drawing.Point(8, 32);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(1248, 29);
            this.txtNotes.TabIndex = 9;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnClear);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnUpdate);
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelButtons.Location = new System.Drawing.Point(15, 323);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panelButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelButtons.Size = new System.Drawing.Size(1398, 46);
            this.panelButtons.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(5, 5);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(125, 36);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(140, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(125, 36);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "مسح";
            this.btnClear.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(275, 5);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(125, 36);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(410, 5);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(125, 36);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(545, 5);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(125, 36);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "إضافة جديد";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearch.Location = new System.Drawing.Point(15, 375);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.panelSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelSearch.Size = new System.Drawing.Size(1398, 42);
            this.panelSearch.TabIndex = 3;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(10, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(1260, 30);
            this.txtSearch.TabIndex = 1;
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblSearch.Location = new System.Drawing.Point(1270, 7);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(118, 28);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث سريع:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewTeachers
            // 
            this.dataGridViewTeachers.AllowUserToAddRows = false;
            this.dataGridViewTeachers.AllowUserToDeleteRows = false;
            this.dataGridViewTeachers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTeachers.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewTeachers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewTeachers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewTeachers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTeachers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTeachers.ColumnHeadersHeight = 42;
            this.dataGridViewTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewTeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTeachers.EnableHeadersVisualStyles = false;
            this.dataGridViewTeachers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewTeachers.Location = new System.Drawing.Point(15, 423);
            this.dataGridViewTeachers.MultiSelect = false;
            this.dataGridViewTeachers.Name = "dataGridViewTeachers";
            this.dataGridViewTeachers.ReadOnly = true;
            this.dataGridViewTeachers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewTeachers.RowHeadersVisible = false;
            this.dataGridViewTeachers.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTeachers.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTeachers.RowTemplate.Height = 34;
            this.dataGridViewTeachers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTeachers.Size = new System.Drawing.Size(1398, 146);
            this.dataGridViewTeachers.TabIndex = 4;
            // 
            // TeachersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "TeachersForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1428, 640);
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.groupBoxPersonal.ResumeLayout(false);
            this.layoutPersonal.ResumeLayout(false);
            this.layoutPersonal.PerformLayout();
            this.groupBoxFinancial.ResumeLayout(false);
            this.layoutFinancial.ResumeLayout(false);
            this.layoutFinancial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBasicSalary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransportAllowance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHousingAllowance)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel mainContainer;
        private System.Windows.Forms.GroupBox groupBoxPersonal;
        private System.Windows.Forms.TableLayoutPanel layoutPersonal;
        private System.Windows.Forms.Label lblEmployeeNumber;
        private System.Windows.Forms.TextBox txtEmployeeNumber;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;
        private System.Windows.Forms.Label lblBirthPlace;
        private System.Windows.Forms.TextBox txtBirthPlace;
        private System.Windows.Forms.Label lblNationality;
        private System.Windows.Forms.ComboBox cmbNationality;
        private System.Windows.Forms.Label lblNationalID;
        private System.Windows.Forms.TextBox txtNationalID;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblQualification;
        private System.Windows.Forms.ComboBox cmbQualification;
        private System.Windows.Forms.Label lblSpecialization;
        private System.Windows.Forms.TextBox txtSpecialization;
        private System.Windows.Forms.Label lblHireDate;
        private System.Windows.Forms.DateTimePicker dtpHireDate;
        private System.Windows.Forms.GroupBox groupBoxFinancial;
        private System.Windows.Forms.TableLayoutPanel layoutFinancial;
        private System.Windows.Forms.Label lblBasicSalary;
        private System.Windows.Forms.NumericUpDown nudBasicSalary;
        private System.Windows.Forms.Label lblTransportAllowance;
        private System.Windows.Forms.NumericUpDown nudTransportAllowance;
        private System.Windows.Forms.Label lblHousingAllowance;
        private System.Windows.Forms.NumericUpDown nudHousingAllowance;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.FlowLayoutPanel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.DataGridView dataGridViewTeachers;
    }
}
