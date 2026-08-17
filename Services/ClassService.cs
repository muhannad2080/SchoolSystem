using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class ClassService
    {
        private readonly ClassRepository repository = new ClassRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllClasses()
        {
            DemandClassLookupAccess();
            return repository.GetAllClasses();
        }

        public DataTable GetClassDetails()
        {
            DemandClassLookupAccess();
            return repository.GetClassDetails();
        }

        public int AddClass(SchoolClass item)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassesManage, "ليس لديك صلاحية إدارة الفصول.");
            ValidateClass(item, false);

            int classId = repository.AddClass(item);
            auditLogService.Record(
                "إضافة",
                "Class",
                classId.ToString(),
                string.Format("إضافة الفصل: {0}، المرحلة: {1}", item.ClassName, item.StageName));
            return classId;
        }

        public bool UpdateClass(SchoolClass item)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassesManage, "ليس لديك صلاحية إدارة الفصول.");
            ValidateClass(item, true);

            bool updated = repository.UpdateClass(item);
            if (updated)
            {
                auditLogService.Record(
                    "تعديل",
                    "Class",
                    item.ClassID.ToString(),
                    string.Format("تعديل الفصل: {0}، المرحلة: {1}، نشط: {2}", item.ClassName, item.StageName, item.IsActive));
            }

            return updated;
        }
        private void ValidateClass(SchoolClass item, bool requireId)
        {
            if (item == null)
                throw new ArgumentException("بيانات الفصل غير صحيحة.");
            if (requireId && item.ClassID <= 0)
                throw new ArgumentException("اختر فصلًا صحيحًا.");
            if (string.IsNullOrWhiteSpace(item.ClassName))
                throw new ArgumentException("اسم الفصل مطلوب.");
            if (string.IsNullOrWhiteSpace(item.StageName))
                throw new ArgumentException("اسم المرحلة مطلوب.");
            if (item.GradeOrder <= 0)
                throw new ArgumentException("ترتيب الفصل غير صحيح.");
        }

        private void DemandClassLookupAccess()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات الفصول.",
                PermissionKeys.ClassesManage,
                PermissionKeys.ClassAssignmentManage,
                PermissionKeys.EnrollmentManage,
                PermissionKeys.AttendanceManage,
                PermissionKeys.SubjectsManage,
                PermissionKeys.TimetableManage,
                PermissionKeys.ReportsView,
                PermissionKeys.FeesManage,
                PermissionKeys.GradesManage);
        }
    }
}
