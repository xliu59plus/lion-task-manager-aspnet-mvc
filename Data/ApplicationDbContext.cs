using LionTaskManagementApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Models;

namespace LionTaskManagementApp.Data;

public class ApplicationDbContext : IdentityDbContext<TaskUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<LionTaskManagementApp.Models.TaskModel> Task { get; set; } = default!;
}
