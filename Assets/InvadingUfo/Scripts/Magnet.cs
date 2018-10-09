using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Magnet : MonoBehaviour
{
    public bool isActive = true;
    public OnTriggerEnterSender attractableObjectCatcher;
    public float maxLength = 100;
    public float power = 10;
    public GameObject attractEffectObject;

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
        if (!isActive)
            return;
        Debug.DrawRay(transform.position, Vector3.down * maxLength);
        UpdateAttractingList();

        if (Input.GetKey(KeyCode.X))
        {
            Attract();
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
        if (!isActive)
            return;
        Attract2();
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
