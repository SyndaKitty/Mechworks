using UnityEngine;

public enum Direction2D
{
    Right,
    Up,
    Left,
    Down
}

public static class DirectionExtensions2D
{
    public static Vector3[] DirectionVector3 = new[]
    {
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, -1, 0)
    };

    public static Vector3Int[] DirectionVector3Int = new[]
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, -1, 0)
    };

    public static Vector3 Vector3(this Direction2D dir)
    {
        return DirectionVector3[(int) dir];
    }

    public static Vector3Int Vector3Int(this Direction2D dir)
    {
        return DirectionVector3Int[(int) dir];
    }

    public static Direction2D[] Directions() => new[]
    {
        Direction2D.Right, Direction2D.Up,
        Direction2D.Left, Direction2D.Down
    };

    public static Direction2D Opposite(this Direction2D dir)
    {
        return (Direction2D) (((int) dir + 2) % 4);
    }

    public static Direction2D Clockwise(this Direction2D dir)
    {
        return (Direction2D)(((int)dir + 3) % 4);
    }

    public static Direction2D CounterClockwise(this Direction2D dir)
    {
        return (Direction2D)(((int)dir + 1) % 4);
    }
}