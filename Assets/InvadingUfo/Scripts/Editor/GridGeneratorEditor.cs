using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
//[CanEditMultipleObjects]
public class GridGeneratorEditor : Editor
{


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("作成"))
        {
            ((GridGenerator)target).MakeGrid();
        }
    }


}
