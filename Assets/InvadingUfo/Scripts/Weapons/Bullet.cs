using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class Bullet : MonoBehaviour
    {
        public BulletPool pool;
        public float speed = 10;
        public float damage = 10;
        public float range = 20;

        private Vector3 start;
        public GameObject Attacker { get; set; }

        public GameObject hitEffectPrefab;

        //２回目防止用
        bool hit;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            start = transform.position;
            hit = false;
        }

        private void Update()
        {

            if (Vector3.Distance(start, transform.position) > range)
            {
                DestroyOrReturnBullet();
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hit)
                return;

            //attackerと衝突判定なし
            if (Attacker != null)
            {
                var commonCore = other.GetComponentInParent<BaseCore>();
                if (commonCore != null)
                {
                    if (commonCore.gameObject == Attacker)
                    {
                        return;
                    }
                }
            }
            var layer = other.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Stage") || layer == LayerMask.NameToLayer("Attractable"))
            {

                CreateHitEffect();
                hit = true;
                DestroyOrReturnBullet();
                return;
            }

            //ダメージ処理
            var target = other.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                CreateHitEffect();
                target.ApplyDamage(damage, Attacker);
                hit = true;

                DestroyOrReturnBullet();
            }

        }

        void CreateHitEffect()
        {
            Instantiate(hitEffectPrefab, transform.position, hitEffectPrefab.transform.rotation);
        }

        void DestroyOrReturnBullet()
        {
            if (pool == null)
            {
                Destroy(gameObject);
            }
            else
            {
                pool.Return(this);
            }
        }
    }
}


