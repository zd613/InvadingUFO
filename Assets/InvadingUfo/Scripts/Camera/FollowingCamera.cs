using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : BaseCamera
{
    //https://answers.unity.com/questions/1329163/how-to-use-quaternionslerp-with-transformlookat.html


    [Header("追跡するオブジェクト")]
    public Transform target;

    public bool useInitialPositionForOffset = true;
    [Tooltip("useInitialPositionForOffsetがfalseの時に使うoffset")]
    public Vector3 offset;
    public float movingSpeed = 2.7f;//適当な値
    public float rotationSpeed = 2.7f;//2.7f適当な値

    private void Awake()
    {
        if (useInitialPositionForOffset)
        {
            offset = transform.position - target.transform.position;
        }


        var desiredRot = Quaternion.LookRotation(target.position - transform.position, transform.up);
        var ea = desiredRot.eulerAngles;
        var desiredRot2 = Quaternion.Euler(ea);
        var q = desiredRot * Quaternion.Euler(0, 0, 10);
        var ea2 = desiredRot2.eulerAngles;
        print(desiredRot);
        print(desiredRot2);

        print(ea);
        print(ea2);
    }

    protected override void LateUpdate()
    {
        if (target == null)
            return;

        base.LateUpdate();

    }


    protected override void Move()
    {
        // var desiredPos = target.position + CalculateOffset();
        var desiredPos = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, movingSpeed * Time.deltaTime);
        //transform.position = desiredPos;
    }

    protected override void Rotate()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);
        Debug.DrawRay(transform.position, transform.up * 2, Color.green);
        //////上下さかさまになってしまうのを防ぐためtransform.up
        var desiredRot = Quaternion.LookRotation(target.position - transform.position,transform.up);


        //print(desiredRot.eulerAngles);
        //var q = Quaternion.AngleAxis(-desiredRot.eulerAngles.z, transform.forward);
        //desiredRot = desiredRot * q;
        var dr = desiredRot * Quaternion.AngleAxis(-desiredRot.eulerAngles.z, transform.forward);
        print(dr.eulerAngles);
        ////TODO:zは回転させない ここおかしい
        ////反転するようになる

        transform.rotation = desiredRot;
        //transform.Rotate(new Vector3(0, 0, -transform.localEulerAngles.z));
    }

    Vector3 CalculateOffset()
    {
        return target.forward * offset.z + target.right * offset.x
            + target.up * offset.y;
    }
}
