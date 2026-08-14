using System;

namespace SchoolSystem.Models
{
    public class AuditLog
    {
        public long AuditLogId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string ActionName { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
