using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

public class PathController : MonoBehaviour
{
    public List<Path> paths;
    public bool isCirclePath;
    [Header("path のオブジェクト設定")]
    public Material material;
    public float size = 1;

    public bool HasPath
    {
        get { if (paths == null) print("null"); return paths.Count > 0; }
    }


    public Path GetNextPath(int currentNumber)
    {
        var next = currentNumber + 1;
        if (next >= paths.Count)
        {
            if (isCirclePath)
            {
                next = 0;
            }
            else
            {
                return null;
            }
        }
        return paths[next];
    }

    private void OnValidate()
    {
        foreach (var item in paths)
        {
            //size
            var scale = item.transform.localScale;
            scale = new Vector3(size, size, size);
            item.transform.localScale = scale;

            //material
            if (material != null)
            {
                var rs = item.GetComponentsInChildren<Renderer>();
                foreach (var renderers in rs)
                {
                    renderers.material = material;
                }
            }

        }

    }
}
