using Kooliprojekt.Data;
using Kooliprojekt.Search;

namespace Kooliprojekt.Services
{
    public interface IProjectItemService
    {

        Task<PagedResult<ProjectItem>> List(int page, int pageSize, ProjectItemSearch search = null);
        Task<ProjectItem> Get(int id);
        Task Save(ProjectItem list);
        Task Delete(int id);
    }
}
