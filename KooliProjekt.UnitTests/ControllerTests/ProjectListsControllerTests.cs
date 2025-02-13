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
        public async Task Edit_Should_Return_NotFound_When_Id_Is_Missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
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
            var result = await _controller.Edit(id) as NotFoundResult;

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
            var result = await _controller.Edit(id) as ViewResult;

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
            var result = _controller.ProjectListExists(projectListId);

            // Assert
            Assert.True(result);
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
        public async Task Edit_Post_Should_Redirect_To_Index_When_Model_Is_Valid()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1, Title = "Updated Project" };
            _projectListServiceMock
                .Setup(service => service.Save(projectList))
                .Returns(Task.CompletedTask);  // Simulate successful save

            // Act
            var result = await _controller.Edit(1, projectList) as RedirectToActionResult;

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


    }
}
