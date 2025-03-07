using KooliProjekt.Data.Repositories;
using Kooliprojekt.Data;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Search;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class ProjectItemRepository : BaseRepository<ProjectItem>, IProjectItemRepository
{
    public ProjectItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<ProjectItem> Get(int id)
    {
        return await DbContext.ProjectItem
            .Include(list => list.WorkLogs)
            .Where(list => list.Id == id)
            .FirstOrDefaultAsync();
    }

public async Task<PagedResult<ProjectItem>> List(int page, int pageSize, ProjectItemSearch search = null)
{
    var query = DbContext.ProjectItem.AsQueryable();
    search = search ?? new ProjectItemSearch();

    if (!string.IsNullOrWhiteSpace(search.Keyword))
    {
        query = query.Where(list => list.Title.Contains(search.Keyword));
    }

    if (search.IsDone != null)
    {
        query = query.Where(list => list.IsDone == search.IsDone.Value);
    }

    return await query
        .OrderBy(list => list.Title)
        .GetPagedAsync(page, pageSize);
}


}
