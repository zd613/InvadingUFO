using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraAction : MonoBehaviour
{

    public GameObject target;
    public float movingSpeed = 0.01f;

    public Vector3 firstPositionOffset;
    public Vector3 lastPositionOffset;

    public event System.Action OnFinished;

    private void Awake()
    {
        transform.position = target.transform.TransformPoint(firstPositionOffset);//target.transform.position + firstPositionOffset;
    }
    float t = 0;
    bool hasFinished = false;

    private void LateUpdate()
    {
        //var desiredPos = target.transform.position + firstPositionOffset;
        var desiredPos = /*target.transform.position +*/ target.transform.TransformPoint(Vector3.Lerp(firstPositionOffset, lastPositionOffset, t));
        //var pos = Vector3.Lerp(transform.position, desiredPos, movingSpeed * Time.deltaTime);
        transform.position = desiredPos;

        t += movingSpeed;

        if (!hasFinished && t >= 1)
        {
            hasFinished = true;
            OnFinished?.Invoke();
        }
        transform.LookAt(target.transform);
    }
}
