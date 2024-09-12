using Microsoft.AspNetCore.Mvc;
using WuliuGO.Models;

namespace WuliuGO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            if ( ! ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

        }

    }
}