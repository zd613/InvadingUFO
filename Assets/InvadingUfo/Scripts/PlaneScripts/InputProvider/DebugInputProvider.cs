using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInputProvider : BaseInputProvider
{
    System.Action updateAction;
    public DebugInputMode inputMode = DebugInputMode.All;

    private void Awake()
    {
        switch (inputMode)
        {
            case DebugInputMode.NoInput:
                updateAction = () => { };
                break;
            case DebugInputMode.OnlyMoving:
                updateAction = BoostInput;
                break;
            case DebugInputMode.MoveAndRotate:
                updateAction = RotationInput;
                break;
            case DebugInputMode.OnlyAttack:
                updateAction = AttackInput;
                break;
            case DebugInputMode.All:
                updateAction = AllInput;
                break;
            default:
                updateAction = () => { };
                break;
        }
    }

    protected override void UpdateInputStatus()
    {
        updateAction();
    }

    void RotationInput()
    {
        //Pitch
        PitchValue = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            PitchValue = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            PitchValue = -1;
        }

        //Yaw

        YawValue = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            YawValue = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            YawValue = -1;
        }
    }

    void BoostInput()
    {
        //boost 
        if (Input.GetKey(KeyCode.Space))
        {
            Boost = true;
        }
        else
        {
            Boost = false;
        }
    }

    void AttackInput()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            BulletAttack = true;
        }
        else
        {
            BulletAttack = false;
        }
    }

    void AllInput()
    {
        RotationInput();
        BoostInput();
        AttackInput();
    }

}

public enum DebugInputMode
{
    NoInput,
    OnlyMoving,
    OnlyAttack,
    MoveAndRotate,

    All,
}