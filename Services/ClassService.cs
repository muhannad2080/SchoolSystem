using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class ClassService
    {
        // تعريف الدالة ClassRepository لتنفيذ العملية المرتبطة بها.
        private readonly ClassRepository repository = new ClassRepository();
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService auditLogService = new AuditLogService();

        // تعريف الدالة GetAllClasses لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllClasses()
        {
            DemandClassLookupAccess();

            return repository.GetAllClasses();
        }

        // تعريف الدالة GetActiveStages لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveStages()
        {
            DemandClassLookupAccess();

            return repository.GetActiveStages();
        }

        // تعريف الدالة GetClassDetails لتنفيذ العملية المرتبطة بها.
        public DataTable GetClassDetails()
        {
            DemandClassLookupAccess();

            return repository.GetClassDetails();
        }

        // تعريف الدالة AddClass لتنفيذ العملية المرتبطة بها.
        public int AddClass(SchoolClass item)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Classes", "Add", "ليس لديك صلاحية إضافة الفصول.");
            ValidateClass(item, false);

            int classId = repository.AddClass(item);
            auditLogService.Record(
                "إضافة",
                "Class",
                classId.ToString(),
                string.Format("إضافة الفصل: {0}، المرحلة: {1}", item.ClassName, item.StageName));

            return classId;
        }

        // تعريف الدالة UpdateClass لتنفيذ العملية المرتبطة بها.
        public bool UpdateClass(SchoolClass item)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Classes", "Edit", "ليس لديك صلاحية تعديل الفصول.");
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
        // تعريف الدالة ValidateClass لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة DemandClassLookupAccess لتنفيذ العملية المرتبطة بها.
        private void DemandClassLookupAccess()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات الفصول.",
                "Classes.View",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ClassesManage,
                "ClassAssignment.View",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ClassAssignmentManage,
                "Enrollment.View",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.EnrollmentManage,
                "Attendance.View",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.AttendanceManage,
                "Subjects.View",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.SubjectsManage,
                "Timetable.View",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.TimetableManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ReportsView,
                "Reports.View",
                "Fees.View",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.FeesManage,
                "Grades.View",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.GradesManage);
        }
    }
}
