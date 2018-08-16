using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Missile : MonoBehaviour
{
    public GameObject target;

    public float speed = 15;
    public float maxRange = 100;
    public GameObject hitEffect;
    public float rotationSpeed = 20;

    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    Vector3 prePos;
    float totalDistance;
    public GameObject attacker;

    private void Awake()
    {
        prePos = transform.position;
        //var rb = GetComponent<Rigidbody>();
        //rb.useGravity = true;
        first = StartCoroutine(FirstS());
    }
    Coroutine first;
    bool firstStage = true;

    IEnumerator FirstS()
    {
        yield return new WaitForSeconds(2);
        //TODO:後ろの火つける
        firstStage = false;
    }

    private void Update()
    {
        if (firstStage)
        {
            transform.Translate(Vector3.forward * speed / 4 * Time.deltaTime);
            transform.Translate(Vector3.down * 2 * Time.deltaTime, Space.World);
            return;
        }


        if (target != null)
        {
            //follow target
            var lookRot = Quaternion.LookRotation(target.transform.position - transform.position);
            var rot = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


        var deltaDistance = Vector3.Distance(prePos, transform.position);
        totalDistance += deltaDistance;
        if (totalDistance > maxRange)
        {
            OutOfRange();
        }

        prePos = transform.position;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == attacker)
            return;
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
