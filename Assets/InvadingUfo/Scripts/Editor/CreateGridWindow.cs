using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace InvadingUfo.Editor
{
    public class CreateGridWindow : EditorWindow
    {
        [MenuItem("CustomEditor/Create Grid Window")]
        public static void Open()
        {
            GetWindow<CreateGridWindow>("create grid");
        }

        public GameObject gridPref;
        Material material1;
        Material material2;
        int countX = 10;
        int countZ = 10;
        int deltaX = 10;
        int deltaZ = 10;

        private void OnGUI()
        {
            GUILayout.Label("1タイルのゲームオブジェクト");
            gridPref = (GameObject)EditorGUILayout.ObjectField(gridPref, typeof(GameObject), allowSceneObjects: true);

            //material
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("material1");
                material1 = (Material)EditorGUILayout.ObjectField(material1, typeof(Material), allowSceneObjects: true);

                GUILayout.Label("material2");
                material2 = (Material)EditorGUILayout.ObjectField(material2, typeof(Material), allowSceneObjects: true);
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("タイルの個数");
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("X:");
                    countX = EditorGUILayout.IntField(countX);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Z:");
                    countZ = EditorGUILayout.IntField(countZ);

                }
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("タイルの中心間の距離");
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("X:");
                    deltaX = EditorGUILayout.IntField(deltaX);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Z:");
                    deltaZ = EditorGUILayout.IntField(deltaZ);

                }
            }


            if (GUILayout.Button("作成"))
            {
                if (gridPref == null)
                {
                    Debug.Log("error");
                    return;
                }
                else
                {
                    Create();
                }
            }
        }

        void Create()
        {
            var gridParent = new GameObject("Grid");
            float xPos = 0, zPos = 0;

            //x,zはcount

            Material material;

            for (int x = 0; x < countX; x++)
            {
                for (int z = 0; z < countZ; z++)
                {
                    var pos = new Vector3(xPos, 0, zPos);
                    var obj = Instantiate(gridPref, pos, Quaternion.identity);

                    if (x % 2 == 0)
                    {
                        if (z % 2 == 0)
                        {
                            material = material1;
                        }
                        else
                        {
                            material = material2;
                        }

                    }
                    else
                    {
                        if (z % 2 == 0)
                        {
                            material = material2;
                        }
                        else
                        {
                            material = material1;
                        }
                    }

                    obj.GetComponent<MeshRenderer>().material = material;
                    obj.transform.SetParent(gridParent.transform);


                    zPos += deltaZ;

                }
                xPos += deltaX;
                zPos = 0;
            }
        }
    }
}