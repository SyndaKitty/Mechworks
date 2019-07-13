using UnityEngine;

public class Mover : MonoBehaviour
{
    public MovementManager MovementManager;
    public TickInfoReference TickInfo;
    public Vector3Int TargetPosition;
    public Vector3Int CurrentPosition;

    ActionScript actionScript;

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
        if (TickInfo.Ticking)
        {
            Tick();
        }
        transform.position = Vector3.Lerp(CurrentPosition, TargetPosition, TickInfo.InterpolatedTime);
    }

    public void Tick()
    {
        CurrentPosition = TargetPosition;

        if (actionScript && actionScript.TryGetNextAction(out var action))
        {
            TargetPosition += action.Offset();
        }
    }

    public void SetTargetPosition(Vector3Int targetPosition)
    {
        TargetPosition = targetPosition;
    }
}
