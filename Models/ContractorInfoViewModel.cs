// using System.ComponentModel.DataAnnotations;

// namespace LionTaskManagementApp.Models;

// public class ContractorInfoViewModel
// {
//     [Required]
//     public string UserId { get; set; } = string.Empty;// You might need this to associate with the user

//     [Required]
//     public decimal PricePerSquareFoot { get; set; }


//     [Required]
//     public int PreferenceDistance { get; set; } 

//     [Required]
//     public string FirstLine { get; set; } = string.Empty;
    
//     public string SecondLine { get; set; } = string.Empty;
    
//     [Required]
//     public string StateProvince { get; set; } = string.Empty;

//     [Required]
//     public string City { get; set; } = string.Empty;

//     [Required]
//     public string ZipCode { get; set; } = string.Empty;

//     [Required]
//     public string LatAndLongitude { get; set; } = string.Empty;
// }
using System.ComponentModel.DataAnnotations;

namespace LionTaskManagementApp.Models
{
    public class ContractorInfoViewModel
    {
        [Required]
        public string UserId { get; set; } = string.Empty; // Associate with the user

        // Contact Profile
        [Required]
        public string CompanyName { get; set; } = string.Empty;

        // EIN & Business Organization documentation (file upload logic required)
        [Required]
        public string EIN { get; set; } = string.Empty;
        public string BusinessDocumentationLink { get; set; } = string.Empty;

        public List<string> uploadedDocuments { get; set;} = new List<string>();

        // Social Media Links
        public string FacebookLink { get; set; } = string.Empty;
        public string InstagramLink { get; set; } = string.Empty;
        public string TikTokLink { get; set; } = string.Empty;

        // Wallpen Hub Profile
        public string WallpenHubProfileLink { get; set; } = string.Empty;

        // Banking Information
        [Required]
        public string BankingInfo { get; set; } = string.Empty; // ACH or Direct Deposit

        // Artwork Specialization
        public string ArtworkSpecialization { get; set; } = string.Empty;

        // Radius Information
        [Required]
        public int MaxTravelDistanceMiles { get; set; }

        [Required]
        public bool ChargeTravelFeesOverLimit { get; set; }

        public decimal TravelFeeOverLimit { get; set; } // Per square footage or one-time fee

        // Wallpen E2 Machine Info
        public string WallpenModel { get; set; } = string.Empty;

        //Pricing Information 
         public decimal? CMYKPrice { get; set; }
         public decimal? WhiteColorPrice { get; set; }
         public decimal? CMYKWhiteColorPrice { get; set;}
         
         //Wallpen E2 Machine information 
        
        public string WallpenMachineModel { get; set; } = string.Empty;
        public string WallpenSerialNumber { get; set; } = string.Empty;

        public bool DoesPrintWhiteColor { get; set; }

        public bool SupportsCMYK { get; set; }

        public decimal MaxPrintWidth { get; set; } //Maximum print width 
        public decimal MaxPrintHeight { get; set; }// Maximum print height
        
        public bool SupportHightResolution { get; set; }// High-resolution support


        // Address Information
        
        [Required]
        public string FullAddress { get; set; } = string.Empty;
        public string FirstLine { get; set; } = string.Empty;
        public string SecondLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateProvince { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string LatAndLongitude { get; set; } = string.Empty;
    }
}

