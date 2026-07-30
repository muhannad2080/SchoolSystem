using System;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class StudentAttendanceService
    {
        private readonly StudentAttendanceRepository repository = new StudentAttendanceRepository();

        public DataTable GetAttendanceSheet(int classId, string section, string academicYear, DateTime date)
        {
            if (classId <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(section))
                throw new ArgumentException("يجب اختيار الشعبة.");

            if (string.IsNullOrWhiteSpace(academicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!Regex.IsMatch(academicYear.Trim(), @"^[0-9]{4}/[0-9]{4}$"))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");

            return repository.GetAttendanceSheet(
                classId,
                section.Trim(),
                academicYear.Trim(),
                date.Date);
        }

        public bool SaveAttendance(StudentAttendance item)
        {
            Validate(item);
            return repository.SaveAttendance(item);
        }

        private void Validate(StudentAttendance item)
        {
            if (item == null)
                throw new ArgumentException("بيانات الحضور غير صحيحة.");

            if (item.StudentID <= 0)
                throw new ArgumentException("بيانات الطالب غير صحيحة.");

            if (item.ClassID <= 0)
                throw new ArgumentException("بيانات الصف غير صحيحة.");

            if (string.IsNullOrWhiteSpace(item.Section))
                throw new ArgumentException("الشعبة مطلوبة.");

            if (string.IsNullOrWhiteSpace(item.AcademicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (string.IsNullOrWhiteSpace(item.Status))
                throw new ArgumentException("حالة الحضور مطلوبة.");

            if (item.Status != "حاضر" &&
                item.Status != "غائب" &&
                item.Status != "متأخر" &&
                item.Status != "مستأذن")
            {
                throw new ArgumentException("حالة الحضور غير صحيحة.");
            }
        }
    }
}
