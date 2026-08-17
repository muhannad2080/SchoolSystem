using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class TimetableService
    {
        private readonly TimetableRepository repository = new TimetableRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetTeachers()
        {
            CurrentUser.DemandAction("Timetable", "View", "ليس لديك صلاحية عرض بيانات الجدول.");
            return repository.GetTeachers();
        }

        public DataTable GetSections(int classId, string academicYear)
        {
            CurrentUser.DemandAction("Timetable", "View", "ليس لديك صلاحية عرض شعب الجدول.");
            if (classId <= 0)
                return new DataTable();

            ValidateAcademicYear(academicYear);
            return repository.GetSections(classId, academicYear.Trim());
        }

        public DataTable GetSubjectsByClass(int classId)
        {
            CurrentUser.DemandAction("Timetable", "View", "ليس لديك صلاحية عرض مواد الجدول.");
            if (classId <= 0)
                return new DataTable();

            return repository.GetSubjectsByClass(classId);
        }

        public DataTable GetAllTimetable()
        {
            CurrentUser.DemandAction("Timetable", "View", "ليس لديك صلاحية عرض الجدول الدراسي.");
            return repository.GetAllTimetable();
        }

        public bool AddTimetable(TimetableEntry item)
        {
            CurrentUser.DemandAction("Timetable", "Add", "ليس لديك صلاحية إضافة حصص الجدول.");
            Validate(item);

            if (item.IsActive)
            {
                if (repository.HasClassConflict(item))
                    throw new ArgumentException("يوجد تعارض: هذا الصف والشعبة لديهم حصة في نفس الوقت.");

                if (repository.HasTeacherConflict(item))
                    throw new ArgumentException("يوجد تعارض: المعلم لديه حصة أخرى في نفس الوقت.");

                if (repository.HasRoomConflict(item))
                    throw new ArgumentException("يوجد تعارض: القاعة مستخدمة في نفس الوقت.");
            }

            bool added = repository.AddTimetable(item);
            if (added)
                auditLogService.Record("إنشاء", "Timetable", item.TimetableID.ToString(),
                    "إضافة حصة للصف رقم " + item.ClassID + " والمادة رقم " + item.SubjectID);
            return added;
        }

        public bool UpdateTimetable(TimetableEntry item)
        {
            CurrentUser.DemandAction("Timetable", "Edit", "ليس لديك صلاحية تعديل حصص الجدول.");
            Validate(item);

            if (item.TimetableID <= 0)
                throw new ArgumentException("اختر حصة صحيحة للتعديل.");

            if (item.IsActive)
            {
                if (repository.HasClassConflict(item))
                    throw new ArgumentException("يوجد تعارض: هذا الصف والشعبة لديهم حصة في نفس الوقت.");

                if (repository.HasTeacherConflict(item))
                    throw new ArgumentException("يوجد تعارض: المعلم لديه حصة أخرى في نفس الوقت.");

                if (repository.HasRoomConflict(item))
                    throw new ArgumentException("يوجد تعارض: القاعة مستخدمة في نفس الوقت.");
            }

            bool updated = repository.UpdateTimetable(item);
            if (updated)
                auditLogService.Record("تعديل", "Timetable", item.TimetableID.ToString(),
                    "تعديل حصة للصف رقم " + item.ClassID + " والمادة رقم " + item.SubjectID);
            return updated;
        }

        public bool DeleteTimetable(int timetableId)
        {
            CurrentUser.DemandAction("Timetable", "Delete", "ليس لديك صلاحية حذف حصص الجدول.");
            if (timetableId <= 0)
                throw new ArgumentException("رقم الحصة غير صحيح.");

            bool deleted = repository.DeleteTimetable(timetableId);
            if (deleted)
                auditLogService.Record("حذف", "Timetable", timetableId.ToString(), "حذف حصة من الجدول الدراسي.");
            return deleted;
        }

        private void ValidateAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            string[] parts = academicYear.Trim().Replace('-', '/').Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2 ||
                parts[0].Length != 4 ||
                parts[1].Length != 4 ||
                !int.TryParse(parts[0], out firstYear) ||
                !int.TryParse(parts[1], out secondYear) ||
                secondYear != firstYear + 1)
            {
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027 أو 1447-1448.");
            }
        }

        private void Validate(TimetableEntry item)
        {
            if (item == null)
                throw new ArgumentException("بيانات الجدول غير صحيحة.");

            if (item.ClassID <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(item.Section) || item.Section.Trim().Length > 100)
                throw new ArgumentException("يجب اختيار شعبة صحيحة بطول لا يتجاوز 100 حرف.");

            if (item.SubjectID <= 0)
                throw new ArgumentException("يجب اختيار المادة.");

            if (item.TeacherID <= 0)
                throw new ArgumentException("يجب اختيار المعلم.");

            if (string.IsNullOrWhiteSpace(item.AcademicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            ValidateAcademicYear(item.AcademicYear);

            if (string.IsNullOrWhiteSpace(item.TermName))
                throw new ArgumentException("يجب اختيار الفصل الدراسي.");

            if (string.IsNullOrWhiteSpace(item.DayName))
                throw new ArgumentException("يجب اختيار اليوم.");

            if (item.PeriodNo <= 0 || item.PeriodNo > 20)
                throw new ArgumentException("رقم الحصة يجب أن يكون بين 1 و20.");

            if (item.StartTime >= item.EndTime)
                throw new ArgumentException("وقت البداية يجب أن يكون قبل وقت النهاية.");

            if (item.RoomName != null && item.RoomName.Trim().Length > 100)
                throw new ArgumentException("اسم القاعة لا يمكن أن يتجاوز 100 حرف.");

            if (item.Notes != null && item.Notes.Trim().Length > 1000)
                throw new ArgumentException("ملاحظات الحصة لا يمكن أن تتجاوز 1000 حرف.");
        }
    }
}
