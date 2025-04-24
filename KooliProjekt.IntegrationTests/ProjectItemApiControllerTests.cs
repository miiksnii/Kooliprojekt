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
    public class ProjectItemApiControllerTests : TestBase
    {
        // Test GET: api/ProjectItems
        [Theory]
        [InlineData("/api/ProjectItems")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType!.ToString());
        }

        // Test GET: api/ProjectItems/{id}
        [Fact]
        public async Task Get_ProjectItem_By_Id_ReturnsNotFound_WhenNotExists()
        {
            var client = Factory.CreateClient();
            var response = await client.GetAsync("/api/ProjectItems/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // Test POST: api/ProjectItems
        [Fact]
        public async Task Post_ProjectItem_ReturnsCreated_WhenValid()
        {
            var client = Factory.CreateClient();
            var newProjectItem = new
            {
                Name = "Test Project Item",
                Title = "Project Item Title",
                StartDate = "2025-01-01T00:00:00",  // Format as string for simplicity
                EstimatedWorkTime = 100,
                AdminName = "Admin User"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newProjectItem), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/ProjectItems", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        // Test PUT: api/ProjectItems/{id}
        [Fact]
        public async Task Put_ProjectItem_ReturnsOk_WhenValid()
        {
            var client = Factory.CreateClient();

            // First, create a ProjectItem to update
            var newProjectItem = new
            {
                Name = "Initial Project Item",
                Title = "Initial Title",
                StartDate = "2025-01-01T00:00:00",
                EstimatedWorkTime = 100,
                AdminName = "Admin User"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newProjectItem), Encoding.UTF8, "application/json");
            var createResponse = await client.PostAsync("/api/ProjectItems", content);

            var createdProjectItem = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
            int createdProjectItemId = createdProjectItem.id;

            // Now update the ProjectItem
            var updatedProjectItem = new
            {
                Id = createdProjectItemId,
                Name = "Updated Project Item",
                Title = "Updated Title",
                StartDate = "2025-02-01T00:00:00",
                EstimatedWorkTime = 200,
                AdminName = "Updated Admin"
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updatedProjectItem), Encoding.UTF8, "application/json");
            var putResponse = await client.PutAsync($"/api/ProjectItems/{createdProjectItemId}", updateContent);

            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        }

        // Test DELETE: api/ProjectItems/{id}
        [Fact]
        public async Task Delete_ProjectItem_ReturnsNotFound_WhenNotExists()
        {
            var client = Factory.CreateClient();
            var response = await client.DeleteAsync("/api/ProjectItems/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task Delete_ProjectItem_ReturnsNoContent_WhenDeleted()
        {
            var client = Factory.CreateClient();

            // First, create a ProjectItem to delete
            var newProjectItem = new
            {
                Name = "Project Item to Delete",
                Title = "Title to Delete",
                StartDate = "2025-01-01T00:00:00",
                EstimatedWorkTime = 100,
                AdminName = "Admin User"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newProjectItem), Encoding.UTF8, "application/json");
            var createResponse = await client.PostAsync("/api/ProjectItems", content);

            var createdProjectItem = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
            int createdProjectItemId = createdProjectItem.id;

            // Now delete the ProjectItem
            var deleteResponse = await client.DeleteAsync($"/api/ProjectItems/{createdProjectItemId}");

            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
