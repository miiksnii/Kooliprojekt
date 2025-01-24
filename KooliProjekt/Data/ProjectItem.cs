namespace Kooliprojekt.Data
{
    public class ProjectItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public int EstimatedWorkTime { get; set; }
        public string AdminName { get; set; }
        public string ?Description { get; set; }
        public bool IsDone { get; set; }
        public ProjectList ProjectList { get; set; }
        public int ProjectListId  { get; set; }
        public List<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();

    }   
}
