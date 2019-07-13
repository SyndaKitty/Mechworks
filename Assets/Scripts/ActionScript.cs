using System;
using UnityEngine;

public class ActionScript : MonoBehaviour
{
    public TickInfoReference TickInfo;
    public bool Repeat;
    public ScriptLine[] ScriptLines;

    readonly Vector3Int[] OffsetFromAction = new []
    {
        new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1),
        new Vector3Int(-1, 0, 0), new Vector3Int(1, 0, 0),
        Vector3Int.zero
    };

    int movementIndex;
    int step;

    public bool TryGetNextAction(out Action move)
    {
        // Check move index
        if (movementIndex >= ScriptLines.Length)
        {
            // Loop if necessary
            if (Repeat)
            {
                movementIndex = 0;
            }
            // Otherwise don't move
            else
            {
                move = Vector3Int.zero;
                return false;
            }
        }

        // Evaluate step
        var movement = ScriptLines[movementIndex];
        move = OffsetFromAction[(int)movement.Action];

        // Move onto next move if necessary
        if (++step >= movement.Iterations)
        {
            step = 0;
            movementIndex++;
        }

        return true;
    }
}

[Serializable]
public struct ScriptLine
{
    public int Iterations;
    public Action Action;
}

[Serializable]
public enum Action
{
    Up, Down,
    Left, Right,
    Wait
}