using System.ComponentModel.DataAnnotations;
namespace LionTaskManagementApp.Areas.Identity.Data;

public class ContractorInfo
{
    [Key]
    public string UserId { get; set;} = string.Empty;

    public decimal CostPerSqrFoot { get; set;} = 0;
    
    public string FullAddress { get; set; } = string.Empty;

    public string LatAndLongitude { get; set; } = string.Empty;

    public string ZipCode { get; set; } = string.Empty;

    public decimal PreferenceDistance { get; set; } = decimal.MaxValue;

    public DateTimeOffset ProfileSubmitTime { get; set; } = DateTimeOffset.MinValue;

    public DateTimeOffset ActivatedTime { get; set; } = DateTimeOffset.MinValue;
}