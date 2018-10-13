using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedObjectPosition : MonoBehaviour
{
    public GameObject fracturedParent;

    private void Update()
    {
        transform.position = CalculatePosition();
    }

    Vector3 CalculatePosition()
    {
        int count = 0;
        var ret = Vector3.zero;
        foreach (Transform item in fracturedParent.transform)
        {
            ret += item.transform.position;
            count++;
        }
        ret /= count;
        return ret;
    }
}
