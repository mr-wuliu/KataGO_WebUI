using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using WuliuGO.GameLogic;
using WuliuGO.Services;
using GameDtos;

namespace WuliuGO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoGameController : ControllerBase
    {
        private readonly GoGameService _goGameService;

        public GoGameController(GoGameService goGameService)
        {
            _goGameService = goGameService;
        }
        [HttpGet("version")]
        public ActionResult<string> GetVersion()
        {
            return "1.0";
        }

        [HttpGet("board")]
        public ActionResult<string> GetBoard()
        {
            return _goGameService.GetBoard();
        }

        [HttpPost("play")]
        public IActionResult PlayAction([FromBody] OperationDto optDto)
        {  
            try
            {
                Color color = optDto.Color.ToString() switch
                {
                    "Black" => Color.Black,
                    "White" => Color.White,
                    "Blank" => Color.Blank,
                    _ => throw new NotImplementedException(),
                };
                GameLogic.Action action = optDto.Action.type.ToString() switch
                {
                    "Position" => new Position() {
                        X = optDto.Action.x,
                        Y = optDto.Action.y,
                    },
                    "Pass"=> new Pass(),
                    "Revoke" => new Revoke(),
                    _ => throw new NotImplementedException(),
                };
                _goGameService.PlayAction(
                new PlayerOperation()
                {
                    Color = color,
                    Action = action,
                }
            );
            }
            catch (Exception e)
            {
                return Ok($"input error : {e}");
            }
  

            return Ok();
        }

        [HttpGet("branch")]
        public ActionResult<string> GetBranch()
        {
            return _goGameService.GetBranch();
        }
        
    }
}