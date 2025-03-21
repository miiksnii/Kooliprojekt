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
    public class ProjectListServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IProjectListRepository> _repositoryMock;
        private readonly ProjectListService _projectListService;

        public ProjectListServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IProjectListRepository>();

            _projectListService = new ProjectListService(_repositoryMock.Object);

            _uowMock.SetupGet(r => r.ProjectListRepository)
                    .Returns(_repositoryMock.Object);
        }

        [Fact]
        public async Task List_should_return_list_of_project_lists()
        {
            var results = new List<ProjectList>
        {
            new ProjectList { Id = 1 },
            new ProjectList { Id = 2 }
        };
            var pagedResult = new PagedResult<ProjectList> { Results = results };
            _repositoryMock.Setup(r => r.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProjectListSearch>()))
                           .ReturnsAsync(pagedResult);

            var result = await _projectListService.List(1, 10);

            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_should_return_project_list_by_id()
        {
            var projectList = new ProjectList { Id = 1 };
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>()))
                           .ReturnsAsync(projectList);

            var result = await _projectListService.Get(1);

            Assert.Equal(projectList, result);
        }

        [Fact]
        public async Task Save_should_invoke_save_on_repository()
        {
            var projectList = new ProjectList { Id = 1 };

            await _projectListService.Save(projectList);

            _repositoryMock.Verify(r => r.Save(It.Is<ProjectList>(pl => pl == projectList)), Times.Once);
        }

        [Fact]
        public async Task Delete_should_invoke_delete_on_repository()
        {
            var projectListId = 1;

            await _projectListService.Delete(projectListId);

            _repositoryMock.Verify(r => r.Delete(It.Is<int>(id => id == projectListId)), Times.Once);
        }

        [Fact]
        public void ProjectListExists_Should_Return_True_If_ProjectList_Exists()
        {
            var projectList = new ProjectList { Id = 1 };
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync(projectList);

            var result = _projectListService.ProjectListExists(1);

            Assert.True(result);
        }

        [Fact]
        public void ProjectListExists_Should_Return_False_If_ProjectList_Does_Not_Exist()
        {
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((ProjectList)null);

            var result = _projectListService.ProjectListExists(1);

            Assert.False(result);
        }
    }
}