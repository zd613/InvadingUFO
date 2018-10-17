using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractState : UfoBaseState
{
    public GameObject target;
    public Magnet magnet;
    bool isAttracting = false;

    float timeout = 15;
    float time = 0;
    //event 
    public event System.Action OnAllObjectsAttracted;
    public event System.Action OnTimeout;
    int objectCounter = 0;

    public override void Execute()
    {
        if (!isAttracting)
        {
            StartAttracting();
        }

        //Debug.Log(magnet.AttractingObjectCount);

        if (magnet.AttractingObjectCount == 0)
        {
            StopToAttract();
            OnAllObjectsAttracted?.Invoke();

        }


        if (time > timeout)
        {
            StopToAttract();
            Debug.Log("attract timeout");

            OnTimeout?.Invoke();
            time = 0;
        }
        time += Time.deltaTime;
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
