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

        [HttpGet("getOpt")]
        public ActionResult<Operation> GetActionResult()
        {
            var opt = new PlayerOperation
            {
                Action = new Position(9, 9),
                Color = Color.Black
            };

            return Ok(opt);
        }

        [HttpGet("branch")]
        public ActionResult<string> GetBranch()
        {
            return _goGameService.GetBranch();
        }

        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            GameLogic.GoGame goGame = new();
            PlayerOperation opt = new(
                Color.Black, new Position (0, 0)
            );
            PlayerOperation opt2 = new(
                Color.White, new Position (0, 1)
            );
            PlayerOperation opt3 = new(
                Color.White, new Position (1, 1)
            );
            PlayerOperation opt4 = new(
                Color.Black, new Position (1, 0)
            );
            PlayerOperation opt5 = new(
                Color.White, new Position (2, 0)
            );
            PlayerOperation opt6 = new(
                Color.Blank, new Revoke()
            );
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt2);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt3);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt4);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt5);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt6);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt6);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt6);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt6);
            Console.WriteLine(goGame.GetBranch());
            goGame.PlayAction(opt6);
            Console.WriteLine(goGame.GetBranch());

            return goGame.GetBoard();
        }
        
    }
}