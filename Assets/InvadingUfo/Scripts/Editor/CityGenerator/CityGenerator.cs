using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ame.GeneratorEditors
{
    public class CityGenerator : EditorWindow
    {

        [MenuItem("Custom/CityGenerator")]
        public static void Open()
        {
            EditorWindow.GetWindow<CityGenerator>("City Generator");

        }

        private void OnGUI()
        {

        }
    }
}