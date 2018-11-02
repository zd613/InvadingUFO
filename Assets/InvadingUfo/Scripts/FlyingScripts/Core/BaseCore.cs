using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCore : MonoBehaviour
{
    [Header("Base Core設定")]
    public bool useInputProvider = true;

    //property
    public bool IsAlive { get { return health.isAlive; } }
    public Health Health { get { return health; } }
    public float Altitude { get { return transform.position.y; } }

    protected Health health;

}
