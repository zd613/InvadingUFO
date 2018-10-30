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
    public Collider attractAreaCollider;

    [Header("Animation")]
    public bool useAnimation;
    public float animationSpeed = 10;
    public GameObject beamEffect;


    bool isPlayingAnimation = false;

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
        PrivateAttract();
    }

    public void StartToAttract()
    {
        isAttracting = true;
        beamEffect.SetActive(true);

        if (useAnimation)
        {
            StartCoroutine(PlayMagnetAnimation());
        }
        else
        {
            //attractAreaCollider.enabled = true;
        }
    }

    public void StopAttracting()
    {
        isAttracting = false;
        //attractAreaCollider.enabled = false;
        beamEffect.SetActive(false);

    }

    //add attractable objects to the list
    void EnterCollider(Collider collider)
    {
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        //if (canAttractMultipleObjects)
        //{
        if (attractingObjects.Contains(attractable))
            return;

        attractingObjects.Add(attractable);
        //}
        //else
        //{
        if (target == null)
        {
            target = attractable;
        }
        //}
    }

    //remove attractable objects if exists
    void GetOutOfCollider(Collider collider)
    {
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        //if (canAttractMultipleObjects)
        //{
        if (!attractingObjects.Contains(attractable))
            return;

        attractingObjects.Remove(attractable);
        //}
        //else
        //{
        //    if (target != null)
        //    {
        //        target = null;
        //    }
        //}
    }



    void PrivateAttract()
    {

        if (!isAttracting)
            return;

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


    IEnumerator PlayMagnetAnimation()
    {

        isPlayingAnimation = true;
        var effect = attractAreaCollider.transform;
        var targetScale = effect.localScale;

        Vector3 scale = Vector3.zero;
        scale.y = targetScale.y;
        while (true)
        {
            effect.localScale = scale;
            scale += targetScale * animationSpeed * Time.deltaTime;
            scale.y = targetScale.y;

            if (scale.x > targetScale.x)
            {
                effect.localScale = targetScale;
                break;
            }
            yield return null;
        }
        isPlayingAnimation = false;
        //attractAreaCollider.enabled = true;
    }

    void UpdateAttractingList()
    {
        //if (canAttractMultipleObjects)
        attractingObjects = attractingObjects.Where(x => x != null).ToList();

        if (!canAttractMultipleObjects)
        {
            float min = float.MaxValue;
            foreach (var item in attractingObjects)
            {
                var d = Vector3.Distance(item.transform.position, transform.position);
                if (d < min)
                {
                    target = item;
                    min = d;
                }
            }

            if (min == float.MaxValue)
                target = null;
        }

    }


    void GetAttractableObject(Collider collider)
    {
        //Debug.Log(collider.name, this);
        //print(collider.name);
        var attractable = collider.GetComponentInParent<AttractableObject>();
        if (attractable == null)
            return;

        var house = attractable.GetComponent<House>();
        if (house != null)
        {
            house.TakeFinancialDamage();
        }
        
        Destroy(attractable.gameObject);
    }
}
