using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rigidbodyでやると順序がおかしい？ので
//transformで値を設定している
public class AARotation : Rotation
{
    [Header("制限")]
    public AngleRange pitchLimit = new AngleRange(-60, 10);
    public AngleRange yawLimit = new AngleRange(-180, 180);

    protected override void Pitch(float value)
    {
        if (value == 0)
            return;
        //LimitRotation();
        //rb.Rotate(Vector3.right * value * power.Pitch * Time.deltaTime);
        transform.Rotate(Vector3.right * value * power.Pitch * Time.deltaTime);

        //limit pitch
        var rot = transform.eulerAngles;
        var la = Mathf.Clamp(GetSignedEulerAngle(rot.x), pitchLimit.Min, pitchLimit.Max);
        rot.x = la;
        //print(transform.eulerAngles);
        transform.eulerAngles = rot;
    }

    protected override void Yaw(float value)
    {
        if (value == 0)
            return;

        transform.Rotate(Vector3.up * value * power.Yaw * Time.deltaTime, Space.World);

        //limit yaw
        var rot = transform.eulerAngles;
        var la = Mathf.Clamp(GetSignedEulerAngle(rot.y), yawLimit.Min, yawLimit.Max);
        rot.y = la;
        transform.eulerAngles = rot;
    }

    float GetSignedEulerAngle(float unsignedEulerAngle)
    {
        return unsignedEulerAngle > 180 ? unsignedEulerAngle - 360 : unsignedEulerAngle;
    }

    float ToUnsignedEulerAngle(float angle)
    {
        return angle < 0 ? angle + 360 : angle;
    }

}
