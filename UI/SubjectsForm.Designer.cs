namespace SchoolSystem.UI
{
    partial class SubjectsForm
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
            this.lblSubjectID = new System.Windows.Forms.Label();
            this.txtSubjectID = new System.Windows.Forms.TextBox();
            this.lblSubjectCode = new System.Windows.Forms.Label();
            this.txtSubjectCode = new System.Windows.Forms.TextBox();
            this.lblSubjectName = new System.Windows.Forms.Label();
            this.txtSubjectName = new System.Windows.Forms.TextBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblMaxDegree = new System.Windows.Forms.Label();
            this.nudMaxDegree = new System.Windows.Forms.NumericUpDown();
            this.lblPassDegree = new System.Windows.Forms.Label();
            this.nudPassDegree = new System.Windows.Forms.NumericUpDown();
            this.lblIsActive = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
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
            this.dataGridViewSubjects = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.groupBoxFields.SuspendLayout();
            this.tableLayoutFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxDegree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPassDegree)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).BeginInit();
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
            this.panelTitle.Size = new System.Drawing.Size(1100, 60);
            this.panelTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1100, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "إدارة المواد الدراسية الثابتة";
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
            this.mainContainer.Controls.Add(this.dataGridViewSubjects, 0, 3);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 4);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 60);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.RowCount = 5;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainContainer.Size = new System.Drawing.Size(1100, 670);
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
            this.groupBoxFields.Size = new System.Drawing.Size(1070, 179);
            this.groupBoxFields.TabIndex = 0;
            this.groupBoxFields.TabStop = false;
            this.groupBoxFields.Text = "بيانات المادة وإعداداتها";
            // 
            // tableLayoutFields
            // 
            this.tableLayoutFields.ColumnCount = 4;
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutFields.Controls.Add(this.lblSubjectID, 0, 0);
            this.tableLayoutFields.Controls.Add(this.txtSubjectID, 1, 0);
            this.tableLayoutFields.Controls.Add(this.lblSubjectCode, 2, 0);
            this.tableLayoutFields.Controls.Add(this.txtSubjectCode, 3, 0);
            this.tableLayoutFields.Controls.Add(this.lblSubjectName, 0, 1);
            this.tableLayoutFields.Controls.Add(this.txtSubjectName, 1, 1);
            this.tableLayoutFields.Controls.Add(this.lblClass, 2, 1);
            this.tableLayoutFields.Controls.Add(this.cmbClass, 3, 1);
            this.tableLayoutFields.Controls.Add(this.lblMaxDegree, 0, 2);
            this.tableLayoutFields.Controls.Add(this.nudMaxDegree, 1, 2);
            this.tableLayoutFields.Controls.Add(this.lblPassDegree, 2, 2);
            this.tableLayoutFields.Controls.Add(this.nudPassDegree, 3, 2);
            this.tableLayoutFields.Controls.Add(this.lblIsActive, 0, 3);
            this.tableLayoutFields.Controls.Add(this.chkIsActive, 1, 3);
            this.tableLayoutFields.Controls.Add(this.lblNotes, 2, 3);
            this.tableLayoutFields.Controls.Add(this.txtNotes, 3, 3);
            this.tableLayoutFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFields.Location = new System.Drawing.Point(12, 29);
            this.tableLayoutFields.Name = "tableLayoutFields";
            this.tableLayoutFields.RowCount = 4;
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.Size = new System.Drawing.Size(1046, 140);
            this.tableLayoutFields.TabIndex = 0;
            // 
            // lblSubjectID
            // 
            this.lblSubjectID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubjectID.Location = new System.Drawing.Point(919, 0);
            this.lblSubjectID.Name = "lblSubjectID";
            this.lblSubjectID.Size = new System.Drawing.Size(124, 35);
            this.lblSubjectID.TabIndex = 0;
            this.lblSubjectID.Text = "رقم المادة:";
            this.lblSubjectID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSubjectID
            // 
            this.txtSubjectID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtSubjectID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSubjectID.Location = new System.Drawing.Point(526, 3);
            this.txtSubjectID.Name = "txtSubjectID";
            this.txtSubjectID.ReadOnly = true;
            this.txtSubjectID.Size = new System.Drawing.Size(387, 28);
            this.txtSubjectID.TabIndex = 0;
            // 
            // lblSubjectCode
            // 
            this.lblSubjectCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubjectCode.Location = new System.Drawing.Point(396, 0);
            this.lblSubjectCode.Name = "lblSubjectCode";
            this.lblSubjectCode.Size = new System.Drawing.Size(124, 35);
            this.lblSubjectCode.TabIndex = 1;
            this.lblSubjectCode.Text = "كود المادة:";
            this.lblSubjectCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSubjectCode
            // 
            this.txtSubjectCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtSubjectCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSubjectCode.Location = new System.Drawing.Point(3, 3);
            this.txtSubjectCode.Name = "txtSubjectCode";
            this.txtSubjectCode.ReadOnly = true;
            this.txtSubjectCode.Size = new System.Drawing.Size(387, 28);
            this.txtSubjectCode.TabIndex = 1;
            // 
            // lblSubjectName
            // 
            this.lblSubjectName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubjectName.Location = new System.Drawing.Point(919, 35);
            this.lblSubjectName.Name = "lblSubjectName";
            this.lblSubjectName.Size = new System.Drawing.Size(124, 35);
            this.lblSubjectName.TabIndex = 2;
            this.lblSubjectName.Text = "اسم المادة:";
            this.lblSubjectName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSubjectName
            // 
            this.txtSubjectName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtSubjectName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSubjectName.Location = new System.Drawing.Point(526, 38);
            this.txtSubjectName.Name = "txtSubjectName";
            this.txtSubjectName.ReadOnly = true;
            this.txtSubjectName.Size = new System.Drawing.Size(387, 28);
            this.txtSubjectName.TabIndex = 2;
            // 
            // lblClass
            // 
            this.lblClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClass.Location = new System.Drawing.Point(396, 35);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(124, 35);
            this.lblClass.TabIndex = 3;
            this.lblClass.Text = "الصف:";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbClass
            // 
            this.cmbClass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.cmbClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.Enabled = false;
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Location = new System.Drawing.Point(3, 38);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(387, 29);
            this.cmbClass.TabIndex = 3;
            // 
            // lblMaxDegree
            // 
            this.lblMaxDegree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMaxDegree.Location = new System.Drawing.Point(919, 70);
            this.lblMaxDegree.Name = "lblMaxDegree";
            this.lblMaxDegree.Size = new System.Drawing.Size(124, 35);
            this.lblMaxDegree.TabIndex = 4;
            this.lblMaxDegree.Text = "الدرجة الكبرى:";
            this.lblMaxDegree.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudMaxDegree
            // 
            this.nudMaxDegree.DecimalPlaces = 2;
            this.nudMaxDegree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMaxDegree.Location = new System.Drawing.Point(526, 73);
            this.nudMaxDegree.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMaxDegree.Name = "nudMaxDegree";
            this.nudMaxDegree.Size = new System.Drawing.Size(387, 28);
            this.nudMaxDegree.TabIndex = 4;
            this.nudMaxDegree.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lblPassDegree
            // 
            this.lblPassDegree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPassDegree.Location = new System.Drawing.Point(396, 70);
            this.lblPassDegree.Name = "lblPassDegree";
            this.lblPassDegree.Size = new System.Drawing.Size(124, 35);
            this.lblPassDegree.TabIndex = 5;
            this.lblPassDegree.Text = "درجة النجاح:";
            this.lblPassDegree.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudPassDegree
            // 
            this.nudPassDegree.DecimalPlaces = 2;
            this.nudPassDegree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPassDegree.Location = new System.Drawing.Point(3, 73);
            this.nudPassDegree.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudPassDegree.Name = "nudPassDegree";
            this.nudPassDegree.Size = new System.Drawing.Size(387, 28);
            this.nudPassDegree.TabIndex = 5;
            this.nudPassDegree.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblIsActive
            // 
            this.lblIsActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsActive.Location = new System.Drawing.Point(919, 105);
            this.lblIsActive.Name = "lblIsActive";
            this.lblIsActive.Size = new System.Drawing.Size(124, 35);
            this.lblIsActive.TabIndex = 6;
            this.lblIsActive.Text = "الحالة:";
            this.lblIsActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkIsActive
            // 
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkIsActive.Location = new System.Drawing.Point(526, 108);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(387, 29);
            this.chkIsActive.TabIndex = 6;
            this.chkIsActive.Text = "المادة نشطة";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // lblNotes
            // 
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.Location = new System.Drawing.Point(396, 105);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(124, 35);
            this.lblNotes.TabIndex = 7;
            this.lblNotes.Text = "ملاحظات:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Location = new System.Drawing.Point(3, 108);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(387, 28);
            this.txtNotes.TabIndex = 7;
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
            this.panelButtons.Location = new System.Drawing.Point(15, 198);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelButtons.Size = new System.Drawing.Size(1070, 52);
            this.panelButtons.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(235, 13);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 36);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "حذف معطل";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(351, 13);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 36);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "حفظ التعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(467, 13);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(140, 36);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "تثبيت المواد";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearch.Location = new System.Drawing.Point(15, 256);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.panelSearch.Size = new System.Drawing.Size(1070, 42);
            this.panelSearch.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(10, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(930, 28);
            this.txtSearch.TabIndex = 0;
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearch.Location = new System.Drawing.Point(940, 7);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(120, 28);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "بحث سريع:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewSubjects
            // 
            this.dataGridViewSubjects.AllowUserToAddRows = false;
            this.dataGridViewSubjects.AllowUserToDeleteRows = false;
            this.dataGridViewSubjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSubjects.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewSubjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewSubjects.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewSubjects.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewSubjects.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSubjects.ColumnHeadersHeight = 42;
            this.dataGridViewSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSubjects.EnableHeadersVisualStyles = false;
            this.dataGridViewSubjects.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewSubjects.Location = new System.Drawing.Point(15, 304);
            this.dataGridViewSubjects.MultiSelect = false;
            this.dataGridViewSubjects.Name = "dataGridViewSubjects";
            this.dataGridViewSubjects.ReadOnly = true;
            this.dataGridViewSubjects.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewSubjects.RowHeadersVisible = false;
            this.dataGridViewSubjects.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.dataGridViewSubjects.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSubjects.RowTemplate.Height = 34;
            this.dataGridViewSubjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSubjects.Size = new System.Drawing.Size(1070, 321);
            this.dataGridViewSubjects.TabIndex = 3;
            this.dataGridViewSubjects.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSubjects_CellClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(15, 631);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1070, 26);
            this.panelBottom.TabIndex = 4;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(1070, 26);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد المواد: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SubjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "SubjectsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1100, 730);
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.groupBoxFields.ResumeLayout(false);
            this.tableLayoutFields.ResumeLayout(false);
            this.tableLayoutFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxDegree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPassDegree)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.TableLayoutPanel mainContainer;

        private System.Windows.Forms.GroupBox groupBoxFields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFields;

        private System.Windows.Forms.Label lblSubjectID;
        private System.Windows.Forms.TextBox txtSubjectID;

        private System.Windows.Forms.Label lblSubjectCode;
        private System.Windows.Forms.TextBox txtSubjectCode;

        private System.Windows.Forms.Label lblSubjectName;
        private System.Windows.Forms.TextBox txtSubjectName;

        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox cmbClass;

        private System.Windows.Forms.Label lblMaxDegree;
        private System.Windows.Forms.NumericUpDown nudMaxDegree;

        private System.Windows.Forms.Label lblPassDegree;
        private System.Windows.Forms.NumericUpDown nudPassDegree;

        private System.Windows.Forms.Label lblIsActive;
        private System.Windows.Forms.CheckBox chkIsActive;

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

        private System.Windows.Forms.DataGridView dataGridViewSubjects;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
