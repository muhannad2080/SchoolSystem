using System;


namespace SchoolSystem.Models
{

    public class SchoolSettings
    {

        public int SchoolSettingsID { get; set; }

        public string SchoolNameAr { get; set; }

        public string SchoolNameEn { get; set; }

        public string OrganizationNameAr { get; set; }

        public string OrganizationNameEn { get; set; }

        public string AddressAr { get; set; }

        public string AddressEn { get; set; }

        public string Phone { get; set; }

        public string AdditionalPhone { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string PrincipalNameAr { get; set; }

        public string PrincipalNameEn { get; set; }

        public string LicenseNumber { get; set; }

        public byte[] Logo { get; set; }

        public string LogoFileName { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int? UpdatedByUserID { get; set; }
    }
}
