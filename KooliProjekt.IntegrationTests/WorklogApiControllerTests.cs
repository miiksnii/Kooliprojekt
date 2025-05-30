using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.IntegrationTests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")] // Prevent tests from running in parallel
    public class WorkLogApiControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/WorkLogs")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType!.ToString());
        }

        [Fact]
        public async Task Get_WorkLog_By_Id_ReturnsNotFound_WhenNotExists()
        {
            var client = Factory.CreateClient();
            var response = await client.GetAsync("/api/WorkLogs/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WorkLog_ReturnsNotFound_WhenNotExists()
        {
            var client = Factory.CreateClient();
            var response = await client.DeleteAsync("/api/WorkLogs/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }       
    }
}
