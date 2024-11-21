using System;
using System.ComponentModel.DataAnnotations;

namespace LionTaskManagementApp.Areas.Identity.Data
{
    public class ContractorInfo
    {
        [Key]
        public string UserId { get; set; } = string.Empty; // User identifier

        // Business Information
        public string CompanyName { get; set; } = string.Empty; // Business Name
        public string EIN { get; set; } = string.Empty; // Employer Identification Number
        public string BusinessDocumentationLink { get; set; } = string.Empty; // Link to uploaded documentation

        // Social Media Links
        public string FacebookLink { get; set; } = string.Empty;
        public string TikTokLink { get; set; } = string.Empty;
        public string InstagramLink { get; set; } = string.Empty;
        public string WallpenHubProfileLink { get; set; } = string.Empty;

      
        // Banking Information
        public string BankingInfo { get; set; } = string.Empty; // ACH or direct deposit info

        // Wallpen Machine Information
        public string WallpenSerialNumber { get; set; } = string.Empty;
        public string WallpenMachineModel { get; set; } = string.Empty; // Updated to avoid "Model" keyword conflict
        public bool DoesPrintWhiteColor { get; set; } // Supports white color printing
        public bool SupportsCMYK { get; set; } // Supports CMYK colors

        // Printing Prices
        public decimal CostPerSqrFoot { get; set; } = 0; // Base price per square foot
        public decimal? CMYKPrice { get; set; } // Price for CMYK colors per square foot
        public decimal? WhiteColorPrice { get; set; } // Price for white color per square foot
        public decimal? CMYKWhiteColorPrice { get; set; } // Price for CMYK + white per square foot

        // Travel Preferences
        public decimal PreferenceDistance { get; set; } = decimal.MaxValue; // Maximum travel distance
        public decimal TravelFeeOverLimit { get; set; } = 0; // Travel fee beyond preference distance
        public bool ChargeTravelFeesOverLimit { get; set; } = false; // Charges travel fees if over limit

        // Artwork Specialization
        public string ArtworkSpecialization { get; set; } = string.Empty; // Specialized art style

        // Address Information
        public string FullAddress { get; set; } = string.Empty;
        public string FirstLine { get; set; } = string.Empty;
        public string SecondLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateProvince { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string LatAndLongitude { get; set; } = string.Empty; // Latitude and Longitude

        // Profile Status
        public DateTimeOffset ProfileSubmitTime { get; set; } = DateTimeOffset.MinValue; // Time profile submitted
        public DateTimeOffset ActivatedTime { get; set; } = DateTimeOffset.MinValue; // Activation time
        public bool IsActive { get; set; } = false; // Business status

        // Notes or Additional Info
        public string AdditionalNotes { get; set; } = string.Empty; // Optional field for extra notes
    }
}
