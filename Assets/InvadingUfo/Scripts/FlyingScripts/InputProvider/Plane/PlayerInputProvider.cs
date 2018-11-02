using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Ame
{
    public class PlayerInputProvider : BasePlaneInputProvider
    {
        public override void UpdateInputStatus()
        {
            ////bullet
            //if (Input.GetKey(KeyCode.Z))
            //{
            //    BulletAttack = true;
            //}
            //else
            //{
            //    BulletAttack = false;
            //}

            ////missile
            //if (Input.GetKey(KeyCode.X))
            //{
            //    MissileAttack = true;
            //}
            //else
            //{
            //    MissileAttack = false;
            //}

            var missile = Input.GetAxis("Special1");
            MissileAttack = missile > 0;

            var bullet = Input.GetAxis("Special2");
            BulletAttack = bullet > 0;

            //Pitch
            //TODO:-をとりのぞきたい　mouse input provider も多分同じなはず
            PitchValue = -Input.GetAxis("Vertical");
            //Yaw
            YawValue = Input.GetAxis("Horizontal");

            //boost 

            if (Input.GetKey(KeyCode.Space))
            {
                Boost = true;
                //OnBoost?.Invoke(true);
            }
            else
            {
                Boost = false;
                //OnBoost?.Invoke(false);
            }
        }
    }
}
