using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletsInPoolOnAwake = 200;

    Stack<Bullet> pool;

    private void Awake()
    {
        pool = new Stack<Bullet>(bulletsInPoolOnAwake);

        for (int i = 0; i < bulletsInPoolOnAwake; i++)
        {
            var bullet = CreateBullet(isActive: false);
            bullet.pool = this;
            pool.Push(bullet);
        }
    }

    Bullet CreateBullet(bool isActive = true)
    {
        var obj = Instantiate(bulletPrefab, parent: transform);
        if (isActive)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
        return obj.GetComponent<Bullet>();
    }


    public Bullet GetBullet()
    {
        if (pool.Count == 0)
        {
            return CreateBullet();
        }
        var obj = pool.Pop();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        pool.Push(bullet);
    }

}
