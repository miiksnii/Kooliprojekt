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

            userManager.CreateAsync(adminUser, "Password123!").GetAwaiter().GetResult();

            var list = new ProjectItem
            {
                Title = "List 1",                // Title of the project item
                AdminName = "Kaspar",            // Admin Name
                Name = "Kaspar",                 // Name of the project item
                EstimatedWorkTime = 123,         // Estimated work time (in hours, I assume)
                IsDone = false,                  // Whether the item is completed or not
                StartDate = DateTime.Now,        // Start date (set to the current date/time)
                ProjectListId = 1,               // Assuming the ProjectListId is 1 (or set to a valid ID)
                ProjectList = new ProjectList
                {
                    Title = "Project List Title", // Set a Title for the ProjectList
                                                  // Initialize any other properties for ProjectList if necessary.
                },
                WorkLogs = new List<WorkLog>     // Optional, initializing with some work logs
    {
         new WorkLog
         {
            Date = DateTime.Now,            // Log date (current date/time)
            TimeSpentInMinutes = 120,       // Example time spent (2 hours)
            WorkerName = "Kaspar",          // Worker name
            Description = "Initial project setup" // Description of work done
         },
    }
            };

            context.ProjectItem.Add(list);

            // Veel andmeid

            context.SaveChanges();
        }
    }
}