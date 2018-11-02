using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractableObject : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Attract(Transform magnet, float power)
    {

        var dir = (magnet.position - transform.position).normalized;
        Debug.DrawRay(transform.position, dir * 20, Color.red);

        if (rb != null)
        {
            rb.AddForce(dir * power);
        }
        else
        {
            transform.Translate(dir * power * Time.deltaTime);
        }

    }
}
