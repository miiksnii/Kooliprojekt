using Kooliprojekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Kooliprojekt.Search;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Kooliprojekt.Services;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ProjectListServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IProjectListRepository> _repositoryMock;
        private readonly ProjectItemService _projectListService;

        public ProjectListServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IProjectListRepository>();

            // Pass both the IUnitOfWork and IProjectListRepository mocks to the constructor
            _projectListService = new ProjectListService(_uowMock.Object, _repositoryMock.Object);

            // Set up the ProjectListRepository in the IUnitOfWork mock
            _uowMock.SetupGet(r => r.ProjectListRepository)
                    .Returns(_repositoryMock.Object);
        }


        [Fact]
        public async Task List_should_return_list_of_project_lists()
        {
            // Arrange
            var results = new List<ProjectList>
            {
                new ProjectList { Id = 1 },
                new ProjectList { Id = 2 }
            };
            var pagedResult = new PagedResult<ProjectList> { Results = results };
            _repositoryMock.Setup(r => r.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ProjectListSearch>()))
                           .ReturnsAsync(pagedResult);

            // Act
            var result = await _projectListService.List(1, 10);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_should_return_project_list_by_id()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1 };
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>()))
                           .ReturnsAsync(projectList);

            // Act
            var result = await _projectListService.Get(1);

            // Assert
            Assert.Equal(projectList, result);
        }

        [Fact]
        public async Task Save_should_invoke_save_on_repository()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1 };

            // Act
            await _projectListService.Save(projectList);

            // Assert
            _repositoryMock.Verify(r => r.Save(It.Is<ProjectList>(pl => pl == projectList)), Times.Once);
        }

        [Fact]
        public async Task Delete_should_invoke_delete_on_repository()
        {
            // Arrange
            var projectListId = 1;

            // Act
            await _projectListService.Delete(projectListId);

            // Assert
            _repositoryMock.Verify(r => r.Delete(It.Is<int>(id => id == projectListId)), Times.Once);
        }
        [Fact]
        public void ProjectListExists_Should_Return_True_If_ProjectList_Exists1()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1 };
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync(projectList);

            // Act
            var result = _projectListService.ProjectItemExists(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ProjectListExists_Should_Return_False_If_ProjectList_Does_Not_Exist1()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((ProjectList)null);

            // Act
            var result = _projectListService.ProjectItemExists(1);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void ProjectListExists_Should_Return_True_If_ProjectList_Exists()
        {
            // Arrange
            var projectList = new ProjectList { Id = 1 };
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync(projectList);

            // Act
            var result = _projectListService.ProjectItemExists(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ProjectListExists_Should_Return_False_If_ProjectList_Does_Not_Exist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((ProjectList)null);

            // Act
            var result = _projectListService.ProjectItemExists(1);

            // Assert
            Assert.False(result);
        }
    }
}