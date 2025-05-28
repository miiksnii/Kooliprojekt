using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.WinFormsApp;
using KooliProjekt.PublicApi.Api;
using Moq;
using Xunit;

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
        public void UpdateView_WithNull_Should_Clear_View()
        {
            // Arrange
            var mockView = new Mock<IWorkLogView>();
            var mockApiClient = new Mock<IApiClient>();
            var presenter = new WorkLogPresenter(mockView.Object, mockApiClient.Object);

            // Act
            presenter.UpdateView(null);

            // Assert
            mockView.VerifySet(v => v.Title = "", Times.Once);
            mockView.VerifySet(v => v.Id = 0, Times.Once);
        }

        [Fact]
        public void UpdateView_WithNonNull_Should_Set_View_Properties()
        {
            // Arrange
            var mockView = new Mock<IWorkLogView>();
            var mockApiClient = new Mock<IApiClient>();
            var presenter = new WorkLogPresenter(mockView.Object, mockApiClient.Object);

            var someView = new Mock<IWorkLogView>();
            someView.SetupGet(v => v.Id).Returns(42);
            someView.SetupGet(v => v.Title).Returns("Test Title");

            // Act
            presenter.UpdateView(someView.Object);

            // Assert
            mockView.VerifySet(v => v.Id = 42, Times.Once);
            mockView.VerifySet(v => v.Title = "Test Title", Times.Once);
        }

    }
}
