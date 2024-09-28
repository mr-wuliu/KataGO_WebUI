using KatagoDtos;
using Microsoft.AspNetCore.Mvc;
using WuliuGO.Services;

namespace WuliuGO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KatagoController : ControllerBase
    {
        private readonly IKatagoServer _katagoService;

        public KatagoController(IKatagoServer katagoService)
        {
            _katagoService = katagoService;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return _katagoService.GetStatus() switch
            {
                true => Ok("running"),
                false => Ok("stopped"),
            };
        }
        // [HttpPost("start")]
        // public IActionResult StartKatago() {
        //     return Ok(_katagoService.StartKatago());
        // }
        // [HttpPost("stop")]
        // public IActionResult StopKatago()
        // {
        //     return _katagoService.StopKatago() ? Ok("Stopped") : Ok("Not running");
        // }
        [HttpPost("analysis")]
        public async Task<IActionResult> Anslysis([FromBody] QueryDto dto) {
            var result = await _katagoService.AnaylyzeBoardAsync(dto);
            return Ok(result);
        } 
        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] string queryId)
        {
            if ( ! queryId.StartsWith("go_"))
            {
                return BadRequest();
            }
            var result = await _katagoService.GetQueryByQueryId(queryId);
            return Ok(result);
        }
    }
}