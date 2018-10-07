using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ame;

namespace Ame
{
    [CustomEditor(typeof(PathController))]
    [CanEditMultipleObjects]
    public class PathControllerEditor : Editor
    {
        GameObject scriptObject;

        private void OnEnable()
        {
            scriptObject = ((PathController)target).gameObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            if (GUILayout.Button("Create Path"))
            {
                CreatePath();
            }

            if (GUILayout.Button("Create Circle Path"))
            {
                CreatePath(circle: true);
            }

            if(GUILayout.Button("Path Sphere Renderer (On Or Off)"))
            {
                Toggle();
            }
        }

        void Toggle()
        {
            foreach (Transform item in scriptObject.transform)
            {
                var meshRenderer = item.GetChild(0).GetComponent<MeshRenderer>();
                meshRenderer.enabled = !meshRenderer.enabled;
                EditorUtility.SetDirty(meshRenderer);
            }
        }

        void CreatePath(bool circle = false)
        {
            var script = scriptObject.GetComponent<PathController>();
            script.paths.Clear();
            //foreach (Transform item in scriptObject.transform)
            //{
            //    var path = item.GetComponent<Path>();
            //    path.previous = null;
            //    path.next = null;
            //}
            script.isCirclePath = circle;

            int num = 0;

            Path pre = null;
            foreach (Transform item in scriptObject.transform)
            {
                var path = item.GetComponent<Path>();
                path.number = num;

                path.previous = pre;
                if (pre != null)
                {
                    pre.next = path;
                }
                script.paths.Add(path);

                pre = path;
                num++;

                EditorUtility.SetDirty(path);
                EditorUtility.SetDirty(pre);
            }


            if (circle)
            {
                if (script.paths.Count > 1)
                {
                    var first = script.paths[0];
                    var last = script.paths[script.paths.Count - 1];
                    first.previous = last;
                    last.next = first;
                }
            }
            else
            {
                script.paths[0].previous = null;
                script.paths[script.paths.Count - 1].next = null;
            }
            EditorUtility.SetDirty(script);

        }
    }
}