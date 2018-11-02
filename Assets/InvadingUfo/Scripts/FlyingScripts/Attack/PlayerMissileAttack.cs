using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Ame
{
    //ミサイルは戦闘機から出たとき少し下側へ落ちて、後ろ側へ流れたのち、速度上げて、敵へと向かう
    public class PlayerMissileAttack : MonoBehaviour
    {
        public bool isActive = true;
        public GameObject missileObject;
        public Transform missileLauncher;
        public GameObject target;


        public float lockonRange = 10;
        public OnTriggerEnterSender lockonColliderEnterSender;
        public OnTriggerExitSender lockonColliderExitSender;

        public float coolTimeSec = 4;
        Coroutine coolDownCoroutine;
        [Header("ミサイル")]
        //いらない
        public int maxMissile;

        int missileCounter;
        public float reloadTime = 5;
        Coroutine reloadCoroutine;
        public Text missileCounterText;
        public Slider reloadSlider;

        public List<MissileTargetInfo> lockonTargets = new List<MissileTargetInfo>();
        Rect rect = new Rect(0, 0, 1, 1);

        public Transform crosshair;
        public GameObject missileHitCamera;
        public float missileHitViewTimeSec = 5;

        [Header("UI")]
        public GameObject missileTargetUI;
        Camera mainCamera;
        public Slider coolTimeSlider;

        public MissileInitialInfo[] missileInitialInfos;
        int index = 0;


        private void Awake()
        {
            //missileCounter = maxMissile;
            missileCounter = missileInitialInfos.Length;
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
            if (!isActive)
                return;
            UpdateTarget();

            //set target
            float min = float.MaxValue;
            MissileTargetInfo nearest = null;
            //var crosshairScreenPos = mainCamera.WorldToViewportPoint(crosshair.position);
            var mouseViewPoint = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            //print(mouseViewPoint);

            foreach (var item in lockonTargets)
            {
                if (item.CanSeeInView)
                {
                    var pos = mainCamera.WorldToViewportPoint(item.BaseCore.transform.position);
                    pos.z = 0;//画面上なのでzは使わない
                    var distance = Vector3.Distance(mouseViewPoint/*crosshairScreenPos*/, pos);
                    if (distance < min)
                    {
                        min = distance;
                        nearest = item;
                    }
                }
            }

            target = nearest?.BaseCore?.gameObject;


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
            if (!isActive)
                return;
            Fire(target);
        }

        void UpdateTarget()
        {
            if (lockonTargets == null)
                return;

            //死んでるやつ削除
            lockonTargets.RemoveAll(x => x == null || x.BaseCore == null || !x.BaseCore.IsAlive);

            //カメラで見えてるか判定
            foreach (var item in lockonTargets)
            {
                item.CanSeeInView = CanSeeInView(item.BaseCore.transform.position);
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

            var pos = missileInitialInfos[index].Transform.position;
            var rot = missileInitialInfos[index].Transform.rotation;


            var obj = Instantiate(missileObject, pos, rot);
            var missile = obj.GetComponent<Missile>();
            missile.attacker = gameObject;
            missile.target = target;

            missileInitialInfos[index].Transform.GetChild(0).gameObject.SetActive(false);

            //player
            missile.OnMissileHit += (t) =>
            {
                if (t.gameObject.layer == LayerMask.NameToLayer("Stage"))
                {
                    return;
                }
                else if (t.gameObject.layer == LayerMask.NameToLayer("Attractable"))
                {
                    return;
                }
                missileHitCamera.SetActive(true);
                //print(missileHitCamera.activeInHierarchy);

                var fc = missileHitCamera.GetComponent<FollowingCamera>();
                fc.target = t;
                fc.SetActiveWithDelay(false, missileHitViewTimeSec);

            };

            index++;


            //missileCounter--;
            if (missileCounterText != null)
            {
                //missileCounterText.text = missileCounter.ToString();
                missileCounterText.text = (missileInitialInfos.Length - index).ToString();

            }

            if (index >= missileInitialInfos.Length)
            {
                reloadCoroutine = StartCoroutine(Reload());
            }
            else
            {
                coolDownCoroutine = StartCoroutine(CoolDown());
            }

            if (missileCounter <= 0)
            {
                reloadCoroutine = StartCoroutine(Reload());
            }
            //else
            //{
            //    coolDownCoroutine = StartCoroutine(CoolDown());
            //}
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
            missileCounter = missileInitialInfos.Length;
            //missileCounter = maxMissile;
            if (reloadSlider != null)
            {
                reloadSlider.value = 0;
            }
            if (missileCounterText != null)
            {
                missileCounterText.text = missileCounter.ToString();
            }

            foreach (var item in missileInitialInfos)
            {
                item.Transform.GetChild(0).gameObject.SetActive(true);
            }
            index = 0;

            reloadCoroutine = null;
        }

        //TODO:common core を持っているやつはプレイヤーと味方とufoのどれかという前提で
        //書いてる
        void TriggerEnter(Collider other)
        {
            var baseCore = other.GetComponentInParent<BaseCore>();
            if (baseCore == null)
                return;


            foreach (var item in lockonTargets)
            {
                if (item.BaseCore == baseCore)
                    return;
            }


            var info = new MissileTargetInfo()
            {
                BaseCore = baseCore,
                CanSeeInView = CanSeeInView(baseCore.transform.position),
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
                if (lockonTargets[i].BaseCore == bc)
                {
                    lockonTargets.RemoveAt(i);
                }
            }
        }


        [System.Serializable]
        public class MissileTargetInfo
        {
            public BaseCore BaseCore;
            public bool CanSeeInView;//画面上に表示されるかどうか
        }

        [System.Serializable]
        public class MissileInitialInfo
        {
            public Transform Transform;
            public GameObject MissileObject;
        }
    }
}
