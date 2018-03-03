using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ame
{
    public class UfoCore : MonoBehaviour, IDamagable
    {
        [SerializeField]
        float speed = 1;
        [SerializeField]
        Vector3 start;
        [SerializeField]
        Vector3 goal;

        public event Action Move;
        public event Action Rotate;

        UfoInputProvider provider;

        public float Hp { get; private set; }

        private void Awake()
        {
            transform.position = start;
            transform.rotation = Quaternion.LookRotation(goal - start);
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


    }
}
