using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

public class UfoHealth : MonoBehaviour, IDamageable
{
    public float hp = 100;

    public void ApplyDamage(float damageValue, GameObject attacker)
    {
        hp -= damageValue;
        print("damaged:hp" + hp + " left");
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    //地面などに当たった時
    //一撃でhp0になる
    private void OnCollisionEnter(Collision collision)
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
