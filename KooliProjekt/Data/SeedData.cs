using Microsoft.AspNetCore.Identity;

namespace Kooliprojekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            if(context.ProjectItem.Any())
            {
                return;
            }

            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin",
                EmailConfirmed = true,
            };

            userManager.CreateAsync(adminUser, "Password123!");

            var list = new ProjectItem();
            list.Title = "List 1";
            context.ProjectItem.Add(list);

            // Veel andmeid

            context.SaveChanges();
        }
    }
}