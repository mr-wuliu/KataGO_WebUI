namespace WuliuGO.GameLogic
{
    public class PlayerOperation
    {

        public Color Color { get; set; }
        public Action? Action { get; set; }
        public PlayerOperation()
        {
            // 可以选择性地为属性设置默认值
            this.Color = default;
            this.Action = null;
        }
        public PlayerOperation(Color color, Action action)
        {
            this.Color = color;
            this.Action = action;
        }
        public override string ToString()
        {
            return $"Operation(Color: {Color}, Move: {Action})";
        }
    }
}