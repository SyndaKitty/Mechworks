using System;

[Serializable]
public class TickInfoReference
{
    public TickInfo Variable;

    public float InterpolatedTime => Variable.InterpolatedTime;
    public int CurrentTick => Variable.CurrentTick;
    public bool Ticking => Variable.Ticking;
}