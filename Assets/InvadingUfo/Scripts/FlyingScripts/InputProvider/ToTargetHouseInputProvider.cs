using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTargetHouseInputProvider : BaseInputProvider
{
    public House targetHouse;
    public Vector3 offset;

    public bool isArrived = false;
    float eps = 1;

    public event System.Action OnReached;

    private void Start()
    {
        CheckArrival();
    }

    public override void UpdateInputStatus()
    {
        if (targetHouse == null)
            return;
        LookAt(targetHouse.transform.position + offset);

        CheckArrival();
    }

    //到着判定
    void CheckArrival()
    {
        if (!isArrived)
            return;
        var distance = Vector3.Distance(transform.position, targetHouse.transform.position + offset);

        if (distance < eps)
        {
            GetComponent<Movement>().isActive = false;
            isArrived = true;
            OnReached?.Invoke();
        }
    }


}
