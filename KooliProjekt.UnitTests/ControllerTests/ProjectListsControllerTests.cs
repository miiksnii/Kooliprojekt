using Kooliprojekt.Controllers;
using Kooliprojekt.Models;
using Kooliprojekt.Services;
using Kooliprojekt.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ProjectListsControllerTests
    {
        private readonly Mock<IProjectListService> _projectListServiceMock;
        private readonly ProjectListsController _controller;

        public ProjectListsControllerTests()
        {
            _projectListServiceMock = new Mock<IProjectListService>();
            _controller = new ProjectListsController(_projectListServiceMock.Object);
        }


        [Fact]
        public async Task Details_Should_Return_NotFound_When_Id_Is_Null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_Should_Return_NotFound_When_ProjectList_Is_Missing()
        {
            // Arrange
            int id = 1;
            var projectList = (ProjectList)null;
            _projectListServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(projectList);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task Details_Should_Return_View_With_Model_When_ProjectList_Was_Found()
        {
            // Arrange
            int id = 1;
            var projectList = new ProjectList { Id = id, Title = "Test Project" };
            _projectListServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(projectList);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Details"
            );
            Assert.Equal(projectList, result.Model);
        }

        [Fact]
        public void Create_Should_Return_View()
        {
            // Act
            var result = _controller.Create() as ViewResult;
            
              
            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Create"
            );
        }

        [Fact]
        public async Task Edit_Should_Return_NotFound_When_ProjectList_Is_Missing()
        {
            // Arrange
            int id = 1;
            var projectList = (ProjectList)null;
            _projectListServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(projectList);

            // Act
            var result = await _controller.Edit(id, projectList) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_Should_Return_View_With_Model_When_ProjectList_Was_Found()
        {
            // Arrange
            int id = 1;
            var projectList = new ProjectList { Id = id, Title = "Test Project" };
            _projectListServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(projectList);

            // Act
            var result = await _controller.Edit(id, projectList) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Edit"
            );
            Assert.Equal(projectList, result.Model);
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Id_Is_Missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_ProjectList_Is_Missing()
        {
            // Arrange
            int id = 1;
            var projectList = (ProjectList)null;
            _projectListServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(projectList);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_Should_Return_View_With_Model_When_ProjectList_Was_Found()
        {
            // Arrange
            int id = 1;
            var projectList = new ProjectList { Id = id, Title = "Test Project" };
            _projectListServiceMock
                .Setup(x => x.Get(id))
                .ReturnsAsync(projectList);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Delete"
            );
            Assert.Equal(projectList, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_Should_Redirect_To_Index_When_ProjectList_Exists()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1, Title = "Test Project" };
            _projectListServiceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(projectList);
            _projectListServiceMock
                .Setup(x => x.Delete(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }



        [Fact]
        public async Task ProjectListExists_Should_Return_True_When_ProjectList_Exists()
        {
            // Arrange
            var projectListId = 1;
            var projectList = new ProjectList { Id = projectListId, Title = "Test Project" };
            _projectListServiceMock
                .Setup(x => x.Get(projectListId))
                .ReturnsAsync(projectList);  // Simulate that the project list exists

            // Act
            var result = _controller.ProjectListExists(projectListId);  // No await because it returns a bool

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task Edit_Post_Should_Return_NotFound_When_DbUpdateConcurrencyException_Is_Thrown()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1, Title = "Updated Project" };
            _projectListServiceMock
                .Setup(x => x.Save(It.IsAny<ProjectList>()))
                .ThrowsAsync(new DbUpdateConcurrencyException());  // Simulate concurrency exception

            // Act
            var result = await _controller.Edit(1, projectList) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task Edit_Post_Should_Return_NotFound_When_DbUpdateConcurrencyException_Is_Thrown_And_ProjectList_Does_Not_Exist()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1, Title = "Updated Project" };

            // Mock Save to throw DbUpdateConcurrencyException
            _projectListServiceMock
                .Setup(x => x.Save(It.IsAny<ProjectList>()))
                .ThrowsAsync(new DbUpdateConcurrencyException());  // Simulate concurrency exception

            // Mock Get to return null, simulating that the ProjectList doesn't exist
            _projectListServiceMock
                .Setup(x => x.Get(projectList.Id))
                .ReturnsAsync((ProjectList)null);  // Simulate that the project list doesn't exist

            // Act
            var result = await _controller.Edit(projectList.Id, projectList) as NotFoundResult;

            // Assert
            Assert.NotNull(result);  // Ensure a NotFoundResult is returned
        }



        [Fact]
        public async Task Create_Post_Should_Redirect_To_Index_When_Model_Is_Valid()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1, Title = "New Project" };
            _projectListServiceMock
                .Setup(service => service.Save(projectList))
                .Returns(Task.CompletedTask);  // Simulate successful save

            // Act
            var result = await _controller.Create(projectList) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);  // Should redirect to Index
        }

        [Fact]
        public async Task Edit_Post_Should_Return_NotFound_When_ProjectList_Id_Is_Changed()
        {
            // Arrange
            var projectList = new ProjectList { Id = 2, Title = "Mismatched Project" };

            // Act
            var result = await _controller.Edit(1, projectList) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Index_Should_Return_View_With_Model_When_ProjectLists_Are_Found()
        {
            // Arrange
            var page = 1;
            var model = new ProjectListIndexModel();
            var pagedResult = new PagedResult<ProjectList> { Results = new List<ProjectList> { new ProjectList { Id = 1, Title = "Test Project" } } };
            model.Data = pagedResult;
            _projectListServiceMock.Setup(x => x.List(page, 5, model.Search)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }

        // Test for Create POST method with invalid model
        [Fact]
        public async Task Create_Post_Should_Return_View_When_Model_Is_Invalid()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1, Title = "Test Project" };
            _controller.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = await _controller.Create(projectList) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectList, result.Model);
        }

        // Test for Edit POST method when IDs do not match
        [Fact]
        public async Task Edit_Post_Should_Return_NotFound_When_Id_Mismatches()
        {
            // Arrange
            var projectList = new ProjectList { Id = 2, Title = "Mismatched Project" };

            // Act
            var result = await _controller.Edit(1, projectList) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ProjectListExists_Should_Return_True_When_ProjectList_Exist()
        {
            // Arrange
            int projectListId = 1;
            var existingProjectList = new ProjectList { Id = projectListId, Title = "Test Project" };

            // Mock the service to return the existing project list
            _projectListServiceMock
                .Setup(service => service.Get(projectListId))
                .ReturnsAsync(existingProjectList);  // Simulate the service returning a valid project list

            // Act
            var result = _controller.ProjectListExists(projectListId);  // Call the method being tested

            // Assert
            Assert.True(result);  // Assert that it returns true, as the project list exists
        }

        [Fact]
        public void ProjectListExists_Should_Return_False_When_ProjectList_Does_Not_Exist()
        {
            // Arrange
            int projectListId = 999;  // An ID that doesn't exist
            _projectListServiceMock
                .Setup(service => service.Get(projectListId))
                .ReturnsAsync((ProjectList)null);  // Simulate the service returning null, meaning the project list doesn't exist

            // Act
            var result = _controller.ProjectListExists(projectListId);  // Call the method being tested

            // Assert
            Assert.False(result);  // Assert that it returns false, as the project list does not exist
        }

        [Fact]
        public void ProjectListExists_Should_Throw_Concurrency_Exception_When_DbUpdateConcurrencyException_Is_Thrown()
        {
            // Arrange
            int projectListId = 1;

            // Mock the service to throw a DbUpdateConcurrencyException when calling Get
            _projectListServiceMock
                .Setup(service => service.Get(projectListId))
                .ThrowsAsync(new DbUpdateConcurrencyException());  // Simulate a concurrency exception

            // Act & Assert
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => Task.Run(() => _controller.ProjectListExists(projectListId)));
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProjectListDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IProjectListService>();
            mockService.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync((ProjectList)null);  // Simulate non-existent project

            var controller = new ProjectListsController(mockService.Object);

            // Act
            var result = await controller.Delete(999);  // ID that doesn't exist

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_RedirectsToIndex_AfterSuccessfulSave()
        {
            // Arrange
            var mockService = new Mock<IProjectListService>();
            var projectList = new ProjectList { Id = 1, Title = "Test Project" };
            mockService.Setup(service => service.Save(projectList)).Returns(Task.CompletedTask);  // Simulate save

            var controller = new ProjectListsController(mockService.Object);

            // Act
            var result = await controller.Create(projectList);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);  // Ensure it redirects to Index
        }

        [Fact]
        public async Task Edit_Should_Return_NotFound_When_ProjectList_Is_Missing2()
        {
            // Arrange
            int? id = null;
            var projectList = (ProjectList)null;
            _projectListServiceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(projectList);

            // Act
            var result = await _controller.Edit(id, projectList) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }


    }
}
