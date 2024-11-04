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
    
    public string SecondLine { get; set; } = string.Empty;
    
    [Required]
    public string StateProvince { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    public string LatAndLongitude { get; set; } = string.Empty;
}
