using System;
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
            CurrentUser.DemandAction("Grades", "View", "ليس لديك صلاحية عرض الدرجات.");
            return repository.GetAllMarks();
        }

        public bool MarkExists(int studentId, int subjectId, string examType, int excludeId = 0)
        {
            CurrentUser.DemandAction("Grades", "View", "ليس لديك صلاحية التحقق من الدرجات.");

            if (studentId <= 0)
                throw new ArgumentException("بيانات الطالب غير صحيحة.");

            if (subjectId <= 0)
                throw new ArgumentException("بيانات المادة غير صحيحة.");

            if (string.IsNullOrWhiteSpace(examType))
                throw new ArgumentException("نوع الاختبار مطلوب.");

            return repository.MarkExists(studentId, subjectId, examType.Trim(), excludeId);
        }

        public void AddMark(Mark mark)
        {
            ValidateMark(mark);

            bool exists = repository.MarkExists(
                mark.StudentID,
                mark.SubjectID,
                mark.ExamType,
                0);

            if (exists)
                throw new InvalidOperationException(
                    "توجد درجة مسجلة مسبقاً لنفس الطالب والمادة ونوع الاختبار.");

            CurrentUser.DemandAction("Grades", "Add", "ليس لديك صلاحية إضافة الدرجات.");

            repository.AddMark(mark);

            auditLogService.Record(
                "إنشاء",
                "Mark",
                mark.MarkID.ToString(),
                string.Format(
                    "الطالب: {0}، المادة: {1}، الدرجة: {2}، الاختبار: {3}",
                    mark.StudentID,
                    mark.SubjectID,
                    mark.MarkValue,
                    mark.ExamType));
        }

        public bool UpdateMark(Mark mark)
        {
            ValidateMark(mark);

            if (mark.MarkID <= 0)
                throw new ArgumentException("رقم الدرجة غير صحيح.");

            if (repository.MarkExists(
                    mark.StudentID,
                    mark.SubjectID,
                    mark.ExamType,
                    mark.MarkID))
            {
                throw new InvalidOperationException(
                    "لا يمكن التعديل؛ توجد درجة أخرى لنفس الطالب والمادة ونوع الاختبار.");
            }

            CurrentUser.DemandAction("Grades", "Edit", "ليس لديك صلاحية تعديل الدرجات.");

            bool updated = repository.UpdateMark(mark);
            if (updated)
                auditLogService.Record(
                    "تعديل",
                    "Mark",
                    mark.MarkID.ToString(),
                    string.Format(
                        "الطالب: {0}، المادة: {1}، الدرجة: {2}، الاختبار: {3}",
                        mark.StudentID,
                        mark.SubjectID,
                        mark.MarkValue,
                        mark.ExamType));

            return updated;
        }

        public bool DeleteMark(int markId)
        {
            CurrentUser.DemandAction("Grades", "Delete", "ليس لديك صلاحية حذف الدرجات.");

            if (markId <= 0)
                throw new ArgumentException("رقم الدرجة غير صحيح.");

            bool deleted = repository.DeleteMark(markId);
            if (deleted)
                auditLogService.Record(
                    "حذف",
                    "Mark",
                    markId.ToString(),
                    "حذف درجة الطالب.");

            return deleted;
        }

        private void ValidateMark(Mark mark)
        {
            if (mark == null)
                throw new ArgumentException("بيانات الدرجة غير صحيحة.");

            if (mark.StudentID <= 0)
                throw new ArgumentException("بيانات الطالب غير صحيحة.");

            if (mark.SubjectID <= 0)
                throw new ArgumentException("بيانات المادة غير صحيحة.");

            if (mark.MarkValue < 0 || mark.MarkValue > 100)
                throw new ArgumentException("الدرجة يجب أن تكون بين 0 و100.");

            mark.ExamType = (mark.ExamType ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(mark.ExamType))
                throw new ArgumentException("نوع الاختبار مطلوب.");

            if (mark.ExamType.Length > 50)
                throw new ArgumentException("نوع الاختبار لا يمكن أن يتجاوز 50 حرفاً.");
        }
    }
}
