
namespace WuliuGO.GameLogic.utils
{
    public class GoNode
    {
        public Color color = Color.Blank;
        public Action? action;

        public bool isMain = true; // 是否是主节点

        // TODO: 添加其他属性
        public override string ToString()
        {
            return $"Color: {color}, Action: {action}";
        }
    }

}