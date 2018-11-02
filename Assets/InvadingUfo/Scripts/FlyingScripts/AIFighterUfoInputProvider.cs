using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFighterUfoInputProvider : BaseUfoInputProvider
{
    public BasePlaneCore target;
    UfoManager ufoManager;
    PlaneManager planeManager;

    PlanePitchYawController pitchYaw = new PlanePitchYawController();

    //ほかの飛行機とぶつからないようにするなど
    public float avoidanceRadius = 100;

    private void Awake()
    {
        pitchYaw.transform = transform;
        attack = GetComponent<Attack>();
        selfCore = GetComponent<BaseUfoCore>();
    }

    private void Start()
    {
        ufoManager = GameObject.FindWithTag("UfoManager").GetComponent<UfoManager>();
        planeManager = GameObject.FindWithTag("PlaneManager").GetComponent<PlaneManager>();

    }

    BaseUfoCore selfCore;
    Attack attack;

    //ターゲットが離れすぎたとき変える
    public float targetChangeDistance = 200;

    public bool debug = false;

    public override void UpdateInputStatus()
    {

        PitchValue = 0;
        YawValue = 0;
        if (target == null || !target.IsAlive)
        {

            target = planeManager.GetPlane(Random.Range(0, planeManager.Count));

            //target = ufoManager.GetUfo(Random.Range(0, ufoManager.Count));
            if (target == null)
            {
                return;
            }
        }
        int layerMask = LayerMask.GetMask("Ufo");
        var colliders = Physics.OverlapSphere(transform.position, avoidanceRadius, layerMask);

        //近くにいる飛行機の数　数える
        var ufoList = new List<BaseUfoCore>();

        for (int i = 0; i < colliders.Length; i++)
        {
            var buc = colliders[i].GetComponentInParent<BaseUfoCore>();
            if (buc == selfCore)
                continue;
            if (!ufoList.Contains(buc))
            {
                ufoList.Add(buc);
            }
        }

        foreach (var item in ufoList)
        {
            var dir = item.transform.forward;

        }



        //移動
        if (ufoList.Count == 0)
        {
            FollowTarget();
        }
        else
        {
            var v3 = Vector3.zero;
            foreach (var item in ufoList)
            {
                var diff = item.transform.position - transform.position;
                v3 += -diff.normalized;

            }

            v3 /= ufoList.Count;
            //print(v3);

            pitchYaw.SetPitchYawLookingAt(transform.position + v3);
            PitchValue = pitchYaw.Pitch;
            YawValue = pitchYaw.Yaw;

        }

        var distance = Vector3.Distance(transform.position, target.transform.position);

        //if (distance > targetChangeDistance)
        //{
        //    target = planeManager.GetPlane(Random.Range(0, planeManager.Count));

        //    if (target == null)
        //        return;
        //}

        //攻撃
        SpecialKey1 = false;
        if (distance < attack.gunRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attack.gunRange))
            {
                var hitLayer = hit.collider.gameObject.layer;
                if (hitLayer != LayerMask.GetMask("Ufo"))
                {
                    SpecialKey1 = true;
                }
            }
        }
    }

    void FollowTarget()
    {
        pitchYaw.SetPitchYawLookingAt(target.transform.position);
        PitchValue = pitchYaw.Pitch;
        YawValue = pitchYaw.Yaw;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);

    }
}
