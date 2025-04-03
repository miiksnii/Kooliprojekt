using Kooliprojekt.Data;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Search;

namespace Kooliprojekt.Services
{
    public class ProjectItemService : IProjectItemService
    {
        private readonly ApplicationDbContext _ProjectItemService;

        public ProjectItemService(ApplicationDbContext context)
        {
            _ProjectItemService = context;
        }

        public async Task<PagedResult<ProjectIList>> List(int page, int pageSize, ProjectItemSearch search = null)
        {
            var query = _ProjectItemService.ProjectItem.AsQueryable();

            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Keyword))
                {
                    search.Keyword = search.Keyword.Trim();

                    query = query.Where(item =>
                        item.Title.Contains(search.Keyword)
                    );
                }

                if (search.IsDone != null)
                {
                    if (search.IsDone.Value)
                    {
                        query = query.Where(item => item.IsDone);
                    }
                    else
                    {
                        query = query.Where(item => !item.IsDone);
                    }
                }
            }

            return await query
                .OrderBy(item => item.Title)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<ProjectIList> Get(int id)
        {
            return await _ProjectItemService.ProjectItem.FindAsync(id);
        }

        public async Task Save(ProjectIList item)
        {
            if (item.Id == 0)
            {
                _ProjectItemService.ProjectItem.Add(item);
            }
            else
            {
                _ProjectItemService.ProjectItem.Update(item);
            }

            await _ProjectItemService.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _ProjectItemService.ProjectItem
                .Where(item => item.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}