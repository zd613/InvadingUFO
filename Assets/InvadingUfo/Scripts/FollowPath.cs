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

    public float thres = 0.3f;
    public float value = 0.3f;

    float previousDeltaDistance;

    public float pitch = 1;
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
        return;
    }


    float GetDeltaDistance()
        => Vector3.Distance(transform.position, currentTarget.transform.position);

    void UpdateTarget()
    {

    }

    //
    void SetPitchAndYaw(Transform target)
    {
        float pitch = 0, yaw = 0;

        //var v3 = transform.TransformDirection(currentTarget.transform.position - transform.position);
        var q = Quaternion.LookRotation(target.transform.position - transform.position);
        //transform.rotation.eulerAnglesをq.eulerAnglesにするとターゲットの方を向く
        //print(transform.rotation.eulerAngles);
        //print("lookat:" + q.eulerAngles);

        float targetX = q.eulerAngles.x;
        float currentX = transform.eulerAngles.x;
        //360で180度以上のところはマイナスの角度に変換
        targetX = ToSignedAngle(targetX);
        currentX = ToSignedAngle(currentX);

        float targetY = q.eulerAngles.y;
        float currentY = transform.eulerAngles.y;
        targetY = ToSignedAngle(targetY);
        currentY = ToSignedAngle(currentY);

        // print(targetX + ":" + currentX);

        //Pitch

        if (Mathf.Abs(targetX - currentX) < 0.08f)//ターゲットの方向向いているとき
        {
        }
        else if (currentX - targetX > 1)//進行方向がforwardの時上側にパスのターゲットがある　
        {
            pitch = -1;
        }
        else if (currentX - targetX > 0)
        {
            pitch = -0.2f;
        }
        else if (currentX - targetX < 0)//下側
        {
            pitch = 0.2f;
        }
        else if (currentX - targetX < -1f)
        {
            pitch = 1;
        }

        //Yaw
        if (Mathf.Abs(targetY - currentY) < 0.08f)//ターゲットの方向向いているとき
        {
        }
        else if (currentX - targetX > 1)//進行方向がforwardの時上側にパスのターゲットがある　
        {
            pitch = -1;
        }
        else if (currentY - targetY > 0)//
        {
            yaw = -0.2f;
        }
        else if (currentY - targetY < 0)//
        {
            yaw = 0.2f;
        }
        else if (currentY - targetY < -1f)
        {
            yaw = 1f;
        }

        YawValue = yaw;
        PitchValue = pitch;
        //rotation.Rotate(pitch, yaw);
    }



    float ToSignedAngle(float angle)
        => angle > 180 ? angle - 360 : angle;
}
