namespace SchoolSystem.UI
{
    partial class PayrollRecordsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel mainContainer;
        private System.Windows.Forms.GroupBox groupBoxPayroll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFields;
        private System.Windows.Forms.Label lblTeacher;
        private System.Windows.Forms.ComboBox cmbTeacher;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.NumericUpDown txtSalaryYear;
        private System.Windows.Forms.Label lblBasicSalary;
        private System.Windows.Forms.TextBox txtBasicSalary;
        private System.Windows.Forms.Label lblAllowances;
        private System.Windows.Forms.TextBox txtAllowances;
        private System.Windows.Forms.Label lblDeductions;
        private System.Windows.Forms.TextBox txtDeductions;
        private System.Windows.Forms.Label lblNetSalary;
        private System.Windows.Forms.TextBox txtNetSalary;
        private System.Windows.Forms.Label lblPaymentDate;
        private System.Windows.Forms.DateTimePicker dtpPaymentDate;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.FlowLayoutPanel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dataGridViewPayrolls;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxPayroll = new System.Windows.Forms.GroupBox();
            this.tableLayoutFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblTeacher = new System.Windows.Forms.Label();
            this.cmbTeacher = new System.Windows.Forms.ComboBox();
            this.lblMonth = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.txtSalaryYear = new System.Windows.Forms.NumericUpDown();
            this.lblBasicSalary = new System.Windows.Forms.Label();
            this.txtBasicSalary = new System.Windows.Forms.TextBox();
            this.lblAllowances = new System.Windows.Forms.Label();
            this.txtAllowances = new System.Windows.Forms.TextBox();
            this.lblDeductions = new System.Windows.Forms.Label();
            this.txtDeductions = new System.Windows.Forms.TextBox();
            this.lblNetSalary = new System.Windows.Forms.Label();
            this.txtNetSalary = new System.Windows.Forms.TextBox();
            this.lblPaymentDate = new System.Windows.Forms.Label();
            this.dtpPaymentDate = new System.Windows.Forms.DateTimePicker();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dataGridViewPayrolls = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalaryYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPayrolls)).BeginInit();
            this.SuspendLayout();

            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Height = 60;
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Controls.Add(this.lblTitle);

            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Text = "مسير الرواتب وسجلات الصرف";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Name = "lblTitle";

            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.RowCount = 5;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));

            this.groupBoxPayroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPayroll.Text = "بيانات الراتب الشهري";
            this.groupBoxPayroll.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxPayroll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxPayroll.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxPayroll.Controls.Add(this.tableLayoutFields);

            this.tableLayoutFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFields.ColumnCount = 6;
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            this.tableLayoutFields.RowCount = 4;
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));

            this.lblTeacher.Text = "المعلم";
            this.lblTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTeacher.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTeacher.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.cmbTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTeacher.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblTeacher, 0, 0);
            this.tableLayoutFields.Controls.Add(this.cmbTeacher, 1, 0);

            this.lblMonth.Text = "الشهر";
            this.lblMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMonth.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.cmbMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbMonth.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblMonth, 2, 0);
            this.tableLayoutFields.Controls.Add(this.cmbMonth, 3, 0);

            this.lblYear.Text = "السنة";
            this.lblYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblYear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.txtSalaryYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSalaryYear.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblYear, 4, 0);
            this.tableLayoutFields.Controls.Add(this.txtSalaryYear, 5, 0);

            this.lblBasicSalary.Text = "الأساسي";
            this.lblBasicSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBasicSalary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblBasicSalary.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.txtBasicSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBasicSalary.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblBasicSalary, 0, 1);
            this.tableLayoutFields.Controls.Add(this.txtBasicSalary, 1, 1);

            this.lblAllowances.Text = "البدلات";
            this.lblAllowances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAllowances.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAllowances.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.txtAllowances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAllowances.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblAllowances, 2, 1);
            this.tableLayoutFields.Controls.Add(this.txtAllowances, 3, 1);

            this.lblDeductions.Text = "الخصومات";
            this.lblDeductions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDeductions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDeductions.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.txtDeductions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDeductions.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblDeductions, 4, 1);
            this.tableLayoutFields.Controls.Add(this.txtDeductions, 5, 1);

            this.lblNetSalary.Text = "الصافي";
            this.lblNetSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetSalary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblNetSalary.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.txtNetSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNetSalary.Margin = new System.Windows.Forms.Padding(4);
            this.txtNetSalary.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.tableLayoutFields.Controls.Add(this.lblNetSalary, 0, 2);
            this.tableLayoutFields.Controls.Add(this.txtNetSalary, 1, 2);

            this.lblPaymentDate.Text = "تاريخ الصرف";
            this.lblPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPaymentDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.dtpPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpPaymentDate.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblPaymentDate, 2, 2);
            this.tableLayoutFields.Controls.Add(this.dtpPaymentDate, 3, 2);

            this.lblPaymentMethod.Text = "طريقة الصرف";
            this.lblPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.cmbPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPaymentMethod.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblPaymentMethod, 4, 2);
            this.tableLayoutFields.Controls.Add(this.cmbPaymentMethod, 5, 2);

            this.lblNotes.Text = "ملاحظات";
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblNotes.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutFields.Controls.Add(this.lblNotes, 0, 3);
            this.tableLayoutFields.Controls.Add(this.txtNotes, 1, 3);

                        this.tableLayoutFields.SetColumnSpan(this.txtNotes, 5);

            this.txtSalaryYear.Minimum = 2000;
            this.txtSalaryYear.Maximum = 2100;
            this.txtSalaryYear.Width = 120;
            this.txtSalaryYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            this.dtpPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPaymentDate.Width = 150;

            this.txtNotes.Multiline = true;
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            this.cmbTeacher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnClear);

            this.btnAdd.Text = "تسجيل وصرف الراتب";
            this.btnAdd.Width = 230;
            this.btnAdd.Height = 40;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.AutoSize = false;
            this.btnAdd.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);

            this.btnRefresh.Text = "تحديث السجلات";
            this.btnRefresh.Width = 140;
            this.btnRefresh.Height = 40;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.AutoSize = false;
            this.btnRefresh.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);

            this.btnClear.Text = "تفريغ الحقول";
            this.btnClear.Width = 125;
            this.btnClear.Height = 40;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.AutoSize = false;
            this.btnClear.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);

            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);

            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Text = "بحث في سجلات الرواتب:";
            this.lblSearch.Width = 190;
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);

            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);

            this.dataGridViewPayrolls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPayrolls.AutoGenerateColumns = true;
            this.dataGridViewPayrolls.ReadOnly = true;
            this.dataGridViewPayrolls.AllowUserToAddRows = false;
            this.dataGridViewPayrolls.AllowUserToDeleteRows = false;
            this.dataGridViewPayrolls.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPayrolls.MultiSelect = false;
            this.dataGridViewPayrolls.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewPayrolls.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewPayrolls.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewPayrolls.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewPayrolls.RowHeadersVisible = false;
            this.dataGridViewPayrolls.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewPayrolls.EnableHeadersVisualStyles = false;
            this.dataGridViewPayrolls.ColumnHeadersHeight = 42;
            this.dataGridViewPayrolls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewPayrolls.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewPayrolls.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dataGridViewPayrolls.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dataGridViewPayrolls.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewPayrolls.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dataGridViewPayrolls.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dataGridViewPayrolls.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dataGridViewPayrolls.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewPayrolls.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dataGridViewPayrolls.RowTemplate.Height = 36;
            this.dataGridViewPayrolls.GridColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dataGridViewPayrolls.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.mainContainer.Controls.Add(this.groupBoxPayroll, 0, 0);
            this.mainContainer.Controls.Add(this.panelButtons, 0, 1);
            this.mainContainer.Controls.Add(this.panelSearch, 0, 2);
            this.mainContainer.Controls.Add(this.dataGridViewPayrolls, 0, 3);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 4);

            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Name = "PayrollRecordsForm";
            this.Size = new System.Drawing.Size(1100, 700);

            ((System.ComponentModel.ISupportInitialize)(this.txtSalaryYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPayrolls)).EndInit();
            this.ResumeLayout(false);
        }

    }
}
