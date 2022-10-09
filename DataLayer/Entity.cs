namespace Synonims.DataLayer
{
    public abstract class Entity<T> where T : Entity<T>
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
