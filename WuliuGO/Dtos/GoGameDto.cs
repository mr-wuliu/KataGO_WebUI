
namespace GameDtos
{

    public class OperationDto
    {
        public required string Color { get; set; }
        public required ActionDto Action { get; set; }
    }

    public class ActionDto
    {
        public required ActionType type;
        public int x;
        public int y;

    }

    public enum ActionType
    {
        Position,
        Pass,
        Revoke,
    }
}