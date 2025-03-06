using Kooliprojekt.Data;

namespace KooliProjekt.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Repository-d on nüüd õigesti lisatud
        public UnitOfWork(ApplicationDbContext context,
                          IProjectListRepository projectListRepository)
        {
            _context = context;

            // Initsialiseerime repository-de omadused
            ProjectListRepository = projectListRepository;
        }

        // Repository omadused
        public IProjectListRepository ProjectListRepository { get; private set; }    
        public IProjectItemRepository ProjectItemRepository { get; private set; }
        // Transaktsioonide algatamine, commiteerimine ja rollback
        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
