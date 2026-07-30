using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class SubjectService
    {
        private readonly SubjectRepository repository = new SubjectRepository();

        public DataTable GetAllSubjects()
        {
            return repository.GetAllSubjects();
        }

        public DataTable GetActiveSubjects()
        {
            return repository.GetActiveSubjects();
        }

        public DataTable GetActiveSubjectsByClass(int classId)
        {
            if (classId <= 0)
                return repository.GetActiveSubjects();

            return repository.GetActiveSubjectsByClass(classId);
        }

        public bool UpdateSubject(Subject subject)
        {
            ValidateSubjectForUpdate(subject);
            return repository.UpdateSubject(subject);
        }

        public int GetSubjectCount()
        {
            return repository.GetSubjectCount();
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
