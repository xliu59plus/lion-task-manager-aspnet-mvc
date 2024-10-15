using Microsoft.AspNetCore.Identity;
namespace LionTaskManagementApp.Areas.Identity.Data;

public class CutomizedUser : IdentityUser
{
    [PersonalData]
    public string? Name { get; set; }
    [PersonalData]
    public DateTime DOB { get; set; }
}

//https://learn.microsoft.com/en-ca/aspnet/core/security/authentication/add-user-data?view=aspnetcore-8.0&tabs=net-cli