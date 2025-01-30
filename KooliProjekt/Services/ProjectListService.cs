using Kooliprojekt.Data;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Search;
namespace Kooliprojekt.Services
{
    public class ProjectListService : IProjectListService
    {
        private readonly ApplicationDbContext _context;
        public ProjectListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int? id)
        {
            await _context.ProjectItem
                .Where(list => list.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<ProjectList> Get(int? id)
        {
            return await _context.ProjectList.FindAsync(id);
        }

        public async Task<PagedResult<ProjectList>> List(int page, int pageSize, ProjectListSearch search = null)
        {
            var query = _context.ProjectList.AsQueryable();

            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Keyword))
                {
                    search.Keyword = search.Keyword.Trim();

                    query = query.Where(list =>
                        list.Title.Contains(search.Keyword) ||
                        list.Items.Any(item => item.Title.Contains(search.Keyword))
                    );
                }

                if (search.IsDone != null)
                {
                    if (search.IsDone.Value)
                    {
                        query = query.Where(list =>
                            list.Items.Any() &&
                            list.Items.All(item => item.IsDone)
                        );
                    }
                    else
                    {
                        query = query.Where(list =>
                            list.Items.Any(item => !item.IsDone)
                        );
                    }
                }
            }

            return await query
                .OrderBy(list => list.Title)
                .GetPagedAsync(page, pageSize);
        }

        public async Task Save(ProjectList list)
        {
            if (list.Id == 0)
            {
                _context.ProjectList.Add(list);
            }
            else
            {
                _context.ProjectList.Update(list);
            }

            await _context.SaveChangesAsync();
        }

    }
}
