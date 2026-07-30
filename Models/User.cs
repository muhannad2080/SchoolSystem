using System;

namespace SchoolSystem.Models
{
    public class User
    {
        public int UserID { get; set; }

        public string FullName { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public string RoleName { get; set; }
        public string Permissions { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public bool IsActive { get; set; }
        public bool MustChangePassword { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
