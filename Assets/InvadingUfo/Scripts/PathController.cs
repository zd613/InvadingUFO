using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public List<Path> paths;
    public bool isCirclePath;

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
}
