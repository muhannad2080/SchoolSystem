namespace SchoolSystem.UI
{
    partial class StudentsForm
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
            this.lblStudentNumber = new System.Windows.Forms.Label();
            this.txtStudentNumber = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.dtpBirthDate = new System.Windows.Forms.DateTimePicker();
            this.lblBirthPlace = new System.Windows.Forms.Label();
            this.txtBirthPlace = new System.Windows.Forms.TextBox();
            this.lblNationality = new System.Windows.Forms.Label();
            this.txtNationality = new System.Windows.Forms.TextBox();
            this.lblNationalId = new System.Windows.Forms.Label();
            this.txtNationalId = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblGuardianName = new System.Windows.Forms.Label();
            this.txtGuardianName = new System.Windows.Forms.TextBox();
            this.lblGuardianRelation = new System.Windows.Forms.Label();
            this.txtGuardianRelation = new System.Windows.Forms.TextBox();
            this.lblGuardianPhone = new System.Windows.Forms.Label();
            this.txtGuardianPhone = new System.Windows.Forms.TextBox();
            this.lblGuardianEmail = new System.Windows.Forms.Label();
            this.txtGuardianEmail = new System.Windows.Forms.TextBox();
            this.lblGuardianJob = new System.Windows.Forms.Label();
            this.txtGuardianJob = new System.Windows.Forms.TextBox();
            this.lblGovernorate = new System.Windows.Forms.Label();
            this.txtGovernorate = new System.Windows.Forms.TextBox();
            this.lblDistrict = new System.Windows.Forms.Label();
            this.txtDistrict = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.gbStudent = new System.Windows.Forms.GroupBox();
            this.tlpStudent = new System.Windows.Forms.TableLayoutPanel();
            this.gbGuardian = new System.Windows.Forms.GroupBox();
            this.tlpGuardian = new System.Windows.Forms.TableLayoutPanel();
            this.gbAddress = new System.Windows.Forms.GroupBox();
            this.tlpAddress = new System.Windows.Forms.TableLayoutPanel();
            this.gbPhoto = new System.Windows.Forms.GroupBox();
            this.picStudent = new System.Windows.Forms.PictureBox();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.btnRemoveImage = new System.Windows.Forms.Button();
            this.layoutRight = new System.Windows.Forms.TableLayoutPanel();
            this.layoutBottom = new System.Windows.Forms.TableLayoutPanel();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.cmbFilterClass = new System.Windows.Forms.ComboBox();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStudentProfile = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gbStudent.SuspendLayout();
            this.tlpStudent.SuspendLayout();
            this.gbGuardian.SuspendLayout();
            this.tlpGuardian.SuspendLayout();
            this.gbAddress.SuspendLayout();
            this.tlpAddress.SuspendLayout();
            this.gbPhoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStudent)).BeginInit();
            this.layoutRight.SuspendLayout();
            this.layoutBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
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
            // lblStudentNumber
            // 
            this.lblStudentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStudentNumber.Location = new System.Drawing.Point(388, 2);
            this.lblStudentNumber.Margin = new System.Windows.Forms.Padding(2);
            this.lblStudentNumber.Name = "lblStudentNumber";
            this.lblStudentNumber.Size = new System.Drawing.Size(96, 21);
            this.lblStudentNumber.TabIndex = 0;
            this.lblStudentNumber.Text = "رقم الطالب (تلقائي)";
            this.lblStudentNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStudentNumber
            // 
            this.txtStudentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStudentNumber.Location = new System.Drawing.Point(3, 1);
            this.txtStudentNumber.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtStudentNumber.Name = "txtStudentNumber";
            this.txtStudentNumber.Size = new System.Drawing.Size(380, 26);
            this.txtStudentNumber.TabIndex = 1;
            // 
            // lblFullName
            // 
            this.lblFullName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFullName.Location = new System.Drawing.Point(388, 27);
            this.lblFullName.Margin = new System.Windows.Forms.Padding(2);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(96, 21);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "اسم الطالب *";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFullName
            // 
            this.txtFullName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFullName.Location = new System.Drawing.Point(3, 26);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(380, 26);
            this.txtFullName.TabIndex = 3;
            // 
            // lblGender
            // 
            this.lblGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGender.Location = new System.Drawing.Point(388, 52);
            this.lblGender.Margin = new System.Windows.Forms.Padding(2);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(96, 21);
            this.lblGender.TabIndex = 4;
            this.lblGender.Text = "الجنس *";
            this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbGender
            // 
            this.cmbGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Location = new System.Drawing.Point(3, 51);
            this.cmbGender.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(380, 26);
            this.cmbGender.TabIndex = 5;
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBirthDate.Location = new System.Drawing.Point(388, 77);
            this.lblBirthDate.Margin = new System.Windows.Forms.Padding(2);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(96, 21);
            this.lblBirthDate.TabIndex = 6;
            this.lblBirthDate.Text = "تاريخ الميلاد *";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpBirthDate
            // 
            this.dtpBirthDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBirthDate.Location = new System.Drawing.Point(3, 76);
            this.dtpBirthDate.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.ShowCheckBox = true;
            this.dtpBirthDate.Size = new System.Drawing.Size(380, 26);
            this.dtpBirthDate.TabIndex = 7;
            // 
            // lblBirthPlace
            // 
            this.lblBirthPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBirthPlace.Location = new System.Drawing.Point(388, 102);
            this.lblBirthPlace.Margin = new System.Windows.Forms.Padding(2);
            this.lblBirthPlace.Name = "lblBirthPlace";
            this.lblBirthPlace.Size = new System.Drawing.Size(96, 21);
            this.lblBirthPlace.TabIndex = 8;
            this.lblBirthPlace.Text = "مكان الميلاد";
            this.lblBirthPlace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBirthPlace
            // 
            this.txtBirthPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBirthPlace.Location = new System.Drawing.Point(3, 101);
            this.txtBirthPlace.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtBirthPlace.Name = "txtBirthPlace";
            this.txtBirthPlace.Size = new System.Drawing.Size(380, 26);
            this.txtBirthPlace.TabIndex = 9;
            // 
            // lblNationality
            // 
            this.lblNationality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNationality.Location = new System.Drawing.Point(388, 127);
            this.lblNationality.Margin = new System.Windows.Forms.Padding(2);
            this.lblNationality.Name = "lblNationality";
            this.lblNationality.Size = new System.Drawing.Size(96, 21);
            this.lblNationality.TabIndex = 10;
            this.lblNationality.Text = "الجنسية";
            this.lblNationality.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNationality
            // 
            this.txtNationality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNationality.Location = new System.Drawing.Point(3, 126);
            this.txtNationality.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtNationality.Name = "txtNationality";
            this.txtNationality.Size = new System.Drawing.Size(380, 26);
            this.txtNationality.TabIndex = 11;
            // 
            // lblNationalId
            // 
            this.lblNationalId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNationalId.Location = new System.Drawing.Point(388, 152);
            this.lblNationalId.Margin = new System.Windows.Forms.Padding(2);
            this.lblNationalId.Name = "lblNationalId";
            this.lblNationalId.Size = new System.Drawing.Size(96, 21);
            this.lblNationalId.TabIndex = 12;
            this.lblNationalId.Text = "رقم الهوية";
            this.lblNationalId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNationalId
            // 
            this.txtNationalId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNationalId.Location = new System.Drawing.Point(3, 151);
            this.txtNationalId.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtNationalId.Name = "txtNationalId";
            this.txtNationalId.Size = new System.Drawing.Size(380, 26);
            this.txtNationalId.TabIndex = 13;
            // 
            // lblPhone
            // 
            this.lblPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPhone.Location = new System.Drawing.Point(388, 177);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(2);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(96, 21);
            this.lblPhone.TabIndex = 14;
            this.lblPhone.Text = "هاتف الطالب";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhone
            // 
            this.txtPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhone.Location = new System.Drawing.Point(3, 176);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(380, 26);
            this.txtPhone.TabIndex = 15;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(388, 202);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(96, 21);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "الحالة *";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(3, 201);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(380, 26);
            this.cmbStatus.TabIndex = 17;
            // 
            // lblGuardianName
            // 
            this.lblGuardianName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGuardianName.Location = new System.Drawing.Point(388, 2);
            this.lblGuardianName.Margin = new System.Windows.Forms.Padding(2);
            this.lblGuardianName.Name = "lblGuardianName";
            this.lblGuardianName.Size = new System.Drawing.Size(96, 21);
            this.lblGuardianName.TabIndex = 0;
            this.lblGuardianName.Text = "اسم ولي الأمر";
            this.lblGuardianName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGuardianName
            // 
            this.txtGuardianName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGuardianName.Location = new System.Drawing.Point(3, 1);
            this.txtGuardianName.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtGuardianName.Name = "txtGuardianName";
            this.txtGuardianName.Size = new System.Drawing.Size(380, 26);
            this.txtGuardianName.TabIndex = 1;
            // 
            // lblGuardianRelation
            // 
            this.lblGuardianRelation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGuardianRelation.Location = new System.Drawing.Point(388, 27);
            this.lblGuardianRelation.Margin = new System.Windows.Forms.Padding(2);
            this.lblGuardianRelation.Name = "lblGuardianRelation";
            this.lblGuardianRelation.Size = new System.Drawing.Size(96, 21);
            this.lblGuardianRelation.TabIndex = 2;
            this.lblGuardianRelation.Text = "صلة القرابة";
            this.lblGuardianRelation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGuardianRelation
            // 
            this.txtGuardianRelation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGuardianRelation.Location = new System.Drawing.Point(3, 26);
            this.txtGuardianRelation.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtGuardianRelation.Name = "txtGuardianRelation";
            this.txtGuardianRelation.Size = new System.Drawing.Size(380, 26);
            this.txtGuardianRelation.TabIndex = 3;
            // 
            // lblGuardianPhone
            // 
            this.lblGuardianPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGuardianPhone.Location = new System.Drawing.Point(388, 52);
            this.lblGuardianPhone.Margin = new System.Windows.Forms.Padding(2);
            this.lblGuardianPhone.Name = "lblGuardianPhone";
            this.lblGuardianPhone.Size = new System.Drawing.Size(96, 21);
            this.lblGuardianPhone.TabIndex = 4;
            this.lblGuardianPhone.Text = "رقم الهاتف";
            this.lblGuardianPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGuardianPhone
            // 
            this.txtGuardianPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGuardianPhone.Location = new System.Drawing.Point(3, 51);
            this.txtGuardianPhone.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtGuardianPhone.Name = "txtGuardianPhone";
            this.txtGuardianPhone.Size = new System.Drawing.Size(380, 26);
            this.txtGuardianPhone.TabIndex = 5;
            // 
            // lblGuardianEmail
            // 
            this.lblGuardianEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGuardianEmail.Location = new System.Drawing.Point(388, 77);
            this.lblGuardianEmail.Margin = new System.Windows.Forms.Padding(2);
            this.lblGuardianEmail.Name = "lblGuardianEmail";
            this.lblGuardianEmail.Size = new System.Drawing.Size(96, 21);
            this.lblGuardianEmail.TabIndex = 6;
            this.lblGuardianEmail.Text = "البريد الإلكتروني";
            this.lblGuardianEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGuardianEmail
            // 
            this.txtGuardianEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGuardianEmail.Location = new System.Drawing.Point(3, 76);
            this.txtGuardianEmail.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtGuardianEmail.Name = "txtGuardianEmail";
            this.txtGuardianEmail.Size = new System.Drawing.Size(380, 26);
            this.txtGuardianEmail.TabIndex = 7;
            // 
            // lblGuardianJob
            // 
            this.lblGuardianJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGuardianJob.Location = new System.Drawing.Point(388, 102);
            this.lblGuardianJob.Margin = new System.Windows.Forms.Padding(2);
            this.lblGuardianJob.Name = "lblGuardianJob";
            this.lblGuardianJob.Size = new System.Drawing.Size(96, 21);
            this.lblGuardianJob.TabIndex = 8;
            this.lblGuardianJob.Text = "الوظيفة";
            this.lblGuardianJob.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGuardianJob
            // 
            this.txtGuardianJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGuardianJob.Location = new System.Drawing.Point(3, 101);
            this.txtGuardianJob.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.txtGuardianJob.Name = "txtGuardianJob";
            this.txtGuardianJob.Size = new System.Drawing.Size(380, 26);
            this.txtGuardianJob.TabIndex = 9;
            // 
            // lblGovernorate
            // 
            this.lblGovernorate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGovernorate.Location = new System.Drawing.Point(267, 3);
            this.lblGovernorate.Margin = new System.Windows.Forms.Padding(2);
            this.lblGovernorate.Name = "lblGovernorate";
            this.lblGovernorate.Size = new System.Drawing.Size(71, 22);
            this.lblGovernorate.TabIndex = 0;
            this.lblGovernorate.Text = "المحافظة";
            this.lblGovernorate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGovernorate
            // 
            this.txtGovernorate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGovernorate.Location = new System.Drawing.Point(3, 3);
            this.txtGovernorate.Name = "txtGovernorate";
            this.txtGovernorate.Size = new System.Drawing.Size(259, 26);
            this.txtGovernorate.TabIndex = 1;
            // 
            // lblDistrict
            // 
            this.lblDistrict.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDistrict.Location = new System.Drawing.Point(267, 31);
            this.lblDistrict.Margin = new System.Windows.Forms.Padding(2);
            this.lblDistrict.Name = "lblDistrict";
            this.lblDistrict.Size = new System.Drawing.Size(71, 22);
            this.lblDistrict.TabIndex = 2;
            this.lblDistrict.Text = "المديرية";
            this.lblDistrict.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDistrict
            // 
            this.txtDistrict.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDistrict.Location = new System.Drawing.Point(3, 31);
            this.txtDistrict.Name = "txtDistrict";
            this.txtDistrict.Size = new System.Drawing.Size(259, 26);
            this.txtDistrict.TabIndex = 3;
            // 
            // lblAddress
            // 
            this.lblAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddress.Location = new System.Drawing.Point(267, 67);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(2);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(71, 22);
            this.lblAddress.TabIndex = 4;
            this.lblAddress.Text = "العنوان";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Location = new System.Drawing.Point(3, 65);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(259, 26);
            this.txtAddress.TabIndex = 5;
            // 
            // gbStudent
            // 
            this.gbStudent.BackColor = System.Drawing.Color.White;
            this.gbStudent.Controls.Add(this.tlpStudent);
            this.gbStudent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbStudent.Location = new System.Drawing.Point(0, 0);
            this.gbStudent.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.gbStudent.Name = "gbStudent";
            this.gbStudent.Padding = new System.Windows.Forms.Padding(8, 20, 8, 5);
            this.gbStudent.Size = new System.Drawing.Size(502, 249);
            this.gbStudent.TabIndex = 3;
            this.gbStudent.TabStop = false;
            this.gbStudent.Text = "البيانات الأساسية للطالب";
            // 
            // tlpStudent
            // 
            this.tlpStudent.ColumnCount = 4;
            this.tlpStudent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStudent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpStudent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStudent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpStudent.Controls.Add(this.lblStudentNumber, 0, 0);
            this.tlpStudent.Controls.Add(this.txtStudentNumber, 1, 0);
            this.tlpStudent.Controls.Add(this.lblGender, 2, 0);
            this.tlpStudent.Controls.Add(this.cmbGender, 3, 0);
            this.tlpStudent.Controls.Add(this.lblFullName, 0, 1);
            this.tlpStudent.Controls.Add(this.txtFullName, 1, 1);
            this.tlpStudent.SetColumnSpan(this.txtFullName, 3);
            this.tlpStudent.Controls.Add(this.lblBirthDate, 0, 2);
            this.tlpStudent.Controls.Add(this.dtpBirthDate, 1, 2);
            this.tlpStudent.Controls.Add(this.lblBirthPlace, 2, 2);
            this.tlpStudent.Controls.Add(this.txtBirthPlace, 3, 2);
            this.tlpStudent.Controls.Add(this.lblNationality, 0, 3);
            this.tlpStudent.Controls.Add(this.txtNationality, 1, 3);
            this.tlpStudent.Controls.Add(this.lblNationalId, 2, 3);
            this.tlpStudent.Controls.Add(this.txtNationalId, 3, 3);
            this.tlpStudent.Controls.Add(this.lblPhone, 0, 4);
            this.tlpStudent.Controls.Add(this.txtPhone, 1, 4);
            this.tlpStudent.Controls.Add(this.lblStatus, 2, 4);
            this.tlpStudent.Controls.Add(this.cmbStatus, 3, 4);
            this.tlpStudent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStudent.Location = new System.Drawing.Point(8, 39);
            this.tlpStudent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStudent.Name = "tlpStudent";
            this.tlpStudent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpStudent.RowCount = 5;
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStudent.Size = new System.Drawing.Size(486, 161);
            this.tlpStudent.TabIndex = 0;
            // 
            // gbGuardian
            // 
            this.gbGuardian.BackColor = System.Drawing.Color.White;
            this.gbGuardian.Controls.Add(this.tlpGuardian);
            this.gbGuardian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGuardian.Location = new System.Drawing.Point(0, 254);
            this.gbGuardian.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.gbGuardian.Name = "gbGuardian";
            this.gbGuardian.Padding = new System.Windows.Forms.Padding(8, 20, 8, 5);
            this.gbGuardian.Size = new System.Drawing.Size(502, 150);
            this.gbGuardian.TabIndex = 2;
            this.gbGuardian.TabStop = false;
            this.gbGuardian.Text = "بيانات ولي الأمر";
            // 
            // tlpGuardian
            // 
            this.tlpGuardian.ColumnCount = 4;
            this.tlpGuardian.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpGuardian.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpGuardian.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpGuardian.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpGuardian.Controls.Add(this.lblGuardianName, 0, 0);
            this.tlpGuardian.Controls.Add(this.txtGuardianName, 1, 0);
            this.tlpGuardian.SetColumnSpan(this.txtGuardianName, 3);
            this.tlpGuardian.Controls.Add(this.lblGuardianRelation, 0, 1);
            this.tlpGuardian.Controls.Add(this.txtGuardianRelation, 1, 1);
            this.tlpGuardian.Controls.Add(this.lblGuardianPhone, 2, 1);
            this.tlpGuardian.Controls.Add(this.txtGuardianPhone, 3, 1);
            this.tlpGuardian.Controls.Add(this.lblGuardianEmail, 0, 2);
            this.tlpGuardian.Controls.Add(this.txtGuardianEmail, 1, 2);
            this.tlpGuardian.Controls.Add(this.lblGuardianJob, 2, 2);
            this.tlpGuardian.Controls.Add(this.txtGuardianJob, 3, 2);
            this.tlpGuardian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGuardian.Location = new System.Drawing.Point(8, 39);
            this.tlpGuardian.Margin = new System.Windows.Forms.Padding(0);
            this.tlpGuardian.Name = "tlpGuardian";
            this.tlpGuardian.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpGuardian.RowCount = 3;
            this.tlpGuardian.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333F));
            this.tlpGuardian.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333F));
            this.tlpGuardian.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3334F));
            this.tlpGuardian.Size = new System.Drawing.Size(486, 106);
            this.tlpGuardian.TabIndex = 0;
            // 
            // gbAddress
            // 
            this.gbAddress.BackColor = System.Drawing.Color.White;
            this.gbAddress.Controls.Add(this.tlpAddress);
            this.gbAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAddress.Location = new System.Drawing.Point(146, 0);
            this.gbAddress.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.gbAddress.Name = "gbAddress";
            this.gbAddress.Padding = new System.Windows.Forms.Padding(8, 20, 8, 5);
            this.gbAddress.Size = new System.Drawing.Size(356, 145);
            this.gbAddress.TabIndex = 1;
            this.gbAddress.TabStop = false;
            this.gbAddress.Text = "السكن والعنوان";
            // 
            // tlpAddress
            // 
            this.tlpAddress.ColumnCount = 2;
            this.tlpAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tlpAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 265F));
            this.tlpAddress.Controls.Add(this.lblGovernorate, 0, 0);
            this.tlpAddress.Controls.Add(this.txtGovernorate, 1, 0);
            this.tlpAddress.Controls.Add(this.lblDistrict, 0, 1);
            this.tlpAddress.Controls.Add(this.txtDistrict, 1, 1);
            this.tlpAddress.Controls.Add(this.lblAddress, 0, 2);
            this.tlpAddress.Controls.Add(this.txtAddress, 1, 2);
            this.tlpAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAddress.Location = new System.Drawing.Point(8, 39);
            this.tlpAddress.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAddress.Name = "tlpAddress";
            this.tlpAddress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpAddress.RowCount = 3;
            this.tlpAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpAddress.Size = new System.Drawing.Size(340, 101);
            this.tlpAddress.TabIndex = 0;
            // 
            // gbPhoto
            // 
            this.gbPhoto.BackColor = System.Drawing.Color.White;
            this.gbPhoto.Controls.Add(this.picStudent);
            this.gbPhoto.Controls.Add(this.btnChooseImage);
            this.gbPhoto.Controls.Add(this.btnRemoveImage);
            this.gbPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPhoto.Location = new System.Drawing.Point(0, 0);
            this.gbPhoto.Margin = new System.Windows.Forms.Padding(0);
            this.gbPhoto.Name = "gbPhoto";
            this.gbPhoto.Padding = new System.Windows.Forms.Padding(6);
            this.gbPhoto.Size = new System.Drawing.Size(141, 145);
            this.gbPhoto.TabIndex = 0;
            this.gbPhoto.TabStop = false;
            this.gbPhoto.Text = "الصورة الشخصية";
            // 
            // picStudent
            // 
            this.picStudent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picStudent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.picStudent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picStudent.Location = new System.Drawing.Point(20, 25);
            this.picStudent.Name = "picStudent";
            this.picStudent.Size = new System.Drawing.Size(100, 100);
            this.picStudent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picStudent.TabIndex = 0;
            this.picStudent.TabStop = false;
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.Location = new System.Drawing.Point(72, 130);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(58, 34);
            this.btnChooseImage.TabIndex = 1;
            this.btnChooseImage.Text = "اختيار";
            this.btnChooseImage.UseVisualStyleBackColor = true;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);
            // 
            // btnRemoveImage
            // 
            this.btnRemoveImage.Location = new System.Drawing.Point(10, 130);
            this.btnRemoveImage.Name = "btnRemoveImage";
            this.btnRemoveImage.Size = new System.Drawing.Size(58, 34);
            this.btnRemoveImage.TabIndex = 2;
            this.btnRemoveImage.Text = "إزالة";
            this.btnRemoveImage.UseVisualStyleBackColor = true;
            this.btnRemoveImage.Click += new System.EventHandler(this.btnRemoveImage_Click);
            // 
            // layoutRight
            // 
            this.layoutRight.ColumnCount = 1;
            this.layoutRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRight.Controls.Add(this.gbStudent, 0, 0);
            this.layoutRight.Controls.Add(this.gbGuardian, 0, 1);
            this.layoutRight.Controls.Add(this.layoutBottom, 0, 2);
            this.layoutRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRight.Location = new System.Drawing.Point(0, 0);
            this.layoutRight.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRight.Name = "layoutRight";
            this.layoutRight.RowCount = 3;
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27F));
            this.layoutRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.layoutRight.Size = new System.Drawing.Size(502, 554);
            this.layoutRight.TabIndex = 0;
            // 
            // layoutBottom
            // 
            this.layoutBottom.ColumnCount = 2;
            this.layoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72F));
            this.layoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.layoutBottom.Controls.Add(this.gbAddress, 0, 0);
            this.layoutBottom.Controls.Add(this.gbPhoto, 1, 0);
            this.layoutBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutBottom.Location = new System.Drawing.Point(0, 409);
            this.layoutBottom.Margin = new System.Windows.Forms.Padding(0);
            this.layoutBottom.Name = "layoutBottom";
            this.layoutBottom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.layoutBottom.RowCount = 1;
            this.layoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutBottom.Size = new System.Drawing.Size(502, 145);
            this.layoutBottom.TabIndex = 4;
            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.AllowUserToResizeRows = false;
            this.dgvStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStudents.BackgroundColor = System.Drawing.Color.White;
            this.dgvStudents.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvStudents.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvStudents.ColumnHeadersHeight = 40;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStudents.EnableHeadersVisualStyles = false;
            this.dgvStudents.Location = new System.Drawing.Point(10, 10);
            this.dgvStudents.Margin = new System.Windows.Forms.Padding(0);
            this.dgvStudents.MultiSelect = false;
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.RowTemplate.Height = 34;
            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.Size = new System.Drawing.Size(652, 554);
            this.dgvStudents.TabIndex = 0;
            this.dgvStudents.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStudents_CellClick);
            this.dgvStudents.SelectionChanged += new System.EventHandler(this.dgvStudents_SelectionChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 66);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.splitContainerMain.Panel1.Controls.Add(this.dgvStudents);
            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainerMain.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.splitContainerMain.Panel2.Controls.Add(this.pnlRight);
            this.splitContainerMain.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainerMain.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerMain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainerMain.Size = new System.Drawing.Size(1200, 574);
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
            this.pnlRight.Size = new System.Drawing.Size(502, 554);
            this.pnlRight.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.Controls.Add(this.lblCount);
            this.pnlSearch.Controls.Add(this.cmbFilterClass);
            this.pnlSearch.Controls.Add(this.cmbFilterStatus);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnReload);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSearch.Size = new System.Drawing.Size(1200, 72);
            this.pnlSearch.TabIndex = 1;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblCount.Location = new System.Drawing.Point(20, 24);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(90, 24);
            this.lblCount.TabIndex = 5;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = false;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblSearch.Location = new System.Drawing.Point(850, 24);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(54, 24);
            this.lblSearch.TabIndex = 6;
            this.lblSearch.Text = "بحث:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFilterClass
            // 
            this.cmbFilterClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFilterClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterClass.FormattingEnabled = true;
            this.cmbFilterClass.Location = new System.Drawing.Point(350, 20);
            this.cmbFilterClass.Name = "cmbFilterClass";
            this.cmbFilterClass.Size = new System.Drawing.Size(155, 30);
            this.cmbFilterClass.TabIndex = 0;
            // 
            // cmbFilterStatus
            // 
            this.cmbFilterStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.FormattingEnabled = true;
            this.cmbFilterStatus.Location = new System.Drawing.Point(538, 20);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(135, 30);
            this.cmbFilterStatus.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(676, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 30);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(896, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(118, 40);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "بحث";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFilters.Location = new System.Drawing.Point(1018, 13);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(118, 40);
            this.btnClearFilters.TabIndex = 4;
            this.btnClearFilters.Text = "مسح الفلاتر";
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new System.EventHandler(this.btnClearFilters_Click);
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Location = new System.Drawing.Point(1142, 13);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(136, 40);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "تحديث البيانات";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.AutoScroll = false;
            this.pnlButtons.BackColor = System.Drawing.Color.White;
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnStudentProfile);
            this.pnlButtons.Controls.Add(this.btnPrint);
            this.pnlButtons.Controls.Add(this.btnExportExcel);
            this.pnlButtons.Controls.Add(this.btnRefresh);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnDelete);
            this.pnlButtons.Controls.Add(this.btnUpdate);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnAdd);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlButtons.Location = new System.Drawing.Point(0, 640);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.pnlButtons.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlButtons.Size = new System.Drawing.Size(1200, 62);
            this.pnlButtons.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(14, 12);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(112, 38);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStudentProfile
            // 
            this.btnStudentProfile.Location = new System.Drawing.Point(134, 12);
            this.btnStudentProfile.Margin = new System.Windows.Forms.Padding(4);
            this.btnStudentProfile.Name = "btnStudentProfile";
            this.btnStudentProfile.Size = new System.Drawing.Size(112, 38);
            this.btnStudentProfile.TabIndex = 1;
            this.btnStudentProfile.Text = "ملف الطالب";
            this.btnStudentProfile.UseVisualStyleBackColor = true;
            this.btnStudentProfile.Click += new System.EventHandler(this.btnStudentProfile_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(254, 12);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(125, 38);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "طباعة البطاقة";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(387, 12);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(145, 38);
            this.btnExportExcel.TabIndex = 3;
            this.btnExportExcel.Text = "تصدير Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(540, 12);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(125, 38);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "تحديث القائمة";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Visible = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(673, 12);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 38);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "إلغاء";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(801, 12);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 38);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "أرشفة الطالب";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(919, 12);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(125, 38);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1052, 12);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 38);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "حفظ";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 58);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 38);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "إضافة طالب";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // StudentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlButtons);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.CancelButton = this.btnCancel;
            this.Name = "StudentsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة الطلاب";

            // Design-time theme: these are direct properties so the WinForms designer
            // can deserialize InitializeComponent without custom method invocations.
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlButtons.BackColor = System.Drawing.Color.White;

            this.gbStudent.BackColor = System.Drawing.Color.White;
            this.gbStudent.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbStudent.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbGuardian.BackColor = System.Drawing.Color.White;
            this.gbGuardian.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbGuardian.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAddress.BackColor = System.Drawing.Color.White;
            this.gbAddress.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbAddress.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPhoto.BackColor = System.Drawing.Color.White;
            this.gbPhoto.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.gbPhoto.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.dgvStudents.BackgroundColor = System.Drawing.Color.White;
            this.dgvStudents.GridColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dgvStudents.EnableHeadersVisualStyles = false;
            this.dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvStudents.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStudents.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvStudents.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.dgvStudents.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvStudents.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvStudents.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStudents.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dgvStudents.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);

            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblCount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.lblStudentNumber.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblStudentNumber.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblFullName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblGender.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblBirthDate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthPlace.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblBirthPlace.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNationality.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblNationality.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNationalId.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblNationalId.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhone.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblPhone.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuardianName.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblGuardianName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuardianRelation.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblGuardianRelation.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuardianPhone.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblGuardianPhone.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuardianEmail.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblGuardianEmail.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuardianJob.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblGuardianJob.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGovernorate.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblGovernorate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistrict.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblDistrict.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblAddress.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.txtStudentNumber.BackColor = System.Drawing.Color.White;
            this.txtStudentNumber.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtStudentNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStudentNumber.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFullName.BackColor = System.Drawing.Color.White;
            this.txtFullName.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBirthPlace.BackColor = System.Drawing.Color.White;
            this.txtBirthPlace.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtBirthPlace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBirthPlace.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNationality.BackColor = System.Drawing.Color.White;
            this.txtNationality.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtNationality.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNationality.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNationalId.BackColor = System.Drawing.Color.White;
            this.txtNationalId.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtNationalId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNationalId.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhone.BackColor = System.Drawing.Color.White;
            this.txtPhone.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuardianName.BackColor = System.Drawing.Color.White;
            this.txtGuardianName.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtGuardianName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGuardianName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuardianRelation.BackColor = System.Drawing.Color.White;
            this.txtGuardianRelation.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtGuardianRelation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGuardianRelation.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuardianPhone.BackColor = System.Drawing.Color.White;
            this.txtGuardianPhone.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtGuardianPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGuardianPhone.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuardianEmail.BackColor = System.Drawing.Color.White;
            this.txtGuardianEmail.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtGuardianEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGuardianEmail.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuardianJob.BackColor = System.Drawing.Color.White;
            this.txtGuardianJob.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtGuardianJob.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGuardianJob.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGovernorate.BackColor = System.Drawing.Color.White;
            this.txtGovernorate.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtGovernorate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGovernorate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDistrict.BackColor = System.Drawing.Color.White;
            this.txtDistrict.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtDistrict.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDistrict.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.BackColor = System.Drawing.Color.White;
            this.txtAddress.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.cmbGender.BackColor = System.Drawing.Color.White;
            this.cmbGender.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.cmbGender.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.BackColor = System.Drawing.Color.White;
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.cmbStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFilterClass.BackColor = System.Drawing.Color.White;
            this.cmbFilterClass.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.cmbFilterClass.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFilterStatus.BackColor = System.Drawing.Color.White;
            this.cmbFilterStatus.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.cmbFilterStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBirthDate.BackColor = System.Drawing.Color.White;
            this.dtpBirthDate.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dtpBirthDate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnReload.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnStudentProfile.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnChooseImage.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnRemoveImage.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);

            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.UseCompatibleTextRendering = true;
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.UseCompatibleTextRendering = true;
            this.btnReload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnReload.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStudentProfile.ForeColor = System.Drawing.Color.White;
            this.btnStudentProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudentProfile.FlatAppearance.BorderSize = 0;
            this.btnStudentProfile.UseVisualStyleBackColor = false;
            this.btnStudentProfile.UseCompatibleTextRendering = true;
            this.btnStudentProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStudentProfile.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.UseCompatibleTextRendering = true;
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrint.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.UseCompatibleTextRendering = true;
            this.btnExportExcel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportExcel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.UseCompatibleTextRendering = true;
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.UseCompatibleTextRendering = true;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.UseCompatibleTextRendering = true;
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.UseCompatibleTextRendering = true;
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.UseCompatibleTextRendering = true;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.UseCompatibleTextRendering = true;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseImage.ForeColor = System.Drawing.Color.White;
            this.btnChooseImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseImage.FlatAppearance.BorderSize = 0;
            this.btnChooseImage.UseVisualStyleBackColor = false;
            this.btnChooseImage.UseCompatibleTextRendering = true;
            this.btnChooseImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnChooseImage.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveImage.ForeColor = System.Drawing.Color.White;
            this.btnRemoveImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveImage.FlatAppearance.BorderSize = 0;
            this.btnRemoveImage.UseVisualStyleBackColor = false;
            this.btnRemoveImage.UseCompatibleTextRendering = true;
            this.btnRemoveImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRemoveImage.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Visible = true;
            this.dtpBirthDate.BackColor = System.Drawing.Color.White;
            this.dtpBirthDate.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dtpBirthDate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnReload.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);

            this.btnClose.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnStudentProfile.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);

            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.UseCompatibleTextRendering = true;
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.UseCompatibleTextRendering = true;
            this.btnReload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnReload.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStudentProfile.ForeColor = System.Drawing.Color.White;
            this.btnStudentProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudentProfile.FlatAppearance.BorderSize = 0;
            this.btnStudentProfile.UseVisualStyleBackColor = false;
            this.btnStudentProfile.UseCompatibleTextRendering = true;
            this.btnStudentProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStudentProfile.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.UseCompatibleTextRendering = true;
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrint.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.UseCompatibleTextRendering = true;
            this.btnExportExcel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportExcel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.UseCompatibleTextRendering = true;
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.UseCompatibleTextRendering = true;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.UseCompatibleTextRendering = true;
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.UseCompatibleTextRendering = true;
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.UseCompatibleTextRendering = true;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.UseCompatibleTextRendering = true;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseImage.ForeColor = System.Drawing.Color.White;
            this.btnChooseImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseImage.FlatAppearance.BorderSize = 0;
            this.btnChooseImage.UseVisualStyleBackColor = false;
            this.btnChooseImage.UseCompatibleTextRendering = true;
            this.btnChooseImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnChooseImage.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveImage.ForeColor = System.Drawing.Color.White;
            this.btnRemoveImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveImage.FlatAppearance.BorderSize = 0;
            this.btnRemoveImage.UseVisualStyleBackColor = false;
            this.btnRemoveImage.UseCompatibleTextRendering = true;
            this.btnRemoveImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRemoveImage.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Visible = true;
            this.Load += new System.EventHandler(this.StudentsForm_Load);
            this.gbStudent.ResumeLayout(false);
            this.tlpStudent.ResumeLayout(false);
            this.tlpStudent.PerformLayout();
            this.gbGuardian.ResumeLayout(false);
            this.tlpGuardian.ResumeLayout(false);
            this.tlpGuardian.PerformLayout();
            this.gbAddress.ResumeLayout(false);
            this.tlpAddress.ResumeLayout(false);
            this.tlpAddress.PerformLayout();
            this.gbPhoto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picStudent)).EndInit();
            this.layoutRight.ResumeLayout(false);
            this.layoutBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
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

        private System.Windows.Forms.Label lblStudentNumber;
        private System.Windows.Forms.TextBox txtStudentNumber;

        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;

        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cmbGender;

        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;

        private System.Windows.Forms.Label lblBirthPlace;
        private System.Windows.Forms.TextBox txtBirthPlace;

        private System.Windows.Forms.Label lblNationality;
        private System.Windows.Forms.TextBox txtNationality;

        private System.Windows.Forms.Label lblNationalId;
        private System.Windows.Forms.TextBox txtNationalId;

        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;

        private System.Windows.Forms.Label lblGuardianName;
        private System.Windows.Forms.TextBox txtGuardianName;

        private System.Windows.Forms.Label lblGuardianRelation;
        private System.Windows.Forms.TextBox txtGuardianRelation;

        private System.Windows.Forms.Label lblGuardianPhone;
        private System.Windows.Forms.TextBox txtGuardianPhone;

        private System.Windows.Forms.Label lblGuardianEmail;
        private System.Windows.Forms.TextBox txtGuardianEmail;

        private System.Windows.Forms.Label lblGuardianJob;
        private System.Windows.Forms.TextBox txtGuardianJob;

        private System.Windows.Forms.Label lblGovernorate;
        private System.Windows.Forms.TextBox txtGovernorate;

        private System.Windows.Forms.Label lblDistrict;
        private System.Windows.Forms.TextBox txtDistrict;

        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;

        // ================================================================
        // GROUPS
        // ================================================================

        private System.Windows.Forms.GroupBox gbStudent;
        private System.Windows.Forms.TableLayoutPanel tlpStudent;

        private System.Windows.Forms.GroupBox gbGuardian;
        private System.Windows.Forms.TableLayoutPanel tlpGuardian;

        private System.Windows.Forms.GroupBox gbAddress;
        private System.Windows.Forms.TableLayoutPanel tlpAddress;

        private System.Windows.Forms.GroupBox gbPhoto;

        private System.Windows.Forms.PictureBox picStudent;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.Button btnRemoveImage;

        private System.Windows.Forms.TableLayoutPanel layoutRight;
        private System.Windows.Forms.TableLayoutPanel layoutBottom;

        // ================================================================
        // MAIN
        // ================================================================

        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.ErrorProvider errorProvider1;

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel pnlRight;

        // ================================================================
        // SEARCH
        // ================================================================

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblCount;

        private System.Windows.Forms.ComboBox cmbFilterClass;
        private System.Windows.Forms.ComboBox cmbFilterStatus;

        private System.Windows.Forms.TextBox txtSearch;

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnReload;

        // ================================================================
        // BUTTONS
        // ================================================================

        private System.Windows.Forms.FlowLayoutPanel pnlButtons;

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStudentProfile;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
    }
}
