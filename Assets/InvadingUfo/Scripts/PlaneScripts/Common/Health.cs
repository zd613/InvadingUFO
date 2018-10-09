using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour, IDamageable
{
    public bool isAlive = true;
    public float hp = 100;
    private float maxHp;
    public GameObject subRoot;

    [Header("UI")]
    //public Image hpBar;
    public Slider hpSlider;

    public event Action OnDamageTaken;
    public event Action OnDied;

    public float MaxHp { get { return maxHp; } }

    [Header("爆発")]
    public bool canExplode = false;
    public GameObject nonFracturedObjectParent;
    public GameObject fracturedObjectsParent;
    public float minForce = 10;
    public float maxForce = 20;
    public float radius;
    public GameObject explosion;
    public Vector3 explosionOffset;
    public GameObject smoke;
    public float minSmokeDuration = 10;
    public float maxSmokeDuration = 15;
    public int maxSmokes = 8;
    public AudioSource explosionSound;

    [Header("接触判定")]
    public bool canCallOnCollisionEnter = false;


    protected virtual void Awake()
    {
        maxHp = hp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyDamage(10, null);
        }
    }


    public void ApplyDamage(float damageValue, GameObject attacker)
    {
        //print(damageValue + "," + attacker.name);
        //print(gameObject.name);
        if (!isAlive)
        {
            return;
        }
        hp -= damageValue;

        ChangeHpBar();

        OnDamageTaken?.Invoke();

        //killed
        if (hp <= 0)
        {
            isAlive = false;

            if (canExplode)
            {
                Explode();
            }
            else
            {
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
            OnDied?.Invoke();
        }
    }

    void Explode()
    {
        nonFracturedObjectParent.SetActive(false);
        fracturedObjectsParent.SetActive(true);

        var move = GetComponent<Movement>();
        float speed = move.speed;


        int smokeCounter = 0;
        foreach (Transform t in fracturedObjectsParent.transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * speed;
                // rb.AddForce(transform.forward * UnityEngine.Random.Range(minForce, maxForce));
                rb.AddExplosionForce(UnityEngine.Random.Range(minForce, maxForce),
                    transform.position, radius);

                if (smoke != null)
                {
                    if (/*UnityEngine.Random.Range(0, 2) == 1 &&*/ smokeCounter < maxSmokes)
                    {
                        var parent = rb.transform;
                        var smokeInstance = Instantiate(smoke, parent.transform.position, transform.rotation);
                        smokeInstance.transform.parent = parent.transform;

                        var particleSystem = smokeInstance.GetComponentInChildren<ParticleSystem>();
                        particleSystem.Stop();
                        var newDuration = UnityEngine.Random.Range(minSmokeDuration, maxSmokeDuration);
                        var main = particleSystem.main;
                        main.duration = newDuration;

                        particleSystem.Play();
                        Destroy(smokeInstance, main.duration);
                    }

                }
            }
        }


        if (explosion != null)
        {
            var ex = Instantiate(explosion, transform.position + explosionOffset, transform.rotation);
        }
        //sound
        if (explosionSound != null)
        {
            explosionSound.Play();
            float soundTime = explosionSound.clip.length;

            subRoot.gameObject.SetActive(false);

            Destroy(gameObject, soundTime);
        }


        //player
        var ob = GetComponentInParent<CommonCore>().gameObject;
        if (ob.tag == "Player")
        {
            var fc = Camera.main.gameObject.GetComponent<FollowingCamera>();
            fc.target = fracturedObjectsParent.GetComponentsInChildren<Transform>()[1];
        }
    }

    //地面などに当たった時
    //一撃でhp0になる
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!canCallOnCollisionEnter)
            return;

        KillInstantly(collision.gameObject);
    }

    public void KillInstantly(GameObject attacker)
    {
        ApplyDamage(hp, attacker);
    }

    protected void ChangeHpBar()
    {
        if (hpSlider == null)
            return;
        hpSlider.value = hp / maxHp;

    }
}
