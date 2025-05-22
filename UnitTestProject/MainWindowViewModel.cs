using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using KooliProjekt.WpfApp.Api;
using KooliProjekt.WpfApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        // Test if Load() calls OnError when API List() fails
        [TestMethod]
        public async Task Load_Should_Invoke_OnError_When_ApiClient_List_Fails()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            var errorMessage = "API List() failed";
            mockApiClient.Setup(api => api.List())
                         .ReturnsAsync(new Result<List<WorkLog>> { Error = errorMessage });

            var viewModel = new MainWindowViewModel(mockApiClient.Object);

            string receivedError = null;
            viewModel.OnError = (msg) => receivedError = msg;

            // Act
            await viewModel.Load();

            // Assert
            Assert.AreEqual(errorMessage, receivedError, "OnError should be called with the correct error message");
        }

        // Test if Save() calls OnError when Save fails
        [TestMethod]
        public async Task Save_Should_Invoke_OnError_When_ApiClient_Save_Fails()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            var errorMessage = "Save failed";
            mockApiClient.Setup(api => api.Save(It.IsAny<WorkLog>()))
                         .ReturnsAsync(new Result { Error = errorMessage });

            var viewModel = new MainWindowViewModel(mockApiClient.Object);
            string errorReceived = null;
            viewModel.OnError = (msg) => errorReceived = msg;

            // Act
            await viewModel.Save(new WorkLog());

            // Assert
            Assert.AreEqual(errorMessage, errorReceived, "OnError should be called with the correct error message");
        }

        // Test if Delete() calls OnError when Delete fails
        [TestMethod]
        public async Task Delete_Should_Invoke_OnError_When_ApiClient_Delete_Fails()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            var errorMessage = "Delete failed";
            mockApiClient.Setup(api => api.Delete(It.IsAny<int>()))
                         .ReturnsAsync(new Result { Error = errorMessage });

            var viewModel = new MainWindowViewModel(mockApiClient.Object);
            string errorReceived = null;
            viewModel.OnError = (msg) => errorReceived = msg;

            // Act
            await viewModel.Delete(1);

            // Assert
            Assert.AreEqual(errorMessage, errorReceived, "OnError should be called with the correct error message");
        }

        // Test if Load() does NOT call OnError when API List() succeeds
        [TestMethod]
        public async Task Load_Should_Not_Invoke_OnError_When_ApiClient_List_Succeeds()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            mockApiClient.Setup(api => api.List())
                         .ReturnsAsync(new Result<List<WorkLog>> { Value = new List<WorkLog>() });

            var viewModel = new MainWindowViewModel(mockApiClient.Object);
            string errorReceived = null;
            viewModel.OnError = (msg) => errorReceived = msg;

            // Act
            await viewModel.Load();

            // Assert
            Assert.IsNull(errorReceived, "OnError should not be called when the API List() succeeds");
        }

        // Test if Save() does NOT call OnError when Save succeeds
        [TestMethod]
        public async Task Save_Should_Not_Invoke_OnError_When_ApiClient_Save_Succeeds()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            mockApiClient.Setup(api => api.Save(It.IsAny<WorkLog>()))
                         .ReturnsAsync(new Result());

            var viewModel = new MainWindowViewModel(mockApiClient.Object);
            string errorReceived = null;
            viewModel.OnError = (msg) => errorReceived = msg;

            // Act
            await viewModel.Save(new WorkLog());

            // Assert
            Assert.IsNull(errorReceived, "OnError should not be called when the Save is successful");
        }

        // Test if Delete() does NOT call OnError when Delete succeeds
        [TestMethod]
        public async Task Delete_Should_Not_Invoke_OnError_When_ApiClient_Delete_Succeeds()
        {
            // Arrange
            var mockApiClient = new Mock<IApiClient>();
            mockApiClient.Setup(api => api.Delete(It.IsAny<int>()))
                         .ReturnsAsync(new Result());

            var viewModel = new MainWindowViewModel(mockApiClient.Object);
            string errorReceived = null;
            viewModel.OnError = (msg) => errorReceived = msg;

            // Act
            await viewModel.Delete(1);

            // Assert
            Assert.IsNull(errorReceived, "OnError should not be called when Delete is successful");
        }
    }
}
