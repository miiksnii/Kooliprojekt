using Kooliprojekt.Data;
using Kooliprojekt.Search;
using System.Threading.Tasks;

namespace Kooliprojekt.Services
{
    public interface IWorkLogService
    {
        Task<PagedResult<WorkLog>> List(int page, int pageSize, WorkLogSearch search = null);
        Task<WorkLog> Get(int id);
        Task Save(WorkLog log);
        Task Delete(int id);
    }
}