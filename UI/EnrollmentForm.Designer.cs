namespace SchoolSystem.UI {
    partial class EnrollmentForm {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent() {
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
            this.gbNotes = new System.Windows.Forms.GroupBox();
            this.rtbNotes = new System.Windows.Forms.RichTextBox();
            this.gbAttachments = new System.Windows.Forms.GroupBox();
            this.flpAttachments = new System.Windows.Forms.FlowLayoutPanel();
            this.gbBasic = new System.Windows.Forms.GroupBox();
            this.tlpBasic = new System.Windows.Forms.TableLayoutPanel();
            this.gbPrevious = new System.Windows.Forms.GroupBox();
            this.tlpPrevious = new System.Windows.Forms.TableLayoutPanel();
            this.gbFees = new System.Windows.Forms.GroupBox();
            this.tlpFees = new System.Windows.Forms.TableLayoutPanel();
            this.dgvEnrollments = new System.Windows.Forms.DataGridView();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
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
            this.gbNotes.SuspendLayout();
            this.gbAttachments.SuspendLayout();
            this.flpAttachments.SuspendLayout();
            this.gbBasic.SuspendLayout();
            this.tlpBasic.SuspendLayout();
            this.gbPrevious.SuspendLayout();
            this.tlpPrevious.SuspendLayout();
            this.gbFees.SuspendLayout();
            this.tlpFees.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEnrollmentID
            // 
            this.lblEnrollmentID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEnrollmentID.AutoSize = true;
            this.lblEnrollmentID.Location = new System.Drawing.Point(628, 20);
            this.lblEnrollmentID.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblEnrollmentID.Name = "lblEnrollmentID";
            this.lblEnrollmentID.Size = new System.Drawing.Size(68, 17);
            this.lblEnrollmentID.TabIndex = 0;
            this.lblEnrollmentID.Text = "رقم الطلب";
            // 
            // txtEnrollmentID
            // 
            this.txtEnrollmentID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEnrollmentID.Location = new System.Drawing.Point(3, 9);
            this.txtEnrollmentID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEnrollmentID.Name = "txtEnrollmentID";
            this.txtEnrollmentID.ReadOnly = true;
            this.txtEnrollmentID.Size = new System.Drawing.Size(619, 24);
            this.txtEnrollmentID.TabIndex = 1;
            // 
            // lblStudentID
            // 
            this.lblStudentID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStudentID.AutoSize = true;
            this.lblStudentID.Location = new System.Drawing.Point(628, 60);
            this.lblStudentID.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStudentID.Name = "lblStudentID";
            this.lblStudentID.Size = new System.Drawing.Size(81, 17);
            this.lblStudentID.TabIndex = 2;
            this.lblStudentID.Text = "اختيار الطالب";
            // 
            // cmbStudentID
            // 
            this.cmbStudentID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStudentID.Location = new System.Drawing.Point(3, 49);
            this.cmbStudentID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbStudentID.Name = "cmbStudentID";
            this.cmbStudentID.Size = new System.Drawing.Size(619, 24);
            this.cmbStudentID.TabIndex = 3;
            this.cmbStudentID.SelectedIndexChanged += new System.EventHandler(this.cmbStudentID_SelectedIndexChanged);
            // 
            // lblStudentName
            // 
            this.lblStudentName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStudentName.AutoSize = true;
            this.lblStudentName.Location = new System.Drawing.Point(628, 100);
            this.lblStudentName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStudentName.Name = "lblStudentName";
            this.lblStudentName.Size = new System.Drawing.Size(77, 17);
            this.lblStudentName.TabIndex = 4;
            this.lblStudentName.Text = "اسم الطالب";
            // 
            // txtStudentName
            // 
            this.txtStudentName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStudentName.Location = new System.Drawing.Point(3, 89);
            this.txtStudentName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStudentName.Name = "txtStudentName";
            this.txtStudentName.ReadOnly = true;
            this.txtStudentName.Size = new System.Drawing.Size(619, 24);
            this.txtStudentName.TabIndex = 5;
            // 
            // lblApplicationDate
            // 
            this.lblApplicationDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblApplicationDate.AutoSize = true;
            this.lblApplicationDate.Location = new System.Drawing.Point(628, 140);
            this.lblApplicationDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblApplicationDate.Name = "lblApplicationDate";
            this.lblApplicationDate.Size = new System.Drawing.Size(91, 17);
            this.lblApplicationDate.TabIndex = 6;
            this.lblApplicationDate.Text = "تاريخ التسجيل";
            // 
            // dtpApplicationDate
            // 
            this.dtpApplicationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpApplicationDate.Location = new System.Drawing.Point(3, 129);
            this.dtpApplicationDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpApplicationDate.Name = "dtpApplicationDate";
            this.dtpApplicationDate.Size = new System.Drawing.Size(619, 24);
            this.dtpApplicationDate.TabIndex = 7;
            // 
            // lblApplicationType
            // 
            this.lblApplicationType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblApplicationType.AutoSize = true;
            this.lblApplicationType.Location = new System.Drawing.Point(628, 180);
            this.lblApplicationType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblApplicationType.Name = "lblApplicationType";
            this.lblApplicationType.Size = new System.Drawing.Size(65, 17);
            this.lblApplicationType.TabIndex = 8;
            this.lblApplicationType.Text = "نوع الطلب";
            // 
            // cmbApplicationType
            // 
            this.cmbApplicationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbApplicationType.Location = new System.Drawing.Point(3, 169);
            this.cmbApplicationType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbApplicationType.Name = "cmbApplicationType";
            this.cmbApplicationType.Size = new System.Drawing.Size(619, 24);
            this.cmbApplicationType.TabIndex = 9;
            // 
            // lblAcademicYear
            // 
            this.lblAcademicYear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAcademicYear.AutoSize = true;
            this.lblAcademicYear.Location = new System.Drawing.Point(628, 220);
            this.lblAcademicYear.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAcademicYear.Name = "lblAcademicYear";
            this.lblAcademicYear.Size = new System.Drawing.Size(89, 17);
            this.lblAcademicYear.TabIndex = 10;
            this.lblAcademicYear.Text = "العام الدراسي";
            // 
            // txtAcademicYear
            // 
            this.txtAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAcademicYear.Location = new System.Drawing.Point(3, 209);
            this.txtAcademicYear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAcademicYear.Name = "txtAcademicYear";
            this.txtAcademicYear.Size = new System.Drawing.Size(619, 24);
            this.txtAcademicYear.TabIndex = 11;
            // 
            // lblClassID
            // 
            this.lblClassID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblClassID.AutoSize = true;
            this.lblClassID.Location = new System.Drawing.Point(628, 260);
            this.lblClassID.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblClassID.Name = "lblClassID";
            this.lblClassID.Size = new System.Drawing.Size(39, 17);
            this.lblClassID.TabIndex = 12;
            this.lblClassID.Text = "الصف";
            // 
            // cmbClassID
            // 
            this.cmbClassID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClassID.Location = new System.Drawing.Point(3, 249);
            this.cmbClassID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbClassID.Name = "cmbClassID";
            this.cmbClassID.Size = new System.Drawing.Size(619, 24);
            this.cmbClassID.TabIndex = 13;
            // 
            // lblSection
            // 
            this.lblSection.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSection.AutoSize = true;
            this.lblSection.Location = new System.Drawing.Point(628, 300);
            this.lblSection.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(50, 17);
            this.lblSection.TabIndex = 14;
            this.lblSection.Text = "الشعبة";
            // 
            // txtSection
            // 
            this.txtSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtSection.FormattingEnabled = true;
            this.txtSection.Location = new System.Drawing.Point(3, 289);
            this.txtSection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSection.Name = "txtSection";
            this.txtSection.Size = new System.Drawing.Size(619, 24);
            this.txtSection.TabIndex = 15;
            // 
            // lblSeatNumber
            // 
            this.lblSeatNumber.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSeatNumber.AutoSize = true;
            this.lblSeatNumber.Location = new System.Drawing.Point(628, 340);
            this.lblSeatNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSeatNumber.Name = "lblSeatNumber";
            this.lblSeatNumber.Size = new System.Drawing.Size(78, 17);
            this.lblSeatNumber.TabIndex = 16;
            this.lblSeatNumber.Text = "رقم الجلوس";
            // 
            // txtSeatNumber
            // 
            this.txtSeatNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSeatNumber.Location = new System.Drawing.Point(3, 329);
            this.txtSeatNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSeatNumber.Name = "txtSeatNumber";
            this.txtSeatNumber.Size = new System.Drawing.Size(619, 24);
            this.txtSeatNumber.TabIndex = 17;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(628, 380);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(72, 17);
            this.lblStatus.TabIndex = 18;
            this.lblStatus.Text = "حالة الطلب";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.Location = new System.Drawing.Point(3, 369);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(619, 24);
            this.cmbStatus.TabIndex = 19;
            // 
            // lblPreviousSchool
            // 
            this.lblPreviousSchool.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPreviousSchool.AutoSize = true;
            this.lblPreviousSchool.Location = new System.Drawing.Point(628, 20);
            this.lblPreviousSchool.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPreviousSchool.Name = "lblPreviousSchool";
            this.lblPreviousSchool.Size = new System.Drawing.Size(90, 17);
            this.lblPreviousSchool.TabIndex = 0;
            this.lblPreviousSchool.Text = "اسم المدرسة";
            // 
            // txtPreviousSchool
            // 
            this.txtPreviousSchool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPreviousSchool.Location = new System.Drawing.Point(3, 9);
            this.txtPreviousSchool.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPreviousSchool.Name = "txtPreviousSchool";
            this.txtPreviousSchool.Size = new System.Drawing.Size(619, 24);
            this.txtPreviousSchool.TabIndex = 1;
            // 
            // lblPreviousClass
            // 
            this.lblPreviousClass.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPreviousClass.AutoSize = true;
            this.lblPreviousClass.Location = new System.Drawing.Point(628, 60);
            this.lblPreviousClass.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPreviousClass.Name = "lblPreviousClass";
            this.lblPreviousClass.Size = new System.Drawing.Size(85, 17);
            this.lblPreviousClass.TabIndex = 2;
            this.lblPreviousClass.Text = "الصف السابق";
            // 
            // txtPreviousClass
            // 
            this.txtPreviousClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPreviousClass.Location = new System.Drawing.Point(3, 49);
            this.txtPreviousClass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPreviousClass.Name = "txtPreviousClass";
            this.txtPreviousClass.Size = new System.Drawing.Size(619, 24);
            this.txtPreviousClass.TabIndex = 3;
            // 
            // lblTransferReason
            // 
            this.lblTransferReason.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTransferReason.AutoSize = true;
            this.lblTransferReason.Location = new System.Drawing.Point(628, 100);
            this.lblTransferReason.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblTransferReason.Name = "lblTransferReason";
            this.lblTransferReason.Size = new System.Drawing.Size(73, 17);
            this.lblTransferReason.TabIndex = 4;
            this.lblTransferReason.Text = "سبب النقل";
            // 
            // txtTransferReason
            // 
            this.txtTransferReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTransferReason.Location = new System.Drawing.Point(3, 89);
            this.txtTransferReason.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTransferReason.Name = "txtTransferReason";
            this.txtTransferReason.Size = new System.Drawing.Size(619, 24);
            this.txtTransferReason.TabIndex = 5;
            // 
            // lblRegistrationFee
            // 
            this.lblRegistrationFee.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRegistrationFee.AutoSize = true;
            this.lblRegistrationFee.Location = new System.Drawing.Point(628, 20);
            this.lblRegistrationFee.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblRegistrationFee.Name = "lblRegistrationFee";
            this.lblRegistrationFee.Size = new System.Drawing.Size(97, 17);
            this.lblRegistrationFee.TabIndex = 0;
            this.lblRegistrationFee.Text = "رسوم التسجيل";
            // 
            // txtRegistrationFee
            // 
            this.txtRegistrationFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRegistrationFee.Location = new System.Drawing.Point(3, 9);
            this.txtRegistrationFee.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRegistrationFee.Name = "txtRegistrationFee";
            this.txtRegistrationFee.Size = new System.Drawing.Size(619, 24);
            this.txtRegistrationFee.TabIndex = 1;
            this.txtRegistrationFee.TextChanged += new System.EventHandler(this.txtFees_TextChanged);
            // 
            // lblPaidAmount
            // 
            this.lblPaidAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPaidAmount.AutoSize = true;
            this.lblPaidAmount.Location = new System.Drawing.Point(628, 60);
            this.lblPaidAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPaidAmount.Name = "lblPaidAmount";
            this.lblPaidAmount.Size = new System.Drawing.Size(90, 17);
            this.lblPaidAmount.TabIndex = 2;
            this.lblPaidAmount.Text = "المبلغ المدفوع";
            // 
            // txtPaidAmount
            // 
            this.txtPaidAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPaidAmount.Location = new System.Drawing.Point(3, 49);
            this.txtPaidAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPaidAmount.Name = "txtPaidAmount";
            this.txtPaidAmount.Size = new System.Drawing.Size(619, 24);
            this.txtPaidAmount.TabIndex = 3;
            this.txtPaidAmount.TextChanged += new System.EventHandler(this.txtFees_TextChanged);
            // 
            // lblRemainingAmount
            // 
            this.lblRemainingAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRemainingAmount.AutoSize = true;
            this.lblRemainingAmount.Location = new System.Drawing.Point(628, 100);
            this.lblRemainingAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblRemainingAmount.Name = "lblRemainingAmount";
            this.lblRemainingAmount.Size = new System.Drawing.Size(93, 17);
            this.lblRemainingAmount.TabIndex = 4;
            this.lblRemainingAmount.Text = "المبلغ المتبقي";
            // 
            // txtRemainingAmount
            // 
            this.txtRemainingAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemainingAmount.Location = new System.Drawing.Point(3, 89);
            this.txtRemainingAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRemainingAmount.Name = "txtRemainingAmount";
            this.txtRemainingAmount.ReadOnly = true;
            this.txtRemainingAmount.Size = new System.Drawing.Size(619, 24);
            this.txtRemainingAmount.TabIndex = 5;
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Location = new System.Drawing.Point(628, 140);
            this.lblPaymentMethod.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(75, 17);
            this.lblPaymentMethod.TabIndex = 6;
            this.lblPaymentMethod.Text = "طريقة الدفع";
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPaymentMethod.Location = new System.Drawing.Point(3, 129);
            this.cmbPaymentMethod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(619, 24);
            this.cmbPaymentMethod.TabIndex = 7;
            // 
            // lblReceiptNo
            // 
            this.lblReceiptNo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblReceiptNo.AutoSize = true;
            this.lblReceiptNo.Location = new System.Drawing.Point(628, 180);
            this.lblReceiptNo.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblReceiptNo.Name = "lblReceiptNo";
            this.lblReceiptNo.Size = new System.Drawing.Size(68, 17);
            this.lblReceiptNo.TabIndex = 8;
            this.lblReceiptNo.Text = "رقم السند";
            // 
            // txtReceiptNo
            // 
            this.txtReceiptNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReceiptNo.Location = new System.Drawing.Point(3, 169);
            this.txtReceiptNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtReceiptNo.Name = "txtReceiptNo";
            this.txtReceiptNo.Size = new System.Drawing.Size(619, 24);
            this.txtReceiptNo.TabIndex = 9;
            // 
            // chkHasBirthCertificate
            // 
            this.chkHasBirthCertificate.AutoSize = true;
            this.chkHasBirthCertificate.Location = new System.Drawing.Point(600, 3);
            this.chkHasBirthCertificate.Name = "chkHasBirthCertificate";
            this.chkHasBirthCertificate.Padding = new System.Windows.Forms.Padding(10);
            this.chkHasBirthCertificate.Size = new System.Drawing.Size(132, 41);
            this.chkHasBirthCertificate.TabIndex = 0;
            this.chkHasBirthCertificate.Text = "شهادة الميلاد";
            // 
            // chkHasGuardianId
            // 
            this.chkHasGuardianId.AutoSize = true;
            this.chkHasGuardianId.Location = new System.Drawing.Point(460, 3);
            this.chkHasGuardianId.Name = "chkHasGuardianId";
            this.chkHasGuardianId.Padding = new System.Windows.Forms.Padding(10);
            this.chkHasGuardianId.Size = new System.Drawing.Size(134, 41);
            this.chkHasGuardianId.TabIndex = 1;
            this.chkHasGuardianId.Text = "هوية ولي الأمر";
            // 
            // chkHasPhoto
            // 
            this.chkHasPhoto.AutoSize = true;
            this.chkHasPhoto.Location = new System.Drawing.Point(323, 3);
            this.chkHasPhoto.Name = "chkHasPhoto";
            this.chkHasPhoto.Padding = new System.Windows.Forms.Padding(10);
            this.chkHasPhoto.Size = new System.Drawing.Size(131, 41);
            this.chkHasPhoto.TabIndex = 2;
            this.chkHasPhoto.Text = "صورة شخصية";
            // 
            // chkHasLastCertificate
            // 
            this.chkHasLastCertificate.AutoSize = true;
            this.chkHasLastCertificate.Location = new System.Drawing.Point(205, 3);
            this.chkHasLastCertificate.Name = "chkHasLastCertificate";
            this.chkHasLastCertificate.Padding = new System.Windows.Forms.Padding(10);
            this.chkHasLastCertificate.Size = new System.Drawing.Size(112, 41);
            this.chkHasLastCertificate.TabIndex = 3;
            this.chkHasLastCertificate.Text = "آخر شهادة";
            // 
            // chkHasMedicalReport
            // 
            this.chkHasMedicalReport.AutoSize = true;
            this.chkHasMedicalReport.Location = new System.Drawing.Point(90, 3);
            this.chkHasMedicalReport.Name = "chkHasMedicalReport";
            this.chkHasMedicalReport.Padding = new System.Windows.Forms.Padding(10);
            this.chkHasMedicalReport.Size = new System.Drawing.Size(109, 41);
            this.chkHasMedicalReport.TabIndex = 4;
            this.chkHasMedicalReport.Text = "تقرير طبي";
            // 
            // gbNotes
            // 
            this.gbNotes.Controls.Add(this.rtbNotes);
            this.gbNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbNotes.Location = new System.Drawing.Point(10, 980);
            this.gbNotes.Name = "gbNotes";
            this.gbNotes.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.gbNotes.Size = new System.Drawing.Size(755, 100);
            this.gbNotes.TabIndex = 0;
            this.gbNotes.TabStop = false;
            this.gbNotes.Text = "الملاحظات";
            // 
            // rtbNotes
            // 
            this.rtbNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbNotes.Location = new System.Drawing.Point(10, 37);
            this.rtbNotes.Name = "rtbNotes";
            this.rtbNotes.Size = new System.Drawing.Size(735, 53);
            this.rtbNotes.TabIndex = 0;
            this.rtbNotes.Text = "";
            // 
            // gbAttachments
            // 
            this.gbAttachments.AutoSize = true;
            this.gbAttachments.Controls.Add(this.flpAttachments);
            this.gbAttachments.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbAttachments.Location = new System.Drawing.Point(10, 886);
            this.gbAttachments.Name = "gbAttachments";
            this.gbAttachments.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.gbAttachments.Size = new System.Drawing.Size(755, 94);
            this.gbAttachments.TabIndex = 1;
            this.gbAttachments.TabStop = false;
            this.gbAttachments.Text = "المرفقات";
            // 
            // flpAttachments
            // 
            this.flpAttachments.AutoSize = true;
            this.flpAttachments.Controls.Add(this.chkHasBirthCertificate);
            this.flpAttachments.Controls.Add(this.chkHasGuardianId);
            this.flpAttachments.Controls.Add(this.chkHasPhoto);
            this.flpAttachments.Controls.Add(this.chkHasLastCertificate);
            this.flpAttachments.Controls.Add(this.chkHasMedicalReport);
            this.flpAttachments.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpAttachments.Location = new System.Drawing.Point(10, 37);
            this.flpAttachments.Name = "flpAttachments";
            this.flpAttachments.Size = new System.Drawing.Size(735, 47);
            this.flpAttachments.TabIndex = 0;
            // 
            // gbBasic
            // 
            this.gbBasic.AutoSize = true;
            this.gbBasic.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbBasic.Controls.Add(this.tlpBasic);
            this.gbBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbBasic.Location = new System.Drawing.Point(10, 10);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.gbBasic.Size = new System.Drawing.Size(755, 452);
            this.gbBasic.TabIndex = 4;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "بيانات التسجيل الأساسية";
            // 
            // tlpBasic
            // 
            this.tlpBasic.AutoSize = true;
            this.tlpBasic.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpBasic.ColumnCount = 2;
            this.tlpBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
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
            this.tlpBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpBasic.Location = new System.Drawing.Point(10, 37);
            this.tlpBasic.Name = "tlpBasic";
            this.tlpBasic.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.tlpBasic.RowCount = 10;
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpBasic.Size = new System.Drawing.Size(735, 405);
            this.tlpBasic.TabIndex = 0;
            // 
            // gbPrevious
            // 
            this.gbPrevious.AutoSize = true;
            this.gbPrevious.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbPrevious.Controls.Add(this.tlpPrevious);
            this.gbPrevious.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbPrevious.Location = new System.Drawing.Point(10, 462);
            this.gbPrevious.Name = "gbPrevious";
            this.gbPrevious.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.gbPrevious.Size = new System.Drawing.Size(755, 172);
            this.gbPrevious.TabIndex = 3;
            this.gbPrevious.TabStop = false;
            this.gbPrevious.Text = "المدرسة السابقة";
            // 
            // tlpPrevious
            // 
            this.tlpPrevious.AutoSize = true;
            this.tlpPrevious.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpPrevious.ColumnCount = 2;
            this.tlpPrevious.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpPrevious.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPrevious.Controls.Add(this.lblPreviousSchool, 0, 0);
            this.tlpPrevious.Controls.Add(this.txtPreviousSchool, 1, 0);
            this.tlpPrevious.Controls.Add(this.lblPreviousClass, 0, 1);
            this.tlpPrevious.Controls.Add(this.txtPreviousClass, 1, 1);
            this.tlpPrevious.Controls.Add(this.lblTransferReason, 0, 2);
            this.tlpPrevious.Controls.Add(this.txtTransferReason, 1, 2);
            this.tlpPrevious.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpPrevious.Location = new System.Drawing.Point(10, 37);
            this.tlpPrevious.Name = "tlpPrevious";
            this.tlpPrevious.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.tlpPrevious.RowCount = 3;
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpPrevious.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpPrevious.Size = new System.Drawing.Size(735, 125);
            this.tlpPrevious.TabIndex = 0;
            // 
            // gbFees
            // 
            this.gbFees.AutoSize = true;
            this.gbFees.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbFees.Controls.Add(this.tlpFees);
            this.gbFees.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFees.Location = new System.Drawing.Point(10, 634);
            this.gbFees.Name = "gbFees";
            this.gbFees.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.gbFees.Size = new System.Drawing.Size(755, 252);
            this.gbFees.TabIndex = 2;
            this.gbFees.TabStop = false;
            this.gbFees.Text = "الرسوم";
            // 
            // tlpFees
            // 
            this.tlpFees.AutoSize = true;
            this.tlpFees.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpFees.ColumnCount = 2;
            this.tlpFees.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
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
            this.tlpFees.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpFees.Location = new System.Drawing.Point(10, 37);
            this.tlpFees.Name = "tlpFees";
            this.tlpFees.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.tlpFees.RowCount = 5;
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpFees.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpFees.Size = new System.Drawing.Size(735, 205);
            this.tlpFees.TabIndex = 0;
            // 
            // dgvEnrollments
            // 
            this.dgvEnrollments.AllowUserToAddRows = false;
            this.dgvEnrollments.AllowUserToDeleteRows = false;
            this.dgvEnrollments.ColumnHeadersHeight = 29;
            this.dgvEnrollments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEnrollments.Location = new System.Drawing.Point(10, 10);
            this.dgvEnrollments.Name = "dgvEnrollments";
            this.dgvEnrollments.ReadOnly = true;
            this.dgvEnrollments.RowHeadersWidth = 51;
            this.dgvEnrollments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEnrollments.Size = new System.Drawing.Size(390, 600);
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
            this.splitContainerMain.Panel1.Controls.Add(this.dgvEnrollments);
            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.splitContainerMain.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.pnlRight);
            this.splitContainerMain.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerMain.Size = new System.Drawing.Size(1200, 620);
            this.splitContainerMain.SplitterDistance = 400;
            this.splitContainerMain.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.Controls.Add(this.gbNotes);
            this.pnlRight.Controls.Add(this.gbAttachments);
            this.pnlRight.Controls.Add(this.gbFees);
            this.pnlRight.Controls.Add(this.gbPrevious);
            this.pnlRight.Controls.Add(this.gbBasic);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(0, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10);
            this.pnlRight.Size = new System.Drawing.Size(796, 620);
            this.pnlRight.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnReload);
            this.pnlSearch.Controls.Add(this.lblCount);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSearch.Size = new System.Drawing.Size(1200, 60);
            this.pnlSearch.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtSearch.Location = new System.Drawing.Point(603, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 24);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(903, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(135, 40);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "بحث";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReload
            // 
            this.btnReload.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReload.Location = new System.Drawing.Point(1038, 10);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(152, 40);
            this.btnReload.TabIndex = 2;
            this.btnReload.Text = "مسح الفلتر";
            this.btnReload.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCount.Location = new System.Drawing.Point(10, 10);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 17);
            this.lblCount.TabIndex = 3;
            // 
            // pnlButtons
            // 
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
            this.pnlButtons.Location = new System.Drawing.Point(0, 680);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(10);
            this.pnlButtons.Size = new System.Drawing.Size(1200, 70);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(-34, 10);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(137, 50);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "إغلاق";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExportExcel.Location = new System.Drawing.Point(103, 10);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(5);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(90, 50);
            this.btnExportExcel.TabIndex = 3;
            this.btnExportExcel.Text = "Excel";
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExportPdf.Location = new System.Drawing.Point(193, 10);
            this.btnExportPdf.Margin = new System.Windows.Forms.Padding(5);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(90, 50);
            this.btnExportPdf.TabIndex = 2;
            this.btnExportPdf.Text = "PDF";
            // 
            // btnPreviewOutput
            // 
            this.btnPreviewOutput.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPreviewOutput.Location = new System.Drawing.Point(283, 10);
            this.btnPreviewOutput.Margin = new System.Windows.Forms.Padding(5);
            this.btnPreviewOutput.Name = "btnPreviewOutput";
            this.btnPreviewOutput.Size = new System.Drawing.Size(110, 50);
            this.btnPreviewOutput.TabIndex = 1;
            this.btnPreviewOutput.Text = "معاينة | Preview";
            // 
            // btnPrintReceipt
            // 
            this.btnPrintReceipt.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrintReceipt.Location = new System.Drawing.Point(393, 10);
            this.btnPrintReceipt.Margin = new System.Windows.Forms.Padding(5);
            this.btnPrintReceipt.Name = "btnPrintReceipt";
            this.btnPrintReceipt.Size = new System.Drawing.Size(108, 50);
            this.btnPrintReceipt.TabIndex = 1;
            this.btnPrintReceipt.Text = "طباعة إيصال";
            this.btnPrintReceipt.Click += new System.EventHandler(this.btnPrintReceipt_Click);
            // 
            // btnPrintForm
            // 
            this.btnPrintForm.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrintForm.Location = new System.Drawing.Point(501, 10);
            this.btnPrintForm.Margin = new System.Windows.Forms.Padding(5);
            this.btnPrintForm.Name = "btnPrintForm";
            this.btnPrintForm.Size = new System.Drawing.Size(149, 50);
            this.btnPrintForm.TabIndex = 2;
            this.btnPrintForm.Text = "طباعة استمارة";
            this.btnPrintForm.Click += new System.EventHandler(this.btnPrintForm_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRefresh.Location = new System.Drawing.Point(650, 10);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 50);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(740, 10);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 50);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "إلغاء";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Location = new System.Drawing.Point(830, 10);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 50);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "حذف";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnUpdate.Location = new System.Drawing.Point(920, 10);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(5);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(90, 50);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(1010, 10);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 50);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "حفظ";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd.Location = new System.Drawing.Point(1100, 10);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 50);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "جديد";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // EnrollmentForm
            // 
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlButtons);
            this.Name = "EnrollmentForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "إدارة التسجيل والقبول";
            this.Load += new System.EventHandler(this.EnrollmentForm_Load);
            this.gbNotes.ResumeLayout(false);
            this.gbAttachments.ResumeLayout(false);
            this.gbAttachments.PerformLayout();
            this.flpAttachments.ResumeLayout(false);
            this.flpAttachments.PerformLayout();
            this.gbBasic.ResumeLayout(false);
            this.gbBasic.PerformLayout();
            this.tlpBasic.ResumeLayout(false);
            this.tlpBasic.PerformLayout();
            this.gbPrevious.ResumeLayout(false);
            this.gbPrevious.PerformLayout();
            this.tlpPrevious.ResumeLayout(false);
            this.tlpPrevious.PerformLayout();
            this.gbFees.ResumeLayout(false);
            this.gbFees.PerformLayout();
            this.tlpFees.ResumeLayout(false);
            this.tlpFees.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }
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
        private System.Windows.Forms.DataGridView dgvEnrollments;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPrintForm;
        private System.Windows.Forms.Button btnPrintReceipt;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPreviewOutput;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Panel pnlRight;
    }
}