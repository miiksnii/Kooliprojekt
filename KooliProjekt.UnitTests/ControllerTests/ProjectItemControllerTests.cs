using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Xunit;
using Kooliprojekt.Controllers;
using Kooliprojekt.Data;
using Kooliprojekt.Services;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ProjectItemControllerTests
    {
        private readonly Mock<IProjectItemService> _projectItemServiceMock;
        private readonly Mock<IProjectListService> _projectListServiceMock;
        private readonly ProjectItemsController _controller;

        public ProjectItemControllerTests()
        {
            _projectItemServiceMock = new Mock<IProjectItemService>();
            _projectListServiceMock = new Mock<IProjectListService>();
            _controller = new ProjectItemsController(_projectListServiceMock.Object, _projectItemServiceMock.Object);
        }




        [Fact]
        public async Task Index_Should_Return_View_With_Model()
        {
            // Arrange
            _projectItemServiceMock.Setup(x => x.List(1, 5, null)).ReturnsAsync(new List<ProjectItem> { new ProjectItem { Id = 1, Title = "Test Item" } });

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<List<ProjectItem>>(result.Model);
        }

        [Fact]
        public async Task Create_Get_Should_Return_View_With_ProjectLists()
        {
            // Arrange
            _projectListServiceMock.Setup(x => x.List(1, 100)).ReturnsAsync(new PagedResult<ProjectList> { Results = new List<ProjectList> { new ProjectList { Id = 1, Title = "Test List" } } });

            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ProjectListId"]);
        }

        [Fact]
        public async Task Create_Post_Should_Redirect_To_Index_When_Model_Is_Valid()
        {
            // Arrange
            var projectItem = new ProjectItem { Id = 1, Title = "Test Item" };

            // Act
            var result = await _controller.Create(projectItem) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _projectItemServiceMock.Verify(x => x.Save(projectItem), Times.Once);
        }

        [Fact]
        public async Task Edit_Get_Should_Return_View_When_ProjectItem_Exists()
        {
            // Arrange
            var projectItem = new ProjectItem { Id = 1, Title = "Test Item" };
            _projectItemServiceMock.Setup(x => x.Get(1)).ReturnsAsync(projectItem);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectItem, result.Model);
        }

        [Fact]
        public async Task Edit_Post_Should_Redirect_To_Index_When_Model_Is_Valid()
        {
            // Arrange
            var projectItem = new ProjectItem { Id = 1, Title = "Updated Item" };

            // Act
            var result = await _controller.Edit(1, projectItem) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _projectItemServiceMock.Verify(x => x.Save(projectItem), Times.Once);
        }

        [Fact]
        public async Task Delete_Get_Should_Return_View_When_ProjectItem_Exists()
        {
            // Arrange
            var projectItem = new ProjectItem { Id = 1, Title = "Test Item" };
            _projectItemServiceMock.Setup(x => x.Get(1)).ReturnsAsync(projectItem);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectItem, result.Model);
        }

        [Fact]
        public async Task Delete_Post_Should_Redirect_To_Index_After_Deletion()
        {
            // Arrange
            var projectItem = new ProjectItem { Id = 1, Title = "Test Item" };
            _projectItemServiceMock.Setup(x => x.Get(1)).ReturnsAsync(projectItem);

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _projectItemServiceMock.Verify(x => x.Delete(1), Times.Once);
        }

        [Fact]
        public async Task ProjectItemExists_Should_Return_True_If_Exists()
        {
            // Arrange
            _projectItemServiceMock.Setup(x => x.Get(1)).ReturnsAsync(new ProjectItem { Id = 1 });

            // Act
            var result = await _controller.ProjectItemExists(1);

            // Assert
            Assert.True(result);
        }
    }
}
