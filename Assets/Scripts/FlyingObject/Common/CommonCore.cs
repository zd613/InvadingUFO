using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommonCore : MonoBehaviour
{
    public AbstractInputProvider inputProvider;

    event Action Move;
    event Action<float, float> Rotate;


    
    Movement movement;
    Rotation rotation;
    Attack attack;

    protected virtual void Awake()
    {
        //
        movement = GetComponent<Movement>();
        rotation = GetComponent<Rotation>();
        attack = GetComponent<Attack>();
        //

        if (movement == null)
        {
            Move += () => NoAction();
        }
        else
        {
            Move += movement.Move;
        }


        if (rotation == null)
        {
            Rotate += 
                (pitch, yaw) => NoAction();
        }
        else
        {
            Rotate += 
                (pitch, yaw) => rotation.Rotate(pitch, yaw);
        }
    }


    protected virtual void Update()
    {
        if (inputProvider.BulletAttack)
        {
            attack.Fire();
        }
    }

    protected virtual void FixedUpdate()
    {
        Rotate(inputProvider.PitchValue, inputProvider.YawValue);
        if (Move != null)
        {
            Move();
        }
    }

    protected void NoAction() { }
}
