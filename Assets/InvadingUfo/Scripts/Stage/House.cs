using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : AttractableObject
{
    //public GameObject house;
    //public GameObject collapsedHouse;

    //bool hasCollapsed = false;


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Stage"))
    //    {
    //        return;
    //    }

    //    Collapse();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    //print(other.name);

    //    if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
    //    {
    //        Collapse();
    //    }
    //}

    //void Collapse()
    //{
    //    if (hasCollapsed)
    //        return;
    //    //print("collapsed");

    //    var rb = GetComponent<Rigidbody>();
    //    Destroy(rb);
    //    house.SetActive(false);
    //    collapsedHouse.SetActive(true);
    //}
}
