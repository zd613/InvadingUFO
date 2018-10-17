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
    protected Coroutine coolDownCoroutine;
    [Header("音")]
    public AudioSource shootSound;

    public float gunRange = 300;

    bool isSoundPlaying = false;

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


    protected virtual void Start()
    {
        if (crosshair != null)
            crosshair.color = normalColor;

        bulletCounter = maxBullet;
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
        if (shootSound != null)
        {
            if (!shootSound.isPlaying)
            {
                shootSound.Play();
            }
        }
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        coolDownCoroutine = StartCoroutine(CoolDown());

        bulletCounter--;
        reloadSlider.value = (float)bulletCounter / maxBullet;

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
        var obj = Instantiate(gunPrefab, muzzleTransform.position, muzzleTransform.rotation);
        var bullet = obj.GetComponent<Bullet>();
        bullet.range = gunRange;
        bullet.Attacker = gameObject;
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

    void UpdateCrosshairImage()
    {
        if (crosshair == null)
            return;

        Ray ray = new Ray(transform.position, transform.forward * gunRange);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            crosshair.transform.position = hit.point;
            //if ()
            crosshair.color = canHitColor;
        }
        else
        {
            crosshair.transform.position = transform.position + transform.forward * gunRange;

            crosshair.color = normalColor;
        }
    }
}