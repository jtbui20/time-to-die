using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitView), true)]
public class UnitInspector : Editor
{
    public override bool RequiresConstantRepaint()
    {
        return Application.isPlaying;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!Application.isPlaying) { return; }

        EditorGUILayout.Space();

        UnitView unitView = (UnitView)target;

        FreeUnit unit = unitView.Unit;

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
        if (!Application.isPlaying) { return; }
        Tools.hidden = true;
    }

    private void OnDisable()
    {
        Tools.hidden = false;
    }
}