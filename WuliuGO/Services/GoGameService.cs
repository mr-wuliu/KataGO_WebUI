using WuliuGO.GameLogic;

namespace WuliuGO.Services
{
    public class GoGameService
    {
        private readonly GoGame _goGame;
        // TODO: Select GoGame by gameID;
        public GoGame GetGoGameByUserId(long gameId)
        {
            throw new NotImplementedException();
        }
        // TODO: Create Game
        public void CreateGoGame()
        {
            throw new NotImplementedException();
        }
        public GoGameService()
        {
            _goGame = new GoGame();
        }

        public string GetBoard()
        {
            return _goGame.GetBoard();
        }

        public void PlayAction(PlayerOperation operation)
        {
            _goGame.PlayAction(operation);
        }

        public string GetBranch()
        {
            return _goGame.GetBranch();
        }
    }
}
