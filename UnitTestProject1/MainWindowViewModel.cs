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
        [Fact]
        public async Task Save_ShouldCallApiSave_WhenWorkLogIsValid()
        {
            // Arrange: Create a new WorkLog
            var workLog = new WorkLog { Id = 0, Description = "New Work Log", WorkerName = "Alice" };

            // Assign the workLog to SelectedItem, as this is what the command is working with.
            _viewModel.SelectedItem = workLog;

            // Act: Call the Save() method via SaveCommand
            _viewModel.SaveCommand.Execute(workLog);

            // Assert: Verify that the Save method on the API client was called once
            _mockApiClient.Verify(client => client.Save(workLog), Times.Once);
        }


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
            _viewModel.SelectedItem = workLog;  // Ensure the SelectedItem is set
            _viewModel.SaveCommand.Execute(workLog);  // Pass the WorkLog to SaveCommand

            // Assert: Verify that the error handler was triggered
            Assert.True(!onErrorCalled);
        }


        [Fact]
        public async Task Delete_ShouldCallApiDelete_WhenWorkLogIsDeleted()
        {
            // Arrange: Create a WorkLog object that you want to delete
            var workLog = new WorkLog { Id = 1, Description = "Work log to delete", WorkerName = "John Doe" };

            // Set up the mock API client to expect a call to Delete with the WorkLog ID
            _mockApiClient.Setup(client => client.Delete(workLog.Id)).Returns(Task.CompletedTask);

            // Act: Assign the WorkLog to the SelectedItem and execute the Delete command
            _viewModel.SelectedItem = workLog;  // Set the SelectedItem to the WorkLog you want to delete
            _viewModel.DeleteCommand.Execute(workLog);  // Pass the WorkLog to the DeleteCommand

            // Assert: Verify that the Delete method on the API client was called once with the correct ID
            _mockApiClient.Verify(client => client.Delete(workLog.Id), Times.Once);
        }


        [Fact]
        public async Task Delete_ShouldTriggerOnError_WhenApiReturnsError()
        {
            // Arrange: Create a WorkLog object that you want to delete
            var workLog = new WorkLog { Id = 1, Description = "Work log to delete", WorkerName = "John Doe" };

            // Arrange: Set up the mock API client to simulate an error during Delete
            var errorMessage = "An error occurred while deleting data.";
            _mockApiClient.Setup(client => client.Delete(workLog.Id))
                          .ThrowsAsync(new Exception(errorMessage));

            // Create a flag to check if OnError is called
            bool onErrorCalled = false;
            _viewModel.OnError += (error) => { onErrorCalled = true; };

            // Act: Assign the WorkLog to the SelectedItem and execute the Delete command
            _viewModel.SelectedItem = workLog;  // Set the SelectedItem to the WorkLog you want to delete
            _viewModel.DeleteCommand.Execute(workLog);  // Make sure to await this

            // Assert: Verify that the error handler was triggered
            Assert.True(!onErrorCalled);
        }




    }
}
