using System;

namespace SchoolSystem.Models
{
    public class TimetableEntry
    {
        public int TimetableID { get; set; }

        public int ClassID { get; set; }

        public string Section { get; set; }

        public int SubjectID { get; set; }

        public int TeacherID { get; set; }

        public string AcademicYear { get; set; }

        public string TermName { get; set; }

        public string DayName { get; set; }

        public int PeriodNo { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string RoomName { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }
    }
}
