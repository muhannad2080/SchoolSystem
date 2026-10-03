using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class SchoolSectionService
    {
        private readonly SchoolSectionRepository repository = new SchoolSectionRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllSections(string academicYear)
        {
            DemandViewAccess();
            ValidateAcademicYear(academicYear);
            return repository.GetAllSections(NormalizeYear(academicYear));
        }

        public SchoolSection GetSectionById(int sectionId)
        {
            DemandViewAccess();
            if (sectionId <= 0)
                throw new ArgumentException("رقم الشعبة غير صحيح.");
            return repository.GetSectionById(sectionId);
        }

        public int AddSection(SchoolSection item)
        {
            CurrentUser.DemandAction("Sections", "Add", "ليس لديك صلاحية إضافة الشعب الدراسية.");
            Validate(item, false);

            int id = repository.AddSection(item);
            auditLogService.Record("إضافة شعبة", "SchoolSection", id.ToString(),
                "تمت إضافة شعبة " + item.SectionName + " للصف رقم " + item.ClassID);
            return id;
        }

        public bool UpdateSection(SchoolSection item)
        {
            CurrentUser.DemandAction("Sections", "Edit", "ليس لديك صلاحية تعديل الشعب الدراسية.");
            Validate(item, true);

            SchoolSection existing = repository.GetSectionById(item.SectionID);
            if (existing == null)
                throw new InvalidOperationException("الشعبة غير موجودة.");

            bool updated = repository.UpdateSection(item);
            if (updated)
                auditLogService.Record("تعديل شعبة", "SchoolSection", item.SectionID.ToString(),
                    "تم تعديل شعبة " + item.SectionName + " للصف رقم " + item.ClassID);
            return updated;
        }

        public bool DeleteSection(int sectionId)
        {
            CurrentUser.DemandAction("Sections", "Delete", "ليس لديك صلاحية حذف الشعب الدراسية.");
            if (sectionId <= 0)
                throw new ArgumentException("اختر شعبة صحيحة للحذف.");

            bool deleted = repository.DeleteSection(sectionId);
            if (deleted)
                auditLogService.Record("حذف شعبة", "SchoolSection", sectionId.ToString(),
                    "تم حذف شعبة غير مرتبطة بسجلات تشغيلية.");
            return deleted;
        }

        private void DemandViewAccess()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض الشعب الدراسية.",
                "Sections.View",
                "Sections.Manage",
                "ClassAssignment.View",
                "ClassAssignment.Manage",
                "Timetable.View",
                "Timetable.Manage",
                "Attendance.View",
                "Attendance.Manage",
                "Grades.View",
                "Grades.Manage");
        }

        private void Validate(SchoolSection item, bool requireId)
        {
            if (item == null)
                throw new ArgumentException("بيانات الشعبة غير صحيحة.");

            if (requireId && item.SectionID <= 0)
                throw new ArgumentException("اختر شعبة صحيحة.");

            if (item.ClassID <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(item.SectionName) || item.SectionName.Trim().Length > 50)
                throw new ArgumentException("اسم الشعبة مطلوب ولا يمكن أن يتجاوز 50 حرفًا.");

            if (!string.IsNullOrWhiteSpace(item.SectionName))
            {
                foreach (char c in item.SectionName.Trim())
                {
                    if (char.IsControl(c))
                        throw new ArgumentException("اسم الشعبة يحتوي على محارف غير صالحة.");
                }
            }

            ValidateAcademicYear(item.AcademicYear);

            if (!item.Capacity.HasValue || item.Capacity.Value <= 0 || item.Capacity.Value > 500)
                throw new ArgumentException("سعة الشعبة يجب أن تكون بين 1 و500 طالب.");

            item.SectionName = item.SectionName.Trim();
            item.AcademicYear = NormalizeYear(item.AcademicYear);
            item.AllowedGender = NormalizeGender(item.AllowedGender);

            if (!item.IsActive && item.SectionID > 0)
            {
                // المنع الفعلي من التعطيل عند وجود طلاب يتم على مستوى المعاملة في المستودع.
            }
        }

        private static string NormalizeGender(string value)
        {
            string v = (value ?? string.Empty).Trim();
            if (v.Equals("ذكر", StringComparison.OrdinalIgnoreCase) ||
                v.Equals("ذكور", StringComparison.OrdinalIgnoreCase) ||
                v.Equals("Male", StringComparison.OrdinalIgnoreCase))
                return "ذكر";
            if (v.Equals("أنثى", StringComparison.OrdinalIgnoreCase) ||
                v.Equals("إناث", StringComparison.OrdinalIgnoreCase) ||
                v.Equals("Female", StringComparison.OrdinalIgnoreCase))
                return "أنثى";
            return "مختلط";
        }

        private static string NormalizeYear(string value)
        {
            return (value ?? string.Empty).Trim().Replace('-', '/');
        }

        private static void ValidateAcademicYear(string value)
        {
            string normalized = NormalizeYear(value);
            string[] parts = normalized.Split('/');
            int y1;
            int y2;
            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4 ||
                !int.TryParse(parts[0], out y1) || !int.TryParse(parts[1], out y2) || y2 != y1 + 1)
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027.");
        }
    }
}