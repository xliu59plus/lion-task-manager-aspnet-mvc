using System.ComponentModel.DataAnnotations;

namespace LionTaskManagementApp.Models.Poster
{
    public class PosterInfo
    {
        [Key]
        public String UserId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateProvince { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public string EIN { get; set; } = string.Empty;
        public string IndustryInformation { get; set; } = string.Empty;
    }
}
