using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rotation))]
public class FollowPath : MonoBehaviour
{
    Movement move;
    Rotation rotation;

    public PathController targetPath;

    public Path currentTarget;
    float eps = 0.5f;



    private void Start()
    {
        move = GetComponent<Movement>();
        rotation = GetComponent<Rotation>();

        if (targetPath.HasPath)
        {
            currentTarget = targetPath.paths[0];

        }
    }

    public float thres = 0.3f;
    public float value = 0.3f;

    private void Update()
    {
        if (!targetPath.HasPath)
            return;
        //飛行機のローカル座標で回転する向き決定

        float pitch = 0, yaw = 0;
        var v3 = transform.TransformDirection(currentTarget.transform.position - transform.position);
        //ローカルのyでピッチ決定
        if (v3.y > 0)
        {
            if (v3.y > thres)
            {
                pitch = -1;
            }
            else
            {
                //pitch = -value;
                pitch = -Mathf.Sin(v3.y * Mathf.PI / 2);
            }
        }
        else if (v3.y < 0)
        {
            if (v3.y > thres)
            {
                pitch = 1;
            }
            else
            {
                pitch = Mathf.Sin(Mathf.Abs(v3.y) * Mathf.PI / 2);
                //pitch = value;
            }
        }


        if (v3.x > 0)
        {
            if (v3.x > thres)
            {
                yaw = 1;
            }
            else
            {
                yaw = Mathf.Sin(v3.x * Mathf.PI / 2);
            }

        }
        else if (v3.x < 0)
        {
            if (v3.x > thres)
            {
                yaw = -1;
            }
            else
            {
                yaw = -Mathf.Sin(Mathf.Abs(v3.x) * Mathf.PI / 2);
            }
        }
        print(v3 + "/" + pitch + "," + yaw);
        if (Vector3.Distance(transform.position, currentTarget.transform.position) < eps)
        {
            currentTarget = currentTarget.next;
        }

        rotation.Rotate(pitch, yaw);
    }
}
