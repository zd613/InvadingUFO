using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : BaseCamera
{
    //https://answers.unity.com/questions/1329163/how-to-use-quaternionslerp-with-transformlookat.html


    [Header("追跡するオブジェクト")]
    public Transform target;

    public Vector3 offset;
    public float movingSpeed;//
    public float rotationSpeed;//2.7f


    protected override void LateUpdate()
    {
        if (target == null)
            return;

        base.LateUpdate();
  
    }

    protected override void Move()
    {
        var desiredPos = target.position + CalculateOffset();
        transform.position = Vector3.Slerp(transform.position, desiredPos, Time.deltaTime * movingSpeed);

    }

    protected override void Rotate()
    {
        var lookRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
    }

    Vector3 CalculateOffset()
    {
        return target.forward * offset.z + target.right * offset.x
            + target.up * offset.y;
    }
}
