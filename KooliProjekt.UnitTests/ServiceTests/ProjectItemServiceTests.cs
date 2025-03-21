using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Services;

namespace KooliProjekt.Tests.Services
{
    public class ProjectItemServiceTests
    {
        private readonly Mock<IProjectItemRepository> _repositoryMock;
        private readonly ProjectItemService _projectItemService;

        public ProjectItemServiceTests()
        {
            _repositoryMock = new Mock<IProjectItemRepository>();
            _projectItemService = new ProjectItemService(null, _repositoryMock.Object);
        }

        [Fact]
        public async Task List_Should_Return_List_Of_ProjectItems()
        {
            var results = new List<ProjectItem>
            {
                new ProjectItem { Id = 1, Title = "Item 1" },
                new ProjectItem { Id = 2, Title = "Item 2" }
            };
            var pagedResult = new PagedResult<ProjectItem> { Results = results };
            _repositoryMock.Setup(r => r.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProjectItemSearch>()))
                           .ReturnsAsync(pagedResult);

            var result = await _projectItemService.List(1, 10);

            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_Should_Return_ProjectItem_By_Id()
        {
            var projectItem = new ProjectItem { Id = 1, Title = "Test Item" };
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>()))
                           .ReturnsAsync(projectItem);

            var result = await _projectItemService.Get(1);

            Assert.Equal(projectItem, result);
        }

        [Fact]
        public async Task Save_Should_Invoke_Save_On_Repository()
        {
            var projectItem = new ProjectItem { Id = 1, Title = "New Item" };

            await _projectItemService.Save(projectItem);

            _repositoryMock.Verify(r => r.Save(It.Is<ProjectItem>(item => item == projectItem)), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Invoke_Delete_On_Repository()
        {
            var projectItemId = 1;

            await _projectItemService.Delete(projectItemId);

            _repositoryMock.Verify(r => r.Delete(It.Is<int>(id => id == projectItemId)), Times.Once);
        }

        [Fact]
        public void ProjectItemExists_Should_Return_True_If_ProjectItem_Exists()
        {
            var projectItem = new ProjectItem { Id = 1, Title = "Existing Item" };
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync(projectItem);

            var result = _projectItemService.ProjectItemExists(1);

            Assert.True(result);
        }

        [Fact]
        public void ProjectItemExists_Should_Return_False_If_ProjectItem_Does_Not_Exist()
        {
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((ProjectItem)null);

            var result = _projectItemService.ProjectItemExists(1);

            Assert.False(result);
        }
    }
}
