using Kooliprojekt.Data;


namespace KooliProjekt.Data.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();

        //ITodoListRepository TodoListRepository { get; }
    }
}
