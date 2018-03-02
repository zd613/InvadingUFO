using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class PlayerCore : MonoBehaviour, IDamagable
    {
        public int hp = 1000;
        public int normalSpeed = 3;

        public GameObject bullet;
        public GameObject muzzle;

        public float bulletCoolTime = 0.1f;
        private float bCoolTime = 0;

        Coroutine bulletCoroutine;

        public float pitch = 10;
        public float yaw = 10;

        PlayerInputProvider provider;
        private float speed;
        public float boostRate = 1.4f;

        private void Start()
        {
            provider = GetComponent<PlayerInputProvider>();
            provider.OnBulletAttack += () => AttackBullet();
            provider.OnPitchRotation +=
                _ => RotatePitch(_);
            provider.OnYawRotation +=
                _ => RotateYaw(_);

            provider.OnBoost +=
                (b) => speed = b ? normalSpeed * boostRate : normalSpeed;
        }

        private void Update()
        {
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

        public void AttackBullet()
        {
            if (bulletCoroutine == null)
            {
                bulletCoroutine = StartCoroutine(CreateBullet());
            }
        }

        public void RotatePitch(float value)
        {
            transform.Rotate(Vector3.right * value * pitch * Time.deltaTime);
        }

        public void RotateYaw(float value)
        {
            transform.Rotate(Vector3.up, value * yaw * Time.deltaTime, Space.World);
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
