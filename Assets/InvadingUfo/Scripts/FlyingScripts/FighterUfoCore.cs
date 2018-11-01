using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FighterUfoCore : BaseUfoCore
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
    protected Attack attack;

    protected virtual void Awake()
    {
        //base
        health = GetComponent<Health>();
        health.OnDeath += () =>
        {
            //magnet.StopAttracting();
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
        attack = GetComponent<Attack>();
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

                var attack = GetComponent<Attack>();
                if (attack != null)
                {
                    attack.isActive = false;
                }

                Destroy(gameObject, 20);
            };
        }

    }

    bool preSpecialKey1 = false;

    protected virtual void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);

        if (useInputProvider)
        {
            if (inputProvider.SpecialKey1)
            {
                if (attack != null)
                {
                    attack.Fire();
                }
            }

            if (attack != null)
            {
                if (preSpecialKey1 == false && inputProvider.SpecialKey1 == true)
                {
                    attack.StartShootSound();
                }
                if (preSpecialKey1 == true && inputProvider.SpecialKey1 == false)
                {
                    attack.StopShootSound();
                }
            }
            preSpecialKey1 = inputProvider.SpecialKey1;

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
