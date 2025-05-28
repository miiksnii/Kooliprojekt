using System.Net.Http;
using System.Net.Http.Json;

namespace KooliProjekt.PublicApi.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7282/api/Worklogs");
        }

        public async Task<Result<List<WorkLog>>> List()
        {
            var result = new Result<List<WorkLog>>();

            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<WorkLog>>("");
            }
            catch (HttpRequestException ex)
            {
                if (ex.HttpRequestError == HttpRequestError.ConnectionError)
                {
                    result.Error = "Ei saa serveriga ühendust. Palun proovi hiljem uuesti.";
                }
                else
                {
                    result.Error = ex.Message;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task Save(WorkLog list)
        {
            if(list.Id == 0)
            {
                await _httpClient.PostAsJsonAsync("", list);
            }
            else
            {
                await _httpClient.PutAsJsonAsync("/" + list.Id, list);
            }
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync("/" + id);
        }
    }
}