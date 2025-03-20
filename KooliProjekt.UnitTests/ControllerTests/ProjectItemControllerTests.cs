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
using Kooliprojekt.Search;
using KooliProjekt.Data.Repositories;

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

        // ------------------------------------------------------------
        // Index Tests
        // ------------------------------------------------------------
        [Fact]
        public async Task Index_Should_Return_Default_View_When_Model_Is_Null()
        {
            // Arrange
            var pagedResult = new PagedResult<ProjectItem>
            {
                Results = new List<ProjectItem> { new ProjectItem { Id = 1, Title = "Test Item" } },
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 5,
                RowCount = 1
            };

            _projectItemServiceMock
                .Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProjectItemSearch>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }

        [Fact]
        public async Task Index_Should_Handle_Empty_Search_Results()
        {
            // Arrange
            var pagedResult = new PagedResult<ProjectItem>
            {
                Results = new List<ProjectItem>(),
                CurrentPage = 1,
                PageCount = 0,
                PageSize = 5,
                RowCount = 0
            };

            _projectItemServiceMock
                .Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProjectItemSearch>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Empty((IEnumerable<ProjectItem>)result.Model.Results);
        }



        // ------------------------------------------------------------
        // Details Tests
        // ------------------------------------------------------------
        [Fact]
        public async Task Details_Should_Return_View_With_Valid_ProjectItem()
        {
            // Arrange
            int id = 1;
            var projectItem = new ProjectItem { Id = id, Title = "Valid Item" };

            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync(projectItem);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectItem, result.Model);
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
        public async Task Details_Should_Return_NotFound_When_ProjectItem_Is_Missing()
        {
            // Arrange
            int id = 1;
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync((ProjectItem)null);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_Should_Return_View_With_Model_When_ProjectItem_Exists()
        {
            // Arrange
            int id = 1;
            var projectItem = new ProjectItem { Id = id, Title = "Test Item" };
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync(projectItem);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectItem, result.Model);
        }

        // ------------------------------------------------------------
        // Create Tests
        // ------------------------------------------------------------
        [Fact]
        public async Task Create_Should_Return_View_With_ProjectLists()
        {
            // Arrange
            var projectLists = new PagedResult<ProjectList>
            {
                Results = new List<ProjectList> { new ProjectList { Id = 1, Title = "Test List" } }
            };

            _projectListServiceMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProjectListSearch>)).ReturnsAsync(projectLists);

            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(_controller.ViewBag.ProjectListId);
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

        // ------------------------------------------------------------
        // Edit Tests
        // ------------------------------------------------------------
        [Fact]
        public async Task Edit_Get_Should_Return_NotFound_When_Id_Is_Null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_Get_Should_Return_NotFound_When_ProjectItem_Is_Missing()
        {
            // Arrange
            int id = 1;
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync((ProjectItem)null);

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_Get_Should_Return_View_When_ProjectItem_Exists()
        {
            // Arrange
            int id = 1;
            var projectItem = new ProjectItem { Id = id, Title = "Test Item" };
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync(projectItem);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectItem, result.Model);
        }

        [Fact]
        public async Task Edit_Post_Should_Redirect_To_Index_When_Valid()
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
        public async Task Edit_Post_Should_Return_View_When_Invalid()
        {
            // Arrange
            var projectItem = new ProjectItem { Id = 1 };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.Edit(1, projectItem) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectItem, result.Model);
        }

        // ------------------------------------------------------------
        // Delete Tests
        // ------------------------------------------------------------
        [Fact]
        public async Task Delete_Get_Should_Return_NotFound_When_Id_Is_Null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_Get_Should_Return_NotFound_When_ProjectItem_Is_Missing()
        {
            // Arrange
            int id = 1;
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync((ProjectItem)null);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_Get_Should_Return_View_When_ProjectItem_Exists()
        {
            // Arrange
            int id = 1;
            var projectItem = new ProjectItem { Id = id, Title = "Test Item" };
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync(projectItem);

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectItem, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_Should_Redirect_To_Index()
        {
            // Arrange
            int id = 1;
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync(new ProjectItem { Id = id });

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _projectItemServiceMock.Verify(x => x.Delete(id), Times.Once);
        }

        // ------------------------------------------------------------
        // ProjectItemExists Tests
        // ------------------------------------------------------------
        [Fact]
        public async Task ProjectItemExists_Should_Return_True_When_Exists()
        {
            // Arrange
            int id = 1;
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync(new ProjectItem { Id = id });

            // Act
            var result = await _controller.ProjectItemExists(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ProjectItemExists_Should_Return_False_When_Missing()
        {
            // Arrange
            int id = 1;
            _projectItemServiceMock.Setup(x => x.Get(id)).ReturnsAsync((ProjectItem)null);

            // Act
            var result = await _controller.ProjectItemExists(id);

            // Assert
            Assert.False(result);
        }

    }
}