using Kooliprojekt.Search;
using Kooliprojekt.Data;


namespace Kooliprojekt.Models
{
    public class WorkLogIndexModel
    {
        public WorkLogSearch Search { get; set; }
        public PagedResult<WorkLog> Data { get; set; }
    }
}
