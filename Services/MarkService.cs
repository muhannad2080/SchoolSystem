using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class MarkService
    {
        private readonly MarkRepository repository = new MarkRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllMarks()
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            return repository.GetAllMarks();
        }

        public bool MarkExists(int studentId, int subjectId, string examType, int excludeId = 0)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            return repository.MarkExists(studentId, subjectId, examType, excludeId);
        }

        public void AddMark(Mark mark)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            repository.AddMark(mark);
            auditLogService.Record("إنشاء", "Mark", mark.MarkID.ToString(),
                string.Format("الطالب: {0}، المادة: {1}، الدرجة: {2}، الاختبار: {3}", mark.StudentID, mark.SubjectID, mark.MarkValue, mark.ExamType));
        }

        public bool UpdateMark(Mark mark)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            bool updated = repository.UpdateMark(mark);
            if (updated)
                auditLogService.Record("تعديل", "Mark", mark.MarkID.ToString(),
                    string.Format("الطالب: {0}، المادة: {1}، الدرجة: {2}، الاختبار: {3}", mark.StudentID, mark.SubjectID, mark.MarkValue, mark.ExamType));
            return updated;
        }

        public bool DeleteMark(int markId)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            bool deleted = repository.DeleteMark(markId);
            if (deleted)
                auditLogService.Record("حذف", "Mark", markId.ToString(), "حذف درجة الطالب.");
            return deleted;
        }
    }
}