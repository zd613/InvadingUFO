using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputProvider : AbstractInputProvider
{
    public float crosshairRayLength = 3;
    public float threashold = 2;

    Vector2 center;

    float radius;//px
    //GameObject testObject;

    private void Awake()
    {
        center = new Vector2(Screen.width / 2, Screen.height / 2);
        //testObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //testObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        radius = Mathf.Max(Screen.height, Screen.width) / 6;
    }

    private void Update()
    {
        //attack
        if (Input.GetMouseButton(0))
        {
            BulletAttack = true;
        }
        else
        {
            BulletAttack = false;
        }

        //rotate
        //RotationInput();

        RotationInput2();


    }

    void RotationInput()
    {

        //スクリーンサイズ変更時に代わるのでupdate
        center = new Vector2(Screen.width / 2, Screen.height / 2);
        radius = Mathf.Min(Screen.height, Screen.width) / 5;

        //x,yに値はいる
        Vector2 mousePos = Input.mousePosition;
        //testObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1));
        //print(mousePos);

        //centerを中心とした座標に変換
        Vector2 relativeMousePos = mousePos - center;
        //print(InCircle(radius, relativeMousePos) + "," + radius + "," + mousePos);
        if (InCircle(radius, relativeMousePos))
        {
            PitchValue = 0;
            YawValue = 0;
        }
        else
        {
            //Pitch
            if (relativeMousePos.y > radius)
            {
                PitchValue = 1;
            }
            else if (relativeMousePos.y < -radius)
            {
                PitchValue = -1;
            }

            //Yaw
            if (relativeMousePos.x > radius)
            {
                YawValue = 1;
            }
            else if (relativeMousePos.x < -radius)
            {
                YawValue = -1;
            }
        }
    }


    void RotationInput2()
    {
        //both screen pos
        var mousePos = Input.mousePosition;
        var crosshairPos =
            RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position + transform.forward * crosshairRayLength);

        print(mousePos + "," + crosshairPos);

        Vector2 deltaV3 = new Vector2(mousePos.x - crosshairPos.x, mousePos.y - crosshairPos.y);

        if (Mathf.Abs(deltaV3.y) < threashold)
        {
            PitchValue = 0;
        }
        else
        {
            if (deltaV3.y > 0)
            {
                PitchValue = 1;
            }
            else
            {
                PitchValue = -1;
            }
        }

        if (Mathf.Abs(deltaV3.x) < threashold)
        {
            YawValue = 0;
        }
        else
        {
            if (deltaV3.x > 0)
            {
                YawValue = 1;
            }
            else
            {
                YawValue = -1;
            }
        }


    }


    //円内にpointがあるかどうか
    bool InCircle(float radius, Vector2 point)
    {
        return (point.sqrMagnitude <= radius * radius);
    }
}
