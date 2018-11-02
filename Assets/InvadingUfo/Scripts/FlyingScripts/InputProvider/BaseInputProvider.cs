using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseInputProvider : MonoBehaviour
{

    public float YawValue { get; protected set; }
    public float PitchValue { get; protected set; }


    protected void Update()
    {
        UpdateInputStatus();
    }

    public virtual void UpdateInputStatus() { }


    /// <summary>
    /// target の方に向くようにpitch とyawを設定する
    /// </summary>
    /// <param name="target"></param>
    protected void LookAt(Vector3 target)
    {
        float pitch = 0, yaw = 0;

        //var v3 = transform.TransformDirection(currentTarget.transform.position - transform.position);
        var q = Quaternion.LookRotation(/*target.transform.position*/ target - transform.position);
        Debug.DrawRay(transform.position, /*target.transform.position*/target - transform.position, Color.yellow);
        //transform.rotation.eulerAnglesをq.eulerAnglesにするとターゲットの方を向く
        //print(transform.rotation.eulerAngles);
        //print("lookat:" + q.eulerAngles);

        float targetX = q.eulerAngles.x;
        float currentX = transform.eulerAngles.x;
        //360で180度以上のところはマイナスの角度に変換
        targetX = Util.ToSignedAngle(targetX);
        currentX = Util.ToSignedAngle(currentX);

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
        var targetDir = /*target.position*/target - transform.position;
        var forward = transform.forward;

        float angleY = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        //print(angleY);


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
}
