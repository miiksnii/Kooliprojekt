using Kooliprojekt.Search;
using Kooliprojekt.Data;


namespace Kooliprojekt.Models
{
    public class ProjectListIndexModel
    {
        public ProjectListSearch Search { get; set; }
        public PagedResult<ProjectList> Data { get; set; }
    }
}
