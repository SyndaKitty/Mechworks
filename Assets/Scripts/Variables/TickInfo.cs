using UnityEngine;

[CreateAssetMenu]
public class TickInfo : ScriptableObject
{
    public float InterpolatedTime;
    public int CurrentTick;
    public bool Ticking;
}