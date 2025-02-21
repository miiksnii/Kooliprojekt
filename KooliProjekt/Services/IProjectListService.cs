using Kooliprojekt.Data;
using Kooliprojekt.Search;
namespace Kooliprojekt.Services
{
    public interface IProjectListService
    {
        Task<PagedResult<ProjectList>> List(int page, int pageSize, ProjectListSearch search = null);
        Task<ProjectList> Get(int id);
        Task Save(ProjectList list);
        Task Delete(int id);
        bool ProjectListExists(int id);
    }
}







