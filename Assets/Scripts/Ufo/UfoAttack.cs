using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoAttack : MonoBehaviour
{

    [Header("プレハブ")]
    public GameObject gunPrefab;
    public Transform muzzleTransform;

    public void Fire()
    {
        Instantiate(gunPrefab, muzzleTransform.position, muzzleTransform.rotation);

    }
}
