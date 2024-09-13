using WuliuGO.GameLogic;

namespace WuliuGO.Models
{
    public class Room
    {
        public string RoomId { get; set; }
        public long Owner { get; set; }
        public long guest { get; set; }
        public long BlackPlayer { get; set; }
        public long WhitePlayer { get; set; }

        public GoGame Game { get; set; }
        public bool IsBlackTurn { get; set; }

        public Room(long blackPlayer, long whitePlayer)
        {
            // 同时需要两个用户, 用于匹配系统
            RoomId = Guid.NewGuid().ToString();
            BlackPlayer = blackPlayer;
            WhitePlayer = whitePlayer;
            Game = new GoGame();
        }
        public Room(long Player)
        {
            // 用于自己开房间, 等待别人进来
            RoomId = Guid.NewGuid().ToString();
            Owner = Player;
            Game = new GoGame();
        }

    }
}