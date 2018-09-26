using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public AttractableCapther dispatcher;
    public float maxLength = 100;
    public float power = 10;

    private void Start()
    {
        dispatcher.OnCaptch += GetAttractedObject;
    }
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
                //targetRigidbody.useGravity = true;
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


    //TODO:
    void Attract()
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


    void GetAttractedObject(Collider collider)
    {
        if (Input.GetKey(KeyCode.X))
        {
            print(collider.name);
            Destroy(collider.transform.gameObject);
            targetRigidbody = null;
        }
    }
}
