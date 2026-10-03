using System;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class StudentTransportService
    {
        private readonly StudentTransportRepository repository = new StudentTransportRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAll()
        {
            CurrentUser.DemandAction("Transport", "View", "ليس لديك صلاحية عرض توزيع الطلاب على النقل.");
            return repository.GetAll();
        }

        public DataTable GetActiveStudents()
        {
            CurrentUser.DemandAction("Transport", "View", "ليس لديك صلاحية عرض الطلاب للنقل.");
            return repository.GetActiveStudents();
        }

        public DataTable GetRoutes()
        {
            CurrentUser.DemandAction("Transport", "View", "ليس لديك صلاحية عرض مسارات النقل.");
            return repository.GetRoutes();
        }

        public bool Add(StudentTransportAssignment assignment)
        {
            CurrentUser.DemandAction("Transport", "Add", "ليس لديك صلاحية إضافة توزيع الطلاب على النقل.");
            Validate(assignment, false);
            bool added = repository.Add(assignment);
            if (added)
                auditLogService.Record("إنشاء", "StudentTransport", assignment.AssignmentID.ToString(),
                    "ربط الطالب " + assignment.StudentID + " بمسار النقل " + assignment.RouteID);
            return added;
        }

        public bool Update(StudentTransportAssignment assignment)
        {
            CurrentUser.DemandAction("Transport", "Edit", "ليس لديك صلاحية تعديل توزيع الطلاب على النقل.");
            Validate(assignment, true);
            bool updated = repository.Update(assignment);
            if (updated)
                auditLogService.Record("تعديل", "StudentTransport", assignment.AssignmentID.ToString(),
                    "تعديل توزيع الطالب " + assignment.StudentID + " على النقل.");
            return updated;
        }

        public bool Delete(int assignmentId)
        {
            CurrentUser.DemandAction("Transport", "Delete", "ليس لديك صلاحية حذف توزيع الطلاب على النقل.");
            if (assignmentId <= 0)
                throw new ArgumentException("رقم توزيع النقل غير صحيح.");

            bool deleted = repository.Delete(assignmentId);
            if (deleted)
                auditLogService.Record("حذف", "StudentTransport", assignmentId.ToString(),
                    "إلغاء توزيع طالب من النقل.");
            return deleted;
        }

        private void Validate(StudentTransportAssignment assignment, bool isUpdate)
        {
            if (assignment == null)
                throw new ArgumentException("بيانات توزيع النقل غير موجودة.");

            if (isUpdate && assignment.AssignmentID <= 0)
                throw new ArgumentException("اختر توزيعًا صحيحًا للتعديل.");

            if (assignment.StudentID <= 0)
                throw new ArgumentException("يجب اختيار الطالب.");

            if (assignment.RouteID <= 0)
                throw new ArgumentException("يجب اختيار مسار النقل.");

            assignment.AcademicYear = NormalizeYear(assignment.AcademicYear);

            if (assignment.PickupPoint != null)
                assignment.PickupPoint = assignment.PickupPoint.Trim();
            if (assignment.DropoffPoint != null)
                assignment.DropoffPoint = assignment.DropoffPoint.Trim();
            if (assignment.Notes != null)
                assignment.Notes = assignment.Notes.Trim();

            if (assignment.PickupPoint == null || assignment.PickupPoint.Length == 0)
                throw new ArgumentException("نقطة صعود الطالب مطلوبة.");
            if (assignment.DropoffPoint == null || assignment.DropoffPoint.Length == 0)
                throw new ArgumentException("نقطة نزول الطالب مطلوبة.");

            if (assignment.PickupPoint.Length > 200 || assignment.DropoffPoint.Length > 200)
                throw new ArgumentException("نقاط الصعود والنزول لا يمكن أن تتجاوز 200 حرف.");
            if (assignment.Notes != null && assignment.Notes.Length > 1000)
                throw new ArgumentException("ملاحظات النقل لا يمكن أن تتجاوز 1000 حرف.");

            if (assignment.StartDate == DateTime.MinValue)
                assignment.StartDate = DateTime.Today;
            if (assignment.StartDate.Date > DateTime.Today && assignment.IsActive)
                throw new ArgumentException("تاريخ بدء النقل لا يمكن أن يكون في المستقبل عند تفعيل الاشتراك.");

            if (assignment.EndDate.HasValue && assignment.EndDate.Value.Date < assignment.StartDate.Date)
                throw new ArgumentException("تاريخ انتهاء النقل لا يمكن أن يسبق تاريخ البداية.");
        }

        private static string NormalizeYear(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("العام الدراسي مطلوب.");

            string normalized = value.Trim().Replace('-', '/');
            Match match = Regex.Match(normalized, "^(\\d{4})/(\\d{4})$");
            if (!match.Success)
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");

            int first = int.Parse(match.Groups[1].Value);
            int second = int.Parse(match.Groups[2].Value);
            if (second != first + 1)
                throw new ArgumentException("العام الدراسي يجب أن يكون متسلسلاً.");

            return normalized;
        }
    }
}