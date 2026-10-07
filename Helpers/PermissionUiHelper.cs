using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Helpers
{
    /// <summary>
    /// طبقة موحدة لمواءمة حالة أزرار الشاشة مع الصلاحيات الفعلية للمستخدم.
    /// لا تمنح أي صلاحية؛ هي تعكس فقط قرار CurrentUser على مستوى الواجهة،
    /// بينما تبقى الخدمة/المستودع مسؤولين عن فرض الأمان فعلياً.
    /// </summary>
    public static class PermissionUiHelper
    {
        private sealed class PermissionGuardState
        {
            public string Module { get; set; }
            public string Action { get; set; }
            public string Message { get; set; }
        }

        private static readonly ConditionalWeakTable<Button, ToolTip> PermissionToolTips =
            new ConditionalWeakTable<Button, ToolTip>();

        private static readonly ConditionalWeakTable<Button, PermissionGuardState> PermissionGuardStates =
            new ConditionalWeakTable<Button, PermissionGuardState>();
        public static void Apply(Control root, string module)
        {
            if (root == null || string.IsNullOrWhiteSpace(module))
                return;

            ApplyRecursive(root, module.Trim());
        }

        private static void ApplyRecursive(Control parent, string module)
        {
            foreach (Control control in parent.Controls)
            {
                Button button = control as Button;
                if (button != null)
                {
                    string action = GetAction(button.Name);
                    if (!string.IsNullOrWhiteSpace(action))
                        ApplyButton(button, module, action);
                }

                if (control.HasChildren)
                    ApplyRecursive(control, module);
            }
        }

        private static void ApplyButton(Button button, string module, string action)
        {
            // لا نفعّل زرًا كان التطبيق قد عطّله بسبب عدم وجود اختيار/حالة نموذج.
            if (!button.Enabled)
                return;

            bool allowed;
            switch (action)
            {
                case "View":
                    allowed = CurrentUser.CanView(module);
                    break;
                case "Add":
                    allowed = CurrentUser.CanAdd(module);
                    break;
                case "Edit":
                    allowed = CurrentUser.CanEdit(module);
                    break;
                case "Delete":
                    allowed = CurrentUser.CanDelete(module);
                    break;
                case "Search":
                    allowed = CurrentUser.CanSearch(module);
                    break;
                case "Print":
                    allowed = CurrentUser.CanPrint(module);
                    break;
                case "Export":
                    allowed = CurrentUser.CanExport(module);
                    break;
                case "Approve":
                    allowed = CurrentUser.CanApprove(module);
                    break;
                case "PermissionManage":
                    allowed = CurrentUser.IsAdmin()
                        || CurrentUser.HasAny(
                            PermissionKeys.PermissionsManage,
                            PermissionKeys.UsersManageRoles,
                            PermissionKeys.UsersManage);
                    break;
                default:
                    allowed = true;
                    break;
            }

            if (allowed)
            {
                button.Enabled = true;
                button.Cursor = Cursors.Default;
                button.AccessibleDescription = button.AccessibleDescription;

                ToolTip existingTip;
                if (PermissionToolTips.TryGetValue(button, out existingTip))
                    existingTip.SetToolTip(button, string.Empty);

                return;
            }

            string permissionName = GetPermissionName(module, action);
            string message =
                "هذا الإجراء غير متاح لهذا الحساب." +
                Environment.NewLine +
                "الصلاحية المطلوبة: " + permissionName;

            button.Enabled = true;
            button.Cursor = Cursors.No;
            button.AccessibleDescription = message;

            ToolTip tip;
            if (!PermissionToolTips.TryGetValue(button, out tip))
            {
                tip = new ToolTip
                {
                    AutoPopDelay = 8000,
                    InitialDelay = 250,
                    ReshowDelay = 100,
                    ShowAlways = true
                };
                PermissionToolTips.Add(button, tip);
            }

            tip.SetToolTip(button, message);

            PermissionGuardState existingGuardState;
            if (!PermissionGuardStates.TryGetValue(button, out existingGuardState))
            {
                PermissionGuardStates.Add(button, new PermissionGuardState
                {
                    Module = module.Trim(),
                    Action = action,
                    Message = message
                });

                button.MouseDown += UnauthorizedButton_MouseDown;
            }
        }

        private static void UnauthorizedButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            Button button = sender as Button;
            if (button == null)
                return;

            PermissionGuardState state;
            if (!PermissionGuardStates.TryGetValue(button, out state))
                return;

            // تحقق مرة أخرى قبل إظهار المنع حتى لا يبقى القرار مبنيًا على حالة قديمة.
            bool allowed;
            switch (state.Action)
            {
                case "View":
                    allowed = CurrentUser.CanView(state.Module);
                    break;
                case "Add":
                    allowed = CurrentUser.CanAdd(state.Module);
                    break;
                case "Edit":
                    allowed = CurrentUser.CanEdit(state.Module);
                    break;
                case "Delete":
                    allowed = CurrentUser.CanDelete(state.Module);
                    break;
                case "Search":
                    allowed = CurrentUser.CanSearch(state.Module);
                    break;
                case "Print":
                    allowed = CurrentUser.CanPrint(state.Module);
                    break;
                case "Export":
                    allowed = CurrentUser.CanExport(state.Module);
                    break;
                case "Approve":
                    allowed = CurrentUser.CanApprove(state.Module);
                    break;
                case "PermissionManage":
                    allowed = CurrentUser.IsAdmin()
                        || CurrentUser.HasAny(
                            PermissionKeys.PermissionsManage,
                            PermissionKeys.UsersManageRoles,
                            PermissionKeys.UsersManage);
                    break;
                default:
                    allowed = true;
                    break;
            }

            if (allowed)
            {
                button.Cursor = Cursors.Default;
                return;
            }

            UIHelper.ShowWarning(state.Message);

            // تعطيل الزر بعد إظهار الرسالة يمنع حدث Click الأصلي من تنفيذ الإجراء.
            button.Enabled = false;
            button.Cursor = Cursors.No;
            button.Refresh();
        }

        private static string GetPermissionName(string module, string action)
        {
            if (string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(action))
                return "الصلاحية المناسبة للوحدة";

            if (action.Equals("Export", StringComparison.OrdinalIgnoreCase))
                return "تصدير البيانات";

            if (action.Equals("PermissionManage", StringComparison.OrdinalIgnoreCase))
                return "إدارة صلاحيات المستخدمين";

            string key = module.Trim() + "." + action.Trim();
            string display = PermissionKeys.GetDisplayName(key);

            return string.IsNullOrWhiteSpace(display)
                ? action.Trim()
                : display;
        }

        private static string GetAction(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            if (name.Equals("btnSavePermissions", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnSelectAllPermissions", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnClearPermissions", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnApplyRolePreset", StringComparison.OrdinalIgnoreCase))
                return "PermissionManage";

            if (name.Equals("btnReloadPermissions", StringComparison.OrdinalIgnoreCase))
                return "View";

            if (name.Equals("btnMarkAllPresent", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnMarkAllAbsent", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnMarkAllLate", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnAssign", StringComparison.OrdinalIgnoreCase))
                return "Edit";

            if (name.Equals("btnRemove", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnRemoveAll", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnDeleteGrade", StringComparison.OrdinalIgnoreCase))
                return "Delete";

            if (name.Equals("btnTransfer", StringComparison.OrdinalIgnoreCase))
                return "Edit";

            if (name.IndexOf("Export", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Export";

            if (name.IndexOf("Print", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Print";

            if (name.Equals("btnApproveMigration", StringComparison.OrdinalIgnoreCase))
                return "Approve";

            if (name.Equals("btnGenerateFees", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnRecordPayment", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnNewReceipt", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnNewPayment", StringComparison.OrdinalIgnoreCase))
                return "Add";

            if (name.StartsWith("btnAdd", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("btnNew", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("btnCreate", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnGenerateFees", StringComparison.OrdinalIgnoreCase))
                return "Add";

            if (name.StartsWith("btnUpdate", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("btnEdit", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("btnSave", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnTransfer", StringComparison.OrdinalIgnoreCase))
                return "Edit";

            if (name.StartsWith("btnDelete", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("btnRemove", StringComparison.OrdinalIgnoreCase))
                return "Delete";

            if (name.Equals("btnSearch", StringComparison.OrdinalIgnoreCase))
                return "Search";

            return string.Empty;
        }
    }
}
