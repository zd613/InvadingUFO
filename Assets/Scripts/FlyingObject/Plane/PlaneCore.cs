using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ame;

public class PlaneCore : MonoBehaviour
{
    public AbstractInputProvider inputProvider;

    event Action Move;
    event Action<float, float> Rotate;


    [Space]
    public Movement movement;
    public Rotation rotation;
    public UfoAttack ufoAttack;

    private void Start()
    {
        Move += movement.Move;
        Rotate += (pitch, yaw) => rotation.Rotate(pitch, yaw);
    }

    private void Update()
    {
        if (inputProvider.BulletAttack)
        {
            ufoAttack.Fire();
        }
    }

    private void FixedUpdate()
    {
        Rotate(inputProvider.PitchValue, inputProvider.YawValue);
        if (Move != null)
        {
            Move();
        }
    }

}
