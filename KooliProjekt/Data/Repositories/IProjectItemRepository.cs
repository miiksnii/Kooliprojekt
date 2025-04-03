using Kooliprojekt.Data;
using Kooliprojekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface IProjectItemRepository
    {
        Task<ProjectIList> Get(int id);
        Task<PagedResult<ProjectIList>> List(int page, int pageSize, ProjectItemSearch search = null);
        Task Save(ProjectIList item);
        Task Delete(int id);
    }
}