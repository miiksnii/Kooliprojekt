using Kooliprojekt.Data;
using Kooliprojekt.Search;

namespace KooliProjekt.Data.Repositories
{
    public interface IProjectItemRepository
    {
        Task<ProjectItem> Get(int id);
        Task<PagedResult<ProjectItem>> List(int page, int pageSize, ProjectItemSearch search = null);
        Task Save(ProjectItem item);
        Task Delete(int id);
    }
}