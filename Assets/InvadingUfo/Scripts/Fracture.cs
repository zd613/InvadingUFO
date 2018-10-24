using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    public GameObject nonFractured;
    public GameObject fractured;


    public bool canExplode = false;
    public float explosionForce = 20;
    public float explosionRadius;

    private void Awake()
    {

    }

    bool isFractured = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isFractured)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            nonFractured.SetActive(false);
            fractured.SetActive(true);


            if (canExplode)
            {
                Explode(other.ClosestPointOnBounds(transform.position));
            }
            isFractured = true;
        }
    }

    //public void Execute()
    //{
    //    nonFractured.SetActive(false);
    //    fractured.SetActive(true);


    //    if (canExplode)
    //    {
    //        Explode(other.ClosestPointOnBounds(transform.position));
    //    }
    //}


    private void Explode(Vector3 position)
    {
        foreach (Transform item in fractured.transform)
        {
            item.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, position, explosionRadius);
        }
    }
}
