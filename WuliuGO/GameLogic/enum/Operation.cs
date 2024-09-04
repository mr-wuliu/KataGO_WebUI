public class Operation {

    public Color Color { get; set; }
    public Action? Action { get; set; }

    public (Color, Action) Operate { get; set; }
    public Operation()
    {
        // 可以选择性地为属性设置默认值
        this.Color = default;
        this.Action = null;
    }
    public Operation(Color color, Action action)
    {
        this.Color = color;
        this.Action = action;
    }
    public override string ToString()
    {
        return $"Operation(Color: {Color}, Move: {Action})";
    }
}