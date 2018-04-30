using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    //https://answers.unity.com/questions/1329163/how-to-use-quaternionslerp-with-transformlookat.html


    [Header("追跡するオブジェクト")]
    public Transform target;

    public Vector3 offset;
    public float movingSpeed;//
    public float rotationSpeed;//2.7f


    private void LateUpdate()
    {
        if (target == null)
            return;

        var desiredPos = target.position + CalculateOffset();
        transform.position = Vector3.Slerp(transform.position, desiredPos, Time.deltaTime * movingSpeed);
        //transform.position = target.transform.position + CalculateOffset();
        //var rot = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotationSpeed * Time.deltaTime);

        //transform.rotation = rot;
        //ターゲットとカメラの向き
        var lookRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
    }

    Vector3 CalculateOffset()
    {
        return target.forward * offset.z + target.right * offset.x
            + target.up * offset.y;
    }
}
