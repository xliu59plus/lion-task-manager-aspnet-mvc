using System.ComponentModel.DataAnnotations;

namespace LionTaskManagementApp.Models;

public class ContractorInfoViewModel
{
    [Required]
    public string UserId { get; set; } = string.Empty;// You might need this to associate with the user

    [Required]
    public decimal PricePerSquareFoot { get; set; }

    [Required]
    public string FullAddress { get; set; } = string.Empty;

    [Required]
    public int PreferenceDistance { get; set; } 

    [Required]
    public string FirstLine { get; set; } = string.Empty;
    
    public string? SecondLine { get; set; } = string.Empty;
    
    [Required]
    public string StateProvince { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    public decimal MaxTravelDistanceMiles { get; set; }
    [Required]
    public string CompanyName { get; internal set; } = string.Empty;
    [Required]
    public string EIN { get; internal set; } = string.Empty;
    [Required]
    public string FacebookLink { get; internal set; } = string.Empty;
    [Required]
    public string InstagramLink { get; internal set; } = string.Empty;
    [Required]
    public string TikTokLink { get; internal set; } = string.Empty;
    [Required]
    public string WallpenHubProfileLink { get; internal set; } = string.Empty;
    [Required]
    public string BankingInfo { get; internal set; } = string.Empty;
    [Required]
    public string ArtworkSpecialization { get; internal set; } = string.Empty;
    [Required]
    public bool DoesPrintWhiteColor { get; internal set; }
    [Required]
    public bool SupportsCMYK { get; internal set; }
    [Required]
    public string WallpenMachineModel { get; internal set; } = string.Empty;
    [Required]
    public string WallpenSerialNumber { get; internal set; } = string.Empty;
    [Required]
    public bool ChargeTravelFeesOverLimit { get; internal set; }
    [Required]
    public decimal TravelFeeOverLimit { get; internal set; }
    [Required]
    public decimal? CMYKPrice { get; internal set; }
    [Required]
    public decimal? WhiteColorPrice { get; internal set; }
    [Required]
    public decimal? CMYKWhiteColorPrice { get; internal set; }

    [Required]
    public string Latitude { get; set; } = string.Empty;
    [Required]
    public string Longitude { get; set; } = string.Empty;

}
