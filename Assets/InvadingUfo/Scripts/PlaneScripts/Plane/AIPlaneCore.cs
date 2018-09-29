using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlaneCore : CommonCore
{
    public Transform target;
    //public OnTriggerEnterSender bullet;


    protected override void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);

        if (target == null)
        {
            return;
        }

        var distance = Vector3.Distance(transform.position, target.position);
        if (distance < attack.gunRange)
        {
            attack?.Fire();
        }

        if (rotation != null)
        {
            //rotation.Rotate(inputProvider.PitchValue, inputProvider.YawValue);
            var q = Quaternion.LookRotation(target.position - transform.position);
            var ea = q.eulerAngles;
            //print(ea + "," + transform.eulerAngles);


            //pitch yを見て判断
            var destY = ea.y;
            var currentY = transform.eulerAngles.y;
            var diffY = destY - currentY;
            //print(diffY);

            float yaw = 0;
            if (diffY > 0)
            {
                yaw = 1;
            }
            else
            {
                yaw = -1;
            }

            rotation.Rotate(0, yaw);
        }

        if (movement != null)
        {
            movement.Move();
        }
    }

}
