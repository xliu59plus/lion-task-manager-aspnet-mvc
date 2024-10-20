using Microsoft.AspNetCore.Identity;
namespace LionTaskManagementApp.Areas.Identity.Data;

public class TaskUser : IdentityUser
{
    [PersonalData]
    public string Name { get; set; }
    [PersonalData]
    public DateTime DOB { get; set; }

    [PersonalData]
    public string PhoneNumber { get; set; }

    [PersonalData]
    public string Location { get; set; }
}