namespace Kooliprojekt.Data
{
    public class ProjectList
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public IList<ProjectItem> Items { get; set; }

        public ProjectList()
        {
            Items = new List<ProjectItem>();
        
        }
    }
}
