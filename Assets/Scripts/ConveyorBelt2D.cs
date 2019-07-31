using UnityEngine;

public class ConveyorBelt2D : MonoBehaviour
{
    public TickInfo TickInfo;
    public BeltManager2D BeltManager;
    
    // Position
    public Vector3Int GridPosition;
    public Direction2D Direction;
 
    // Movement
    public bool Moving;
    public Transform Item;
    
    // Instantiation
    public GameObject SpawnItem;

    // Visuals
    public Sprite[] Sprites;

    void Awake()
    {
        // Snap to grid
        GridPosition = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            Mathf.RoundToInt(transform.position.z)
        );
        transform.position = GridPosition;

        if (!BeltManager.Register(this))
        {
            Destroy(gameObject);
            return;
        }

        // Spawn item
        if (SpawnItem)
        {
            Item = Instantiate(SpawnItem).transform;
        }
    }

    void Update()
    {
        if (Item)
        {
            if (Moving)
            {
                Item.position = GridPosition + Direction.Vector3() * TickInfo.InterpolatedTime;
            }

            if (TickInfo.Ticking)
            {
                print("Trying to move");
                BeltManager.MoveItem(GridPosition, Direction, Item);
            }
        }
    }

    public void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = Sprites[(int)Direction % 2];
    }

    void OnDrawGizmosSelected()
    {
        float theta = Mathf.PI / 2 * (int)Direction;
        Vector3 dir = Direction.Vector3();
        Gizmos.DrawRay(transform.position, dir);
    }
}