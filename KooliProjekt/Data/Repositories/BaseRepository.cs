using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Data;

namespace KooliProjekt.Data.Repositories
{
    public abstract class BaseRepository<T> where T : Entity
    {
        protected ApplicationDbContext DbContext { get; }

        public BaseRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public virtual async Task<T> Get(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<PagedResult<T>> List(int page, int pageSize)
        {
            return await DbContext.Set<T>()
                .OrderByDescending(x => x.Id)
                .GetPagedAsync(page, pageSize);
        }

        public virtual async Task Save(T item)
        {
            try
            {
                // Handle insert (new item)
                if (item.Id == 0)
                {
                    DbContext.Set<T>().Add(item);
                }
                else
                {
                    // Handle update (existing item)
                    var entry = DbContext.Entry(item);
                    entry.State = EntityState.Modified;

                    // Handle concurrency conflict (if applicable)
                    try
                    {
                        await DbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        // Handle concurrency exception (optional: log the error)
                        throw new DbUpdateConcurrencyException("A concurrency conflict occurred while saving the data.", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any other exceptions here
                throw new Exception("An error occurred while saving the item.", ex);
            }
        }


        public virtual async Task Delete(int id)
        {
            await DbContext.Set<T>()
                .Where(item => item.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}