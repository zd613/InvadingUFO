using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour, IDamagable
{
    public bool isAlive = true;
    public float hp = 100;
    private float maxHp;

    [Header("UI")]
    public Image hpBar;

    public event Action OnDamageTaken;

    public float MaxHp { get { return maxHp; } }

    protected virtual void Awake()
    {
        maxHp = hp;
    }

    public void ApplyDamage(float damageValue, GameObject attacker)
    {
        if (!isAlive)
        {
            return;
        }
        hp -= damageValue;

        ChangeHpBar();
        print("damaged:hp" + hp + " left");
        if (OnDamageTaken != null)
        {
            OnDamageTaken();
        }

        //killed
        if (hp <= 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
    }

    //地面などに当たった時
    //一撃でhp0になる
    protected virtual void OnCollisionEnter(Collision collision)
    {
        var bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (bullet.Attacker == gameObject)
                return;
        }
        ApplyDamage(hp, collision.gameObject);

    }

    protected void ChangeHpBar()
    {
        if (hpBar == null)
            return;
        hpBar.fillAmount = hp / maxHp;
    }
}
