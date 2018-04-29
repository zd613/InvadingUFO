using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ame
{
    public class UfoCore : MonoBehaviour, IDamagable
    {

        public event Action Move;
        public event Action Rotate;

        UfoInputProvider provider;
        
        public float Hp { get; private set; }

        private void Awake()
        {
             Hp = 300;
            Move = () => { };
            Rotate = () => { };
        }

        private void Start()
        {
            provider = GetComponent<UfoInputProvider>();
            
        }

        private void Update()
        {
            Move();
            Rotate();
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
