using Kooliprojekt.Controllers;
<<<<<<< HEAD
using Kooliprojekt.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Moq;
=======
using Microsoft.AspNetCore.Mvc;
using Xunit;
>>>>>>> 70b27eba397d84857bbd0cb5e4abd84079f159fe

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_should_return_index_view()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));
        }
<<<<<<< HEAD

        [Fact]
        public void Privacy_should_return_privacy_view()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Privacy" ||
                        string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public void Error_should_return_error_view_with_error_view_model()
        {
            // Arrange
            var controller = new HomeController();

            // Mock the HttpContext to simulate TraceIdentifier and Activity
            var mockHttpContext = new Mock<HttpContext>();
            var mockTraceIdentifier = "some-trace-identifier";
            mockHttpContext.SetupGet(c => c.TraceIdentifier).Returns(mockTraceIdentifier);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ErrorViewModel>(result.Model);
            var model = result.Model as ErrorViewModel;
            Assert.NotNull(model);
            Assert.Equal(mockTraceIdentifier, model.RequestId); // Ensure RequestId matches the mocked TraceIdentifier
        }
    }
    
}
=======
    }
}
>>>>>>> 70b27eba397d84857bbd0cb5e4abd84079f159fe
