using Synonims.Services.Synonims.Models;

namespace Synonims.Services.Synonims
{
    public interface ISynonimService
    {
        Task<SynonimModel?> GetSynonimsForWord(string word);

        Task AddSynonim(string keyword, string linkedWord);
        Task EmptyColletion();
    }
}
