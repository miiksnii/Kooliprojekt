using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.WinFormsApp;
using KooliProjekt.PublicApi.Api;
using Moq;
using Xunit;

// Define the Result class within the correct namespace for testing purposes.
// This class is assumed to be the actual return type of IApiClient.List() based on the error.
namespace KooliProjekt.PublicApi.Api
{
    public class Result<T>
    {
        public T Value { get; }
        // In a real project, this Result class might contain properties for success/failure status or error messages.
        // For the purpose of this test, only the Value property is needed as per the WorkLogPresenter's usage.

        public Result(T value)
        {
            Value = value;
        }
    }
}

namespace Kooliproject.WinFormsApp.UnitTests
{
    public class WorkLogPresenterTests
    {
        [Fact]
        public void Constructor_Should_Set_Presenter_On_View()
        {
            // Arrange
            var mockView = new Mock<IWorkLogView>();
            var mockApiClient = new Mock<IApiClient>();

            // Act
            var presenter = new WorkLogPresenter(mockView.Object, mockApiClient.Object);

            // Assert
            mockView.VerifySet(v => v.Presenter = presenter, Times.Once);
        }

        [Fact]
        public void UpdateView_Should_Clear_And_Set_Default_Values_When_WorkLog_Is_Null()
        {
            // Arrange
            var mockView = new Mock<IWorkLogView>();
            var mockApiClient = new Mock<IApiClient>();
            var presenter = new WorkLogPresenter(mockView.Object, mockApiClient.Object);

            // Act
            presenter.UpdateView(null);

            // Assert
            mockView.VerifySet(v => v.Id = "0", Times.Once);
            mockView.VerifySet(v => v.Date = DateTime.Now.ToString("yyyy-MM-dd"), Times.Once);
            mockView.VerifySet(v => v.TimeSpent = "1", Times.Once);
            mockView.VerifySet(v => v.WorkerName = string.Empty, Times.Once);
            mockView.VerifySet(v => v.Description = string.Empty, Times.Once);
        }

        [Fact]
        public void UpdateView_Should_Populate_View_With_WorkLog_Data_When_WorkLog_Is_Not_Null()
        {
            // Arrange
            var mockView = new Mock<IWorkLogView>();
            var mockApiClient = new Mock<IApiClient>();
            var presenter = new WorkLogPresenter(mockView.Object, mockApiClient.Object);

            var testWorkLog = new ApiWorkLog
            {
                Id = 123,
                Date = new DateTime(2023, 10, 26),
                TimeSpentInMinutes = 60,
                WorkerName = "John Doe",
                Description = "Completed project tasks"
            };

            // Act
            presenter.UpdateView(testWorkLog);

            // Assert
            mockView.VerifySet(v => v.Id = "123", Times.Once);
            mockView.VerifySet(v => v.Date = "2023-10-26", Times.Once);
            mockView.VerifySet(v => v.TimeSpent = "60", Times.Once);
            mockView.VerifySet(v => v.WorkerName = "John Doe", Times.Once);
            mockView.VerifySet(v => v.Description = "Completed project tasks", Times.Once);
        }

        [Fact]
        public void UpdateView_Should_Handle_Null_Properties_In_ApiWorkLog()
        {
            // Arrange
            var mockView = new Mock<IWorkLogView>();
            var mockApiClient = new Mock<IApiClient>();
            var presenter = new WorkLogPresenter(mockView.Object, mockApiClient.Object);

            var testWorkLog = new ApiWorkLog
            {
                Id = 456,
                Date = null, // Null date
                TimeSpentInMinutes = null, // Null time spent
                WorkerName = null, // Null worker name
                Description = null // Null description
            };

            // Act
            presenter.UpdateView(testWorkLog);

            // Assert
            mockView.VerifySet(v => v.Id = "456", Times.Once);
            // When workLog.Date is null, the presenter sets _todoView.Date to null.
            mockView.VerifySet(v => v.Date = null, Times.Once);
            mockView.VerifySet(v => v.TimeSpent = string.Empty, Times.Once);
            mockView.VerifySet(v => v.WorkerName = string.Empty, Times.Once);
            mockView.VerifySet(v => v.Description = string.Empty, Times.Once);
        }
    }
}