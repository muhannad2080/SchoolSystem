using System;

namespace SchoolSystem.Models
{
    public class Room
    {
        public int RoomID { get; set; }

        public string RoomCode { get; set; }

        public string RoomName { get; set; }

        public string RoomType { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; }

        public bool IsActive { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
