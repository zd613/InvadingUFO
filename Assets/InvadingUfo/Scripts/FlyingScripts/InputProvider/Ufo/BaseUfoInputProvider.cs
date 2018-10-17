using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUfoInputProvider : BaseInputProvider
{
    public bool SpecialKey1 { get; protected set; }
    public bool SpecialKey2 { get; protected set; }



    public override void UpdateInputStatus()
    {
        //bullet
        if (Input.GetKey(KeyCode.Z))
        {
            SpecialKey1 = true;
        }
        else
        {
            SpecialKey1 = false;
        }

        //missile
        if (Input.GetKey(KeyCode.X))
        {
            SpecialKey2 = true;
        }
        else
        {
            SpecialKey2 = false;
        }


        //Pitch
        PitchValue = Input.GetAxis("Vertical");
        YawValue = Input.GetAxis("Horizontal");
    }
}
