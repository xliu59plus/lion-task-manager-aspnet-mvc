using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; // For IFormFile

namespace LionTaskManagementApp.Models;

public class TaskModel
{
    [Key]
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
    public float Length { get; set; } = 0;

    [Required]
    public float Height { get; set; } = 0;

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
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }
    public string? TakenById { get; set; }

    [Required]
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.MinValue;

    [Required]
    public DateTimeOffset LastUpdatedTime { get; set; } = DateTimeOffset.MinValue;

    [Required]
    public DateTimeOffset Deadline { get; set; } = DateTimeOffset.MinValue;

    [Required]
    public int ProjectResolution { get; set; }

    [Required]
    public string IndoorOutdoor { get; set; } = string.Empty;

    public string? WallType { get; set; }

    [NotMapped]
    public IFormFile? WallPic { get; set; }

    [NotMapped]
    public IFormFile? Artwork { get; set; }

    [Required]
    public bool DowngradeResolution { get; set; }

    public string? WallPicKey { get; set; }

    [NotMapped]
    public string? WallPicUrl { get; set; }

    public string? ArtworkKey { get; set; }

    [NotMapped]
    public string? ArtworkUrl { get; set; }

    [Required]
    public string? PaymentSessionId { get; set; } = String.Empty;

    [Required]
    public string PaymentIntentId { get; set; } = String.Empty;
}