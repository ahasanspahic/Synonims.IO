using Synonims.DataLayer.SynonimRepositories;
using Synonims.DataLayer.SynonimRepositories.Models;
using Synonims.Services.Synonims.Models;

namespace Synonims.Services.Synonims
{
    public class SynonimService : ISynonimService
    {

        private ISynonimRepository _synonimyRepository;

        public SynonimService(ISynonimRepository synonimRepository)
        {
            _synonimyRepository = synonimRepository;
        }

        public async Task<SynonimModel?> GetSynonimsForWord(string word)
        {
            var synonims = await _synonimyRepository.GetSynonimsForWord(word);
            
            var result =  SynonimModel.FromEntity(synonims);
            if (result == null)
                return null;

            var trainsentSynonims = new List<SynonimModel>();
            foreach(var synonim in result!.Synonims)
            {
                var temp = await _synonimyRepository.GetSynonimsForWord(synonim);
                trainsentSynonims.Add(SynonimModel.FromEntity(temp));
            }
            result.TransientSynoninms = trainsentSynonims;
            return result;
        }

        public async Task AddSynonim(string keyword, string linkedWord)
        {
            // To make sure we add both words transiently, add them in as requested
            await AddSynonimToDatabase(keyword, linkedWord);
            // and then add them in reverse order to respect the transient rule
            await AddSynonimToDatabase(linkedWord, keyword);
        }

        public async Task EmptyColletion()
        {
            await _synonimyRepository.DeleteAllAsync(x => x.Version > 0);
        }


        private async Task AddSynonimToDatabase(string keyword, string linkedWord)
        {
            var existingKeyword = await _synonimyRepository.GetSynonimsForWord(keyword);
            if (existingKeyword == null)
            {
                var entity = new SynonimEntity()
                {
                    Keyword = keyword,
                    Synonims = new List<string>() { linkedWord.ToLower() }
                };
                await _synonimyRepository.InsertAsync(entity);
            }
            else
            {
                if (existingKeyword.Synonims.Contains(linkedWord.ToLower()))
                    return;
                var synonims = existingKeyword.Synonims.ToList();
                existingKeyword.Version++;
                synonims.Add(linkedWord.ToLower());
                existingKeyword.Synonims = synonims;
                await _synonimyRepository.UpdateAsync(existingKeyword);
            }
        }
    }
}
