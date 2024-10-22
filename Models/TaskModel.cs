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
    public string Description { get; set; } = string.Empty;

    [Required]
    public float length {get; set;} = 0;

    [Required]
    public float height {get; set;} = 0;

    public string? DeniedList { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    public string? TakenById { get; set; }

    [Required]
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.MinValue;
}

// dotnet aspnet-codegenerator controller -name TasksController -m LionTaskManagementApp.Models.Task -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries *@
