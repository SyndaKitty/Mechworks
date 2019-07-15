using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    // Input
    public TickInfo TickInfo;

    // Mover info
    readonly List<Mover> movers = new List<Mover>();
    readonly Dictionary<Vector3Int, Mover> obstacleLookup = new Dictionary<Vector3Int, Mover>();

    // Debug
    //List<Vector3Int> debugObstacles = new List<Vector3Int>();

    // Movement tracking
    readonly List<Movement> requestedMovements = new List<Movement>();
    readonly Dictionary<Vector3Int, Movement> movementLookup = new Dictionary<Vector3Int, Movement>();
    readonly Dictionary<Vector3Int, Vector3Int> movementDependency = new Dictionary<Vector3Int, Vector3Int>();

    // Statics
    static readonly Vector3Int[] Directions = new[] {new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, 0, -1), new Vector3Int(0, 0, 1)};

    void Update()
    {
        if (TickInfo.Ticking)
        {
            CheckMovements();
            WriteResults();

            //debugObstacles = obstacleLookup.Keys.ToList();

            ClearMovementData();
        }
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
        if (obstacleLookup.ContainsKey(place))
        {
            return false;
        }

        movers.Add(mover);
        obstacleLookup.Add(place, mover);

        return true;
    }

    public void RequestMovement(Mover mover, Vector3Int start, Vector3Int destination, Action action)
    {
        // Validate it's a move we handle
        if (action >= Action.Wait) return;

        var movement = new Movement(mover, start, destination, action);
        requestedMovements.Add(movement);
        movementLookup.Add(movement.Start, movement);
    }

    void CheckMovements()
    {
        foreach (var movement in requestedMovements)
        {
            //print("");
            //print("Checking " + movement.Mover.name);
            // Chaining - Handle multiple movers moving in a chain
            if (obstacleLookup.TryGetValue(movement.Destination, out var obstacle))
            {
                //print("Mover in the way, moving " + obstacle.DesiredAction);
                // If there is a mover in the way, and it's not moving in the same direction: block
                if (obstacle.DesiredAction != movement.Action)
                {
                    //print("Moving in different direction");
                    BlockMovement(movement);
                } 
                // If the mover in front of us is already blocked, so are we
                else if (movementLookup.TryGetValue(movement.Destination, out var destMovement) && destMovement.Blocked)
                {
                    //print("Mover in front of us is already blocked");
                    BlockMovement(movement);
                }
                else
                {
                    //print("Mover ahead of us is valid, recording dependency from " + movement.Start + " " + movement.Destination);
                    // Record dependency
                    movementDependency.Add(movement.Start, movement.Destination);
                }
            }
            // Collision - Handle multiple movers trying to move into same space
            else
            {
                //print("Nothing in way, checking surroundings");
                // Check for other movers
                var direction = movement.Start - movement.Destination;
                foreach (var otherDirection in Directions)
                {
                    if (direction == otherDirection) continue;
                    //print(direction + " " + otherDirection);
                    var otherOrigin = movement.Destination + otherDirection;
                    if (movementLookup.TryGetValue(otherOrigin, out var otherMovement) && otherMovement.Destination == movement.Destination)
                    {
                        //print(otherMovement.Mover.name + " is also moving to our destination");
                        if (!otherMovement.Blocked)
                        {
                            BlockMovement(otherMovement);
                        }

                        if (!movement.Blocked)
                        {
                            BlockMovement(movement);
                        }
                    }
                }
            }
        }
    }

    void WriteResults()
    {
        foreach (var movement in requestedMovements)
        {
            // Tell movers if they were able to move
            if (movement.Blocked)
            {
                movement.Mover.MovementSuccessful = false;
                movement.Mover.CurrentPosition = movement.Start;
                movement.Mover.TargetPosition = movement.Start;
            }
            else
            {
                movement.Mover.MovementSuccessful = true;
                movement.Mover.CurrentPosition = movement.Start;
                movement.Mover.TargetPosition = movement.Destination;
                
                // Recalculate positions
                var behind = movement.Start + movement.Start - movement.Destination;
                obstacleLookup.Remove(movement.Start);
            }
        }

        foreach (var movement in requestedMovements)
        {
            if (movement.Blocked) continue;
            obstacleLookup[movement.Destination] = movement.Mover;
        }
    }

    void ClearMovementData()
    {
        requestedMovements.Clear();
        movementLookup.Clear();
        movementDependency.Clear();
    }

    void BlockMovement(Movement movement)
    {
        while (true)
        {
            //print("Marking " + movement.Mover.name + " as blocked");
            // Prevent duplicate calls
            if (movement.Blocked) return;
            movement.Blocked = true;
            // Check behind the mover
            var behind = movement.Start + movement.Start - movement.Destination;
            if (movementDependency.TryGetValue(behind, out var origin))
            {
                // Propagate blocking down the chain
                var action = movement.Action;
                movement = movementLookup[behind];
                if (movement.Action != action) return;
            }
            else
            {
                break;
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    var color = Color.yellow;
    //    color.a = 0.3f;
    //    Gizmos.color = color;

    //    foreach (var obstacle in debugObstacles)
    //    {
    //        Gizmos.DrawCube(obstacle, Vector3.one);
    //    }
    //}
}

#region Movement Specific Data Structures
class Movement
{
    public Mover Mover;
    public Vector3Int Start;
    public Vector3Int Destination;
    public Action Action;
    public bool Blocked;

    public Movement(Mover mover, Vector3Int start, Vector3Int destination, Action action)
    {
        Mover = mover;
        Start = start;
        Destination = destination;
        Action = action;
        Blocked = false;
    }
}

#endregion