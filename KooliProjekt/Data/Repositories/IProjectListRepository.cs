using Kooliprojekt.Data;

namespace KooliProjekt.Data.Repositories
{
    public interface IProjectListRepository
    {
        Task<ProjectList> Get(int id);
        Task<PagedResult<ProjectList>> List(int page, int pageSize);
        Task Save(ProjectList item);
        Task Delete(int id);
    }
}