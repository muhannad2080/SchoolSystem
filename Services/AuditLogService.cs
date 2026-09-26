using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{
    // تعريف الصنف AuditLogService المسؤول عن وظائف طبقة Services.
    public class AuditLogService
    {
        // تعريف الدالة AuditLogRepository لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogRepository repository = new AuditLogRepository();

        // تعريف الدالة Record لتنفيذ العملية المرتبطة بها.
        public void Record(string actionName, string entityName, string entityId, string details)
        {
            Record(InferModule(entityName), actionName, entityName, entityId, details);
        }

        // تعريف الدالة Record لتنفيذ العملية المرتبطة بها.
        public void Record(string module, string actionName, string entityName, string entityId, string details)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                User user = CurrentUser.User;

                repository.Write(new AuditLog
                {
                    UserId = user == null ? (int?)null : user.UserID,
                    UserName = user == null ? "نظام" : (user.FullName ?? user.UserName),
                    Module = string.IsNullOrWhiteSpace(module) ? "System" : module.Trim(),
                    MachineName = Environment.MachineName,
                    IpAddress = ResolveLocalIp(),
                    ActionName = actionName,
                    EntityName = entityName,
                    EntityId = entityId,
                    Details = details
                });
            }

            catch (Exception ex)
            {
                Helpers.ApplicationLogger.LogException("AuditLog", ex);
            }
        }

        // تعريف الدالة InferModule لتنفيذ العملية المرتبطة بها.
        private static string InferModule(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName)) return "System";
            string value = entityName.Trim();
            if (value.Equals("Student", StringComparison.OrdinalIgnoreCase)) return "Students";
            if (value.Equals("Enrollment", StringComparison.OrdinalIgnoreCase)) return "Enrollment";
            if (value.Equals("Fee", StringComparison.OrdinalIgnoreCase)) return "Fees";
            if (value.Equals("Voucher", StringComparison.OrdinalIgnoreCase)) return "Vouchers";
            if (value.Equals("User", StringComparison.OrdinalIgnoreCase)) return "Users";
            return value;
        }

        // تعريف الدالة ResolveLocalIp لتنفيذ العملية المرتبطة بها.
        private static string ResolveLocalIp()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                return Dns.GetHostEntry(Dns.GetHostName()).AddressList
                    .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a))
                    ?.ToString() ?? string.Empty;
            }

            catch
            {

                return string.Empty;
            }
        }

        // تعريف الدالة GetRecent لتنفيذ العملية المرتبطة بها.
        public DataTable GetRecent(DateTime fromDate, DateTime toDate, string search)
        {

            return GetRecent(fromDate, toDate, search, string.Empty, string.Empty, string.Empty);
        }

        public DataTable GetRecent(DateTime fromDate, DateTime toDate, string search,
            string userName, string actionName, string entityName)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية عرض سجل التدقيق.");

            return repository.GetRecent(fromDate, toDate, search, userName, actionName, entityName);
        }

        // تعريف الدالة GetFilterValues لتنفيذ العملية المرتبطة بها.
        public DataTable GetFilterValues(string filterName)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية عرض سجل التدقيق.");

            return repository.GetFilterValues(filterName);
        }
    }
}
