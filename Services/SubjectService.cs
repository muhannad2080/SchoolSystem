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
            CurrentUser.DemandPermission(PermissionKeys.SubjectsManage, "ليس لديك صلاحية إدارة المواد.");
            ValidateSubjectForUpdate(subject);
            return repository.UpdateSubject(subject);
        }

        public int GetSubjectCount()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات المواد.",
                PermissionKeys.SubjectsManage,
                PermissionKeys.GradesManage,
                PermissionKeys.TimetableManage,
                PermissionKeys.ReportsView,
                PermissionKeys.DashboardView);
            return repository.GetSubjectCount();
        }

        private void DemandSubjectLookupAccess()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات المواد.",
                PermissionKeys.SubjectsManage,
                PermissionKeys.GradesManage,
                PermissionKeys.TimetableManage,
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
