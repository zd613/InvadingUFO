using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FindTargetState : UfoBaseState
{
    public HouseManager houseManager;
    public House targetHouse;

    public event Action OnTargetNotFound;
    public event Action OnTargetFound;

    public override void Execute()
    {
        targetHouse = houseManager?.GetRandomHouse();
        if (targetHouse == null)
        {
            OnTargetNotFound?.Invoke();
        }
        else
        {
            OnTargetFound?.Invoke();

        }

    }

    public override string ToString()
    {
        return "Find Target";
    }
}
