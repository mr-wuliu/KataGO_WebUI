
using Microsoft.AspNetCore.Mvc;

namespace WuliuGO.Controllers
{
    [Route("view")]
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
            ViewData["Version"] = "1.0";
            return View("GoPage");
        }
    }
    
}