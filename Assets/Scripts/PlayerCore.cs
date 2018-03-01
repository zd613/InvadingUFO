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

        public GameObject bullet;

        public float bulletCoolTime = 0.1f;
        private float bCoolTime = 0;

        Coroutine bulletCoroutine;

        private void Start()
        {

        }

        private void Update()
        {
            Attack();
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

        void Attack()
        {
            if (Input.GetKey(KeyCode.Z))
            {
                if (bulletCoroutine == null)
                {
                    bulletCoroutine = StartCoroutine(CreateBullet());
                }
            }
        }

        IEnumerator CreateBullet()
        {
            Instantiate(bullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(bulletCoolTime);
            bulletCoroutine = null;
        }
    }
}
