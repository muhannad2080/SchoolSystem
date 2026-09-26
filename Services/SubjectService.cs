using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class SubjectService
    {
        // تعريف الدالة SubjectRepository لتنفيذ العملية المرتبطة بها.
        private readonly SubjectRepository repository = new SubjectRepository();
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService auditLogService = new AuditLogService();

        // تعريف الدالة GetAllSubjects لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllSubjects()
        {
            DemandSubjectLookupAccess();

            return repository.GetAllSubjects();
        }

        // تعريف الدالة GetActiveSubjects لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveSubjects()
        {
            DemandSubjectLookupAccess();

            return repository.GetActiveSubjects();
        }

        // تعريف الدالة GetActiveSubjectsByClass لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveSubjectsByClass(int classId)
        {
            DemandSubjectLookupAccess();
            if (classId <= 0)
                return repository.GetActiveSubjects();


            return repository.GetActiveSubjectsByClass(classId);
        }

        // تعريف الدالة UpdateSubject لتنفيذ العملية المرتبطة بها.
        public bool UpdateSubject(Subject subject)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Subjects", "Edit", "ليس لديك صلاحية تعديل المواد.");
            ValidateSubjectForUpdate(subject);
            bool updated = repository.UpdateSubject(subject);
            if (updated)
            {
                auditLogService.Record(
                    "تعديل مادة",
                    "Subject",
                    subject.SubjectID.ToString(),
                    "تم تعديل المادة رقم " + subject.SubjectID);
            }

            return updated;
        }

        // تعريف الدالة GetSubjectCount لتنفيذ العملية المرتبطة بها.
        public int GetSubjectCount()
        {
            DemandSubjectLookupAccess();

            return repository.GetSubjectCount();
        }

        // تعريف الدالة DemandSubjectLookupAccess لتنفيذ العملية المرتبطة بها.
        private void DemandSubjectLookupAccess()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات المواد.",
                "Subjects.View",
                "Subjects.Manage",
                "Grades.View",
                "Grades.Manage",
                "Timetable.View",
                "Timetable.Manage",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ReportsView,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.DashboardView);
        }

        // تعريف الدالة ValidateSubjectForUpdate لتنفيذ العملية المرتبطة بها.
        private void ValidateSubjectForUpdate(Subject subject)
        {
            if (subject == null)

                throw new ArgumentException("بيانات المادة غير صحيحة.");


            if (subject.SubjectID <= 0)

                throw new ArgumentException("اختر مادة صحيحة للتعديل.");


            if (subject.MaxDegree <= 0)

                throw new ArgumentException("الدرجة الكبرى يجب أن تكون أكبر من صفر.");


            if (subject.PassDegree < 0)

                throw new ArgumentException("درجة النجاح لا يمكن أن تكون أقل من صفر.");


            if (subject.PassDegree > subject.MaxDegree)

                throw new ArgumentException("درجة النجاح لا يمكن أن تكون أكبر من الدرجة الكبرى.");
        }
    }
}
