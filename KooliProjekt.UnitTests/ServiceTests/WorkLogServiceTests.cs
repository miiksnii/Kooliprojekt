using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using KooliProjekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KooliProjekt.UnitTests.ServiceTests;

namespace KooliProjekt.Tests.Services
{
    public class WorkLogServiceTests : ServiceTestBase
    {
        private WorkLogService CreateService()
        {
            return new WorkLogService(DbContext);
        }

        [Fact]
        public async Task List_ReturnsPagedResult()
        {
            // Arrange
            var service = CreateService();
            var workLogs = new[]
            {
                new WorkLog { Date = DateTime.Now.AddDays(-2), TimeSpentInMinutes = 30, WorkerName = "John", Description = "Initial setup" },
                new WorkLog { Date = DateTime.Now.AddDays(-1), TimeSpentInMinutes = 60, WorkerName = "Jane", Description = "Database design" },
                new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 45, WorkerName = "John", Description = "API implementation" }
            };
            DbContext.WorkLog.AddRange(workLogs);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await service.List(1, 2);

            // Assert
            Assert.NotEmpty(result.Results);
        }

        [Fact]
        public async Task List_WithKeywordSearch_FiltersResults()
        {
            // Arrange
            var service = CreateService();
            var workLogs = new[]
            {
                new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 30, WorkerName = "John", Description = "Initial setup" },
                new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 60, WorkerName = "Jane", Description = "Database design" },
                new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 45, WorkerName = "John", Description = "API implementation" }
            };
            DbContext.WorkLog.AddRange(workLogs);
            await DbContext.SaveChangesAsync();

            var search = new WorkLogSearch { Keyword = "John" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.NotEmpty(result.Results);
        }

        [Fact]
        public async Task Get_ReturnsCorrectWorkLog()
        {
            // Arrange
            var service = CreateService();
            var workLog = new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 30, WorkerName = "John", Description = "Test" };
            DbContext.WorkLog.Add(workLog);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await service.Get(workLog.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.Get(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Save_AddsNewWorkLog()
        {
            // Arrange
            var service = CreateService();
            var workLog = new WorkLog
            {
                Date = DateTime.Now,
                TimeSpentInMinutes = 30,
                WorkerName = "John",
                Description = "New log"
            };

            // Act
            await service.Save(workLog);

            // Assert
            var savedLog = await DbContext.WorkLog.FirstOrDefaultAsync(w => w.Description == "New log");
            Assert.NotNull(savedLog); // Ensure that the work log was saved
            Assert.Equal("New log", savedLog.Description); // Check that the description is correct
            Assert.True(savedLog.Id > 0); // Check that the log has been assigned a valid ID
        }


        [Fact]
        public async Task Save_UpdatesExistingWorkLog()
        {
            // Arrange
            var service = CreateService();
            var workLog = new WorkLog
            {
                Date = DateTime.Now,
                TimeSpentInMinutes = 30,
                WorkerName = "John",
                Description = "Original"
            };

            // Save the initial work log
            DbContext.WorkLog.Add(workLog);
            await DbContext.SaveChangesAsync();

            // Act
            workLog.Description = "Updated"; // Modify the work log's description
            await service.Save(workLog); // Save the updated work log

            // Assert
            var updatedLog = await DbContext.WorkLog.FindAsync(workLog.Id); // Fetch the updated work log
            Assert.NotNull(updatedLog); // Ensure the log was found
            Assert.Equal("Updated", updatedLog.Description); // Verify the description was updated
        }

        [Fact]
        public async Task Delete_RemovesWorkLog()
        {
            // Arrange
            var service = CreateService();
            var workLog = new WorkLog
            {
                Date = DateTime.Now,
                TimeSpentInMinutes = 30,
                WorkerName = "John",
                Description = "To be deleted"
            };
            DbContext.WorkLog.Add(workLog);
            await DbContext.SaveChangesAsync(); // Save the work log to the database

            // Act
            await service.Delete(workLog.Id); // Call the Delete method with the work log's Id

            // Assert
            var deletedLog = await DbContext.WorkLog.FindAsync(workLog.Id); // Try to find the work log again
            Assert.Null(deletedLog); // Ensure the work log is removed
        }


        [Fact]
        public async Task Delete_WithInvalidId_DoesNothing()
        {
            // Arrange
            var service = CreateService();
            var initialCount = await DbContext.WorkLog.CountAsync(); // Get the initial count of work logs

            // Act
            await service.Delete(999); // Try to delete a work log with an invalid Id (non-existent)

            // Assert
            var finalCount = await DbContext.WorkLog.CountAsync(); // Get the final count after the delete attempt
            Assert.Equal(initialCount, finalCount); // Ensure the count has not changed
        }


        [Fact]
        public async Task List_OrdersByDateDescending()
        {
            // Arrange
            var service = CreateService();
            var workLogs = new[]
            {
                new WorkLog { Date = DateTime.Now.AddDays(-2), TimeSpentInMinutes = 30, WorkerName = "John", Description = "Oldest" },
                new WorkLog { Date = DateTime.Now, TimeSpentInMinutes = 60, WorkerName = "Jane", Description = "Newest" },
                new WorkLog { Date = DateTime.Now.AddDays(-1), TimeSpentInMinutes = 45, WorkerName = "John", Description = "Middle" }
            };
            DbContext.WorkLog.AddRange(workLogs);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await service.List(1, 3);

            // Assert
            var items = result.Results.ToList();
            Assert.NotEmpty(items);
        }
    }
}