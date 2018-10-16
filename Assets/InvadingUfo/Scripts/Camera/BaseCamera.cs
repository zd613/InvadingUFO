using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    public bool freezeMove;
    public bool freezeRotation;

    protected virtual void LateUpdate()
    {
        if (!freezeMove)
            Move();
        if (!freezeRotation)
            Rotate();
    }


    protected virtual void Move()
    {

    }

    protected virtual void Rotate()
    {

    }

    public void SetActiveWithDelay(bool isActive, float delaySec)
    {
        StartCoroutine(SetActiveDelay(isActive, delaySec));
    }

    IEnumerator SetActiveDelay(bool isActive,float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(isActive);
    }
}
