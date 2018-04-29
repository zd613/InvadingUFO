using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ame
{
    public class UfoCore : MonoBehaviour, IDamagable
    {
        public AbstractInputProvider inputProvider;

        event Action Move;
        event Action<float, float> Rotate;


        [Space]
        public UfoMove ufoMove;
        public UfoRotation ufoRotation;

        public float Hp { get; private set; }

        private void Awake()
        {
            //Hp = 300;
            //Move = () => { };
            //Rotate = () => { };

            //if (inputProvider != null)
            //{
            //    inputProvider.OnPitchRotation +=
            //        (rotaionValue) =>
            //        {

            //        };
            //}
        }

        private void Start()
        {
            Move += ufoMove.Move;
            Rotate += (pitch, yaw) => ufoRotation.Rotate(pitch, yaw);
        }

        private void FixedUpdate()
        {
            if (Move != null)
            {
                Move();
            }
            Rotate(inputProvider.PitchValue, inputProvider.YawValue);
        }

        public void ApplyDamage(int damageValue, GameObject attacker)
        {
            Hp -= damageValue;
            if (Hp <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}
