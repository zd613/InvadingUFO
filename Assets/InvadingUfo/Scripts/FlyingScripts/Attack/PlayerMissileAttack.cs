using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Ame
{
    //ミサイルは戦闘機から出たとき少し下側へ落ちて、後ろ側へ流れたのち、速度上げて、敵へと向かう
    public class PlayerMissileAttack : MonoBehaviour
    {
        public GameObject missileObject;
        public Transform missileLauncher;
        public GameObject target;


        public float lockonRange = 10;
        public OnTriggerEnterSender lockonColliderEnterSender;
        public OnTriggerExitSender lockonColliderExitSender;

        public float coolTimeSec = 4;
        Coroutine coolDownCoroutine;
        [Header("ミサイル")]
        public int maxMissile;
        int missileCounter;
        public float reloadTime = 5;
        Coroutine reloadCoroutine;
        public UnityEngine.UI.Text missileCounterText;
        public UnityEngine.UI.Slider reloadSlider;

        public List<MissileTargetInfo> lockonTargets = new List<MissileTargetInfo>();
        Rect rect = new Rect(0, 0, 1, 1);

        public Transform crosshair;
        public GameObject missileHitCamera;
        public float missileHitViewTimeSec = 5;

        [Header("UI")]
        public GameObject missileTargetUI;
        Camera mainCamera;
        public UnityEngine.UI.Slider coolTimeSlider;

        private void Awake()
        {
            missileCounter = maxMissile;

        }



        private void Start()
        {
            if (lockonColliderEnterSender != null)
            {
                lockonColliderEnterSender.OnTriggerEnterCalled += TriggerEnter;
            }
            if (lockonColliderExitSender != null)
            {
                lockonColliderExitSender.OnTriggerExitCalled += TriggerExit;
            }


            if (missileCounterText != null)
            {
                missileCounterText.text = missileCounter.ToString();
            }
            mainCamera = Camera.main;
        }

        void Update()
        {
            UpdateTarget();

            //set target
            float min = float.MaxValue;
            MissileTargetInfo nearest = null;
            var crosshairScreenPos = mainCamera.WorldToViewportPoint(crosshair.position);

            foreach (var item in lockonTargets)
            {
                if (item.CanSeeInView)
                {
                    var pos = mainCamera.WorldToViewportPoint(item.CommonCore.transform.position);
                    var distance = Vector3.Distance(crosshairScreenPos, pos);
                    if (distance < min)
                    {
                        min = distance;
                        nearest = item;
                    }
                }
            }

            target = nearest?.CommonCore?.gameObject;

            //update ui
            if (missileTargetUI != null)
            {
                if (target != null)
                {

                    if (!missileTargetUI.activeInHierarchy)
                        missileTargetUI.SetActive(true);
                    var pos = RectTransformUtility.WorldToScreenPoint(mainCamera, target.transform.position);
                    missileTargetUI.transform.position = pos;

                }
                else
                {
                    if (missileTargetUI.activeInHierarchy)
                        missileTargetUI.SetActive(false);
                }
            }
        }

        public void Fire()
        {
            Fire(target);
        }

        void UpdateTarget()
        {
            if (lockonTargets == null)
                return;

            //死んでるやつ削除
            lockonTargets.RemoveAll(x => x == null || x.CommonCore == null || !x.CommonCore.IsAlive);

            //カメラで見えてるか判定
            foreach (var item in lockonTargets)
            {
                item.CanSeeInView = CanSeeInView(item.CommonCore.transform.position);
            }

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


            //player
            missile.OnMissileHit += (t) =>
            {
                missileHitCamera.SetActive(true);
                //print(missileHitCamera.activeInHierarchy);

                var fc = missileHitCamera.GetComponent<FollowingCamera>();
                fc.target = t;
                fc.SetActiveWithDelay(false, missileHitViewTimeSec);

            };

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
            //yield return new WaitForSeconds(coolTimeSec);
            float time = 0;
            while (true)
            {
                yield return null;
                time += Time.deltaTime;
                coolTimeSlider.value = time / coolTimeSec;
                if (time > coolTimeSec)
                {
                    break;
                }
            }
            coolTimeSlider.value = 0;
            coolDownCoroutine = null;
        }

        bool CanSeeInView(Vector3 position)
        {
            var toLocalPos = transform.InverseTransformPoint(position);//zが正なら正面　負なら後ろ側にターゲットがいる

            return toLocalPos.z > 0 && rect.Contains(Camera.main.WorldToViewportPoint(position));
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
        void TriggerEnter(Collider other)
        {
            var cc = other.GetComponentInParent<BaseCore>();
            if (cc == null)
                return;


            foreach (var item in lockonTargets)
            {
                if (item.CommonCore == cc)
                    return;
            }


            var info = new MissileTargetInfo()
            {
                CommonCore = cc,
                CanSeeInView = CanSeeInView(cc.transform.position),
            };
            lockonTargets.Add(info);
        }

        private void TriggerExit(Collider other)
        {
            var bc = other.GetComponentInParent<BaseCore>();
            if (bc == null)
                return;

            for (int i = 0; i < lockonTargets.Count; i++)
            {
                if (lockonTargets[i].CommonCore == bc)
                {
                    lockonTargets.RemoveAt(i);
                }
            }
        }


        [System.Serializable]
        public class MissileTargetInfo
        {
            public BaseCore CommonCore;
            public bool CanSeeInView;//画面上に表示されるかどうか
        }
    }
}
