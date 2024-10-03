using System.Diagnostics;
using KatagoDtos;
using WuliuGO.GameLogic;

namespace WuliuGO.Services
{
    public class GoGameService
    {
        private readonly CacheService _cacheService;
        private readonly IKatagoServer _katagoServer;
        
        public GoGameService(IKatagoServer katagoServer, CacheService cacheService)
        {
            _cacheService = cacheService;
            _katagoServer = katagoServer;
        }
        public GoGame? GetGoGameByUserId(long gameId)
        {
            return _cacheService.GetGoGame(gameId);
        }
        public bool CreateGoGame(long userId)
        {
            return _cacheService.CreateRoom(userId);
        }

        public string? GetBoard(long userId)
        {
            if (! _cacheService.isInRoom(userId)){
                return null;
            } else {
                var game = _cacheService.GetGoGame(userId);
                Debug.Assert(game != null);
                return game.GetBoard();
            }
        }
        public void PlayAction(long userId, PlayerOperation operation)
        {
            if (_cacheService.isInRoom(userId))
            {
                var game = _cacheService.GetGoGame(userId);
                Debug.Assert(game != null);
                game.PlayAction(operation);
            }
        }
        public string? GetBranch(long userId)
        {
            if (_cacheService.isInRoom(userId))
            {
                var game = _cacheService.GetGoGame(userId);
                Debug.Assert(game != null);
                return game.GetBranch();
            }
            else {
                return null;
            }
        }
        public async Task<string> AnalysisGame(long userId)
        {
            var moves = GetBranchList(userId);
            string queryId = await _katagoServer.AnaylyzeBoardAsync(
                new KatagoDtos.QueryDto()
                {
                    moves = moves,
                }
            );
            return queryId;
        }
        public async Task<KatagoQueryRest?> GetQueryByQueryId(string queryId)
        {
            var result = await _katagoServer.GetQueryByQueryId(queryId);
            return result;
        }
        private List<List<string>> GetBranchList(long userId)
        {
            var game = _cacheService.GetGoGame(userId);
            if (game == null) {
                return [];
            }
            var branchList = game.GetMoves();
            return branchList;
        }
        public async Task<List<double>?> GetPolicyByQueryId(string queryId)
        {
            return await _katagoServer.GetKatagoPolicy(queryId);
        }
        
    }
}
