using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class PlayerCore : MonoBehaviour, IDamagable, IRestrictableRotation
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

        bool lockRotaion = false;
        int lockRotationValue = 1;



        private void Start()
        {
            provider = GetComponent<PlayerInputProvider>();
            provider.OnBulletAttack += () => AttackBullet();
            provider.OnPitchRotation +=
                _ => RotatePitch(_);
            provider.OnYawRotation +=
                _ => RotateYaw(_);

            provider.OnBoost +=
                (b) => SetSpeed(b);
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




        void SetSpeed(bool boost)
        {
            speed = boost ? normalSpeed * boostRate : normalSpeed;
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
            transform.Rotate(Vector3.right * value * lockRotationValue * pitch * Time.deltaTime);
        }

        public void RotateYaw(float value)
        {
            transform.Rotate(Vector3.up, value * yaw * lockRotationValue * Time.deltaTime, Space.World);
        }

        IEnumerator CreateBullet()
        {
            var obj = Instantiate(bullet, muzzle.transform.position, transform.rotation);
            obj.GetComponent<Bullet>().SetAttacker(gameObject);
            yield return new WaitForSeconds(bulletCoolTime);
            bulletCoroutine = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ApplyDamage(hp, collision.transform.root.gameObject);
        }

        public void RestrictRotation()
        {
            lockRotationValue = 0;
            lockRotaion = true;
        }

        public void FreeRotation()
        {
            lockRotationValue = 1;
            lockRotaion = false;
        }
    }
}
