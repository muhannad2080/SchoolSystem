namespace SchoolSystem.UI
{
    partial class UsersForm
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
            this.groupBoxAccount = new System.Windows.Forms.GroupBox();
            this.tableLayoutFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblIsActive = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.lblMustChangePassword = new System.Windows.Forms.Label();
            this.chkMustChangePassword = new System.Windows.Forms.CheckBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.groupBoxPermissions = new System.Windows.Forms.GroupBox();
            this.checkedListPermissions = new System.Windows.Forms.CheckedListBox();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dataGridViewUsers = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelTitle)).BeginInit();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.groupBoxAccount.SuspendLayout();
            this.tableLayoutFields.SuspendLayout();
            this.groupBoxPermissions.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
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
            this.lblTitle.Text = "إدارة المستخدمين والصلاحيات";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainContainer
            // 
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainContainer.ColumnCount = 2;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68F));
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.mainContainer.Controls.Add(this.groupBoxAccount, 0, 0);
            this.mainContainer.Controls.Add(this.groupBoxPermissions, 1, 0);
            this.mainContainer.Controls.Add(this.panelButtons, 0, 1);
            this.mainContainer.Controls.Add(this.panelSearch, 0, 2);
            this.mainContainer.Controls.Add(this.dataGridViewUsers, 0, 3);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 4);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 58);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.RowCount = 5;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainContainer.Size = new System.Drawing.Size(1100, 642);
            this.mainContainer.TabIndex = 1;
            // 
            // groupBoxAccount
            // 
            this.groupBoxAccount.BackColor = System.Drawing.Color.White;
            this.groupBoxAccount.Controls.Add(this.tableLayoutFields);
            this.groupBoxAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAccount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxAccount.Location = new System.Drawing.Point(360, 13);
            this.groupBoxAccount.Name = "groupBoxAccount";
            this.groupBoxAccount.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxAccount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxAccount.Size = new System.Drawing.Size(725, 204);
            this.groupBoxAccount.TabIndex = 0;
            this.groupBoxAccount.TabStop = false;
            this.groupBoxAccount.Text = "بيانات الحساب";
            // 
            // tableLayoutFields
            // 
            this.tableLayoutFields.ColumnCount = 4;
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutFields.Controls.Add(this.lblFullName, 0, 0);
            this.tableLayoutFields.Controls.Add(this.txtFullName, 1, 0);
            this.tableLayoutFields.Controls.Add(this.lblUserName, 2, 0);
            this.tableLayoutFields.Controls.Add(this.txtUserName, 3, 0);
            this.tableLayoutFields.Controls.Add(this.lblPassword, 0, 1);
            this.tableLayoutFields.Controls.Add(this.txtPassword, 1, 1);
            this.tableLayoutFields.Controls.Add(this.lblConfirmPassword, 2, 1);
            this.tableLayoutFields.Controls.Add(this.txtConfirmPassword, 3, 1);
            this.tableLayoutFields.Controls.Add(this.lblRole, 0, 2);
            this.tableLayoutFields.Controls.Add(this.cmbRole, 1, 2);
            this.tableLayoutFields.Controls.Add(this.lblEmail, 2, 2);
            this.tableLayoutFields.Controls.Add(this.txtEmail, 3, 2);
            this.tableLayoutFields.Controls.Add(this.lblPhone, 0, 3);
            this.tableLayoutFields.Controls.Add(this.txtPhone, 1, 3);
            this.tableLayoutFields.Controls.Add(this.lblIsActive, 2, 3);
            this.tableLayoutFields.Controls.Add(this.chkIsActive, 3, 3);
            this.tableLayoutFields.Controls.Add(this.lblMustChangePassword, 0, 4);
            this.tableLayoutFields.Controls.Add(this.chkMustChangePassword, 1, 4);
            this.tableLayoutFields.Controls.Add(this.chkShowPassword, 3, 4);
            this.tableLayoutFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFields.Location = new System.Drawing.Point(12, 29);
            this.tableLayoutFields.Name = "tableLayoutFields";
            this.tableLayoutFields.RowCount = 5;
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutFields.Size = new System.Drawing.Size(701, 165);
            this.tableLayoutFields.TabIndex = 0;
            // 
            // lblFullName
            // 
            this.lblFullName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFullName.Location = new System.Drawing.Point(579, 0);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(119, 33);
            this.lblFullName.TabIndex = 0;
            this.lblFullName.Text = "الاسم الكامل:";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFullName
            // 
            this.txtFullName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFullName.Location = new System.Drawing.Point(354, 3);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(219, 28);
            this.txtFullName.TabIndex = 1;
            // 
            // lblUserName
            // 
            this.lblUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserName.Location = new System.Drawing.Point(229, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(119, 33);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "اسم المستخدم:";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUserName
            // 
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserName.Location = new System.Drawing.Point(3, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(220, 28);
            this.txtUserName.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPassword.Location = new System.Drawing.Point(579, 33);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(119, 33);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "كلمة المرور:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Location = new System.Drawing.Point(354, 36);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(219, 28);
            this.txtPassword.TabIndex = 5;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConfirmPassword.Location = new System.Drawing.Point(229, 33);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(119, 33);
            this.lblConfirmPassword.TabIndex = 6;
            this.lblConfirmPassword.Text = "تأكيد المرور:";
            this.lblConfirmPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConfirmPassword.Location = new System.Drawing.Point(3, 36);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '●';
            this.txtConfirmPassword.Size = new System.Drawing.Size(220, 28);
            this.txtConfirmPassword.TabIndex = 7;
            // 
            // lblRole
            // 
            this.lblRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRole.Location = new System.Drawing.Point(579, 66);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(119, 33);
            this.lblRole.TabIndex = 8;
            this.lblRole.Text = "الدور:";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbRole
            // 
            this.cmbRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.Location = new System.Drawing.Point(354, 69);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(219, 29);
            this.cmbRole.TabIndex = 9;
            // 
            // lblEmail
            // 
            this.lblEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEmail.Location = new System.Drawing.Point(229, 66);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(119, 33);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "البريد:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEmail
            // 
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Location = new System.Drawing.Point(3, 69);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(220, 28);
            this.txtEmail.TabIndex = 11;
            // 
            // lblPhone
            // 
            this.lblPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPhone.Location = new System.Drawing.Point(579, 99);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(119, 33);
            this.lblPhone.TabIndex = 12;
            this.lblPhone.Text = "الهاتف:";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPhone
            // 
            this.txtPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPhone.Location = new System.Drawing.Point(354, 102);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(219, 28);
            this.txtPhone.TabIndex = 13;
            // 
            // lblIsActive
            // 
            this.lblIsActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsActive.Location = new System.Drawing.Point(229, 99);
            this.lblIsActive.Name = "lblIsActive";
            this.lblIsActive.Size = new System.Drawing.Size(119, 33);
            this.lblIsActive.TabIndex = 14;
            this.lblIsActive.Text = "نشط:";
            this.lblIsActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkIsActive
            // 
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkIsActive.Location = new System.Drawing.Point(3, 102);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(220, 27);
            this.chkIsActive.TabIndex = 15;
            this.chkIsActive.Text = "الحساب فعال";
            // 
            // lblMustChangePassword
            // 
            this.lblMustChangePassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMustChangePassword.Location = new System.Drawing.Point(579, 132);
            this.lblMustChangePassword.Name = "lblMustChangePassword";
            this.lblMustChangePassword.Size = new System.Drawing.Size(119, 33);
            this.lblMustChangePassword.TabIndex = 16;
            this.lblMustChangePassword.Text = "تغيير كلمة المرور:";
            this.lblMustChangePassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkMustChangePassword
            // 
            this.chkMustChangePassword.Checked = true;
            this.chkMustChangePassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMustChangePassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkMustChangePassword.Location = new System.Drawing.Point(354, 135);
            this.chkMustChangePassword.Name = "chkMustChangePassword";
            this.chkMustChangePassword.Size = new System.Drawing.Size(219, 27);
            this.chkMustChangePassword.TabIndex = 17;
            this.chkMustChangePassword.Text = "إلزامي عند الدخول";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShowPassword.Location = new System.Drawing.Point(3, 135);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(220, 27);
            this.chkShowPassword.TabIndex = 18;
            this.chkShowPassword.Text = "إظهار كلمة المرور";
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // groupBoxPermissions
            // 
            this.groupBoxPermissions.BackColor = System.Drawing.Color.White;
            this.groupBoxPermissions.Controls.Add(this.checkedListPermissions);
            this.groupBoxPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPermissions.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxPermissions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxPermissions.Location = new System.Drawing.Point(15, 13);
            this.groupBoxPermissions.Name = "groupBoxPermissions";
            this.groupBoxPermissions.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.groupBoxPermissions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxPermissions.Size = new System.Drawing.Size(339, 204);
            this.groupBoxPermissions.TabIndex = 1;
            this.groupBoxPermissions.TabStop = false;
            this.groupBoxPermissions.Text = "الصلاحيات";
            // 
            // checkedListPermissions
            // 
            this.checkedListPermissions.CheckOnClick = true;
            this.checkedListPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListPermissions.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.checkedListPermissions.FormattingEnabled = true;
            this.checkedListPermissions.Location = new System.Drawing.Point(12, 29);
            this.checkedListPermissions.Name = "checkedListPermissions";
            this.checkedListPermissions.Size = new System.Drawing.Size(315, 165);
            this.checkedListPermissions.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.mainContainer.SetColumnSpan(this.panelButtons, 2);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnClear);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnUpdate);
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelButtons.Location = new System.Drawing.Point(15, 223);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelButtons.Size = new System.Drawing.Size(1070, 48);
            this.panelButtons.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(3, 11);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(115, 36);
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
            this.btnClear.Location = new System.Drawing.Point(124, 11);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(115, 36);
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
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(245, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(115, 36);
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
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(366, 11);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(115, 36);
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
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(487, 11);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(115, 36);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "إضافة";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.mainContainer.SetColumnSpan(this.panelSearch, 2);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearch.Location = new System.Drawing.Point(15, 277);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.panelSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelSearch.Size = new System.Drawing.Size(1070, 42);
            this.panelSearch.TabIndex = 3;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(10, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(930, 28);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblSearch.Location = new System.Drawing.Point(940, 7);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(120, 28);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "بحث سريع:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewUsers
            // 
            this.dataGridViewUsers.AllowUserToAddRows = false;
            this.dataGridViewUsers.AllowUserToDeleteRows = false;
            this.dataGridViewUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewUsers.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewUsers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewUsers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewUsers.ColumnHeadersHeight = 42;
            this.mainContainer.SetColumnSpan(this.dataGridViewUsers, 2);
            this.dataGridViewUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewUsers.EnableHeadersVisualStyles = false;
            this.dataGridViewUsers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewUsers.Location = new System.Drawing.Point(15, 325);
            this.dataGridViewUsers.MultiSelect = false;
            this.dataGridViewUsers.Name = "dataGridViewUsers";
            this.dataGridViewUsers.ReadOnly = true;
            this.dataGridViewUsers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewUsers.RowHeadersVisible = false;
            this.dataGridViewUsers.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.dataGridViewUsers.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewUsers.RowTemplate.Height = 34;
            this.dataGridViewUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUsers.Size = new System.Drawing.Size(1070, 272);
            this.dataGridViewUsers.TabIndex = 4;
            this.dataGridViewUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewUsers_CellClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainContainer.SetColumnSpan(this.panelBottom, 2);
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(15, 603);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1070, 26);
            this.panelBottom.TabIndex = 5;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(1070, 26);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد المستخدمين: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UsersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "UsersForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1100, 700);
            ((System.ComponentModel.ISupportInitialize)(this.panelTitle)).EndInit();
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.groupBoxAccount.ResumeLayout(false);
            this.tableLayoutFields.ResumeLayout(false);
            this.tableLayoutFields.PerformLayout();
            this.groupBoxPermissions.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel mainContainer;

        private System.Windows.Forms.GroupBox groupBoxAccount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFields;

        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;

        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;

        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;

        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;

        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbRole;

        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;

        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;

        private System.Windows.Forms.Label lblIsActive;
        private System.Windows.Forms.CheckBox chkIsActive;

        private System.Windows.Forms.Label lblMustChangePassword;
        private System.Windows.Forms.CheckBox chkMustChangePassword;

        private System.Windows.Forms.CheckBox chkShowPassword;

        private System.Windows.Forms.GroupBox groupBoxPermissions;
        private System.Windows.Forms.CheckedListBox checkedListPermissions;

        private System.Windows.Forms.FlowLayoutPanel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;

        private System.Windows.Forms.DataGridView dataGridViewUsers;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
