using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlaneInputProvider : BaseInputProvider
{
    public bool BulletAttack { get; protected set; }
    public bool MissileAttack { get; protected set; }
    public bool Boost { get; protected set; }
}
