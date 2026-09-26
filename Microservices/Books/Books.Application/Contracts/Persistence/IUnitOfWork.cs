namespace Books.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
