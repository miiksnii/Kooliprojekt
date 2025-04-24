using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class WorkLogServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkLogService _service;

        public WorkLogServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // ensure isolated DB per test run
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _service = new WorkLogService(_context);
        }

        [Fact]
        public async Task Save_should_add_new_worklog_when_id_is_zero()
        {
            // Arrange
            var workLog = new WorkLog
            {
                Id = 0,
                Date = DateTime.Today,
                TimeSpentInMinutes = 60,
                WorkerName = "Alice",
                Description = "Initial task"
            };

            // Act
            await _service.Save(workLog);

            // Assert
            var result = await _context.WorkLog.FirstOrDefaultAsync();
            Assert.NotNull(result);
            Assert.Equal("Alice", result.WorkerName);
        }

        [Fact]
        public async Task Save_should_update_existing_worklog_when_id_is_nonzero()
        {
            // Arrange
            var workLog = new WorkLog
            {
                Date = DateTime.Today,
                TimeSpentInMinutes = 60,
                WorkerName = "Bob",
                Description = "Original"
            };
            _context.WorkLog.Add(workLog);
            await _context.SaveChangesAsync();

            workLog.Description = "Updated";

            // Act
            await _service.Save(workLog);

            // Assert
            var result = await _context.WorkLog.FindAsync(workLog.Id);
            Assert.Equal("Updated", result.Description);
        }

        [Fact]
        public async Task Get_should_return_worklog_by_id()
        {
            // Arrange
            var workLog = new WorkLog
            {
                WorkerName = "Charlie",
                Description = "Testing Get"
            };
            _context.WorkLog.Add(workLog);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.Get(workLog.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Charlie", result.WorkerName);
        }

        [Fact]
        public async Task Delete_should_remove_worklog_by_id()
        {
            // Arrange
            var workLog = new WorkLog
            {
                WorkerName = "DeleteMe",
                Description = "To be deleted"
            };
            _context.WorkLog.Add(workLog);
            await _context.SaveChangesAsync();

            // Act
            await _service.Delete(workLog.Id);

            // Assert
            var deleted = await _context.WorkLog.FindAsync(workLog.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task List_should_return_filtered_results()
        {
            // Arrange
            _context.WorkLog.AddRange(
                new WorkLog { WorkerName = "Anna", Description = "Fix bug" },
                new WorkLog { WorkerName = "Anna", Description = "Build UI" },
                new WorkLog { WorkerName = "Ben", Description = "Write tests" }
            );
            await _context.SaveChangesAsync();

            var search = new WorkLogSearch
            {
                Keyword = "Anna"
            };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.Results.Count);
            Assert.All(result.Results, r => Assert.Contains("Anna", r.WorkerName));
        }

        [Fact]
        public async Task List_should_return_paged_results()
        {
            // Arrange
            for (int i = 1; i <= 25; i++)
            {
                _context.WorkLog.Add(new WorkLog
                {
                    WorkerName = $"User{i}",
                    Description = $"Task {i}",
                    Date = DateTime.Now,
                    TimeSpentInMinutes = i
                });
            }
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.List(page: 2, pageSize: 10);

            // Assert
            Assert.Equal(10, result.Results.Count); // second page
            Assert.Equal(3, result.PageCount); // 25 items / 10 = 3 pages
        }
    }
}
