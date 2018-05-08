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
        public Movement move;
        public Rotation rotation;
        public Attack attack;

        private void Start()
        {
            Move += move.Move;
            Rotate += (pitch, yaw) => rotation.Rotate(pitch, yaw);
        }

        private void Update()
        {
            if (inputProvider.BulletAttack)
            {
                attack.Fire();
            }
        }

        private void FixedUpdate()
        {
            Rotate(inputProvider.PitchValue, inputProvider.YawValue);
            if (Move != null)
            {
                Move();
            }      
        }


    }
}
