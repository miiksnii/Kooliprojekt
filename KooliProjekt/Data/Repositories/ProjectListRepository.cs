using Kooliprojekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories {
    public class ProjectListRepository : BaseRepository<ProjectList>, IProjectListRepository
    {
        public ProjectListRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<ProjectList> Get(int id)
        {
            return await DbContext.ProjectList
                .Include(list => list.Items)
                .Where(list => list.Id == id)
                .FirstOrDefaultAsync();
        }

        public override async Task<PagedResult<ProjectList>> List(int page, int pageSize)
        {
            return await DbContext.ProjectList
                .OrderBy(list => list.Title)
                .GetPagedAsync(page, pageSize);
        }
    }
}
