using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputProvider : AbstractInputProvider
{
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

        //スクリーンサイズ変更時に代わるのでupdate
        center = new Vector2(Screen.width / 2, Screen.height / 2);
        radius = Mathf.Max(Screen.height, Screen.width) / 6;

        //x,yに値はいる
        var mousePos = Input.mousePosition;
        //testObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1));

        print(mousePos);


        //Pitch
        if (mousePos.y > center.y + radius)
        {
            PitchValue = 1;
        }
        else if (mousePos.y < center.y - radius)
        {
            PitchValue = -1;
        }
        else
        {
            PitchValue = 0;
        }

        //Yaw
        if (mousePos.x > center.x + radius)
        {
            YawValue = 1;
        }
        else if (mousePos.x < center.x - radius)
        {
            YawValue = -1;
        }
        else
        {
            YawValue = 0;
        }
    }
}
