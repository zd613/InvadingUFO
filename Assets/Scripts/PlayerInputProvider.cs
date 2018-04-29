using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Ame
{
    public class PlayerInputProvider : MonoBehaviour
    {
        //
        public bool BulletAttack { get; private set; }
        public float PitchValue { get; private set; }
        public float YawValue { get; private set; }
        public bool Boost { get; private set; }


        //
        //攻撃
        public Action OnBulletAttack;

        //移動
        public Action<bool> OnBoost;

        //回転
        public Action<float> OnPitchRotation;
        public Action<float> OnYawRotation;

        private void Update()
        {

            if (Input.GetKey(KeyCode.Z))
            {
                BulletAttack = true;
                if (OnBulletAttack != null)
                {
                    OnBulletAttack();
                }
            }
            else
            {
                BulletAttack = false;
            }

            //Pitch
            PitchValue = 0;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                PitchValue = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                PitchValue = -1;
            }
            if (OnPitchRotation != null)
            {
                OnPitchRotation(PitchValue);
            }

            //Yaw

            YawValue = 0;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                YawValue = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                YawValue = -1;
            }
            if (OnYawRotation != null)
            {
                OnYawRotation(YawValue);
            }


            //boost 

            if (Input.GetKey(KeyCode.Space))
            {
                Boost = true;
                if (OnBoost != null)
                {
                    OnBoost(true);
                }
            }
            else
            {
                Boost = false;
                if (OnBoost != null)
                {
                    OnBoost(false);
                }
            }

        }
    }
}
