using Synonims.DataLayer.SynonimRepositories.Models;

namespace Synonims.DataLayer.SynonimRepositories
{
    public class SynonimMemoryRepository : MemoryRepository<SynonimEntity>, ISynonimRepository
    {
        public async Task<SynonimEntity> GetSynonimsForWord(string word)
        {

            var lookupResult = await Where(x => x.Keyword == word);
            return lookupResult.SingleOrDefault();
        }
    }
}
