using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TickAnimator : MonoBehaviour
{
    public TickInfo TickInfo;
    public Sprite[] Sprites;
    public float Speed;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int frame = Mathf.RoundToInt(((TickInfo.InterpolatedTime * Speed) % 1)* Sprites.Length) % Sprites.Length;
        if (frame < 0) frame += Sprites.Length;
        sr.sprite = Sprites[frame];
    }
}
