using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;

public class Attack : MonoBehaviour
{
    [Header("プレハブ、トランスフォーム")]
    public GameObject gunPrefab;
    public Transform muzzleTransform;
    public ParticleSystem muzzleFlash;
    [Header("パラメータ")]
    public bool showDebugRay = true;
    public float coolTimeSecond = 0.25f;
    protected Coroutine coolDownCoroutine;
    [Header("音")]
    public AudioSource shootSound;

    public float gunRange = 20;


    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (showDebugRay)
            Debug.DrawRay(muzzleTransform.position, transform.forward * gunRange);
    }

    public virtual bool Fire()
    {
        if (coolDownCoroutine != null)
        {
            return false;
        }

        CreateBullet();
        if (shootSound != null)
        {
            shootSound.Play();
        }
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        coolDownCoroutine = StartCoroutine(CoolDown());
        return true;
    }

    protected void CreateBullet()
    {
        var obj = Instantiate(gunPrefab, muzzleTransform.position, muzzleTransform.rotation);
        var bullet = obj.GetComponent<Bullet>();
        bullet.Attacker = gameObject;
    }

    protected IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolTimeSecond);
        coolDownCoroutine = null;
    }
}