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

//class Program
//{
//    static void Main()
//    {
//        Bar("Baz");
//        var i = Foo<string>("Baz"); // what is the type of var?
//    }
//    static void Bar(dynamic d)
//    {
//        var i = Foo<string>(d); // what is the type of var?
//    }
//    static T Foo<T>(dynamic d)
//    {
//        return (T)d;
//    }
//}