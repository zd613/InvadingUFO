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
    public event Action OnDeath;

    public float MaxHp { get { return maxHp; } }

    [Header("爆発")]
    public bool canExplode = false;
    public GameObject nonFracturedObjectParent;
    public GameObject fracturedObjectsParent;
    public FracturedObjectPosition fracturedCenter;
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
    public StageDamageMode stageDamageMode = StageDamageMode.TakeDamage;

    Rigidbody rb;
    protected virtual void Awake()
    {
        maxHp = hp;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ApplyDamage(10, null);
        //}

        //衝突時にダメージ受ける場合　回転速度などをなくす
        var v = rb.velocity;
        if (!Mathf.Approximately(v.x, 0) || !Mathf.Approximately(v.y, 0) || !Mathf.Approximately(v.z, 0))
        {
            rb.velocity = Vector3.zero;
        }

        v = rb.angularVelocity;
        if (!Mathf.Approximately(v.x, 0) || !Mathf.Approximately(v.y, 0) || !Mathf.Approximately(v.z, 0))
        {
            rb.angularVelocity = Vector3.zero;
        }

    }

    public void ApplyDamage(float damageValue, GameObject attacker)
    {
        if(attacker==gameObject)
        {
            print("damage" + damageValue + "," + attacker.name);

        }
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
            OnDeath?.Invoke();
        }
    }

    void Explode()
    {
        nonFracturedObjectParent.SetActive(false);
        fracturedObjectsParent.SetActive(true);

        var move = GetComponent<Movement>();
        float speed = move.speed;
        if (fracturedCenter != null)
            fracturedCenter.enabled = true;

        int smokeCounter = 0;
        foreach (Transform t in fracturedObjectsParent.transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (move.isActive)
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


    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!canCallOnCollisionEnter)
            return;

        //if (collision.gameObject.layer == LayerMask.NameToLayer("Stage"))
        if (stageDamageMode == StageDamageMode.TakeDamage)
        {
            ApplyDamage(StageData.DamageValue, collision.gameObject);
        }
        else
        {
            KillInstantly(collision.gameObject);
        }
    }

    
    protected virtual void OnCollisionStay(Collision collision)
    {
        if (stageDamageMode == StageDamageMode.TakeDamage)
        {
            ApplyDamage(StageData.DamageValue, collision.gameObject);
        }
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
