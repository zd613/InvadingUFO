using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        Instantiate(gunPrefab, muzzleTransform.position, muzzleTransform.rotation);
        fireCoroutine = StartCoroutine(FireInterval());
        return true;
    }

    IEnumerator FireInterval()
    {
        yield return new WaitForSeconds(coolTimeSecond);
        fireCoroutine = null;
    }
}
