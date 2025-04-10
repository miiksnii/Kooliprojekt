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
    public class WorkLogsControllerTests
    {
        private readonly Mock<IWorkLogService> _mockWorkLogService;
        private readonly Mock<IProjectItemService> _mockProjectItemService;
        private readonly WorkLogsController _controller;

        public WorkLogsControllerTests()
        {
            _mockWorkLogService = new Mock<IWorkLogService>();
            _mockProjectItemService = new Mock<IProjectItemService>();
            _controller = new WorkLogsController(_mockWorkLogService.Object, _mockProjectItemService.Object);
        }

        [Fact]
        public async Task Index_should_return_view_with_model()
        {
            // Arrange
            var expectedData = new PagedResult<WorkLog>
            {
                Results = new List<WorkLog>
                {
                    new WorkLog { Id = 1, WorkerName = "Worker 1", Description = "Task 1" },
                    new WorkLog { Id = 2, WorkerName = "Worker 2", Description = "Task 2" }
                }
            };

            var searchModel = new WorkLogIndexModel();

            _mockWorkLogService.Setup(s => s.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WorkLogSearch>()))
                .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.Index(1, searchModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkLogIndexModel>(result.Model);
            var model = result.Model as WorkLogIndexModel;
            Assert.Equal(expectedData, model.Data);
        }

        [Fact]
        public async Task Create_GET_should_return_view_with_selectlist()
        {
            // Arrange
            var projectItems = new PagedResult<ProjectIList>
            {
                Results = new List<ProjectIList>
                {
                    new ProjectIList { Id = 1, Title = "Item 1" },
                    new ProjectIList { Id = 2, Title = "Item 2" }
                }
            };

            _mockProjectItemService.Setup(s => s.List(1, 100, It.IsAny<ProjectItemSearch>()))
                .ReturnsAsync(projectItems);

            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ProjectIListId"]);
            Assert.IsType<SelectList>(result.ViewData["ProjectIListId"]);
        }

        [Fact]
        public async Task Create_POST_should_repopulate_selectlist_when_model_is_invalid()
        {
            // Arrange
            var workLog = new WorkLog
            {
                WorkerName = "", // Invalid
                Description = "" // Invalid
            };

            var projectItems = new PagedResult<ProjectIList>
            {
                Results = new List<ProjectIList>
                {
                    new ProjectIList { Id = 1, Title = "Item 1" }
                }
            };

            _controller.ModelState.AddModelError("WorkerName", "Worker name is required");
            _mockProjectItemService.Setup(s => s.List(1, 100, It.IsAny<ProjectItemSearch>()))
                .ReturnsAsync(projectItems);

            // Act
            var result = await _controller.Create(workLog) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkLog>(result.Model);
            Assert.NotNull(result.ViewData["ProjectIListId"]);
            _mockWorkLogService.Verify(s => s.Save(It.IsAny<WorkLog>()), Times.Never);
        }

        [Fact]
        public async Task Edit_POST_should_return_notfound_when_id_mismatch()
        {
            // Arrange
            var workLog = new WorkLog { Id = 1 };

            // Act
            var result = await _controller.Edit(2, workLog) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_POST_should_redirect_to_index_when_model_is_valid()
        {
            // Arrange
            var workLog = new WorkLog { Id = 1, WorkerName = "Worker", Description = "Task" };

            // Act
            var result = await _controller.Edit(1, workLog) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockWorkLogService.Verify(s => s.Save(workLog), Times.Once);
        }

        [Fact]
        public async Task Edit_POST_should_repopulate_selectlist_when_model_is_invalid()
        {
            // Arrange
            var workLog = new WorkLog { Id = 1, WorkerName = "" };
            var projectItems = new PagedResult<ProjectIList>
            {
                Results = new List<ProjectIList>
                {
                    new ProjectIList { Id = 1, Title = "Item 1" }
                }
            };

            _controller.ModelState.AddModelError("WorkerName", "Worker name is required");
            _mockProjectItemService.Setup(s => s.List(1, 100, It.IsAny<ProjectItemSearch>()))
                .ReturnsAsync(projectItems);

            // Act
            var result = await _controller.Edit(1, workLog) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkLog>(result.Model);
            Assert.NotNull(result.ViewData["ProjectIListId"]);
            _mockWorkLogService.Verify(s => s.Save(It.IsAny<WorkLog>()), Times.Never);
        }

        [Fact]
        public async Task Delete_GET_should_return_notfound_when_id_is_null()
        {
            // Act
            var result = await _controller.Delete(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_GET_should_return_notfound_when_worklog_not_found()
        {
            // Arrange
            _mockWorkLogService.Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync((WorkLog)null);

            // Act
            var result = await _controller.Delete(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_GET_should_return_view_with_model_when_worklog_exists()
        {
            // Arrange
            var workLog = new WorkLog { Id = 1, WorkerName = "Worker", Description = "Task" };
            _mockWorkLogService.Setup(s => s.Get(1))
                .ReturnsAsync(workLog);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkLog>(result.Model);
            Assert.Equal(workLog, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockWorkLogService.Verify(s => s.Delete(1), Times.Once);
        }
    }
}