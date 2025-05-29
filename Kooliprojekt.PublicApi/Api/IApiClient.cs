namespace KooliProjekt.PublicApi.Api
{
    public interface IApiClient
    {
        Task<Result<List<WorkLog>>> List();
        Task<Result> Save(WorkLog list);
        Task<Result<WorkLog>> Get(int id);
        Task Delete(int id);
    }
}