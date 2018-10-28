using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour, Ame.IDamageable
{
    public GameObject nonFractured;
    public GameObject fractured;


    public bool canExplode = false;
    public float explosionForce = 20;
    public float explosionRadius;

    bool isFractured = false;

    public float hp = 200;

    public void FractureObject()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        nonFractured.SetActive(false);
        fractured.SetActive(true);


        if (canExplode)
        {
            Explode(transform.position);
        }
        isFractured = true;
        Destroy(gameObject, 15);
    }

    private void Explode(Vector3 position)
    {
        foreach (Transform item in fractured.transform)
        {
            item.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, position, explosionRadius);
        }
    }

    public void ApplyDamage(float damageValue, GameObject attacker)
    {
        hp -= damageValue;
        if (hp <= 0)
        {
            FractureObject();
        }
    }
}
