namespace SchoolSystem.UI
{
    partial class EnrollmentForm
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.lblSearch = new System.Windows.Forms.Label();
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
            this.gbBasic.SuspendLayout();
            this.tlpBasic.SuspendLayout();
            this.gbPrevious.SuspendLayout();
            this.tlpPrevious.SuspendLayout();
            this.gbFees.SuspendLayout();
            this.tlpFees.SuspendLayout();
            this.gbAttachments.SuspendLayout();
            this.flpAttachments.SuspendLayout();
            this.gbNotes.SuspendLayout();
            this.layoutRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEnrollmentID
            // 
            this.lblEnrollmentID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnrollmentID.Location = new System.Drawing.Point(359, 3);
            this.lblEnrollmentID.Margin = new System.Windows.Forms.Padding(2);
            this.lblEnrollmentID.Name = "lblEnrollmentID";
            this.lblEnrollmentID.Size = new System.Drawing.Size(104, 25);
            this.lblEnrollmentID.TabIndex = 0;
            this.lblEnrollmentID.Text = "رقم الطلب";
            this.lblEnrollmentID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEnrollmentID
            // 
            this.txtEnrollmentID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEnrollmentID.Location = new System.Drawing.Point(3, 3);
            this.txtEnrollmentID.Name = "txtEnrollmentID";
            this.txtEnrollmentID.ReadOnly = true;
            this.txtEnrollmentID.Size = new System.Drawing.Size(372, 26);
            this.txtEnrollmentID.TabIndex = 1;
            // 
            // lblStudentID
            // 
            this.lblStudentID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStudentID.Location = new System.Drawing.Point(359, 34);
            this.lblStudentID.Margin = new System.Windows.Forms.Padding(2);
            this.lblStudentID.Name = "lblStudentID";
            this.lblStudentID.Size = new System.Drawing.Size(104, 25);
            this.lblStudentID.TabIndex = 2;
            this.lblStudentID.Text = "اختيار الطالب";
            this.lblStudentID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStudentID
            // 
            this.cmbStudentID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStudentID.Location = new System.Drawing.Point(3, 34);
            this.cmbStudentID.Name = "cmbStudentID";
            this.cmbStudentID.Size = new System.Drawing.Size(372, 26);
            this.cmbStudentID.TabIndex = 3;
            this.cmbStudentID.SelectedIndexChanged += new System.EventHandler(this.cmbStudentID_SelectedIndexChanged);
            // 
            // lblStudentName
            // 
            this.lblStudentName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStudentName.Location = new System.Drawing.Point(359, 65);
            this.lblStudentName.Margin = new System.Windows.Forms.Padding(2);
            this.lblStudentName.Name = "lblStudentName";
            this.lblStudentName.Size = new System.Drawing.Size(104, 25);
            this.lblStudentName.TabIndex = 4;
            this.lblStudentName.Text = "اسم الطالب";
            this.lblStudentName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStudentName
            // 
            this.txtStudentName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStudentName.Location = new System.Drawing.Point(3, 65);
            this.txtStudentName.Name = "txtStudentName";
            this.txtStudentName.ReadOnly = true;
            this.txtStudentName.Size = new System.Drawing.Size(372, 26);
            this.txtStudentName.TabIndex = 5;
            // 
            // lblApplicationDate
            // 
            this.lblApplicationDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApplicationDate.Location = new System.Drawing.Point(359, 96);
            this.lblApplicationDate.Margin = new System.Windows.Forms.Padding(2);
            this.lblApplicationDate.Name = "lblApplicationDate";
            this.lblApplicationDate.Size = new System.Drawing.Size(104, 25);
            this.lblApplicationDate.TabIndex = 6;
            this.lblApplicationDate.Text = "تاريخ التسجيل";
            this.lblApplicationDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpApplicationDate
            // 
            this.dtpApplicationDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpApplicationDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpApplicationDate.Location = new System.Drawing.Point(3, 96);
            this.dtpApplicationDate.Name = "dtpApplicationDate";
            this.dtpApplicationDate.Size = new System.Drawing.Size(372, 26);
            this.dtpApplicationDate.TabIndex = 7;
            // 
            // lblApplicationType
            // 
            this.lblApplicationType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApplicationType.Location = new System.Drawing.Point(359, 127);
            this.lblApplicationType.Margin = new System.Windows.Forms.Padding(2);
            this.lblApplicationType.Name = "lblApplicationType";
            this.lblApplicationType.Size = new System.Drawing.Size(104, 25);
            this.lblApplicationType.TabIndex = 8;
            this.lblApplicationType.Text = "نوع الطلب";
            this.lblApplicationType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbApplicationType
            // 
            this.cmbApplicationType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbApplicationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbApplicationType.FormattingEnabled = true;
            this.cmbApplicationType.Location = new System.Drawing.Point(3, 127);
            this.cmbApplicationType.Name = "cmbApplicationType";
            this.cmbApplicationType.Size = new System.Drawing.Size(372, 26);
            this.cmbApplicationType.TabIndex = 9;
            // 
            // lblAcademicYear
            // 
            this.lblAcademicYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAcademicYear.Location = new System.Drawing.Point(359, 158);
            this.lblAcademicYear.Margin = new System.Windows.Forms.Padding(2);
            this.lblAcademicYear.Name = "lblAcademicYear";
            this.lblAcademicYear.Size = new System.Drawing.Size(104, 25);
            this.lblAcademicYear.TabIndex = 10;
            this.lblAcademicYear.Text = "العام الدراسي";
            this.lblAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAcademicYear
            // 
            this.txtAcademicYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAcademicYear.Location = new System.Drawing.Point(3, 158);
            this.txtAcademicYear.Name = "txtAcademicYear";
            this.txtAcademicYear.Size = new System.Drawing.Size(372, 26);
            this.txtAcademicYear.TabIndex = 11;
            // 
            // lblClassID
            // 
            this.lblClassID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClassID.Location = new System.Drawing.Point(359, 189);
            this.lblClassID.Margin = new System.Windows.Forms.Padding(2);
            this.lblClassID.Name = "lblClassID";
            this.lblClassID.Size = new System.Drawing.Size(104, 25);
            this.lblClassID.TabIndex = 12;
            this.lblClassID.Text = "الصف";
            this.lblClassID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbClassID
            // 
            this.cmbClassID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbClassID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClassID.FormattingEnabled = true;
            this.cmbClassID.Location = new System.Drawing.Point(3, 189);
            this.cmbClassID.Name = "cmbClassID";
            this.cmbClassID.Size = new System.Drawing.Size(372, 26);
            this.cmbClassID.TabIndex = 13;
            this.cmbClassID.SelectedIndexChanged += new System.EventHandler(this.cmbClassID_SelectedIndexChanged);
            // 
            // lblSection
            // 
            this.lblSection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSection.Location = new System.Drawing.Point(359, 220);
            this.lblSection.Margin = new System.Windows.Forms.Padding(2);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(104, 25);
            this.lblSection.TabIndex = 14;
            this.lblSection.Text = "الشعبة";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSection
            // 
            this.txtSection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtSection.FormattingEnabled = true;
            this.txtSection.Location = new System.Drawing.Point(3, 220);
            this.txtSection.Name = "txtSection";
            this.txtSection.Size = new System.Drawing.Size(372, 26);
            this.txtSection.TabIndex = 15;
            // 
            // lblSeatNumber
            // 
            this.lblSeatNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSeatNumber.Location = new System.Drawing.Point(359, 251);
            this.lblSeatNumber.Margin = new System.Windows.Forms.Padding(2);
            this.lblSeatNumber.Name = "lblSeatNumber";
            this.lblSeatNumber.Size = new System.Drawing.Size(104, 25);
            this.lblSeatNumber.TabIndex = 16;
            this.lblSeatNumber.Text = "رقم الجلوس";
            this.lblSeatNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSeatNumber
            // 
            this.txtSeatNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSeatNumber.Location = new System.Drawing.Point(3, 251);
            this.txtSeatNumber.Name = "txtSeatNumber";
            this.txtSeatNumber.Size = new System.Drawing.Size(372, 26);
            this.txtSeatNumber.TabIndex = 17;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(359, 282);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(104, 25);
            this.lblStatus.TabIndex = 18;
            this.lblStatus.Text = "حالة الطلب";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(3, 282);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(372, 26);
            this.cmbStatus.TabIndex = 19;
            // 
            // lblPreviousSchool
            // 
            this.lblPreviousSchool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPreviousSchool.Location = new System.Drawing.Point(359, 3);
            this.lblPreviousSchool.Margin = new System.Windows.Forms.Padding(2);
            this.lblPreviousSchool.Name = "lblPreviousSchool";
            this.lblPreviousSchool.Size = new System.Drawing.Size(104, 25);
            this.lblPreviousSchool.TabIndex = 0;
            this.lblPreviousSchool.Text = "اسم المدرسة";
            this.lblPreviousSchool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPreviousSchool
            // 
            this.txtPreviousSchool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousSchool.Location = new System.Drawing.Point(3, 3);
            this.txtPreviousSchool.Name = "txtPreviousSchool";
            this.txtPreviousSchool.Size = new System.Drawing.Size(372, 26);
            this.txtPreviousSchool.TabIndex = 1;
            // 
            // lblPreviousClass
            // 
            this.lblPreviousClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPreviousClass.Location = new System.Drawing.Point(359, 34);
            this.lblPreviousClass.Margin = new System.Windows.Forms.Padding(2);
            this.lblPreviousClass.Name = "lblPreviousClass";
            this.lblPreviousClass.Size = new System.Drawing.Size(104, 25);
            this.lblPreviousClass.TabIndex = 2;
            this.lblPreviousClass.Text = "الصف السابق";
            this.lblPreviousClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPreviousClass
            // 
            this.txtPreviousClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousClass.Location = new System.Drawing.Point(3, 34);
            this.txtPreviousClass.Name = "txtPreviousClass";
            this.txtPreviousClass.Size = new System.Drawing.Size(372, 26);
            this.txtPreviousClass.TabIndex = 3;
            // 
            // lblTransferReason
            // 
            this.lblTransferReason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransferReason.Location = new System.Drawing.Point(359, 66);
            this.lblTransferReason.Margin = new System.Windows.Forms.Padding(2);
            this.lblTransferReason.Name = "lblTransferReason";
            this.lblTransferReason.Size = new System.Drawing.Size(104, 25);
            this.lblTransferReason.TabIndex = 4;
            this.lblTransferReason.Text = "سبب النقل";
            this.lblTransferReason.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTransferReason
            // 
            this.txtTransferReason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTransferReason.Location = new System.Drawing.Point(3, 66);
            this.txtTransferReason.Name = "txtTransferReason";
            this.txtTransferReason.Size = new System.Drawing.Size(372, 26);
            this.txtTransferReason.TabIndex = 5;
            // 
            // lblRegistrationFee
            // 
            this.lblRegistrationFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRegistrationFee.Location = new System.Drawing.Point(359, 3);
            this.lblRegistrationFee.Margin = new System.Windows.Forms.Padding(2);
            this.lblRegistrationFee.Name = "lblRegistrationFee";
            this.lblRegistrationFee.Size = new System.Drawing.Size(104, 25);
            this.lblRegistrationFee.TabIndex = 0;
            this.lblRegistrationFee.Text = "رسوم التسجيل";
            this.lblRegistrationFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRegistrationFee
            // 
            this.txtRegistrationFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegistrationFee.Location = new System.Drawing.Point(3, 3);
            this.txtRegistrationFee.Name = "txtRegistrationFee";
            this.txtRegistrationFee.Size = new System.Drawing.Size(372, 26);
            this.txtRegistrationFee.TabIndex = 1;
            this.txtRegistrationFee.TextChanged += new System.EventHandler(this.txtFees_TextChanged);
            // 
            // lblPaidAmount
            // 
            this.lblPaidAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPaidAmount.Location = new System.Drawing.Point(359, 34);
            this.lblPaidAmount.Margin = new System.Windows.Forms.Padding(2);
            this.lblPaidAmount.Name = "lblPaidAmount";
            this.lblPaidAmount.Size = new System.Drawing.Size(104, 25);
            this.lblPaidAmount.TabIndex = 2;
            this.lblPaidAmount.Text = "المبلغ المدفوع";
            this.lblPaidAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPaidAmount
            // 
            this.txtPaidAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPaidAmount.Location = new System.Drawing.Point(3, 34);
            this.txtPaidAmount.Name = "txtPaidAmount";
            this.txtPaidAmount.Size = new System.Drawing.Size(372, 26);
            this.txtPaidAmount.TabIndex = 3;
            this.txtPaidAmount.TextChanged += new System.EventHandler(this.txtFees_TextChanged);
            // 
            // lblRemainingAmount
            // 
            this.lblRemainingAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemainingAmount.Location = new System.Drawing.Point(359, 65);
            this.lblRemainingAmount.Margin = new System.Windows.Forms.Padding(2);
            this.lblRemainingAmount.Name = "lblRemainingAmount";
            this.lblRemainingAmount.Size = new System.Drawing.Size(104, 25);
            this.lblRemainingAmount.TabIndex = 4;
            this.lblRemainingAmount.Text = "المبلغ المتبقي";
            this.lblRemainingAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRemainingAmount
            // 
            this.txtRemainingAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemainingAmount.Location = new System.Drawing.Point(3, 65);
            this.txtRemainingAmount.Name = "txtRemainingAmount";
            this.txtRemainingAmount.ReadOnly = true;
            this.txtRemainingAmount.Size = new System.Drawing.Size(372, 26);
            this.txtRemainingAmount.TabIndex = 5;
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPaymentMethod.Location = new System.Drawing.Point(359, 96);
            this.lblPaymentMethod.Margin = new System.Windows.Forms.Padding(2);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(104, 25);
            this.lblPaymentMethod.TabIndex = 6;
            this.lblPaymentMethod.Text = "طريقة الدفع";
            this.lblPaymentMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.FormattingEnabled = true;
            this.cmbPaymentMethod.Location = new System.Drawing.Point(3, 96);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(372, 26);
            this.cmbPaymentMethod.TabIndex = 7;
            // 
            // lblReceiptNo
            // 
            this.lblReceiptNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReceiptNo.Location = new System.Drawing.Point(359, 142);
            this.lblReceiptNo.Margin = new System.Windows.Forms.Padding(2);
            this.lblReceiptNo.Name = "lblReceiptNo";
            this.lblReceiptNo.Size = new System.Drawing.Size(104, 25);
            this.lblReceiptNo.TabIndex = 8;
            this.lblReceiptNo.Text = "رقم السند";
            this.lblReceiptNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReceiptNo
            // 
            this.txtReceiptNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceiptNo.Location = new System.Drawing.Point(3, 142);
            this.txtReceiptNo.Name = "txtReceiptNo";
            this.txtReceiptNo.ReadOnly = true;
            this.txtReceiptNo.Size = new System.Drawing.Size(372, 26);
            this.txtReceiptNo.TabIndex = 9;
            // 
            // chkHasBirthCertificate
            // 
            this.chkHasBirthCertificate.AutoSize = true;
            this.chkHasBirthCertificate.Location = new System.Drawing.Point(4, 4);
            this.chkHasBirthCertificate.Margin = new System.Windows.Forms.Padding(4);
            this.chkHasBirthCertificate.Name = "chkHasBirthCertificate";
            this.chkHasBirthCertificate.Padding = new System.Windows.Forms.Padding(4);
            this.chkHasBirthCertificate.Size = new System.Drawing.Size(124, 30);
            this.chkHasBirthCertificate.TabIndex = 0;
            this.chkHasBirthCertificate.Text = "شهادة الميلاد";
            this.chkHasBirthCertificate.UseVisualStyleBackColor = true;
            // 
            // chkHasGuardianId
            // 
            this.chkHasGuardianId.AutoSize = true;
            this.chkHasGuardianId.Location = new System.Drawing.Point(136, 4);
            this.chkHasGuardianId.Margin = new System.Windows.Forms.Padding(4);
            this.chkHasGuardianId.Name = "chkHasGuardianId";
            this.chkHasGuardianId.Padding = new System.Windows.Forms.Padding(4);
            this.chkHasGuardianId.Size = new System.Drawing.Size(130, 30);
            this.chkHasGuardianId.TabIndex = 1;
            this.chkHasGuardianId.Text = "هوية ولي الأمر";
            this.chkHasGuardianId.UseVisualStyleBackColor = true;
            // 
            // chkHasPhoto
            // 
            this.chkHasPhoto.AutoSize = true;
            this.chkHasPhoto.Location = new System.Drawing.Point(274, 4);
            this.chkHasPhoto.Margin = new System.Windows.Forms.Padding(4);
            this.chkHasPhoto.Name = "chkHasPhoto";
            this.chkHasPhoto.Padding = new System.Windows.Forms.Padding(4);
            this.chkHasPhoto.Size = new System.Drawing.Size(126, 30);
            this.chkHasPhoto.TabIndex = 2;
            this.chkHasPhoto.Text = "صورة شخصية";
            this.chkHasPhoto.UseVisualStyleBackColor = true;
            // 
            // chkHasLastCertificate
            // 
            this.chkHasLastCertificate.AutoSize = true;
            this.chkHasLastCertificate.Location = new System.Drawing.Point(4, 42);
            this.chkHasLastCertificate.Margin = new System.Windows.Forms.Padding(4);
            this.chkHasLastCertificate.Name = "chkHasLastCertificate";
            this.chkHasLastCertificate.Padding = new System.Windows.Forms.Padding(4);
            this.chkHasLastCertificate.Size = new System.Drawing.Size(104, 30);
            this.chkHasLastCertificate.TabIndex = 3;
            this.chkHasLastCertificate.Text = "آخر شهادة";
            this.chkHasLastCertificate.UseVisualStyleBackColor = true;
            // 
            // chkHasMedicalReport
            // 
            this.chkHasMedicalReport.AutoSize = true;
            this.chkHasMedicalReport.Location = new System.Drawing.Point(116, 42);
            this.chkHasMedicalReport.Margin = new System.Windows.Forms.Padding(4);
            this.chkHasMedicalReport.Name = "chkHasMedicalReport";
            this.chkHasMedicalReport.Padding = new System.Windows.Forms.Padding(4);
            this.chkHasMedicalReport.Size = new System.Drawing.Size(101, 30);
            this.chkHasMedicalReport.TabIndex = 4;
            this.chkHasMedicalReport.Text = "تقرير طبي";
            this.chkHasMedicalReport.UseVisualStyleBackColor = true;
            // 
            // gbBasic
            // 
            this.gbBasic.BackColor = System.Drawing.Color.White;
            this.gbBasic.Controls.Add(this.tlpBasic);
            this.gbBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBasic.Location = new System.Drawing.Point(0, 0);
            this.gbBasic.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Padding = new System.Windows.Forms.Padding(8, 20, 8, 5);
            this.gbBasic.Size = new System.Drawing.Size(481, 350);
            this.gbBasic.TabIndex = 0;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "بيانات التسجيل الأساسية";
            // 
            // tlpBasic
            // 
            this.tlpBasic.ColumnCount = 2;
            this.tlpBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tlpBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBasic.Controls.Add(this.lblEnrollmentID, 0, 0);
            this.tlpBasic.Controls.Add(this.txtEnrollmentID, 1, 0);
            this.tlpBasic.Controls.Add(this.lblStudentID, 0, 1);
            this.tlpBasic.Controls.Add(this.cmbStudentID, 1, 1);
            this.tlpBasic.Controls.Add(this.lblStudentName, 0, 2);
            this.tlpBasic.Controls.Add(this.txtStudentName, 1, 2);
            this.tlpBasic.Controls.Add(this.lblApplicationDate, 0, 3);
            this.tlpBasic.Controls.Add(this.dtpApplicationDate, 1, 3);
            this.tlpBasic.Controls.Add(this.lblApplicationType, 0, 4);
            this.tlpBasic.Controls.Add(this.cmbApplicationType, 1, 4);
            this.tlpBasic.Controls.Add(this.lblAcademicYear, 0, 5);
            this.tlpBasic.Controls.Add(this.txtAcademicYear, 1, 5);
            this.tlpBasic.Controls.Add(this.lblClassID, 0, 6);
            this.tlpBasic.Controls.Add(this.cmbClassID, 1, 6);
            this.tlpBasic.Controls.Add(this.lblSection, 0, 7);
            this.tlpBasic.Controls.Add(this.txtSection, 1, 7);
            this.tlpBasic.Controls.Add(this.lblSeatNumber, 0, 8);
            this.tlpBasic.Controls.Add(this.txtSeatNumber, 1, 8);
            this.tlpBasic.Controls.Add(this.lblStatus, 0, 9);
            this.tlpBasic.Controls.Add(this.cmbStatus, 1, 9);
            this.tlpBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBasic.Location = new System.Drawing.Point(8, 39);
            this.tlpBasic.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBasic.Name = "tlpBasic";
            this.tlpBasic.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpBasic.RowCount = 10;
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBasic.Size = new System.Drawing.Size(465, 306);
            this.tlpBasic.TabIndex = 0;
            // 
            // gbPrevious
            // 
            this.gbPrevious.BackColor = System.Drawing.Color.White;
            this.gbPrevious.Controls.Add(this.tlpPrevious);
            this.gbPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPrevious.Location = new System.Drawing.Point(0, 355);
            this.gbPrevious.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.gbPrevious.Name = "gbPrevious";
            this.gbPrevious.Padding = new System.Windows.Forms.Padding(8, 20, 8, 5);
            this.gbPrevious.Size = new System.Drawing.Size(481, 140);
            this.gbPrevious.TabIndex = 1;
            this.gbPrevious.TabStop = false;
            this.gbPrevious.Text = "المدرسة السابقة";
            // 
            // tlpPrevious
            // 
            this.tlpPrevious.ColumnCount = 2;
            this.tlpPrevious.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tlpPrevious.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPrevious.Controls.Add(this.lblPreviousSchool, 0, 0);
            this.tlpPrevious.Controls.Add(this.txtPreviousSchool, 1, 0);
            this.tlpPrevious.Controls.Add(this.lblPreviousClass, 0, 1);
            this.tlpPrevious.Controls.Add(this.txtPreviousClass, 1, 1);
            this.tlpPrevious.Controls.Add(this.lblTransferReason, 0, 2);
            this.tlpPrevious.Controls.Add(this.txtTransferReason, 1, 2);
            this.tlpPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPrevious.Location = new System.Drawing.Point(8, 39);
            this.tlpPrevious.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPrevious.Name = "tlpPrevious";
            this.tlpPrevious.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpPrevious.RowCount = 3;
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpPrevious.Size = new System.Drawing.Size(465, 96);
            this.tlpPrevious.TabIndex = 0;
            // 
            // gbFees
            // 
            this.gbFees.BackColor = System.Drawing.Color.White;
            this.gbFees.Controls.Add(this.tlpFees);
            this.gbFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFees.Location = new System.Drawing.Point(0, 500);
            this.gbFees.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.gbFees.Name = "gbFees";
            this.gbFees.Padding = new System.Windows.Forms.Padding(8, 20, 8, 5);
            this.gbFees.Size = new System.Drawing.Size(481, 230);
            this.gbFees.TabIndex = 2;
            this.gbFees.TabStop = false;
            this.gbFees.Text = "الرسوم";
            // 
            // tlpFees
            // 
            this.tlpFees.ColumnCount = 2;
            this.tlpFees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tlpFees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFees.Controls.Add(this.lblRegistrationFee, 0, 0);
            this.tlpFees.Controls.Add(this.txtRegistrationFee, 1, 0);
            this.tlpFees.Controls.Add(this.lblPaidAmount, 0, 1);
            this.tlpFees.Controls.Add(this.txtPaidAmount, 1, 1);
            this.tlpFees.Controls.Add(this.lblRemainingAmount, 0, 2);
            this.tlpFees.Controls.Add(this.txtRemainingAmount, 1, 2);
            this.tlpFees.Controls.Add(this.lblPaymentMethod, 0, 3);
            this.tlpFees.Controls.Add(this.cmbPaymentMethod, 1, 3);
            this.tlpFees.Controls.Add(this.lblReceiptNo, 0, 4);
            this.tlpFees.Controls.Add(this.txtReceiptNo, 1, 4);
            this.tlpFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFees.Location = new System.Drawing.Point(8, 39);
            this.tlpFees.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFees.Name = "tlpFees";
            this.tlpFees.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpFees.RowCount = 5;
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpFees.Size = new System.Drawing.Size(465, 186);
            this.tlpFees.TabIndex = 0;
            // 
            // gbAttachments
            // 
            this.gbAttachments.BackColor = System.Drawing.Color.White;
            this.gbAttachments.Controls.Add(this.flpAttachments);
            this.gbAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAttachments.Location = new System.Drawing.Point(0, 735);
            this.gbAttachments.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.gbAttachments.Name = "gbAttachments";
            this.gbAttachments.Padding = new System.Windows.Forms.Padding(8, 20, 8, 5);
            this.gbAttachments.Size = new System.Drawing.Size(481, 80);
            this.gbAttachments.TabIndex = 3;
            this.gbAttachments.TabStop = false;
            this.gbAttachments.Text = "المرفقات المطلوبة";
            // 
            // flpAttachments
            // 
            this.flpAttachments.AutoScroll = true;
            this.flpAttachments.Controls.Add(this.chkHasBirthCertificate);
            this.flpAttachments.Controls.Add(this.chkHasGuardianId);
            this.flpAttachments.Controls.Add(this.chkHasPhoto);
            this.flpAttachments.Controls.Add(this.chkHasLastCertificate);
            this.flpAttachments.Controls.Add(this.chkHasMedicalReport);
            this.flpAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpAttachments.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpAttachments.Location = new System.Drawing.Point(8, 39);
            this.flpAttachments.Name = "flpAttachments";
            this.flpAttachments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flpAttachments.Size = new System.Drawing.Size(465, 36);
            this.flpAttachments.TabIndex = 0;
            // 
            // gbNotes
            // 
            this.gbNotes.BackColor = System.Drawing.Color.White;
            this.gbNotes.Controls.Add(this.rtbNotes);
            this.gbNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbNotes.Location = new System.Drawing.Point(3, 823);
            this.gbNotes.Name = "gbNotes";
            this.gbNotes.Padding = new System.Windows.Forms.Padding(8, 20, 8, 8);
            this.gbNotes.Size = new System.Drawing.Size(475, 114);
            this.gbNotes.TabIndex = 4;
            this.gbNotes.TabStop = false;
            this.gbNotes.Text = "الملاحظات";
            // 
            // rtbNotes
            // 
            this.rtbNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbNotes.Location = new System.Drawing.Point(8, 39);
            this.rtbNotes.Margin = new System.Windows.Forms.Padding(0);
            this.rtbNotes.Name = "rtbNotes";
            this.rtbNotes.Size = new System.Drawing.Size(459, 67);
            this.rtbNotes.TabIndex = 0;
            this.rtbNotes.Text = "";
            // 
            // layoutRight
            // 
            this.layoutRight.ColumnCount = 1;
            this.layoutRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRight.Controls.Add(this.gbBasic, 0, 0);
            this.layoutRight.Controls.Add(this.gbPrevious, 0, 1);
            this.layoutRight.Controls.Add(this.gbFees, 0, 2);
            this.layoutRight.Controls.Add(this.gbAttachments, 0, 3);
            this.layoutRight.Controls.Add(this.gbNotes, 0, 4);
            this.layoutRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutRight.Location = new System.Drawing.Point(0, 0);
            this.layoutRight.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRight.Name = "layoutRight";
            this.layoutRight.RowCount = 5;
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 355F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 235F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.layoutRight.Size = new System.Drawing.Size(481, 940);
            this.layoutRight.TabIndex = 0;
            // 
            // dgvEnrollments
            // 
            this.dgvEnrollments.AllowUserToAddRows = false;
            this.dgvEnrollments.AllowUserToDeleteRows = false;
            this.dgvEnrollments.AllowUserToResizeRows = false;
            this.dgvEnrollments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEnrollments.BackgroundColor = System.Drawing.Color.White;
            this.dgvEnrollments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvEnrollments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvEnrollments.ColumnHeadersHeight = 40;
            this.dgvEnrollments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvEnrollments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEnrollments.EnableHeadersVisualStyles = false;
            this.dgvEnrollments.Location = new System.Drawing.Point(10, 10);
            this.dgvEnrollments.Margin = new System.Windows.Forms.Padding(0);
            this.dgvEnrollments.MultiSelect = false;
            this.dgvEnrollments.Name = "dgvEnrollments";
            this.dgvEnrollments.ReadOnly = true;
            this.dgvEnrollments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvEnrollments.RowHeadersWidth = 40;
            this.dgvEnrollments.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvEnrollments.RowTemplate.Height = 34;
            this.dgvEnrollments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEnrollments.Size = new System.Drawing.Size(652, 534);
            this.dgvEnrollments.TabIndex = 0;
            this.dgvEnrollments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEnrollments_CellClick);
            this.dgvEnrollments.SelectionChanged += new System.EventHandler(this.dgvEnrollments_SelectionChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 60);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.splitContainerMain.Panel1.Controls.Add(this.dgvEnrollments);
            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainerMain.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerMain.Panel1MinSize = 420;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.splitContainerMain.Panel2.Controls.Add(this.pnlRight);
            this.splitContainerMain.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainerMain.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerMain.Panel2MinSize = 420;
            this.splitContainerMain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainerMain.Size = new System.Drawing.Size(1200, 554);
            this.splitContainerMain.SplitterDistance = 672;
            this.splitContainerMain.SplitterWidth = 6;
            this.splitContainerMain.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlRight.Controls.Add(this.layoutRight);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(10, 10);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(502, 534);
            this.pnlRight.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.Controls.Add(this.tlpSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(8);
            this.pnlSearch.Size = new System.Drawing.Size(1200, 60);
            this.pnlSearch.TabIndex = 1;
            // 
            // tlpSearch
            // 
            this.tlpSearch.ColumnCount = 6;
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearch.Controls.Add(this.btnReload, 0, 0);
            this.tlpSearch.Controls.Add(this.btnSearch, 1, 0);
            this.tlpSearch.Controls.Add(this.txtSearch, 2, 0);
            this.tlpSearch.Controls.Add(this.lblSearch, 3, 0);
            this.tlpSearch.Controls.Add(this.lblCount, 5, 0);
            this.tlpSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearch.Location = new System.Drawing.Point(8, 8);
            this.tlpSearch.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSearch.Name = "tlpSearch";
            this.tlpSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpSearch.RowCount = 1;
            this.tlpSearch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearch.Size = new System.Drawing.Size(1184, 44);
            this.tlpSearch.TabIndex = 0;
            // 
            // btnReload
            // 
            this.btnReload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReload.Margin = new System.Windows.Forms.Padding(4);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(118, 36);
            this.btnReload.MinimumSize = new System.Drawing.Size(118, 36);
            this.btnReload.TabIndex = 2;
            this.btnReload.Text = "مسح الفلاتر";
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnReload.UseCompatibleTextRendering = true;
            this.btnReload.AutoEllipsis = false;
            this.btnReload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnReload.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnReload.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 36);
            this.btnSearch.MinimumSize = new System.Drawing.Size(110, 36);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "بحث";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSearch.UseCompatibleTextRendering = true;
            this.btnSearch.AutoEllipsis = false;
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSearch.TabIndex = 4;
            this.lblSearch.Text = "بحث:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCount
            // 
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.lblCount.Name = "lblCount";
            this.lblCount.TabIndex = 3;
            this.lblCount.Text = "العدد: 0";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlButtons
            // 
            this.pnlButtons.AutoScroll = true;
            this.pnlButtons.BackColor = System.Drawing.Color.White;
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnExportExcel);
            this.pnlButtons.Controls.Add(this.btnExportPdf);
            this.pnlButtons.Controls.Add(this.btnPreviewOutput);
            this.pnlButtons.Controls.Add(this.btnPrintReceipt);
            this.pnlButtons.Controls.Add(this.btnPrintForm);
            this.pnlButtons.Controls.Add(this.btnRefresh);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnDelete);
            this.pnlButtons.Controls.Add(this.btnUpdate);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnAdd);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlButtons.Location = new System.Drawing.Point(0, 614);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlButtons.Size = new System.Drawing.Size(1200, 136);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1041, 12);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(145, 40);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "إغلاق النافذة";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(888, 12);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(145, 40);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "تصدير إلى Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.UseCompatibleTextRendering = true;
            this.btnExportExcel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.Location = new System.Drawing.Point(735, 12);
            this.btnExportPdf.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(145, 40);
            this.btnExportPdf.TabIndex = 2;
            this.btnExportPdf.Text = "تصدير إلى PDF";
            this.btnExportPdf.UseVisualStyleBackColor = false;
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPdf.FlatAppearance.BorderSize = 0;
            this.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnExportPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportPdf.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportPdf.UseCompatibleTextRendering = true;
            this.btnExportPdf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPreviewOutput
            // 
            this.btnPreviewOutput.Location = new System.Drawing.Point(582, 12);
            this.btnPreviewOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreviewOutput.Name = "btnPreviewOutput";
            this.btnPreviewOutput.Size = new System.Drawing.Size(145, 40);
            this.btnPreviewOutput.TabIndex = 3;
            this.btnPreviewOutput.Text = "معاينة المخرجات";
            this.btnPreviewOutput.UseVisualStyleBackColor = false;
            this.btnPreviewOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviewOutput.FlatAppearance.BorderSize = 0;
            this.btnPreviewOutput.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPreviewOutput.ForeColor = System.Drawing.Color.White;
            this.btnPreviewOutput.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPreviewOutput.UseCompatibleTextRendering = true;
            this.btnPreviewOutput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPreviewOutput.Click += new System.EventHandler(this.btnPreviewOutput_Click);
            // 
            // btnPrintReceipt
            // 
            this.btnPrintReceipt.Location = new System.Drawing.Point(429, 12);
            this.btnPrintReceipt.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintReceipt.Name = "btnPrintReceipt";
            this.btnPrintReceipt.Size = new System.Drawing.Size(145, 40);
            this.btnPrintReceipt.TabIndex = 4;
            this.btnPrintReceipt.Text = "طباعة الإيصال";
            this.btnPrintReceipt.UseVisualStyleBackColor = false;
            this.btnPrintReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintReceipt.FlatAppearance.BorderSize = 0;
            this.btnPrintReceipt.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPrintReceipt.ForeColor = System.Drawing.Color.White;
            this.btnPrintReceipt.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPrintReceipt.UseCompatibleTextRendering = true;
            this.btnPrintReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrintReceipt.Click += new System.EventHandler(this.btnPrintReceipt_Click);
            // 
            // btnPrintForm
            // 
            this.btnPrintForm.Location = new System.Drawing.Point(276, 12);
            this.btnPrintForm.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintForm.Name = "btnPrintForm";
            this.btnPrintForm.Size = new System.Drawing.Size(145, 40);
            this.btnPrintForm.TabIndex = 5;
            this.btnPrintForm.Text = "طباعة استمارة التسجيل";
            this.btnPrintForm.UseVisualStyleBackColor = false;
            this.btnPrintForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintForm.FlatAppearance.BorderSize = 0;
            this.btnPrintForm.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPrintForm.ForeColor = System.Drawing.Color.White;
            this.btnPrintForm.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPrintForm.UseCompatibleTextRendering = true;
            this.btnPrintForm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrintForm.Click += new System.EventHandler(this.btnPrintForm_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(123, 12);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(145, 40);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "تحديث القائمة";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.UseCompatibleTextRendering = true;
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1041, 60);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(145, 40);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "إلغاء التعديل";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCancel.UseCompatibleTextRendering = true;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(888, 60);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(145, 40);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "حذف التسجيل";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDelete.UseCompatibleTextRendering = true;
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(735, 60);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(145, 40);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "تعديل التسجيل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.UseCompatibleTextRendering = true;
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(582, 60);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(145, 40);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "حفظ التسجيل";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSave.UseCompatibleTextRendering = true;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(429, 60);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(145, 40);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "إضافة تسجيل جديد";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAdd.UseCompatibleTextRendering = true;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // EnrollmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlButtons);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1050, 650);
            this.Name = "EnrollmentForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة التسجيل والقبول";
            this.Load += new System.EventHandler(this.EnrollmentForm_Load);
            this.gbBasic.ResumeLayout(false);
            this.tlpBasic.ResumeLayout(false);
            this.tlpBasic.PerformLayout();
            this.gbPrevious.ResumeLayout(false);
            this.tlpPrevious.ResumeLayout(false);
            this.tlpPrevious.PerformLayout();
            this.gbFees.ResumeLayout(false);
            this.tlpFees.ResumeLayout(false);
            this.tlpFees.PerformLayout();
            this.gbAttachments.ResumeLayout(false);
            this.flpAttachments.ResumeLayout(false);
            this.flpAttachments.PerformLayout();
            this.gbNotes.ResumeLayout(false);
            this.layoutRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        // ================================================================
        // CONTROLS
        // ================================================================

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

        // ================================================================
        // GROUPS
        // ================================================================

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

        // ================================================================
        // MAIN
        // ================================================================

        private System.Windows.Forms.TableLayoutPanel layoutRight;

        private System.Windows.Forms.DataGridView dgvEnrollments;

        private System.Windows.Forms.ErrorProvider errorProvider1;

        private System.Windows.Forms.SplitContainer splitContainerMain;

        private System.Windows.Forms.Panel pnlRight;

        // ================================================================
        // SEARCH
        // ================================================================

        private System.Windows.Forms.Panel pnlSearch;

        private System.Windows.Forms.TableLayoutPanel tlpSearch;

        private System.Windows.Forms.Label lblSearch;

        private System.Windows.Forms.TextBox txtSearch;

        private System.Windows.Forms.Button btnSearch;

        private System.Windows.Forms.Button btnReload;

        private System.Windows.Forms.Label lblCount;

        // ================================================================
        // BUTTONS
        // ================================================================

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
