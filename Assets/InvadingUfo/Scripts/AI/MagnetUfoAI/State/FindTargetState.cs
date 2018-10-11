using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FindTargetState : UfoBaseState
{
    public HouseManager houseManager;
    public House targetHouse;

    public event Action ProcessFinished;

    public override void Execute()
    {
        targetHouse = houseManager.GetRandomHouse();
        ProcessFinished?.Invoke();
        
    }

    public override string ToString()
    {
        return "Find Target";
    }
}
