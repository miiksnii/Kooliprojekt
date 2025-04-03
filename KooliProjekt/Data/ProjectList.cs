using KooliProjekt.Data;
using System.ComponentModel.DataAnnotations;

namespace Kooliprojekt.Data
{
    public class ProjectList : Entity
    {
        public string Title { get; set; }
        public IList<ProjectIList> Items { get; set; }

        public ProjectList()
        {
            Items = new List<ProjectIList>();
        }
    }
}
