using System;

namespace SchoolSystem.Models
{
    public class Mark
    {
        public int MarkID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public int? TeacherID { get; set; }
        public decimal MarkValue { get; set; }
        public string ExamType { get; set; }
        public DateTime CreatedAt { get; set; }

        // للعرض فقط
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
    }
}