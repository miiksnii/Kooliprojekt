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

        public async Task<Result<List<ApiWorkLog>>> List()
        {
            // Proovime API-st saada otse Result<List<ApiWorkLog>> objekti
            try
            {
                // Deserialiseerime JSON-i Result<T> struktuuri
                var apiResult = await _httpClient.GetFromJsonAsync<Result<List<ApiWorkLog>>>("");

                // Kui JSON-i lugemine ebaõnnestus või tagastati null
                if (apiResult == null)
                {
                    return new Result<List<ApiWorkLog>>
                    {
                        Error = "Serveri vastust ei õnnestu töödelda."
                    };
                }

                return apiResult;
            }
            catch (HttpRequestException ex)
            {
                var result = new Result<List<ApiWorkLog>>();
                if (ex.HttpRequestError == HttpRequestError.ConnectionError)
                    result.Error = "Ei saa serveriga ühendust. Palun proovi hiljem uuesti.";
                else
                    result.Error = ex.Message;
                return result;
            }
            catch (Exception ex)
            {
                return new Result<List<ApiWorkLog>> { Error = ex.Message };
            }
        }


        public async Task<Result<ApiWorkLog>> Get(int id)
        {
            var result = new Result<ApiWorkLog>();

            try
            {
                var response = await _httpClient.GetAsync($"{id}"); // ← siit ära "/" + id
                if (response.IsSuccessStatusCode)
                {
                    result.Value = await response.Content.ReadFromJsonAsync<ApiWorkLog>();
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


        public async Task<Result> Save(ApiWorkLog workLog)
        {
            HttpResponseMessage response;

            if (workLog.Id == 0)
            {
                // Create (POST /api/worklogs/)
                response = await _httpClient.PostAsJsonAsync("", workLog);
            }
            else
            {
                // Update (PUT /api/worklogs/{id})
                response = await _httpClient.PutAsJsonAsync(workLog.Id.ToString(), workLog);
            }

            if (!response.IsSuccessStatusCode)
            {
                // Parsime viga-body’st Result-objekti
                return await response.Content.ReadFromJsonAsync<Result>();
            }

            // Edu korral tagastame tühja Result-i (.Error == null)
            return new Result();
        }




        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync(id.ToString()); 
        }
    }
}