using Microsoft.AspNetCore.Identity;
namespace LionTaskManagementApp.Areas.Identity.Data;

public class TaskUser : IdentityUser
{
    [PersonalData]
    public string Name { get; set; } = string.Empty;
    [PersonalData]
    public DateTime DOB { get; set; } = DateTime.MinValue;

    [PersonalData]
    public string Location { get; set; } = string.Empty;
}