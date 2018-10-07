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
    public AudioSource explosionSound;


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
                Destroy(gameObject);
            }
            OnDied?.Invoke();
        }
    }

    void Explode()
    {
        if (!canExplode)
            return;
        nonFracturedObjectParent.SetActive(false);
        fracturedObjectsParent.SetActive(true);

        foreach (Transform t in fracturedObjectsParent.transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * UnityEngine.Random.Range(minForce, maxForce));
                rb.AddExplosionForce(UnityEngine.Random.Range(minForce, maxForce),
                    transform.position, radius);
            }
        }



        return;
        if (explosion != null)
        {
            var ex = Instantiate(explosion, transform.position, transform.rotation);
            ex.transform.localScale = ex.transform.localScale * 5;//大きさ適当
        }

        //sound
        if (explosionSound != null)
        {
            explosionSound.Play();
            float soundTime = explosionSound.clip.length;

            subRoot.gameObject.SetActive(false);

            Destroy(gameObject, soundTime);
        }
        else
        {

        }
    }

    //地面などに当たった時
    //一撃でhp0になる
    //protected virtual void OnCollisionEnter(Collision collision)
    //{
    //    var bullet = collision.gameObject.GetComponent<Bullet>();
    //    if (bullet != null)
    //    {
    //        if (bullet.Attacker == gameObject)
    //            return;
    //    }
    //    ApplyDamage(hp, collision.gameObject);

    //}

    protected void ChangeHpBar()
    {
        if (hpSlider == null)
            return;
        hpSlider.value = hp / maxHp;

    }
}
