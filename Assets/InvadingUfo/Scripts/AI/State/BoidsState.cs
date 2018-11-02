using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//群衆シュミレーション
public class BoidsState : BaseState
{
    public BoidsManager boidsManager;

    // 0- 1
    public Transform transform;


    PlanePitchYawController pyc = new PlanePitchYawController();
    public float Turbulence = 1;

    public float pitch;
    public float yaw;

    public event System.Action UpdateAfterExecuted;
    public float MinDistance = 40;
    public float MaxDistance = 100;

    Vector3 velocity;

    public override void Execute()
    {
        pyc.transform = transform;

        velocity = Vector3.zero;
        Separation();
        Alignment();
        Cohesion();


        pyc.SetPitchYawLookingAt(transform.position + velocity);
        pitch = pyc.Pitch;
        yaw = pyc.Yaw;

        UpdateAfterExecuted?.Invoke();
    }

    void Separation()
    {

    }

    void Alignment()
    {
        foreach (var item in boidsManager.gameObjectList)
        {
            if (item.transform == transform)
                continue;

            var diff = item.transform.position - transform.position;

            if (diff.magnitude < Random.Range(MinDistance, MaxDistance))
            {
                velocity = (velocity + diff.normalized).normalized;//?
            }
        }
    }

    //中央へ移動
    void Cohesion()
    {
        var center = boidsManager.CalculateCenterPosition();

        var centerDir = (center - transform.position).normalized;
        //Debug.Log(center);


        var dir = (transform.forward * Turbulence + centerDir * (1 - Turbulence)).normalized;
        //Debug.Log(dir);

        velocity = (velocity + dir).normalized;
    }
}
