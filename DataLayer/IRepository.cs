namespace Synonims.DataLayer
{
    public interface IRepository<T> where T : Entity<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAllAsync(Predicate<T> predicate);
    }
}
