using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTargetHouseInputProvider : BaseInputProvider
{
    public House targetHouse;
    public Vector3 offset;

    bool isArrived = false;
    float eps = 1;

    private void Start()
    {
        CheckArrival();
    }

    protected override void UpdateInputStatus()
    {
        if (targetHouse == null)
            return;
        LookAt(targetHouse.transform.position + offset);

        CheckArrival();
    }

    //到着判定
    void CheckArrival()
    {
        var distance = Vector3.Distance(transform.position, targetHouse.transform.position + offset);

        if (distance < eps)
        {
            GetComponent<Movement>().isActive = false;
            isArrived = true;
        }
    }


}
