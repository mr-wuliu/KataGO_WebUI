using Microsoft.AspNetCore.Mvc;
using WuliuGO.Models;
using WuliuGO.Services;

namespace WuliuGO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("add")]
        public  IActionResult AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] long userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with Id: {userId} not NotFound");
            }
            _userService.SetCurrentUser(userId);
            return Ok(new {
                Message = "Login successful",
                UserId = user.Id
            });
        }
        [HttpGet("info")]
        public IActionResult Test()
        {
            // 验证是否登录成功
            var userId = _userService.GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }
            return Ok($"User with Id: {userId} is logged in");
        }

    }
}