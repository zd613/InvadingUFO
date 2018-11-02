using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool isActive = true;

    [Header("パラメータ")]
    public float speed;
    protected Rigidbody rb;

    public float Speed { get; protected set; }

    [Space]
    public bool altitudeConstraint = true;
    public float minAltitude = 0;
    public float maxAltitude = 50;

    [Space]
    public float boostRate = 1.5f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Move(bool boost = false)
    {
        if (!isActive)
        {
            return;
        }

        Vector3 forward = transform.forward;

        //上下移動制限
        if (altitudeConstraint)
        {
            var rotX = transform.eulerAngles.x;
            if (transform.position.y < minAltitude)
            {
                if (rotX < 180)//下向き
                {
                    forward.y = 0;
                }
            }
            else if (transform.position.y > maxAltitude)
            {
                if (rotX > 180)
                {
                    forward.y = 0;
                }
            }
        }

        if (boost)
        {
            Speed = speed * boostRate;
        }
        else
        {
            Speed = speed;
        }

        rb.MovePosition(transform.position + forward * Speed * Time.deltaTime);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }
}
