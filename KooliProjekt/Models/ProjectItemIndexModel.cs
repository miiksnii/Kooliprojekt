using Kooliprojekt.Data;
using Kooliprojekt.Search;

namespace KooliProjekt.Models
{
    public class ProjectItemIndexModel
    {
        public ProjectItemSearch Search { get; set; }
        public PagedResult<ProjectItem> Data { get; set; }
    }
}
