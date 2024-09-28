using WuliuGO.GameLogic;

namespace WuliuGO.Services
{
    public class CacheService
    {
        // TODO: 支持多人对局
        private readonly Dictionary<long, GoGame> _room = [];

        public bool CreateRoom(long userId)
        {
            if (_room.ContainsKey(userId))
                return false;
            else
            {
                var game = new GoGame();
                _room.Add(userId, game);
                return true;
            }
        }
        public bool isInRoom(long userId)
        {
            return _room.ContainsKey(userId);
        }
        public GoGame? GetGoGame(long userId)
        {
            if (_room.TryGetValue(userId, out var game))
                return game;
            else
                return null;
        }
        public bool EndGame(long userId)
        {
            if (_room.ContainsKey(userId))
            {
                _room.Remove(userId);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool RebuildGame(long userId, GoGame game)
        {
            if (_room.ContainsKey(userId))
            {
                _room[userId] = game;
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}