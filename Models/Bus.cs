using System;

namespace SchoolSystem.Models
{
    public class Bus
    {
        public int BusID { get; set; }
        public string BusNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public int Capacity { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
