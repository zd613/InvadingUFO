using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ame
{
    public class UfoCore : MonoBehaviour
    {
        public AbstractInputProvider inputProvider;

        event Action Move;
        event Action<float, float> Rotate;


        [Space]
        public UfoMove ufoMove;
        public UfoRotation ufoRotation;
        public UfoAttack ufoAttack;

        public float Hp { get; private set; }

        private void Start()
        {
            Move += ufoMove.Move;
            Rotate += (pitch, yaw) => ufoRotation.Rotate(pitch, yaw);
        }

        private void Update()
        {
            if (inputProvider.BulletAttack)
            {
                ufoAttack.Fire();
            }
        }

        private void FixedUpdate()
        {
            if (Move != null)
            {
                Move();
            }
            Rotate(inputProvider.PitchValue, inputProvider.YawValue);

            
        }


    }
}
