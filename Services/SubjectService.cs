using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class SubjectService
    {
        private readonly SubjectRepository repository = new SubjectRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllSubjects()
        {
            DemandSubjectLookupAccess();
            return repository.GetAllSubjects();
        }

        public DataTable GetActiveSubjects()
        {
            DemandSubjectLookupAccess();
            return repository.GetActiveSubjects();
        }

        public DataTable GetActiveSubjectsByClass(int classId)
        {
            DemandSubjectLookupAccess();
            if (classId <= 0)
                return repository.GetActiveSubjects();

            return repository.GetActiveSubjectsByClass(classId);
        }

        public bool UpdateSubject(Subject subject)
        {
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

        public int GetSubjectCount()
        {
            DemandSubjectLookupAccess();
            return repository.GetSubjectCount();
        }

        private void DemandSubjectLookupAccess()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات المواد.",
                "Subjects.View",
                "Subjects.Manage",
                "Grades.View",
                "Grades.Manage",
                "Timetable.View",
                "Timetable.Manage",
                PermissionKeys.ReportsView,
                PermissionKeys.DashboardView);
        }

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
