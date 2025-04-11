using Kooliprojekt.Data;
using Kooliprojekt.Models;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ProjectListServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly ProjectListService _service;
        private readonly Mock<DbSet<ProjectList>> _mockProjectLists;
        private readonly Mock<DbSet<ProjectIList>> _mockProjectItems;
        private readonly Mock<DbSet<WorkLog>> _mockWorkLogs;

        public ProjectListServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    
    // Modify this existing line to pass the options
    _mockContext = new Mock<ApplicationDbContext>(options);  // <- Added (options)
            _mockProjectLists = new Mock<DbSet<ProjectList>>();
            _mockProjectItems = new Mock<DbSet<ProjectIList>>();
            _mockWorkLogs = new Mock<DbSet<WorkLog>>();

            _service = new ProjectListService(_mockContext.Object);
        }

        // Renaming the 'Save_should_add_new_list_if_id_zero' method
        [Fact]
        public async Task Save_ProjectList_should_add_new_list_if_id_zero()
        {
            // Arrange
            var newList = new ProjectList { Id = 0, Title = "New List" };
            _mockContext.Setup(c => c.ProjectList.Add(newList));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _service.Save(newList);

            // Assert
            _mockContext.Verify(c => c.ProjectList.Add(newList), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        // Renaming the 'Save_should_update_existing_list_if_id_nonzero' method
        [Fact]
        public async Task Save_ProjectList_should_update_existing_list_if_id_nonzero()
        {
            // Arrange
            var existingList = new ProjectList { Id = 1, Title = "Existing List" };
            _mockContext.Setup(c => c.ProjectList.Update(existingList));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _service.Save(existingList);

            // Assert
            _mockContext.Verify(c => c.ProjectList.Update(existingList), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
}