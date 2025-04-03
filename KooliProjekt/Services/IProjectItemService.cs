using Kooliprojekt.Data;
using Kooliprojekt.Search;

namespace Kooliprojekt.Services
{
    public interface IProjectItemService
    {

        Task<PagedResult<ProjectIList>> List(int page, int pageSize, ProjectItemSearch search = null);
        Task<ProjectIList> Get(int id);
        Task Save(ProjectIList list);
        Task Delete(int id);
    }
}
