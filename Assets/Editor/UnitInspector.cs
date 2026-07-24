using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BombView), true)]
public class UnitInspector : Editor
{
    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        BombView unitView = (BombView)target;

        FreeUnit unit = unitView.Bomb;

        EditorGUI.BeginChangeCheck();
        int healthField = EditorGUILayout.IntField("Health", unit.Health);
        Vector3 positionField = EditorGUILayout.Vector3Field("Position", unit.Position);

        if (EditorGUI.EndChangeCheck())
        {
        unit.SetHealth(healthField);
        unit.Position = positionField;

        //EditorUtility.SetDirty(target);
        }
    }

    public void OnSceneGUI()
    {
        Tools.hidden = true;
    }

    private void OnDisable()
    {
        Tools.hidden = false;
    }
}