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
    public class AuditLogService
    {
        private readonly AuditLogRepository repository = new AuditLogRepository();

        public void Record(string actionName, string entityName, string entityId, string details)
        {
            Record(InferModule(entityName), actionName, entityName, entityId, details);
        }

        public void Record(string module, string actionName, string entityName, string entityId, string details)
        {
            try
            {
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

        private static string ResolveLocalIp()
        {
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

        public DataTable GetRecent(DateTime fromDate, DateTime toDate, string search)
        {
            return GetRecent(fromDate, toDate, search, string.Empty, string.Empty, string.Empty);
        }

        public DataTable GetRecent(DateTime fromDate, DateTime toDate, string search,
            string userName, string actionName, string entityName)
        {
            CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية عرض سجل التدقيق.");
            return repository.GetRecent(fromDate, toDate, search, userName, actionName, entityName);
        }

        public DataTable GetFilterValues(string filterName)
        {
            CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية عرض سجل التدقيق.");
            return repository.GetFilterValues(filterName);
        }
    }
}
