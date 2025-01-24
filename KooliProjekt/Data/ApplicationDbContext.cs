using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Kooliprojekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        public DbSet<ProjectItem> ProjectItem { get; set; }
        public DbSet<ProjectList> ProjectList { get; set; }
        public DbSet<WorkLog> WorkLog { get; set; } 
    }
}
