using Kooliprojekt.Data;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Search;

namespace Kooliprojekt.Services
{
    public class WorkLogService : IWorkLogService
    {
        private readonly ApplicationDbContext _context;

        public WorkLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<WorkLog>> List(int page, int pageSize, WorkLogSearch search = null)
        {
            var query = _context.WorkLog.AsQueryable();

            if (search != null)
            {
                // Keyword search (matches WorkerName or Description)
                if (!string.IsNullOrWhiteSpace(search.Keyword))
                {
                    search.Keyword = search.Keyword.Trim();
                    query = query.Where(log =>
                        log.Description.Contains(search.Keyword) ||
                        log.WorkerName.Contains(search.Keyword)
                    );
                }

                // IsDone is in WorkLogSearch but not in WorkLog.
                // If needed, you could map it to another property (e.g., filter logs with TimeSpentInMinutes > 0).
                // Example (uncomment if needed):
                // if (search.IsDone.HasValue)
                // {
                //     query = query.Where(log => 
                //         search.IsDone.Value ? log.TimeSpentInMinutes > 0 : log.TimeSpentInMinutes == 0);
                // }
            }

            return await query
                .OrderByDescending(log => log.Date) // Keep date sorting
                .GetPagedAsync(page, pageSize);
        }

        public async Task<WorkLog> Get(int id)
        {
            return await _context.WorkLog
                .FirstOrDefaultAsync(log => log.Id == id);
        }

        public async Task Save(WorkLog log)
        {
            if (log.Id == 0)
            {
                _context.WorkLog.Add(log);
            }
            else
            {
                _context.WorkLog.Update(log);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _context.WorkLog
                .Where(log => log.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}