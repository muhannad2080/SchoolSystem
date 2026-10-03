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

        // تعريف الدالة AddSubject لتنفيذ العملية المرتبطة بها.
        public int AddSubject(Subject subject)
        {
            CurrentUser.DemandAction("Subjects", "Add", "ليس لديك صلاحية إضافة المواد.");
            if (subject == null)
                throw new ArgumentException("بيانات المادة غير صحيحة.");

            // الكود يُولّد تلقائياً داخل المعاملة في المستودع، ولا يُطلب من المستخدم إدخاله.
            ValidateSubject(subject, false);

            int subjectId = repository.AddSubject(subject);
            auditLogService.Record(
                "إضافة مادة",
                "Subject",
                subjectId.ToString(),
                "إضافة المادة: " + subject.SubjectName);

            return subjectId;
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
        // تعريف الدالة DeleteSubject لتنفيذ العملية المرتبطة بها.
        public bool DeleteSubject(int subjectId)
        {
            CurrentUser.DemandAction("Subjects", "Delete", "ليس لديك صلاحية حذف المواد.");
            if (subjectId <= 0)
                throw new ArgumentException("رقم المادة غير صحيح.");

            bool deleted = repository.DeleteSubject(subjectId);
            if (deleted)
            {
                auditLogService.Record(
                    "حذف مادة",
                    "Subject",
                    subjectId.ToString(),
                    "حذف المادة من النظام.");
            }

            return deleted;
        }

        private void ValidateSubjectForUpdate(Subject subject)
        {
            ValidateSubject(subject, true);
        }

        private void ValidateSubject(Subject subject, bool requireId)
        {
            if (subject == null)
                throw new ArgumentException("بيانات المادة غير صحيحة.");

            if (requireId && subject.SubjectID <= 0)
                throw new ArgumentException("اختر مادة صحيحة.");

            if (requireId && (string.IsNullOrWhiteSpace(subject.SubjectCode) || subject.SubjectCode.Trim().Length < 2 || subject.SubjectCode.Trim().Length > 30))
                throw new ArgumentException("كود المادة غير صالح.");

            if (!requireId && !string.IsNullOrWhiteSpace(subject.SubjectCode) && subject.SubjectCode.Trim().Length > 30)
                throw new ArgumentException("كود المادة المولد لا يمكن أن يتجاوز 30 حرفاً.");

            if (!string.IsNullOrWhiteSpace(subject.SubjectCode))
            {
                foreach (char character in subject.SubjectCode.Trim())
                {
                    if (!(char.IsLetterOrDigit(character) || character == '_' || character == '-'))
                        throw new ArgumentException("كود المادة يجب أن يحتوي على حروف أو أرقام أو (_) أو (-) فقط.");
                }
            }

            if (string.IsNullOrWhiteSpace(subject.SubjectName) || subject.SubjectName.Trim().Length < 2 || subject.SubjectName.Trim().Length > 100)
                throw new ArgumentException("اسم المادة يجب أن يكون بين حرفين و100 حرف.");

            if (subject.ClassID.HasValue && subject.ClassID.Value <= 0)
                subject.ClassID = null;

            if (subject.MaxDegree <= 0)
                throw new ArgumentException("الدرجة الكبرى يجب أن تكون أكبر من صفر.");

            if (subject.MaxDegree > 1000)
                throw new ArgumentException("الدرجة الكبرى لا يمكن أن تتجاوز 1000.");

            if (subject.PassDegree < 0)
                throw new ArgumentException("درجة النجاح لا يمكن أن تكون أقل من صفر.");

            if (subject.PassDegree > subject.MaxDegree)
                throw new ArgumentException("درجة النجاح لا يمكن أن تكون أكبر من الدرجة الكبرى.");

            if (subject.Notes != null && subject.Notes.Trim().Length > 1000)
                throw new ArgumentException("الملاحظات لا يمكن أن تتجاوز 1000 حرف.");

            subject.SubjectCode = string.IsNullOrWhiteSpace(subject.SubjectCode) ? null : subject.SubjectCode.Trim();
            subject.SubjectName = subject.SubjectName.Trim();
            subject.Notes = string.IsNullOrWhiteSpace(subject.Notes) ? null : subject.Notes.Trim();
        }
    }
}
