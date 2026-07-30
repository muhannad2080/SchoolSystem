using System;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class TeacherAttendanceService
    {
        private readonly TeacherAttendanceRepository repository = new TeacherAttendanceRepository();

        private readonly TimeSpan schoolStartTime = new TimeSpan(8, 0, 0);
        private readonly TimeSpan schoolEndTime = new TimeSpan(14, 0, 0);

        public DataTable GetAllAttendance()
        {
            return repository.GetAllAttendance();
        }

        public bool AddAttendance(TeacherAttendance attendance)
        {
            ValidateAttendance(attendance, false);
            ApplyAttendanceCalculations(attendance);
            return repository.AddAttendance(attendance);
        }

        public bool UpdateAttendance(TeacherAttendance attendance)
        {
            ValidateAttendance(attendance, true);
            ApplyAttendanceCalculations(attendance);
            return repository.UpdateAttendance(attendance);
        }

        public bool DeleteAttendance(int attendanceId)
        {
            if (attendanceId <= 0)
                throw new ArgumentException("رقم سجل الحضور غير صحيح.");

            return repository.DeleteAttendance(attendanceId);
        }

        public bool AttendanceExists(int teacherId, DateTime attendanceDate)
        {
            return repository.AttendanceExists(teacherId, attendanceDate.Date, 0);

        }

        public bool AttendanceExists(int teacherId, DateTime attendanceDate, int excludedAttendanceId)
        {
            return repository.AttendanceExists(teacherId, attendanceDate.Date, excludedAttendanceId);
        }

        public void ApplyAttendanceCalculations(TeacherAttendance attendance)
        {
            attendance.LateMinutes = 0;
            attendance.EarlyLeaveMinutes = 0;
            attendance.WorkHours = 0;

            if (attendance.Status == "غائب" ||
                attendance.Status == "إجازة" ||
                attendance.Status == "مريض")
            {
                attendance.CheckInTime = null;
                attendance.CheckOutTime = null;
                return;
            }

            if (attendance.CheckInTime.HasValue)
            {
                if (attendance.CheckInTime.Value > schoolStartTime)
                    attendance.LateMinutes = Convert.ToInt32((attendance.CheckInTime.Value - schoolStartTime).TotalMinutes);
            }

            if (attendance.CheckOutTime.HasValue)
            {
                if (attendance.CheckOutTime.Value < schoolEndTime)
                    attendance.EarlyLeaveMinutes = Convert.ToInt32((schoolEndTime - attendance.CheckOutTime.Value).TotalMinutes);
            }

            if (attendance.CheckInTime.HasValue && attendance.CheckOutTime.HasValue)
            {
                TimeSpan duration = attendance.CheckOutTime.Value - attendance.CheckInTime.Value;
                if (duration.TotalMinutes > 0)
                    attendance.WorkHours = Convert.ToDecimal(Math.Round(duration.TotalHours, 2));
            }
        }

        private void ValidateAttendance(TeacherAttendance attendance, bool isUpdate)
        {
            if (attendance == null)
                throw new ArgumentException("بيانات الحضور غير صحيحة.");

            if (isUpdate && attendance.AttendanceID <= 0)
                throw new ArgumentException("اختر سجل حضور صحيح للتعديل.");

            if (attendance.TeacherID <= 0)
                throw new ArgumentException("يجب اختيار المعلم.");

            if (attendance.AttendanceDate.Date > DateTime.Today)
                throw new ArgumentException("لا يمكن تسجيل حضور بتاريخ مستقبلي.");

            if (string.IsNullOrWhiteSpace(attendance.Status))
                throw new ArgumentException("يجب اختيار حالة الحضور.");

            if (!IsValidStatus(attendance.Status))
                throw new ArgumentException("حالة الحضور غير صحيحة.");

            bool noTimeRequired =
                attendance.Status == "غائب" ||
                attendance.Status == "إجازة" ||
                attendance.Status == "مريض";

            bool reasonRequired =
                attendance.Status == "غائب" ||
                attendance.Status == "إجازة" ||
                attendance.Status == "مريض" ||
                attendance.Status == "مأذون" ||
                attendance.Status == "انصراف مبكر";

            if (!noTimeRequired)
            {
                if (!attendance.CheckInTime.HasValue)
                    throw new ArgumentException("وقت الحضور مطلوب لهذه الحالة.");

                if (attendance.CheckOutTime.HasValue &&
                    attendance.CheckOutTime.Value <= attendance.CheckInTime.Value)
                    throw new ArgumentException("وقت الانصراف يجب أن يكون بعد وقت الحضور.");
            }

            if (reasonRequired && string.IsNullOrWhiteSpace(attendance.AbsenceReason))
                throw new ArgumentException("سبب الغياب أو الإذن مطلوب لهذه الحالة.");

            if (!string.IsNullOrWhiteSpace(attendance.AbsenceReason) &&
                !IsValidArabicText(attendance.AbsenceReason))
                throw new ArgumentException("سبب الغياب يجب أن يحتوي على حروف فقط بدون أرقام أو رموز غير مسموحة.");

            if (!string.IsNullOrWhiteSpace(attendance.Notes) &&
                !IsValidArabicText(attendance.Notes))
                throw new ArgumentException("الملاحظات يجب أن تحتوي على حروف فقط بدون أرقام أو رموز غير مسموحة.");
        }

        private bool IsValidStatus(string status)
        {
            return status == "حاضر" ||
                   status == "غائب" ||
                   status == "متأخر" ||
                   status == "إجازة" ||
                   status == "مريض" ||
                   status == "مأذون" ||
                   status == "انصراف مبكر";
        }

        private bool IsValidArabicText(string text)
        {
            return Regex.IsMatch(text, @"^[\u0600-\u06FFa-zA-Z\s\.\،\,\-\_]+$");
        }
    }
}
