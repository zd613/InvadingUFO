using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagnetUfoCore : BaseUfoCore
{
    [Header("BasePlaneCore")]
    public BaseUfoInputProvider inputProvider;

    public GameObject ufoGameObject;
    public float rotationSpeed = 20;

    public Rotation Rotation { get { return rotation; } }

    //event
    public event Action OnDeath;
    //

    protected Movement movement;
    protected Rotation rotation;
    protected Magnet magnet;

    protected virtual void Awake()
    {
        //base
        health = GetComponent<Health>();
        health.OnDeath += () =>
        {
            magnet.StopAttracting();
            OnDeath?.Invoke();
        };


        //
        if (useInputProvider && inputProvider == null)
        {
            Debug.Log("Input Provider is null.set input provider", this);
        }
        //
        movement = GetComponent<Movement>();
        rotation = GetComponent<Rotation>();
        magnet = GetComponent<Magnet>();
    }

    protected virtual void Start()
    {
        //base core
        if (health != null)
        {
            health.OnDeath += () =>
            {
                GetComponent<Rigidbody>().isKinematic = true;
                if (movement != null)
                    movement.isActive = false;
                if (rotation != null)
                    rotation.isActive = false;

                var magnet = GetComponent<Magnet>();
                if (magnet != null)
                {
                    magnet.attractAreaCollider.enabled = false;
                    magnet.isActive = false;
                }

                Destroy(gameObject, 30);
            };
        }

    }


    protected virtual void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);

        if (useInputProvider)
        {
            if (inputProvider.SpecialKey1)
            {
                if (magnet != null)
                {
                    magnet.Attract();
                }
            }

            if (rotation != null)
            {
                rotation.CancelRotationByCollisionHit();
                rotation.Rotate(inputProvider.PitchValue, inputProvider.YawValue);
            }

            if (movement != null)
            {
                movement.Move();
            }
        }
        Animation();
    }

    void Animation()
    {
        if (ufoGameObject == null)
            return;
        if (!health.isAlive)
            return;
        ufoGameObject.transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
    }
}