using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.WpfApp;
using KooliProjekt.WpfApp.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        [Fact]
        public async Task Load_PopulatesLists_WhenApiReturnsData()
        {
            // Arrange
            var mockClient = new Mock<IApiClient>();
            var sampleData = new List<WorkLog> { new WorkLog { Id = 1, Description = "Test" } };
            mockClient.Setup(c => c.List()).ReturnsAsync(new Result<List<WorkLog>> { Value = sampleData });

            var viewModel = new MainWindowViewModel(mockClient.Object);

            // Act
            await viewModel.Load();

            // Assert
            Assert.Single(viewModel.Lists);
            Assert.Equal("Test", viewModel.Lists[0].Description);
        }

        [Fact]
        public async Task Load_CallsOnError_WhenApiReturnsError()
        {
            // Arrange
            var mockClient = new Mock<IApiClient>();
            mockClient.Setup(c => c.List()).ReturnsAsync(new Result<List<WorkLog>> { Error = "Some error" });

            var viewModel = new MainWindowViewModel(mockClient.Object);

            string capturedError = null;
            viewModel.OnError = (err) => capturedError = err;

            // Act
            await viewModel.Load();

            // Assert
            Assert.Equal("Some error", capturedError);
        }

        [Fact]
        public async Task SaveCommand_CallsSaveAndReload_WhenExecuted()
        {
            // Arrange
            var mockClient = new Mock<IApiClient>();
            var viewModel = new MainWindowViewModel(mockClient.Object);

            viewModel.SelectedItem = new WorkLog { Id = 1, Description = "Save test" };

            mockClient.Setup(c => c.Save(It.IsAny<WorkLog>())).Returns(Task.CompletedTask);
            mockClient.Setup(c => c.List()).ReturnsAsync(new Result<List<WorkLog>> { Value = new List<WorkLog>() });

            // Act
            viewModel.SaveCommand.Execute(null);
            await Task.Delay(50); // Allow async code in command to execute

            // Assert
            mockClient.Verify(c => c.Save(It.IsAny<WorkLog>()), Times.Once);
            mockClient.Verify(c => c.List(), Times.Once);
        }

        [Fact]
        public async Task DeleteCommand_DoesNothing_WhenConfirmDeleteReturnsFalse()
        {
            // Arrange
            var mockClient = new Mock<IApiClient>();
            var viewModel = new MainWindowViewModel(mockClient.Object);
            var workLog = new WorkLog { Id = 123 };
            viewModel.SelectedItem = workLog;
            viewModel.Lists.Add(workLog);

            viewModel.ConfirmDelete = _ => false;

            // Act
            viewModel.DeleteCommand.Execute(null);
            await Task.Delay(50); // Allow async code in command to execute

            // Assert
            mockClient.Verify(c => c.Delete(It.IsAny<int>()), Times.Never);
            Assert.Single(viewModel.Lists); // still there
        }

        [Fact]
        public async Task DeleteCommand_RemovesItem_WhenConfirmed()
        {
            // Arrange
            var mockClient = new Mock<IApiClient>();
            var viewModel = new MainWindowViewModel(mockClient.Object);
            var workLog = new WorkLog { Id = 123 };
            viewModel.SelectedItem = workLog;
            viewModel.Lists.Add(workLog);

            viewModel.ConfirmDelete = _ => true;
            mockClient.Setup(c => c.Delete(workLog.Id)).Returns(Task.CompletedTask);

            // Act
            viewModel.DeleteCommand.Execute(null);
            await Task.Delay(50); // Allow async code in command to execute

            // Assert
            mockClient.Verify(c => c.Delete(workLog.Id), Times.Once);
            Assert.Empty(viewModel.Lists);
            Assert.Null(viewModel.SelectedItem);
        }

        [Fact]
        public void NewCommand_CreatesNewSelectedItem()
        {
            // Arrange
            var viewModel = new MainWindowViewModel(new Mock<IApiClient>().Object);

            // Act
            viewModel.NewCommand.Execute(null);

            // Assert
            Assert.NotNull(viewModel.SelectedItem);
        }
    }
}
