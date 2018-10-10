using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Magnet : MonoBehaviour
{
    public bool isActive = true;
    public float maxLength = 100;
    public float power = 10;
    public GameObject attractEffectObject;

    public int AttractingObjectCount { get { return attractingObjects.Count; } }

    public OnTriggerEnterSender attractableObjectCatcher;
    public OnTriggerEnterSender enterSender;
    public OnTriggerExitSender exitSender;

    public bool isAttracting = false;
    GameObject target;
    Rigidbody targetRigidbody;
    List<AttractableObject> attractingObjects = new List<AttractableObject>();


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


        if (isAttracting)
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




    public void Attract()
    {
        if (!isActive)
            return;
        Attract2();
    }

    public void StartToAttract()
    {
        isAttracting = true;
        attractEffectObject.SetActive(true);
    }

    public void StopAttracting()
    {
        isAttracting = false;
        attractEffectObject.SetActive(false);
    }

    //add attractable objects to the list
    void EnterCollider(Collider collider)
    {
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        if (attractingObjects.Contains(attractable))
            return;

        attractingObjects.Add(attractable);
    }

    //remove attractable objects if exists
    void GetOutOfCollider(Collider collider)
    {
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        if (!attractingObjects.Contains(attractable))
            return;

        attractingObjects.Remove(attractable);
    }



    void Attract2()
    {
        foreach (var item in attractingObjects)
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
        attractingObjects = attractingObjects.Where(x => x != null).ToList();

    }


    void GetAttractableObject(Collider collider)
    {
        //Debug.Log(collider.name, this);
        //print(collider.name);
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        Destroy(collider.transform.gameObject);
        targetRigidbody = null;
    }
}
