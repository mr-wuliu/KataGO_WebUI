using Microsoft.AspNetCore.Mvc;
using WuliuGO.Services;

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
        public IActionResult PlayAction([FromBody] Operation operation)
        {
            _goGameService.PlayAction(operation);
            return Ok();
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
            Operation opt = new(
                Color.Black, new Position (0, 0)
            );
            Operation opt2 = new(
                Color.White, new Position (0, 1)
            );
            Operation opt3 = new(
                Color.White, new Position (1, 1)
            );
            Operation opt4 = new(
                Color.Black, new Position (1, 0)
            );
            Operation opt5 = new(
                Color.White, new Position (2, 0)
            );
            Operation opt6 = new(
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