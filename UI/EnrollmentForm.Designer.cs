namespace SchoolSystem.UI
{
    partial class EnrollmentForm
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
            this.lblEnrollmentID = new System.Windows.Forms.Label();
            this.txtEnrollmentID = new System.Windows.Forms.TextBox();
            this.lblStudentID = new System.Windows.Forms.Label();
            this.cmbStudentID = new System.Windows.Forms.ComboBox();
            this.lblStudentName = new System.Windows.Forms.Label();
            this.txtStudentName = new System.Windows.Forms.TextBox();
            this.lblApplicationDate = new System.Windows.Forms.Label();
            this.dtpApplicationDate = new System.Windows.Forms.DateTimePicker();
            this.lblApplicationType = new System.Windows.Forms.Label();
            this.cmbApplicationType = new System.Windows.Forms.ComboBox();
            this.lblAcademicYear = new System.Windows.Forms.Label();
            this.txtAcademicYear = new System.Windows.Forms.TextBox();
            this.lblClassID = new System.Windows.Forms.Label();
            this.cmbClassID = new System.Windows.Forms.ComboBox();
            this.lblSection = new System.Windows.Forms.Label();
            this.txtSection = new System.Windows.Forms.ComboBox();
            this.lblSeatNumber = new System.Windows.Forms.Label();
            this.txtSeatNumber = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblPreviousSchool = new System.Windows.Forms.Label();
            this.txtPreviousSchool = new System.Windows.Forms.TextBox();
            this.lblPreviousClass = new System.Windows.Forms.Label();
            this.txtPreviousClass = new System.Windows.Forms.TextBox();
            this.lblTransferReason = new System.Windows.Forms.Label();
            this.txtTransferReason = new System.Windows.Forms.TextBox();
            this.lblRegistrationFee = new System.Windows.Forms.Label();
            this.txtRegistrationFee = new System.Windows.Forms.TextBox();
            this.lblPaidAmount = new System.Windows.Forms.Label();
            this.txtPaidAmount = new System.Windows.Forms.TextBox();
            this.lblRemainingAmount = new System.Windows.Forms.Label();
            this.txtRemainingAmount = new System.Windows.Forms.TextBox();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblReceiptNo = new System.Windows.Forms.Label();
            this.txtReceiptNo = new System.Windows.Forms.TextBox();
            this.chkHasBirthCertificate = new System.Windows.Forms.CheckBox();
            this.chkHasGuardianId = new System.Windows.Forms.CheckBox();
            this.chkHasPhoto = new System.Windows.Forms.CheckBox();
            this.chkHasLastCertificate = new System.Windows.Forms.CheckBox();
            this.chkHasMedicalReport = new System.Windows.Forms.CheckBox();
            this.gbBasic = new System.Windows.Forms.GroupBox();
            this.tlpBasic = new System.Windows.Forms.TableLayoutPanel();
            this.gbPrevious = new System.Windows.Forms.GroupBox();
            this.tlpPrevious = new System.Windows.Forms.TableLayoutPanel();
            this.gbFees = new System.Windows.Forms.GroupBox();
            this.tlpFees = new System.Windows.Forms.TableLayoutPanel();
            this.gbAttachments = new System.Windows.Forms.GroupBox();
            this.flpAttachments = new System.Windows.Forms.FlowLayoutPanel();
            this.gbNotes = new System.Windows.Forms.GroupBox();
            this.rtbNotes = new System.Windows.Forms.RichTextBox();
            this.layoutRight = new System.Windows.Forms.TableLayoutPanel();
            this.dgvEnrollments = new System.Windows.Forms.DataGridView();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.tlpSearch = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnPreviewOutput = new System.Windows.Forms.Button();
            this.btnPrintReceipt = new System.Windows.Forms.Button();
            this.btnPrintForm = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();

            this.splitContainerMain.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.layoutRight.SuspendLayout();
            this.gbBasic.SuspendLayout();
            this.tlpBasic.SuspendLayout();
            this.gbPrevious.SuspendLayout();
            this.tlpPrevious.SuspendLayout();
            this.gbFees.SuspendLayout();
            this.tlpFees.SuspendLayout();
            this.gbAttachments.SuspendLayout();
            this.flpAttachments.SuspendLayout();
            this.gbNotes.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.MinimumSize = new System.Drawing.Size(1100, 760);
            this.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة التسجيل والقبول";
            this.Name = "EnrollmentForm";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Load += new System.EventHandler(this.EnrollmentForm_Load);

            // Main split
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.splitContainerMain.SplitterWidth = 7;
            this.splitContainerMain.SplitterDistance = 545;
            this.splitContainerMain.IsSplitterFixed = false;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitContainerMain.Panel1.AutoScroll = true;
            this.splitContainerMain.Panel1.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.splitContainerMain.Panel2.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);

            // Left form layout
            this.layoutRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutRight.AutoSize = false;
            this.layoutRight.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.layoutRight.ColumnCount = 1;
            this.layoutRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRight.Location = new System.Drawing.Point(8, 8);
            this.layoutRight.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRight.Name = "layoutRight";
            this.layoutRight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.layoutRight.RowCount = 5;
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutRight.Size = new System.Drawing.Size(505, 725);
            this.layoutRight.TabIndex = 0;

            // Basic group
            this.gbBasic.BackColor = System.Drawing.Color.White;
            this.gbBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBasic.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbBasic.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbBasic.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.gbBasic.Padding = new System.Windows.Forms.Padding(9, 24, 9, 8);
            this.gbBasic.Text = "بيانات التسجيل الأساسية";
            this.gbBasic.Controls.Add(this.tlpBasic);

            this.tlpBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBasic.ColumnCount = 4;
            this.tlpBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlpBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlpBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBasic.RowCount = 5;
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpBasic.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBasic.Padding = new System.Windows.Forms.Padding(0);
            this.tlpBasic.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

            this.lblEnrollmentID.AutoSize = false;
            this.lblEnrollmentID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEnrollmentID.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblEnrollmentID.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblEnrollmentID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblEnrollmentID.Text = "رقم الطلب";
            this.lblEnrollmentID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblStudentID.AutoSize = false;
            this.lblStudentID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStudentID.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblStudentID.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblStudentID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblStudentID.Text = "اختيار الطالب";
            this.lblStudentID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblStudentName.AutoSize = false;
            this.lblStudentName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStudentName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblStudentName.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblStudentName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblStudentName.Text = "اسم الطالب";
            this.lblStudentName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblApplicationDate.AutoSize = false;
            this.lblApplicationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationDate.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblApplicationDate.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblApplicationDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblApplicationDate.Text = "تاريخ التسجيل";
            this.lblApplicationDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblApplicationType.AutoSize = false;
            this.lblApplicationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationType.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblApplicationType.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblApplicationType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblApplicationType.Text = "نوع الطلب";
            this.lblApplicationType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblAcademicYear.AutoSize = false;
            this.lblAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicYear.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblAcademicYear.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblAcademicYear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblAcademicYear.Text = "العام الدراسي";
            this.lblAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblClassID.AutoSize = false;
            this.lblClassID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassID.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblClassID.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblClassID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblClassID.Text = "الصف";
            this.lblClassID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblSection.AutoSize = false;
            this.lblSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSection.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblSection.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblSection.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblSection.Text = "الشعبة";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblSeatNumber.AutoSize = false;
            this.lblSeatNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSeatNumber.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblSeatNumber.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblSeatNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblSeatNumber.Text = "رقم الجلوس";
            this.lblSeatNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblStatus.AutoSize = false;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblStatus.Text = "حالة الطلب";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.tlpBasic.Controls.Add(this.lblEnrollmentID, 0, 0);
            this.tlpBasic.Controls.Add(this.txtEnrollmentID, 1, 0);
            this.tlpBasic.Controls.Add(this.lblStudentID, 2, 0);
            this.tlpBasic.Controls.Add(this.cmbStudentID, 3, 0);
            this.tlpBasic.Controls.Add(this.lblStudentName, 0, 1);
            this.tlpBasic.Controls.Add(this.txtStudentName, 1, 1);
            this.tlpBasic.Controls.Add(this.lblApplicationDate, 2, 1);
            this.tlpBasic.Controls.Add(this.dtpApplicationDate, 3, 1);
            this.tlpBasic.Controls.Add(this.lblApplicationType, 0, 2);
            this.tlpBasic.Controls.Add(this.cmbApplicationType, 1, 2);
            this.tlpBasic.Controls.Add(this.lblAcademicYear, 2, 2);
            this.tlpBasic.Controls.Add(this.txtAcademicYear, 3, 2);
            this.tlpBasic.Controls.Add(this.lblClassID, 0, 3);
            this.tlpBasic.Controls.Add(this.cmbClassID, 1, 3);
            this.tlpBasic.Controls.Add(this.lblSection, 2, 3);
            this.tlpBasic.Controls.Add(this.txtSection, 3, 3);
            this.tlpBasic.Controls.Add(this.lblSeatNumber, 0, 4);
            this.tlpBasic.Controls.Add(this.txtSeatNumber, 1, 4);
            this.tlpBasic.Controls.Add(this.lblStatus, 2, 4);
            this.tlpBasic.Controls.Add(this.cmbStatus, 3, 4);

            // Common basic input settings
            this.txtEnrollmentID.ReadOnly = true;
            this.txtEnrollmentID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEnrollmentID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtEnrollmentID.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.txtEnrollmentID.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtEnrollmentID.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);

            this.cmbStudentID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStudentID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStudentID.IntegralHeight = false;
            this.cmbStudentID.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.cmbStudentID.DropDownWidth = 330;

            this.txtStudentName.ReadOnly = true;
            this.txtStudentName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStudentName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtStudentName.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.txtStudentName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);

            this.dtpApplicationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpApplicationDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpApplicationDate.CustomFormat = "dd/MM/yyyy";
            this.dtpApplicationDate.MinDate = new System.DateTime(1900, 1, 1);
            this.dtpApplicationDate.MaxDate = System.DateTime.Today;
            this.dtpApplicationDate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.dtpApplicationDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dtpApplicationDate.RightToLeftLayout = true;

            this.cmbApplicationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbApplicationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbApplicationType.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.cmbApplicationType.IntegralHeight = false;

            this.txtAcademicYear.ReadOnly = true;
            this.txtAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAcademicYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAcademicYear.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.txtAcademicYear.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);

            this.cmbClassID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClassID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClassID.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.cmbClassID.IntegralHeight = false;

            this.txtSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.txtSection.IntegralHeight = false;

            this.txtSeatNumber.ReadOnly = true;
            this.txtSeatNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSeatNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSeatNumber.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.txtSeatNumber.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);

            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.cmbStatus.IntegralHeight = false;

            // Previous school
            this.lblPreviousSchool.AutoSize = false;
            this.lblPreviousSchool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPreviousSchool.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPreviousSchool.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblPreviousSchool.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblPreviousSchool.Text = "المدرسة السابقة";
            this.lblPreviousSchool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblPreviousClass.AutoSize = false;
            this.lblPreviousClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPreviousClass.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPreviousClass.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblPreviousClass.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblPreviousClass.Text = "الصف السابق";
            this.lblPreviousClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblTransferReason.AutoSize = false;
            this.lblTransferReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransferReason.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblTransferReason.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblTransferReason.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblTransferReason.Text = "سبب النقل";
            this.lblTransferReason.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblRegistrationFee.AutoSize = false;
            this.lblRegistrationFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRegistrationFee.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblRegistrationFee.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblRegistrationFee.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblRegistrationFee.Text = "رسوم التسجيل";
            this.lblRegistrationFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblPaidAmount.AutoSize = false;
            this.lblPaidAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaidAmount.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPaidAmount.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblPaidAmount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblPaidAmount.Text = "المبلغ المدفوع";
            this.lblPaidAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblRemainingAmount.AutoSize = false;
            this.lblRemainingAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemainingAmount.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblRemainingAmount.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblRemainingAmount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblRemainingAmount.Text = "المبلغ المتبقي";
            this.lblRemainingAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblPaymentMethod.AutoSize = false;
            this.lblPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPaymentMethod.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblPaymentMethod.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblPaymentMethod.Text = "طريقة الدفع";
            this.lblPaymentMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblReceiptNo.AutoSize = false;
            this.lblReceiptNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReceiptNo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblReceiptNo.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblReceiptNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblReceiptNo.Text = "رقم السند";
            this.lblReceiptNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;


            this.gbPrevious.BackColor = System.Drawing.Color.White;
            this.gbPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPrevious.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbPrevious.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbPrevious.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.gbPrevious.Padding = new System.Windows.Forms.Padding(9, 24, 9, 8);
            this.gbPrevious.Text = "بيانات المدرسة السابقة";
            this.gbPrevious.Controls.Add(this.tlpPrevious);

            this.tlpPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPrevious.ColumnCount = 4;
            this.tlpPrevious.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlpPrevious.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPrevious.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlpPrevious.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPrevious.RowCount = 3;
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpPrevious.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPrevious.Padding = new System.Windows.Forms.Padding(0);
            this.tlpPrevious.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

            this.tlpPrevious.Controls.Add(this.lblPreviousSchool, 0, 0);
            this.tlpPrevious.Controls.Add(this.txtPreviousSchool, 1, 0);
            this.tlpPrevious.Controls.Add(this.lblPreviousClass, 2, 0);
            this.tlpPrevious.Controls.Add(this.txtPreviousClass, 3, 0);
            this.tlpPrevious.Controls.Add(this.lblTransferReason, 0, 1);
            this.tlpPrevious.Controls.Add(this.txtTransferReason, 1, 1);

            this.tlpPrevious.SetColumnSpan(this.txtTransferReason, 3);
            this.txtPreviousSchool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPreviousClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTransferReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPreviousSchool.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.txtPreviousClass.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.txtTransferReason.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.txtPreviousSchool.MaxLength = 150;
            this.txtPreviousClass.MaxLength = 100;
            this.txtTransferReason.MaxLength = 250;

            // Fees
            this.gbFees.BackColor = System.Drawing.Color.White;
            this.gbFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFees.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbFees.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbFees.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.gbFees.Padding = new System.Windows.Forms.Padding(9, 24, 9, 8);
            this.gbFees.Text = "الرسوم والسداد";
            this.gbFees.Controls.Add(this.tlpFees);

            this.tlpFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFees.ColumnCount = 4;
            this.tlpFees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlpFees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlpFees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFees.RowCount = 3;
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpFees.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFees.Padding = new System.Windows.Forms.Padding(0);
            this.tlpFees.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

            this.tlpFees.Controls.Add(this.lblRegistrationFee, 0, 0);
            this.tlpFees.Controls.Add(this.txtRegistrationFee, 1, 0);
            this.tlpFees.Controls.Add(this.lblPaidAmount, 2, 0);
            this.tlpFees.Controls.Add(this.txtPaidAmount, 3, 0);
            this.tlpFees.Controls.Add(this.lblRemainingAmount, 0, 1);
            this.tlpFees.Controls.Add(this.txtRemainingAmount, 1, 1);
            this.tlpFees.Controls.Add(this.lblPaymentMethod, 2, 1);
            this.tlpFees.Controls.Add(this.cmbPaymentMethod, 3, 1);
            this.tlpFees.Controls.Add(this.lblReceiptNo, 0, 2);
            this.tlpFees.Controls.Add(this.txtReceiptNo, 1, 2);

            this.tlpFees.SetColumnSpan(this.txtReceiptNo, 3);
            this.txtRegistrationFee.ReadOnly = true;
            this.txtRegistrationFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRegistrationFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRegistrationFee.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.txtPaidAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPaidAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemainingAmount.ReadOnly = true;
            this.txtRemainingAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRemainingAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemainingAmount.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPaymentMethod.IntegralHeight = false;
            this.txtReceiptNo.ReadOnly = true;
            this.txtReceiptNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtReceiptNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReceiptNo.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.txtRegistrationFee.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.txtPaidAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.txtRemainingAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.cmbPaymentMethod.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.txtReceiptNo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);

            // Attachments
            this.gbAttachments.BackColor = System.Drawing.Color.White;
            this.gbAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAttachments.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbAttachments.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbAttachments.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.gbAttachments.Padding = new System.Windows.Forms.Padding(9, 24, 9, 7);
            this.gbAttachments.Text = "المرفقات المطلوبة";
            this.gbAttachments.Controls.Add(this.flpAttachments);

            this.flpAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpAttachments.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpAttachments.WrapContents = true;
            this.flpAttachments.AutoScroll = false;
            this.flpAttachments.Padding = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.flpAttachments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flpAttachments.Controls.Add(this.chkHasBirthCertificate);
            this.flpAttachments.Controls.Add(this.chkHasGuardianId);
            this.flpAttachments.Controls.Add(this.chkHasPhoto);
            this.flpAttachments.Controls.Add(this.chkHasLastCertificate);
            this.flpAttachments.Controls.Add(this.chkHasMedicalReport);

            this.chkHasBirthCertificate.AutoSize = true;
            this.chkHasBirthCertificate.Text = "شهادة الميلاد";
            this.chkHasGuardianId.AutoSize = true;
            this.chkHasGuardianId.Text = "هوية ولي الأمر";
            this.chkHasPhoto.AutoSize = true;
            this.chkHasPhoto.Text = "صورة شخصية";
            this.chkHasLastCertificate.AutoSize = true;
            this.chkHasLastCertificate.Text = "آخر شهادة";
            this.chkHasMedicalReport.AutoSize = true;
            this.chkHasMedicalReport.Text = "تقرير طبي";
            this.chkHasBirthCertificate.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.chkHasGuardianId.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.chkHasPhoto.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.chkHasLastCertificate.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.chkHasMedicalReport.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);

            // Notes
            this.gbNotes.BackColor = System.Drawing.Color.White;
            this.gbNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbNotes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbNotes.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbNotes.Margin = new System.Windows.Forms.Padding(0);
            this.gbNotes.Padding = new System.Windows.Forms.Padding(9, 24, 9, 8);
            this.gbNotes.Text = "الملاحظات";
            this.gbNotes.Controls.Add(this.rtbNotes);

            this.rtbNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbNotes.BackColor = System.Drawing.Color.White;
            this.rtbNotes.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.rtbNotes.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rtbNotes.Margin = new System.Windows.Forms.Padding(0);
            this.rtbNotes.MaxLength = 1000;

            this.layoutRight.Controls.Add(this.gbBasic, 0, 0);
            this.layoutRight.Controls.Add(this.gbPrevious, 0, 1);
            this.layoutRight.Controls.Add(this.gbFees, 0, 2);
            this.layoutRight.Controls.Add(this.gbAttachments, 0, 3);
            this.layoutRight.Controls.Add(this.gbNotes, 0, 4);

            this.splitContainerMain.Panel1.Controls.Add(this.layoutRight);

            // Right search
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlRight.Padding = new System.Windows.Forms.Padding(0);
            this.pnlRight.Controls.Add(this.dgvEnrollments);
            this.pnlRight.Controls.Add(this.pnlSearch);

            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Height = 60;
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(7);
            this.pnlSearch.Controls.Add(this.tlpSearch);

            this.tlpSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearch.ColumnCount = 4;
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpSearch.RowCount = 1;
            this.tlpSearch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearch.Margin = new System.Windows.Forms.Padding(0);

            this.tlpSearch.Controls.Add(this.lblCount, 0, 0);
            this.tlpSearch.Controls.Add(this.btnReload, 1, 0);
            this.tlpSearch.Controls.Add(this.btnSearch, 2, 0);
            this.tlpSearch.Controls.Add(this.txtSearch, 3, 0);

            this.lblCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblCount.Text = "العدد: 0";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSearch.MaxLength = 120;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);

            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearch.Text = "بحث";
            this.btnReload.Text = "مسح الفلتر";

            // Grid
            this.dgvEnrollments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEnrollments.BackgroundColor = System.Drawing.Color.White;
            this.dgvEnrollments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEnrollments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvEnrollments.AllowUserToAddRows = false;
            this.dgvEnrollments.AllowUserToDeleteRows = false;
            this.dgvEnrollments.AllowUserToResizeRows = false;
            this.dgvEnrollments.MultiSelect = false;
            this.dgvEnrollments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEnrollments.ReadOnly = true;
            this.dgvEnrollments.RowHeadersVisible = false;
            this.dgvEnrollments.AutoGenerateColumns = true;
            this.dgvEnrollments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEnrollments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.dgvEnrollments.RowTemplate.Height = 38;
            this.dgvEnrollments.ColumnHeadersHeight = 42;
            this.dgvEnrollments.EnableHeadersVisualStyles = false;
            this.dgvEnrollments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvEnrollments.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvEnrollments.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvEnrollments.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dgvEnrollments.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvEnrollments.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.dgvEnrollments.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvEnrollments.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvEnrollments.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvEnrollments.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvEnrollments.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dgvEnrollments.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvEnrollments.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dgvEnrollments.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvEnrollments.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvEnrollments.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvEnrollments.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvEnrollments.GridColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dgvEnrollments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvEnrollments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEnrollments_CellClick);
            this.dgvEnrollments.SelectionChanged += new System.EventHandler(this.dgvEnrollments_SelectionChanged);

            // Search/button event wiring
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnReload.Click += new System.EventHandler(this.btnRefresh_Click);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.cmbStudentID.SelectedIndexChanged += new System.EventHandler(this.cmbStudentID_SelectedIndexChanged);
            this.dtpApplicationDate.ValueChanged += new System.EventHandler(this.dtpApplicationDate_ValueChanged);
            this.cmbClassID.SelectedIndexChanged += new System.EventHandler(this.cmbClassID_SelectedIndexChanged);
            this.txtAcademicYear.Leave += new System.EventHandler(this.txtAcademicYear_Leave);
            this.txtRegistrationFee.TextChanged += new System.EventHandler(this.txtFees_TextChanged);
            this.txtPaidAmount.TextChanged += new System.EventHandler(this.txtFees_TextChanged);

            // Action bar
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 110;
            this.pnlButtons.BackColor = System.Drawing.Color.White;
            this.pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlButtons.WrapContents = true;
            this.pnlButtons.AutoScroll = false;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(6);
            this.pnlButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

            this.btnAdd.Text = "إضافة تسجيل جديد";
            this.btnAdd.AutoSize = false;
            this.btnAdd.Size = new System.Drawing.Size(135, 38);
            this.btnAdd.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAdd.UseCompatibleTextRendering = true;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Text = "حفظ التسجيل";
            this.btnSave.AutoSize = false;
            this.btnSave.Size = new System.Drawing.Size(135, 38);
            this.btnSave.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSave.UseCompatibleTextRendering = true;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Text = "تعديل التسجيل";
            this.btnUpdate.AutoSize = false;
            this.btnUpdate.Size = new System.Drawing.Size(135, 38);
            this.btnUpdate.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.UseCompatibleTextRendering = true;
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Text = "حذف التسجيل";
            this.btnDelete.AutoSize = false;
            this.btnDelete.Size = new System.Drawing.Size(135, 38);
            this.btnDelete.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDelete.UseCompatibleTextRendering = true;
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Text = "إلغاء التعديل";
            this.btnCancel.AutoSize = false;
            this.btnCancel.Size = new System.Drawing.Size(135, 38);
            this.btnCancel.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCancel.UseCompatibleTextRendering = true;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Text = "تحديث القائمة";
            this.btnRefresh.AutoSize = false;
            this.btnRefresh.Size = new System.Drawing.Size(135, 38);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.UseCompatibleTextRendering = true;
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrintForm.Text = "طباعة استمارة التسجيل";
            this.btnPrintForm.AutoSize = false;
            this.btnPrintForm.Size = new System.Drawing.Size(135, 38);
            this.btnPrintForm.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnPrintForm.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintForm.FlatAppearance.BorderSize = 0;
            this.btnPrintForm.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPrintForm.ForeColor = System.Drawing.Color.White;
            this.btnPrintForm.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPrintForm.UseCompatibleTextRendering = true;
            this.btnPrintForm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrintForm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrintReceipt.Text = "طباعة الإيصال";
            this.btnPrintReceipt.AutoSize = false;
            this.btnPrintReceipt.Size = new System.Drawing.Size(135, 38);
            this.btnPrintReceipt.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnPrintReceipt.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintReceipt.FlatAppearance.BorderSize = 0;
            this.btnPrintReceipt.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPrintReceipt.ForeColor = System.Drawing.Color.White;
            this.btnPrintReceipt.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPrintReceipt.UseCompatibleTextRendering = true;
            this.btnPrintReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrintReceipt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPreviewOutput.Text = "معاينة المخرجات";
            this.btnPreviewOutput.AutoSize = false;
            this.btnPreviewOutput.Size = new System.Drawing.Size(135, 38);
            this.btnPreviewOutput.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnPreviewOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreviewOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviewOutput.FlatAppearance.BorderSize = 0;
            this.btnPreviewOutput.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPreviewOutput.ForeColor = System.Drawing.Color.White;
            this.btnPreviewOutput.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPreviewOutput.UseCompatibleTextRendering = true;
            this.btnPreviewOutput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPreviewOutput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportPdf.Text = "تصدير إلى PDF";
            this.btnExportPdf.AutoSize = false;
            this.btnExportPdf.Size = new System.Drawing.Size(135, 38);
            this.btnExportPdf.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnExportPdf.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPdf.FlatAppearance.BorderSize = 0;
            this.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnExportPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportPdf.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportPdf.UseCompatibleTextRendering = true;
            this.btnExportPdf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportPdf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportExcel.Text = "تصدير إلى Excel";
            this.btnExportExcel.AutoSize = false;
            this.btnExportExcel.Size = new System.Drawing.Size(135, 38);
            this.btnExportExcel.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.UseCompatibleTextRendering = true;
            this.btnExportExcel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Text = "إغلاق";
            this.btnClose.AutoSize = false;
            this.btnClose.Size = new System.Drawing.Size(135, 38);
            this.btnClose.MinimumSize = new System.Drawing.Size(135, 38);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;

            this.pnlButtons.Controls.Add(this.btnAdd);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnUpdate);
            this.pnlButtons.Controls.Add(this.btnDelete);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnRefresh);
            this.pnlButtons.Controls.Add(this.btnPrintForm);
            this.pnlButtons.Controls.Add(this.btnPrintReceipt);
            this.pnlButtons.Controls.Add(this.btnPreviewOutput);
            this.pnlButtons.Controls.Add(this.btnExportPdf);
            this.pnlButtons.Controls.Add(this.btnExportExcel);
            this.pnlButtons.Controls.Add(this.btnClose);

            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnPrintReceipt.Click += new System.EventHandler(this.btnPrintReceipt_Click);
            this.btnPrintForm.Click += new System.EventHandler(this.btnPrintForm_Click);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // Error provider
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;

            // Add main controls
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.pnlButtons);

            // Resume
            this.pnlButtons.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.tlpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.gbNotes.ResumeLayout(false);
            this.flpAttachments.ResumeLayout(false);
            this.tlpFees.ResumeLayout(false);
            this.gbFees.ResumeLayout(false);
            this.tlpPrevious.ResumeLayout(false);
            this.gbPrevious.ResumeLayout(false);
            this.tlpBasic.ResumeLayout(false);
            this.gbBasic.ResumeLayout(false);
            this.layoutRight.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.PerformLayout();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // WinForms controls
        private System.Windows.Forms.Label lblEnrollmentID;
        private System.Windows.Forms.TextBox txtEnrollmentID;
        private System.Windows.Forms.Label lblStudentID;
        private System.Windows.Forms.ComboBox cmbStudentID;
        private System.Windows.Forms.Label lblStudentName;
        private System.Windows.Forms.TextBox txtStudentName;
        private System.Windows.Forms.Label lblApplicationDate;
        private System.Windows.Forms.DateTimePicker dtpApplicationDate;
        private System.Windows.Forms.Label lblApplicationType;
        private System.Windows.Forms.ComboBox cmbApplicationType;
        private System.Windows.Forms.Label lblAcademicYear;
        private System.Windows.Forms.TextBox txtAcademicYear;
        private System.Windows.Forms.Label lblClassID;
        private System.Windows.Forms.ComboBox cmbClassID;
        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.ComboBox txtSection;
        private System.Windows.Forms.Label lblSeatNumber;
        private System.Windows.Forms.TextBox txtSeatNumber;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblPreviousSchool;
        private System.Windows.Forms.TextBox txtPreviousSchool;
        private System.Windows.Forms.Label lblPreviousClass;
        private System.Windows.Forms.TextBox txtPreviousClass;
        private System.Windows.Forms.Label lblTransferReason;
        private System.Windows.Forms.TextBox txtTransferReason;
        private System.Windows.Forms.Label lblRegistrationFee;
        private System.Windows.Forms.TextBox txtRegistrationFee;
        private System.Windows.Forms.Label lblPaidAmount;
        private System.Windows.Forms.TextBox txtPaidAmount;
        private System.Windows.Forms.Label lblRemainingAmount;
        private System.Windows.Forms.TextBox txtRemainingAmount;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblReceiptNo;
        private System.Windows.Forms.TextBox txtReceiptNo;
        private System.Windows.Forms.CheckBox chkHasBirthCertificate;
        private System.Windows.Forms.CheckBox chkHasGuardianId;
        private System.Windows.Forms.CheckBox chkHasPhoto;
        private System.Windows.Forms.CheckBox chkHasLastCertificate;
        private System.Windows.Forms.CheckBox chkHasMedicalReport;
        private System.Windows.Forms.GroupBox gbBasic;
        private System.Windows.Forms.TableLayoutPanel tlpBasic;
        private System.Windows.Forms.GroupBox gbPrevious;
        private System.Windows.Forms.TableLayoutPanel tlpPrevious;
        private System.Windows.Forms.GroupBox gbFees;
        private System.Windows.Forms.TableLayoutPanel tlpFees;
        private System.Windows.Forms.GroupBox gbAttachments;
        private System.Windows.Forms.FlowLayoutPanel flpAttachments;
        private System.Windows.Forms.GroupBox gbNotes;
        private System.Windows.Forms.RichTextBox rtbNotes;
        private System.Windows.Forms.TableLayoutPanel layoutRight;
        private System.Windows.Forms.DataGridView dgvEnrollments;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.FlowLayoutPanel pnlButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnPreviewOutput;
        private System.Windows.Forms.Button btnPrintReceipt;
        private System.Windows.Forms.Button btnPrintForm;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;

    }
}
