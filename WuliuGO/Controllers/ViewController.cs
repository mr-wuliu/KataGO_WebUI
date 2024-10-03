
using Microsoft.AspNetCore.Mvc;

namespace WuliuGO.Controllers
{
    [ApiController]
    [Route("Home")]
    public class ViewController : Controller
    {
        [HttpGet("welcome")]
        public IActionResult Index()
        {
            return View("Index");
        }
        [HttpGet("game")]
        public IActionResult GoPage()
        {
            ViewData["Version"] = GetVersion();
            return View("GoPage");
        }

        public string GetVersion()
        {
            return "1.0";
        }
    }
    
}