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
            //TODO:UFO Coreを一般的なものに変える
            var obj = other.GetComponentInParent<UfoCore>().gameObject;
            if (obj == Attacker)
            {
                return;
            }

            var target = other.gameObject.GetComponentInParent<IDamagable>();
            if (target != null)
            {
                print(obj.name);
                target.ApplyDamage(damage, Attacker);
                Destroy(gameObject);
            }

        }
    }
}
