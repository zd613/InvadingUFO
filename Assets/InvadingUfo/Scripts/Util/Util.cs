using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static float ToSignedAngle(float angle)
        => angle > 180 ? angle - 360 : angle;

}
