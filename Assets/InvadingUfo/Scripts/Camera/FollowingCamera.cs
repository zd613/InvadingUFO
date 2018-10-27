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

    public float pitchRotationSpeed = 60;
    Camera targetCamera;
    //time:0-1 の間
    public AnimationCurve yawCurve;

    private void Awake()
    {
        if (useInitialPositionForOffset)
        {
            offset = transform.position - target.transform.position;
        }
        targetCamera = GetComponent<Camera>();
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
        var pos = Vector3.Lerp(transform.position, desiredPos, movingSpeed * Time.deltaTime);
        pos.y = pos.y < 0 ? 0 : pos.y;//地面より下行かないように
        transform.position = pos;
    }


    //TODO:x と yだけ操作して　zは0で固定しておきたい
    protected override void Rotate()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);
        Debug.DrawRay(transform.position, transform.up * 2, Color.green);

        Rotate2();

    }

    void Rotate1()
    {
        Yaw();
        Pitch();
    }

    //一回転しないなら動くはず
    void Rotate2()
    {
        //////上下さかさまになってしまうのを防ぐためtransform.up
        var desiredRot = Quaternion.LookRotation(target.position - transform.position, transform.up);
        desiredRot = Quaternion.Slerp(transform.rotation, desiredRot, 0.2f);
        var ea = desiredRot.eulerAngles;
        ea.z = 0;
        desiredRot = Quaternion.Euler(ea);
        //var dr = desiredRot * Quaternion.AngleAxis(-desiredRot.eulerAngles.z, transform.forward);
        transform.rotation = desiredRot;
    }

    void Yaw()
    {
        //y
        var targetDir = target.position - transform.position;
        var forward = transform.forward;
        //上下は無視するのでyを0にする
        targetDir.y = 0;
        forward.y = 0;
        var angleY = Vector3.SignedAngle(forward, targetDir, Vector3.up);

        //print("angle Y " + angleY);
        //ほぼターゲットを見てる
        if (Mathf.Abs(angleY) < 2)
        {
            //transform.Rotate(Vector3.up, -angleY, Space.World);
        }
        //
        else
        {
            float rotationValueY = 0;
            var signY = Mathf.Sign(angleY);

            var absY = Mathf.Abs(angleY);
            const float thres = 20;
            if (absY > thres)
            {
                rotationValueY = 1;
            }
            else
            {
                rotationValueY = yawCurve.Evaluate(absY / thres);
            }
            //rotationValueYが正のとき右側へ回転
            rotationValueY *= signY;

            //print(rotationValueY);

            transform.Rotate(Vector3.up, rotationValueY * rotationSpeed * Time.deltaTime, Space.World);
        }

    }

    void Pitch()
    {
        var forward = transform.forward;
        var targetDir = target.position - transform.position;
        var localTargetDir = transform.InverseTransformDirection(targetDir);
        var angleX = Vector3.SignedAngle(Vector3.forward, localTargetDir, transform.right);


        //angleX +が下側　- が上側にターゲット
        print(angleX);
        if (Mathf.Abs(angleX) < 2)
        {
            //transform.Rotate(Vector3.up, -angleY, Space.World);
        }
        //
        else
        {
            float rotationValueX = 0;
            var signX = Mathf.Sign(angleX);

            var absX = Mathf.Abs(angleX);
            const float thres = 40;
            if (absX > thres)
            {
                rotationValueX = 1;
            }
            else
            {
                rotationValueX = yawCurve.Evaluate(absX / thres);
            }
            //rotationValueYが正のとき下側へ回転
            rotationValueX *= signX;

            //print(rotationValueY);

            transform.Rotate(Vector3.right, rotationValueX * pitchRotationSpeed * Time.deltaTime);
        }

        transform.Rotate(transform.right, angleX * Time.deltaTime);
    }

    Vector3 CalculateOffset()
    {
        return target.forward * offset.z + target.right * offset.x
            + target.up * offset.y;
    }


    //元：32 74
    float focalLength;
    public void SetFocalLength(float focalLength)
    {
        targetCamera.focalLength = focalLength;
    }

}
