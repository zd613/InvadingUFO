using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommonCore : MonoBehaviour
{
    public AbstractInputProvider inputProvider;

    protected event Action Move;
    protected event Action<float, float> Rotate;



    protected Movement movement;
    protected Rotation rotation;
    protected Attack attack;

    protected virtual void Awake()
    {
        if (inputProvider == null)
        {
            Debug.Log("Input Provider is null.set input provider");
        }
        //
        movement = GetComponent<Movement>();
        rotation = GetComponent<Rotation>();
        attack = GetComponent<Attack>();
        //
    }


    protected virtual void Update()
    {
        if (inputProvider.BulletAttack)
        {
            if (attack != null)
            {
                attack.Fire();
            }
        }

        if (rotation != null)
        {
            rotation.Rotate(inputProvider.PitchValue, inputProvider.YawValue);
        }

        if (movement != null)
        {
            movement.Move();
        }
    }

    //protected virtual void FixedUpdate()
    //{

    //}

    protected void NoAction() { }
}
