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
        private readonly UserService userService = new UserService();

        private int selectedUserId = 0;
        private DataTable allUsers;
        private bool isLoading = false;
        private bool permissionsUserModified = false;
        private bool suppressPermissionTracking = false;
        private bool selectedSystemAdministrator = false;
        private FlowLayoutPanel permissionActions;
        private TableLayoutPanel permissionLayout;
        private Button btnSelectAllPermissions;
        private Button btnClearPermissions;
        private Button btnApplyRolePreset;
        private Button btnSavePermissions;
        private Button btnReloadPermissions;
        private Button btnCloseForm;
        private Label lblPermissionHeader;
        private Label lblRoleScreens;

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
                return DisplayText;
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
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowOnly,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = true,
                RightToLeft = RightToLeft.Yes,
                Padding = new Padding(0, 4, 0, 4),
                Margin = new Padding(0),
                MinimumSize = new Size(0, 38)
            };

            btnSavePermissions = new Button
            {
                Name = "btnSavePermissions",
                Text = "حفظ الصلاحيات",
                Width = 150,
                Height = 30,
                TabIndex = 0,
                UseVisualStyleBackColor = false
            };
            btnSelectAllPermissions = new Button
            {
                Name = "btnSelectAllPermissions",
                Text = "تحديد الكل",
                Width = 110,
                Height = 30,
                TabIndex = 1,
                UseVisualStyleBackColor = false
            };
            btnClearPermissions = new Button
            {
                Name = "btnClearPermissions",
                Text = "إلغاء تحديد الكل",
                Width = 130,
                Height = 30,
                TabIndex = 2,
                UseVisualStyleBackColor = false
            };

            btnApplyRolePreset = new Button
            {
                Name = "btnApplyRolePreset",
                Text = "تطبيق صلاحيات الدور",
                Width = 150,
                Height = 30,
                TabIndex = 3,
                UseVisualStyleBackColor = false
            };

            btnReloadPermissions = new Button
            {
                Name = "btnReloadPermissions",
                Text = "إعادة تحميل",
                Width = 120,
                Height = 30,
                TabIndex = 4,
                UseVisualStyleBackColor = false
            };

            btnCloseForm = new Button
            {
                Name = "btnCloseForm",
                Text = "إغلاق",
                Width = 100,
                Height = 30,
                TabIndex = 5,
                UseVisualStyleBackColor = false
            };

            btnSavePermissions.AccessibleName = "حفظ صلاحيات الشاشات للمستخدم المحدد";
            btnSelectAllPermissions.AccessibleName = "تحديد جميع صلاحيات الشاشات";
            btnClearPermissions.AccessibleName = "إلغاء تحديد جميع صلاحيات الشاشات";
            btnApplyRolePreset.AccessibleName = "تطبيق الصلاحيات الافتراضية لدور المستخدم";
            btnReloadPermissions.AccessibleName = "إعادة تحميل الصلاحيات من قاعدة البيانات";
            btnCloseForm.AccessibleName = "إغلاق شاشة المستخدمين والعودة إلى الرئيسية";

            Button[] permissionButtons =
            {
                btnSavePermissions, btnSelectAllPermissions, btnClearPermissions,
                btnApplyRolePreset, btnReloadPermissions, btnCloseForm
            };

            foreach (Button button in permissionButtons)
                button.Margin = new Padding(2);

            btnSavePermissions.Enabled = true;
            btnSavePermissions.Visible = true;
            btnSavePermissions.Click += btnSavePermissions_Click;

            btnSelectAllPermissions.Enabled = true;
            btnSelectAllPermissions.Visible = true;
            btnSelectAllPermissions.Click += btnSelectAllPermissions_Click;

            btnClearPermissions.Enabled = true;
            btnClearPermissions.Visible = true;
            btnClearPermissions.Click += btnClearPermissions_Click;

            btnApplyRolePreset.Enabled = true;
            btnApplyRolePreset.Visible = true;
            btnApplyRolePreset.Click += btnApplyRolePreset_Click;

            btnReloadPermissions.Enabled = true;
            btnReloadPermissions.Visible = true;
            btnReloadPermissions.Click += btnReloadPermissions_Click;

            btnCloseForm.Enabled = true;
            btnCloseForm.Visible = true;
            btnCloseForm.Click += btnCloseForm_Click;

            permissionActions.Controls.Add(btnSavePermissions);
            permissionActions.Controls.Add(btnSelectAllPermissions);
            permissionActions.Controls.Add(btnClearPermissions);
            permissionActions.Controls.Add(btnApplyRolePreset);
            permissionActions.Controls.Add(btnReloadPermissions);
            permissionActions.Controls.Add(btnCloseForm);

            lblPermissionHeader = new Label
            {
                Name = "lblPermissionHeader",
                Dock = DockStyle.Top,
                Height = 26,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new System.Drawing.Font(UIHelper.FontFamily, 9.5F, System.Drawing.FontStyle.Bold),
                ForeColor = UIHelper.PrimaryColor,
                Text = "صلاحيات المستخدم",
                Margin = new Padding(0, 0, 0, 2)
            };

            lblRoleScreens = new Label
            {
                Name = "lblRoleScreens",
                Dock = DockStyle.Bottom,
                Height = 40,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new System.Drawing.Font(UIHelper.FontFamily, 8.5F),
                ForeColor = UIHelper.MutedTextColor,
                Text = "",
                Margin = new Padding(0, 2, 0, 0)
            };

            // لا نضع العناصر مباشرة فوق بعضها داخل GroupBox؛ فهذا كان يؤدي إلى
            // اختفاء CheckedListBox عند تمدد شريط الأزرار. نستخدم تخطيطاً داخلياً
            // بصف مستقل للقائمة وصفوف ثابتة للعناوين والأزرار.
            permissionLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                RightToLeft = RightToLeft.Yes,
                Padding = new Padding(4, 2, 4, 2),
                Margin = new Padding(0),
                AutoScroll = true
            };
            permissionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            permissionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            permissionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            permissionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            permissionLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            checkedListPermissions.Dock = DockStyle.Fill;
            checkedListPermissions.Margin = new Padding(0, 2, 0, 2);
            checkedListPermissions.MinimumSize = new Size(0, 90);
            checkedListPermissions.IntegralHeight = false;

            lblPermissionHeader.Dock = DockStyle.Fill;
            lblRoleScreens.Dock = DockStyle.Fill;
            permissionActions.Dock = DockStyle.Fill;

            groupBoxPermissions.Controls.Clear();
            permissionLayout.Controls.Add(lblPermissionHeader, 0, 0);
            permissionLayout.Controls.Add(checkedListPermissions, 0, 1);
            permissionLayout.Controls.Add(lblRoleScreens, 0, 2);
            permissionLayout.Controls.Add(permissionActions, 0, 3);
            groupBoxPermissions.Controls.Add(permissionLayout);
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
                UIHelper.StylePrimaryButton(btnSavePermissions);
                UIHelper.StyleButton(btnSelectAllPermissions, UIHelper.AccentColor);
                UIHelper.StyleButton(btnClearPermissions, UIHelper.NeutralColor);
                UIHelper.StyleButton(btnApplyRolePreset, UIHelper.AccentColor);
                UIHelper.StyleButton(btnReloadPermissions, UIHelper.NeutralColor);
                UIHelper.StyleButton(btnCloseForm, UIHelper.DangerColor);
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
            // عند تغيير الدور تُطبَّق صلاحياته الافتراضية على القائمة فقط إذا لم يعدّل
            // المدير التحديدات يدوياً (سواء لحساب جديد أو قائم). أي تعديل يدوي يلغي
            // التطبيق التلقائي حتى لا تُمحى الصلاحيات المختارة بغير قصد.
            if (!isLoading && !permissionsUserModified)
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

        private async void btnSavePermissions_Click(object sender, EventArgs e)
        {
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

                    PermissionListItem item = checkedListPermissions.Items[i] as PermissionListItem;
                    if (item != null && !string.IsNullOrWhiteSpace(item.Key))
                        selectedKeys.Add(item.Key);
                }

                string userNameBeforeReload = txtUserName.Text.Trim();
                if (string.IsNullOrWhiteSpace(userNameBeforeReload))
                    userNameBeforeReload = selectedUserId.ToString();

                string effective = await Task.Run(() =>
                    userService.SaveUserPermissions(selectedUserId, selectedKeys));

                if (CurrentUser.IsLoggedIn && CurrentUser.User != null &&
                    CurrentUser.User.UserID == selectedUserId)
                    MainForm.Instance?.RefreshCurrentUserSession();

                await LoadUsersAsync();

                ShowInfo(string.Format(
                    "تم حفظ صلاحيات الشاشات بنجاح للمستخدم {0}. عدد الشاشات: {1}",
                    userNameBeforeReload,
                    PermissionKeys.GetScreenKeysFromPermissions(effective).Count));

                permissionsUserModified = false;
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حفظ الصلاحيات", ex);
            }
        }

        private async void btnReloadPermissions_Click(object sender, EventArgs e)
        {
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

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            MainForm.Instance?.ShowWelcomeScreen();
        }

        /// <summary>
        /// يُحدِّث رأس لوحة الصلاحيات (اسم المستخدم والدور).
        /// </summary>
        private void UpdatePermissionHeader(string userName, string roleName)
        {
            if (lblPermissionHeader == null)
                return;

            string displayName = string.IsNullOrWhiteSpace(userName) ? "مستخدم جديد" : userName;
            string role = string.IsNullOrWhiteSpace(roleName) ? "-" : roleName;
            lblPermissionHeader.Text = string.Format("صلاحيات المستخدم — المستخدم: {0} — الدور: {1}", displayName, role);
        }

        /// <summary>
        /// يُحدِّث تلميح الشاشات التي يوفرها الدور تلقائياً (قراءة فقط).
        /// </summary>
        private void UpdateRoleScreensLabel(string roleName)
        {
            if (lblRoleScreens == null)
                return;

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
                string screen = PermissionKeys.ToScreenPermission(permission.Trim());
                if (!string.IsNullOrWhiteSpace(screen) && !screens.Contains(screen, StringComparer.OrdinalIgnoreCase))
                    screens.Add(screen);
            }

            lblRoleScreens.Text = screens.Count == 0
                ? "صلاحيات الدور التلقائية: لا توجد (يُحدد يدوياً من القائمة)"
                : "صلاحيات الدور التلقائية: " + string.Join("، ",
                    screens.Select(key => PermissionKeys.GetDisplayName(key)));
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

                UpdateRoleScreensLabel(roleName);
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
            if (!CurrentUser.CanAccessModule("Users"))
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
            bool canManage = CurrentUser.CanAccessModule("Users");
            bool canView = canManage;
            bool canEditSelectedPermissions = canManage && !selectedSystemAdministrator;

            btnAdd.Enabled = canManage;
            btnUpdate.Enabled = canManage;
            btnDelete.Enabled = canManage && !selectedSystemAdministrator;
            cmbRole.Enabled = canEditSelectedPermissions;
            chkIsActive.Enabled = canEditSelectedPermissions;
            checkedListPermissions.Enabled = canEditSelectedPermissions;
            btnSelectAllPermissions.Enabled = canEditSelectedPermissions;
            btnClearPermissions.Enabled = canEditSelectedPermissions;
            btnApplyRolePreset.Enabled = canEditSelectedPermissions;
            btnSavePermissions.Enabled = canEditSelectedPermissions;
            btnReloadPermissions.Enabled = canManage;
            btnCloseForm.Enabled = true;

            if (selectedSystemAdministrator && lblPermissionHeader != null)
                lblPermissionHeader.Text = "صلاحيات المستخدم — مدير النظام (محمي — وصول كامل ثابت)";

            if (groupBoxPermissions != null)
                groupBoxPermissions.Enabled = canView;

            btnAdd.AccessibleDescription = canManage ? "إضافة مستخدم" : "لا توجد صلاحية إدارة المستخدمين";
            btnUpdate.AccessibleDescription = canManage ? "تعديل مستخدم" : "لا توجد صلاحية إدارة المستخدمين";
            btnDelete.AccessibleDescription = canManage ? "حذف مستخدم" : "لا توجد صلاحية إدارة المستخدمين";
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
            selectedSystemAdministrator = PermissionKeys.IsSystemAdministratorRole(roleName);
            if (selectedSystemAdministrator)
                permissions = PermissionKeys.Serialize(PermissionKeys.ScreenPermissions);

            SetPermissionsFromString(permissions);
            permissionsUserModified = false;

            UpdatePermissionHeader(txtUserName.Text.Trim(), roleName);
            UpdateRoleScreensLabel(roleName);
            ApplyPermissionUiState();
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
            selectedSystemAdministrator = false;

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

            UpdatePermissionHeader("", "");
            UpdateRoleScreensLabel(cmbRole.SelectedItem != null ? cmbRole.SelectedItem.ToString() : "");

            permissionsUserModified = false;
            ApplyPermissionUiState();
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
