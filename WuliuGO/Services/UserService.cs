using WuliuGO.Extensions;
using WuliuGO.Models;

namespace WuliuGO.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private const string UserIdSessionKey = "UserId";

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void SetCurrentUser(long userId)
        {
            _httpContextAccessor.HttpContext?.Session.SetLong(UserIdSessionKey, userId);
        }
        public long GetCurrentUserId()
        {
            var userIdStr = _httpContextAccessor.HttpContext?.Session.GetString(UserIdSessionKey);
            return long.TryParse(userIdStr, out var userId) ? userId : 0;
        }
        public async Task<User> GetUserByIdAsync(long id) {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync() {
            var users = await _userRepository.GetAllUsersAsync();
            return users;
        }
        public async void AddUserAsync(User user) {
            await _userRepository.AddUserAsync(user);
        }
    }
}