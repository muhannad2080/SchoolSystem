namespace SchoolSystem.UI
{
    partial class TimetableForm
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
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxFields = new System.Windows.Forms.GroupBox();
            this.tableLayoutFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblTimetableID = new System.Windows.Forms.Label();
            this.txtTimetableID = new System.Windows.Forms.TextBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.lblTerm = new System.Windows.Forms.Label();
            this.cmbTerm = new System.Windows.Forms.ComboBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblSection = new System.Windows.Forms.Label();
            this.cmbSection = new System.Windows.Forms.ComboBox();
            this.lblSubject = new System.Windows.Forms.Label();
            this.cmbSubject = new System.Windows.Forms.ComboBox();
            this.lblTeacher = new System.Windows.Forms.Label();
            this.cmbTeacher = new System.Windows.Forms.ComboBox();
            this.lblDay = new System.Windows.Forms.Label();
            this.cmbDay = new System.Windows.Forms.ComboBox();
            this.lblPeriodNo = new System.Windows.Forms.Label();
            this.nudPeriodNo = new System.Windows.Forms.NumericUpDown();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.lblRoom = new System.Windows.Forms.Label();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblIsActive = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dataGridViewTimetable = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.groupBoxFields.SuspendLayout();
            this.tableLayoutFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPeriodNo)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTimetable)).BeginInit();
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
            this.lblTitle.Text = "إدارة الجدول الدراسي";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainContainer
            // 
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.groupBoxFields, 0, 0);
            this.mainContainer.Controls.Add(this.panelButtons, 0, 1);
            this.mainContainer.Controls.Add(this.panelSearch, 0, 2);
            this.mainContainer.Controls.Add(this.dataGridViewTimetable, 0, 3);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 4);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 60);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.RowCount = 5;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 205F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainContainer.Size = new System.Drawing.Size(1180, 670);
            this.mainContainer.TabIndex = 1;
            // 
            // groupBoxFields
            // 
            this.groupBoxFields.BackColor = System.Drawing.Color.White;
            this.groupBoxFields.Controls.Add(this.tableLayoutFields);
            this.groupBoxFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFields.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxFields.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxFields.Location = new System.Drawing.Point(15, 13);
            this.groupBoxFields.Name = "groupBoxFields";
            this.groupBoxFields.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxFields.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxFields.Size = new System.Drawing.Size(1150, 199);
            this.groupBoxFields.TabIndex = 0;
            this.groupBoxFields.TabStop = false;
            this.groupBoxFields.Text = "بيانات الحصة الدراسية";
            // 
            // tableLayoutFields
            // 
            this.tableLayoutFields.ColumnCount = 6;
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutFields.Controls.Add(this.lblTimetableID, 0, 0);
            this.tableLayoutFields.Controls.Add(this.txtTimetableID, 1, 0);
            this.tableLayoutFields.Controls.Add(this.lblYear, 2, 0);
            this.tableLayoutFields.Controls.Add(this.txtYear, 3, 0);
            this.tableLayoutFields.Controls.Add(this.lblTerm, 4, 0);
            this.tableLayoutFields.Controls.Add(this.cmbTerm, 5, 0);
            this.tableLayoutFields.Controls.Add(this.lblClass, 0, 1);
            this.tableLayoutFields.Controls.Add(this.cmbClass, 1, 1);
            this.tableLayoutFields.Controls.Add(this.lblSection, 2, 1);
            this.tableLayoutFields.Controls.Add(this.cmbSection, 3, 1);
            this.tableLayoutFields.Controls.Add(this.lblSubject, 4, 1);
            this.tableLayoutFields.Controls.Add(this.cmbSubject, 5, 1);
            this.tableLayoutFields.Controls.Add(this.lblTeacher, 0, 2);
            this.tableLayoutFields.Controls.Add(this.cmbTeacher, 1, 2);
            this.tableLayoutFields.Controls.Add(this.lblDay, 2, 2);
            this.tableLayoutFields.Controls.Add(this.cmbDay, 3, 2);
            this.tableLayoutFields.Controls.Add(this.lblPeriodNo, 4, 2);
            this.tableLayoutFields.Controls.Add(this.nudPeriodNo, 5, 2);
            this.tableLayoutFields.Controls.Add(this.lblStart, 0, 3);
            this.tableLayoutFields.Controls.Add(this.dtpStart, 1, 3);
            this.tableLayoutFields.Controls.Add(this.lblEnd, 2, 3);
            this.tableLayoutFields.Controls.Add(this.dtpEnd, 3, 3);
            this.tableLayoutFields.Controls.Add(this.lblRoom, 4, 3);
            this.tableLayoutFields.Controls.Add(this.txtRoom, 5, 3);
            this.tableLayoutFields.Controls.Add(this.lblNotes, 0, 4);
            this.tableLayoutFields.Controls.Add(this.txtNotes, 1, 4);
            this.tableLayoutFields.Controls.Add(this.lblIsActive, 4, 4);
            this.tableLayoutFields.Controls.Add(this.chkIsActive, 5, 4);
            this.tableLayoutFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFields.Location = new System.Drawing.Point(12, 29);
            this.tableLayoutFields.Name = "tableLayoutFields";
            this.tableLayoutFields.RowCount = 5;
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.Size = new System.Drawing.Size(1126, 160);
            this.tableLayoutFields.TabIndex = 0;
            // 
            // lblTimetableID
            // 
            this.lblTimetableID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimetableID.Location = new System.Drawing.Point(1014, 0);
            this.lblTimetableID.Name = "lblTimetableID";
            this.lblTimetableID.Size = new System.Drawing.Size(109, 32);
            this.lblTimetableID.TabIndex = 0;
            this.lblTimetableID.Text = "رقم الحصة:";
            this.lblTimetableID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTimetableID
            // 
            this.txtTimetableID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtTimetableID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTimetableID.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtTimetableID.Location = new System.Drawing.Point(754, 3);
            this.txtTimetableID.Name = "txtTimetableID";
            this.txtTimetableID.ReadOnly = true;
            this.txtTimetableID.Size = new System.Drawing.Size(254, 28);
            this.txtTimetableID.TabIndex = 0;
            // 
            // lblYear
            // 
            this.lblYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblYear.Location = new System.Drawing.Point(639, 0);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(109, 32);
            this.lblYear.TabIndex = 1;
            this.lblYear.Text = "العام الدراسي:";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtYear
            // 
            this.txtYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtYear.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtYear.Location = new System.Drawing.Point(379, 3);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(254, 28);
            this.txtYear.TabIndex = 1;
            // 
            // lblTerm
            // 
            this.lblTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTerm.Location = new System.Drawing.Point(264, 0);
            this.lblTerm.Name = "lblTerm";
            this.lblTerm.Size = new System.Drawing.Size(109, 32);
            this.lblTerm.TabIndex = 2;
            this.lblTerm.Text = "الفصل:";
            this.lblTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbTerm
            // 
            this.cmbTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTerm.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbTerm.FormattingEnabled = true;
            this.cmbTerm.Location = new System.Drawing.Point(3, 3);
            this.cmbTerm.Name = "cmbTerm";
            this.cmbTerm.Size = new System.Drawing.Size(255, 29);
            this.cmbTerm.TabIndex = 2;
            // 
            // lblClass
            // 
            this.lblClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClass.Location = new System.Drawing.Point(1014, 32);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(109, 32);
            this.lblClass.TabIndex = 3;
            this.lblClass.Text = "الصف:";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbClass
            // 
            this.cmbClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Location = new System.Drawing.Point(754, 35);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(254, 29);
            this.cmbClass.TabIndex = 3;
            // 
            // lblSection
            // 
            this.lblSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSection.Location = new System.Drawing.Point(639, 32);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(109, 32);
            this.lblSection.TabIndex = 4;
            this.lblSection.Text = "الشعبة:";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSection
            // 
            this.cmbSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSection.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbSection.FormattingEnabled = true;
            this.cmbSection.Location = new System.Drawing.Point(379, 35);
            this.cmbSection.Name = "cmbSection";
            this.cmbSection.Size = new System.Drawing.Size(254, 29);
            this.cmbSection.TabIndex = 4;
            // 
            // lblSubject
            // 
            this.lblSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubject.Location = new System.Drawing.Point(264, 32);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(109, 32);
            this.lblSubject.TabIndex = 5;
            this.lblSubject.Text = "المادة:";
            this.lblSubject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSubject
            // 
            this.cmbSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubject.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbSubject.FormattingEnabled = true;
            this.cmbSubject.Location = new System.Drawing.Point(3, 35);
            this.cmbSubject.Name = "cmbSubject";
            this.cmbSubject.Size = new System.Drawing.Size(255, 29);
            this.cmbSubject.TabIndex = 5;
            // 
            // lblTeacher
            // 
            this.lblTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTeacher.Location = new System.Drawing.Point(1014, 64);
            this.lblTeacher.Name = "lblTeacher";
            this.lblTeacher.Size = new System.Drawing.Size(109, 32);
            this.lblTeacher.TabIndex = 6;
            this.lblTeacher.Text = "المعلم:";
            this.lblTeacher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbTeacher
            // 
            this.cmbTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTeacher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeacher.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbTeacher.FormattingEnabled = true;
            this.cmbTeacher.Location = new System.Drawing.Point(754, 67);
            this.cmbTeacher.Name = "cmbTeacher";
            this.cmbTeacher.Size = new System.Drawing.Size(254, 29);
            this.cmbTeacher.TabIndex = 6;
            // 
            // lblDay
            // 
            this.lblDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDay.Location = new System.Drawing.Point(639, 64);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(109, 32);
            this.lblDay.TabIndex = 7;
            this.lblDay.Text = "اليوم:";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDay
            // 
            this.cmbDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDay.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.Location = new System.Drawing.Point(379, 67);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(254, 29);
            this.cmbDay.TabIndex = 7;
            // 
            // lblPeriodNo
            // 
            this.lblPeriodNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPeriodNo.Location = new System.Drawing.Point(264, 64);
            this.lblPeriodNo.Name = "lblPeriodNo";
            this.lblPeriodNo.Size = new System.Drawing.Size(109, 32);
            this.lblPeriodNo.TabIndex = 8;
            this.lblPeriodNo.Text = "رقم الحصة:";
            this.lblPeriodNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudPeriodNo
            // 
            this.nudPeriodNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPeriodNo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.nudPeriodNo.Location = new System.Drawing.Point(3, 67);
            this.nudPeriodNo.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudPeriodNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPeriodNo.Name = "nudPeriodNo";
            this.nudPeriodNo.Size = new System.Drawing.Size(255, 28);
            this.nudPeriodNo.TabIndex = 8;
            this.nudPeriodNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblStart
            // 
            this.lblStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStart.Location = new System.Drawing.Point(1014, 96);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(109, 32);
            this.lblStart.TabIndex = 9;
            this.lblStart.Text = "وقت البداية:";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "HH:mm";
            this.dtpStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpStart.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(754, 99);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.ShowUpDown = true;
            this.dtpStart.Size = new System.Drawing.Size(254, 28);
            this.dtpStart.TabIndex = 9;
            // 
            // lblEnd
            // 
            this.lblEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEnd.Location = new System.Drawing.Point(639, 96);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(109, 32);
            this.lblEnd.TabIndex = 10;
            this.lblEnd.Text = "وقت النهاية:";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "HH:mm";
            this.dtpEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpEnd.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(379, 99);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.ShowUpDown = true;
            this.dtpEnd.Size = new System.Drawing.Size(254, 28);
            this.dtpEnd.TabIndex = 10;
            // 
            // lblRoom
            // 
            this.lblRoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoom.Location = new System.Drawing.Point(264, 96);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(109, 32);
            this.lblRoom.TabIndex = 11;
            this.lblRoom.Text = "القاعة:";
            this.lblRoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRoom
            // 
            this.txtRoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoom.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtRoom.Location = new System.Drawing.Point(3, 99);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(255, 28);
            this.txtRoom.TabIndex = 11;
            // 
            // lblNotes
            // 
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.Location = new System.Drawing.Point(1014, 128);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(109, 32);
            this.lblNotes.TabIndex = 12;
            this.lblNotes.Text = "ملاحظات:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            this.tableLayoutFields.SetColumnSpan(this.txtNotes, 3);
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtNotes.Location = new System.Drawing.Point(379, 131);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(629, 28);
            this.txtNotes.TabIndex = 12;
            // 
            // lblIsActive
            // 
            this.lblIsActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsActive.Location = new System.Drawing.Point(264, 128);
            this.lblIsActive.Name = "lblIsActive";
            this.lblIsActive.Size = new System.Drawing.Size(109, 32);
            this.lblIsActive.TabIndex = 13;
            this.lblIsActive.Text = "الحالة:";
            this.lblIsActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkIsActive
            // 
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkIsActive.Font = new System.Drawing.Font("Tahoma", 10F);
            this.chkIsActive.Location = new System.Drawing.Point(3, 131);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(255, 26);
            this.chkIsActive.TabIndex = 13;
            this.chkIsActive.Text = "الحصة نشطة";
            this.chkIsActive.UseVisualStyleBackColor = true;
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
            this.panelButtons.Location = new System.Drawing.Point(15, 218);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelButtons.Size = new System.Drawing.Size(1150, 52);
            this.panelButtons.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(3, 13);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 36);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(119, 13);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(110, 36);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "تفريغ";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(235, 13);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 36);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(351, 13);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 36);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(467, 13);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(110, 36);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "إضافة";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearch.Location = new System.Drawing.Point(15, 276);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.panelSearch.Size = new System.Drawing.Size(1150, 42);
            this.panelSearch.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(10, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(1020, 28);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearch.Location = new System.Drawing.Point(1030, 7);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(110, 28);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "بحث سريع:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewTimetable
            // 
            this.dataGridViewTimetable.AllowUserToAddRows = false;
            this.dataGridViewTimetable.AllowUserToDeleteRows = false;
            this.dataGridViewTimetable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTimetable.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewTimetable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewTimetable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewTimetable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewTimetable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTimetable.ColumnHeadersHeight = 42;
            this.dataGridViewTimetable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTimetable.EnableHeadersVisualStyles = false;
            this.dataGridViewTimetable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewTimetable.Location = new System.Drawing.Point(15, 324);
            this.dataGridViewTimetable.MultiSelect = false;
            this.dataGridViewTimetable.Name = "dataGridViewTimetable";
            this.dataGridViewTimetable.ReadOnly = true;
            this.dataGridViewTimetable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewTimetable.RowHeadersVisible = false;
            this.dataGridViewTimetable.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.dataGridViewTimetable.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTimetable.RowTemplate.Height = 34;
            this.dataGridViewTimetable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTimetable.Size = new System.Drawing.Size(1150, 301);
            this.dataGridViewTimetable.TabIndex = 3;
            this.dataGridViewTimetable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTimetable_CellClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(15, 631);
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
            this.lblRecordCount.Text = "عدد الحصص: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TimetableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "TimetableForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1180, 730);
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.groupBoxFields.ResumeLayout(false);
            this.tableLayoutFields.ResumeLayout(false);
            this.tableLayoutFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPeriodNo)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTimetable)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.TableLayoutPanel mainContainer;

        private System.Windows.Forms.GroupBox groupBoxFields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFields;

        private System.Windows.Forms.Label lblTimetableID;
        private System.Windows.Forms.TextBox txtTimetableID;

        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.TextBox txtYear;

        private System.Windows.Forms.Label lblTerm;
        private System.Windows.Forms.ComboBox cmbTerm;

        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox cmbClass;

        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.ComboBox cmbSection;

        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.ComboBox cmbSubject;

        private System.Windows.Forms.Label lblTeacher;
        private System.Windows.Forms.ComboBox cmbTeacher;

        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.ComboBox cmbDay;

        private System.Windows.Forms.Label lblPeriodNo;
        private System.Windows.Forms.NumericUpDown nudPeriodNo;

        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStart;

        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpEnd;

        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.TextBox txtRoom;

        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;

        private System.Windows.Forms.Label lblIsActive;
        private System.Windows.Forms.CheckBox chkIsActive;

        private System.Windows.Forms.FlowLayoutPanel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;

        private System.Windows.Forms.DataGridView dataGridViewTimetable;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
