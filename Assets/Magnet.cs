using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float maxLength = 100;
    public float power = 10;

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * maxLength);

        if (Input.GetKey(KeyCode.X))
        {
            Attract();
        }
        else
        {
            if (targetRigidbody != null)
            {
                targetRigidbody.useGravity = true;
                targetRigidbody = null;
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            var move = GetComponent<Movement>();
            move.isActive = !move.isActive;
        }
    }

    GameObject target;
    Rigidbody targetRigidbody;


    //TODO
    void Attract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance: maxLength))
        {
            if (!targetRigidbody)//null
            {
                targetRigidbody = hit.transform.GetComponent<Rigidbody>();
                if (targetRigidbody == null)
                    return;
                targetRigidbody.useGravity = false;
            }
            else if (target != hit.transform.gameObject)//change target
            {
                targetRigidbody.useGravity = true;


                targetRigidbody = hit.transform.GetComponent<Rigidbody>();
                targetRigidbody.useGravity = false;
            }

            targetRigidbody.AddForce(Vector3.up * power);


            //get target
            if (Vector3.Distance(transform.position, hit.transform.position) < 20)
            {
                Destroy(hit.transform.gameObject);
                targetRigidbody = null;
            }
        }

    }



}
