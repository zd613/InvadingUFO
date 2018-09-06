using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミサイルは戦闘機から出たとき少し下側へ落ちて、後ろ側へ流れたのち、速度上げて、敵へと向かう
public class MissileAttack : MonoBehaviour
{
    public GameObject missileObject;
    public Transform missileLauncher;
    public GameObject target;

    public float lockonRange = 10;

    // Use this for initialization
    void Start()
    {
        //UpdateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();

        if (Input.GetKeyDown(KeyCode.M))
        {

            Fire(target);
        }
    }

    void UpdateTarget()
    {
        var objects = GameObject.FindObjectsOfType(typeof(Health));

        GameObject nearest = null;
        float min = float.MaxValue;

        foreach (var item in objects)
        {

            var go = item as MonoBehaviour;

            //変換できて、自分自身でない
            if (go != null && !ReferenceEquals(go.transform, transform))
            {
                print("hello");
                var distance = Vector3.Distance(transform.position, go.transform.position);
                if (distance > lockonRange)
                {
                    continue;
                }

                if (min > distance)
                {
                    min = distance;
                    nearest = go.gameObject;
                }
            }
        }
        print(min);
        target = nearest;
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
}
