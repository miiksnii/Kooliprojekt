using KooliProjekt.Data;
using System.ComponentModel.DataAnnotations;


namespace Kooliprojekt.Data
{
    public class WorkLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TimeSpentInMinutes { get; set; }
        public string WorkerName { get; set; }
        public string Description { get; set; }
    }
}