using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.Security;

namespace SchoolSystem.UI
{
    public partial class UsersForm : UserControl
    {
        private readonly UserService userService = new UserService();

        private int selectedUserId = 0;
        private DataTable allUsers;
        private bool isLoading = false;
        private bool permissionsUserModified = false;
        private bool suppressPermissionTracking = false;
        private FlowLayoutPanel permissionActions;
        private Button btnSelectAllPermissions;
        private Button btnClearPermissions;
        private Button btnApplyRolePreset;

        private sealed class PermissionListItem
        {
            public string Key { get; private set; }
            public string DisplayText { get; private set; }

            public PermissionListItem(string key, string displayText)
            {
                Key = key ?? string.Empty;
                DisplayText = displayText ?? string.Empty;
            }

            public override string ToString()
            {
                return Key + " - " + DisplayText;
            }
        }

        public UsersForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            InitializePermissionActions();
            ApplyCustomStyles();
            ApplyPermissionUiState();
            checkedListPermissions.ItemCheck += checkedListPermissions_ItemCheck;
            Dock = DockStyle.Fill;
            Load += UsersForm_Load;
        }

        private void InitializePermissionActions()
        {
            // تُنشأ الأدوات برمجياً بعد InitializeComponent حتى يبقى المصمم مستقراً
            // وتبقى القائمة قابلة للعرض في نسخ Visual Studio المختلفة.
            permissionActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 36,
                AutoSize = false,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                RightToLeft = RightToLeft.Yes,
                Padding = new Padding(0, 3, 0, 0),
                Margin = new Padding(0)
            };

            btnSelectAllPermissions = new Button
            {
                Name = "btnSelectAllPermissions",
                Text = "منح كل الصلاحيات",
                Width = 140,
                Height = 30,
                TabIndex = 0,
                UseVisualStyleBackColor = false
            };
            btnClearPermissions = new Button
            {
                Name = "btnClearPermissions",
                Text = "إلغاء كل الصلاحيات",
                Width = 140,
                Height = 30,
                TabIndex = 1,
                UseVisualStyleBackColor = false
            };

            btnApplyRolePreset = new Button
            {
                Name = "btnApplyRolePreset",
                Text = "تطبيق صلاحيات الدور",
                Width = 150,
                Height = 30,
                TabIndex = 2,
                UseVisualStyleBackColor = false
            };

            btnSelectAllPermissions.AccessibleName = "منح المستخدم كل الصلاحيات";
            btnSelectAllPermissions.Enabled = true;
            btnSelectAllPermissions.Visible = true;
            btnSelectAllPermissions.Click += btnSelectAllPermissions_Click;
            btnClearPermissions.AccessibleName = "إلغاء جميع صلاحيات المستخدم";
            btnClearPermissions.Enabled = true;
            btnClearPermissions.Visible = true;
            btnClearPermissions.Click += btnClearPermissions_Click;
            btnApplyRolePreset.AccessibleName = "تطبيق الصلاحيات الافتراضية لدور المستخدم";
            btnApplyRolePreset.Enabled = true;
            btnApplyRolePreset.Visible = true;
            btnApplyRolePreset.Click += btnApplyRolePreset_Click;
            permissionActions.Controls.Add(btnSelectAllPermissions);
            permissionActions.Controls.Add(btnClearPermissions);
            permissionActions.Controls.Add(btnApplyRolePreset);

            groupBoxPermissions.Controls.Add(permissionActions);
            permissionActions.Visible = true;
            permissionActions.Enabled = true;
            permissionActions.BringToFront();
            checkedListPermissions.SendToBack();
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewUsers);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            if (btnSelectAllPermissions != null)
            {
                UIHelper.StyleButton(btnSelectAllPermissions, UIHelper.AccentColor);
                UIHelper.StyleButton(btnClearPermissions, UIHelper.NeutralColor);
                UIHelper.StyleButton(btnApplyRolePreset, UIHelper.AccentColor);
            }
            UIHelper.StyleTextBox(txtFullName);
            UIHelper.StyleTextBox(txtUserName);
            UIHelper.StyleTextBox(txtPassword);
            UIHelper.StyleTextBox(txtConfirmPassword);
            UIHelper.StyleTextBox(txtEmail);
            UIHelper.StyleTextBox(txtPhone);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleComboBox(cmbRole);
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        private async void UsersForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                userService.EnsureDefaultAdmin();

                LoadRoles();
                LoadPermissions();

                await LoadUsersAsync();

                ApplyPermissionUiState();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل بيانات المستخدمين", ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void LoadRoles()
        {
            cmbRole.Items.Clear();

            cmbRole.Items.Add("مدير النظام");
            cmbRole.Items.Add("الإدارة");
            cmbRole.Items.Add("شؤون الطلاب");
            cmbRole.Items.Add("المعلمون");
            cmbRole.Items.Add("المالية");
            cmbRole.Items.Add("المكتبة");
            cmbRole.Items.Add("النقل");
            cmbRole.Items.Add("التقارير");
            cmbRole.Items.Add("مدير المدرسة");
            cmbRole.Items.Add("وكيل المدرسة");
            cmbRole.Items.Add("شؤون الموظفين");
            cmbRole.Items.Add("أمين المكتبة");
            cmbRole.Items.Add("مسؤول النقل");
            cmbRole.Items.Add("موظف الاستقبال");
            cmbRole.Items.Add("مدقق");

            // لا نختار مدير النظام افتراضيًا؛ الحسابات الجديدة تبدأ بدور محدود.
            int safeDefaultIndex = cmbRole.Items.IndexOf("التقارير");
            if (safeDefaultIndex < 0)
                safeDefaultIndex = 0;
            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = safeDefaultIndex;

            cmbRole.SelectedIndexChanged -= cmbRole_SelectedIndexChanged;
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;
        }

        private void LoadPermissions()
        {
            checkedListPermissions.Items.Clear();

            // واجهة المدير تعرض صلاحية الشاشة فقط (Module.View)، ولا تعرض
            // مفاتيح العمليات الداخلية مثل Add/Edit/Delete. العمليات الفعلية
            // تبقى محكومة داخل الخدمات والأدوار، بينما هذه القائمة تحدد ما يظهر
            // للمستخدم في MainForm.
            foreach (string permissionKey in PermissionKeys.ScreenPermissions)
                AddPermission(permissionKey, PermissionKeys.GetDisplayName(permissionKey));

            ApplyRolePreset();
        }

        private void AddPermission(string key, string text)
        {
            string normalizedKey = PermissionKeys.NormalizePermissionKey(key);
            if (string.IsNullOrWhiteSpace(normalizedKey))
                return;

            checkedListPermissions.Items.Add(new PermissionListItem(normalizedKey, text));
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allUsers = await Task.Run(() => userService.GetAllUsers());

                ApplyFilter(txtSearch.Text.Trim());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFilter(string searchText)
        {
            if (allUsers == null)
                return;

            DataView dv = allUsers.DefaultView;

            string filter = "";
            string safe = UIHelper.EscapeDataViewFilterValue(searchText);

            if (!string.IsNullOrWhiteSpace(safe))
            {
                filter =
                    "(FullName LIKE '%" + safe + "%' " +
                    "OR UserName LIKE '%" + safe + "%' " +
                    "OR RoleName LIKE '%" + safe + "%' " +
                    "OR Email LIKE '%" + safe + "%' " +
                    "OR Phone LIKE '%" + safe + "%')";
            }

            dv.RowFilter = filter;

            dataGridViewUsers.DataSource = dv;

            lblRecordCount.Text = "عدد المستخدمين: " + dv.Count;

            FormatGrid();
        }

        private string EscapeFilter(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return value
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
        }

        private void FormatGrid()
        {
            if (dataGridViewUsers.Columns.Count == 0)
                return;

            UIHelper.StyleDataGridView(dataGridViewUsers);
            dataGridViewUsers.ReadOnly = true;
            dataGridViewUsers.AllowUserToDeleteRows = false;
            dataGridViewUsers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(
                UIHelper.FontFamily, UIHelper.BodyFontSize, System.Drawing.FontStyle.Bold);

            SetHeader("UserID", "الرقم");
            SetHeader("FullName", "الاسم الكامل");
            SetHeader("UserName", "اسم المستخدم");
            SetHeader("RoleName", "الدور");
            SetHeader("Permissions", "الصلاحيات");
            SetHeader("Email", "البريد");
            SetHeader("Phone", "الهاتف");
            SetHeader("IsActive", "نشط");
            SetHeader("MustChangePassword", "تغيير كلمة المرور");
            SetHeader("FailedLoginAttempts", "المحاولات الفاشلة");
            SetHeader("LockedAt", "وقت القفل");
            SetHeader("LastLoginAt", "آخر دخول");
            SetHeader("CreatedAt", "تاريخ الإنشاء");
            SetHeader("UpdatedAt", "آخر تعديل");

            if (dataGridViewUsers.Columns.Contains("UserID"))
                dataGridViewUsers.Columns["UserID"].Visible = false;
        }

        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewUsers.Columns.Contains(columnName))
                dataGridViewUsers.Columns[columnName].HeaderText = headerText;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            // الدور يطبّق صلاحيات افتراضية للمستخدم الجديد فقط، وفقط إذا لم يعدّل
            // المدير التحديدات يدويًا. أي تغيير يدوي في مربعات الصلاحيات يلغي
            // التطبيق التلقائي حتى لا تُمحى الصلاحيات المختارة بغير قصد.
            if (!isLoading && selectedUserId == 0 && !permissionsUserModified)
                ApplyRolePreset();
        }

        private void btnApplyRolePreset_Click(object sender, EventArgs e)
        {
            if (!CanManagePermissions())
                return;
            ApplyRolePreset();
            permissionsUserModified = true;
            UIHelper.ShowInformation("تم تطبيق الصلاحيات الافتراضية للدور المحدد. اضغط تعديل أو إضافة لحفظ التغيير.");
        }

        private void checkedListPermissions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isLoading || suppressPermissionTracking)
                return;
            permissionsUserModified = true;
        }

        private void ApplyRolePreset()
        {
            if (checkedListPermissions.Items.Count == 0 || cmbRole.SelectedItem == null)
                return;

            suppressPermissionTracking = true;
            try
            {
                ClearPermissionChecks();

                string roleName = PermissionKeys.NormalizeRoleName(cmbRole.SelectedItem.ToString());
                string permissions = PermissionKeys.GetRoleDefaults(roleName);
                foreach (string permission in permissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    CheckPermission(permission.Trim());
            }
            finally
            {
                suppressPermissionTracking = false;
            }
        }

        private void btnClearPermissions_Click(object sender, EventArgs e)
        {
            if (!CanManagePermissions())
                return;
            ClearPermissionChecks();
            permissionsUserModified = true;
            UIHelper.ShowInformation("تم إلغاء تحديد جميع الصلاحيات. اضغط تعديل أو إضافة لحفظ التغيير.");
        }

        private void btnSelectAllPermissions_Click(object sender, EventArgs e)
        {
            if (!CanManagePermissions())
                return;
            CheckAllPermissions();
            permissionsUserModified = true;
            UIHelper.ShowInformation("تم تحديد جميع الصلاحيات. اضغط تعديل أو إضافة لحفظ التغيير.");
        }

        private bool CanManagePermissions()
        {
            if (CurrentUser.HasAny(PermissionKeys.UsersManageRoles, PermissionKeys.UsersManage))
                return true;
            UIHelper.ShowError("ليس لديك صلاحية إدارة أدوار وصلاحيات المستخدمين.");
            return false;
        }

        private void ClearPermissionChecks()
        {
            if (checkedListPermissions == null)
                return;
            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                checkedListPermissions.SetItemChecked(i, false);
        }

        private void CheckAllPermissions()
        {
            if (checkedListPermissions == null)
                return;
            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                checkedListPermissions.SetItemChecked(i, true);
        }

        private void CheckPermission(string permissionKey)
        {
            // الأدوار القديمة تحتوي على مفاتيح عملية مثل Students.Add أو Students.Manage.
            // عند عرضها في الواجهة نحولها إلى Students.View حتى لا تعتمد القائمة على
            // ترتيب أو عدد مفاتيح العمليات.
            string normalizedKey = PermissionKeys.ToScreenPermission(permissionKey);
            if (string.IsNullOrWhiteSpace(normalizedKey))
                return;

            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
            {
                PermissionListItem item = checkedListPermissions.Items[i] as PermissionListItem;
                if (item != null && string.Equals(item.Key, normalizedKey, StringComparison.OrdinalIgnoreCase))
                    checkedListPermissions.SetItemChecked(i, true);
            }
        }

        private string GetSelectedPermissions()
        {
            List<string> selectedKeys = new List<string>();

            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
            {
                if (!checkedListPermissions.GetItemChecked(i))
                    continue;

                PermissionListItem item = checkedListPermissions.Items[i] as PermissionListItem;
                if (item != null && !string.IsNullOrWhiteSpace(item.Key))
                    selectedKeys.Add(item.Key);
            }

            return PermissionKeys.Serialize(selectedKeys);
        }

        private void SetPermissionsFromString(string permissions)
        {
            suppressPermissionTracking = true;
            try
            {
                ClearPermissionChecks();

                if (string.IsNullOrWhiteSpace(permissions))
                    return;

                string normalizedPermissions = PermissionKeys.NormalizePermissions(permissions);
                string[] parts = normalizedPermissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                HashSet<string> selectedKeys = new HashSet<string>(
                    parts.Select(part => PermissionKeys.ToScreenPermission(part.Trim()))
                         .Where(key => !string.IsNullOrWhiteSpace(key)),
                    StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                {
                    PermissionListItem item = checkedListPermissions.Items[i] as PermissionListItem;
                    if (item != null && selectedKeys.Contains(item.Key))
                        checkedListPermissions.SetItemChecked(i, true);
                }
            }
            finally
            {
                suppressPermissionTracking = false;
            }
        }

        private User BuildUserModel()
        {
            return new User
            {
                UserID = selectedUserId,
                FullName = txtFullName.Text.Trim(),
                UserName = txtUserName.Text.Trim(),
                RoleName = PermissionKeys.NormalizeRoleName(cmbRole.Text),
                Permissions = PermissionKeys.NormalizePermissions(GetSelectedPermissions()),
                Email = txtEmail.Text.Trim(),
                Phone = NormalizeDigits(txtPhone.Text).Trim(),
                IsActive = chkIsActive.Checked,
                MustChangePassword = chkMustChangePassword.Checked,
                Password = txtPassword.Text.Trim()
            };
        }

        private bool ValidateInputs(bool isUpdate)
        {
            string fullName = txtFullName.Text.Trim();
            string userName = txtUserName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = NormalizeDigits(txtPhone.Text).Trim();

            if (!UIHelper.IsValidArabicOrLatinName(fullName))
            {
                UIHelper.FocusAndWarn(txtFullName, "الاسم الكامل مطلوب ويجب أن يحتوي على أحرف فقط وبطول مناسب.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                ShowWarning("أدخل اسم المستخدم.");
                txtUserName.Focus();
                return false;
            }

            if (userName.Contains(" "))
            {
                ShowWarning("اسم المستخدم لا يجب أن يحتوي على مسافات.");
                txtUserName.Focus();
                return false;
            }

            if (userName.Length < 3)
            {
                ShowWarning("اسم المستخدم يجب ألا يقل عن 3 أحرف.");
                txtUserName.Focus();
                return false;
            }

            if (cmbRole.SelectedItem == null)
            {
                ShowWarning("اختر الدور.");
                cmbRole.Focus();
                return false;
            }

            // عند إنشاء حساب جديد يجب تحديد صلاحية واحدة على الأقل.
            // عند تعديل حساب موجود نسمح بحفظ صفر صلاحيات لتنفيذ زر "منع الكل".
            if (!isUpdate && checkedListPermissions.CheckedItems.Count == 0)
            {
                ShowWarning("اختر صلاحية واحدة على الأقل.");
                return false;
            }

            bool updatePassword = !isUpdate || !string.IsNullOrWhiteSpace(txtPassword.Text);

            if (updatePassword)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    ShowWarning("أدخل كلمة المرور.");
                    txtPassword.Focus();
                    return false;
                }

                if (txtPassword.Text.Length < 10 || !txtPassword.Text.Any(char.IsLetter) || !txtPassword.Text.Any(char.IsDigit))
                {
                    ShowWarning("كلمة المرور يجب ألا تقل عن 10 أحرف وتحتوي على أحرف وأرقام.");
                    txtPassword.Focus();
                    return false;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ShowWarning("كلمة المرور وتأكيدها غير متطابقين.");
                    txtConfirmPassword.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(email) && !UIHelper.IsValidEmail(email))
            {
                UIHelper.FocusAndWarn(txtEmail, "البريد الإلكتروني غير صحيح.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(phone) && !UIHelper.IsValidPhone(phone))
            {
                UIHelper.FocusAndWarn(txtPhone, "رقم الهاتف غير صحيح.");
                return false;
            }

            return true;
        }

        private void ApplyPermissionUiState()
        {
            bool canView = CurrentUser.CanView("Users") || CurrentUser.HasPermission(PermissionKeys.UsersManage);
            bool canAdd = CurrentUser.CanAdd("Users") || CurrentUser.HasPermission(PermissionKeys.UsersManage);
            bool canEdit = CurrentUser.CanEdit("Users") || CurrentUser.HasPermission(PermissionKeys.UsersManage);
            bool canDelete = CurrentUser.CanDelete("Users") || CurrentUser.HasPermission(PermissionKeys.UsersManage);
            bool canManageRoles = CurrentUser.HasAny(PermissionKeys.UsersManageRoles, PermissionKeys.UsersManage);

            btnAdd.Enabled = canAdd;
            btnUpdate.Enabled = canEdit;
            btnDelete.Enabled = canDelete;
            cmbRole.Enabled = canManageRoles;
            checkedListPermissions.Enabled = canManageRoles;
            btnSelectAllPermissions.Enabled = canManageRoles;
            btnClearPermissions.Enabled = canManageRoles;
            btnApplyRolePreset.Enabled = canManageRoles;

            if (groupBoxPermissions != null)
                groupBoxPermissions.Enabled = canView;

            btnAdd.AccessibleDescription = canAdd ? "إضافة مستخدم" : "لا توجد صلاحية إضافة المستخدمين";
            btnUpdate.AccessibleDescription = canEdit ? "تعديل مستخدم" : "لا توجد صلاحية تعديل المستخدمين";
            btnDelete.AccessibleDescription = canDelete ? "حذف مستخدم" : "لا توجد صلاحية حذف المستخدمين";
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs(false))
                    return;

                User user = BuildUserModel();

                await Task.Run(() => userService.AddUser(user, txtPassword.Text));

                ShowInfo("تمت إضافة المستخدم بنجاح.");

                await LoadUsersAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة المستخدم", ex);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserId <= 0)
                {
                    ShowWarning("اختر مستخدماً من الجدول أولاً.");
                    return;
                }

                if (!ValidateInputs(true))
                    return;

                User user = BuildUserModel();
                bool updatePassword = !string.IsNullOrWhiteSpace(txtPassword.Text);

                bool updated = await Task.Run(() =>
                    userService.UpdateUser(user, txtPassword.Text, updatePassword));

                ShowInfo(updated ? "تم تعديل المستخدم بنجاح." : "لم يتم تعديل المستخدم.");

                if (updated && CurrentUser.IsLoggedIn && CurrentUser.User.UserID == selectedUserId)
                    MainForm.Instance?.RefreshCurrentUserSession();

                await LoadUsersAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل المستخدم", ex);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserId <= 0)
                {
                    ShowWarning("اختر مستخدماً من الجدول أولاً.");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "هل تريد حذف هذا المستخدم؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                if (result != DialogResult.Yes)
                    return;

                bool deleted = await Task.Run(() => userService.DeleteUser(selectedUserId));

                ShowInfo(deleted ? "تم حذف المستخدم بنجاح." : "لم يتم حذف المستخدم.");

                await LoadUsersAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حذف المستخدم", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadUsersAsync();
            ClearInputs();
        }

        private void dataGridViewUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewUsers.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewUsers.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        private void FillFieldsFromRow(DataRow row)
        {
            if (row == null)
                return;

            selectedUserId = ReadRowInt(row, "UserID");

            txtFullName.Text = ReadRowText(row, "FullName");
            txtUserName.Text = ReadRowText(row, "UserName");

            string roleName = ReadRowText(row, "RoleName");

            if (cmbRole.Items.Contains(roleName))
                cmbRole.SelectedItem = roleName;
            else
                cmbRole.Text = roleName;

            txtEmail.Text = ReadRowText(row, "Email");
            txtPhone.Text = ReadRowText(row, "Phone");

            chkIsActive.Checked = ReadRowBool(row, "IsActive", true);
            chkMustChangePassword.Checked = ReadRowBool(row, "MustChangePassword", true);

            txtPassword.Clear();
            txtConfirmPassword.Clear();

            string permissions = ReadRowText(row, "Permissions");
            SetPermissionsFromString(permissions);
            permissionsUserModified = false;
        }

        private string ReadRowText(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
                return string.Empty;

            object value = row[columnName];
            return value == null || value == DBNull.Value ? string.Empty : value.ToString();
        }

        private int ReadRowInt(DataRow row, string columnName)
        {
            int value;
            return int.TryParse(ReadRowText(row, columnName), out value) ? value : 0;
        }

        private bool ReadRowBool(DataRow row, string columnName, bool fallback)
        {
            string value = ReadRowText(row, columnName);
            if (string.IsNullOrWhiteSpace(value))
                return fallback;

            bool parsed;
            return bool.TryParse(value, out parsed) ? parsed : fallback;
        }

        private bool ContainsDigits(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return NormalizeDigits(value).Any(char.IsDigit);
        }

        private bool IsValidEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                var address = new System.Net.Mail.MailAddress(value.Trim());
                return address.Address.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private string NormalizeDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return value
                .Replace('٠', '0')
                .Replace('١', '1')
                .Replace('٢', '2')
                .Replace('٣', '3')
                .Replace('٤', '4')
                .Replace('٥', '5')
                .Replace('٦', '6')
                .Replace('٧', '7')
                .Replace('٨', '8')
                .Replace('٩', '9');
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool show = chkShowPassword.Checked;

            txtPassword.PasswordChar = show ? '\0' : '●';
            txtConfirmPassword.PasswordChar = show ? '\0' : '●';
        }

        private void ClearInputs()
        {
            selectedUserId = 0;

            txtFullName.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtEmail.Clear();
            txtPhone.Clear();

            if (cmbRole.Items.Count > 0)
            {
                // الحسابات الجديدة تبدأ بدور محدود، ولا يجوز أن يعود النموذج
                // تلقائيًا إلى مدير النظام بعد الحفظ أو الضغط على مسح.
                int safeDefaultIndex = cmbRole.Items.IndexOf("التقارير");
                cmbRole.SelectedIndex = safeDefaultIndex >= 0 ? safeDefaultIndex : 0;
            }

            chkIsActive.Checked = true;
            chkMustChangePassword.Checked = true;
            chkShowPassword.Checked = false;

            txtPassword.PasswordChar = '●';
            txtConfirmPassword.PasswordChar = '●';

            ApplyRolePreset();

            permissionsUserModified = false;
            txtFullName.Focus();
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

    }
}
