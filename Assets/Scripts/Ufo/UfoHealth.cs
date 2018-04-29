using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

public class UfoHealth : MonoBehaviour, IDamagable
{
    public float hp = 100;

    public void ApplyDamage(float damageValue, GameObject attacker)
    {
        hp -= damageValue;
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
