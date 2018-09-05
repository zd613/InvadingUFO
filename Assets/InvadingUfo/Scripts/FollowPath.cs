using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rotation))]
public class FollowPath : AbstractInputProvider
{
    Movement move;
    Rotation rotation;

    public PathController targetPath;

    public Path currentTarget;
    float eps = 0.1f;



    private void Start()
    {
        move = GetComponent<Movement>();
        rotation = GetComponent<Rotation>();

        if (targetPath.HasPath)
        {
            currentTarget = targetPath.paths[0];

        }
        previousDeltaDistance = GetDeltaDistance();
    }


    float previousDeltaDistance;

    Vector3 pre;


    private void Update()
    {
        if (!targetPath.HasPath)
            return;

        if (currentTarget == null)
            return;

        //飛行機のローカル座標で回転する向き決定
        //var delta = GetDeltaDistance();
        //var diff = Mathf.Abs(delta - previousDeltaDistance);
        //var deltaV3 = (transform.position - pre);
        //var dd = deltaV3.z / Time.deltaTime;
        //print("diff:" + dd);


        //TODO:ターゲットが頻繁に更新されてる
        var d = GetDeltaDistance();
        if (d < eps)
        {
            if (currentTarget.next == null)
                return;
            currentTarget = currentTarget.next;
            d = GetDeltaDistance();

            previousDeltaDistance = d;
            print("change target : go through");
            return;
        }

        //else if (d > previousDeltaDistance)
        //{
        //    print("離れていってる");
        //    if (currentTarget.next == null)
        //        return;
        //    currentTarget = currentTarget.next;
        //    d = GetDeltaDistance();
        //    previousDeltaDistance = d;
        //    return;
        //}



        SetPitchAndYaw(currentTarget.transform);

        previousDeltaDistance = d;
    }


    float GetDeltaDistance()
        => Vector3.Distance(transform.position, currentTarget.transform.position);


    void SetPitchAndYaw(Transform target)
    {
        float pitch = 0, yaw = 0;

        //var v3 = transform.TransformDirection(currentTarget.transform.position - transform.position);
        var q = Quaternion.LookRotation(target.transform.position - transform.position);
        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.yellow);
        //transform.rotation.eulerAnglesをq.eulerAnglesにするとターゲットの方を向く
        //print(transform.rotation.eulerAngles);
        //print("lookat:" + q.eulerAngles);

        float targetX = q.eulerAngles.x;
        float currentX = transform.eulerAngles.x;
        //360で180度以上のところはマイナスの角度に変換
        targetX = ToSignedAngle(targetX);
        currentX = ToSignedAngle(currentX);

        //Pitch

        var deltaRotX = currentX - targetX;

        if (Mathf.Abs(currentX - targetX) < 0.08f)//ターゲットの方向向いているとき
        {
        }
        else if (currentX - targetX > 1)//進行方向がforwardの時上側にパスのターゲットがある　
        {
            pitch = -1;
        }
        else if (currentX - targetX > 0)//上側　回転少なく
        {
            pitch = -0.2f;
        }
        else if (currentX - targetX < -1)//下側　回転最大
        {
            pitch = 1;
        }
        else if (currentX - targetX < 0)//下側　回転少なく
        {
            pitch = 0.2f;
        }

        //Yaw
        var targetDir = target.position - transform.position;
        var forward = transform.forward;

        float angleY = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        print(angleY);


        //右側に存在
        if (angleY > 0.001f)
        {
            if (angleY < 1)
            {
                yaw = 0.2f;
            }
            else
            {
                yaw = 1;
            }
        }
        //左に存在
        else if (angleY < -0.001f)
        {
            if (angleY > -1)
            {
                yaw = -0.2f;
            }
            else
            {
                yaw = -1;
            }
        }

        YawValue = yaw;
        PitchValue = pitch;
    }



    float ToSignedAngle(float angle)
        => angle > 180 ? angle - 360 : angle;

}
