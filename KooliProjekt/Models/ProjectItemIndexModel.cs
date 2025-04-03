using Kooliprojekt.Data;
using Kooliprojekt.Search;

namespace Kooliprojekt.Models
{
    public class ProjectItemIndexModel
    {
        public ProjectItemSearch Search { get; set; }
        public PagedResult<ProjectIList> Data { get; set; }
    }
}
