using Kooliprojekt.Data;
using Kooliprojekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface IProjectListRepository
    {
        Task<ProjectList> Get(int id);
        Task<PagedResult<ProjectList>> List(int page, int pageSize, ProjectListSearch search = null);
        Task Save(ProjectList item);
        Task Delete(int id);
    }
}