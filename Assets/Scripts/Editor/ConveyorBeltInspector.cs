using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConveyorBelt2D))]
public class ConveyorBeltInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            (target as ConveyorBelt2D).SetSprite();
        }
    }
}
