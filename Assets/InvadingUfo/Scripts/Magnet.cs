using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Magnet : MonoBehaviour
{
    public OnTriggerEnterSender attractableObjectCatcher;
    public float maxLength = 100;
    public float power = 10;

    public OnTriggerEnterSender enterSender;
    public OnTriggerExitSender exitSender;

    private void Start()
    {
        attractableObjectCatcher.OnTriggerEnterCalled += GetAttractableObject;
        enterSender.OnTriggerEnterCalled += EnterCollider;
        exitSender.OnTriggerExitCalled += GetOutOfCollider;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * maxLength);
        UpdateAttractingList();

        if (Input.GetKey(KeyCode.X))
        {
            //Attract();
            Attract2();
        }
        else
        {
            if (targetRigidbody != null)
            {
                //targetRigidbody.useGravity = true;
                targetRigidbody = null;
            }
        }

    }

    GameObject target;
    Rigidbody targetRigidbody;


    public void Attract()
    {
        Attract2();
    }

    //TODO:
    void Attract1()
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Attractable");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance: maxLength, layerMask: layerMask))
        {
            var attractable = hit.transform.GetComponentInParent<IAttractable>();
            print(hit.transform.name);
            print(attractable == null ? "null" : "not");
            if (attractable == null)
                return;

            if (targetRigidbody == null)//null
            {
                targetRigidbody = hit.transform.GetComponent<Rigidbody>();
                if (targetRigidbody == null)
                    return;

                //targetRigidbody.useGravity = false;
            }
            else if (target != hit.transform.gameObject)//change target
            {
                //targetRigidbody.useGravity = true;


                targetRigidbody = hit.transform.GetComponent<Rigidbody>();

            }

            targetRigidbody.AddForce(Vector3.up * power);


        }

    }

    //add attractable objects to the list
    void EnterCollider(Collider collider)
    {
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        if (attractableObjects.Contains(attractable))
            return;

        attractableObjects.Add(attractable);
    }

    //remove attractable objects if exists
    void GetOutOfCollider(Collider collider)
    {
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        if (!attractableObjects.Contains(attractable))
            return;

        attractableObjects.Remove(attractable);
    }


    List<AttractableObject> attractableObjects = new List<AttractableObject>();

    void Attract2()
    {
        foreach (var item in attractableObjects)
        {
            if (item == null)
                continue;
            if (item.gameObject == null)
                continue;

            item.Attract(transform, power);
        }
    }

    void UpdateAttractingList()
    {
        attractableObjects = attractableObjects.Where(x => x != null).ToList();

    }


    void GetAttractableObject(Collider collider)
    {
        //Debug.Log(collider.name, this);
        //print(collider.name);
        Destroy(collider.transform.gameObject);
        targetRigidbody = null;
    }



    class C
    {
        public IAttractable IAttractable;
        public GameObject GameObject;
    }
}
