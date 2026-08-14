using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class MarkService
    {
        private readonly MarkRepository repository = new MarkRepository();

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
        }

        public bool UpdateMark(Mark mark)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            return repository.UpdateMark(mark);
        }

        public bool DeleteMark(int markId)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            return repository.DeleteMark(markId);
        }
    }
}