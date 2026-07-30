namespace SchoolSystem.UI
{
    partial class ClassesForm
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
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabClasses = new System.Windows.Forms.TabPage();
            this.mainClasses = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxClassFields = new System.Windows.Forms.GroupBox();
            this.tableClassFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblClassID = new System.Windows.Forms.Label();
            this.txtClassID = new System.Windows.Forms.TextBox();
            this.lblClassCode = new System.Windows.Forms.Label();
            this.txtClassCode = new System.Windows.Forms.TextBox();
            this.lblClassName = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.lblStageName = new System.Windows.Forms.Label();
            this.txtStageName = new System.Windows.Forms.TextBox();
            this.lblGradeOrder = new System.Windows.Forms.Label();
            this.nudGradeOrder = new System.Windows.Forms.NumericUpDown();
            this.lblClassActive = new System.Windows.Forms.Label();
            this.chkClassActive = new System.Windows.Forms.CheckBox();
            this.lblClassNotes = new System.Windows.Forms.Label();
            this.txtClassNotes = new System.Windows.Forms.TextBox();
            this.panelClassButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClassRefresh = new System.Windows.Forms.Button();
            this.btnClassClear = new System.Windows.Forms.Button();
            this.btnClassUpdate = new System.Windows.Forms.Button();
            this.panelClassSearch = new System.Windows.Forms.Panel();
            this.txtClassSearch = new System.Windows.Forms.TextBox();
            this.lblClassSearch = new System.Windows.Forms.Label();
            this.dataGridViewClasses = new System.Windows.Forms.DataGridView();
            this.panelClassBottom = new System.Windows.Forms.Panel();
            this.lblClassCount = new System.Windows.Forms.Label();
            this.tabRooms = new System.Windows.Forms.TabPage();
            this.mainRooms = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxRoomFields = new System.Windows.Forms.GroupBox();
            this.tableRoomFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblRoomID = new System.Windows.Forms.Label();
            this.txtRoomID = new System.Windows.Forms.TextBox();
            this.lblRoomCode = new System.Windows.Forms.Label();
            this.txtRoomCode = new System.Windows.Forms.TextBox();
            this.lblRoomName = new System.Windows.Forms.Label();
            this.txtRoomName = new System.Windows.Forms.TextBox();
            this.lblRoomType = new System.Windows.Forms.Label();
            this.cmbRoomType = new System.Windows.Forms.ComboBox();
            this.lblCapacity = new System.Windows.Forms.Label();
            this.nudCapacity = new System.Windows.Forms.NumericUpDown();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.lblRoomActive = new System.Windows.Forms.Label();
            this.chkRoomActive = new System.Windows.Forms.CheckBox();
            this.lblRoomNotes = new System.Windows.Forms.Label();
            this.txtRoomNotes = new System.Windows.Forms.TextBox();
            this.panelRoomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRoomRefresh = new System.Windows.Forms.Button();
            this.btnRoomClear = new System.Windows.Forms.Button();
            this.btnRoomDelete = new System.Windows.Forms.Button();
            this.btnRoomUpdate = new System.Windows.Forms.Button();
            this.btnRoomAdd = new System.Windows.Forms.Button();
            this.panelRoomSearch = new System.Windows.Forms.Panel();
            this.txtRoomSearch = new System.Windows.Forms.TextBox();
            this.lblRoomSearch = new System.Windows.Forms.Label();
            this.dataGridViewRooms = new System.Windows.Forms.DataGridView();
            this.panelRoomBottom = new System.Windows.Forms.Panel();
            this.lblRoomCount = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabClasses.SuspendLayout();
            this.mainClasses.SuspendLayout();
            this.groupBoxClassFields.SuspendLayout();
            this.tableClassFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGradeOrder)).BeginInit();
            this.panelClassButtons.SuspendLayout();
            this.panelClassSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClasses)).BeginInit();
            this.panelClassBottom.SuspendLayout();
            this.tabRooms.SuspendLayout();
            this.mainRooms.SuspendLayout();
            this.groupBoxRoomFields.SuspendLayout();
            this.tableRoomFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCapacity)).BeginInit();
            this.panelRoomButtons.SuspendLayout();
            this.panelRoomSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRooms)).BeginInit();
            this.panelRoomBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1100, 58);
            this.panelTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1100, 58);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "إدارة الفصول الدراسية والقاعات";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabClasses);
            this.tabControl.Controls.Add(this.tabRooms);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 58);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControl.RightToLeftLayout = true;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1100, 662);
            this.tabControl.TabIndex = 1;
            // 
            // tabClasses
            // 
            this.tabClasses.Controls.Add(this.mainClasses);
            this.tabClasses.Location = new System.Drawing.Point(4, 30);
            this.tabClasses.Name = "tabClasses";
            this.tabClasses.Padding = new System.Windows.Forms.Padding(3);
            this.tabClasses.Size = new System.Drawing.Size(1092, 628);
            this.tabClasses.TabIndex = 0;
            this.tabClasses.Text = "الفصول الدراسية";
            this.tabClasses.UseVisualStyleBackColor = true;
            // 
            // mainClasses
            // 
            this.mainClasses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainClasses.ColumnCount = 1;
            this.mainClasses.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainClasses.Controls.Add(this.groupBoxClassFields, 0, 0);
            this.mainClasses.Controls.Add(this.panelClassButtons, 0, 1);
            this.mainClasses.Controls.Add(this.panelClassSearch, 0, 2);
            this.mainClasses.Controls.Add(this.dataGridViewClasses, 0, 3);
            this.mainClasses.Controls.Add(this.panelClassBottom, 0, 4);
            this.mainClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainClasses.Location = new System.Drawing.Point(3, 3);
            this.mainClasses.Name = "mainClasses";
            this.mainClasses.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainClasses.RowCount = 5;
            this.mainClasses.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.mainClasses.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.mainClasses.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.mainClasses.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainClasses.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainClasses.Size = new System.Drawing.Size(1086, 622);
            this.mainClasses.TabIndex = 0;
            // 
            // groupBoxClassFields
            // 
            this.groupBoxClassFields.BackColor = System.Drawing.Color.White;
            this.groupBoxClassFields.Controls.Add(this.tableClassFields);
            this.groupBoxClassFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxClassFields.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxClassFields.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxClassFields.Location = new System.Drawing.Point(15, 13);
            this.groupBoxClassFields.Name = "groupBoxClassFields";
            this.groupBoxClassFields.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxClassFields.Size = new System.Drawing.Size(1056, 164);
            this.groupBoxClassFields.TabIndex = 0;
            this.groupBoxClassFields.TabStop = false;
            this.groupBoxClassFields.Text = "بيانات الفصل";
            // 
            // tableClassFields
            // 
            this.tableClassFields.ColumnCount = 4;
            this.tableClassFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableClassFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableClassFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableClassFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableClassFields.Controls.Add(this.lblClassID, 0, 0);
            this.tableClassFields.Controls.Add(this.txtClassID, 1, 0);
            this.tableClassFields.Controls.Add(this.lblClassCode, 2, 0);
            this.tableClassFields.Controls.Add(this.txtClassCode, 3, 0);
            this.tableClassFields.Controls.Add(this.lblClassName, 0, 1);
            this.tableClassFields.Controls.Add(this.txtClassName, 1, 1);
            this.tableClassFields.Controls.Add(this.lblStageName, 2, 1);
            this.tableClassFields.Controls.Add(this.txtStageName, 3, 1);
            this.tableClassFields.Controls.Add(this.lblGradeOrder, 0, 2);
            this.tableClassFields.Controls.Add(this.nudGradeOrder, 1, 2);
            this.tableClassFields.Controls.Add(this.lblClassActive, 2, 2);
            this.tableClassFields.Controls.Add(this.chkClassActive, 3, 2);
            this.tableClassFields.Controls.Add(this.lblClassNotes, 0, 3);
            this.tableClassFields.Controls.Add(this.txtClassNotes, 1, 3);
            this.tableClassFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableClassFields.Location = new System.Drawing.Point(12, 29);
            this.tableClassFields.Name = "tableClassFields";
            this.tableClassFields.RowCount = 4;
            this.tableClassFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableClassFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableClassFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableClassFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableClassFields.Size = new System.Drawing.Size(1032, 125);
            this.tableClassFields.TabIndex = 0;
            // 
            // lblClassID
            // 
            this.lblClassID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassID.Location = new System.Drawing.Point(910, 0);
            this.lblClassID.Name = "lblClassID";
            this.lblClassID.Size = new System.Drawing.Size(119, 31);
            this.lblClassID.TabIndex = 0;
            this.lblClassID.Text = "رقم الفصل:";
            this.lblClassID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClassID
            // 
            this.txtClassID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtClassID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtClassID.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtClassID.Location = new System.Drawing.Point(519, 3);
            this.txtClassID.Name = "txtClassID";
            this.txtClassID.ReadOnly = true;
            this.txtClassID.Size = new System.Drawing.Size(385, 28);
            this.txtClassID.TabIndex = 1;
            // 
            // lblClassCode
            // 
            this.lblClassCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassCode.Location = new System.Drawing.Point(394, 0);
            this.lblClassCode.Name = "lblClassCode";
            this.lblClassCode.Size = new System.Drawing.Size(119, 31);
            this.lblClassCode.TabIndex = 2;
            this.lblClassCode.Text = "كود الفصل:";
            this.lblClassCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClassCode
            // 
            this.txtClassCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtClassCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtClassCode.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtClassCode.Location = new System.Drawing.Point(3, 3);
            this.txtClassCode.Name = "txtClassCode";
            this.txtClassCode.ReadOnly = true;
            this.txtClassCode.Size = new System.Drawing.Size(385, 28);
            this.txtClassCode.TabIndex = 3;
            // 
            // lblClassName
            // 
            this.lblClassName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassName.Location = new System.Drawing.Point(910, 31);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(119, 31);
            this.lblClassName.TabIndex = 4;
            this.lblClassName.Text = "اسم الفصل:";
            this.lblClassName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClassName
            // 
            this.txtClassName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtClassName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtClassName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtClassName.Location = new System.Drawing.Point(519, 34);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.ReadOnly = true;
            this.txtClassName.Size = new System.Drawing.Size(385, 28);
            this.txtClassName.TabIndex = 5;
            // 
            // lblStageName
            // 
            this.lblStageName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStageName.Location = new System.Drawing.Point(394, 31);
            this.lblStageName.Name = "lblStageName";
            this.lblStageName.Size = new System.Drawing.Size(119, 31);
            this.lblStageName.TabIndex = 6;
            this.lblStageName.Text = "المرحلة:";
            this.lblStageName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtStageName
            // 
            this.txtStageName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStageName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtStageName.Location = new System.Drawing.Point(3, 34);
            this.txtStageName.Name = "txtStageName";
            this.txtStageName.Size = new System.Drawing.Size(385, 28);
            this.txtStageName.TabIndex = 7;
            // 
            // lblGradeOrder
            // 
            this.lblGradeOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGradeOrder.Location = new System.Drawing.Point(910, 62);
            this.lblGradeOrder.Name = "lblGradeOrder";
            this.lblGradeOrder.Size = new System.Drawing.Size(119, 31);
            this.lblGradeOrder.TabIndex = 8;
            this.lblGradeOrder.Text = "الترتيب:";
            this.lblGradeOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudGradeOrder
            // 
            this.nudGradeOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudGradeOrder.Font = new System.Drawing.Font("Tahoma", 10F);
            this.nudGradeOrder.Location = new System.Drawing.Point(519, 65);
            this.nudGradeOrder.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudGradeOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGradeOrder.Name = "nudGradeOrder";
            this.nudGradeOrder.Size = new System.Drawing.Size(385, 28);
            this.nudGradeOrder.TabIndex = 9;
            this.nudGradeOrder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblClassActive
            // 
            this.lblClassActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassActive.Location = new System.Drawing.Point(394, 62);
            this.lblClassActive.Name = "lblClassActive";
            this.lblClassActive.Size = new System.Drawing.Size(119, 31);
            this.lblClassActive.TabIndex = 10;
            this.lblClassActive.Text = "الحالة:";
            this.lblClassActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkClassActive
            // 
            this.chkClassActive.Checked = true;
            this.chkClassActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClassActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkClassActive.Font = new System.Drawing.Font("Tahoma", 10F);
            this.chkClassActive.Location = new System.Drawing.Point(3, 65);
            this.chkClassActive.Name = "chkClassActive";
            this.chkClassActive.Size = new System.Drawing.Size(385, 25);
            this.chkClassActive.TabIndex = 11;
            this.chkClassActive.Text = "الفصل نشط";
            // 
            // lblClassNotes
            // 
            this.lblClassNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassNotes.Location = new System.Drawing.Point(910, 93);
            this.lblClassNotes.Name = "lblClassNotes";
            this.lblClassNotes.Size = new System.Drawing.Size(119, 32);
            this.lblClassNotes.TabIndex = 12;
            this.lblClassNotes.Text = "ملاحظات:";
            this.lblClassNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClassNotes
            // 
            this.tableClassFields.SetColumnSpan(this.txtClassNotes, 3);
            this.txtClassNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtClassNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtClassNotes.Location = new System.Drawing.Point(3, 96);
            this.txtClassNotes.Name = "txtClassNotes";
            this.txtClassNotes.Size = new System.Drawing.Size(901, 28);
            this.txtClassNotes.TabIndex = 13;
            // 
            // panelClassButtons
            // 
            this.panelClassButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelClassButtons.Controls.Add(this.btnClassRefresh);
            this.panelClassButtons.Controls.Add(this.btnClassClear);
            this.panelClassButtons.Controls.Add(this.btnClassUpdate);
            this.panelClassButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClassButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelClassButtons.Location = new System.Drawing.Point(15, 183);
            this.panelClassButtons.Name = "panelClassButtons";
            this.panelClassButtons.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.panelClassButtons.Size = new System.Drawing.Size(1056, 49);
            this.panelClassButtons.TabIndex = 1;
            // 
            // btnClassRefresh
            // 
            this.btnClassRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnClassRefresh.FlatAppearance.BorderSize = 0;
            this.btnClassRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClassRefresh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnClassRefresh.ForeColor = System.Drawing.Color.White;
            this.btnClassRefresh.Location = new System.Drawing.Point(3, 12);
            this.btnClassRefresh.Name = "btnClassRefresh";
            this.btnClassRefresh.Size = new System.Drawing.Size(110, 36);
            this.btnClassRefresh.TabIndex = 0;
            this.btnClassRefresh.Text = "تحديث";
            this.btnClassRefresh.UseVisualStyleBackColor = false;
            this.btnClassRefresh.Click += new System.EventHandler(this.btnClassRefresh_Click);
            // 
            // btnClassClear
            // 
            this.btnClassClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnClassClear.FlatAppearance.BorderSize = 0;
            this.btnClassClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClassClear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnClassClear.ForeColor = System.Drawing.Color.White;
            this.btnClassClear.Location = new System.Drawing.Point(119, 12);
            this.btnClassClear.Name = "btnClassClear";
            this.btnClassClear.Size = new System.Drawing.Size(110, 36);
            this.btnClassClear.TabIndex = 1;
            this.btnClassClear.Text = "تفريغ";
            this.btnClassClear.UseVisualStyleBackColor = false;
            this.btnClassClear.Click += new System.EventHandler(this.btnClassClear_Click);
            // 
            // btnClassUpdate
            // 
            this.btnClassUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnClassUpdate.FlatAppearance.BorderSize = 0;
            this.btnClassUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClassUpdate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnClassUpdate.ForeColor = System.Drawing.Color.White;
            this.btnClassUpdate.Location = new System.Drawing.Point(235, 12);
            this.btnClassUpdate.Name = "btnClassUpdate";
            this.btnClassUpdate.Size = new System.Drawing.Size(125, 36);
            this.btnClassUpdate.TabIndex = 2;
            this.btnClassUpdate.Text = "حفظ التعديل";
            this.btnClassUpdate.UseVisualStyleBackColor = false;
            this.btnClassUpdate.Click += new System.EventHandler(this.btnClassUpdate_Click);
            // 
            // panelClassSearch
            // 
            this.panelClassSearch.BackColor = System.Drawing.Color.White;
            this.panelClassSearch.Controls.Add(this.txtClassSearch);
            this.panelClassSearch.Controls.Add(this.lblClassSearch);
            this.panelClassSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClassSearch.Location = new System.Drawing.Point(15, 238);
            this.panelClassSearch.Name = "panelClassSearch";
            this.panelClassSearch.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.panelClassSearch.Size = new System.Drawing.Size(1056, 39);
            this.panelClassSearch.TabIndex = 2;
            // 
            // txtClassSearch
            // 
            this.txtClassSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtClassSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtClassSearch.Location = new System.Drawing.Point(10, 7);
            this.txtClassSearch.Name = "txtClassSearch";
            this.txtClassSearch.Size = new System.Drawing.Size(916, 28);
            this.txtClassSearch.TabIndex = 0;
            this.txtClassSearch.TextChanged += new System.EventHandler(this.txtClassSearch_TextChanged);
            // 
            // lblClassSearch
            // 
            this.lblClassSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblClassSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblClassSearch.Location = new System.Drawing.Point(926, 7);
            this.lblClassSearch.Name = "lblClassSearch";
            this.lblClassSearch.Size = new System.Drawing.Size(120, 25);
            this.lblClassSearch.TabIndex = 1;
            this.lblClassSearch.Text = "بحث سريع:";
            this.lblClassSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewClasses
            // 
            this.dataGridViewClasses.AllowUserToAddRows = false;
            this.dataGridViewClasses.AllowUserToDeleteRows = false;
            this.dataGridViewClasses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewClasses.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewClasses.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewClasses.ColumnHeadersHeight = 29;
            this.dataGridViewClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewClasses.Location = new System.Drawing.Point(15, 283);
            this.dataGridViewClasses.MultiSelect = false;
            this.dataGridViewClasses.Name = "dataGridViewClasses";
            this.dataGridViewClasses.ReadOnly = true;
            this.dataGridViewClasses.RowHeadersVisible = false;
            this.dataGridViewClasses.RowHeadersWidth = 51;
            this.dataGridViewClasses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewClasses.Size = new System.Drawing.Size(1056, 294);
            this.dataGridViewClasses.TabIndex = 3;
            this.dataGridViewClasses.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewClasses_CellClick);
            // 
            // panelClassBottom
            // 
            this.panelClassBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelClassBottom.Controls.Add(this.lblClassCount);
            this.panelClassBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClassBottom.Location = new System.Drawing.Point(15, 583);
            this.panelClassBottom.Name = "panelClassBottom";
            this.panelClassBottom.Size = new System.Drawing.Size(1056, 26);
            this.panelClassBottom.TabIndex = 4;
            // 
            // lblClassCount
            // 
            this.lblClassCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblClassCount.Location = new System.Drawing.Point(0, 0);
            this.lblClassCount.Name = "lblClassCount";
            this.lblClassCount.Size = new System.Drawing.Size(1056, 26);
            this.lblClassCount.TabIndex = 0;
            this.lblClassCount.Text = "عدد الفصول: 0";
            this.lblClassCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabRooms
            // 
            this.tabRooms.Controls.Add(this.mainRooms);
            this.tabRooms.Location = new System.Drawing.Point(4, 30);
            this.tabRooms.Name = "tabRooms";
            this.tabRooms.Padding = new System.Windows.Forms.Padding(3);
            this.tabRooms.Size = new System.Drawing.Size(1092, 628);
            this.tabRooms.TabIndex = 1;
            this.tabRooms.Text = "القاعات";
            this.tabRooms.UseVisualStyleBackColor = true;
            // 
            // mainRooms
            // 
            this.mainRooms.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainRooms.ColumnCount = 1;
            this.mainRooms.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainRooms.Controls.Add(this.groupBoxRoomFields, 0, 0);
            this.mainRooms.Controls.Add(this.panelRoomButtons, 0, 1);
            this.mainRooms.Controls.Add(this.panelRoomSearch, 0, 2);
            this.mainRooms.Controls.Add(this.dataGridViewRooms, 0, 3);
            this.mainRooms.Controls.Add(this.panelRoomBottom, 0, 4);
            this.mainRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainRooms.Location = new System.Drawing.Point(3, 3);
            this.mainRooms.Name = "mainRooms";
            this.mainRooms.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainRooms.RowCount = 5;
            this.mainRooms.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.mainRooms.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.mainRooms.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.mainRooms.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainRooms.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainRooms.Size = new System.Drawing.Size(1086, 622);
            this.mainRooms.TabIndex = 0;
            // 
            // groupBoxRoomFields
            // 
            this.groupBoxRoomFields.BackColor = System.Drawing.Color.White;
            this.groupBoxRoomFields.Controls.Add(this.tableRoomFields);
            this.groupBoxRoomFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRoomFields.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxRoomFields.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxRoomFields.Location = new System.Drawing.Point(15, 13);
            this.groupBoxRoomFields.Name = "groupBoxRoomFields";
            this.groupBoxRoomFields.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxRoomFields.Size = new System.Drawing.Size(1056, 169);
            this.groupBoxRoomFields.TabIndex = 0;
            this.groupBoxRoomFields.TabStop = false;
            this.groupBoxRoomFields.Text = "بيانات القاعة";
            // 
            // tableRoomFields
            // 
            this.tableRoomFields.ColumnCount = 4;
            this.tableRoomFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableRoomFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableRoomFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableRoomFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableRoomFields.Controls.Add(this.lblRoomID, 0, 0);
            this.tableRoomFields.Controls.Add(this.txtRoomID, 1, 0);
            this.tableRoomFields.Controls.Add(this.lblRoomCode, 2, 0);
            this.tableRoomFields.Controls.Add(this.txtRoomCode, 3, 0);
            this.tableRoomFields.Controls.Add(this.lblRoomName, 0, 1);
            this.tableRoomFields.Controls.Add(this.txtRoomName, 1, 1);
            this.tableRoomFields.Controls.Add(this.lblRoomType, 2, 1);
            this.tableRoomFields.Controls.Add(this.cmbRoomType, 3, 1);
            this.tableRoomFields.Controls.Add(this.lblCapacity, 0, 2);
            this.tableRoomFields.Controls.Add(this.nudCapacity, 1, 2);
            this.tableRoomFields.Controls.Add(this.lblLocation, 2, 2);
            this.tableRoomFields.Controls.Add(this.txtLocation, 3, 2);
            this.tableRoomFields.Controls.Add(this.lblRoomActive, 0, 3);
            this.tableRoomFields.Controls.Add(this.chkRoomActive, 1, 3);
            this.tableRoomFields.Controls.Add(this.lblRoomNotes, 2, 3);
            this.tableRoomFields.Controls.Add(this.txtRoomNotes, 3, 3);
            this.tableRoomFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRoomFields.Location = new System.Drawing.Point(12, 29);
            this.tableRoomFields.Name = "tableRoomFields";
            this.tableRoomFields.RowCount = 4;
            this.tableRoomFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableRoomFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableRoomFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableRoomFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableRoomFields.Size = new System.Drawing.Size(1032, 130);
            this.tableRoomFields.TabIndex = 0;
            // 
            // lblRoomID
            // 
            this.lblRoomID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoomID.Location = new System.Drawing.Point(910, 0);
            this.lblRoomID.Name = "lblRoomID";
            this.lblRoomID.Size = new System.Drawing.Size(119, 32);
            this.lblRoomID.TabIndex = 0;
            this.lblRoomID.Text = "رقم القاعة:";
            this.lblRoomID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRoomID
            // 
            this.txtRoomID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtRoomID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoomID.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtRoomID.Location = new System.Drawing.Point(519, 3);
            this.txtRoomID.Name = "txtRoomID";
            this.txtRoomID.ReadOnly = true;
            this.txtRoomID.Size = new System.Drawing.Size(385, 28);
            this.txtRoomID.TabIndex = 1;
            // 
            // lblRoomCode
            // 
            this.lblRoomCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoomCode.Location = new System.Drawing.Point(394, 0);
            this.lblRoomCode.Name = "lblRoomCode";
            this.lblRoomCode.Size = new System.Drawing.Size(119, 32);
            this.lblRoomCode.TabIndex = 2;
            this.lblRoomCode.Text = "كود القاعة:";
            this.lblRoomCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRoomCode
            // 
            this.txtRoomCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoomCode.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtRoomCode.Location = new System.Drawing.Point(3, 3);
            this.txtRoomCode.Name = "txtRoomCode";
            this.txtRoomCode.Size = new System.Drawing.Size(385, 28);
            this.txtRoomCode.TabIndex = 3;
            // 
            // lblRoomName
            // 
            this.lblRoomName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoomName.Location = new System.Drawing.Point(910, 32);
            this.lblRoomName.Name = "lblRoomName";
            this.lblRoomName.Size = new System.Drawing.Size(119, 32);
            this.lblRoomName.TabIndex = 4;
            this.lblRoomName.Text = "اسم القاعة:";
            this.lblRoomName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRoomName
            // 
            this.txtRoomName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoomName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtRoomName.Location = new System.Drawing.Point(519, 35);
            this.txtRoomName.Name = "txtRoomName";
            this.txtRoomName.Size = new System.Drawing.Size(385, 28);
            this.txtRoomName.TabIndex = 5;
            // 
            // lblRoomType
            // 
            this.lblRoomType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoomType.Location = new System.Drawing.Point(394, 32);
            this.lblRoomType.Name = "lblRoomType";
            this.lblRoomType.Size = new System.Drawing.Size(119, 32);
            this.lblRoomType.TabIndex = 6;
            this.lblRoomType.Text = "نوع القاعة:";
            this.lblRoomType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbRoomType
            // 
            this.cmbRoomType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbRoomType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoomType.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbRoomType.Items.AddRange(new object[] {
            "قاعة دراسية",
            "معمل",
            "قاعة نشاط",
            "مكتبة"});
            this.cmbRoomType.Location = new System.Drawing.Point(3, 35);
            this.cmbRoomType.Name = "cmbRoomType";
            this.cmbRoomType.Size = new System.Drawing.Size(385, 29);
            this.cmbRoomType.TabIndex = 7;
            // 
            // lblCapacity
            // 
            this.lblCapacity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapacity.Location = new System.Drawing.Point(910, 64);
            this.lblCapacity.Name = "lblCapacity";
            this.lblCapacity.Size = new System.Drawing.Size(119, 32);
            this.lblCapacity.TabIndex = 8;
            this.lblCapacity.Text = "السعة:";
            this.lblCapacity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCapacity
            // 
            this.nudCapacity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCapacity.Font = new System.Drawing.Font("Tahoma", 10F);
            this.nudCapacity.Location = new System.Drawing.Point(519, 67);
            this.nudCapacity.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudCapacity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCapacity.Name = "nudCapacity";
            this.nudCapacity.Size = new System.Drawing.Size(385, 28);
            this.nudCapacity.TabIndex = 9;
            this.nudCapacity.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lblLocation
            // 
            this.lblLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLocation.Location = new System.Drawing.Point(394, 64);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(119, 32);
            this.lblLocation.TabIndex = 10;
            this.lblLocation.Text = "الموقع:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLocation
            // 
            this.txtLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocation.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtLocation.Location = new System.Drawing.Point(3, 67);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(385, 28);
            this.txtLocation.TabIndex = 11;
            // 
            // lblRoomActive
            // 
            this.lblRoomActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoomActive.Location = new System.Drawing.Point(910, 96);
            this.lblRoomActive.Name = "lblRoomActive";
            this.lblRoomActive.Size = new System.Drawing.Size(119, 34);
            this.lblRoomActive.TabIndex = 12;
            this.lblRoomActive.Text = "الحالة:";
            this.lblRoomActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkRoomActive
            // 
            this.chkRoomActive.Checked = true;
            this.chkRoomActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRoomActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkRoomActive.Font = new System.Drawing.Font("Tahoma", 10F);
            this.chkRoomActive.Location = new System.Drawing.Point(519, 99);
            this.chkRoomActive.Name = "chkRoomActive";
            this.chkRoomActive.Size = new System.Drawing.Size(385, 28);
            this.chkRoomActive.TabIndex = 13;
            this.chkRoomActive.Text = "القاعة نشطة";
            // 
            // lblRoomNotes
            // 
            this.lblRoomNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoomNotes.Location = new System.Drawing.Point(394, 96);
            this.lblRoomNotes.Name = "lblRoomNotes";
            this.lblRoomNotes.Size = new System.Drawing.Size(119, 34);
            this.lblRoomNotes.TabIndex = 14;
            this.lblRoomNotes.Text = "ملاحظات:";
            this.lblRoomNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRoomNotes
            // 
            this.txtRoomNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoomNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtRoomNotes.Location = new System.Drawing.Point(3, 99);
            this.txtRoomNotes.Name = "txtRoomNotes";
            this.txtRoomNotes.Size = new System.Drawing.Size(385, 28);
            this.txtRoomNotes.TabIndex = 15;
            // 
            // panelRoomButtons
            // 
            this.panelRoomButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelRoomButtons.Controls.Add(this.btnRoomRefresh);
            this.panelRoomButtons.Controls.Add(this.btnRoomClear);
            this.panelRoomButtons.Controls.Add(this.btnRoomDelete);
            this.panelRoomButtons.Controls.Add(this.btnRoomUpdate);
            this.panelRoomButtons.Controls.Add(this.btnRoomAdd);
            this.panelRoomButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoomButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelRoomButtons.Location = new System.Drawing.Point(15, 188);
            this.panelRoomButtons.Name = "panelRoomButtons";
            this.panelRoomButtons.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.panelRoomButtons.Size = new System.Drawing.Size(1056, 49);
            this.panelRoomButtons.TabIndex = 1;
            // 
            // btnRoomRefresh
            // 
            this.btnRoomRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnRoomRefresh.FlatAppearance.BorderSize = 0;
            this.btnRoomRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoomRefresh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRoomRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRoomRefresh.Location = new System.Drawing.Point(3, 12);
            this.btnRoomRefresh.Name = "btnRoomRefresh";
            this.btnRoomRefresh.Size = new System.Drawing.Size(110, 36);
            this.btnRoomRefresh.TabIndex = 0;
            this.btnRoomRefresh.Text = "تحديث";
            this.btnRoomRefresh.UseVisualStyleBackColor = false;
            this.btnRoomRefresh.Click += new System.EventHandler(this.btnRoomRefresh_Click);
            // 
            // btnRoomClear
            // 
            this.btnRoomClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnRoomClear.FlatAppearance.BorderSize = 0;
            this.btnRoomClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoomClear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRoomClear.ForeColor = System.Drawing.Color.White;
            this.btnRoomClear.Location = new System.Drawing.Point(119, 12);
            this.btnRoomClear.Name = "btnRoomClear";
            this.btnRoomClear.Size = new System.Drawing.Size(110, 36);
            this.btnRoomClear.TabIndex = 1;
            this.btnRoomClear.Text = "تفريغ";
            this.btnRoomClear.UseVisualStyleBackColor = false;
            this.btnRoomClear.Click += new System.EventHandler(this.btnRoomClear_Click);
            // 
            // btnRoomDelete
            // 
            this.btnRoomDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnRoomDelete.FlatAppearance.BorderSize = 0;
            this.btnRoomDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoomDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRoomDelete.ForeColor = System.Drawing.Color.White;
            this.btnRoomDelete.Location = new System.Drawing.Point(235, 12);
            this.btnRoomDelete.Name = "btnRoomDelete";
            this.btnRoomDelete.Size = new System.Drawing.Size(110, 36);
            this.btnRoomDelete.TabIndex = 2;
            this.btnRoomDelete.Text = "حذف";
            this.btnRoomDelete.UseVisualStyleBackColor = false;
            this.btnRoomDelete.Click += new System.EventHandler(this.btnRoomDelete_Click);
            // 
            // btnRoomUpdate
            // 
            this.btnRoomUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnRoomUpdate.FlatAppearance.BorderSize = 0;
            this.btnRoomUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoomUpdate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRoomUpdate.ForeColor = System.Drawing.Color.White;
            this.btnRoomUpdate.Location = new System.Drawing.Point(351, 12);
            this.btnRoomUpdate.Name = "btnRoomUpdate";
            this.btnRoomUpdate.Size = new System.Drawing.Size(110, 36);
            this.btnRoomUpdate.TabIndex = 3;
            this.btnRoomUpdate.Text = "تعديل";
            this.btnRoomUpdate.UseVisualStyleBackColor = false;
            this.btnRoomUpdate.Click += new System.EventHandler(this.btnRoomUpdate_Click);
            // 
            // btnRoomAdd
            // 
            this.btnRoomAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnRoomAdd.FlatAppearance.BorderSize = 0;
            this.btnRoomAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoomAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRoomAdd.ForeColor = System.Drawing.Color.White;
            this.btnRoomAdd.Location = new System.Drawing.Point(467, 12);
            this.btnRoomAdd.Name = "btnRoomAdd";
            this.btnRoomAdd.Size = new System.Drawing.Size(110, 36);
            this.btnRoomAdd.TabIndex = 4;
            this.btnRoomAdd.Text = "إضافة";
            this.btnRoomAdd.UseVisualStyleBackColor = false;
            this.btnRoomAdd.Click += new System.EventHandler(this.btnRoomAdd_Click);
            // 
            // panelRoomSearch
            // 
            this.panelRoomSearch.BackColor = System.Drawing.Color.White;
            this.panelRoomSearch.Controls.Add(this.txtRoomSearch);
            this.panelRoomSearch.Controls.Add(this.lblRoomSearch);
            this.panelRoomSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoomSearch.Location = new System.Drawing.Point(15, 243);
            this.panelRoomSearch.Name = "panelRoomSearch";
            this.panelRoomSearch.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.panelRoomSearch.Size = new System.Drawing.Size(1056, 39);
            this.panelRoomSearch.TabIndex = 2;
            // 
            // txtRoomSearch
            // 
            this.txtRoomSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRoomSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtRoomSearch.Location = new System.Drawing.Point(10, 7);
            this.txtRoomSearch.Name = "txtRoomSearch";
            this.txtRoomSearch.Size = new System.Drawing.Size(916, 28);
            this.txtRoomSearch.TabIndex = 0;
            this.txtRoomSearch.TextChanged += new System.EventHandler(this.txtRoomSearch_TextChanged);
            // 
            // lblRoomSearch
            // 
            this.lblRoomSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblRoomSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblRoomSearch.Location = new System.Drawing.Point(926, 7);
            this.lblRoomSearch.Name = "lblRoomSearch";
            this.lblRoomSearch.Size = new System.Drawing.Size(120, 25);
            this.lblRoomSearch.TabIndex = 1;
            this.lblRoomSearch.Text = "بحث سريع:";
            this.lblRoomSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewRooms
            // 
            this.dataGridViewRooms.AllowUserToAddRows = false;
            this.dataGridViewRooms.AllowUserToDeleteRows = false;
            this.dataGridViewRooms.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRooms.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewRooms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewRooms.ColumnHeadersHeight = 29;
            this.dataGridViewRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRooms.Location = new System.Drawing.Point(15, 288);
            this.dataGridViewRooms.MultiSelect = false;
            this.dataGridViewRooms.Name = "dataGridViewRooms";
            this.dataGridViewRooms.ReadOnly = true;
            this.dataGridViewRooms.RowHeadersVisible = false;
            this.dataGridViewRooms.RowHeadersWidth = 51;
            this.dataGridViewRooms.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRooms.Size = new System.Drawing.Size(1056, 289);
            this.dataGridViewRooms.TabIndex = 3;
            this.dataGridViewRooms.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRooms_CellClick);
            // 
            // panelRoomBottom
            // 
            this.panelRoomBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelRoomBottom.Controls.Add(this.lblRoomCount);
            this.panelRoomBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoomBottom.Location = new System.Drawing.Point(15, 583);
            this.panelRoomBottom.Name = "panelRoomBottom";
            this.panelRoomBottom.Size = new System.Drawing.Size(1056, 26);
            this.panelRoomBottom.TabIndex = 4;
            // 
            // lblRoomCount
            // 
            this.lblRoomCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoomCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblRoomCount.Location = new System.Drawing.Point(0, 0);
            this.lblRoomCount.Name = "lblRoomCount";
            this.lblRoomCount.Size = new System.Drawing.Size(1056, 26);
            this.lblRoomCount.TabIndex = 0;
            this.lblRoomCount.Text = "عدد القاعات: 0";
            this.lblRoomCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ClassesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "ClassesForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1100, 720);
            this.panelTitle.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabClasses.ResumeLayout(false);
            this.mainClasses.ResumeLayout(false);
            this.groupBoxClassFields.ResumeLayout(false);
            this.tableClassFields.ResumeLayout(false);
            this.tableClassFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGradeOrder)).EndInit();
            this.panelClassButtons.ResumeLayout(false);
            this.panelClassSearch.ResumeLayout(false);
            this.panelClassSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClasses)).EndInit();
            this.panelClassBottom.ResumeLayout(false);
            this.tabRooms.ResumeLayout(false);
            this.mainRooms.ResumeLayout(false);
            this.groupBoxRoomFields.ResumeLayout(false);
            this.tableRoomFields.ResumeLayout(false);
            this.tableRoomFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCapacity)).EndInit();
            this.panelRoomButtons.ResumeLayout(false);
            this.panelRoomSearch.ResumeLayout(false);
            this.panelRoomSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRooms)).EndInit();
            this.panelRoomBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabClasses;
        private System.Windows.Forms.TabPage tabRooms;

        private System.Windows.Forms.TableLayoutPanel mainClasses;
        private System.Windows.Forms.GroupBox groupBoxClassFields;
        private System.Windows.Forms.TableLayoutPanel tableClassFields;
        private System.Windows.Forms.Label lblClassID;
        private System.Windows.Forms.TextBox txtClassID;
        private System.Windows.Forms.Label lblClassCode;
        private System.Windows.Forms.TextBox txtClassCode;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label lblStageName;
        private System.Windows.Forms.TextBox txtStageName;
        private System.Windows.Forms.Label lblGradeOrder;
        private System.Windows.Forms.NumericUpDown nudGradeOrder;
        private System.Windows.Forms.Label lblClassActive;
        private System.Windows.Forms.CheckBox chkClassActive;
        private System.Windows.Forms.Label lblClassNotes;
        private System.Windows.Forms.TextBox txtClassNotes;
        private System.Windows.Forms.FlowLayoutPanel panelClassButtons;
        private System.Windows.Forms.Button btnClassUpdate;
        private System.Windows.Forms.Button btnClassClear;
        private System.Windows.Forms.Button btnClassRefresh;
        private System.Windows.Forms.Panel panelClassSearch;
        private System.Windows.Forms.TextBox txtClassSearch;
        private System.Windows.Forms.Label lblClassSearch;
        private System.Windows.Forms.DataGridView dataGridViewClasses;
        private System.Windows.Forms.Panel panelClassBottom;
        private System.Windows.Forms.Label lblClassCount;

        private System.Windows.Forms.TableLayoutPanel mainRooms;
        private System.Windows.Forms.GroupBox groupBoxRoomFields;
        private System.Windows.Forms.TableLayoutPanel tableRoomFields;
        private System.Windows.Forms.Label lblRoomID;
        private System.Windows.Forms.TextBox txtRoomID;
        private System.Windows.Forms.Label lblRoomCode;
        private System.Windows.Forms.TextBox txtRoomCode;
        private System.Windows.Forms.Label lblRoomName;
        private System.Windows.Forms.TextBox txtRoomName;
        private System.Windows.Forms.Label lblRoomType;
        private System.Windows.Forms.ComboBox cmbRoomType;
        private System.Windows.Forms.Label lblCapacity;
        private System.Windows.Forms.NumericUpDown nudCapacity;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label lblRoomActive;
        private System.Windows.Forms.CheckBox chkRoomActive;
        private System.Windows.Forms.Label lblRoomNotes;
        private System.Windows.Forms.TextBox txtRoomNotes;
        private System.Windows.Forms.FlowLayoutPanel panelRoomButtons;
        private System.Windows.Forms.Button btnRoomAdd;
        private System.Windows.Forms.Button btnRoomUpdate;
        private System.Windows.Forms.Button btnRoomDelete;
        private System.Windows.Forms.Button btnRoomClear;
        private System.Windows.Forms.Button btnRoomRefresh;
        private System.Windows.Forms.Panel panelRoomSearch;
        private System.Windows.Forms.TextBox txtRoomSearch;
        private System.Windows.Forms.Label lblRoomSearch;
        private System.Windows.Forms.DataGridView dataGridViewRooms;
        private System.Windows.Forms.Panel panelRoomBottom;
        private System.Windows.Forms.Label lblRoomCount;
    }
}
