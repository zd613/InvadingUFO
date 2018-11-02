using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class NotMovingTarget : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private float hp = 1000;
        public float Hp
        {
            get { return hp; }
        }

        public void ApplyDamage(float damageValue, GameObject attacker)
        {
            hp -= damageValue;
            if (Hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
