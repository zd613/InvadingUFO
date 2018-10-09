﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommonCore : MonoBehaviour
{
    public bool useInputProvider = true;
    public BaseInputProvider inputProvider;

    protected event Action Move;
    protected event Action<float, float> Rotate;

    public float Altitude { get { return transform.position.y; } }


    protected Movement movement;
    protected Rotation rotation;
    protected Attack attack;
    protected Health health;

    protected virtual void Awake()
    {
        if (useInputProvider && inputProvider == null)
        {
            Debug.Log("Input Provider is null.set input provider");
        }
        //
        movement = GetComponent<Movement>();
        rotation = GetComponent<Rotation>();
        attack = GetComponent<Attack>();
        health = GetComponent<Health>();
        //


    }

    protected virtual void Start()
    {
        if (health != null)
        {
            health.OnDied += () =>
            {
                GetComponent<Rigidbody>().isKinematic = true;
                if (movement != null)
                    movement.isActive = false;
                if (rotation != null)
                    rotation.isActive = false;
                if (attack != null)
                {
                    attack.isActive = false;
                    attack.crosshair?.gameObject?.SetActive(false);
                }

                var magnet = GetComponent<Magnet>();
                if (magnet != null)
                {
                    magnet.isActive = false;
                    magnet.attractEffectObject.SetActive(false);
                }


            };
        }
    }


    protected virtual void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
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

    protected void NoAction() { }
}
