using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    readonly List<Mover> movers = new List<Mover>();
    readonly Dictionary<Vector3Int, Mover> moverLookup = new Dictionary<Vector3Int, Mover>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

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
}
