using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class NotMovingTarget : MonoBehaviour, IDamagable
    {
        [SerializeField]
        private int hp = 1000;
        public int Hp
        {
            get { return hp; }
        }

        public void ApplyDamage(int damageValue, GameObject attacker)
        {
            hp -= damageValue;
            if (Hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
