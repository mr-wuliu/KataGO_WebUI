using WuliuGO.GameLogic;

namespace WuliuGO.Services
{
    public class GoGameService
    {
        private readonly GoGame _goGame;

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
