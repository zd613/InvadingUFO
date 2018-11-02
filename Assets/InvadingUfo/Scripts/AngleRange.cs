using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AngleRange
{
    [SerializeField]
    private float max;

    public float Max
    {
        get { return max; }
        set { max = value; }
    }

    [SerializeField]
    private float min;

    public float Min
    {
        get { return min; }
        set { min = value; }
    }


    public AngleRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    //範囲内にangleがあるかどうか境界は入ってる
    public bool IsIn(float angle)
    {
        return (min <= angle && angle <= max);
    }

    //angleRangeの中にangleが入ってるかどうか境界は入ってる
    public static bool IsIn(AngleRange angleRange, float angle)
    {
        return (angleRange.Min <= angle && angle <= angleRange.Max);
    }
}
