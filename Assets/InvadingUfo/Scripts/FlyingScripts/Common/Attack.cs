using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ame;
using UnityEngine.UI;

//TODO:BulletAttackなどの名前へ変更する
public class Attack : MonoBehaviour
{
    public bool isActive = true;
    [Header("プレハブ、トランスフォーム")]
    public GameObject gunPrefab;
    public Transform muzzleTransform;
    public ParticleSystem muzzleFlash;
    [Header("パラメータ")]
    public bool showDebugRay = true;
    public float coolTimeSecond = 0.25f;
    public float damage = 1;
    protected Coroutine coolDownCoroutine;
    [Header("音")]
    public AudioSource shootSound;

    public float gunRange = 300;


    [Header("crosshair")]
    public SpriteRenderer crosshair;
    public Color normalColor = Color.white;
    public Color canHitColor = Color.red;

    [Header("bullet")]
    public int maxBullet;
    public float reloadTime = 3;
    public int bulletCounter = 0;
    Coroutine reloadCoroutine;
    //TODO: UIのクラスへ移動 
    //reloadの時と、弾撃つ時のsliderを変えた方がいいかも
    //reload時は円状のuiで赤にするなど
    public Slider reloadSlider;

    [Header("pool")]
    public BulletPool pool;


    protected virtual void Start()
    {
        if (crosshair != null)
            crosshair.color = normalColor;

        bulletCounter = maxBullet;

        layerMaskValue = ~LayerMask.GetMask(new string[] { "AreaWall", "Weapon" });

    }

    protected virtual void Update()
    {
        if (!isActive)
            return;
        if (showDebugRay)
            Debug.DrawRay(muzzleTransform.position, transform.forward * gunRange);

        UpdateCrosshairImage();
    }

    public virtual bool Fire()
    {
        if (!isActive)
            return false;
        if (reloadCoroutine != null)
            return false;
        if (coolDownCoroutine != null)
            return false;

        CreateBullet();

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        coolDownCoroutine = StartCoroutine(CoolDown());

        bulletCounter--;
        if (reloadSlider != null)
        {
            reloadSlider.value = (float)bulletCounter / maxBullet;
        }

        if (bulletCounter <= 0)
        {
            bulletCounter = 0;
            if (reloadSlider != null)
            {
                reloadSlider.value = 0;
            }
            reloadCoroutine = StartCoroutine(Reload());
        }


        return true;
    }

    protected void CreateBullet()
    {
        Bullet bullet;
        if (pool == null)
        {
            //print("null");

            var obj = Instantiate(gunPrefab, muzzleTransform.position, muzzleTransform.rotation);
            bullet = obj.GetComponent<Bullet>();

        }
        else
        {

            bullet = pool.GetBullet();
            bullet.pool = pool;
            bullet.gameObject.transform.position = muzzleTransform.position;
            bullet.gameObject.transform.rotation = muzzleTransform.rotation;
        }
        bullet.range = gunRange;
        bullet.damage = damage;
        bullet.Attacker = gameObject;
        bullet.Initialize();
    }

    protected IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolTimeSecond);
        coolDownCoroutine = null;
    }

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
        bulletCounter = maxBullet;

        if (reloadSlider != null)
        {
            reloadSlider.value = 1;
        }
        reloadCoroutine = null;
    }


    public bool isRaycastHit;

    void UpdateCrosshairImage()
    {
        if (crosshair == null)
            return;

        Ray ray = new Ray(transform.position, transform.forward * gunRange);
        RaycastHit hit;

        //layerMask = ~layerMask;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, gunRange, layerMaskValue))
        {

            hitLine.SetPosition(1, transform.InverseTransformPoint(hit.point));
        }
        else
        {
            hitLine.SetPosition(1, new Vector3(0, 0, 0));
        }

        crosshair.transform.position = transform.position + transform.forward * gunRange;
        gunRangeLine.SetPosition(1, transform.InverseTransformPoint(transform.position + transform.forward * gunRange));

    }

    int layerMaskValue;
    public LineRenderer gunRangeLine;
    public LineRenderer hitLine;


    public void StartShootSound()
    {
        if (shootSound != null)
        {
            shootSound.Play();
        }
    }

    public void StopShootSound()
    {
        if (shootSound != null)
        {
            shootSound.Stop();
        }
    }
}