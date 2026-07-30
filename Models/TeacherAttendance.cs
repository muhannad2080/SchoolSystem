using System;

namespace SchoolSystem.Models
{
    public class TeacherAttendance
    {
        public int AttendanceID { get; set; }

        public int TeacherID { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string Status { get; set; }

        public TimeSpan? CheckInTime { get; set; }

        public TimeSpan? CheckOutTime { get; set; }

        public int LateMinutes { get; set; }

        public int EarlyLeaveMinutes { get; set; }

        public decimal WorkHours { get; set; }

        public string AbsenceReason { get; set; }

        public string Notes { get; set; }

        public DateTime RecordedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
