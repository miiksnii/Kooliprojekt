using Kooliprojekt.Controllers;
using Kooliprojekt.Data;
using Kooliprojekt.Models;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ProjectItemsControllerTests
    {
        private readonly Mock<IProjectListService> _mockListService;
        private readonly Mock<IProjectItemService> _mockItemService;
        private readonly ProjectItemsController _controller;

        public ProjectItemsControllerTests()
        {
            _mockListService = new Mock<IProjectListService>();
            _mockItemService = new Mock<IProjectItemService>();
            _controller = new ProjectItemsController(_mockListService.Object, _mockItemService.Object);
        }

        [Fact]
        public async Task Index_should_return_view_with_model()
        {
            // Arrange
            var expectedModel = new ProjectItemIndexModel
            {
                Data = new PagedResult<ProjectIList>
                {
                    Results = new List<ProjectIList>
                    {
                        new ProjectIList { Id = 1, Title = "Item 1" },
                        new ProjectIList { Id = 2, Title = "Item 2" }
                    }
                }
            };

            _mockItemService.Setup(s => s.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProjectItemSearch>()))
                           .ReturnsAsync(expectedModel.Data);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectItemIndexModel>(result.Model);
            var model = result.Model as ProjectItemIndexModel;
            Assert.Equal(2, model.Data.Results.Count);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_null()
        {
            // Act
            var result = await _controller.Details(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_item_not_found()
        {
            // Arrange
            _mockItemService.Setup(s => s.Get(It.IsAny<int>()))
                          .ReturnsAsync((ProjectIList)null);

            // Act
            var result = await _controller.Details(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_item_exists()
        {
            // Arrange
            var expectedItem = new ProjectIList { Id = 1, Title = "Test Item" };
            _mockItemService.Setup(s => s.Get(1))
                          .ReturnsAsync(expectedItem);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectIList>(result.Model);
            var model = result.Model as ProjectIList;
            Assert.Equal(expectedItem.Id, model.Id);
        }

        [Fact]
        public async Task Create_GET_should_return_view_with_selectlist()
        {
            // Arrange
            var projectLists = new PagedResult<ProjectList>
            {
                Results = new List<ProjectList>
        {
            new ProjectList { Id = 1, Title = "List 1" }
        }
            };

            // Explicitly provide all parameters (page, pageSize, search)
            _mockListService.Setup(s => s.List(1, 100, It.IsAny<ProjectListSearch>()))
                          .ReturnsAsync(projectLists);

            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ProjectListId"]);
            Assert.IsType<SelectList>(result.ViewData["ProjectListId"]);
        }

        [Fact]
        public async Task Create_POST_should_redirect_to_index_when_model_is_valid()
        {
            // Arrange
            var projectItem = new ProjectIList { Title = "Valid Item" };

            // Act
            var result = await _controller.Create(projectItem) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockItemService.Verify(s => s.Save(projectItem), Times.Once);
        }


        [Fact]
        public async Task Edit_GET_should_return_notfound_when_id_is_null()
        {
            // Act
            var result = await _controller.Edit(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_GET_should_return_notfound_when_item_not_found()
        {
            // Arrange
            _mockItemService.Setup(s => s.Get(It.IsAny<int>()))
                          .ReturnsAsync((ProjectIList)null);

            // Act
            var result = await _controller.Edit(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_GET_should_return_view_with_model_and_selectlist()
        {
            // Arrange
            var expectedItem = new ProjectIList { Id = 1, Title = "Test Item", ProjectListId = 1 };
            var projectLists = new PagedResult<ProjectList>
            {
                Results = new List<ProjectList>
        {
            new ProjectList { Id = 1, Title = "List 1" }
        }
            };

            _mockItemService.Setup(s => s.Get(1))
                          .ReturnsAsync(expectedItem);
            // Explicitly provide all parameters (page, pageSize, search)
            _mockListService.Setup(s => s.List(1, 100, It.IsAny<ProjectListSearch>()))
                          .ReturnsAsync(projectLists);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectIList>(result.Model);
            Assert.NotNull(result.ViewData["ProjectListId"]);
        }

        [Fact]
        public async Task Edit_POST_should_handle_all_scenarios()
        {
            // Arrange
            var testItem = new ProjectIList { Id = 1, Title = "Test Item" };

            // Case 1: ID mismatch
            var result1 = await _controller.Edit(2, testItem) as NotFoundResult;
            Assert.NotNull(result1);

            // Case 2: Invalid model state
            _controller.ModelState.AddModelError("Title", "Required");
            var result2 = await _controller.Edit(1, new ProjectIList { Id = 1, Title = "" }) as ViewResult;
            Assert.NotNull(result2);
            Assert.False(_controller.ModelState.IsValid);

            // Case 3: Valid edit
            _controller.ModelState.Clear();
            _mockItemService.Setup(s => s.Save(It.IsAny<ProjectIList>()))
                          .Returns(Task.CompletedTask);

            var result3 = await _controller.Edit(1, testItem) as RedirectToActionResult;
            Assert.NotNull(result3);
            Assert.Equal("Index", result3.ActionName);
            _mockItemService.Verify(s => s.Save(testItem), Times.Once);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            // Act
            var result = await _controller.Delete(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_item_not_found()
        {
            // Arrange
            _mockItemService.Setup(s => s.Get(It.IsAny<int>()))
                          .ReturnsAsync((ProjectIList)null);

            // Act
            var result = await _controller.Delete(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_item_exists()
        {
            // Arrange
            var expectedItem = new ProjectIList { Id = 1, Title = "Test Item" };
            _mockItemService.Setup(s => s.Get(1))
                          .ReturnsAsync(expectedItem);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectIList>(result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            // Arrange
            _mockItemService.Setup(s => s.Get(1))
                          .ReturnsAsync(new ProjectIList());

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockItemService.Verify(s => s.Delete(1), Times.Once);
        }

        [Fact]
        public async Task ProjectItemExists_should_return_true_when_item_exists()
        {
            // Arrange
            _mockItemService.Setup(s => s.Get(1))
                          .ReturnsAsync(new ProjectIList());

            // Act
            var result = await _controller.ProjectItemExists(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ProjectItemExists_should_return_false_when_item_does_not_exist()
        {
            // Arrange
            _mockItemService.Setup(s => s.Get(1))
                          .ReturnsAsync((ProjectIList)null);

            // Act
            var result = await _controller.ProjectItemExists(1);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task Create_POST_should_repopulate_selectlist_when_model_is_invalid()
        {
            // Arrange
            var invalidItem = new ProjectIList
            {
                Title = "", // Invalid - required field
                Name = "",  // Invalid - required field
                StartDate = default, // Invalid - required field
                EstimatedWorkTime = 0, // Invalid - required field
                AdminName = "" // Invalid - required field
            };

            var projectLists = new PagedResult<ProjectList>
            {
                Results = new List<ProjectList>
        {
            new ProjectList { Id = 1, Title = "List 1" }
        }
            };

            // Force model state to be invalid
            _controller.ModelState.AddModelError("Title", "Title is required");
            _controller.ModelState.AddModelError("Name", "Name is required");
            _controller.ModelState.AddModelError("StartDate", "Start date is required");
            _controller.ModelState.AddModelError("EstimatedWorkTime", "Work time is required");
            _controller.ModelState.AddModelError("AdminName", "Admin name is required");

            _mockListService.Setup(s => s.List(1, 100, It.IsAny<ProjectListSearch>()))
                          .ReturnsAsync(projectLists);

            // Act
            var result = await _controller.Create(invalidItem) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectIList>(result.Model);

            // Verify SelectList was repopulated
            var selectList = result.ViewData["ProjectListId"] as SelectList;
            Assert.NotNull(selectList);
            Assert.Single(selectList); // Should contain our single test list

            // Verify service was never called to save invalid data
            _mockItemService.Verify(s => s.Save(It.IsAny<ProjectIList>()), Times.Never);
        }
    }
}