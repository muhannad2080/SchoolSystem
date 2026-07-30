using System;

namespace SchoolSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        // يتولد تلقائياً من قاعدة البيانات/Repository
        public string StudentNumber { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public string Nationality { get; set; }

        public string NationalId { get; set; }

        public string StudentPhone { get; set; }

        public string Status { get; set; }

        // ولي الأمر
        public string GuardianName { get; set; }

        public string GuardianRelation { get; set; }

        public string GuardianPhone { get; set; }

        public string GuardianEmail { get; set; }

        public string GuardianJob { get; set; }

        // العنوان
        public string Governorate { get; set; }

        public string District { get; set; }

        public string Address { get; set; }

        // الصورة
        public byte[] Photo { get; set; }

        // للعرض فقط داخل الجدول
        public string CurrentClassName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
