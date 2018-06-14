using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Missile : MonoBehaviour
{
    public float speed = 15;
    public float maxRange = 100;
    public GameObject target;
    public GameObject hitEffect;

    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    Vector3 startPos;

    private void Awake()
    {

        startPos = transform.position;

    }

    private void Update()
    {
        if (target != null)
        {
            //follow target
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, startPos) > maxRange)
        {
            OutOfRange();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            var obj = Instantiate(hitEffect, contact.point, transform.rotation);
            //var ls = obj.transform.localScale;
            //ls *= 2;
            //obj.transform.localScale = ls;
        }
        print("missile hit:" + collision.gameObject.name);
        Destroy(gameObject);
    }

    void OutOfRange()
    {
        Destroy(gameObject);
    }
}
