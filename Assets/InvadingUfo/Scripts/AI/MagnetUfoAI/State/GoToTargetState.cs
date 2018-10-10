using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoToTargetState : MagnetUfoBaseState
{
    public Transform myTransform;
    public Transform target;
    public float eps = 1;

    //event
    public event Action TargetNotFound;
    public event Action OnReached;

    public override void Execute()
    {
        if (target == null)
        {
            TargetNotFound?.Invoke();
            return;
        }

        var distance = Vector3.Distance(myTransform.position, target.position);
        if (distance < eps)
        {
            OnReached?.Invoke();
        }



    }
}
