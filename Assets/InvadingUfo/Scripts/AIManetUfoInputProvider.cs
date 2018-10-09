using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManetUfoInputProvider : BaseInputProvider
{
    Magnet magnet;


    House targetHouse;



    private void Start()
    {
        magnet = GetComponent<Magnet>();
    }

    protected override void UpdateInputStatus()
    {
        magnet.Attract();
    }

    void GoToTarget()
    {

    }
}
