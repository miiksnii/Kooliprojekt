namespace Kooliprojekt.Data
{
    public class WorkLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TimeSpentInMinutes { get; set; } // Time spent in minutes
        public string WorkerName { get; set; } // The person who did the work
        public string Description { get; set; } // Activity description
    }
}
