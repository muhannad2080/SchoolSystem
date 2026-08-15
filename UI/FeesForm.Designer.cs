namespace SchoolSystem.UI
{
    partial class FeesForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();

            this.panelTitle = new Krypton.Toolkit.KryptonPanel();
            this.lblTitle = new System.Windows.Forms.Label();

            this.panelFields = new System.Windows.Forms.Panel();
            this.tableFields = new System.Windows.Forms.TableLayoutPanel();

            this.lblStudent = new System.Windows.Forms.Label();
            this.cmbStudent = new System.Windows.Forms.ComboBox();
            this.lblAcademicYear = new System.Windows.Forms.Label();
            this.cmbAcademicYear = new System.Windows.Forms.ComboBox();

            this.lblFeeType = new System.Windows.Forms.Label();
            this.cmbFeeType = new System.Windows.Forms.ComboBox();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();

            this.lblDiscountAmount = new System.Windows.Forms.Label();
            this.txtDiscountAmount = new System.Windows.Forms.TextBox();
            this.lblNetAmount = new System.Windows.Forms.Label();
            this.txtNetAmount = new System.Windows.Forms.TextBox();

            this.lblPaidAmount = new System.Windows.Forms.Label();
            this.txtPaidAmount = new System.Windows.Forms.TextBox();
            this.lblRemainingAmount = new System.Windows.Forms.Label();
            this.txtRemainingAmount = new System.Windows.Forms.TextBox();

            this.lblDueDate = new System.Windows.Forms.Label();
            this.dtpDueDate = new System.Windows.Forms.DateTimePicker();
            this.lblPaymentDate = new System.Windows.Forms.Label();
            this.dtpPaymentDate = new System.Windows.Forms.DateTimePicker();

            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblReceiptNumber = new System.Windows.Forms.Label();
            this.txtReceiptNumber = new System.Windows.Forms.TextBox();

            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();

            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblSummary = new System.Windows.Forms.Label();

            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnGenerateFees = new System.Windows.Forms.Button();

            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();

            this.dataGridViewFees = new System.Windows.Forms.DataGridView();

            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();

            this.panelTitle.SuspendLayout();
            this.panelFields.SuspendLayout();
            this.tableFields.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFees)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();

            // panelTitle
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(33, 42, 57);
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1100, 55);
            this.panelTitle.TabIndex = 0;

            // lblTitle
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1100, 55);
            this.lblTitle.Text = "إدارة الرسوم الدراسية والتحصيل";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // panelFields
            this.panelFields.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.panelFields.Controls.Add(this.tableFields);
            this.panelFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFields.Location = new System.Drawing.Point(0, 55);
            this.panelFields.Name = "panelFields";
            this.panelFields.Padding = new System.Windows.Forms.Padding(15);
            this.panelFields.Size = new System.Drawing.Size(1100, 285);

            // tableFields
            this.tableFields.ColumnCount = 4;
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableFields.Location = new System.Drawing.Point(15, 15);
            this.tableFields.Name = "tableFields";
            this.tableFields.RowCount = 7;
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableFields.Size = new System.Drawing.Size(1070, 255);

            // Row 0
            this.lblStudent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStudent.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblStudent.Text = "الطالب:";
            this.lblStudent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblStudent, 0, 0);

            this.cmbStudent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStudent.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.tableFields.Controls.Add(this.cmbStudent, 1, 0);

            this.lblAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicYear.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblAcademicYear.Text = "العام الدراسي:";
            this.lblAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblAcademicYear, 2, 0);

            this.cmbAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAcademicYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAcademicYear.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.tableFields.Controls.Add(this.cmbAcademicYear, 3, 0);

            // Row 1
            this.lblFeeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeeType.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblFeeType.Text = "نوع الرسوم:";
            this.lblFeeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblFeeType, 0, 1);

            this.cmbFeeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFeeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbFeeType.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.tableFields.Controls.Add(this.cmbFeeType, 1, 1);

            this.lblTotalAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblTotalAmount.Text = "إجمالي الرسوم:";
            this.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblTotalAmount, 2, 1);

            this.txtTotalAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTotalAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtTotalAmount.Text = "0";
            this.txtTotalAmount.TextChanged += new System.EventHandler(this.AmountFields_TextChanged);
            this.tableFields.Controls.Add(this.txtTotalAmount, 3, 1);

            // Row 2
            this.lblDiscountAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiscountAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblDiscountAmount.Text = "الخصم:";
            this.lblDiscountAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblDiscountAmount, 0, 2);

            this.txtDiscountAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiscountAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtDiscountAmount.Text = "0";
            this.txtDiscountAmount.TextChanged += new System.EventHandler(this.AmountFields_TextChanged);
            this.tableFields.Controls.Add(this.txtDiscountAmount, 1, 2);

            this.lblNetAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblNetAmount.Text = "الصافي:";
            this.lblNetAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblNetAmount, 2, 2);

            this.txtNetAmount.BackColor = System.Drawing.Color.FromArgb(235, 245, 251);
            this.txtNetAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNetAmount.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtNetAmount.ReadOnly = true;
            this.txtNetAmount.Text = "0";
            this.tableFields.Controls.Add(this.txtNetAmount, 3, 2);

            // Row 3
            this.lblPaidAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaidAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblPaidAmount.Text = "المدفوع:";
            this.lblPaidAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblPaidAmount, 0, 3);

            this.txtPaidAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPaidAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtPaidAmount.Text = "0";
            this.txtPaidAmount.TextChanged += new System.EventHandler(this.AmountFields_TextChanged);
            this.tableFields.Controls.Add(this.txtPaidAmount, 1, 3);

            this.lblRemainingAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemainingAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblRemainingAmount.Text = "المتبقي:";
            this.lblRemainingAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblRemainingAmount, 2, 3);

            this.txtRemainingAmount.BackColor = System.Drawing.Color.FromArgb(253, 237, 236);
            this.txtRemainingAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemainingAmount.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtRemainingAmount.ReadOnly = true;
            this.txtRemainingAmount.Text = "0";
            this.tableFields.Controls.Add(this.txtRemainingAmount, 3, 3);

            // Row 4
            this.lblDueDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDueDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblDueDate.Text = "تاريخ الاستحقاق:";
            this.lblDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblDueDate, 0, 4);

            this.dtpDueDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDueDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDueDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dtpDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tableFields.Controls.Add(this.dtpDueDate, 1, 4);

            this.lblPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblPaymentDate.Text = "تاريخ الدفع:";
            this.lblPaymentDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblPaymentDate, 2, 4);

            this.dtpPaymentDate.CustomFormat = "dd/MM/yyyy";
            this.dtpPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpPaymentDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dtpPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPaymentDate.ShowCheckBox = true;
            this.tableFields.Controls.Add(this.dtpPaymentDate, 3, 4);

            // Row 5
            this.lblPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblPaymentMethod.Text = "طريقة الدفع:";
            this.lblPaymentMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblPaymentMethod, 0, 5);

            this.cmbPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.tableFields.Controls.Add(this.cmbPaymentMethod, 1, 5);

            this.lblReceiptNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReceiptNumber.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblReceiptNumber.Text = "رقم السند:";
            this.lblReceiptNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblReceiptNumber, 2, 5);

            this.txtReceiptNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReceiptNumber.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.tableFields.Controls.Add(this.txtReceiptNumber, 3, 5);

            // Row 6
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblStatus.Text = "الحالة:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblStatus, 0, 6);

            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.tableFields.Controls.Add(this.cmbStatus, 1, 6);

            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblNotes.Text = "ملاحظات:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableFields.Controls.Add(this.lblNotes, 2, 6);

            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.tableFields.Controls.Add(this.txtNotes, 3, 6);

            // panelSummary
            this.panelSummary.BackColor = System.Drawing.Color.White;
            this.panelSummary.Controls.Add(this.lblSummary);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 340);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1100, 38);

            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.lblSummary.Text = "الإجمالي: 0 | الخصم: 0 | الصافي: 0 | المدفوع: 0 | المتبقي: 0";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // panelButtons
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnUpdate);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnClear);
            this.panelButtons.Controls.Add(this.btnGenerateFees);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 378);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1100, 55);

            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(970, 10);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(115, 35);
            this.btnAdd.Text = "إضافة";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(845, 10);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(115, 35);
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(720, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(115, 35);
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.btnClear.BackColor = System.Drawing.Color.FromArgb(127, 140, 141);
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(595, 10);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(115, 35);
            this.btnClear.Text = "تفريغ";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.btnGenerateFees.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnGenerateFees.FlatAppearance.BorderSize = 0;
            this.btnGenerateFees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateFees.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerateFees.ForeColor = System.Drawing.Color.White;
            this.btnGenerateFees.Location = new System.Drawing.Point(420, 10);
            this.btnGenerateFees.Name = "btnGenerateFees";
            this.btnGenerateFees.Size = new System.Drawing.Size(165, 35);
            this.btnGenerateFees.Text = "توليد رسوم الطالب";
            this.btnGenerateFees.UseVisualStyleBackColor = false;
            this.btnGenerateFees.Click += new System.EventHandler(this.btnGenerateFees_Click);

            // panelSearch
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.cmbFilterStatus);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 433);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1100, 45);

            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblSearch.Location = new System.Drawing.Point(970, 10);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(110, 25);
            this.lblSearch.Text = "بحث / تصفية:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(720, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(240, 24);
            this.txtSearch.TextChanged += new System.EventHandler(this.FilterControls_Changed);

            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbFilterStatus.Location = new System.Drawing.Point(550, 10);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(160, 24);
            this.cmbFilterStatus.SelectedIndexChanged += new System.EventHandler(this.FilterControls_Changed);

            // dataGridViewFees
            this.dataGridViewFees.AllowUserToAddRows = false;
            this.dataGridViewFees.AllowUserToDeleteRows = false;
            this.dataGridViewFees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewFees.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewFees.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFees.EnableHeadersVisualStyles = false;
            this.dataGridViewFees.Location = new System.Drawing.Point(0, 478);
            this.dataGridViewFees.MultiSelect = false;
            this.dataGridViewFees.Name = "dataGridViewFees";
            this.dataGridViewFees.ReadOnly = true;
            this.dataGridViewFees.RowHeadersVisible = false;
            this.dataGridViewFees.RowTemplate.Height = 34;
            this.dataGridViewFees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFees.Size = new System.Drawing.Size(1100, 192);
            this.dataGridViewFees.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFees_CellClick);

            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(33, 42, 57);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewFees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewFees.ColumnHeadersHeight = 38;

            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dataGridViewFees.DefaultCellStyle = dataGridViewCellStyle2;

            // panelBottom
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 670);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1100, 30);

            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Text = "عدد السجلات: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // FeesForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewFees);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelFields);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "FeesForm";
            this.Text = "إدارة الرسوم الدراسية";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1100, 700);

            this.panelTitle.ResumeLayout(false);
            this.panelFields.ResumeLayout(false);
            this.tableFields.ResumeLayout(false);
            this.tableFields.PerformLayout();
            this.panelSummary.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFees)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelFields;
        private System.Windows.Forms.TableLayoutPanel tableFields;

        private System.Windows.Forms.Label lblStudent;
        private System.Windows.Forms.ComboBox cmbStudent;
        private System.Windows.Forms.Label lblAcademicYear;
        private System.Windows.Forms.ComboBox cmbAcademicYear;
        private System.Windows.Forms.Label lblFeeType;
        private System.Windows.Forms.ComboBox cmbFeeType;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label lblDiscountAmount;
        private System.Windows.Forms.TextBox txtDiscountAmount;
        private System.Windows.Forms.Label lblNetAmount;
        private System.Windows.Forms.TextBox txtNetAmount;
        private System.Windows.Forms.Label lblPaidAmount;
        private System.Windows.Forms.TextBox txtPaidAmount;
        private System.Windows.Forms.Label lblRemainingAmount;
        private System.Windows.Forms.TextBox txtRemainingAmount;
        private System.Windows.Forms.Label lblDueDate;
        private System.Windows.Forms.DateTimePicker dtpDueDate;
        private System.Windows.Forms.Label lblPaymentDate;
        private System.Windows.Forms.DateTimePicker dtpPaymentDate;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblReceiptNumber;
        private System.Windows.Forms.TextBox txtReceiptNumber;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblSummary;

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnGenerateFees;

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilterStatus;

        private System.Windows.Forms.DataGridView dataGridViewFees;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
