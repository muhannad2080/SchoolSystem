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
            this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
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
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnGenerateFees = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnRecordPayment = new System.Windows.Forms.Button();
            this.btnOutstandingReport = new System.Windows.Forms.Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.dataGridViewFees = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.rootLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTitle)).BeginInit();
            this.panelTitle.SuspendLayout();
            this.panelFields.SuspendLayout();
            this.tableFields.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFees)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // rootLayout
            // 
            this.rootLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.panelTitle, 0, 0);
            this.rootLayout.Controls.Add(this.panelFields, 0, 1);
            this.rootLayout.Controls.Add(this.panelSummary, 0, 2);
            this.rootLayout.Controls.Add(this.panelButtons, 0, 3);
            this.rootLayout.Controls.Add(this.panelSearch, 0, 4);
            this.rootLayout.Controls.Add(this.dataGridViewFees, 0, 5);
            this.rootLayout.Controls.Add(this.panelBottom, 0, 6);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Location = new System.Drawing.Point(0, 0);
            this.rootLayout.Margin = new System.Windows.Forms.Padding(0);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.RowCount = 7;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.rootLayout.Size = new System.Drawing.Size(1100, 700);
            this.rootLayout.TabIndex = 0;
            // 
            // panelTitle
            // 
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1100, 49);
            this.panelTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1100, 49);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "إدارة الرسوم الدراسية والتحصيل";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelFields
            // 
            this.panelFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelFields.Controls.Add(this.tableFields);
            this.panelFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFields.Location = new System.Drawing.Point(0, 49);
            this.panelFields.Margin = new System.Windows.Forms.Padding(0);
            this.panelFields.Name = "panelFields";
            this.panelFields.Padding = new System.Windows.Forms.Padding(18, 7, 18, 7);
            this.panelFields.Size = new System.Drawing.Size(1100, 196);
            this.panelFields.TabIndex = 1;
            // 
            // tableFields
            // 
            this.tableFields.ColumnCount = 6;
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6667F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6667F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3333F));
            this.tableFields.Controls.Add(this.lblStudent, 0, 0);
            this.tableFields.Controls.Add(this.cmbStudent, 1, 0);
            this.tableFields.Controls.Add(this.lblAcademicYear, 2, 0);
            this.tableFields.Controls.Add(this.cmbAcademicYear, 3, 0);
            this.tableFields.Controls.Add(this.lblFeeType, 4, 0);
            this.tableFields.Controls.Add(this.cmbFeeType, 5, 0);
            this.tableFields.Controls.Add(this.lblTotalAmount, 0, 1);
            this.tableFields.Controls.Add(this.txtTotalAmount, 1, 1);
            this.tableFields.Controls.Add(this.lblDiscountAmount, 2, 1);
            this.tableFields.Controls.Add(this.txtDiscountAmount, 3, 1);
            this.tableFields.Controls.Add(this.lblNetAmount, 4, 1);
            this.tableFields.Controls.Add(this.txtNetAmount, 5, 1);
            this.tableFields.Controls.Add(this.lblPaidAmount, 0, 2);
            this.tableFields.Controls.Add(this.txtPaidAmount, 1, 2);
            this.tableFields.Controls.Add(this.lblRemainingAmount, 2, 2);
            this.tableFields.Controls.Add(this.txtRemainingAmount, 3, 2);
            this.tableFields.Controls.Add(this.lblDueDate, 4, 2);
            this.tableFields.Controls.Add(this.dtpDueDate, 5, 2);
            this.tableFields.Controls.Add(this.lblPaymentDate, 0, 3);
            this.tableFields.Controls.Add(this.dtpPaymentDate, 1, 3);
            this.tableFields.Controls.Add(this.lblPaymentMethod, 2, 3);
            this.tableFields.Controls.Add(this.cmbPaymentMethod, 3, 3);
            this.tableFields.Controls.Add(this.lblReceiptNumber, 4, 3);
            this.tableFields.Controls.Add(this.txtReceiptNumber, 5, 3);
            this.tableFields.Controls.Add(this.lblStatus, 0, 4);
            this.tableFields.Controls.Add(this.cmbStatus, 1, 4);
            this.tableFields.Controls.Add(this.lblNotes, 2, 4);
            this.tableFields.Controls.Add(this.txtNotes, 3, 4);
            this.tableFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableFields.Location = new System.Drawing.Point(18, 7);
            this.tableFields.Margin = new System.Windows.Forms.Padding(0);
            this.tableFields.Name = "tableFields";
            this.tableFields.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableFields.RowCount = 5;
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableFields.Size = new System.Drawing.Size(1064, 182);
            this.tableFields.TabIndex = 0;
            // 
            // lblStudent
            // 
            this.lblStudent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStudent.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStudent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblStudent.Location = new System.Drawing.Point(988, 0);
            this.lblStudent.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(76, 36);
            this.lblStudent.TabIndex = 0;
            this.lblStudent.Text = "الطالب";
            this.lblStudent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStudent
            // 
            this.cmbStudent.BackColor = System.Drawing.Color.White;
            this.cmbStudent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStudent.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbStudent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.cmbStudent.Location = new System.Drawing.Point(789, 4);
            this.cmbStudent.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.cmbStudent.Name = "cmbStudent";
            this.cmbStudent.Size = new System.Drawing.Size(193, 28);
            this.cmbStudent.TabIndex = 1;
            // 
            // lblAcademicYear
            // 
            this.lblAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicYear.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblAcademicYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblAcademicYear.Location = new System.Drawing.Point(697, 0);
            this.lblAcademicYear.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblAcademicYear.Name = "lblAcademicYear";
            this.lblAcademicYear.Size = new System.Drawing.Size(82, 36);
            this.lblAcademicYear.TabIndex = 2;
            this.lblAcademicYear.Text = "العام الدراسي";
            this.lblAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbAcademicYear
            // 
            this.cmbAcademicYear.BackColor = System.Drawing.Color.White;
            this.cmbAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAcademicYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAcademicYear.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbAcademicYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.cmbAcademicYear.Location = new System.Drawing.Point(498, 4);
            this.cmbAcademicYear.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.cmbAcademicYear.Name = "cmbAcademicYear";
            this.cmbAcademicYear.Size = new System.Drawing.Size(193, 28);
            this.cmbAcademicYear.TabIndex = 3;
            // 
            // lblFeeType
            // 
            this.lblFeeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeeType.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblFeeType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblFeeType.Location = new System.Drawing.Point(412, 0);
            this.lblFeeType.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblFeeType.Name = "lblFeeType";
            this.lblFeeType.Size = new System.Drawing.Size(76, 36);
            this.lblFeeType.TabIndex = 4;
            this.lblFeeType.Text = "نوع الرسوم";
            this.lblFeeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFeeType
            // 
            this.cmbFeeType.BackColor = System.Drawing.Color.White;
            this.cmbFeeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFeeType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbFeeType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.cmbFeeType.Location = new System.Drawing.Point(0, 4);
            this.cmbFeeType.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cmbFeeType.Name = "cmbFeeType";
            this.cmbFeeType.Size = new System.Drawing.Size(406, 28);
            this.cmbFeeType.TabIndex = 5;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblTotalAmount.Location = new System.Drawing.Point(988, 36);
            this.lblTotalAmount.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(76, 36);
            this.lblTotalAmount.TabIndex = 6;
            this.lblTotalAmount.Text = "الإجمالي";
            this.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.BackColor = System.Drawing.Color.White;
            this.txtTotalAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTotalAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTotalAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.txtTotalAmount.Location = new System.Drawing.Point(789, 40);
            this.txtTotalAmount.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(193, 27);
            this.txtTotalAmount.TabIndex = 7;
            this.txtTotalAmount.Text = "0";
            this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalAmount.TextChanged += new System.EventHandler(this.AmountFields_TextChanged);
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiscountAmount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDiscountAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblDiscountAmount.Location = new System.Drawing.Point(697, 36);
            this.lblDiscountAmount.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            this.lblDiscountAmount.Size = new System.Drawing.Size(82, 36);
            this.lblDiscountAmount.TabIndex = 8;
            this.lblDiscountAmount.Text = "الخصم";
            this.lblDiscountAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDiscountAmount
            // 
            this.txtDiscountAmount.BackColor = System.Drawing.Color.White;
            this.txtDiscountAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiscountAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDiscountAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.txtDiscountAmount.Location = new System.Drawing.Point(498, 40);
            this.txtDiscountAmount.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.txtDiscountAmount.Name = "txtDiscountAmount";
            this.txtDiscountAmount.Size = new System.Drawing.Size(193, 27);
            this.txtDiscountAmount.TabIndex = 9;
            this.txtDiscountAmount.Text = "0";
            this.txtDiscountAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDiscountAmount.TextChanged += new System.EventHandler(this.AmountFields_TextChanged);
            // 
            // lblNetAmount
            // 
            this.lblNetAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetAmount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNetAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(120)))), ((int)(((byte)(92)))));
            this.lblNetAmount.Location = new System.Drawing.Point(412, 36);
            this.lblNetAmount.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblNetAmount.Name = "lblNetAmount";
            this.lblNetAmount.Size = new System.Drawing.Size(76, 36);
            this.lblNetAmount.TabIndex = 10;
            this.lblNetAmount.Text = "الصافي";
            this.lblNetAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNetAmount
            // 
            this.txtNetAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(248)))), ((int)(((byte)(244)))));
            this.txtNetAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNetAmount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.txtNetAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(100)))), ((int)(((byte)(78)))));
            this.txtNetAmount.Location = new System.Drawing.Point(0, 40);
            this.txtNetAmount.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtNetAmount.Name = "txtNetAmount";
            this.txtNetAmount.ReadOnly = true;
            this.txtNetAmount.Size = new System.Drawing.Size(406, 27);
            this.txtNetAmount.TabIndex = 11;
            this.txtNetAmount.Text = "0";
            this.txtNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPaidAmount
            // 
            this.lblPaidAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaidAmount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPaidAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblPaidAmount.Location = new System.Drawing.Point(988, 72);
            this.lblPaidAmount.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblPaidAmount.Name = "lblPaidAmount";
            this.lblPaidAmount.Size = new System.Drawing.Size(76, 36);
            this.lblPaidAmount.TabIndex = 12;
            this.lblPaidAmount.Text = "المدفوع";
            this.lblPaidAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPaidAmount
            // 
            this.txtPaidAmount.BackColor = System.Drawing.Color.White;
            this.txtPaidAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPaidAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPaidAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.txtPaidAmount.Location = new System.Drawing.Point(789, 76);
            this.txtPaidAmount.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.txtPaidAmount.Name = "txtPaidAmount";
            this.txtPaidAmount.Size = new System.Drawing.Size(193, 27);
            this.txtPaidAmount.TabIndex = 13;
            this.txtPaidAmount.Text = "0";
            this.txtPaidAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPaidAmount.TextChanged += new System.EventHandler(this.AmountFields_TextChanged);
            // 
            // lblRemainingAmount
            // 
            this.lblRemainingAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemainingAmount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblRemainingAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(72)))), ((int)(((byte)(66)))));
            this.lblRemainingAmount.Location = new System.Drawing.Point(697, 72);
            this.lblRemainingAmount.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblRemainingAmount.Name = "lblRemainingAmount";
            this.lblRemainingAmount.Size = new System.Drawing.Size(82, 36);
            this.lblRemainingAmount.TabIndex = 14;
            this.lblRemainingAmount.Text = "المتبقي";
            this.lblRemainingAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRemainingAmount
            // 
            this.txtRemainingAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(243)))), ((int)(((byte)(242)))));
            this.txtRemainingAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemainingAmount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.txtRemainingAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(60)))), ((int)(((byte)(55)))));
            this.txtRemainingAmount.Location = new System.Drawing.Point(498, 76);
            this.txtRemainingAmount.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.txtRemainingAmount.Name = "txtRemainingAmount";
            this.txtRemainingAmount.ReadOnly = true;
            this.txtRemainingAmount.Size = new System.Drawing.Size(193, 27);
            this.txtRemainingAmount.TabIndex = 15;
            this.txtRemainingAmount.Text = "0";
            this.txtRemainingAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDueDate
            // 
            this.lblDueDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDueDate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDueDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblDueDate.Location = new System.Drawing.Point(412, 72);
            this.lblDueDate.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.Size = new System.Drawing.Size(76, 36);
            this.lblDueDate.TabIndex = 16;
            this.lblDueDate.Text = "الاستحقاق";
            this.lblDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDueDate
            // 
            this.dtpDueDate.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDueDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDueDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDueDate.Location = new System.Drawing.Point(0, 76);
            this.dtpDueDate.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.dtpDueDate.Name = "dtpDueDate";
            this.dtpDueDate.Size = new System.Drawing.Size(406, 27);
            this.dtpDueDate.TabIndex = 17;
            // 
            // lblPaymentDate
            // 
            this.lblPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentDate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPaymentDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblPaymentDate.Location = new System.Drawing.Point(988, 108);
            this.lblPaymentDate.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblPaymentDate.Name = "lblPaymentDate";
            this.lblPaymentDate.Size = new System.Drawing.Size(76, 36);
            this.lblPaymentDate.TabIndex = 18;
            this.lblPaymentDate.Text = "تاريخ الدفع";
            this.lblPaymentDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpPaymentDate
            // 
            this.dtpPaymentDate.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpPaymentDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPaymentDate.Location = new System.Drawing.Point(789, 112);
            this.dtpPaymentDate.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.dtpPaymentDate.Name = "dtpPaymentDate";
            this.dtpPaymentDate.ShowCheckBox = true;
            this.dtpPaymentDate.Size = new System.Drawing.Size(193, 27);
            this.dtpPaymentDate.TabIndex = 19;
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPaymentMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblPaymentMethod.Location = new System.Drawing.Point(697, 108);
            this.lblPaymentMethod.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(82, 36);
            this.lblPaymentMethod.TabIndex = 20;
            this.lblPaymentMethod.Text = "طريقة الدفع";
            this.lblPaymentMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.BackColor = System.Drawing.Color.White;
            this.cmbPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbPaymentMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.cmbPaymentMethod.Location = new System.Drawing.Point(498, 112);
            this.cmbPaymentMethod.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(193, 28);
            this.cmbPaymentMethod.TabIndex = 21;
            // 
            // lblReceiptNumber
            // 
            this.lblReceiptNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReceiptNumber.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblReceiptNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblReceiptNumber.Location = new System.Drawing.Point(412, 108);
            this.lblReceiptNumber.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblReceiptNumber.Name = "lblReceiptNumber";
            this.lblReceiptNumber.Size = new System.Drawing.Size(76, 36);
            this.lblReceiptNumber.TabIndex = 22;
            this.lblReceiptNumber.Text = "رقم السند";
            this.lblReceiptNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReceiptNumber
            // 
            this.txtReceiptNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.txtReceiptNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReceiptNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtReceiptNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(114)))), ((int)(((byte)(126)))));
            this.txtReceiptNumber.Location = new System.Drawing.Point(0, 112);
            this.txtReceiptNumber.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtReceiptNumber.Name = "txtReceiptNumber";
            this.txtReceiptNumber.ReadOnly = true;
            this.txtReceiptNumber.Size = new System.Drawing.Size(406, 27);
            this.txtReceiptNumber.TabIndex = 23;
            this.txtReceiptNumber.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblStatus.Location = new System.Drawing.Point(988, 144);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(76, 38);
            this.lblStatus.TabIndex = 24;
            this.lblStatus.Text = "الحالة";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.White;
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.cmbStatus.Location = new System.Drawing.Point(789, 148);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(193, 28);
            this.cmbStatus.TabIndex = 25;
            // 
            // lblNotes
            // 
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(78)))), ((int)(((byte)(94)))));
            this.lblNotes.Location = new System.Drawing.Point(697, 144);
            this.lblNotes.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(82, 38);
            this.lblNotes.TabIndex = 26;
            this.lblNotes.Text = "الملاحظات";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.White;
            this.tableFields.SetColumnSpan(this.txtNotes, 3);
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(58)))));
            this.txtNotes.Location = new System.Drawing.Point(0, 148);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(691, 27);
            this.txtNotes.TabIndex = 27;
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.White;
            this.panelSummary.Controls.Add(this.lblSummary);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSummary.Location = new System.Drawing.Point(0, 245);
            this.panelSummary.Margin = new System.Windows.Forms.Padding(0);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.panelSummary.Size = new System.Drawing.Size(1100, 35);
            this.panelSummary.TabIndex = 2;
            // 
            // lblSummary
            // 
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(105)))), ((int)(((byte)(84)))));
            this.lblSummary.Location = new System.Drawing.Point(12, 0);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(1076, 35);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "الإجمالي: 0    |    الخصم: 0    |    الصافي: 0    |    المدفوع: 0    |    المتبقي" +
    ": 0";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelButtons
            // 
            this.panelButtons.AutoScroll = true;
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnUpdate);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnClear);
            this.panelButtons.Controls.Add(this.btnGenerateFees);
            this.panelButtons.Controls.Add(this.btnExportExcel);
            this.panelButtons.Controls.Add(this.btnExportPdf);
            this.panelButtons.Controls.Add(this.btnRecordPayment);
            this.panelButtons.Controls.Add(this.btnOutstandingReport);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelButtons.Location = new System.Drawing.Point(0, 280);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(14, 5, 14, 5);
            this.panelButtons.Size = new System.Drawing.Size(1100, 56);
            this.panelButtons.TabIndex = 3;
            this.panelButtons.WrapContents = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(18, 10);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 36);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "إضافة";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(138)))), ((int)(((byte)(42)))));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(114, 10);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(88, 36);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(210, 10);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(88, 36);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(306, 10);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 36);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "تفريغ";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnGenerateFees
            // 
            this.btnGenerateFees.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnGenerateFees.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerateFees.FlatAppearance.BorderSize = 0;
            this.btnGenerateFees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateFees.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnGenerateFees.ForeColor = System.Drawing.Color.White;
            this.btnGenerateFees.Location = new System.Drawing.Point(402, 10);
            this.btnGenerateFees.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGenerateFees.Name = "btnGenerateFees";
            this.btnGenerateFees.Size = new System.Drawing.Size(122, 36);
            this.btnGenerateFees.TabIndex = 4;
            this.btnGenerateFees.Text = "توليد الرسوم";
            this.btnGenerateFees.UseVisualStyleBackColor = false;
            this.btnGenerateFees.Click += new System.EventHandler(this.btnGenerateFees_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(124)))), ((int)(((byte)(89)))));
            this.btnExportExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(532, 10);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(112, 36);
            this.btnExportExcel.TabIndex = 5;
            this.btnExportExcel.Text = "تصدير Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(58)))), ((int)(((byte)(237)))));
            this.btnExportPdf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportPdf.FlatAppearance.BorderSize = 0;
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPdf.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnExportPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportPdf.Location = new System.Drawing.Point(652, 10);
            this.btnExportPdf.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(104, 36);
            this.btnExportPdf.TabIndex = 6;
            this.btnExportPdf.Text = "تصدير PDF";
            this.btnExportPdf.UseVisualStyleBackColor = false;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // 
            // btnRecordPayment
            // 
            this.btnRecordPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(65)))), ((int)(((byte)(12)))));
            this.btnRecordPayment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecordPayment.FlatAppearance.BorderSize = 0;
            this.btnRecordPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecordPayment.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnRecordPayment.ForeColor = System.Drawing.Color.White;
            this.btnRecordPayment.Location = new System.Drawing.Point(764, 10);
            this.btnRecordPayment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRecordPayment.Name = "btnRecordPayment";
            this.btnRecordPayment.Size = new System.Drawing.Size(116, 36);
            this.btnRecordPayment.TabIndex = 7;
            this.btnRecordPayment.Text = "تحصيل دفعة";
            this.btnRecordPayment.UseVisualStyleBackColor = false;
            this.btnRecordPayment.Click += new System.EventHandler(this.btnRecordPayment_Click);
            // 
            // btnOutstandingReport
            // 
            this.btnOutstandingReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btnOutstandingReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOutstandingReport.FlatAppearance.BorderSize = 0;
            this.btnOutstandingReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutstandingReport.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnOutstandingReport.ForeColor = System.Drawing.Color.White;
            this.btnOutstandingReport.Location = new System.Drawing.Point(888, 10);
            this.btnOutstandingReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOutstandingReport.Name = "btnOutstandingReport";
            this.btnOutstandingReport.Size = new System.Drawing.Size(126, 36);
            this.btnOutstandingReport.TabIndex = 8;
            this.btnOutstandingReport.Text = "تقرير المتأخرات";
            this.btnOutstandingReport.UseVisualStyleBackColor = false;
            this.btnOutstandingReport.Click += new System.EventHandler(this.btnOutstandingReport_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.cmbFilterStatus);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearch.Location = new System.Drawing.Point(0, 336);
            this.panelSearch.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(18, 3, 18, 3);
            this.panelSearch.Size = new System.Drawing.Size(1100, 28);
            this.panelSearch.TabIndex = 4;
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblSearch.Location = new System.Drawing.Point(590, 3);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(62, 22);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.txtSearch.Location = new System.Drawing.Point(652, 3);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSearch.Size = new System.Drawing.Size(270, 26);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.FilterControls_Changed);
            // 
            // cmbFilterStatus
            // 
            this.cmbFilterStatus.BackColor = System.Drawing.Color.White;
            this.cmbFilterStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbFilterStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.cmbFilterStatus.Location = new System.Drawing.Point(922, 3);
            this.cmbFilterStatus.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(160, 27);
            this.cmbFilterStatus.TabIndex = 2;
            this.cmbFilterStatus.SelectedIndexChanged += new System.EventHandler(this.FilterControls_Changed);
            // 
            // dataGridViewFees
            // 
            this.dataGridViewFees.AllowUserToAddRows = false;
            this.dataGridViewFees.AllowUserToDeleteRows = false;
            this.dataGridViewFees.AllowUserToResizeRows = false;
            this.dataGridViewFees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewFees.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewFees.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewFees.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewFees.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewFees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewFees.ColumnHeadersHeight = 38;
            this.dataGridViewFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewFees.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFees.EnableHeadersVisualStyles = false;
            this.dataGridViewFees.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(235)))), ((int)(((byte)(240)))));
            this.dataGridViewFees.Location = new System.Drawing.Point(0, 364);
            this.dataGridViewFees.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewFees.MultiSelect = false;
            this.dataGridViewFees.Name = "dataGridViewFees";
            this.dataGridViewFees.ReadOnly = true;
            this.dataGridViewFees.RowHeadersVisible = false;
            this.dataGridViewFees.RowHeadersWidth = 51;
            this.dataGridViewFees.RowTemplate.Height = 34;
            this.dataGridViewFees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFees.Size = new System.Drawing.Size(1100, 315);
            this.dataGridViewFees.TabIndex = 5;
            this.dataGridViewFees.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFees_CellClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 679);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1100, 21);
            this.panelBottom.TabIndex = 6;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblRecordCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(1100, 21);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد السجلات: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FeesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.rootLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "FeesForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1100, 700);
            this.rootLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelTitle)).EndInit();
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

        // ============================================================
        // Main Layout
        // ============================================================

        private System.Windows.Forms.TableLayoutPanel rootLayout;

        // ============================================================
        // Header
        // ============================================================

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;

        // ============================================================
        // Fields
        // ============================================================

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

        // ============================================================
        // Summary
        // ============================================================

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblSummary;

        // ============================================================
        // Actions
        // ============================================================

        private System.Windows.Forms.FlowLayoutPanel panelButtons;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnGenerateFees;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnRecordPayment;
        private System.Windows.Forms.Button btnOutstandingReport;

        // ============================================================
        // Search
        // ============================================================

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilterStatus;

        // ============================================================
        // Data
        // ============================================================

        private System.Windows.Forms.DataGridView dataGridViewFees;

        // ============================================================
        // Footer
        // ============================================================

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
