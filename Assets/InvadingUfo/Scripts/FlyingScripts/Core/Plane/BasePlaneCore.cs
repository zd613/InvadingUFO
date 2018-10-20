using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasePlaneCore : BaseCore
{
    [Header("BasePlaneCore")]
    public BasePlaneInputProvider inputProvider;

    protected event Action Move;
    protected event Action<float, float> Rotate;

    public Rotation Rotation { get { return rotation; } }

    //event
    public event Action OnDeath;
    //

    protected Movement movement;
    protected Rotation rotation;
    protected Attack attack;


    //TODO:一般的なやつに変更する
    protected Ame.PlayerMissileAttack playerMissileAttack;

    protected virtual void Awake()
    {
        //base
        health = GetComponent<Health>();
        health.OnDeath += () => OnDeath?.Invoke();


        //
        if (useInputProvider && inputProvider == null)
        {
            Debug.Log("Input Provider is null.set input provider", this);
        }
        //
        movement = GetComponent<Movement>();
        rotation = GetComponent<Rotation>();
        attack = GetComponent<Attack>();
        playerMissileAttack = GetComponent<Ame.PlayerMissileAttack>();


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
                if (attack != null)
                {
                    attack.isActive = false;
                    if (attack.crosshair != null)
                    {
                        attack.crosshair.gameObject.SetActive(false);
                    }
                }

                var magnet = GetComponent<Magnet>();
                if (magnet != null)
                {
                    magnet.attractAreaCollider.enabled = false;
                    magnet.isActive = false;
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

        if (inputProvider.MissileAttack)
        {
            if (playerMissileAttack != null)
            {
                playerMissileAttack.Fire();
            }
        }

        //fixed updateで動かすとカクカクした
        if (rotation != null)
        {
            rotation.Rotate(inputProvider.PitchValue, inputProvider.YawValue);
        }

        if (movement != null)
        {
            movement.Move();
        }
    }

}
