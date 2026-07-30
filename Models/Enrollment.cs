using System;

namespace SchoolSystem.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; } // For display
        public string StudentNumber { get; set; } // For display
        
        public DateTime ApplicationDate { get; set; }
        public string ApplicationType { get; set; }
        public string AcademicYear { get; set; }
        
        public int ClassID { get; set; }
        public string ClassName { get; set; } // For display
        public string Section { get; set; }
        public string SeatNumber { get; set; }
        public string Status { get; set; }
        
        public string PreviousSchool { get; set; }
        public string PreviousClass { get; set; }
        public string TransferReason { get; set; }
        
        public decimal RegistrationFee { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string ReceiptNo { get; set; }
        
        public bool HasBirthCertificate { get; set; }
        public bool HasGuardianId { get; set; }
        public bool HasPhoto { get; set; }
        public bool HasLastCertificate { get; set; }
        public bool HasMedicalReport { get; set; }
        
        public string Notes { get; set; }
    }
}
