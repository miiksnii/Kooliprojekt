namespace KooliProjekt.PublicApi.Api
{
    public interface IApiClient
    {
        Task<Result<List<ApiWorkLog>>> List();
        Task<Result> Save(ApiWorkLog list);
        Task<Result<ApiWorkLog>> Get(int id);
        Task Delete(int id);
    }
}