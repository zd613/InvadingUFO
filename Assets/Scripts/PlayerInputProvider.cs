using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Ame
{
    public class PlayerInputProvider : MonoBehaviour
    {
        public Action OnBulletAttack;

        
        public Action<bool> OnBoost;
        public Action<float> OnPitchRotation;
        public Action<float> OnYawRotation;


        private void Update()
        {
            if (Input.GetKey(KeyCode.Z))
            {
                if (OnBulletAttack != null)
                    OnBulletAttack();
            }
            if (OnPitchRotation != null)
            {
                float p = 0;
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    p = 1;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    p = -1;
                }

                OnPitchRotation(p);
            }

            if (OnYawRotation != null)
            {
                float y = 0;
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    y = 1;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    y = -1;
                }
                OnYawRotation(y);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (OnBoost != null)
                {
                    OnBoost(true);
                }
            }
            else
            {
                OnBoost(false);
            }
        }
    }
}
