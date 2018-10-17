//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Ame;

////anti aircraft core
//public class AAPlayerCore : BaseCore
//{
//    [Space]
//    public GameObject crosshair;

//    float gunRange = 0;
//    RectTransform crosshairRectTransform;

//    public float GunRange { get { return gunRange; } }

//    protected override void Awake()
//    {
//        base.Awake();
//        SetGunRange();

//        if (crosshair != null)
//        {
//            crosshairRectTransform = crosshair.GetComponent<RectTransform>();

//        }
//    }

//    protected override void Update()
//    {
//        if (inputProvider.BulletAttack)
//        {
//            if (attack != null)
//            {
//                attack.Fire();
//            }
//        }

//        if (rotation != null)
//        {
//            rotation.Rotate(inputProvider.PitchValue, inputProvider.YawValue);
//        }
//        UpdateCrosshairPosition();
//    }

//    void SetGunRange()
//    {
//        var attack = GetComponent<Attack>();
//        if (attack != null)
//        {
//            var gp = attack.gunPrefab;
//            if (gp != null)
//            {
//                var bullet = gp.GetComponent<Bullet>();
//                if (bullet != null)
//                {
//                    gunRange = bullet.range;
//                }
//            }
//        }
//    }

//    void UpdateCrosshairPosition()
//    {
//        if (crosshair == null)
//            return;

//        Vector3 targetPos = transform.position + transform.forward * gunRange;
//        crosshairRectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos);
//    }
//}
