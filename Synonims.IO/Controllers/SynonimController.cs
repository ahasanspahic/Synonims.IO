using Microsoft.AspNetCore.Mvc;
using Synonims.Services.Synonims;
using Synonims.Services.Synonims.Models;

namespace Synonims.IO.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SynonimController : ControllerBase
    {

        private ISynonimService _service { get; }

        public SynonimController(ISynonimService service)
        {
            _service = service;
        }


        [HttpGet(Name = "GetSynonimsForWord")]
        public async Task<SynonimModel?> Get(string word)
        {
            var result = await _service.GetSynonimsForWord(word);

            return result;
        }

        [HttpPost(Name = "Add new synonim")]
        public async Task<ActionResult> Post(string keyword, string linkedWord)
        {
            await _service.AddSynonim(keyword, linkedWord);

            return Ok();
        }

        [HttpDelete(Name = "Empty the database")]
        public async Task<ActionResult> Delete()
        {
            await _service.EmptyColletion();

            return Ok();
        }
    }
}