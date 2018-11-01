using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasePlaneCore : BaseCore
{
    [Header("BasePlaneCore")]
    public BasePlaneInputProvider inputProvider;

    public Rotation Rotation { get { return rotation; } }

    //event
    public event Action OnDeath;
    //

    protected Movement movement;
    protected Rotation rotation;
    protected Attack attack;


    //TODO:一般的なやつに変更する
    protected Ame.PlayerMissileAttack playerMissileAttack;


    //TODO:あとで適切な場所に移動
    public List<GameObject> missileObjectToHideOnDeath;

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


                var missileAttack = GetComponent<Ame.PlayerMissileAttack>();
                if (missileAttack != null)
                {
                    missileAttack.isActive = false;
                }

                foreach (var item in missileObjectToHideOnDeath)
                {
                    item.SetActive(false);
                }

                Destroy(gameObject, 15);
            };
        }

        mainCamera = Camera.main;

    }


    bool preAttack = false;

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

        //attack sound

        if (attack != null)
        {

            if (preAttack == false && inputProvider.BulletAttack == true)
            {
                attack.StartShootSound();
            }
            if (preAttack == true && inputProvider.BulletAttack == false)
            {
                attack.StopShootSound();
            }
        }
        preAttack = inputProvider.BulletAttack;

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
            rotation.CancelRotationByCollisionHit();
        }

        if (movement != null)
        {
            movement.Move(inputProvider.Boost);
        }

        if (gameObject.tag == "Player")
        {
            if (inputProvider.Boost)
            {
                f -= fspeed * Time.deltaTime;
            }
            else
            {
                f += fspeed * Time.deltaTime;
            }
            f = Mathf.Clamp(f, 25, 40);
            mainCamera.focalLength = f;
        }
    }

    Camera mainCamera;

    //プレイヤーのブースとじのカメラ操作
    float f = 40;
    public float fspeed = 10;
    public float minFocalLength = 25;
    public float maxFocalLength = 40;
}
