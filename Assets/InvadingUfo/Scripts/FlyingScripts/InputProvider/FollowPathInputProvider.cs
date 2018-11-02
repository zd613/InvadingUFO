using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rotation))]
public class FollowPathInputProvider : BaseInputProvider
{
    Movement move;
    Rotation rotation;

    public PathController targetPath;
    public float changeTargetDistance = 1f;

    public Path currentTarget;

    float previousDeltaDistance;
    Vector3 pre;



    private void Start()
    {
        move = GetComponent<Movement>();
        rotation = GetComponent<Rotation>();

        if (targetPath.HasPath)
        {
            currentTarget = targetPath.paths[0];

        }
        previousDeltaDistance = GetDeltaDistance();
    }




    public override void UpdateInputStatus()
    {
        if (!targetPath.HasPath)
            return;

        if (currentTarget == null)
            return;

        //飛行機のローカル座標で回転する向き決定
        //var delta = GetDeltaDistance();
        //var diff = Mathf.Abs(delta - previousDeltaDistance);
        //var deltaV3 = (transform.position - pre);
        //var dd = deltaV3.z / Time.deltaTime;
        //print("diff:" + dd);


        //TODO:ターゲットが頻繁に更新されてる
        var d = GetDeltaDistance();
        if (d < changeTargetDistance)
        {
            if (currentTarget.next == null)
                return;
            currentTarget = currentTarget.next;
            d = GetDeltaDistance();

            previousDeltaDistance = d;
            //print("change target : go through");
            return;
        }

        //else if (d > previousDeltaDistance)
        //{
        //    print("離れていってる");
        //    if (currentTarget.next == null)
        //        return;
        //    currentTarget = currentTarget.next;
        //    d = GetDeltaDistance();
        //    previousDeltaDistance = d;
        //    return;
        //}



        LookAt(currentTarget.transform.transform.position);

        previousDeltaDistance = d;
    }

    float GetDeltaDistance()
        => Vector3.Distance(transform.position, currentTarget.transform.position);
}
