using System.ComponentModel.DataAnnotations;

namespace LionTaskManagementApp.Models;

public class TaskModel
{
    [Key]  // This explicitly marks Id as the primary key
    public int Id { get; set; }

    [Required]
    public string OwnerId { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public decimal Budget { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public float Length {get; set;} = 0;

    [Required]
    public float Height {get; set;} = 0;

    public string DeniedList { get; set; } = string.Empty;

    public string RequestList { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = string.Empty;

    [Required]
    public string FullAddress { get; set; } = string.Empty;

    [Required]
    public string FirstLine { get; set; } = string.Empty;

    public string? SecondLine { get; set; }

    [Required]
    public string StateProvince { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    public string LatAndLongitude { get; set; } = string.Empty;

    public string? TakenById { get; set; }

    [Required]
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.MinValue;
}