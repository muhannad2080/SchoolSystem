using System;

namespace SchoolSystem.Models
{
    public class StudentTransportAssignment
    {
        public int AssignmentID { get; set; }
        public int StudentID { get; set; }
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public int RouteID { get; set; }
        public string RouteName { get; set; }
        public string BusNumber { get; set; }
        public string AcademicYear { get; set; }
        public string PickupPoint { get; set; }
        public string DropoffPoint { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}