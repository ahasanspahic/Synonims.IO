using Synonims.DataLayer.SynonimRepositories.Models;

namespace Synonims.Services.Synonims.Models
{
    public class SynonimModel
    {
        public String? Keyword { get; set; }
        public IEnumerable<String> Synonims { get; set; }
        /// <summary>
        /// Used to show all linked synonims and their synonims as well, respecting Transient Rule.
        /// eg. If B is linked to A, and C is linked to B, that means C is also linked to A, where link assumes transiency.
        /// </summary>
        public IEnumerable<SynonimModel> TransientSynoninms { get; set; }
        public static SynonimModel FromEntity(SynonimEntity entity)
        {
            if (entity == null)
                return null;
            return new()
            {
                Keyword = entity.Keyword,
                Synonims = entity.Synonims,
                TransientSynoninms = new List<SynonimModel>()
            };
        }
    }
}
