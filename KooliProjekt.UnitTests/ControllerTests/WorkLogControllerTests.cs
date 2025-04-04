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
            var expectedModel = new WorkLogIndexModel
            {
                Data = new PagedResult<WorkLog>
                {
                    Results = new List<WorkLog>
                    {
                        new WorkLog { Id = 1, WorkerName = "Worker 1" },
                        new WorkLog { Id = 2, WorkerName = "Worker 2" }
                    }
                }
            };

            _mockWorkLogService.Setup(s => s.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WorkLogSearch>()))
                             .ReturnsAsync(expectedModel.Data);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkLogIndexModel>(result.Model);
            var model = result.Model as WorkLogIndexModel;
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
        public async Task Details_should_return_notfound_when_worklog_not_found()
        {
            // Arrange
            _mockWorkLogService.Setup(s => s.Get(It.IsAny<int>()))
                             .ReturnsAsync((WorkLog)null);

            // Act
            var result = await _controller.Details(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_worklog_exists()
        {
            // Arrange
            var expectedWorkLog = new WorkLog { Id = 1, WorkerName = "Test Worker" };
            _mockWorkLogService.Setup(s => s.Get(1))
                             .ReturnsAsync(expectedWorkLog);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkLog>(result.Model);
            var model = result.Model as WorkLog;
            Assert.Equal(expectedWorkLog.Id, model.Id);
        }

        [Fact]
        public async Task Create_GET_should_return_view_with_selectlist()
        {
            // Arrange
            var projectItems = new PagedResult<ProjectIList>
            {
                Results = new List<ProjectIList>
                {
                    new ProjectIList { Id = 1, Title = "Project 1" }
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
            var invalidWorkLog = new WorkLog { WorkerName = "" }; // Invalid
            var projectItems = new PagedResult<ProjectIList>
            {
                Results = new List<ProjectIList>
                {
                    new ProjectIList { Id = 1, Title = "Project 1" }
                }
            };

            _controller.ModelState.AddModelError("WorkerName", "Required");
            _mockProjectItemService.Setup(s => s.List(1, 100, It.IsAny<ProjectItemSearch>()))
                                 .ReturnsAsync(projectItems);

            // Act
            var result = await _controller.Create(invalidWorkLog) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkLog>(result.Model);
            Assert.NotNull(result.ViewData["ProjectIListId"]);
            _mockWorkLogService.Verify(s => s.Save(It.IsAny<WorkLog>()), Times.Never);
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
        public async Task Edit_GET_should_return_notfound_when_worklog_not_found()
        {
            // Arrange
            _mockWorkLogService.Setup(s => s.Get(It.IsAny<int>()))
                             .ReturnsAsync((WorkLog)null);

            // Act
            var result = await _controller.Edit(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_POST_should_handle_all_scenarios()
        {
            // Arrange
            var testWorkLog = new WorkLog { Id = 1, WorkerName = "Test Worker" };

            // Case 1: ID mismatch
            var result1 = await _controller.Edit(2, testWorkLog) as NotFoundResult;
            Assert.NotNull(result1);

            // Case 2: Invalid model state
            _controller.ModelState.AddModelError("WorkerName", "Required");
            var result2 = await _controller.Edit(1, new WorkLog { Id = 1, WorkerName = "" }) as ViewResult;
            Assert.NotNull(result2);
            Assert.False(_controller.ModelState.IsValid);

            // Case 3: Valid edit
            _controller.ModelState.Clear();
            _mockWorkLogService.Setup(s => s.Save(It.IsAny<WorkLog>()))
                             .Returns(Task.CompletedTask);

            var result3 = await _controller.Edit(1, testWorkLog) as RedirectToActionResult;
            Assert.NotNull(result3);
            Assert.Equal("Index", result3.ActionName);
            _mockWorkLogService.Verify(s => s.Save(testWorkLog), Times.Once);
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
        public async Task Delete_should_return_notfound_when_worklog_not_found()
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
        public async Task Delete_should_return_view_with_model_when_worklog_exists()
        {
            // Arrange
            var expectedWorkLog = new WorkLog { Id = 1, WorkerName = "Test Worker" };
            _mockWorkLogService.Setup(s => s.Get(1))
                             .ReturnsAsync(expectedWorkLog);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkLog>(result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            // Arrange
            _mockWorkLogService.Setup(s => s.Get(1))
                             .ReturnsAsync(new WorkLog());

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockWorkLogService.Verify(s => s.Delete(1), Times.Once);
        }
    }
}