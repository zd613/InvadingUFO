using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePitchYawController
{
    public Transform transform;



    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    public PlanePitchYawController() { }
    public PlanePitchYawController(Transform transform)
    {
        this.transform = transform;
    }

    //ターゲット方向へ向くようにpitch yawをセットする
    public void SetPitchYawLookingAt(Vector3 targetPosition)
    {

        float pitch = 0;
        float yaw = 0;

        var q = Quaternion.LookRotation(/*target.transform.position*/ targetPosition - transform.position);
        Debug.DrawRay(transform.position, /*target.transform.position*/targetPosition - transform.position, Color.yellow);


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
        var targetDir = /*target.position*/targetPosition - transform.position;
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

        Pitch = pitch;

        //TODO:マイナスなのでプラスになるように調整する
        Yaw = -yaw;
    }
}
