using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

public class UfoAttack : MonoBehaviour
{

    [Header("プレハブ")]
    public GameObject gunPrefab;
    public Transform muzzleTransform;
    [Header("パラメータ")]
    public float coolTimeSecond;
    Coroutine fireCoroutine;


    public bool Fire()
    {
        if (fireCoroutine != null)
        {
            return false;
        }

        CreateBullet();

        fireCoroutine = StartCoroutine(FireInterval());
        return true;
    }

    void CreateBullet()
    {
        var obj = Instantiate(gunPrefab, muzzleTransform.position, muzzleTransform.rotation);
        var bullet = obj.GetComponent<Bullet>();
        bullet.Attacker = gameObject;
    }

    IEnumerator FireInterval()
    {
        yield return new WaitForSeconds(coolTimeSecond);
        fireCoroutine = null;
    }
}
