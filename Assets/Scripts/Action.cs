using System;
using UnityEngine;

[Serializable]
public enum Action
{
    Up, Down,
    Left, Right,
    Wait
}

static class ActionExtensions
{
    public static Vector3Int Offset(this Action action)
    {
        switch (action)
        {
            case Action.Down: return new Vector3Int( 0, 0, -1);
            case Action.Up:   return new Vector3Int( 0, 0,  1);
            case Action.Left: return new Vector3Int(-1, 0,  0);
            case Action.Right: return new Vector3Int(1, 0,  0);
            default: return Vector3Int.zero;
        }
    }
}