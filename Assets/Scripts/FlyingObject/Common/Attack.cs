using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

public class Attack : MonoBehaviour
{
    [Header("プレハブ")]
    public GameObject gunPrefab;
    public Transform muzzleTransform;
    [Header("パラメータ")]
    public float coolTimeSecond;
    protected Coroutine fireCoroutine;


    public virtual bool Fire()
    {
        if (fireCoroutine != null)
        {
            return false;
        }

        CreateBullet();

        fireCoroutine = StartCoroutine(FireInterval());
        return true;
    }

    protected void CreateBullet()
    {
        var obj = Instantiate(gunPrefab, muzzleTransform.position, muzzleTransform.rotation);
        var bullet = obj.GetComponent<Bullet>();
        bullet.Attacker = gameObject;
    }

    protected IEnumerator FireInterval()
    {
        yield return new WaitForSeconds(coolTimeSecond);
        fireCoroutine = null;
    }
}
