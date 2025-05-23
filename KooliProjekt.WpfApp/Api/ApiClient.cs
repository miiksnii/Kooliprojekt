using System.Net.Http;
using System.Net.Http.Json;

namespace KooliProjekt.WpfApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7282/api/Worklogs/");
        }

        public async Task<Result<List<WorkLog>>> List()
        {
            var result = new Result<List<WorkLog>>();

            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<WorkLog>>("");
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task Save(WorkLog list)
        {
            try
            {
                if (list.Id == 0)
                {
                    await _httpClient.PostAsJsonAsync("", list);
                }
                else
                {
                    await _httpClient.PutAsJsonAsync(list.Id.ToString(), list);
                }
            }
            catch (Exception ex)
            {
                // Handle the error as needed (e.g., logging)
                Console.WriteLine($"Error in Save: {ex.Message}");
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _httpClient.DeleteAsync(id.ToString());
            }
            catch (Exception ex)
            {
                // Handle the error as needed (e.g., logging)
                Console.WriteLine($"Error in Delete: {ex.Message}");
            }
        }
    }
}
