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
    protected Coroutine fireCoroutine;
    [Header("音")]
    public AudioSource shootSound;

    private float rayLength;


    protected virtual void Start()
    {
        var aapc = GetComponent<AAPlayerCore>();
        if (aapc != null)
        {
            rayLength = aapc.GunRange;
        }
    }

    protected virtual void Update()
    {
        if (showDebugRay)
            Debug.DrawRay(muzzleTransform.position, transform.forward * rayLength);
    }

    public virtual bool Fire()
    {
        if (fireCoroutine != null)
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
        fireCoroutine = StartCoroutine(FireInterval());
        return true;
    }

    protected void CreateBullet()
    {
        var obj = Instantiate(gunPrefab, muzzleTransform.position, muzzleTransform.rotation);
        var bullet = obj.GetComponent<Bullet>();
        bullet.Attacker = gameObject;
    }

    protected IEnumerator FireInterval()
    {
        yield return new WaitForSeconds(coolTimeSecond);
        fireCoroutine = null;
    }
}