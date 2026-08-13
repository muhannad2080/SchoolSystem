using System;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class TimetableService
    {
        private readonly TimetableRepository repository = new TimetableRepository();

        public DataTable GetTeachers()
        {
            EnsureCanManageTimetable();
            return repository.GetTeachers();
        }

        public DataTable GetSubjectsByClass(int classId)
        {
            EnsureCanManageTimetable();
            if (classId <= 0)
                return new DataTable();

            return repository.GetSubjectsByClass(classId);
        }

        public DataTable GetAllTimetable()
        {
            EnsureCanManageTimetable();
            return repository.GetAllTimetable();
        }

        public bool AddTimetable(TimetableEntry item)
        {
            EnsureCanManageTimetable();
            Validate(item);

            if (repository.HasClassConflict(item))
                throw new ArgumentException("يوجد تعارض: هذا الصف والشعبة لديهم حصة في نفس الوقت.");

            if (repository.HasTeacherConflict(item))
                throw new ArgumentException("يوجد تعارض: المعلم لديه حصة أخرى في نفس الوقت.");

            if (repository.HasRoomConflict(item))
                throw new ArgumentException("يوجد تعارض: القاعة مستخدمة في نفس الوقت.");

            return repository.AddTimetable(item);
        }

        public bool UpdateTimetable(TimetableEntry item)
        {
            EnsureCanManageTimetable();
            Validate(item);

            if (item.TimetableID <= 0)
                throw new ArgumentException("اختر حصة صحيحة للتعديل.");

            if (repository.HasClassConflict(item))
                throw new ArgumentException("يوجد تعارض: هذا الصف والشعبة لديهم حصة في نفس الوقت.");

            if (repository.HasTeacherConflict(item))
                throw new ArgumentException("يوجد تعارض: المعلم لديه حصة أخرى في نفس الوقت.");

            if (repository.HasRoomConflict(item))
                throw new ArgumentException("يوجد تعارض: القاعة مستخدمة في نفس الوقت.");

            return repository.UpdateTimetable(item);
        }

        public bool DeleteTimetable(int timetableId)
        {
            EnsureCanManageTimetable();
            if (timetableId <= 0)
                throw new ArgumentException("رقم الحصة غير صحيح.");

            return repository.DeleteTimetable(timetableId);
        }

        private static void EnsureCanManageTimetable()
        {
            if (!CurrentUser.HasPermission(PermissionKeys.TimetableManage))
                throw new UnauthorizedAccessException("ليس لديك صلاحية إدارة الجدول الدراسي.");
        }

        private void Validate(TimetableEntry item)
        {
            if (item == null)
                throw new ArgumentException("بيانات الجدول غير صحيحة.");

            if (item.ClassID <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(item.Section))
                throw new ArgumentException("يجب اختيار الشعبة.");

            if (item.SubjectID <= 0)
                throw new ArgumentException("يجب اختيار المادة.");

            if (item.TeacherID <= 0)
                throw new ArgumentException("يجب اختيار المعلم.");

            if (string.IsNullOrWhiteSpace(item.AcademicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!Regex.IsMatch(item.AcademicYear.Trim(), @"^[0-9]{4}/[0-9]{4}$"))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");

            if (string.IsNullOrWhiteSpace(item.TermName))
                throw new ArgumentException("يجب اختيار الفصل الدراسي.");

            if (string.IsNullOrWhiteSpace(item.DayName))
                throw new ArgumentException("يجب اختيار اليوم.");

            if (item.PeriodNo <= 0)
                throw new ArgumentException("رقم الحصة غير صحيح.");

            if (item.StartTime >= item.EndTime)
                throw new ArgumentException("وقت البداية يجب أن يكون قبل وقت النهاية.");
        }
    }
}
