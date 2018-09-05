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


        //var desiredRot = Quaternion.LookRotation(target.position - transform.position, transform.up);
        //var ea = desiredRot.eulerAngles;
        //var desiredRot2 = Quaternion.Euler(ea);
        //var q = desiredRot * Quaternion.Euler(0, 0, 10);
        //var ea2 = desiredRot2.eulerAngles;
        //print(desiredRot);
        //print(desiredRot2);

        //print(ea);
        //print(ea2);
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


    //TODO:x と yだけ操作して　zは0で固定しておきたい
    protected override void Rotate()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);
        Debug.DrawRay(transform.position, transform.up * 2, Color.green);


        //y
        var targetDir = target.position - transform.position;
        var forward = transform.forward;
        var angleY = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        print(angleY);

        //ほぼターゲットを見てる
        if (Mathf.Abs(angleY) < 2)
        {
            //transform.Rotate(Vector3.up, -angleY, Space.World);
        }
        //
        else
        {
            float rotationValueY = 0;
            if (angleY > 5)//カメラの右側にターゲット
            {
                rotationValueY = 1;
            }
            else if (angleY > 2)
            {
                rotationValueY = 0.01f;
            }
            else if (angleY < -5)
            {
                rotationValueY = -1;
            }
            else if (angleY < -2)
            {
                rotationValueY = -0.01f;
            }
            print(rotationValueY);

            transform.Rotate(Vector3.up, rotationValueY * rotationSpeed * Time.deltaTime, Space.World);
        }

        //var angleX = Vector3.SignedAngle(forward, targetDir, transform.right);


        //transform.Rotate(transform.right, angleX * Time.deltaTime);






        return;

        //////上下さかさまになってしまうのを防ぐためtransform.up
        var desiredRot = Quaternion.LookRotation(target.position - transform.position, transform.up);
        var dr = desiredRot * Quaternion.AngleAxis(-desiredRot.eulerAngles.z, transform.forward);
        transform.rotation = desiredRot;



    }

    Vector3 CalculateOffset()
    {
        return target.forward * offset.z + target.right * offset.x
            + target.up * offset.y;
    }
}
