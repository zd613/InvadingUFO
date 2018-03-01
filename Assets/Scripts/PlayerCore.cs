using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class PlayerCore : MonoBehaviour, IDamagable
    {
        public int hp = 1000;
        public int speed = 3;

        private void Start()
        {

        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void ApplyDamage(int damageValue)
        {
            hp -= damageValue;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
