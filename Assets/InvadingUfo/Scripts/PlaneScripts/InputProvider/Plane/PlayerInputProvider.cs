using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Ame
{
    public class PlayerInputProvider : BasePlaneInputProvider
    {
        //いらない？
        //
        //攻撃
        //public event Action OnBulletAttack;
        //public event Action OnMissileAttack;

        ////移動
        //public event Action<bool> OnBoost;

        ////回転
        //public event Action<float> OnPitchRotation;
        //public event Action<float> OnYawRotation;

        //

        public override void UpdateInputStatus()
        {
            //bullet
            if (Input.GetKey(KeyCode.Z))
            {
                BulletAttack = true;
                //OnBulletAttack?.Invoke();
            }
            else
            {
                BulletAttack = false;
            }

            //missile
            if (Input.GetKey(KeyCode.X))
            {
                MissileAttack = true;
            }
            else
            {
                MissileAttack = false;
            }


            //Pitch
            //PitchValue = 0;
            PitchValue = Input.GetAxis("Vertical");

            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    PitchValue = 1;
            //}
            //else if (Input.GetKey(KeyCode.DownArrow))
            //{
            //    PitchValue = -1;
            //}
            //OnPitchRotation?.Invoke(PitchValue);


            //Yaw

            YawValue = Input.GetAxis("Horizontal");

            //YawValue = 0;
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    YawValue = 1;
            //}
            //else if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    YawValue = -1;
            //}
            //OnYawRotation?.Invoke(YawValue);


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
