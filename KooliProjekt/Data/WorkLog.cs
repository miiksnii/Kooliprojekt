using KooliProjekt.Data;
using System.ComponentModel.DataAnnotations;

namespace Kooliprojekt.Data
{
    public class WorkLog
    {
        public int Id { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Range(1, 1440)]
        public int? TimeSpentInMinutes { get; set; }

        [StringLength(100)]
        public string? WorkerName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
