namespace Synonims.DataLayer
{
    public abstract class MemoryRepository<T> : MemoryRepositoryBase<T> where T : Entity<T>
    {
        public MemoryRepository() : base ()
        {

        }
    }
}
