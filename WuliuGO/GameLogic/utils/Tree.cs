
namespace WuliuGO.GameLogic.utils
{
    public class TreeNode<T>
    {
        public T? Value { get; set; }
        public List<TreeNode<T>> Children = [];
        public TreeNode<T>? Parent { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is TreeNode<T> other)
            {
                return EqualityComparer<T>.Default.Equals(this.Value, other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (Value is null)
            {
                return 0;
            }
            return EqualityComparer<T?>.Default.GetHashCode(Value);
        }


        public void AddChild(TreeNode<T> child)
        {
            ArgumentNullException.ThrowIfNull(child);

            if (!Children.Contains(child))
            {
                Children.Add(child);
                child.Parent = this;
            }
        }

        public void RemoveChild(TreeNode<T> child)
        {
            ArgumentNullException.ThrowIfNull(child);

            if (Children.Remove(child))
            {
                child.Parent = null;
            }
        }
    }
}