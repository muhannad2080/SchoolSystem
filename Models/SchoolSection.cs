using System;

namespace SchoolSystem.Models
{
    public class SchoolSection
    {
        public int SectionID { get; set; }
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string AcademicYear { get; set; }
        public bool IsActive { get; set; }
        public int? Capacity { get; set; }
        public string AllowedGender { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AssignedCount { get; set; }
    }
}