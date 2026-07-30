namespace SchoolSystem.UI
{
    partial class FeePlansForm
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
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelFields = new System.Windows.Forms.Panel();
            this.tableFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblAcademicYear = new System.Windows.Forms.Label();
            this.cmbAcademicYear = new System.Windows.Forms.ComboBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblFeeType = new System.Windows.Forms.Label();
            this.cmbFeeType = new System.Windows.Forms.ComboBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblDueDate = new System.Windows.Forms.Label();
            this.dtpDueDate = new System.Windows.Forms.DateTimePicker();
            this.lblIsRequired = new System.Windows.Forms.Label();
            this.chkIsRequired = new System.Windows.Forms.CheckBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dataGridViewFeePlans = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.panelFields.SuspendLayout();
            this.tableFields.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFeePlans)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1100, 55);
            this.panelTitle.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1100, 55);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "تعريف رسوم الصفوف";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelFields
            // 
            this.panelFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelFields.Controls.Add(this.tableFields);
            this.panelFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFields.Location = new System.Drawing.Point(0, 55);
            this.panelFields.Name = "panelFields";
            this.panelFields.Padding = new System.Windows.Forms.Padding(15);
            this.panelFields.Size = new System.Drawing.Size(1100, 185);
            this.panelFields.TabIndex = 4;
            // 
            // tableFields
            // 
            this.tableFields.ColumnCount = 4;
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableFields.Controls.Add(this.lblAcademicYear, 0, 0);
            this.tableFields.Controls.Add(this.cmbAcademicYear, 1, 0);
            this.tableFields.Controls.Add(this.lblClass, 2, 0);
            this.tableFields.Controls.Add(this.cmbClass, 3, 0);
            this.tableFields.Controls.Add(this.lblFeeType, 0, 1);
            this.tableFields.Controls.Add(this.cmbFeeType, 1, 1);
            this.tableFields.Controls.Add(this.lblAmount, 2, 1);
            this.tableFields.Controls.Add(this.txtAmount, 3, 1);
            this.tableFields.Controls.Add(this.lblDueDate, 0, 2);
            this.tableFields.Controls.Add(this.dtpDueDate, 1, 2);
            this.tableFields.Controls.Add(this.lblIsRequired, 2, 2);
            this.tableFields.Controls.Add(this.chkIsRequired, 3, 2);
            this.tableFields.Controls.Add(this.lblNotes, 0, 3);
            this.tableFields.Controls.Add(this.txtNotes, 1, 3);
            this.tableFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableFields.Location = new System.Drawing.Point(15, 15);
            this.tableFields.Name = "tableFields";
            this.tableFields.RowCount = 4;
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableFields.Size = new System.Drawing.Size(1070, 155);
            this.tableFields.TabIndex = 0;
            // 
            // lblAcademicYear
            // 
            this.lblAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicYear.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblAcademicYear.Location = new System.Drawing.Point(948, 0);
            this.lblAcademicYear.Name = "lblAcademicYear";
            this.lblAcademicYear.Size = new System.Drawing.Size(119, 38);
            this.lblAcademicYear.TabIndex = 0;
            this.lblAcademicYear.Text = "العام الدراسي:";
            this.lblAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbAcademicYear
            // 
            this.cmbAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAcademicYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAcademicYear.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.cmbAcademicYear.Location = new System.Drawing.Point(538, 3);
            this.cmbAcademicYear.Name = "cmbAcademicYear";
            this.cmbAcademicYear.Size = new System.Drawing.Size(404, 27);
            this.cmbAcademicYear.TabIndex = 1;
            // 
            // lblClass
            // 
            this.lblClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClass.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblClass.Location = new System.Drawing.Point(413, 0);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(119, 38);
            this.lblClass.TabIndex = 2;
            this.lblClass.Text = "الصف:";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbClass
            // 
            this.cmbClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.cmbClass.Location = new System.Drawing.Point(3, 3);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(404, 27);
            this.cmbClass.TabIndex = 3;
            // 
            // lblFeeType
            // 
            this.lblFeeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFeeType.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblFeeType.Location = new System.Drawing.Point(948, 38);
            this.lblFeeType.Name = "lblFeeType";
            this.lblFeeType.Size = new System.Drawing.Size(119, 38);
            this.lblFeeType.TabIndex = 4;
            this.lblFeeType.Text = "نوع الرسوم:";
            this.lblFeeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFeeType
            // 
            this.cmbFeeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFeeType.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.cmbFeeType.Location = new System.Drawing.Point(538, 41);
            this.cmbFeeType.Name = "cmbFeeType";
            this.cmbFeeType.Size = new System.Drawing.Size(404, 27);
            this.cmbFeeType.TabIndex = 5;
            // 
            // lblAmount
            // 
            this.lblAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblAmount.Location = new System.Drawing.Point(413, 38);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(119, 38);
            this.lblAmount.TabIndex = 6;
            this.lblAmount.Text = "المبلغ:";
            this.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAmount
            // 
            this.txtAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAmount.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtAmount.Location = new System.Drawing.Point(3, 41);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(404, 27);
            this.txtAmount.TabIndex = 7;
            this.txtAmount.Text = "0";
            // 
            // lblDueDate
            // 
            this.lblDueDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDueDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblDueDate.Location = new System.Drawing.Point(948, 76);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.Size = new System.Drawing.Size(119, 38);
            this.lblDueDate.TabIndex = 8;
            this.lblDueDate.Text = "تاريخ الاستحقاق:";
            this.lblDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDueDate
            // 
            this.dtpDueDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDueDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDueDate.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.dtpDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDueDate.Location = new System.Drawing.Point(538, 79);
            this.dtpDueDate.Name = "dtpDueDate";
            this.dtpDueDate.Size = new System.Drawing.Size(404, 27);
            this.dtpDueDate.TabIndex = 9;
            // 
            // lblIsRequired
            // 
            this.lblIsRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsRequired.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblIsRequired.Location = new System.Drawing.Point(413, 76);
            this.lblIsRequired.Name = "lblIsRequired";
            this.lblIsRequired.Size = new System.Drawing.Size(119, 38);
            this.lblIsRequired.TabIndex = 10;
            this.lblIsRequired.Text = "إلزامية؟";
            this.lblIsRequired.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkIsRequired
            // 
            this.chkIsRequired.Checked = true;
            this.chkIsRequired.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkIsRequired.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.chkIsRequired.Location = new System.Drawing.Point(3, 79);
            this.chkIsRequired.Name = "chkIsRequired";
            this.chkIsRequired.Size = new System.Drawing.Size(404, 32);
            this.chkIsRequired.TabIndex = 11;
            this.chkIsRequired.Text = "نعم";
            this.chkIsRequired.UseVisualStyleBackColor = true;
            // 
            // lblNotes
            // 
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblNotes.Location = new System.Drawing.Point(948, 114);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(119, 41);
            this.lblNotes.TabIndex = 12;
            this.lblNotes.Text = "ملاحظات:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNotes
            // 
            this.tableFields.SetColumnSpan(this.txtNotes, 3);
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.txtNotes.Location = new System.Drawing.Point(3, 117);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(939, 27);
            this.txtNotes.TabIndex = 13;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnUpdate);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnClear);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 240);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1100, 55);
            this.panelButtons.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(970, 10);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(115, 35);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "إضافة";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(845, 10);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(115, 35);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(720, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(115, 35);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(595, 10);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(115, 35);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "تفريغ";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 295);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1100, 45);
            this.panelSearch.TabIndex = 2;
            // 
            // lblSearch
            // 
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblSearch.Location = new System.Drawing.Point(970, 10);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(110, 25);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(720, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(240, 28);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // dataGridViewFeePlans
            // 
            this.dataGridViewFeePlans.AllowUserToAddRows = false;
            this.dataGridViewFeePlans.AllowUserToDeleteRows = false;
            this.dataGridViewFeePlans.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewFeePlans.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewFeePlans.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewFeePlans.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewFeePlans.ColumnHeadersHeight = 38;
            this.dataGridViewFeePlans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFeePlans.EnableHeadersVisualStyles = false;
            this.dataGridViewFeePlans.Location = new System.Drawing.Point(0, 340);
            this.dataGridViewFeePlans.MultiSelect = false;
            this.dataGridViewFeePlans.Name = "dataGridViewFeePlans";
            this.dataGridViewFeePlans.ReadOnly = true;
            this.dataGridViewFeePlans.RowHeadersVisible = false;
            this.dataGridViewFeePlans.RowHeadersWidth = 51;
            this.dataGridViewFeePlans.RowTemplate.Height = 34;
            this.dataGridViewFeePlans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFeePlans.Size = new System.Drawing.Size(1100, 330);
            this.dataGridViewFeePlans.TabIndex = 0;
            this.dataGridViewFeePlans.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFeePlans_CellClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 670);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1100, 30);
            this.panelBottom.TabIndex = 1;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(1100, 30);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد السجلات: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FeePlansForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewFeePlans);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelFields);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "FeePlansForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1100, 700);
            this.panelTitle.ResumeLayout(false);
            this.panelFields.ResumeLayout(false);
            this.tableFields.ResumeLayout(false);
            this.tableFields.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFeePlans)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel panelFields;
        private System.Windows.Forms.TableLayoutPanel tableFields;

        private System.Windows.Forms.Label lblAcademicYear;
        private System.Windows.Forms.ComboBox cmbAcademicYear;

        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox cmbClass;

        private System.Windows.Forms.Label lblFeeType;
        private System.Windows.Forms.ComboBox cmbFeeType;

        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.TextBox txtAmount;

        private System.Windows.Forms.Label lblDueDate;
        private System.Windows.Forms.DateTimePicker dtpDueDate;

        private System.Windows.Forms.Label lblIsRequired;
        private System.Windows.Forms.CheckBox chkIsRequired;

        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;

        private System.Windows.Forms.DataGridView dataGridViewFeePlans;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
