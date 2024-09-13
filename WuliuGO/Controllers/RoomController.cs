using Microsoft.AspNetCore.Mvc;
using WuliuGO.Services;

namespace WuliuGO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly RoomService _roomPool;
        public RoomController(RoomService roomPool, UserService userService)
        {
            _roomPool = roomPool;
            _userService = userService;
        }

        [HttpPost("createRoom")]
        public IActionResult CreateRoom()
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            var room = _roomPool.CreateRoom(userId);
            return Ok(room);
        }
    }
}