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
            _httpClient.BaseAddress = new Uri("https://localhost:7282/api/worklogs/");

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

        public async Task<Result<WorkLog>> Get(int id)
        {
            var result = new Result<WorkLog>();

            try
            {
                var response = await _httpClient.GetAsync($"{id}"); // ← siit ära "/" + id
                if (response.IsSuccessStatusCode)
                {
                    result.Value = await response.Content.ReadFromJsonAsync<WorkLog>();
                }
                else
                {
                    result.Error = $"Töölogi ID-ga {id} ei leitud.";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }


        public async Task<Result> Save(WorkLog workLog)
        {
            HttpResponseMessage response;

            if (workLog.Id == 0)
            {
                response = await _httpClient.PostAsJsonAsync("", workLog);
            }
            else
            {
                response = await _httpClient.PutAsJsonAsync(workLog.Id.ToString(), workLog);
            }

            if (!response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Result>();
            }

            return new Result();
        }



        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync(id.ToString()); 
        }
    }
}