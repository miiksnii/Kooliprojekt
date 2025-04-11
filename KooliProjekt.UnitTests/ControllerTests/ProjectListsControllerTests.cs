using Kooliprojekt.Controllers;
using Kooliprojekt.Data;
using Kooliprojekt.Models;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ProjectListsControllerTests
    {
        private readonly Mock<IProjectListService> _mockService;
        private readonly ProjectListsController _controller;

        public ProjectListsControllerTests()
        {
            _mockService = new Mock<IProjectListService>();
            _controller = new ProjectListsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_should_return_view_with_model()
        {
            // Arrange
            var expectedModel = new ProjectListIndexModel
            {
                Data = new PagedResult<ProjectList>
                {
                    Results = new List<ProjectList>
            {
                new ProjectList { Id = 1, Title = "Test List 1" },
                new ProjectList { Id = 2, Title = "Test List 2" }
            }
                }
            };

            // Create a default search object or mock it as needed
            var search = new ProjectListSearch(); // Assuming ProjectListSearch exists
                                                  // Or if you need to set search parameters:
                                                  // var search = new ProjectListSearch { SearchString = "test" };

            _mockService.Setup(s => s.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProjectListSearch>()))
                         .ReturnsAsync(expectedModel.Data);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectListIndexModel>(result.Model);
            var model = result.Model as ProjectListIndexModel;
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
        public async Task Details_should_return_notfound_when_project_not_found()
        {
            // Arrange
            _mockService.Setup(s => s.Get(It.IsAny<int>()))  // Added missing parenthesis here
                       .ReturnsAsync((ProjectList)null);

            // Act
            var result = await _controller.Details(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_project_exists()
        {
            // Arrange
            var expectedProject = new ProjectList { Id = 1, Title = "Test List" };
            _mockService.Setup(s => s.Get(1))
                         .ReturnsAsync(expectedProject);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectList>(result.Model);
            var model = result.Model as ProjectList;
            Assert.Equal(expectedProject.Id, model.Id);
            Assert.Equal(expectedProject.Title, model.Title);
        }

        [Fact]
        public void Create_should_return_view()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_should_redirect_to_index_when_model_is_valid()
        {
            // Arrange
            var projectList = new ProjectList { Title = "New List" };

            // Act
            var result = await _controller.Create(projectList) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockService.Verify(s => s.Save(projectList), Times.Once);
        }

        [Fact]
        public async Task Create_should_return_view_with_model_when_model_is_invalid()
        {
            // Arrange
            var projectList = new ProjectList { Title = "" };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.Create(projectList) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectList>(result.Model);
            _mockService.Verify(s => s.Save(It.IsAny<ProjectList>()), Times.Never);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_null()
        {
            // Act
            var result = await _controller.Edit(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_project_not_found()
        {
            // Arrange
            _mockService.Setup(s => s.Get(It.IsAny<int>()))
                         .ReturnsAsync((ProjectList)null);

            // Act
            var result = await _controller.Edit(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_project_exists()
        {
            // Arrange
            var expectedProject = new ProjectList { Id = 1, Title = "Test List" };
            _mockService.Setup(s => s.Get(1))
                         .ReturnsAsync(expectedProject);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectList>(result.Model);
            var model = result.Model as ProjectList;
            Assert.Equal(expectedProject.Id, model.Id);
            Assert.Equal(expectedProject.Title, model.Title);
        }

        [Fact]
        public async Task Edit_should_redirect_to_index_when_model_is_valid()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1, Title = "Updated List" };

            // Act
            var result = await _controller.Edit(1, projectList) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockService.Verify(s => s.Save(projectList), Times.Once);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_model_is_invalid()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1, Title = "" };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.Edit(1, projectList) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectList>(result.Model);
            _mockService.Verify(s => s.Save(It.IsAny<ProjectList>()), Times.Never);
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
        public async Task Delete_should_return_notfound_when_project_not_found()
        {
            // Arrange
            _mockService.Setup(s => s.Get(It.IsAny<int>()))
                         .ReturnsAsync((ProjectList)null);

            // Act
            var result = await _controller.Delete(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_project_exists()
        {
            // Arrange
            var expectedProject = new ProjectList { Id = 1, Title = "Test List" };
            _mockService.Setup(s => s.Get(1))
                         .ReturnsAsync(expectedProject);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProjectList>(result.Model);
            var model = result.Model as ProjectList;
            Assert.Equal(expectedProject.Id, model.Id);
            Assert.Equal(expectedProject.Title, model.Title);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockService.Verify(s => s.DeleteWithDependenciesAsync(1), Times.Once);
        }

        [Fact]
        public void ProjectListExists_should_return_true_when_project_exists()
        {
            // Arrange
            var expectedProject = new ProjectList { Id = 1, Title = "Test List" };
            _mockService.Setup(s => s.Get(1))
                         .ReturnsAsync(expectedProject);

            // Act
            var result = _controller.ProjectListExists(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ProjectListExists_should_return_false_when_project_does_not_exist()
        {
            // Arrange
            _mockService.Setup(s => s.Get(1))
                         .ReturnsAsync((ProjectList)null);

            // Act
            var result = _controller.ProjectListExists(1);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task Edit_POST_should_handle_all_scenarios()
        {
            // Arrange
            var testProject = new ProjectList { Id = 1, Title = "Test Project" };

            // Case 1: ID mismatch (untested path)
            var result1 = await _controller.Edit(2, testProject) as NotFoundResult;
            Assert.NotNull(result1);

            // Case 2: Invalid model state
            _controller.ModelState.AddModelError("Title", "Required");
            var result2 = await _controller.Edit(1, new ProjectList { Id = 1, Title = "" }) as ViewResult;
            Assert.NotNull(result2);
            Assert.False(_controller.ModelState.IsValid);

            // Case 3: Valid edit
            _controller.ModelState.Clear();
            _mockService.Setup(s => s.Save(It.IsAny<ProjectList>()))
                       .Returns(Task.CompletedTask);

            var result3 = await _controller.Edit(1, testProject) as RedirectToActionResult;
            Assert.NotNull(result3);
            Assert.Equal("Index", result3.ActionName);
            _mockService.Verify(s => s.Save(testProject), Times.Once);
        }



    }
}