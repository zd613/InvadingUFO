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
        public GameObject muzzle;

        public float bulletCoolTime = 0.1f;
        private float bCoolTime = 0;

        Coroutine bulletCoroutine;

        public float pitch = 10;
        public float yaw = 10;


        private void Update()
        {
            Attack();
            Rotate();
            Move();
        }

        public void ApplyDamage(int damageValue, GameObject attacker)
        {
            hp -= damageValue;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }

        void Move()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        void Rotate()
        {
            //pitch
            float p = 0;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                p = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                p = -1;
            }
            transform.Rotate(Vector3.right * p * pitch * Time.deltaTime);

            //yaw
            float y = 0;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                y = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                y = -1;
            }
            transform.Rotate(Vector3.up, y * yaw * Time.deltaTime, Space.World);


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
            var obj = Instantiate(bullet, muzzle.transform.position, transform.rotation);
            obj.GetComponent<Bullet>().SetAttacker(gameObject);
            yield return new WaitForSeconds(bulletCoolTime);
            bulletCoroutine = null;
        }
    }
}
