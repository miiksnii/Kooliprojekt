using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.WpfApp.Api;
using Xunit;
using KooliProjekt.WpfApp;

namespace KooliProjekt.UnitTests
{
    public class MainWindowViewModelTests
    {
        private Mock<IApiClient> _mockApiClient;
        private MainWindowViewModel _viewModel;

        // Setup method to initialize tests
        public MainWindowViewModelTests()
        {
            // Create a mock of the IApiClient interface
            _mockApiClient = new Mock<IApiClient>();

            // Initialize the ViewModel with the mocked API client
            _viewModel = new MainWindowViewModel(_mockApiClient.Object);
        }

        // Test if Load() method works when no error occurs
        [Fact]
        public async Task Load_ShouldLoadDataSuccessfully_WhenApiReturnsData()
        {
            // Arrange: Prepare the mock API client to return a list of work logs
            var workLogs = new List<WorkLog>
            {
                new WorkLog { Id = 1, Description = "Work log 1", WorkerName = "John Doe" },
                new WorkLog { Id = 2, Description = "Work log 2", WorkerName = "Jane Smith" }
            };

            _mockApiClient.Setup(client => client.List()).ReturnsAsync(new Result<List<WorkLog>> { Value = workLogs });

            // Act: Call the Load() method in the ViewModel
            await _viewModel.Load();

            // Assert: Verify that the data is loaded correctly
            Assert.Equal(2, _viewModel.Lists.Count);
            Assert.Equal("Work log 1", _viewModel.Lists[0].Description);
            Assert.Equal("Work log 2", _viewModel.Lists[1].Description);
        }

        // Test if Load() method handles errors correctly
        [Fact]
        public async Task Load_ShouldTriggerOnError_WhenApiReturnsError()
        {
            // Arrange: Prepare the mock API client to return an error message
            var errorMessage = "An error occurred while loading data.";
            _mockApiClient.Setup(client => client.List()).ReturnsAsync(new Result<List<WorkLog>> { Error = errorMessage });

            // Create a flag to check if OnError is called
            bool onErrorCalled = false;
            _viewModel.OnError += (error) => { onErrorCalled = true; };

            // Act: Call the Load() method
            await _viewModel.Load();

            // Assert: Verify that the error handler was triggered
            Assert.True(onErrorCalled);
        }

        // Test if Save() method works without errors
        [Fact]
        public async Task Save_ShouldCallApiSave_WhenWorkLogIsValid()
        {
            // Arrange: Create a new WorkLog
            var workLog = new WorkLog { Id = 0, Description = "New Work Log", WorkerName = "Alice" };

            // Act: Call the Save() method
            _viewModel.SaveCommand.Execute(workLog);

            // Assert: Verify that the Save method on the API client was called once
            _mockApiClient.Verify(client => client.Save(workLog), Times.Once);
        }

        // Test if Save() method handles errors correctly
        [Fact]
        public async Task Save_ShouldTriggerOnError_WhenApiReturnsError()
        {
            // Arrange: Prepare the mock API client to simulate an error during Save
            var errorMessage = "An error occurred while saving data.";
            _mockApiClient.Setup(client => client.Save(It.IsAny<WorkLog>())).ThrowsAsync(new Exception(errorMessage));

            // Create a flag to check if OnError is called
            bool onErrorCalled = false;
            _viewModel.OnError += (error) => { onErrorCalled = true; };

            // Act: Call the Save() method
            var workLog = new WorkLog { Id = 0, Description = "New Work Log", WorkerName = "Alice" };
            _viewModel.SaveCommand.Execute(workLog);

            // Assert: Verify that the error handler was triggered
            Assert.True(onErrorCalled);
        }

        // Test if Delete() method works correctly
        [Fact]
        public async Task Delete_ShouldCallApiDelete_WhenWorkLogIsDeleted()
        {
            // Arrange: Set up the mock API client
            var workLogId = 1;
            _mockApiClient.Setup(client => client.Delete(workLogId)).Returns(Task.CompletedTask);

            // Act: Call the Delete() method
            _viewModel.DeleteCommand.Execute(workLogId);

            // Assert: Verify that the Delete method was called on the API client
            _mockApiClient.Verify(client => client.Delete(workLogId), Times.Once);
        }

        // Test if Delete() method handles errors correctly
        [Fact]
        public async Task Delete_ShouldTriggerOnError_WhenApiReturnsError()
        {
            // Arrange: Set up the mock API client to simulate an error during Delete
            var errorMessage = "An error occurred while deleting data.";
            _mockApiClient.Setup(client => client.Delete(It.IsAny<int>())).ThrowsAsync(new Exception(errorMessage));

            // Create a flag to check if OnError is called
            bool onErrorCalled = false;
            _viewModel.OnError += (error) => { onErrorCalled = true; };

            // Act: Call the Delete() method
            _viewModel.DeleteCommand.Execute(1);

            // Assert: Verify that the error handler was triggered
            Assert.True(onErrorCalled);
        }
    }
}
