

using WuliuGO.GameLogic.utils;

/**
 * 围棋模块
 */
namespace WuliuGO.GameLogic
{
    public class GoGame
    {

        // GoGame只维护分支, 不维护局面
        // 当切换分支的时候, 需要重新构建局面
        // 分支后续选择手数最多的子分支
        private const int MAX_BOARD_SIZE = 19;
        private const int MAX_BRANCH_NUM = 10;
        private Board _board;

        // 根节点
        private readonly TreeNode<GoNode> _goTree;
        // 当前节点
        private TreeNode<GoNode> _currentNdoe;
        public GoGame()
        {
            this._board = new Board(19);
            this._goTree = new TreeNode<GoNode>();
            this._currentNdoe = this._goTree;

        }

        public bool PlayAction(Operation opt)
        {
            /**
             * 使用 is 的有优势:
             * is 会忽略定义在该类上的任何符号重载
             * is 还可以用于将表达式进行模式匹配
             * static bool IsFirstFridayOfOctober(DateTime date) => 
             *     date is { Month: 10, Day: <=7, DayOfWeek: DayOfWeek.Friday };
             * is 也有声明模式的用法, 即代码的用法.
             * 
             */
            if (opt.Action is Position positionMove)
            {
                return ExecutePositionMove(positionMove, opt.Color);
            }
            else if (opt.Action is Revoke revoke)
            {
                return ExecuteRevoke(revoke);
            }
            else if (opt.Action is Pass pass)
            {
                // TODO: 暂时不支持跳过
                return false;
            }
            else if (opt.Action is NoneAction none)
            {
                // 非初始节点不允许None操作
                return false;
            }
            // TODO: 更新节点和状态
            return true;
        }

        private bool ExecutePositionMove(Position move, Color color)
        {
            if ( ! _board.PlaceStone(move, color))
            {
                return false;
            }
            GoNode add = new()
            {
                action = move,
                color = color
            };
            // 保存节点
            TreeNode<GoNode> newNode = new()
            {
                Value = add
            };
            this._currentNdoe.AddChild(newNode);
            this._currentNdoe = newNode;

            return true;
        }
        public bool ExecuteRevoke(Revoke revoke)
        {
            // 执行撤销操作
            if (! _board.Revoke()) return false;
            if (this._currentNdoe.Parent == null) return false;
            TreeNode<GoNode> parent = this._currentNdoe.Parent;
            parent.RemoveChild(this._currentNdoe);
            this._currentNdoe = parent;
            return true;
        }

        public string GetBoard() 
        {
            return _board.ToString();
        }
        
        public string GetBranch()
        {
    
            List<string> builders = [];
            TreeNode<GoNode>? node = _currentNdoe;
            while (node != null)
            {
                if (node.Value == null )
                {
                    builders.Add("Head");
                }
                else{
                    builders.Add("->{" + node.Value + "}");
                }
                node = node.Parent;
            }
            builders.Reverse();
            return string.Join("", builders);
        }
    }
}