using KooliProjekt.Data.Repositories;
using Kooliprojekt.Data;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Search;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    public async Task<PagedResult<ProjectList>> List(int page, int pageSize, ProjectListSearch search = null)
    {
        /*return await DbContext.ProjectList
            .OrderBy(list => list.Title)
            .GetPagedAsync(page, pageSize);*/
        var query = DbContext.ProjectList.AsQueryable();
        search = search ?? new ProjectListSearch();

        if (!string.IsNullOrWhiteSpace(search.Keyword))
        {
            query = query.Where(list => list.Title.Contains(search.Keyword));
        }

        if (search.IsDone != null)
        {
            query = query.Where(list => list.Items.Any());

            if (search.IsDone.Value)
            {
                query = query.Where(list => list.Items.All(item => item.IsDone));
            }
            else
            {
                query = query.Where(list => list.Items.Any(item => !item.IsDone));
            }
        }

        return await query
            .OrderBy(list => list.Title)
            .GetPagedAsync(page, pageSize);
    }
}
