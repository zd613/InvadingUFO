using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Magnet : MonoBehaviour
{
    public bool isActive = true;

    [Header("設定")]
    public bool canAttractMultipleObjects = true;
    public float power = 10;
    public GameObject attractEffectObject;

    public int AttractingObjectCount
    {
        get
        {
            if (canAttractMultipleObjects)
                return attractingObjects.Count;
            else
                return target == null ? 0 : 1;
        }

    }

    public OnTriggerEnterSender attractableObjectCatcher;
    public OnTriggerEnterSender enterSender;
    public OnTriggerExitSender exitSender;

    public bool isAttracting = false;
    public AttractableObject target;
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

        Debug.DrawRay(transform.position, Vector3.down * 3);


        UpdateAttractingList();


        if (isAttracting)
        {
            Attract();
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

        if (canAttractMultipleObjects)
        {
            if (attractingObjects.Contains(attractable))
                return;

            attractingObjects.Add(attractable);
        }
        else
        {
            if (target == null)
            {
                target = attractable;
            }
        }
    }

    //remove attractable objects if exists
    void GetOutOfCollider(Collider collider)
    {
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        if (canAttractMultipleObjects)
        {
            if (!attractingObjects.Contains(attractable))
                return;

            attractingObjects.Remove(attractable);
        }
        else
        {
            if (target != null)
            {
                target = null;
            }
        }
    }



    void Attract2()
    {
        if (canAttractMultipleObjects)
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
        else
        {
            if (target != null)
                target.Attract(transform, power);
        }
    }

    void UpdateAttractingList()
    {
        if (canAttractMultipleObjects)
            attractingObjects = attractingObjects.Where(x => x != null).ToList();
    }


    void GetAttractableObject(Collider collider)
    {
        //Debug.Log(collider.name, this);
        //print(collider.name);
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        Destroy(attractable.gameObject);
    }

    //public void AttractTarget()
    //{
    //    target
    //}
}
