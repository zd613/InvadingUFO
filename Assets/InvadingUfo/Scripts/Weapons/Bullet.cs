using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10;
        public float damage = 10;
        public float range = 20;

        private Vector3 start;
        public GameObject Attacker { get; set; }

        public GameObject hitEffectPrefab;

        //２回目防止用
        bool hit = false;

        private void Awake()
        {
            start = transform.position;
        }

        private void Update()
        {

            if (Vector3.Distance(start, transform.position) > range)
            {
                //print(range);
                //print(Vector3.Distance(start, transform.position));
                Destroy(gameObject);
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
                var commonCore = other.GetComponentInParent<CommonCore>();
                if (commonCore != null)
                {
                    if (commonCore.gameObject == Attacker)
                    {
                        return;
                    }
                }
            }

            print("hi");

            if (other.gameObject.layer == LayerMask.NameToLayer("Stage"))
            {
                print("stage");

                CreateHitEffect();
                hit = true;
                Destroy(gameObject);
                return;
            }

            //ダメージ処理
            var target = other.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                CreateHitEffect();
                target.ApplyDamage(damage, Attacker);
                hit = true;

                Destroy(gameObject);
            }

        }

        void CreateHitEffect()
        {
            Instantiate(hitEffectPrefab, transform.position, hitEffectPrefab.transform.rotation);
        }
    }
}


