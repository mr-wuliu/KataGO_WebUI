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
        private readonly UserService _userService;

        public GoGameController(
            GoGameService goGameService,
            UserService userService)
        {
            _userService = userService;
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
            var userId = _userService.GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }
            return _goGameService.GetBoard(userId) ?? "null";
        }
        [HttpPost("create")]
        public IActionResult CreateGame([FromBody] GameInitDto dto)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var status = _goGameService.CreateGoGame(userId);
            if (status)
            {
                return Ok("success");
            }
            else
            {
                return BadRequest("failed to create game");
            }
        }
        [HttpPost("play")]
        public IActionResult PlayAction([FromBody] OperationDto optDto)
        { 
            var userId = _userService.GetCurrentUserId();
            if (userId == 0) {
                return Unauthorized();
            }
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
                    userId,
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
            var userId = _userService.GetCurrentUserId();
            if ( userId == 0 ) {
                return Unauthorized();
            }
            return _goGameService.GetBranch(userId) ?? "null";
        }
        [HttpPost("analysis")]
        public async Task<IActionResult> AnalysisGame() {
            var userId = _userService.GetCurrentUserId();
            if (userId == 0) {
                return Unauthorized();
            }
            var queryId = await _goGameService.AnalysisGame(userId);
            return Ok(queryId);
        }
        [HttpPost("query")]
        public async Task<IActionResult> QueryAnalysis([FromBody]string  queryId)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == 0) {
                return Unauthorized();
            }
            var result = await _goGameService.GetQueryByQueryId(queryId);
            if (result == null) {
                return NotFound();
            }
            return Ok(result);
        }

    }
}