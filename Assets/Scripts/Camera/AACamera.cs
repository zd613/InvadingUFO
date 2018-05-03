using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AACamera : MonoBehaviour
{
    [Header("targer")]
    public Transform antiAircraftGun;
    [Header("crosshair")]
    public Transform aaTarget;

    Vector3 offset;
    public float movingSpeed = 2.7f;//
    public float rotationSpeed = 2.7f;//2.7f

    private void Awake()
    {
        offset = transform.position - antiAircraftGun.transform.position;
    }

    private void LateUpdate()
    {
        if (antiAircraftGun == null)
            return;

        var desiredPos = antiAircraftGun.position + CalculateOffset();
        transform.position = Vector3.Slerp(transform.position, desiredPos, Time.deltaTime * movingSpeed);

        //ターゲットとカメラの向き
        var lookRot = Quaternion.LookRotation(aaTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
    }

    Vector3 CalculateOffset()
    {
        return antiAircraftGun.forward * offset.z + antiAircraftGun.right * offset.x
            + antiAircraftGun.up * offset.y;
    }
}
