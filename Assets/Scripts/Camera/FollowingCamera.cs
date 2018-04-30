using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    //https://answers.unity.com/questions/1329163/how-to-use-quaternionslerp-with-transformlookat.html


    [Header("追跡するオブジェクト")]
    public Transform target;

    public Vector3 offset;
    public float rotationSpeed;//2.7f

    private void LateUpdate()
    {
        if (target == null)
            return;
        transform.position = target.position + CalculateOffset();

        //ターゲットとカメラの向き
        var lookRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime);
    }

    Vector3 CalculateOffset()
    {
        return target.forward * offset.z + target.right * offset.x
            + target.up * offset.y;
    }
}
