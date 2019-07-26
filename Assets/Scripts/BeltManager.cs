using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeltManager : MonoBehaviour
{
    public TickInfo TickInfo;
    readonly Dictionary<Vector3Int, ConveyorBelt> belts = new Dictionary<Vector3Int, ConveyorBelt>();

    // Movement info
    // Key is origin, value is destination
    readonly Dictionary<Vector3Int, Vector3Int> movementDependency = new Dictionary<Vector3Int, Vector3Int>();
    readonly Dictionary<Vector3Int, ItemMovement> movementLookup = new Dictionary<Vector3Int, ItemMovement>();
    readonly List<ItemMovement> itemMovements = new List<ItemMovement>();

    // Transfer items from old belts to new belts & clear movement data
    void Update()
    {
        if (!TickInfo.Ticking) return;

        // Remove from old
        foreach (var movement in itemMovements)
        {
            if (movement.Blocked) continue;
            print($"Removing item from {movement.Origin}");
            belts[movement.Origin].Item = null;
        }
        
        // Add to new
        foreach (var movement in itemMovements)
        {
            if (movement.Blocked) continue;
            print($"Added item to {movement.Destination}");
            belts[movement.Destination].Item = movement.Item;
        }

        itemMovements.Clear();
        movementDependency.Clear();
        movementLookup.Clear();
    }

    // Evaluate movements & Write output
    void LateUpdate()
    {
        if (!TickInfo.Ticking) return;

        // Evaluate movements
        foreach (var movement in itemMovements)
        {
            // If there is no belt in the destination, we're blocked
            if (!belts.ContainsKey(movement.Destination))
            {
                BlockMovement(movement.Origin);
                continue;
            }

            // If there is a belt, check for blocking
            if (movementLookup.TryGetValue(movement.Destination, out var aheadMovement))
            {
                if (aheadMovement.Blocked)
                {
                    BlockMovement(movement.Origin);
                }
                else
                {
                    movementDependency.Add(movement.Origin, movement.Destination);
                }
            }
        }

        // Write output
        foreach (var movement in itemMovements)
        {
            var belt = belts[movement.Origin];
            belt.Moving = !movement.Blocked;
        }
    }

    public void BlockMovement(Vector3Int origin)
    {
        print("Blocking " + origin);
        var movement = movementLookup[origin];
        if (movement.Blocked) return;

        // Keep a stack of blocks that need to be checked
        Stack<Vector3Int> originsToBeProcessed = new Stack<Vector3Int>(64);
        originsToBeProcessed.Push(origin);
        movement.Blocked = true;

        // Process all blocks in chain`
        while (originsToBeProcessed.Count > 0)
        {
            // Pop movement off the stack
            origin = originsToBeProcessed.Pop();
            movement = movementLookup[origin];

            // Check for blocking in each direction
            foreach (var dir in DirectionExtensions.Directions())
            {
                // Ignore blocking in the direction we're going
                if (movement.Direction == dir) continue;
                if (movementDependency.TryGetValue(origin + dir.Vector3Int(), out var behindOrigin))
                {
                    var behindMovement = movementLookup[behindOrigin];
                    // If it's moving to us, then it needs to be blocked
                    if (behindMovement.Direction == dir.Opposite() && !behindMovement.Blocked)
                    {
                        behindMovement.Blocked = true;
                        originsToBeProcessed.Push(behindOrigin);
                    }
                }
            }
        }

        //var behind = movement.Origin - movement.Direction.Vector3Int();
        //if (movementLookup.TryGetValue(behind, out var behindMovement))
        //{
        //    BlockMovement();
        //}
    }

    public bool Register(ConveyorBelt belt)
    {
        // Check for existing belt
        if (belts.ContainsKey(belt.GridPosition))
        {
            return false;
        }

        belts[belt.GridPosition] = belt;

        return true;
    }

    public void MoveItem(Vector3Int origin, Direction direction, Transform item)
    {
        ItemMovement movement = new ItemMovement(origin, direction, item);
        itemMovements.Add(movement);
        movementLookup[origin] = movement;
    }
}

class ItemMovement
{
    public Vector3Int Origin;
    public Vector3Int Destination;
    public Direction Direction;
    public Transform Item;
    public bool Blocked;

    public ItemMovement(Vector3Int origin, Direction direction, Transform item)
    {
        Origin = origin;
        Direction = direction;
        Destination = origin + direction.Vector3Int();
        Item = item;
    }
}