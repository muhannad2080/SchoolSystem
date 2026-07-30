using System;

namespace SchoolSystem.Models
{
    public class StudentClass
    {
        public int StudentClassID { get; set; }

        public int StudentID { get; set; }

        public int ClassID { get; set; }

        public string Section { get; set; }

        public string AcademicYear { get; set; }

        public DateTime AssignedDate { get; set; }

        public int? AssignedBy { get; set; }
    }
}
