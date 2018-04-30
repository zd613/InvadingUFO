using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

public class Health : MonoBehaviour, IDamagable
{
    public bool isAlive = true;
    public float hp = 100;

    public void ApplyDamage(float damageValue, GameObject attacker)
    {
        if (!isAlive)
        {
            return;
        }
        hp -= damageValue;
        print("damaged:hp" + hp + " left");
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
}
