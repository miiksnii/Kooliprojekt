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

        [Fact]
        public async Task Post_WorkLog_ReturnsCreated_WhenValid()
        {
            var client = Factory.CreateClient();
            var newWorkLog = new
            {
                Name = "Test Work Log",
                Description = "This is a test description",
                ProjectItemId = 1, // Assuming ProjectItem with Id 1 exists
                HoursWorked = 5
            };

            var content = new StringContent(JsonConvert.SerializeObject(newWorkLog), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/WorkLogs", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Check if Location header is set for created resource
            var locationHeader = response.Headers.Location.ToString();
            Assert.Contains("/api/WorkLogs", locationHeader);
        }

        [Fact]
        public async Task Put_WorkLog_ReturnsOk_WhenValid()
        {
            var client = Factory.CreateClient();

            // First, create a WorkLog to update
            var newWorkLog = new
            {
                Name = "Initial Work Log",
                Description = "This is the initial work log",
                ProjectItemId = 1, // Assuming ProjectItem with Id 1 exists
                HoursWorked = 3
            };

            var content = new StringContent(JsonConvert.SerializeObject(newWorkLog), Encoding.UTF8, "application/json");
            var createResponse = await client.PostAsync("/api/WorkLogs", content);

            var createdWorkLog = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
            int createdWorkLogId = createdWorkLog.id;

            // Now update the WorkLog
            var updatedWorkLog = new
            {
                Id = createdWorkLogId,
                Name = "Updated Work Log",
                Description = "This is an updated work log",
                ProjectItemId = 1, // Assuming ProjectItem with Id 1 exists
                HoursWorked = 4
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updatedWorkLog), Encoding.UTF8, "application/json");
            var putResponse = await client.PutAsync($"/api/WorkLogs/{createdWorkLogId}", updateContent);

            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        }
    }
}
