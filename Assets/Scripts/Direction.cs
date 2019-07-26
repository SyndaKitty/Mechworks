using UnityEngine;

public enum Direction
{
    Right,
    Up,
    Left,
    Down
}

public static class DirectionExtensions
{
    public static Vector3[] DirectionVector3 = new[]
    {
        new Vector3(1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, -1)
    };

    public static Vector3Int[] DirectionVector3Int = new[]
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, -1)
    };

    public static Vector3 Vector3(this Direction dir)
    {
        return DirectionVector3[(int) dir];
    }

    public static Vector3Int Vector3Int(this Direction dir)
    {
        return DirectionVector3Int[(int) dir];
    }

    public static Direction[] Directions() => new[]
    {
        Direction.Right, Direction.Up,
        Direction.Left, Direction.Down
    };

    public static Direction Opposite(this Direction dir)
    {
        return (Direction) (((int) dir + 2) % 4);
    }
}