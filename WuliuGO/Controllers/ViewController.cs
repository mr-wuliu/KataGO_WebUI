using Microsoft.AspNetCore.Mvc;
using WuliuGO.Services;

namespace WuliuGO.Controllers
{
    [Route("view")]
    public class ViewController : Controller
    {
        private readonly GoGameService _goGameService;
        private readonly UserService _userService;
        public ViewController(GoGameService goGameService,UserService userService)
        {
            _goGameService = goGameService;
            _userService = userService; 
        }
        [HttpGet("welcome")]
        public IActionResult Index()
        {
            return View("Index");
        }
        [HttpGet("game")]
        public IActionResult GoPage()
        {
            return View("GoPage");
        }
    }
    
}