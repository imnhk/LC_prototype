using System.Collections;
using UnityEngine;
using UnityEditor;

[UnityEditor.CustomEditor(typeof(MapMaker))]
public class MapMakerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();

        MapMaker myScript = (MapMaker)this.target;

        if (GUILayout.Button("Add Item"))
        {
            myScript.AddItemSpot();
        }

        if (GUILayout.Button("Add Enemy"))
        {
            myScript.AddEnemySpot();
        }

        if (GUILayout.Button("Create Map"))
        {
            myScript.CreateMap();
        }
    }
}
