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

    void Start()
    {
        
    }

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

    public void RequestMovement(Mover mover, Vector3Int targetLocation, Action action)
    {
        var movement = new Movement(mover, targetLocation, action);

        requestedMovements.Add(movement);

        directionalLookup.Add(new DirectionalKey(targetLocation, action), requestedMovements.Count);
    }

    void CheckMovements()
    {

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

    public Movement(Mover mover, Vector3Int targetPosition, Action action)
    {
        Mover = mover;
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