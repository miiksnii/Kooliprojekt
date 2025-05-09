using System.Net.Http;
using System.Net.Http.Json;

namespace KooliProjekt.WpfApp.Api
{
    public class WorklogApiClient : IWorklogApiClient
    {
        private readonly HttpClient _httpClient;

        public WorklogApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");
        }

        public async Task<List<WorkLog>> List()
        {
            var result = await _httpClient.GetFromJsonAsync<List<WorkLog>>("TodoLists");

            // Ensure result is not null by providing a default empty list if result is null
            return result ?? new List<WorkLog>();
        }


        public async Task Save(WorkLog list)
        {
            if(list.Id == 0)
            {
                await _httpClient.PostAsJsonAsync("TodoLists", list);
            }
            else
            {
                await _httpClient.PutAsJsonAsync("TodoLists/" + list.Id, list);
            }
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync("TodoLists/" + id);
        }
    }
}