using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour, Ame.IDamageable, IFinancialDamage
{
    public GameObject nonFractured;
    public GameObject fractured;


    public bool canExplode = false;
    public float explosionForce = 20;
    public float explosionRadius;

    bool isFractured = false;

    public float hp = 200;
    public float destroyDelay = 15;

    public AudioSource fractureSound;

    //event
    public event Action<long> OnFinancialDamageOccured;

    public void FractureObject()
    {

        if (isFractured)
            return;
        if (fractureSound != null)
        {
            print("player");

            fractureSound.Play();

        }
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
        //OnFractured?.Invoke();
        OnFinancialDamageOccured?.Invoke(GetComponent<Price>().price);

        Destroy(gameObject, destroyDelay);
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
