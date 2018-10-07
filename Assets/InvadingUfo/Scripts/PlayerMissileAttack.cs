using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ame
{
    //ミサイルは戦闘機から出たとき少し下側へ落ちて、後ろ側へ流れたのち、速度上げて、敵へと向かう
    public class PlayerMissileAttack : MonoBehaviour
    {
        public GameObject missileObject;
        public Transform missileLauncher;
        public GameObject target;

        public float lockonRange = 10;
        public OnTriggerEnterSender lockonCollider;

        private void Start()
        {
            if (lockonCollider == null)
            {
                return;
            }
            lockonCollider.OnTriggerEnterCalled += A;
        }

        public List<CommonCore> lockonTargets = new List<CommonCore>();

        // Update is called once per frame
        void Update()
        {
            UpdateTarget();

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (coolDownCoroutine == null)
                {
                    Fire(target);
                    coolDownCoroutine = StartCoroutine(CoolDown());
                }
            }
        }

        //TODO:update targetをkaku
        void UpdateTarget()
        {
            if (lockonTargets == null)
                return;

            lockonTargets.RemoveAll(x => x == null);
            lockonTargets.RemoveAll(x => x.gameObject == null);//撃墜されてたら


            //var objects = GameObject.FindObjectsOfType(typeof(Health));

            //GameObject nearest = null;
            //float min = float.MaxValue;

            //foreach (var item in objects)
            //{

            //    var go = item as MonoBehaviour;

            //    //変換できて、自分自身でない
            //    if (go != null && !ReferenceEquals(go.transform, transform))
            //    {
            //        var distance = Vector3.Distance(transform.position, go.transform.position);
            //        if (distance > lockonRange)
            //        {
            //            continue;
            //        }

            //        if (min > distance)
            //        {
            //            min = distance;
            //            nearest = go.gameObject;
            //        }
            //    }
            //}
            //target = nearest;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, lockonRange);
        }

        void Fire(GameObject target)
        {
            var obj = Instantiate(missileObject, missileLauncher.position, transform.rotation);
            var missile = obj.GetComponent<Missile>();
            missile.attacker = gameObject;
            missile.target = target;


        }

        public float coolTimeSec = 4;

        Coroutine coolDownCoroutine;
        IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(coolTimeSec);
            coolDownCoroutine = null;
        }

        //TODO:common core を持っているやつはプレイヤーと味方とufoのどれかという前提で
        //書いてる
        void A(Collider other)
        {
            var cc = other.GetComponentInParent<CommonCore>();
            if (cc == null)
                return;

            if (lockonTargets.Contains(cc))
                return;


            lockonTargets.Add(cc);
        }
    }
}
