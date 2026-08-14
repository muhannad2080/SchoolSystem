using System;
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

        public UsersForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            Dock = DockStyle.Fill;
            Load += UsersForm_Load;
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewUsers);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
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

                ClearInputs();
            }
            catch (Exception ex)
            {
                LogException("UsersForm_Load", ex);
                ShowError("تعذر تحميل بيانات المستخدمين. تحقق من الاتصال وحاول مرة أخرى.");
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

            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 0;

            cmbRole.SelectedIndexChanged -= cmbRole_SelectedIndexChanged;
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;
        }

        private void LoadPermissions()
        {
            checkedListPermissions.Items.Clear();

            AddPermission(PermissionKeys.DashboardView, "عرض لوحة التحكم");

            AddPermission(PermissionKeys.StudentsView, "عرض الطلاب");
            AddPermission(PermissionKeys.StudentsManage, "إدارة الطلاب");
            AddPermission(PermissionKeys.EnrollmentManage, "القبول والتسجيل");
            AddPermission(PermissionKeys.ClassAssignmentManage, "توزيع الطلاب على الفصول");

            AddPermission(PermissionKeys.TeachersManage, "إدارة المعلمين");
            AddPermission(PermissionKeys.StaffAttendanceManage, "حضور وانصراف الموظفين");
            AddPermission(PermissionKeys.PayrollManage, "الرواتب والعقود");

            AddPermission(PermissionKeys.SubjectsManage, "إدارة المواد");
            AddPermission(PermissionKeys.ClassesManage, "إدارة الصفوف والفصول");
            AddPermission(PermissionKeys.TimetableManage, "الجداول الدراسية");

            AddPermission(PermissionKeys.AttendanceManage, "حضور الطلاب");
            AddPermission(PermissionKeys.GradesManage, "إدارة الدرجات");

            AddPermission(PermissionKeys.FeesManage, "الرسوم الدراسية");
            AddPermission(PermissionKeys.VouchersManage, "السندات قبض/صرف");
            AddPermission(PermissionKeys.ExpensesManage, "المصروفات");

            AddPermission(PermissionKeys.LibraryManage, "المكتبة");
            AddPermission(PermissionKeys.TransportManage, "النقل");

            AddPermission(PermissionKeys.ReportsView, "التقارير");
            AddPermission(PermissionKeys.UsersManage, "إدارة المستخدمين والصلاحيات");
            AddPermission(PermissionKeys.AuditLogsView, "عرض سجل التدقيق");
            AddPermission(PermissionKeys.SettingsManage, "الإعدادات والنسخ الاحتياطي");

            ApplyRolePreset();
        }

        private void AddPermission(string key, string text)
        {
            checkedListPermissions.Items.Add(key + " - " + text);
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
            // الدور يطبّق صلاحيات افتراضية للمستخدم الجديد فقط.
            // عند تحرير مستخدم موجود نحافظ على الصلاحيات المخصصة التي حمّلناها من قاعدة البيانات.
            if (!isLoading && selectedUserId == 0)
                ApplyRolePreset();
        }

        private void ApplyRolePreset()
        {
            if (checkedListPermissions.Items.Count == 0 || cmbRole.SelectedItem == null)
                return;

            ClearPermissionChecks();

            string roleName = PermissionKeys.NormalizeRoleName(cmbRole.SelectedItem.ToString());
            string permissions = PermissionKeys.GetRoleDefaults(roleName);
            foreach (string permission in permissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                CheckPermission(permission.Trim());
        }

        private void ClearPermissionChecks()
        {
            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                checkedListPermissions.SetItemChecked(i, false);
        }

        private void CheckAllPermissions()
        {
            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                checkedListPermissions.SetItemChecked(i, true);
        }

        private void CheckPermission(string permissionKey)
        {
            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
            {
                string item = checkedListPermissions.Items[i].ToString();

                string itemKey = item.Split('-')[0].Trim();
                if (string.Equals(itemKey, PermissionKeys.NormalizePermissionKey(permissionKey), StringComparison.OrdinalIgnoreCase))
                    checkedListPermissions.SetItemChecked(i, true);
            }
        }

        private string GetSelectedPermissions()
        {
            StringBuilder sb = new StringBuilder();

            foreach (object item in checkedListPermissions.CheckedItems)
            {
                string text = item.ToString();
                string key = text.Split('-')[0].Trim();

                if (sb.Length > 0)
                    sb.Append(",");

                sb.Append(key);
            }

            return PermissionKeys.NormalizePermissions(sb.ToString());
        }

        private void SetPermissionsFromString(string permissions)
        {
            ClearPermissionChecks();

            if (string.IsNullOrWhiteSpace(permissions))
                return;

            string normalizedPermissions = PermissionKeys.NormalizePermissions(permissions);
            string[] parts = normalizedPermissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
            {
                string item = checkedListPermissions.Items[i].ToString();

                foreach (string part in parts)
                {
                    string key = part.Trim();

                    string itemKey = item.Split('-')[0].Trim();
                    string normalizedKey = PermissionKeys.NormalizePermissionKey(key);
                    if (!string.IsNullOrWhiteSpace(normalizedKey) &&
                        string.Equals(itemKey, normalizedKey, StringComparison.OrdinalIgnoreCase))
                        checkedListPermissions.SetItemChecked(i, true);
                }
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

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ShowWarning("أدخل الاسم الكامل.");
                txtFullName.Focus();
                return false;
            }

            if (ContainsDigits(fullName))
            {
                ShowWarning("الاسم الكامل لا يجب أن يحتوي على أرقام.");
                txtFullName.Focus();
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

            if (checkedListPermissions.CheckedItems.Count == 0)
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

                if (txtPassword.Text.Length < 6)
                {
                    ShowWarning("كلمة المرور يجب ألا تقل عن 6 أحرف.");
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

            if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
            {
                ShowWarning("البريد الإلكتروني غير صحيح.");
                txtEmail.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!phone.All(char.IsDigit))
                {
                    ShowWarning("رقم الهاتف يجب أن يحتوي على أرقام فقط.");
                    txtPhone.Focus();
                    return false;
                }

                if (phone.Length < 7 || phone.Length > 15)
                {
                    ShowWarning("رقم الهاتف غير صحيح.");
                    txtPhone.Focus();
                    return false;
                }
            }

            return true;
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
                LogException("AddUser", ex);
                ShowError(GetSafeOperationError("الإضافة", ex));
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
                LogException("UpdateUser", ex);
                ShowError(GetSafeOperationError("التعديل", ex));
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
                LogException("DeleteUser", ex);
                ShowError(GetSafeOperationError("الحذف", ex));
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
            selectedUserId = Convert.ToInt32(row["UserID"]);

            txtFullName.Text = row["FullName"] == DBNull.Value ? "" : row["FullName"].ToString();
            txtUserName.Text = row["UserName"] == DBNull.Value ? "" : row["UserName"].ToString();

            string roleName = row["RoleName"] == DBNull.Value ? "" : row["RoleName"].ToString();

            if (cmbRole.Items.Contains(roleName))
                cmbRole.SelectedItem = roleName;
            else
                cmbRole.Text = roleName;

            txtEmail.Text = row["Email"] == DBNull.Value ? "" : row["Email"].ToString();
            txtPhone.Text = row["Phone"] == DBNull.Value ? "" : row["Phone"].ToString();

            chkIsActive.Checked = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
            chkMustChangePassword.Checked = row["MustChangePassword"] != DBNull.Value && Convert.ToBoolean(row["MustChangePassword"]);

            txtPassword.Clear();
            txtConfirmPassword.Clear();

            string permissions = row["Permissions"] == DBNull.Value ? "" : row["Permissions"].ToString();
            SetPermissionsFromString(permissions);
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
                cmbRole.SelectedIndex = 0;

            chkIsActive.Checked = true;
            chkMustChangePassword.Checked = true;
            chkShowPassword.Checked = false;

            txtPassword.PasswordChar = '●';
            txtConfirmPassword.PasswordChar = '●';

            ApplyRolePreset();

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

        private string GetSafeOperationError(string operation, Exception ex)
        {
            if (ex is UnauthorizedAccessException || ex is InvalidOperationException)
                return ex.Message;

            return "تعذر تنفيذ " + operation + " المستخدم. تحقق من البيانات والاتصال ثم حاول مرة أخرى.";
        }

        private void LogException(string operation, Exception ex)
        {
            try
            {
                string directory = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SchoolSystem", "Logs");
                System.IO.Directory.CreateDirectory(directory);
                string path = System.IO.Path.Combine(directory, "errors.log");
                System.IO.File.AppendAllText(path,
                    DateTime.Now.ToString("s") + " [" + operation + "] " + ex + Environment.NewLine);
            }
            catch
            {
                // لا نسمح لفشل التسجيل بأن يعطل واجهة المستخدم.
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
    }
}
