using System;
using System.Windows.Forms;
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
                default:
                    allowed = true;
                    break;
            }

            button.Enabled = allowed;
            button.AccessibleDescription = allowed
                ? button.AccessibleDescription
                : "لا توجد الصلاحية المطلوبة لهذا الإجراء";
        }

        private static string GetAction(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            if (name.Equals("btnSavePermissions", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnSelectAllPermissions", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnClearPermissions", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnApplyRolePreset", StringComparison.OrdinalIgnoreCase))
                return "Edit";

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

            if (name.Equals("btnAdd", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnAddBook", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnAddBus", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnAddRoute", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnClassAdd", StringComparison.OrdinalIgnoreCase))
                return "Add";

            if (name.Equals("btnUpdate", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnUpdateBus", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnUpdateRoute", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnUpdateAssignment", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnSaveAll", StringComparison.OrdinalIgnoreCase))
                return "Edit";

            if (name.Equals("btnDelete", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnDeleteBook", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnDeleteBus", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnDeleteRoute", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("btnDeleteAssignment", StringComparison.OrdinalIgnoreCase))
                return "Delete";

            if (name.Equals("btnSearch", StringComparison.OrdinalIgnoreCase))
                return "Search";

            return string.Empty;
        }
    }
}
