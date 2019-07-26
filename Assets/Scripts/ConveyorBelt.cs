using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public TickInfo TickInfo;
    public Transform Item;
    public bool Moving;
    public BeltManager BeltManager;
    public Vector3Int GridPosition;
    public Direction Direction;
    public Vector3 ItemOffset = new Vector3(0, .5f, 0);

    void Awake()
    {
        // Snap to grid
        GridPosition = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            Mathf.RoundToInt(transform.position.z)
        );
        transform.position = GridPosition;

        // Determine direction based off of rotation
        float yTurns = Mathf.RoundToInt(transform.rotation.eulerAngles.y / -90);
        transform.eulerAngles = new Vector3(0, yTurns * -90, 0);

        Direction = (Direction)(yTurns % 4 + (yTurns < 0 ? 4 : 0));

        if (!BeltManager.Register(this))
        {
            Destroy(gameObject);
            return;
        }

        // Check for item above
        var ray = new Ray(GridPosition, Vector3.up);
        var layer = 1 << LayerMask.NameToLayer("Item");
        if (Physics.Raycast(ray, out var hitInfo, 1, layer))
        {
            print("asd");
            Item = hitInfo.transform;
        }
        else
        {
            Item = null;
        }
    }

    void Update()
    {
        if (Item)
        {
            if (Moving)
            {
                Item.position = GridPosition + ItemOffset + Direction.Vector3() * TickInfo.InterpolatedTime;
            }

            if (TickInfo.Ticking)
            {
                print($"Requesting movement for {GridPosition} {Direction}");
                BeltManager.MoveItem(GridPosition, Direction, Item);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        float theta = Mathf.PI / 2 * (int)Direction;
        Vector3 dir = Direction.Vector3();
        Gizmos.DrawRay(transform.position, dir);
    }
}
