namespace KooliProjekt.WpfApp.Api
{
    public interface IWorklogApiClient
    {
        Task<List<WorkLog>> List();
        Task Save(WorkLog list);
        Task Delete(int id);
    }
}