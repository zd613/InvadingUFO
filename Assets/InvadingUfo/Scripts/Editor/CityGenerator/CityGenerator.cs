using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CityGenerator : EditorWindow
{

    [MenuItem("Custom/CityGenerator")]
    public static void Open()
    {
        var window = EditorWindow.GetWindow<CityGenerator>("City Generator");

    }

    private void OnGUI()
    {

    }
}
