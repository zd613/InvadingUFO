using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [Header("追跡するオブジェクト")]
    public Transform target;

    public Vector3 offset;

    private void LateUpdate()
    {
        if (target == null)
            return;
        transform.position = target.position + CalculateOffset();

        transform.LookAt(target);
    }

    Vector3 CalculateOffset()
    {
        return target.forward * offset.z + target.right * offset.x
            + target.up * offset.y;
    }
}
