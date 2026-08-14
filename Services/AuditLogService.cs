using System;
using System.Data;
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
            try
            {
                User user = CurrentUser.User;
                repository.Write(new AuditLog
                {
                    UserId = user == null ? (int?)null : user.UserID,
                    UserName = user == null ? "نظام" : (user.FullName ?? user.UserName),
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

        public DataTable GetRecent(DateTime fromDate, DateTime toDate, string search)
        {
            CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية عرض سجل التدقيق.");
            return repository.GetRecent(fromDate, toDate, search);
        }
    }
}
