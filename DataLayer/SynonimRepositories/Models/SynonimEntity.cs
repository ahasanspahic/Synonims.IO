namespace Synonims.DataLayer.SynonimRepositories.Models
{
    public class SynonimEntity : Entity<SynonimEntity>
    {
        public string Keyword { get; set; }   
        public IEnumerable<string> Synonims { get; set; }

    }
}
