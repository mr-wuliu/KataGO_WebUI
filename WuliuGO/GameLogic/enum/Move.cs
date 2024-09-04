
/**
 * 行棋枚举类, 规定可以行棋的方法
 */

 // Move 基类
 public abstract class Action
 {
    public abstract override string ToString();
 }


 public class Action<T> :Action
 {
    public T Value {get;}

    public Action(T value) => Value = value;
    public override string ToString()
    {
        return Value?.ToString() ?? "None";
    }

}

// 设置 Pass, Position, None 类型
public class Position : Action
{
    public int X { get; }
    public int Y { get; }
    public Position(int x, int y) => (this.X,this.Y) = (x,y);
    public override string ToString() => $"({X},{Y})";

    // 重新实现Hash和Equal
    public override bool Equals(object? obj)
    {        
        if (obj is Position other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        // 选择一个合适的哈希算法，这里使用一个简单的计算方法
        return X * 31 + Y;
    }
}

public class Pass : Action
{
    public override string ToString()
    {
        return "Pass";
    }
}

public class NoneAction : Action
{
    public override string ToString()
    {
        return "None";
    }
}

public class Revoke : Action
{
    public override string ToString()
    {
        return "Revoke";
    }
}
