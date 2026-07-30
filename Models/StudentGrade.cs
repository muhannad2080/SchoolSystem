using System;

namespace SchoolSystem.Models
{
    public class StudentGrade
    {
        public int GradeID { get; set; }

        public int StudentID { get; set; }

        public int SubjectID { get; set; }

        public int ClassID { get; set; }

        public string Section { get; set; }

        public string AcademicYear { get; set; }

        public string TermName { get; set; }

        public decimal Quiz1 { get; set; }

        public decimal Quiz2 { get; set; }

        public decimal CourseWork { get; set; }

        public decimal FinalExam { get; set; }

        public decimal Total { get; set; }

        public string GradeLetter { get; set; }

        public string ResultStatus { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
