using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbstractInputProvider : MonoBehaviour {
    public bool BulletAttack { get; protected set; }
    public float PitchValue { get; protected set; }
    public float YawValue { get; protected set; }
    public bool Boost { get; protected set; }
    
}
