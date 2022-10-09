using Synonims.DataLayer.SynonimRepositories.Models;

namespace Synonims.DataLayer.SynonimRepositories
{
    public interface ISynonimRepository : IRepository<SynonimEntity>
    {
        Task<SynonimEntity> GetSynonimsForWord(string word);
    }
}
