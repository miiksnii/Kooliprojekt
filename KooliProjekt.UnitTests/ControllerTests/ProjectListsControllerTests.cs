using Kooliprojekt.Controllers;
using Kooliprojekt.Data;
using Kooliprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ProjectListsControllerTests
    {
        private readonly Mock<IProjectListService> _projectlistsServiceMock;
        private readonly ProjectListsController _controller;

        public ProjectListsControllerTests()
        {
            _projectlistsServiceMock = new Mock<IProjectListService>();
            _controller = new ProjectListsController(_projectlistsServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<ProjectList>
            {
                new ProjectList { Id = 1, Title = "Test 1" },
                new ProjectList { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<ProjectList> { Results = data };
            _projectlistsServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}
