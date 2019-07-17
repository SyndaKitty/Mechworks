using System;
using UnityEngine;

public class ActionScript : MonoBehaviour
{
    public bool Repeat;
    public ScriptLine[] ScriptLines;

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