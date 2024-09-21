using Microsoft.AspNetCore.Mvc;
using WuliuGO.Models;
using WuliuGO.Services;

namespace WuliuGO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;
        public UserController(IUserRepository userRepository, UserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with Id: {userId} not NotFound");
            }
            // Store the user Id in the session
            _userService.SetCurrentUser(userId);
            // HttpContext.Session.SetString("UserId", user.Id.ToString());
            return Ok(new {
                Message = "Login successful",
                UserId = user.Id
            });
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            // 验证是否登录成功
            // var userId = HttpContext.Session.GetInt32("UserId");
            var userId = _userService.GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }
            return Ok($"User with Id: {userId} is logged in");
        }

    }
}