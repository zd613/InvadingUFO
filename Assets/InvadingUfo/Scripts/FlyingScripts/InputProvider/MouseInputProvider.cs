﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Ame;

//public class MouseInputProvider : BaseInputProvider
//{
//    public float crosshairRayLength = 3;
//    public float threashold = 2;
//    private float threashold2;
//    Vector2 center;

//    float radius;//px

//    private void Awake()
//    {
//        crosshairRayLength = GetComponent<Attack>().gunPrefab.GetComponent<Bullet>().range;
//        center = new Vector2(Screen.width / 2, Screen.height / 2);
//        //testObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        //testObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
//        radius = Mathf.Max(Screen.height, Screen.width) / 6;

//        threashold2 = threashold / 2;
//    }

//    public override void UpdateInputStatus()
//    {
//        //attack
//        if (Input.GetMouseButton(0))
//        {
//            BulletAttack = true;
//        }
//        else
//        {
//            BulletAttack = false;
//        }

//        RotationInput2();

//        //print(PitchValue + "," + YawValue);

//    }

//    void RotationInput()
//    {

//        //スクリーンサイズ変更時に代わるのでupdate
//        center = new Vector2(Screen.width / 2, Screen.height / 2);
//        radius = Mathf.Min(Screen.height, Screen.width) / 5;

//        //x,yに値はいる
//        Vector2 mousePos = Input.mousePosition;
//        //testObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1));
//        //print(mousePos);

//        //centerを中心とした座標に変換
//        Vector2 relativeMousePos = mousePos - center;
//        //print(InCircle(radius, relativeMousePos) + "," + radius + "," + mousePos);
//        if (InCircle(radius, relativeMousePos))
//        {
//            PitchValue = 0;
//            YawValue = 0;
//        }
//        else
//        {
//            //Pitch
//            if (relativeMousePos.y > radius)
//            {
//                PitchValue = 1;
//            }
//            else if (relativeMousePos.y < -radius)
//            {
//                PitchValue = -1;
//            }

//            //Yaw
//            if (relativeMousePos.x > radius)
//            {
//                YawValue = 1;
//            }
//            else if (relativeMousePos.x < -radius)
//            {
//                YawValue = -1;
//            }
//        }
//    }

//    void RotationInput2()
//    {
//        //both screen pos
//        var mousePos = Input.mousePosition;
//        var crosshairPos =
//            RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position + transform.forward * crosshairRayLength);

//        //print(mousePos + "," + crosshairPos);

//        Vector2 deltaV3 = new Vector2(mousePos.x - crosshairPos.x, mousePos.y - crosshairPos.y);
//        /*
//        if (Mathf.Abs(deltaV3.y) < threashold)
//        {
//            if (deltaV3.y > threashold2)
//            {
//                PitchValue = 0.3f;
//            }
//            else if (deltaV3.y < -threashold2)
//            {
//                PitchValue = -0.3f;
//            }
//            else
//            {
//                PitchValue = 0;

//            }
//        }
//        else
//        {
//            if (deltaV3.y > 0)
//            {
//                PitchValue = 1;
//            }
//            else
//            {
//                PitchValue = -1;
//            }
//        }*/

//        PitchValue = GetInputValue(deltaV3.y);
//        YawValue = GetInputValue(deltaV3.x);
//        /*
//        if (Mathf.Abs(deltaV3.x) < threashold)
//        {
//            if (deltaV3.x > threashold2)
//            {
//                YawValue = 0.01f;
//            }
//            else if (deltaV3.x < -threashold2)
//            {
//                YawValue = -0.01f;
//            }
//            else
//            {
//                YawValue = 0;
//            }
//            print(YawValue);

//        }
//        else
//        {
//            if (deltaV3.x > 0)
//            {
//                YawValue = 1;
//            }
//            else
//            {
//                YawValue = -1;
//            }
//        }
//        */

//    }

//    void YawInput(float delta)
//    {
//        float eps = 3;//px
//        //y=sqrt(x)にしたがってyawValueを決める
//        //var th = Screen.width / 80;//閾値
//        float absDelta = Mathf.Abs(delta);
//        //if (absDelta > th)
//        //{
//        //    if (delta > 0)
//        //        YawValue = 1;
//        //    else
//        //        YawValue = -1;
//        //}
//        //else if (absDelta < eps)
//        //{
//        //    YawValue = 0;
//        //}
//        //else
//        //{
//        //    YawValue = Mathf.Sqrt(absDelta / th);
//        //    if (YawValue > 1)
//        //        YawValue = 1;
//        //    else if (YawValue < 0.01)
//        //    {
//        //        YawValue = 0;
//        //    }
//        //    if (delta < 0)
//        //        YawValue = -YawValue;
//        //}

//    }

//    float GetInputValue(float delta)
//    {
//        float absDelta = Mathf.Abs(delta);
//        float inputValue = 0;
//        if (absDelta < 0.5f)
//        {
//            inputValue = 0;
//        }
//        else
//        {
//            inputValue = delta / 50;//y=1/50 x
//            if (inputValue > 1)
//                inputValue = 1;
//            else if (inputValue < -1)
//                inputValue = -1;
//        }
//        return inputValue;
//    }

//    //円内にpointがあるかどうか
//    bool InCircle(float radius, Vector2 point)
//    {
//        return (point.sqrMagnitude <= radius * radius);
//    }
//}