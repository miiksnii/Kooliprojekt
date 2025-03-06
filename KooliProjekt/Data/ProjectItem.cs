using KooliProjekt.Data;
using System.ComponentModel.DataAnnotations;

namespace Kooliprojekt.Data
{
    public class ProjectItem : Entity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int EstimatedWorkTime { get; set; }
        [Required]
        public string AdminName { get; set; }
        public string ?Description { get; set; }
        public bool IsDone { get; set; }
        public ProjectList ProjectList { get; set; }
        public int ProjectListId  { get; set; }
        public List<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();

    }   
}
