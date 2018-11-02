using System.Collections.Generic;
using UnityEngine;

public class AIPlaneInputProvider : BasePlaneInputProvider
{
    public BaseUfoCore target;
    UfoManager ufoManager;
    PlaneManager planeManager;
    StateContext context;

    PlanePitchYawController pitchYaw = new PlanePitchYawController();

    //ほかの飛行機とぶつからないようにするなど
    public float avoidanceRadius = 100;

    private void Awake()
    {

        pitchYaw.transform = transform;
        attack = GetComponent<Attack>();

        //selfColliders = GetComponentsInChildren<Collider>();
        selfCore = GetComponent<BasePlaneCore>();
    }

    private void Start()
    {
        ufoManager = GameObject.FindWithTag("UfoManager").GetComponent<UfoManager>();
        planeManager = GameObject.FindWithTag("PlaneManager").GetComponent<PlaneManager>();

    }

    //Collider[] selfColliders;

    BasePlaneCore selfCore;

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
            target = ufoManager.GetUfo(Random.Range(0, ufoManager.Count));
            if (target == null)
            {
                return;
            }
        }
        int layerMask = LayerMask.GetMask("Plane", "Player");
        var colliders = Physics.OverlapSphere(transform.position, avoidanceRadius, layerMask);

        //近くにいる飛行機の数　数える
        var planeList = new List<BasePlaneCore>();

        for (int i = 0; i < colliders.Length; i++)
        {
            var pbc = colliders[i].GetComponentInParent<BasePlaneCore>();
            if (pbc == selfCore)
                continue;
            if (!planeList.Contains(pbc))
            {
                planeList.Add(pbc);
            }
        }

        foreach (var item in planeList)
        {
            var dir = item.transform.forward;

        }



        //移動
        if (planeList.Count == 0)
        {
            FollowTarget();
            //if (debug)
            //    print("target");

        }
        else
        {
            var v3 = Vector3.zero;
            foreach (var item in planeList)
            {


                var diff = item.transform.position - transform.position;
                v3 += -diff.normalized;

            }

            if (v3 == Vector3.zero)
            {
                //if(debug)
                //{
                //    foreach (var item in planeList)
                //    {
                //        print(item.name);

                //    }
                //}
                //if (planeList.Count == 0)
                //{
                //    print("count==0)");

                //}
                //print(colliders.Length);

            }

            v3 /= planeList.Count;
            //print(v3);


            pitchYaw.SetPitchYawLookingAt(transform.position + v3);
            PitchValue = pitchYaw.Pitch;
            YawValue = pitchYaw.Yaw;
            //if (debug)
            //    print("avoid");

        }

        var distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > targetChangeDistance)
        {
            target = ufoManager.GetUfo(Random.Range(0, ufoManager.Count));
            if (target == null)
                return;
        }

        //攻撃
        BulletAttack = false;
        if (distance < attack.gunRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attack.gunRange))
            {
                var hitLayer = hit.collider.gameObject.layer;
                if (hitLayer != LayerMask.GetMask("Plane", "Player"))
                {
                    BulletAttack = true;
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
