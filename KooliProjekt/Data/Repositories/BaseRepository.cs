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
                if (item.Id == 0)
                {
                    // Insert new item
                    DbContext.Set<T>().Add(item);
                }
                else
                {
                    // Update existing item
                    //var entry = DbContext.Entry(item);
                    //entry.State = EntityState.Modified;
                    DbContext.Update(item);
                }

                // Commit changes to the database
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the concurrency exception (optional)
                throw new DbUpdateConcurrencyException("A concurrency conflict occurred while saving the data.", ex);
            }
            catch (Exception ex)
            {
                // Log any other exception (optional)
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