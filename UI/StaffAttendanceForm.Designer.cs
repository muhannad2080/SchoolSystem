namespace SchoolSystem.UI
{
    partial class StaffAttendanceForm
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
            this.panelTitle = new Krypton.Toolkit.KryptonPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxFields = new System.Windows.Forms.GroupBox();
            this.tableLayoutFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblTeacher = new System.Windows.Forms.Label();
            this.cmbTeacher = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblCheckIn = new System.Windows.Forms.Label();
            this.dtpCheckIn = new System.Windows.Forms.DateTimePicker();
            this.lblCheckOut = new System.Windows.Forms.Label();
            this.dtpCheckOut = new System.Windows.Forms.DateTimePicker();
            this.lblWorkHours = new System.Windows.Forms.Label();
            this.txtWorkHours = new System.Windows.Forms.TextBox();
            this.lblLateMinutes = new System.Windows.Forms.Label();
            this.txtLateMinutes = new System.Windows.Forms.TextBox();
            this.lblEarlyLeaveMinutes = new System.Windows.Forms.Label();
            this.txtEarlyLeaveMinutes = new System.Windows.Forms.TextBox();
            this.lblAbsenceReason = new System.Windows.Forms.Label();
            this.txtAbsenceReason = new System.Windows.Forms.TextBox();
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
            this.dataGridViewAttendance = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelTitle)).BeginInit();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.groupBoxFields.SuspendLayout();
            this.tableLayoutFields.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAttendance)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1050, 58);
            this.panelTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1050, 58);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "الحضور والانصراف للمعلمين";
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
            this.mainContainer.Controls.Add(this.dataGridViewAttendance, 0, 3);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 4);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 58);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.RowCount = 5;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainContainer.Size = new System.Drawing.Size(1050, 642);
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
            this.groupBoxFields.Size = new System.Drawing.Size(1020, 209);
            this.groupBoxFields.TabIndex = 0;
            this.groupBoxFields.TabStop = false;
            this.groupBoxFields.Text = "بيانات الحضور والانصراف";
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
            this.tableLayoutFields.Controls.Add(this.lblTeacher, 0, 0);
            this.tableLayoutFields.Controls.Add(this.cmbTeacher, 1, 0);
            this.tableLayoutFields.Controls.Add(this.lblDate, 2, 0);
            this.tableLayoutFields.Controls.Add(this.dtpDate, 3, 0);
            this.tableLayoutFields.Controls.Add(this.lblStatus, 4, 0);
            this.tableLayoutFields.Controls.Add(this.cmbStatus, 5, 0);
            this.tableLayoutFields.Controls.Add(this.lblCheckIn, 0, 1);
            this.tableLayoutFields.Controls.Add(this.dtpCheckIn, 1, 1);
            this.tableLayoutFields.Controls.Add(this.lblCheckOut, 2, 1);
            this.tableLayoutFields.Controls.Add(this.dtpCheckOut, 3, 1);
            this.tableLayoutFields.Controls.Add(this.lblWorkHours, 4, 1);
            this.tableLayoutFields.Controls.Add(this.txtWorkHours, 5, 1);
            this.tableLayoutFields.Controls.Add(this.lblLateMinutes, 0, 2);
            this.tableLayoutFields.Controls.Add(this.txtLateMinutes, 1, 2);
            this.tableLayoutFields.Controls.Add(this.lblEarlyLeaveMinutes, 2, 2);
            this.tableLayoutFields.Controls.Add(this.txtEarlyLeaveMinutes, 3, 2);
            this.tableLayoutFields.Controls.Add(this.lblAbsenceReason, 4, 2);
            this.tableLayoutFields.Controls.Add(this.txtAbsenceReason, 5, 2);
            this.tableLayoutFields.Controls.Add(this.lblNotes, 0, 3);
            this.tableLayoutFields.Controls.Add(this.txtNotes, 1, 3);
            this.tableLayoutFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFields.Location = new System.Drawing.Point(12, 29);
            this.tableLayoutFields.Name = "tableLayoutFields";
            this.tableLayoutFields.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.RowCount = 4;
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.Size = new System.Drawing.Size(996, 170);
            this.tableLayoutFields.TabIndex = 0;
            // 
            // lblTeacher
            // 
            this.lblTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTeacher.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTeacher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblTeacher.Location = new System.Drawing.Point(880, 4);
            this.lblTeacher.Name = "lblTeacher";
            this.lblTeacher.Size = new System.Drawing.Size(109, 40);
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
            this.cmbTeacher.Location = new System.Drawing.Point(667, 8);
            this.cmbTeacher.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTeacher.Name = "cmbTeacher";
            this.cmbTeacher.Size = new System.Drawing.Size(206, 27);
            this.cmbTeacher.TabIndex = 0;
            // 
            // lblDate
            // 
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblDate.Location = new System.Drawing.Point(551, 4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(109, 40);
            this.lblDate.TabIndex = 1;
            this.lblDate.Text = "التاريخ:";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(338, 8);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(206, 27);
            this.dtpDate.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblStatus.Location = new System.Drawing.Point(222, 4);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(109, 40);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "الحالة:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbStatus.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(8, 8);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(4);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(207, 27);
            this.cmbStatus.TabIndex = 2;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            // 
            // lblCheckIn
            // 
            this.lblCheckIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCheckIn.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCheckIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblCheckIn.Location = new System.Drawing.Point(880, 44);
            this.lblCheckIn.Name = "lblCheckIn";
            this.lblCheckIn.Size = new System.Drawing.Size(109, 40);
            this.lblCheckIn.TabIndex = 3;
            this.lblCheckIn.Text = "وقت الحضور:";
            this.lblCheckIn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpCheckIn
            // 
            this.dtpCheckIn.CustomFormat = "HH:mm";
            this.dtpCheckIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpCheckIn.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dtpCheckIn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCheckIn.Location = new System.Drawing.Point(667, 48);
            this.dtpCheckIn.Margin = new System.Windows.Forms.Padding(4);
            this.dtpCheckIn.Name = "dtpCheckIn";
            this.dtpCheckIn.ShowUpDown = true;
            this.dtpCheckIn.Size = new System.Drawing.Size(206, 27);
            this.dtpCheckIn.TabIndex = 3;
            this.dtpCheckIn.ValueChanged += new System.EventHandler(this.dtpCheckIn_ValueChanged);
            // 
            // lblCheckOut
            // 
            this.lblCheckOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCheckOut.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCheckOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblCheckOut.Location = new System.Drawing.Point(551, 44);
            this.lblCheckOut.Name = "lblCheckOut";
            this.lblCheckOut.Size = new System.Drawing.Size(109, 40);
            this.lblCheckOut.TabIndex = 4;
            this.lblCheckOut.Text = "وقت الانصراف:";
            this.lblCheckOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpCheckOut
            // 
            this.dtpCheckOut.CustomFormat = "HH:mm";
            this.dtpCheckOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpCheckOut.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dtpCheckOut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCheckOut.Location = new System.Drawing.Point(338, 48);
            this.dtpCheckOut.Margin = new System.Windows.Forms.Padding(4);
            this.dtpCheckOut.Name = "dtpCheckOut";
            this.dtpCheckOut.ShowUpDown = true;
            this.dtpCheckOut.Size = new System.Drawing.Size(206, 27);
            this.dtpCheckOut.TabIndex = 4;
            this.dtpCheckOut.ValueChanged += new System.EventHandler(this.dtpCheckOut_ValueChanged);
            // 
            // lblWorkHours
            // 
            this.lblWorkHours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWorkHours.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblWorkHours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblWorkHours.Location = new System.Drawing.Point(222, 44);
            this.lblWorkHours.Name = "lblWorkHours";
            this.lblWorkHours.Size = new System.Drawing.Size(109, 40);
            this.lblWorkHours.TabIndex = 5;
            this.lblWorkHours.Text = "ساعات العمل:";
            this.lblWorkHours.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWorkHours
            // 
            this.txtWorkHours.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtWorkHours.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWorkHours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWorkHours.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtWorkHours.Location = new System.Drawing.Point(8, 48);
            this.txtWorkHours.Margin = new System.Windows.Forms.Padding(4);
            this.txtWorkHours.Name = "txtWorkHours";
            this.txtWorkHours.ReadOnly = true;
            this.txtWorkHours.Size = new System.Drawing.Size(207, 27);
            this.txtWorkHours.TabIndex = 5;
            this.txtWorkHours.Text = "0";
            this.txtWorkHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLateMinutes
            // 
            this.lblLateMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLateMinutes.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblLateMinutes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblLateMinutes.Location = new System.Drawing.Point(880, 84);
            this.lblLateMinutes.Name = "lblLateMinutes";
            this.lblLateMinutes.Size = new System.Drawing.Size(109, 40);
            this.lblLateMinutes.TabIndex = 6;
            this.lblLateMinutes.Text = "دقائق التأخير:";
            this.lblLateMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLateMinutes
            // 
            this.txtLateMinutes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtLateMinutes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLateMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLateMinutes.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtLateMinutes.Location = new System.Drawing.Point(667, 88);
            this.txtLateMinutes.Margin = new System.Windows.Forms.Padding(4);
            this.txtLateMinutes.Name = "txtLateMinutes";
            this.txtLateMinutes.ReadOnly = true;
            this.txtLateMinutes.Size = new System.Drawing.Size(206, 27);
            this.txtLateMinutes.TabIndex = 6;
            this.txtLateMinutes.Text = "0";
            this.txtLateMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEarlyLeaveMinutes
            // 
            this.lblEarlyLeaveMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEarlyLeaveMinutes.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEarlyLeaveMinutes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblEarlyLeaveMinutes.Location = new System.Drawing.Point(551, 84);
            this.lblEarlyLeaveMinutes.Name = "lblEarlyLeaveMinutes";
            this.lblEarlyLeaveMinutes.Size = new System.Drawing.Size(109, 40);
            this.lblEarlyLeaveMinutes.TabIndex = 7;
            this.lblEarlyLeaveMinutes.Text = "خروج مبكر:";
            this.lblEarlyLeaveMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEarlyLeaveMinutes
            // 
            this.txtEarlyLeaveMinutes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtEarlyLeaveMinutes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEarlyLeaveMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEarlyLeaveMinutes.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtEarlyLeaveMinutes.Location = new System.Drawing.Point(338, 88);
            this.txtEarlyLeaveMinutes.Margin = new System.Windows.Forms.Padding(4);
            this.txtEarlyLeaveMinutes.Name = "txtEarlyLeaveMinutes";
            this.txtEarlyLeaveMinutes.ReadOnly = true;
            this.txtEarlyLeaveMinutes.Size = new System.Drawing.Size(206, 27);
            this.txtEarlyLeaveMinutes.TabIndex = 7;
            this.txtEarlyLeaveMinutes.Text = "0";
            this.txtEarlyLeaveMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblAbsenceReason
            // 
            this.lblAbsenceReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAbsenceReason.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAbsenceReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblAbsenceReason.Location = new System.Drawing.Point(222, 84);
            this.lblAbsenceReason.Name = "lblAbsenceReason";
            this.lblAbsenceReason.Size = new System.Drawing.Size(109, 40);
            this.lblAbsenceReason.TabIndex = 8;
            this.lblAbsenceReason.Text = "السبب:";
            this.lblAbsenceReason.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAbsenceReason
            // 
            this.txtAbsenceReason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtAbsenceReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAbsenceReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAbsenceReason.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtAbsenceReason.Location = new System.Drawing.Point(8, 88);
            this.txtAbsenceReason.Margin = new System.Windows.Forms.Padding(4);
            this.txtAbsenceReason.Name = "txtAbsenceReason";
            this.txtAbsenceReason.Size = new System.Drawing.Size(207, 27);
            this.txtAbsenceReason.TabIndex = 8;
            this.txtAbsenceReason.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAbsenceReason_KeyPress);
            // 
            // lblNotes
            // 
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblNotes.Location = new System.Drawing.Point(880, 124);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(109, 42);
            this.lblNotes.TabIndex = 9;
            this.lblNotes.Text = "ملاحظات:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutFields.SetColumnSpan(this.txtNotes, 5);
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtNotes.Location = new System.Drawing.Point(8, 128);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(865, 27);
            this.txtNotes.TabIndex = 9;
            this.txtNotes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNotes_KeyPress);
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
            this.panelButtons.Location = new System.Drawing.Point(15, 228);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panelButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelButtons.Size = new System.Drawing.Size(1020, 46);
            this.panelButtons.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(5, 6);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(115, 36);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(130, 6);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(115, 36);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "تفريغ";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(255, 6);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(115, 36);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(380, 6);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(115, 36);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(505, 6);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(115, 36);
            this.btnAdd.TabIndex = 10;
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
            this.panelSearch.Location = new System.Drawing.Point(15, 280);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.panelSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelSearch.Size = new System.Drawing.Size(1020, 42);
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
            this.txtSearch.Size = new System.Drawing.Size(880, 28);
            this.txtSearch.TabIndex = 15;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblSearch.Location = new System.Drawing.Point(890, 7);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(120, 28);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث سريع:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewAttendance
            // 
            this.dataGridViewAttendance.AllowUserToAddRows = false;
            this.dataGridViewAttendance.AllowUserToDeleteRows = false;
            this.dataGridViewAttendance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAttendance.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewAttendance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewAttendance.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewAttendance.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAttendance.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewAttendance.ColumnHeadersHeight = 42;
            this.dataGridViewAttendance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewAttendance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAttendance.EnableHeadersVisualStyles = false;
            this.dataGridViewAttendance.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewAttendance.Location = new System.Drawing.Point(15, 328);
            this.dataGridViewAttendance.MultiSelect = false;
            this.dataGridViewAttendance.Name = "dataGridViewAttendance";
            this.dataGridViewAttendance.ReadOnly = true;
            this.dataGridViewAttendance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewAttendance.RowHeadersVisible = false;
            this.dataGridViewAttendance.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.dataGridViewAttendance.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewAttendance.RowTemplate.Height = 34;
            this.dataGridViewAttendance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAttendance.Size = new System.Drawing.Size(1020, 269);
            this.dataGridViewAttendance.TabIndex = 3;
            this.dataGridViewAttendance.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAttendance_CellClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(15, 603);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1020, 26);
            this.panelBottom.TabIndex = 4;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(1020, 26);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد السجلات: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StaffAttendanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "StaffAttendanceForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1050, 700);
            ((System.ComponentModel.ISupportInitialize)(this.panelTitle)).EndInit();
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.groupBoxFields.ResumeLayout(false);
            this.tableLayoutFields.ResumeLayout(false);
            this.tableLayoutFields.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAttendance)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel mainContainer;

        private System.Windows.Forms.GroupBox groupBoxFields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFields;

        private System.Windows.Forms.Label lblTeacher;
        private System.Windows.Forms.ComboBox cmbTeacher;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblCheckIn;
        private System.Windows.Forms.DateTimePicker dtpCheckIn;

        private System.Windows.Forms.Label lblCheckOut;
        private System.Windows.Forms.DateTimePicker dtpCheckOut;
        private System.Windows.Forms.Label lblAbsenceReason;
        private System.Windows.Forms.TextBox txtAbsenceReason;

        private System.Windows.Forms.Label lblLateMinutes;
        private System.Windows.Forms.TextBox txtLateMinutes;
        private System.Windows.Forms.Label lblEarlyLeaveMinutes;
        private System.Windows.Forms.TextBox txtEarlyLeaveMinutes;

        private System.Windows.Forms.Label lblWorkHours;
        private System.Windows.Forms.TextBox txtWorkHours;
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

        private System.Windows.Forms.DataGridView dataGridViewAttendance;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
