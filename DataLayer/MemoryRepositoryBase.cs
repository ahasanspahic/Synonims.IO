namespace Synonims.DataLayer
{
    public abstract class MemoryRepositoryBase<T> : IRepository<T> where T : Entity<T>
    {
        private readonly object _lock = new();
        private readonly HashSet<T> _items = new HashSet<T>(1000);

        public MemoryRepositoryBase()
        {

        }
        /// <summary>
        /// Used to allow consumer to empty memory database for easier testing.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task DeleteAllAsync(Predicate<T> predicate)
        {
            _items.RemoveWhere(predicate);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<T>> Where(Func<T, bool> predicate)
        {
            lock (_lock)
                return Task.FromResult(_items.Where(predicate));
        }

        public Task InsertAsync(T entity)
        {
            lock (_lock)
            {
                if (_items.Any(x => x.Id == entity.Id))
                {
                    throw new ArgumentException("Already existing");
                }
                entity.Version = 1;
                entity.Id = Guid.NewGuid();
                _items.Add(entity);
            }
            return Task.CompletedTask;

            
        }

        public Task UpdateAsync(T entity)
        {
            lock (_lock)
            {
                if (!_items.Any(x => x.Id == entity.Id))
                {
                    throw new ArgumentException("Missing entity");
                }
                entity.Version++;
                _items.RemoveWhere(_x => _x.Id == entity.Id);
                _items.Add(entity);
            }
            return Task.CompletedTask;
        }
    }
}
