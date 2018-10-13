using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputProvider : BaseInputProvider
{
    StateContext context = new StateContext();

    BoidsState boidsState = new BoidsState();
    public BoidsManager boidsManager;
    public float turbulence = 0.1f;


    private void Start()
    {

        boidsState.UpdateAfterExecuted += () =>
        {
            PitchValue = boidsState.pitch;
            YawValue = boidsState.yaw;
        };
        boidsState.transform = transform;
        boidsState.boidsManager = boidsManager;
        boidsState.Turbulence = turbulence;
        context.State = boidsState;


    }


    public override void UpdateInputStatus()
    {
        PitchValue = 0;
        YawValue = 0;

        context.Execute();
        //print(PitchValue + "," + YawValue);

    }
}
