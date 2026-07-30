using System;

namespace SchoolSystem.Models
{
    public class BusRoute
    {
        public int RouteID { get; set; }

        public string RouteName { get; set; }

        public int BusID { get; set; }
        public string BusNumber { get; set; }

        public string StartPoint { get; set; }
        public string EndPoint { get; set; }

        public TimeSpan? DepartureTime { get; set; }
        public TimeSpan? ArrivalTime { get; set; }

        public decimal? Fee { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
