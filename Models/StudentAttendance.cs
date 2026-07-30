using System;

namespace SchoolSystem.Models
{
    public class StudentAttendance
    {
        public int AttendanceID { get; set; }

        public int StudentID { get; set; }

        public int ClassID { get; set; }

        public string Section { get; set; }

        public string AcademicYear { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string Status { get; set; }

        public TimeSpan? ArrivalTime { get; set; }

        public string ExcuseStatus { get; set; }

        public string Notes { get; set; }
    }
}
