using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        // تعريف الدالة UserService لتنفيذ العملية المرتبطة بها.
        private readonly UserService userService = new UserService();


        private int selectedUserId = 0;

        private DataTable allUsers;

        private bool isLoading = false;

        private bool permissionsUserModified = false;
        // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
        private bool suppressPermissionTracking = false;

        private bool selectedSystemAdministrator = false;


        // تعريف الصنف PermissionListItem المسؤول عن وظائف طبقة UI.
        private sealed class PermissionListItem
        {

            public string Key { get; private set; }

            public string DisplayText { get; private set; }

            // تعريف الدالة PermissionListItem لتنفيذ العملية المرتبطة بها.
            public PermissionListItem(string key, string displayText)
            {
                Key = key ?? string.Empty;
                DisplayText = displayText ?? string.Empty;
            }

            // تعريف الدالة ToString لتنفيذ العملية المرتبطة بها.
            public override string ToString()
            {

                return DisplayText;
            }
        }

        // تعريف الدالة UsersForm لتنفيذ العملية المرتبطة بها.
        public UsersForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            ApplyPermissionUiState();
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            Dock = DockStyle.Fill;
            Load += UsersForm_Load;

            // تحديد أقصى طول لأسماء المستخدمين.
            txtUserName.MaxLength = 50;
        }

        // تعريف الدالة ApplyCustomStyles لتنفيذ العملية المرتبطة بها.
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
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                UIHelper.StylePrimaryButton(btnSavePermissions);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                UIHelper.StyleButton(btnSelectAllPermissions, UIHelper.AccentColor);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                UIHelper.StyleButton(btnClearPermissions, UIHelper.NeutralColor);
                UIHelper.StyleButton(btnApplyRolePreset, UIHelper.AccentColor);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                UIHelper.StyleButton(btnReloadPermissions, UIHelper.NeutralColor);
                UIHelper.StyleButton(btnCloseForm, UIHelper.DangerColor);
            }
            UIHelper.StyleTextBox(txtFullName);
            UIHelper.StyleTextBox(txtUserName);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            UIHelper.StyleTextBox(txtPassword);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            UIHelper.StyleTextBox(txtConfirmPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            UIHelper.StyleTextBox(txtEmail);
            UIHelper.StyleTextBox(txtPhone);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleComboBox(cmbRole);
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        // تعريف الدالة UsersForm_Load لتنفيذ العملية المرتبطة بها.
        private async void UsersForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                isLoading = true;

                userService.EnsureDefaultAdmin();

                LoadRoles();
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                LoadPermissions();


                await LoadUsersAsync();

                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة LoadRoles لتنفيذ العملية المرتبطة بها.
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


            int safeDefaultIndex = cmbRole.Items.IndexOf("التقارير");
            if (safeDefaultIndex < 0)
                safeDefaultIndex = 0;
            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = safeDefaultIndex;

            cmbRole.SelectedIndexChanged -= cmbRole_SelectedIndexChanged;

            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;
        }

        // تعريف الدالة LoadPermissions لتنفيذ العملية المرتبطة بها.
        private void LoadPermissions()
        {
            checkedListPermissions.Items.Clear();

            // واجهة المدير تعرض صلاحية الشاشة فقط (Module.View)، ولا تعرض



            foreach (string permissionKey in PermissionKeys.ScreenPermissions)
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                AddPermission(permissionKey, PermissionKeys.GetDisplayName(permissionKey));

            ApplyRolePreset();
        }

        // تعريف الدالة AddPermission لتنفيذ العملية المرتبطة بها.
        private void AddPermission(string key, string text)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            string normalizedKey = PermissionKeys.NormalizePermissionKey(key);
            if (string.IsNullOrWhiteSpace(normalizedKey))
                return;


            checkedListPermissions.Items.Add(new PermissionListItem(normalizedKey, text));
        }

        // تعريف الدالة LoadUsersAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadUsersAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة ApplyFilter لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة EscapeFilter لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة FormatGrid لتنفيذ العملية المرتبطة بها.
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
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            SetHeader("Permissions", "الصلاحيات");
            SetHeader("Email", "البريد");
            SetHeader("Phone", "الهاتف");
            SetHeader("IsActive", "نشط");
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            SetHeader("MustChangePassword", "تغيير كلمة المرور");
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            SetHeader("FailedLoginAttempts", "المحاولات الفاشلة");
            SetHeader("LockedAt", "وقت القفل");
            SetHeader("LastLoginAt", "آخر دخول");
            SetHeader("CreatedAt", "تاريخ الإنشاء");
            SetHeader("UpdatedAt", "آخر تعديل");

            if (dataGridViewUsers.Columns.Contains("UserID"))
                dataGridViewUsers.Columns["UserID"].Visible = false;
        }

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewUsers.Columns.Contains(columnName))
                dataGridViewUsers.Columns[columnName].HeaderText = headerText;
        }

        // تعريف الدالة txtSearch_TextChanged لتنفيذ العملية المرتبطة بها.
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        // تعريف الدالة cmbRole_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            // عند تغيير الدور تُطبَّق صلاحياته الافتراضية على القائمة فقط إذا لم يعدّل

            // التطبيق التلقائي حتى لا تُمحى الصلاحيات المختارة بغير قصد.
            if (!isLoading && !permissionsUserModified)
                ApplyRolePreset();
        }

        // تعريف الدالة btnApplyRolePreset_Click لتنفيذ العملية المرتبطة بها.
        private void btnApplyRolePreset_Click(object sender, EventArgs e)
        {
            if (!CanManagePermissions())
                return;
            ApplyRolePreset();
            permissionsUserModified = true;
            UIHelper.ShowInformation("تم تطبيق الصلاحيات الافتراضية للدور المحدد. اضغط تعديل أو إضافة لحفظ التغيير.");
        }

        // تعريف الدالة checkedListPermissions_ItemCheck لتنفيذ العملية المرتبطة بها.
        private void checkedListPermissions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isLoading || suppressPermissionTracking)
                return;
            permissionsUserModified = true;
        }

        // تعريف الدالة btnSavePermissions_Click لتنفيذ العملية المرتبطة بها.
        private async void btnSavePermissions_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (!CanManagePermissions())
                    return;

                if (selectedUserId <= 0)
                {
                    ShowWarning("اختر مستخدماً من الجدول أولاً.");

                    return;
                }

                List<string> selectedKeys = new List<string>();

                for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                {
                    if (!checkedListPermissions.GetItemChecked(i))
                        continue;

                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    PermissionListItem item = checkedListPermissions.Items[i] as PermissionListItem;
                    if (item != null && !string.IsNullOrWhiteSpace(item.Key))
                        selectedKeys.Add(item.Key);
                }

                string userNameBeforeReload = txtUserName.Text.Trim();
                if (string.IsNullOrWhiteSpace(userNameBeforeReload))
                    userNameBeforeReload = selectedUserId.ToString();


                string effective = await Task.Run(() =>
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    userService.SaveUserPermissions(selectedUserId, selectedKeys));

                if (CurrentUser.IsLoggedIn && CurrentUser.User != null &&
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    CurrentUser.User.UserID == selectedUserId)
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    MainForm.Instance?.RefreshCurrentUserSession();


                await LoadUsersAsync();

                ShowInfo(string.Format(
                    "تم حفظ صلاحيات الشاشات بنجاح للمستخدم {0}. عدد الشاشات: {1}",
                    userNameBeforeReload,
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    PermissionKeys.GetScreenKeysFromPermissions(effective).Count));
                ApplicationLogger.LogUsage("شاشة المستخدمين", "حفظ الصلاحيات", "تم حفظ صلاحيات الشاشات بنجاح للمستخدم " + userNameBeforeReload);

                permissionsUserModified = false;
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("حفظ الصلاحيات", ex);
            }
        }

        // تعريف الدالة btnReloadPermissions_Click لتنفيذ العملية المرتبطة بها.
        private async void btnReloadPermissions_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (selectedUserId <= 0)
                {
                    ShowWarning("اختر مستخدماً من الجدول أولاً.");

                    return;
                }


                await LoadUsersAsync();

                foreach (DataGridViewRow row in dataGridViewUsers.Rows)
                {
                    DataRowView rowView = row.DataBoundItem as DataRowView;
                    if (rowView == null)
                        continue;

                    object value = rowView.Row["UserID"];
                    if (value != null && value != DBNull.Value &&
                        Convert.ToInt32(value) == selectedUserId)
                    {
                        dataGridViewUsers.ClearSelection();
                        row.Selected = true;
                        FillFieldsFromRow(rowView.Row);
                        break;
                    }
                }

                ShowInfo("تم إعادة تحميل صلاحيات المستخدم من قاعدة البيانات.");
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("إعادة تحميل الصلاحيات", ex);
            }
        }

        // تعريف الدالة btnCloseForm_Click لتنفيذ العملية المرتبطة بها.
        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            MainForm.Instance?.ShowWelcomeScreen();
        }

        /// <summary>
        /// يُحدِّث رأس لوحة الصلاحيات (اسم المستخدم والدور).
        /// </summary>
        // تعريف الدالة UpdatePermissionHeader لتنفيذ العملية المرتبطة بها.
        private void UpdatePermissionHeader(string userName, string roleName)
        {
            if (lblPermissionHeader == null)
                return;

            string displayName = string.IsNullOrWhiteSpace(userName) ? "مستخدم جديد" : userName;
            string role = string.IsNullOrWhiteSpace(roleName) ? "-" : roleName;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            lblPermissionHeader.Text = string.Format("صلاحيات المستخدم — المستخدم: {0} — الدور: {1}", displayName, role);
        }

        /// <summary>
        /// يُحدِّث تلميح الشاشات التي يوفرها الدور تلقائياً (قراءة فقط).
        /// </summary>
        // تعريف الدالة UpdateRoleScreensLabel لتنفيذ العملية المرتبطة بها.
        private void UpdateRoleScreensLabel(string roleName)
        {
            if (lblRoleScreens == null)
                return;

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            string normalizedRole = PermissionKeys.NormalizeRoleName(roleName);
            if (string.IsNullOrWhiteSpace(normalizedRole))
            {
                lblRoleScreens.Text = "";

                return;
            }

            List<string> screens = new List<string>();
            foreach (string permission in PermissionKeys.GetRoleDefaults(normalizedRole)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string screen = PermissionKeys.ToScreenPermission(permission.Trim());
                if (!string.IsNullOrWhiteSpace(screen) && !screens.Contains(screen, StringComparer.OrdinalIgnoreCase))
                    screens.Add(screen);
            }

            lblRoleScreens.Text = screens.Count == 0
                ? "صلاحيات الدور التلقائية: لا توجد (يُحدد يدوياً من القائمة)"
                : "صلاحيات الدور التلقائية: " + string.Join("، ",
                    screens.Select(key => PermissionKeys.GetDisplayName(key)));
        }

        // تعريف الدالة ApplyRolePreset لتنفيذ العملية المرتبطة بها.
        private void ApplyRolePreset()
        {
            if (checkedListPermissions.Items.Count == 0 || cmbRole.SelectedItem == null)
                return;

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            suppressPermissionTracking = true;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                ClearPermissionChecks();

                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string roleName = PermissionKeys.NormalizeRoleName(cmbRole.SelectedItem.ToString());
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string permissions = PermissionKeys.GetRoleDefaults(roleName);
                foreach (string permission in permissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    CheckPermission(permission.Trim());

                UpdateRoleScreensLabel(roleName);
            }

            finally
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                suppressPermissionTracking = false;
            }
        }

        // تعريف الدالة btnClearPermissions_Click لتنفيذ العملية المرتبطة بها.
        private void btnClearPermissions_Click(object sender, EventArgs e)
        {
            if (!CanManagePermissions())
                return;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            ClearPermissionChecks();
            permissionsUserModified = true;
            UIHelper.ShowInformation("تم إلغاء تحديد جميع الصلاحيات. اضغط تعديل أو إضافة لحفظ التغيير.");
        }

        // تعريف الدالة btnSelectAllPermissions_Click لتنفيذ العملية المرتبطة بها.
        private void btnSelectAllPermissions_Click(object sender, EventArgs e)
        {
            if (!CanManagePermissions())
                return;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CheckAllPermissions();
            permissionsUserModified = true;
            UIHelper.ShowInformation("تم تحديد جميع الصلاحيات. اضغط تعديل أو إضافة لحفظ التغيير.");
        }

        // تعريف الدالة CanManagePermissions لتنفيذ العملية المرتبطة بها.
        private bool CanManagePermissions()
        {
            if (!CurrentUser.IsAdmin() && !CurrentUser.HasAny(PermissionKeys.PermissionsManage, PermissionKeys.UsersManageRoles, PermissionKeys.UsersManage))
            {
                UIHelper.ShowError("ليس لديك صلاحية إدارة أدوار وصلاحيات المستخدمين.");

                return false;
            }

            if (selectedSystemAdministrator)
            {
                UIHelper.ShowInformation("حساب مدير النظام محمي وتُمنح له جميع صلاحيات الشاشات تلقائياً. لا يمكن تعديل صلاحياته من الواجهة.");

                return false;
            }


            return true;
        }

        // تعريف الدالة ClearPermissionChecks لتنفيذ العملية المرتبطة بها.
        private void ClearPermissionChecks()
        {
            if (checkedListPermissions == null)
                return;

            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                checkedListPermissions.SetItemChecked(i, false);
        }

        // تعريف الدالة CheckAllPermissions لتنفيذ العملية المرتبطة بها.
        private void CheckAllPermissions()
        {
            if (checkedListPermissions == null)
                return;

            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                checkedListPermissions.SetItemChecked(i, true);
        }

        // تعريف الدالة CheckPermission لتنفيذ العملية المرتبطة بها.
        private void CheckPermission(string permissionKey)
        {



            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            string normalizedKey = PermissionKeys.ToScreenPermission(permissionKey);
            if (string.IsNullOrWhiteSpace(normalizedKey))
                return;


            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionListItem item = checkedListPermissions.Items[i] as PermissionListItem;
                if (item != null && string.Equals(item.Key, normalizedKey, StringComparison.OrdinalIgnoreCase))
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    checkedListPermissions.SetItemChecked(i, true);
            }
        }

        // تعريف الدالة GetSelectedPermissions لتنفيذ العملية المرتبطة بها.
        private string GetSelectedPermissions()
        {
            List<string> selectedKeys = new List<string>();


            for (int i = 0; i < checkedListPermissions.Items.Count; i++)
            {
                if (!checkedListPermissions.GetItemChecked(i))
                    continue;

                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionListItem item = checkedListPermissions.Items[i] as PermissionListItem;
                if (item != null && !string.IsNullOrWhiteSpace(item.Key))
                    selectedKeys.Add(item.Key);
            }


            return PermissionKeys.Serialize(selectedKeys);
        }

        // تعريف الدالة SetPermissionsFromString لتنفيذ العملية المرتبطة بها.
        private void SetPermissionsFromString(string permissions)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            suppressPermissionTracking = true;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                ClearPermissionChecks();

                if (string.IsNullOrWhiteSpace(permissions))
                    return;

                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string normalizedPermissions = PermissionKeys.NormalizePermissions(permissions);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string[] parts = normalizedPermissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                HashSet<string> selectedKeys = new HashSet<string>(
                    parts.Select(part => PermissionKeys.ToScreenPermission(part.Trim()))
                         .Where(key => !string.IsNullOrWhiteSpace(key)),
                    StringComparer.OrdinalIgnoreCase);


                for (int i = 0; i < checkedListPermissions.Items.Count; i++)
                {
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    PermissionListItem item = checkedListPermissions.Items[i] as PermissionListItem;
                    if (item != null && selectedKeys.Contains(item.Key))
                        // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                        checkedListPermissions.SetItemChecked(i, true);
                }
            }

            finally
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                suppressPermissionTracking = false;
            }
        }

        // تعريف الدالة BuildUserModel لتنفيذ العملية المرتبطة بها.
        private User BuildUserModel()
        {

            return new User
            {
                UserID = selectedUserId,
                FullName = txtFullName.Text.Trim(),
                UserName = txtUserName.Text.Trim(),
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                RoleName = PermissionKeys.NormalizeRoleName(cmbRole.Text),
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                Permissions = PermissionKeys.NormalizePermissions(GetSelectedPermissions()),
                Email = txtEmail.Text.Trim(),
                Phone = NormalizeDigits(txtPhone.Text).Trim(),
                IsActive = chkIsActive.Checked,
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                MustChangePassword = chkMustChangePassword.Checked,
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                Password = txtPassword.Text.Trim()
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            };
        }

        // تعريف الدالة ValidateInputs لتنفيذ العملية المرتبطة بها.
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

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            bool updatePassword = !isUpdate || !string.IsNullOrWhiteSpace(txtPassword.Text);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (updatePassword)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    ShowWarning("أدخل كلمة المرور.");
                    // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                    txtPassword.Focus();
                    // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                    return false;
                }

                if (txtPassword.Text.Length < 10 || !txtPassword.Text.Any(char.IsLetter) || !txtPassword.Text.Any(char.IsDigit))
                {
                    ShowWarning("كلمة المرور يجب ألا تقل عن 10 أحرف وتحتوي على أحرف وأرقام.");
                    // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                    txtPassword.Focus();
                    // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                    return false;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ShowWarning("كلمة المرور وتأكيدها غير متطابقين.");
                    // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                    txtConfirmPassword.Focus();
                    // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
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

        // تعريف الدالة ApplyPermissionUiState لتنفيذ العملية المرتبطة بها.
        private void ApplyPermissionUiState()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            bool canManage = CurrentUser.CanAccessModule("Users");
            bool canView = canManage;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            bool canEditSelectedPermissions = canManage && !selectedSystemAdministrator;

            btnAdd.Enabled = canManage;
            btnUpdate.Enabled = canManage;
            btnDelete.Enabled = canManage && !selectedSystemAdministrator;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            cmbRole.Enabled = canEditSelectedPermissions;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            chkIsActive.Enabled = canEditSelectedPermissions;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            checkedListPermissions.Enabled = canEditSelectedPermissions;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnSelectAllPermissions.Enabled = canEditSelectedPermissions;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnClearPermissions.Enabled = canEditSelectedPermissions;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnApplyRolePreset.Enabled = canEditSelectedPermissions;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnSavePermissions.Enabled = canEditSelectedPermissions;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnReloadPermissions.Enabled = canManage;
            btnCloseForm.Enabled = true;

            if (selectedSystemAdministrator && lblPermissionHeader != null)
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                lblPermissionHeader.Text = "صلاحيات المستخدم — مدير النظام (محمي — وصول كامل ثابت)";

            if (groupBoxPermissions != null)
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                groupBoxPermissions.Enabled = canView;

            btnAdd.AccessibleDescription = canManage ? "إضافة مستخدم" : "لا توجد صلاحية إدارة المستخدمين";
            btnUpdate.AccessibleDescription = canManage ? "تعديل مستخدم" : "لا توجد صلاحية إدارة المستخدمين";
            btnDelete.AccessibleDescription = canManage ? "حذف مستخدم" : "لا توجد صلاحية إدارة المستخدمين";
        }

        // تعريف الدالة btnAdd_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (!ValidateInputs(false))
                    return;

                User user = BuildUserModel();

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                string password = txtPassword.Text;
                await Task.Run(() => userService.AddUser(user, password));

                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                ShowInfo("تمت إضافة المستخدم بنجاح.");
                ApplicationLogger.LogUsage("شاشة المستخدمين", "إضافة مستخدم", "تمت إضافة المستخدم بنجاح");


                await LoadUsersAsync();
                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة المستخدم", ex);
            }
        }

        // تعريف الدالة btnUpdate_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                bool updatePassword = !string.IsNullOrWhiteSpace(txtPassword.Text);
                string password = txtPassword.Text;


                bool updated = await Task.Run(() =>
                    // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                    userService.UpdateUser(user, password, updatePassword));

                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                ShowInfo(updated ? "تم تعديل المستخدم بنجاح." : "لم يتم تعديل المستخدم.");
                if (updated) ApplicationLogger.LogUsage("شاشة المستخدمين", "تعديل مستخدم", "تم تعديل المستخدم بنجاح");

                if (updated && CurrentUser.IsLoggedIn && CurrentUser.User.UserID == selectedUserId)
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    MainForm.Instance?.RefreshCurrentUserSession();


                await LoadUsersAsync();
                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل المستخدم", ex);
            }
        }

        // تعريف الدالة btnDelete_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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
                if (deleted) ApplicationLogger.LogUsage("شاشة المستخدمين", "حذف مستخدم", "تم حذف المستخدم بنجاح");


                await LoadUsersAsync();
                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("حذف المستخدم", ex);
            }
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // تعريف الدالة btnRefresh_Click لتنفيذ العملية المرتبطة بها.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {

            await LoadUsersAsync();
            ClearInputs();
        }

        // تعريف الدالة dataGridViewUsers_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewUsers.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewUsers.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        // تعريف الدالة FillFieldsFromRow لتنفيذ العملية المرتبطة بها.
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
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            chkMustChangePassword.Checked = ReadRowBool(row, "MustChangePassword", true);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            txtPassword.Clear();
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            txtConfirmPassword.Clear();

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            string permissions = ReadRowText(row, "Permissions");
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            selectedSystemAdministrator = PermissionKeys.IsSystemAdministratorRole(roleName);
            if (selectedSystemAdministrator)
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                permissions = PermissionKeys.Serialize(PermissionKeys.ScreenPermissions);

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            SetPermissionsFromString(permissions);
            permissionsUserModified = false;

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            UpdatePermissionHeader(txtUserName.Text.Trim(), roleName);
            UpdateRoleScreensLabel(roleName);
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            ApplyPermissionUiState();
        }

        // تعريف الدالة ReadRowText لتنفيذ العملية المرتبطة بها.
        private string ReadRowText(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
                return string.Empty;

            object value = row[columnName];

            return value == null || value == DBNull.Value ? string.Empty : value.ToString();
        }

        // تعريف الدالة ReadRowInt لتنفيذ العملية المرتبطة بها.
        private int ReadRowInt(DataRow row, string columnName)
        {
            int value;

            return int.TryParse(ReadRowText(row, columnName), out value) ? value : 0;
        }

        // تعريف الدالة ReadRowBool لتنفيذ العملية المرتبطة بها.
        private bool ReadRowBool(DataRow row, string columnName, bool fallback)
        {
            string value = ReadRowText(row, columnName);
            if (string.IsNullOrWhiteSpace(value))
                return fallback;

            bool parsed;

            return bool.TryParse(value, out parsed) ? parsed : fallback;
        }

        // تعريف الدالة ContainsDigits لتنفيذ العملية المرتبطة بها.
        private bool ContainsDigits(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;


            return NormalizeDigits(value).Any(char.IsDigit);
        }

        // تعريف الدالة IsValidEmail لتنفيذ العملية المرتبطة بها.
        private bool IsValidEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة NormalizeDigits لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة chkShowPassword_CheckedChanged لتنفيذ العملية المرتبطة بها.
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            bool show = chkShowPassword.Checked;

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtPassword.PasswordChar = show ? '\0' : '●';
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtConfirmPassword.PasswordChar = show ? '\0' : '●';
        }

        // تعريف الدالة ClearInputs لتنفيذ العملية المرتبطة بها.
        private void ClearInputs()
        {
            selectedUserId = 0;
            selectedSystemAdministrator = false;

            txtFullName.Clear();
            txtUserName.Clear();
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            txtPassword.Clear();
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            txtConfirmPassword.Clear();
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            txtEmail.Clear();
            txtPhone.Clear();

            if (cmbRole.Items.Count > 0)
            {


                int safeDefaultIndex = cmbRole.Items.IndexOf("التقارير");
                cmbRole.SelectedIndex = safeDefaultIndex >= 0 ? safeDefaultIndex : 0;
            }

            chkIsActive.Checked = true;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            chkMustChangePassword.Checked = true;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            chkShowPassword.Checked = false;

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtPassword.PasswordChar = '●';
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtConfirmPassword.PasswordChar = '●';

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            ApplyRolePreset();

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            UpdatePermissionHeader("", "");
            UpdateRoleScreensLabel(cmbRole.SelectedItem != null ? cmbRole.SelectedItem.ToString() : "");

            permissionsUserModified = false;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            ApplyPermissionUiState();
            txtFullName.Focus();
        }

        // تعريف الدالة ShowInfo لتنفيذ العملية المرتبطة بها.
        private void ShowInfo(string message)
        {

            MessageBox.Show(message, "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        // تعريف الدالة ShowWarning لتنفيذ العملية المرتبطة بها.
        private void ShowWarning(string message)
        {

            MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

    }
}
