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

    public bool TryGetNextAction(out Action action)
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
                action = Action.Wait;
                return false;
            }
        }

        // Evaluate step
        var scriptLine = ScriptLines[movementIndex];
        action = scriptLine.Action;

        // Move onto next move if necessary
        if (++step >= scriptLine.Iterations)
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