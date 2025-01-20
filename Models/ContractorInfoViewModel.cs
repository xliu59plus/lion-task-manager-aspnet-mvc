using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LionTaskManagementApp.Models;

public class ContractorInfoViewModel
{
    [Required]
    public string UserId { get; set; } = string.Empty; // You might need this to associate with the user

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
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public string EIN { get; set; } = string.Empty;

    [Required]
    public string FacebookLink { get; set; } = string.Empty;

    [Required]
    public string InstagramLink { get; set; } = string.Empty;

    [Required]
    public string TikTokLink { get; set; } = string.Empty;

    [Required]
    public string WallpenHubProfileLink { get; set; } = string.Empty;

    [Required]
    public string ArtworkSpecialization { get; set; } = string.Empty;

    [Required]
    public bool DoesPrintWhiteColor { get; set; }

    [Required]
    public bool SupportsCMYK { get; set; }

    [Required]
    public string WallpenMachineModel { get; set; } = string.Empty;

    [Required]
    public string WallpenSerialNumber { get; set; } = string.Empty;

    [Required]
    public bool DoesChargeTravelFeesOverLimit { get; set; }

    [Required]
    public decimal TravelFeeOverLimit { get; set; }

    [Required]
    public string FeeType { get; set; } = string.Empty;

    [Required]
    public decimal? CMYKPrice { get; set; }

    [Required]
    public decimal? WhiteColorPrice { get; set; }

    [Required]
    public decimal? CMYKWhiteColorPrice { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }
}

