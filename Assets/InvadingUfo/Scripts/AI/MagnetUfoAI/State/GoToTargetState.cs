using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoToTargetState : MagnetUfoBaseState
{
    public Transform transform;
    public Transform target;
    public Vector3 offset;
    public Rigidbody rigidbody;
    public float eps = 1;

    PlanePitchYawController pitchYawController = new PlanePitchYawController();
    public float pitch;
    public float yaw;

    //event
    public event Action TargetNotFound;
    public event Action OnReached;
    public event Action UpdateAfterExecuted;




    public override void Execute()
    {
        if (target == null)
        {
            TargetNotFound?.Invoke();
            return;
        }
        pitchYawController.transform = transform;

        CheckArrival();

        pitchYawController.SetPitchYawLookingAt(target.position + offset);
        pitch = pitchYawController.Pitch;
        yaw = pitchYawController.Yaw;


        UpdateAfterExecuted?.Invoke();
    }


    void CheckArrival()
    {
        var distance = Vector3.Distance(transform.position, target.position + offset);
        if (distance < eps)
        {
            OnReached?.Invoke();
        }
    }

    public override string ToString()
    {
        return "Go To The Target";
    }
}
