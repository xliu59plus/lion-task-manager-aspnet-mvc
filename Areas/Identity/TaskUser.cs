using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace LionTaskManagementApp.Areas.Identity.Data;

public class TaskUser : IdentityUser
{
    [PersonalData]
    public string Name { get; set; } = string.Empty;

    public DateTimeOffset DOB { get; set; } = DateTimeOffset.MinValue;

    public string Location { get; set; } = string.Empty;
}