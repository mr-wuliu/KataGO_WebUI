using WuliuGO.Extensions;

namespace WuliuGO.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string UserIdSessionKey = "UserId";

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void SetCurrentUser(int userId)
        {
            _httpContextAccessor.HttpContext?.Session.SetLong(UserIdSessionKey, userId);
        }
        public long GetCurrentUserId()
        {
            var userIdStr = _httpContextAccessor.HttpContext?.Session.GetString(UserIdSessionKey);
            return long.TryParse(userIdStr, out var userId) ? userId : 0;
        }
    }
}