using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlaneInputProvider : BasePlaneInputProvider
{
    public Transform target;
    public Collider targetSearchCollider;
    public OnTriggerEnterSender targetSearchSender;
    //update
    private void Awake()
    {
        //targetSearchSender.OnTriggerEnterCalled+=
    }

    private List<Transform> targets;

    public override void UpdateInputStatus()
    {
        if (target == null)
        {
            return;
        }



    }
   
}
