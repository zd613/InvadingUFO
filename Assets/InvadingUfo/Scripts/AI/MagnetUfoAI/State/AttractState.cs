using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractState : UfoBaseState
{
    public Magnet magnet;
    bool isAttracting = false;


    //event 
    public event System.Action OnAllObjectsAttracted;
    int objectCounter = 0;

    public override void Execute()
    {
        if (!isAttracting)
        {
            StartAttracting();
        }

        Debug.Log(magnet.AttractingObjectCount);

        if (magnet.AttractingObjectCount == 0)
        {
            StopToAttract();
            OnAllObjectsAttracted?.Invoke();

        }
    }

    void StartAttracting()
    {
        magnet.StartToAttract();
        isAttracting = true;
    }

    void StopToAttract()
    {
        magnet.StopAttracting();
        isAttracting = false;
    }

    public override string ToString()
    {
        return "Attract";
    }
}
