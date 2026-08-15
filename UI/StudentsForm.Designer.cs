namespace SchoolSystem.UI {
    partial class StudentsForm {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent() {
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
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cmbFilterClass = new System.Windows.Forms.ComboBox();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
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
            this.lblStudentNumber.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStudentNumber.AutoSize = true;
            this.lblStudentNumber.Location = new System.Drawing.Point(628, 30);
            this.lblStudentNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStudentNumber.Name = "lblStudentNumber";
            this.lblStudentNumber.Size = new System.Drawing.Size(71, 17);
            this.lblStudentNumber.TabIndex = 0;
            this.lblStudentNumber.Text = "رقم الطالب";
            // 
            // txtStudentNumber
            // 
            this.txtStudentNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStudentNumber.Location = new System.Drawing.Point(3, 19);
            this.txtStudentNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStudentNumber.Name = "txtStudentNumber";
            this.txtStudentNumber.Size = new System.Drawing.Size(619, 24);
            this.txtStudentNumber.TabIndex = 1;
            // 
            // lblFullName
            // 
            this.lblFullName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(628, 70);
            this.lblFullName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(91, 17);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "الاسم الرباعي";
            // 
            // txtFullName
            // 
            this.txtFullName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFullName.Location = new System.Drawing.Point(3, 59);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(619, 24);
            this.txtFullName.TabIndex = 3;
            // 
            // lblGender
            // 
            this.lblGender.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(628, 110);
            this.lblGender.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(48, 17);
            this.lblGender.TabIndex = 4;
            this.lblGender.Text = "الجنس";
            // 
            // cmbGender
            // 
            this.cmbGender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGender.Location = new System.Drawing.Point(3, 99);
            this.cmbGender.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(619, 24);
            this.cmbGender.TabIndex = 5;
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBirthDate.AutoSize = true;
            this.lblBirthDate.Location = new System.Drawing.Point(628, 150);
            this.lblBirthDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(77, 17);
            this.lblBirthDate.TabIndex = 6;
            this.lblBirthDate.Text = "تاريخ الميلاد";
            // 
            // dtpBirthDate
            // 
            this.dtpBirthDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpBirthDate.Location = new System.Drawing.Point(3, 139);
            this.dtpBirthDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.Size = new System.Drawing.Size(619, 24);
            this.dtpBirthDate.TabIndex = 7;
            // 
            // lblBirthPlace
            // 
            this.lblBirthPlace.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBirthPlace.AutoSize = true;
            this.lblBirthPlace.Location = new System.Drawing.Point(628, 190);
            this.lblBirthPlace.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBirthPlace.Name = "lblBirthPlace";
            this.lblBirthPlace.Size = new System.Drawing.Size(77, 17);
            this.lblBirthPlace.TabIndex = 8;
            this.lblBirthPlace.Text = "مكان الميلاد";
            // 
            // txtBirthPlace
            // 
            this.txtBirthPlace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBirthPlace.Location = new System.Drawing.Point(3, 179);
            this.txtBirthPlace.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBirthPlace.Name = "txtBirthPlace";
            this.txtBirthPlace.Size = new System.Drawing.Size(619, 24);
            this.txtBirthPlace.TabIndex = 9;
            // 
            // lblNationality
            // 
            this.lblNationality.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblNationality.AutoSize = true;
            this.lblNationality.Location = new System.Drawing.Point(628, 230);
            this.lblNationality.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblNationality.Name = "lblNationality";
            this.lblNationality.Size = new System.Drawing.Size(58, 17);
            this.lblNationality.TabIndex = 10;
            this.lblNationality.Text = "الجنسية";
            // 
            // txtNationality
            // 
            this.txtNationality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNationality.Location = new System.Drawing.Point(3, 219);
            this.txtNationality.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNationality.Name = "txtNationality";
            this.txtNationality.Size = new System.Drawing.Size(619, 24);
            this.txtNationality.TabIndex = 11;
            // 
            // lblNationalId
            // 
            this.lblNationalId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblNationalId.AutoSize = true;
            this.lblNationalId.Location = new System.Drawing.Point(628, 270);
            this.lblNationalId.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblNationalId.Name = "lblNationalId";
            this.lblNationalId.Size = new System.Drawing.Size(66, 17);
            this.lblNationalId.TabIndex = 12;
            this.lblNationalId.Text = "رقم الهوية";
            // 
            // txtNationalId
            // 
            this.txtNationalId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNationalId.Location = new System.Drawing.Point(3, 259);
            this.txtNationalId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNationalId.Name = "txtNationalId";
            this.txtNationalId.Size = new System.Drawing.Size(619, 24);
            this.txtNationalId.TabIndex = 13;
            // 
            // lblPhone
            // 
            this.lblPhone.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(628, 310);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(80, 17);
            this.lblPhone.TabIndex = 14;
            this.lblPhone.Text = "هاتف الطالب";
            // 
            // txtPhone
            // 
            this.txtPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPhone.Location = new System.Drawing.Point(3, 299);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(619, 24);
            this.txtPhone.TabIndex = 15;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(628, 350);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 17);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "الحالة";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.Location = new System.Drawing.Point(3, 339);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(619, 24);
            this.cmbStatus.TabIndex = 17;
            // 
            // lblGuardianName
            // 
            this.lblGuardianName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGuardianName.AutoSize = true;
            this.lblGuardianName.Location = new System.Drawing.Point(628, 30);
            this.lblGuardianName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGuardianName.Name = "lblGuardianName";
            this.lblGuardianName.Size = new System.Drawing.Size(92, 17);
            this.lblGuardianName.TabIndex = 0;
            this.lblGuardianName.Text = "اسم ولي الأمر";
            // 
            // txtGuardianName
            // 
            this.txtGuardianName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGuardianName.Location = new System.Drawing.Point(3, 19);
            this.txtGuardianName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGuardianName.Name = "txtGuardianName";
            this.txtGuardianName.Size = new System.Drawing.Size(619, 24);
            this.txtGuardianName.TabIndex = 1;
            // 
            // lblGuardianRelation
            // 
            this.lblGuardianRelation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGuardianRelation.AutoSize = true;
            this.lblGuardianRelation.Location = new System.Drawing.Point(628, 70);
            this.lblGuardianRelation.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGuardianRelation.Name = "lblGuardianRelation";
            this.lblGuardianRelation.Size = new System.Drawing.Size(71, 17);
            this.lblGuardianRelation.TabIndex = 2;
            this.lblGuardianRelation.Text = "صلة القرابة";
            // 
            // txtGuardianRelation
            // 
            this.txtGuardianRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGuardianRelation.Location = new System.Drawing.Point(3, 59);
            this.txtGuardianRelation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGuardianRelation.Name = "txtGuardianRelation";
            this.txtGuardianRelation.Size = new System.Drawing.Size(619, 24);
            this.txtGuardianRelation.TabIndex = 3;
            // 
            // lblGuardianPhone
            // 
            this.lblGuardianPhone.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGuardianPhone.AutoSize = true;
            this.lblGuardianPhone.Location = new System.Drawing.Point(628, 110);
            this.lblGuardianPhone.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGuardianPhone.Name = "lblGuardianPhone";
            this.lblGuardianPhone.Size = new System.Drawing.Size(69, 17);
            this.lblGuardianPhone.TabIndex = 4;
            this.lblGuardianPhone.Text = "رقم الهاتف";
            // 
            // txtGuardianPhone
            // 
            this.txtGuardianPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGuardianPhone.Location = new System.Drawing.Point(3, 99);
            this.txtGuardianPhone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGuardianPhone.Name = "txtGuardianPhone";
            this.txtGuardianPhone.Size = new System.Drawing.Size(619, 24);
            this.txtGuardianPhone.TabIndex = 5;
            // 
            // lblGuardianEmail
            // 
            this.lblGuardianEmail.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGuardianEmail.AutoSize = true;
            this.lblGuardianEmail.Location = new System.Drawing.Point(628, 150);
            this.lblGuardianEmail.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGuardianEmail.Name = "lblGuardianEmail";
            this.lblGuardianEmail.Size = new System.Drawing.Size(100, 17);
            this.lblGuardianEmail.TabIndex = 6;
            this.lblGuardianEmail.Text = "البريد الإلكتروني";
            // 
            // txtGuardianEmail
            // 
            this.txtGuardianEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGuardianEmail.Location = new System.Drawing.Point(3, 139);
            this.txtGuardianEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGuardianEmail.Name = "txtGuardianEmail";
            this.txtGuardianEmail.Size = new System.Drawing.Size(619, 24);
            this.txtGuardianEmail.TabIndex = 7;
            // 
            // lblGuardianJob
            // 
            this.lblGuardianJob.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGuardianJob.AutoSize = true;
            this.lblGuardianJob.Location = new System.Drawing.Point(628, 190);
            this.lblGuardianJob.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGuardianJob.Name = "lblGuardianJob";
            this.lblGuardianJob.Size = new System.Drawing.Size(50, 17);
            this.lblGuardianJob.TabIndex = 8;
            this.lblGuardianJob.Text = "الوظيفة";
            // 
            // txtGuardianJob
            // 
            this.txtGuardianJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGuardianJob.Location = new System.Drawing.Point(3, 179);
            this.txtGuardianJob.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGuardianJob.Name = "txtGuardianJob";
            this.txtGuardianJob.Size = new System.Drawing.Size(619, 24);
            this.txtGuardianJob.TabIndex = 9;
            // 
            // lblGovernorate
            // 
            this.lblGovernorate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGovernorate.AutoSize = true;
            this.lblGovernorate.Location = new System.Drawing.Point(628, 30);
            this.lblGovernorate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblGovernorate.Name = "lblGovernorate";
            this.lblGovernorate.Size = new System.Drawing.Size(62, 17);
            this.lblGovernorate.TabIndex = 0;
            this.lblGovernorate.Text = "المحافظة";
            // 
            // txtGovernorate
            // 
            this.txtGovernorate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGovernorate.Location = new System.Drawing.Point(3, 19);
            this.txtGovernorate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGovernorate.Name = "txtGovernorate";
            this.txtGovernorate.Size = new System.Drawing.Size(619, 24);
            this.txtGovernorate.TabIndex = 1;
            // 
            // lblDistrict
            // 
            this.lblDistrict.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDistrict.AutoSize = true;
            this.lblDistrict.Location = new System.Drawing.Point(628, 70);
            this.lblDistrict.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDistrict.Name = "lblDistrict";
            this.lblDistrict.Size = new System.Drawing.Size(54, 17);
            this.lblDistrict.TabIndex = 2;
            this.lblDistrict.Text = "المديرية";
            // 
            // txtDistrict
            // 
            this.txtDistrict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDistrict.Location = new System.Drawing.Point(3, 59);
            this.txtDistrict.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDistrict.Name = "txtDistrict";
            this.txtDistrict.Size = new System.Drawing.Size(619, 24);
            this.txtDistrict.TabIndex = 3;
            // 
            // lblAddress
            // 
            this.lblAddress.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(628, 110);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 17);
            this.lblAddress.TabIndex = 4;
            this.lblAddress.Text = "العنوان";
            // 
            // txtAddress
            // 
            this.txtAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddress.Location = new System.Drawing.Point(3, 99);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(619, 24);
            this.txtAddress.TabIndex = 5;
            // 
            // gbStudent
            // 
            this.gbStudent.AutoSize = true;
            this.gbStudent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbStudent.Controls.Add(this.tlpStudent);
            this.gbStudent.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbStudent.Location = new System.Drawing.Point(10, 10);
            this.gbStudent.Name = "gbStudent";
            this.gbStudent.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.gbStudent.Size = new System.Drawing.Size(755, 422);
            this.gbStudent.TabIndex = 3;
            this.gbStudent.TabStop = false;
            this.gbStudent.Text = "بيانات الطالب";
            // 
            // tlpStudent
            // 
            this.tlpStudent.AutoSize = true;
            this.tlpStudent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpStudent.ColumnCount = 2;
            this.tlpStudent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpStudent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStudent.Controls.Add(this.lblStudentNumber, 0, 0);
            this.tlpStudent.Controls.Add(this.txtStudentNumber, 1, 0);
            this.tlpStudent.Controls.Add(this.lblFullName, 0, 1);
            this.tlpStudent.Controls.Add(this.txtFullName, 1, 1);
            this.tlpStudent.Controls.Add(this.lblGender, 0, 2);
            this.tlpStudent.Controls.Add(this.cmbGender, 1, 2);
            this.tlpStudent.Controls.Add(this.lblBirthDate, 0, 3);
            this.tlpStudent.Controls.Add(this.dtpBirthDate, 1, 3);
            this.tlpStudent.Controls.Add(this.lblBirthPlace, 0, 4);
            this.tlpStudent.Controls.Add(this.txtBirthPlace, 1, 4);
            this.tlpStudent.Controls.Add(this.lblNationality, 0, 5);
            this.tlpStudent.Controls.Add(this.txtNationality, 1, 5);
            this.tlpStudent.Controls.Add(this.lblNationalId, 0, 6);
            this.tlpStudent.Controls.Add(this.txtNationalId, 1, 6);
            this.tlpStudent.Controls.Add(this.lblPhone, 0, 7);
            this.tlpStudent.Controls.Add(this.txtPhone, 1, 7);
            this.tlpStudent.Controls.Add(this.lblStatus, 0, 8);
            this.tlpStudent.Controls.Add(this.cmbStatus, 1, 8);
            this.tlpStudent.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpStudent.Location = new System.Drawing.Point(10, 37);
            this.tlpStudent.Name = "tlpStudent";
            this.tlpStudent.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.tlpStudent.RowCount = 9;
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStudent.Size = new System.Drawing.Size(735, 375);
            this.tlpStudent.TabIndex = 0;
            // 
            // gbGuardian
            // 
            this.gbGuardian.AutoSize = true;
            this.gbGuardian.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbGuardian.Controls.Add(this.tlpGuardian);
            this.gbGuardian.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbGuardian.Location = new System.Drawing.Point(10, 432);
            this.gbGuardian.Name = "gbGuardian";
            this.gbGuardian.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.gbGuardian.Size = new System.Drawing.Size(755, 262);
            this.gbGuardian.TabIndex = 2;
            this.gbGuardian.TabStop = false;
            this.gbGuardian.Text = "ولي الأمر";
            // 
            // tlpGuardian
            // 
            this.tlpGuardian.AutoSize = true;
            this.tlpGuardian.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpGuardian.ColumnCount = 2;
            this.tlpGuardian.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpGuardian.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGuardian.Controls.Add(this.lblGuardianName, 0, 0);
            this.tlpGuardian.Controls.Add(this.txtGuardianName, 1, 0);
            this.tlpGuardian.Controls.Add(this.lblGuardianRelation, 0, 1);
            this.tlpGuardian.Controls.Add(this.txtGuardianRelation, 1, 1);
            this.tlpGuardian.Controls.Add(this.lblGuardianPhone, 0, 2);
            this.tlpGuardian.Controls.Add(this.txtGuardianPhone, 1, 2);
            this.tlpGuardian.Controls.Add(this.lblGuardianEmail, 0, 3);
            this.tlpGuardian.Controls.Add(this.txtGuardianEmail, 1, 3);
            this.tlpGuardian.Controls.Add(this.lblGuardianJob, 0, 4);
            this.tlpGuardian.Controls.Add(this.txtGuardianJob, 1, 4);
            this.tlpGuardian.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpGuardian.Location = new System.Drawing.Point(10, 37);
            this.tlpGuardian.Name = "tlpGuardian";
            this.tlpGuardian.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.tlpGuardian.RowCount = 5;
            this.tlpGuardian.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpGuardian.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpGuardian.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpGuardian.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpGuardian.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpGuardian.Size = new System.Drawing.Size(735, 215);
            this.tlpGuardian.TabIndex = 0;
            // 
            // gbAddress
            // 
            this.gbAddress.AutoSize = true;
            this.gbAddress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbAddress.Controls.Add(this.tlpAddress);
            this.gbAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbAddress.Location = new System.Drawing.Point(10, 694);
            this.gbAddress.Name = "gbAddress";
            this.gbAddress.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.gbAddress.Size = new System.Drawing.Size(755, 182);
            this.gbAddress.TabIndex = 1;
            this.gbAddress.TabStop = false;
            this.gbAddress.Text = "العنوان";
            // 
            // tlpAddress
            // 
            this.tlpAddress.AutoSize = true;
            this.tlpAddress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpAddress.ColumnCount = 2;
            this.tlpAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAddress.Controls.Add(this.lblGovernorate, 0, 0);
            this.tlpAddress.Controls.Add(this.txtGovernorate, 1, 0);
            this.tlpAddress.Controls.Add(this.lblDistrict, 0, 1);
            this.tlpAddress.Controls.Add(this.txtDistrict, 1, 1);
            this.tlpAddress.Controls.Add(this.lblAddress, 0, 2);
            this.tlpAddress.Controls.Add(this.txtAddress, 1, 2);
            this.tlpAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpAddress.Location = new System.Drawing.Point(10, 37);
            this.tlpAddress.Name = "tlpAddress";
            this.tlpAddress.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.tlpAddress.RowCount = 3;
            this.tlpAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpAddress.Size = new System.Drawing.Size(735, 135);
            this.tlpAddress.TabIndex = 0;
            // 
            // gbPhoto
            // 
            this.gbPhoto.Controls.Add(this.picStudent);
            this.gbPhoto.Controls.Add(this.btnChooseImage);
            this.gbPhoto.Controls.Add(this.btnRemoveImage);
            this.gbPhoto.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbPhoto.Location = new System.Drawing.Point(10, 876);
            this.gbPhoto.Name = "gbPhoto";
            this.gbPhoto.Padding = new System.Windows.Forms.Padding(10);
            this.gbPhoto.Size = new System.Drawing.Size(755, 160);
            this.gbPhoto.TabIndex = 0;
            this.gbPhoto.TabStop = false;
            this.gbPhoto.Text = "الصورة";
            // 
            // picStudent
            // 
            this.picStudent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picStudent.Dock = System.Windows.Forms.DockStyle.Right;
            this.picStudent.Location = new System.Drawing.Point(635, 27);
            this.picStudent.Name = "picStudent";
            this.picStudent.Size = new System.Drawing.Size(110, 123);
            this.picStudent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picStudent.TabIndex = 0;
            this.picStudent.TabStop = false;
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.Location = new System.Drawing.Point(30, 40);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(120, 35);
            this.btnChooseImage.TabIndex = 1;
            this.btnChooseImage.Text = "اختيار الصورة";
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);
            // 
            // btnRemoveImage
            // 
            this.btnRemoveImage.Location = new System.Drawing.Point(30, 85);
            this.btnRemoveImage.Name = "btnRemoveImage";
            this.btnRemoveImage.Size = new System.Drawing.Size(120, 35);
            this.btnRemoveImage.TabIndex = 2;
            this.btnRemoveImage.Text = "حذف الصورة";
            this.btnRemoveImage.Click += new System.EventHandler(this.btnRemoveImage_Click);
            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.BackgroundColor = System.Drawing.Color.White;
            this.dgvStudents.ColumnHeadersHeight = 29;
            this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStudents.Location = new System.Drawing.Point(10, 10);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersWidth = 51;
            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.Size = new System.Drawing.Size(390, 600);
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
            this.splitContainerMain.Location = new System.Drawing.Point(0, 60);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.dgvStudents);
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
            this.pnlRight.Controls.Add(this.gbPhoto);
            this.pnlRight.Controls.Add(this.gbAddress);
            this.pnlRight.Controls.Add(this.gbGuardian);
            this.pnlRight.Controls.Add(this.gbStudent);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(0, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10);
            this.pnlRight.Size = new System.Drawing.Size(796, 620);
            this.pnlRight.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.cmbFilterClass);
            this.pnlSearch.Controls.Add(this.cmbFilterStatus);
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
            // cmbFilterClass
            // 
            this.cmbFilterClass.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbFilterClass.Location = new System.Drawing.Point(373, 10);
            this.cmbFilterClass.Name = "cmbFilterClass";
            this.cmbFilterClass.Size = new System.Drawing.Size(216, 24);
            this.cmbFilterClass.TabIndex = 0;
            // 
            // cmbFilterStatus
            // 
            this.cmbFilterStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbFilterStatus.Location = new System.Drawing.Point(589, 10);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(120, 24);
            this.cmbFilterStatus.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtSearch.Location = new System.Drawing.Point(709, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(215, 24);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(924, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 40);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "بحث";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReload
            // 
            this.btnReload.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReload.Location = new System.Drawing.Point(1034, 10);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(156, 40);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "تحديث";
            this.btnReload.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCount.Location = new System.Drawing.Point(10, 10);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 17);
            this.lblCount.TabIndex = 5;
            // 
            // pnlButtons
            // 
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
            this.pnlButtons.Location = new System.Drawing.Point(0, 680);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(10);
            this.pnlButtons.Size = new System.Drawing.Size(1200, 70);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(380, 10);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 50);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "إغلاق";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStudentProfile
            //
            this.btnStudentProfile.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnStudentProfile.Location = new System.Drawing.Point(380, 10);
            this.btnStudentProfile.Margin = new System.Windows.Forms.Padding(5);
            this.btnStudentProfile.Name = "btnStudentProfile";
            this.btnStudentProfile.Size = new System.Drawing.Size(110, 50);
            this.btnStudentProfile.TabIndex = 1;
            this.btnStudentProfile.Text = "ملف الطالب";
            this.btnStudentProfile.Click += new System.EventHandler(this.btnStudentProfile_Click);
            //
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(470, 10);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(90, 50);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "طباعة";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExportExcel.Location = new System.Drawing.Point(560, 10);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(5);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(90, 50);
            this.btnExportExcel.TabIndex = 2;
            this.btnExportExcel.Text = "تصدير Excel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
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
            // StudentsForm
            // 
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlButtons);
            this.Name = "StudentsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "إدارة الطلاب";
            this.Load += new System.EventHandler(this.StudentsForm_Load);
            this.gbStudent.ResumeLayout(false);
            this.gbStudent.PerformLayout();
            this.tlpStudent.ResumeLayout(false);
            this.tlpStudent.PerformLayout();
            this.gbGuardian.ResumeLayout(false);
            this.gbGuardian.PerformLayout();
            this.tlpGuardian.ResumeLayout(false);
            this.tlpGuardian.PerformLayout();
            this.gbAddress.ResumeLayout(false);
            this.gbAddress.PerformLayout();
            this.tlpAddress.ResumeLayout(false);
            this.tlpAddress.PerformLayout();
            this.gbPhoto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picStudent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
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
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.ComboBox cmbFilterClass;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStudentProfile;
        private System.Windows.Forms.Panel pnlRight;
    }
}