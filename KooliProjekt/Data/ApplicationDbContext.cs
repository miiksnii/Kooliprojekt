using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Kooliprojekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public virtual DbSet<ProjectIList> ProjectItem { get; set; }
    public virtual DbSet<ProjectList> ProjectList { get; set; }
    public virtual DbSet<WorkLog> WorkLog { get; set; }
    }
}
