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
        public float coolTimeSec = 4;
        Coroutine coolDownCoroutine;
        [Header("ミサイル")]
        public int maxMissile;
        int missileCounter;
        public float reloadTime = 5;
        Coroutine reloadCoroutine;
        public UnityEngine.UI.Text missileCounterText;
        public UnityEngine.UI.Slider reloadSlider;



        private void Awake()
        {
            missileCounter = maxMissile;
        }

        private void Start()
        {
            if (lockonCollider == null)
            {
                return;
            }
            lockonCollider.OnTriggerEnterCalled += A;

            if (missileCounterText != null)
            {
                missileCounterText.text = missileCounter.ToString();
            }
        }

        public List<CommonCore> lockonTargets = new List<CommonCore>();

        // Update is called once per frame
        void Update()
        {
            UpdateTarget();

            if (Input.GetKeyDown(KeyCode.M))
            {
                Fire(null);
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
            if (coolDownCoroutine != null)
                return;
            if (reloadCoroutine != null)
                return;

            var obj = Instantiate(missileObject, missileLauncher.position, transform.rotation);
            var missile = obj.GetComponent<Missile>();
            missile.attacker = gameObject;
            missile.target = target;

            missileCounter--;
            if (missileCounterText != null)
            {
                missileCounterText.text = missileCounter.ToString();
            }


            if (missileCounter <= 0)
            {
                reloadCoroutine = StartCoroutine(Reload());
            }
            else
            {
                coolDownCoroutine = StartCoroutine(CoolDown());
            }


        }

        IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(coolTimeSec);
            coolDownCoroutine = null;
        }


        //attackのと違う
        protected IEnumerator Reload()
        {
            yield return null;
            float time = 0;
            while (true)
            {
                time += Time.deltaTime;
                if (time > reloadTime)
                {
                    break;
                }
                if (reloadSlider != null)
                {
                    reloadSlider.value = time / reloadTime;
                }
                yield return null;
            }
            missileCounter = maxMissile;
            if (reloadSlider != null)
            {
                reloadSlider.value = 0;
            }
            if (missileCounterText != null)
            {
                missileCounterText.text = missileCounter.ToString();
            }
            reloadCoroutine = null;
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
