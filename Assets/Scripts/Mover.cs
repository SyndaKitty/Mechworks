using UnityEngine;

public class Mover : MonoBehaviour
{
    public MovementManager MovementManager;
    public TickInfoReference TickInfo;

    ActionScript actionScript;
    Vector3Int TargetPosition;
    Vector3Int CurrentPosition;

    void Awake()
    {
        actionScript = GetComponent<ActionScript>();

        Vector3Int place = new Vector3Int(
            Mathf.RoundToInt(transform.position.x), 
            Mathf.RoundToInt(transform.position.y), 
            Mathf.RoundToInt(transform.position.z)
        );

        if (MovementManager.RegisterMover(this, place))
        {
            TargetPosition = CurrentPosition = place;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(CurrentPosition, TargetPosition, TickInfo.InterpolatedTime);
        if (TickInfo.Ticking)
        {
            Tick();
        }
    }

    public void Tick()
    {
        CurrentPosition = TargetPosition;
        transform.position = TargetPosition;

        if (actionScript && actionScript.TryGetNextAction(out var offset))
        {
            TargetPosition += offset;
        }
        //else switch (Random.Range(0, 4))
        //{
        //    case 0:
        //        TargetPosition += new Vector3Int(1, 0, 0);
        //        break;
        //    case 1:
        //        TargetPosition += new Vector3Int(-1, 0, 0);
        //        break;
        //    case 2:
        //        TargetPosition += new Vector3Int(0, 0, 1);
        //        break;
        //    case 3:
        //        TargetPosition += new Vector3Int(0, 0, -1);
        //        break;
        //}

        
    }

    public void SetTargetPosition(Vector3Int targetPosition)
    {
        TargetPosition = targetPosition;
    }
}
