using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    readonly List<Mover> movers = new List<Mover>();
    readonly Dictionary<Vector3Int, Mover> moverLookup = new Dictionary<Vector3Int, Mover>();

    #region MovementTracking
    readonly List<Movement> requestedMovements = new List<Movement>();
    readonly Dictionary<DirectionalKey, int> directionalLookup = new Dictionary<DirectionalKey, int>();
    #endregion MovementTracking

    void LateUpdate()
    {
        CheckMovements();
    }

    /// <summary>
    /// Attempt to add a mover, and record it's position.
    /// Fails if place specified is already being used
    /// </summary>
    /// <param name="mover">The mover to add</param>
    /// <param name="place">Where to place the mover</param>
    /// <returns></returns>
    public bool RegisterMover(Mover mover, Vector3Int place)
    {
        if (moverLookup.ContainsKey(place))
        {
            return false;
        }

        movers.Add(mover);
        moverLookup.Add(place, mover);

        return true;
    }

    public void RequestMovement(Mover mover, Vector3Int currentLocation, Vector3Int targetLocation, Action action)
    {
        // Validate it's a move we handle
        if (action != Action.Down || action != Action.Left || action != Action.Left || action != Action.Right) return;

        var movement = new Movement(mover, currentLocation, targetLocation, action);
        requestedMovements.Add(movement);

        // Check for moves that are in line behind this one
        var lookBehind = new DirectionalKey(currentLocation, action);
        if (directionalLookup.TryGetValue(lookBehind, out var index))
        {
            movement.IsAhead = true;
            requestedMovements[index].IsBehind = true;
        }

        // Check for moves that are in line ahead of this one
        var lookAhead = new DirectionalKey(targetLocation + targetLocation - currentLocation, action);
        if (directionalLookup.TryGetValue(lookAhead, out index))
        {
            movement.IsBehind = true;
            requestedMovements[index].IsAhead = true;
        }

        directionalLookup.Add(new DirectionalKey(targetLocation, action), requestedMovements.Count);
    }

    void CheckMovements()
    {
        foreach (var movement in requestedMovements)
        {

        }
    }
}

#region Movement Specific Data Structures
class Movement
{
    public Mover Mover;
    public Vector3Int FromPosition;
    public Vector3Int TargetPosition;
    public Action Action;
    public bool IsAhead;
    public bool IsBehind;

    public Movement(Mover mover, Vector3Int currentPosition, Vector3Int targetPosition, Action action)
    {
        Mover = mover;
        FromPosition = currentPosition;
        TargetPosition = targetPosition;
        Action = action;
    }
}

struct DirectionalKey : IEqualityComparer<DirectionalKey>
{
    public Action Action;
    public Vector3Int Destination;

    public DirectionalKey(Vector3Int destination, Action action)
    {
        Destination = destination;
        Action = action;
    }

    public bool Equals(DirectionalKey x, DirectionalKey y)
    {
        return x.Action == y.Action && x.Destination.Equals(y.Destination);
    }

    public int GetHashCode(DirectionalKey obj)
    {
        unchecked
        {
            return ((int)obj.Action * 397) ^ obj.Destination.GetHashCode();
        }
    }
}
#endregion