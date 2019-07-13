using System;
using UnityEngine;

[Serializable]
public class IntReference : ScriptableObject
{
    public bool UseConstant = true;
    public float ConstantValue;
    public IntVariable Variable;

    public float Value => UseConstant ? ConstantValue : Variable.Value;
}