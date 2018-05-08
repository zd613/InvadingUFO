using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Limit
{
    [SerializeField]
    private float min;

    public float Min
    {
        get { return min; }
    }

    [SerializeField]
    private float max;

    public float Max
    {
        get { return max; }
    }

    public Limit(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

}
