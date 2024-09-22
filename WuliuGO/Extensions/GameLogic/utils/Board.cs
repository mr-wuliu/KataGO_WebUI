namespace WuliuGO.GameLogic.utils
{
    public class Board
    {
        private const int MAX_SIZE = 19;
        private const int BOARD_STATE = 3;

        private int[,] _board;
        public int Size { get; set; }
        private int _hashCode;
        private static readonly int[,,] ZobristTable;
        private readonly HashSet<int> previousStates; // 用于存储历史状态哈希值
        private readonly Stack<(int[,], int)> history;
        static Board()
        {
            Random rand = new();
            ZobristTable = new int[MAX_SIZE, MAX_SIZE, BOARD_STATE];

            for (int x = 0; x < MAX_SIZE; x++)
            {
                for (int y = 0; y < MAX_SIZE; y++)
                {
                    for (int s = 0; s < BOARD_STATE; s++)
                    {
                        ZobristTable[x, y, s] = rand.Next();
                    }
                }
            }
        }
        public Board(int size)
        {
            Size = size;
            _board = new int[size, size];
            _hashCode = ComputHash(); // 计算初始hash值
            previousStates = [];
            history = new Stack<(int[,], int)>();
        }

        public bool PlaceStone(Position move, Color color)
        {
            // 输入检查
            if (_board[move.X, move.Y] != 0) return false;
            if (move.X < 0 || move.Y < 0 || move.X >= Size || move.Y >= Size)
            {
                return false;
            }
            // 保存当前状态到历史栈
            history.Push((CopyBoard(), _hashCode));
            // 落子
            UpdateBoard(move, color);
            // 提子
            var capNum = 0;
            var opponent = 3 - color;
            foreach (var cp in GetNeighbors(move))
            {
                if (GetColor(cp) == opponent && !HasLiberty(cp))
                {
                    capNum += RemoveGroup(cp);
                }
            }
            // 检查是否形成循环局面
            if (!previousStates.Add(_hashCode))
            {
                Undo();
                return false;
            }
            
            return true;
        }
        public bool Revoke()
        {
            // 撤回一步棋
            if (history.Count > 0)
            {
                // 移除当前hash
                previousStates.Remove(_hashCode);
                // 弹出栈顶状态
                (int[,] previousBoard, int previousHash) = history.Pop();
                _board = previousBoard;
                _hashCode = previousHash;
                return true;
            }
            return false;
        }
        public Color GetColor(Position local)
        {
            int po = _board[local.X, local.Y];
            Color color = po switch
            {
                0 => Color.Blank,
                1 => Color.Black,
                2 => Color.White,
                _ => throw new ArgumentOutOfRangeException(nameof(local), $"Unexpected value {po} at position ({local.X}, {local.Y})")
            };
            return color;
        }

        private void UpdateBoard(Position pos, Color color)
        {
            int value = (int)color;
            // 更新前移除旧状态的哈希值
            _hashCode ^= ZobristTable[pos.X, pos.Y, _board[pos.X, pos.Y]];
            // 更新棋盘
            _board[pos.X, pos.Y] = value;
            // 更新后添加新状态的的哈希值
            _hashCode ^= ZobristTable[pos.X, pos.Y, value];
        }
        private void Undo()
        {
            if (history.Count > 0)
            {
                // 弹出栈顶的历史状态
                (int[,] previousBoard, int previousHash) = history.Pop();
                _board = previousBoard;
                _hashCode = previousHash;
            }
        }
        private int[,] CopyBoard()
        {
            int[,] copy = new int[Size, Size];
            Array.Copy(_board, copy, _board.Length);
            return copy;
        }
        private int RemoveGroup(Position pos)
        {
            // 移除一组棋子
            var stack = new Stack<Position>();
            stack.Push(pos);

            var color = GetColor(pos);
            int captured = 0;
            while (stack.Count > 0)
            {
                var cp = stack.Pop();
                if (GetColor(cp) == color)
                {
                    UpdateBoard(cp, Color.Blank);
                    captured++;
                    foreach (var up in GetNeighbors(cp))
                    {
                        if (GetColor(up) == color)
                        {
                            stack.Push(up);
                        }
                    }
                }
            }
            return captured;
        }
        private List<Position> GetNeighbors(Position pos)
        {
            var directions = new List<(int, int)>
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };
            var neighbours = new List<Position>();
            int x = pos.X;
            int y = pos.Y;

            foreach (var (dx, dy) in directions)
            {
                int cx = pos.X + dx;
                int cy = pos.Y + dy;
                if (cx >= 0 & cx < Size && cy >= 0 && cy < Size)
                {
                    neighbours.Add(new Position(cx, cy));
                }
            }
            return neighbours;
        }
        private bool HasLiberty(Position pos)
        {
            var visited = new HashSet<Position>();
            var stack = new Stack<Position>();
            stack.Push(pos);

            while (stack.Count > 0)
            {
                var cp = stack.Pop();
                if (!visited.Add(cp)) continue;
                visited.Add(cp);
                foreach (var near in GetNeighbors(cp))
                {
                    if (GetColor(near) == Color.Blank)
                    {
                        return true;
                    }
                    else if (GetColor(near) == GetColor(cp))
                    {
                        stack.Push(near);
                    }
                }
            }
            return false;
        }

        private int ComputHash()
        {
            int hash = 0;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    int state = _board[x, y];
                    hash ^= ZobristTable[x, y, state];
                }
            }
            return hash;
        }

        public override string ToString()
        {
            string columnLabels = "ABCDEFGHJKLMNOPQRST";
            string result = "";
            int boardSize = 19;
            for (int y = boardSize; y > 0; y--)
            {
                string row = $"{y,2}";
                for (int x = 0; x < boardSize; ++x)
                {
                    var stone = _board[x, y - 1];
                    row += stone switch
                    {
                        0 => " .",
                        1 => " #",
                        2 => " o",
                        _ => throw new Exception("Unknown stone value"),
                    };
                }
                result += row + Environment.NewLine;
            }
            return result + "   " + string.Join(" ", columnLabels.ToArray());
        }
    }
}