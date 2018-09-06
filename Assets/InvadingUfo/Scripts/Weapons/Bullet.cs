using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class Bullet : MonoBehaviour
    {
        public int speed = 10;
        public int damage = 10;
        public int distance = 20;

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

            if (Vector3.Distance(start, transform.position) > distance)
            {
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


            print("trigger");
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


