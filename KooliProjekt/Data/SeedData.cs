using Microsoft.AspNetCore.Identity;

namespace Kooliprojekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            if (context.ProjectItem.Any())
            {
                return;
            }

            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true,
            };

            userManager.CreateAsync(adminUser, "Password123!").GetAwaiter().GetResult();

            var projectLists = new List<ProjectList>
        {
            new ProjectList { Title = "Project List 1" },
            new ProjectList { Title = "Project List 2" },
            new ProjectList { Title = "Project List 3" }
        };

            context.ProjectList.AddRange(projectLists);
            context.SaveChanges();

            var projectItems = new List<ProjectItem>
        {
            new ProjectItem
            {
                Title = "List 1",
                AdminName = "Kaspar",
                Name = "Kaspar",
                EstimatedWorkTime = 123,
                IsDone = false,
                StartDate = DateTime.Now,
                ProjectListId = projectLists[0].Id,
                WorkLogs = new List<WorkLog>
                {
                    new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 120, WorkerName = "Kaspar", Description = "Initial setup" },
                    new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 60, WorkerName = "John", Description = "Reviewed requirements" }
                }
            },
            new ProjectItem
            {
                Title = "List 2",
                AdminName = "John",
                Name = "John",
                EstimatedWorkTime = 95,
                IsDone = true,
                StartDate = DateTime.Now.AddDays(-10),
                ProjectListId = projectLists[1].Id,
                WorkLogs = new List<WorkLog>
                {
                    new WorkLog { Date = DateTime.Now.AddDays(-8), TimeSpentInMinutes = 200, WorkerName = "John", Description = "Completed task setup" }
                }
            },
            new ProjectItem
            {
                Title = "List 3",
                AdminName = "Alice",
                Name = "Alice",
                EstimatedWorkTime = 45,
                IsDone = false,
                StartDate = DateTime.Now.AddDays(-3),
                ProjectListId = projectLists[2].Id,
                WorkLogs = new List<WorkLog>
                {
                    new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 90, WorkerName = "Alice", Description = "Started documentation" }
                }
            }
        };

            context.ProjectItem.AddRange(projectItems);
            context.SaveChanges();
        }
    }

}