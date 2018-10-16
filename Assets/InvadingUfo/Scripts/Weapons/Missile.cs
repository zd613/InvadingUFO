using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Ame
{
    public class Missile : MonoBehaviour
    {
        public GameObject target;

        public float speed = 15;
        public float maxRange = 100;
        public GameObject hitEffect;
        public float rotationSpeed = 20;
        public float damage = 100;


        public AudioSource fireSound;

        public GameObject hitSoundPrefab;

        public GameObject Target
        {
            get { return target; }
            set { target = value; }
        }

        Vector3 prePos;
        float totalDistance;
        public GameObject attacker;
        public float firstStageWaitSec = 0.2f;
        public float firstStageGravity = 0.2f;
        public float relativeSpeed;


        //発射時に発射したものと当たらないため
        public float noHitTimeSecToAttacker = 1;
        bool canHitToAttacker = false;

        //event 
        //transform:target
        public event System.Action<Transform> OnMissileHit;

        private void Awake()
        {
            prePos = transform.position;
            //var rb = GetComponent<Rigidbody>();
            //rb.useGravity = true;
            first = StartCoroutine(FirstSage());
            StartCoroutine(SwitchDamageToAttacker());
            fireSound.Play();
        }
        Coroutine first;
        bool firstStage = true;

        IEnumerator SwitchDamageToAttacker()
        {
            var collider = GetComponentInChildren<Collider>();
            collider.isTrigger = true;
            yield return new WaitForSeconds(noHitTimeSecToAttacker);
            canHitToAttacker = true;
            //collider.isTrigger = false;
        }

        IEnumerator FirstSage()
        {
            yield return new WaitForSeconds(firstStageWaitSec);
            //TODO:後ろの火つける
            firstStage = false;
        }

        private void Update()
        {
            if (firstStage)
            {
                transform.Translate(Vector3.forward * relativeSpeed * Time.deltaTime);
                transform.Translate(Vector3.down * firstStageGravity * Time.deltaTime, Space.World);
                return;
            }



            if (target != null)
            {
                //follow target
                var lookRot = Quaternion.LookRotation(target.transform.position - transform.position);
                var rot = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
                transform.rotation = rot;
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);


            var deltaDistance = Vector3.Distance(prePos, transform.position);
            totalDistance += deltaDistance;
            if (totalDistance > maxRange)
            {
                OutOfRange();
            }

            prePos = transform.position;
        }


        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (!canHitToAttacker)
        //    {
        //        if (collision.gameObject == attacker)
        //            return;
        //    }

        //    foreach (var contact in collision.contacts)
        //    {
        //        var obj = Instantiate(hitEffect, contact.point, transform.rotation);

        //    }

        //    //hp 削る
        //    var health = collision.gameObject.GetComponent<IDamageable>();
        //    if (health != null)
        //    {
        //        health.ApplyDamage(damage, attacker);
        //    }

        //    print("missile hit:" + collision.gameObject.name);

        //    if (hitSoundPrefab != null)
        //    {
        //        Instantiate(hitSoundPrefab, transform.position, Quaternion.identity);
        //    }

        //    Destroy(gameObject);
        //}

        private void OnTriggerEnter(Collider other)
        {
            //print("other");

            var collision = other;
            if (!canHitToAttacker)
            {
                if (collision.gameObject == attacker)
                    return;
            }


            var obj = Instantiate(hitEffect, transform.position, transform.rotation);


            //hp 削る
            //print("missile hit");
            var health = collision.gameObject.GetComponentInParent<Health>();
            if (health != null)
            {
                health.ApplyDamage(damage, attacker);
                //print("apply damage");
            }


            if (hitSoundPrefab != null)
            {
                Instantiate(hitSoundPrefab, transform.position, Quaternion.identity);
            }
            OnMissileHit?.Invoke(health.transform);

            Destroy(gameObject);
        }

        void OutOfRange()
        {
            Destroy(gameObject);
        }
    }
}