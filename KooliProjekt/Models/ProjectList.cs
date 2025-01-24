namespace KooliProjekt.Models
{
    public class ProjectItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool Start { get; set; }
        public bool EstimatedTimeSpend { get; set; }
        public bool Manager { get; set; }
    }
}
