using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミサイルは戦闘機から出たとき少し下側へ落ちて、後ろ側へ流れたのち、速度上げて、敵へと向かう
public class MissileAttack : MonoBehaviour
{
    public GameObject missileObject;
    public Transform missileLauncher;
    public GameObject target;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Fire();
        }
    }

    void Fire()
    {
        var obj = Instantiate(missileObject, missileLauncher.position, transform.rotation);
        var missile = obj.GetComponent<Missile>();
        missile.attacker = gameObject;
        missile.target = target;
    }
}
