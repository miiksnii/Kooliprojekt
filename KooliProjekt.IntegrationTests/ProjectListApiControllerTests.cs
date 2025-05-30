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
    public class ProjectListApiControllerTests : TestBase
    {
        // Test GET: api/ProjectList
        [Theory]
        [InlineData("/api/ProjectList")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType!.ToString());
        }

        // Test GET: api/ProjectList/{id}
        [Fact]
        public async Task Get_ProjectList_By_Id_ReturnsNotFound_WhenNotExists()
        {
            var client = Factory.CreateClient();
            var response = await client.GetAsync("/api/ProjectList/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // Test POST: api/ProjectList
        [Fact]
        public async Task Post_ProjectList_ReturnsCreated_WhenValid()
        {
            var client = Factory.CreateClient();
            var newProjectList = new
            {
                Name = "Test Project List",
                Title = "Project List Title",
                StartDate = "2025-01-01T00:00:00", // Format as string for simplicity
                EstimatedWorkTime = 100,
                AdminName = "Admin User"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newProjectList), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/ProjectList", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Test PUT: api/ProjectList/{id}
        [Fact]
        public async Task Put_ProjectList_ReturnsOk_WhenValid()
        {
            var client = Factory.CreateClient();

            // First, create a ProjectList to update
            var newProjectList = new
            {
                Name = "Initial Project List",
                Title = "Initial Title",
                StartDate = "2025-01-01T00:00:00",
                EstimatedWorkTime = 100,
                AdminName = "Admin User"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newProjectList), Encoding.UTF8, "application/json");
            var createResponse = await client.PostAsync("/api/ProjectList", content);

            var createdProjectList = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
            int createdProjectListId = createdProjectList.id;

            // Now update the ProjectList
            var updatedProjectList = new
            {
                Id = createdProjectListId,
                Name = "Updated Project List",
                Title = "Updated Title",
                StartDate = "2025-02-01T00:00:00",
                EstimatedWorkTime = 200,
                AdminName = "Updated Admin"
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updatedProjectList), Encoding.UTF8, "application/json");
            var putResponse = await client.PutAsync($"/api/ProjectList/{createdProjectListId}", updateContent);

            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        }

        // Test DELETE: api/ProjectList/{id}
        [Fact]
        public async Task Delete_ProjectList_ReturnsNotFound_WhenNotExists()
        {
            var client = Factory.CreateClient();
            var response = await client.DeleteAsync("/api/ProjectList/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
