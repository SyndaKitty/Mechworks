using UnityEngine;

public class Mover : MonoBehaviour
{
    public MovementManager MovementManager;
    public TickInfoReference TickInfo;

    public Vector3Int TargetPosition;
    public Vector3Int CurrentPosition;
    public Action DesiredAction;
    public bool MovementSuccessful;

    ActionScript actionScript;

    void Awake()
    {
        DesiredAction = Action.Wait;
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
            DesiredAction = action;
            MovementManager.RequestMovement(this, CurrentPosition, CurrentPosition + action.Offset(), action);
        }
    }
}
