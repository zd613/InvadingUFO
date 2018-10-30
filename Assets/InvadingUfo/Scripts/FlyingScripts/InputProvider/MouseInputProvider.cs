using Ame;
using UnityEngine;
using static UnityEngine.Mathf;

public class MouseInputProvider : BasePlaneInputProvider
{

    [Header("設定")]
    //pixel
    public float innerCircleRadius = 100;

    //pixel
    public float outerCircleRadius = 200;

    [Header("古いやつ")]
    public float crosshairRayLength = 3;
    public float threashold = 2;
    private float threashold2;
    Vector2 center;

    float radius;//px

    public GameObject arrowUI;


    private void Awake()
    {
        crosshairRayLength = GetComponent<Attack>().gunPrefab.GetComponent<Bullet>().range;
        center = new Vector2(Screen.width / 2, Screen.height / 2);
        //testObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //testObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        radius = Max(Screen.height, Screen.width) / 6;

        threashold2 = threashold / 2;
    }

    public override void UpdateInputStatus()
    {
        //bullet
        if (Input.GetMouseButton(0))
        {
            BulletAttack = true;
        }
        else
        {
            BulletAttack = false;
        }

        //missile
        if (Input.GetMouseButton(1))
        {
            MissileAttack = true;
        }
        else
        {
            MissileAttack = false;
        }

        //pitch yaw
        SetPitchAndYaw();

        if (Input.GetKey(KeyCode.Space))
        {
            Boost = true;
        }
        else
        {
            Boost = false;
        }

    }

    void SetPitchAndYaw()
    {
        var mousePos = Input.mousePosition;

        var screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        //画面中心が中心の座標
        Vector2 relativePos = mousePos - screenCenter;





        //内側の円より外のとき

        //y軸正が0°の時計回り角度　正　反時計回り　負
        var angle = -Vector3.SignedAngle(Vector3.up, relativePos, Vector3.forward);

        var squareX = relativePos.x;
        var squareY = relativePos.y;

        //一辺がouter radius の正方形に直す
        squareX = Mathf.Clamp(squareX, -outerCircleRadius, outerCircleRadius);
        squareY = Mathf.Clamp(squareY, -outerCircleRadius, outerCircleRadius);

        float sh = 0;//正方形との好転と中心間の距離

        //正方形と中心からの線が交わった時の長さ
        if (angle >= 45)
        {
            sh = outerCircleRadius / Cos(Deg2Rad * (90 - angle));
        }
        else
        {
            sh = outerCircleRadius / Cos(Deg2Rad * angle);
        }
        var rateSquareToCircle = outerCircleRadius / sh;


        //円へと直す
        if (squareX * squareX + squareY * squareY < outerCircleRadius * outerCircleRadius)
        {
            arrowUI.transform.position = (Vector2)(new Vector3(squareX, squareY, 0) + screenCenter);
        }
        else
        {
            arrowUI.transform.position = (new Vector3(squareX, squareY, 0).normalized * outerCircleRadius) + screenCenter;
        }



        //UpdateArrowUI();


        //if (vec.sqrMagnitude > innerCircleRadius * innerCircleRadius)
        //{

        //    //上が0 時計回り180　まで正　反時計回り-180まで　負
        //    var angle = -Vector3.SignedAngle(Vector3.up, vec, Vector3.forward);
        //    print(angle);
        //    x = Mathf.Sin(Mathf.Deg2Rad * angle) * outerCircleRadius;
        //    y = Mathf.Cos(Mathf.Deg2Rad * angle) * outerCircleRadius;
        //    print(x + "," + y);

        //}

        //if (vec.x > 0)
        //{

        //}
        //else if (vec.x < 0)
        //{

        //}

        //var tx = innerCircleRadius * (YawValue * outerCircleRadius);

        //if (Mathf.Abs(relativePos.x) > innerCircleRadius)
        //{
        //    //print(vec.x + "," + innerCircleRadius);

        //    x = relativePos.x / outerCircleRadius;
        //    x = Mathf.Clamp(x, -1, 1);

        //}

        //if (Mathf.Abs(relativePos.y) > innerCircleRadius)
        //{
        //    y = relativePos.y / outerCircleRadius;
        //    y = Mathf.Clamp(y, -1, 1);
        //}
        float yaw = 0;
        float pitch = 0;

        //内側の円より内側
        if (squareX * squareX + squareY * squareY < innerCircleRadius * innerCircleRadius)
        {
            if (arrowUI.activeInHierarchy)
            {
                arrowUI.SetActive(false);
            }
        }
        //外側
        else
        {
            yaw = squareX / outerCircleRadius;
            pitch = squareY / outerCircleRadius;


            if (!arrowUI.activeInHierarchy)
            {
                arrowUI.SetActive(true);
            }
            arrowUI.transform.rotation = Quaternion.Euler(0, 0, -angle + 90);
        }

        yaw = Mathf.Clamp(yaw, -1, 1);
        pitch = Mathf.Clamp(pitch, -1, 1);

        YawValue = yaw;
        PitchValue = -pitch;



        //update ui

        //var item = arrowUI;


        ////if (YawValue == 0 && PitchValue == 0)
        ////{
        ////    if (item.activeInHierarchy)
        ////        item.SetActive(false);
        ////    continue;
        ////}
        ////else
        ////{
        ////    if (!item.activeInHierarchy)
        ////        item.SetActive(true);
        ////}

        //var mx = Input.mousePosition.x;
        //var my = Input.mousePosition.y;

        //var pos = new Vector3(YawValue * outerCircleRadius, -PitchValue * outerCircleRadius, 0);

        ////print(pos);
        ////if (pos.x * pos.x + pos.y + pos.y >= outerCircleRadius * outerCircleRadius)//外側の円より外側
        ////{
        ////    pos = pos.normalized * outerCircleRadius;

        ////}
        ////pos = pos.normalized * outerCircleRadius;

        //item.transform.localPosition = pos;

        ////TODO:上が0 右が-90 下　180 左90になるよくわからん 左右逆なるはずでは？検証
        //var angle = Vector3.SignedAngle(Vector3.up, pos, Vector3.forward);


        ////arrowの上-90 右0 下90 左180に合わせるため angle を左右逆転して90度ひく
        ////angle = -angle;
        ////print(angle - 90);
        //var rot = Quaternion.Euler(0, 0, angle + 90);
        //item.transform.localRotation = rot;



    }

    private void UpdateArrowUI()
    {
        //updat ui
        var pos = Input.mousePosition;
        pos.z = 0;

        //TODO: 上が0 右が-90 下180 左90になるよくわからん 左右逆なるはずでは？検証
        var angle = Vector3.SignedAngle(Vector3.up, pos, Vector3.forward);

        arrowUI.transform.position = pos;


        //arrowの上 - 90 右0 下90 左180に合わせるため angle を左右逆転して90度ひく
        //angle = -angle;
        var rot = Quaternion.Euler(0, 0, angle - 90);
        arrowUI.transform.rotation = rot;

        //ここまで
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

        //print(mousePos + "," + crosshairPos);

        Vector2 deltaV3 = new Vector2(mousePos.x - crosshairPos.x, mousePos.y - crosshairPos.y);
        /*
        if (Mathf.Abs(deltaV3.y) < threashold)
        {
            if (deltaV3.y > threashold2)
            {
                PitchValue = 0.3f;
            }
            else if (deltaV3.y < -threashold2)
            {
                PitchValue = -0.3f;
            }
            else
            {
                PitchValue = 0;

            }
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
        }*/

        PitchValue = GetInputValue(deltaV3.y);
        YawValue = GetInputValue(deltaV3.x);




        /*
        if (Mathf.Abs(deltaV3.x) < threashold)
        {
            if (deltaV3.x > threashold2)
            {
                YawValue = 0.01f;
            }
            else if (deltaV3.x < -threashold2)
            {
                YawValue = -0.01f;
            }
            else
            {
                YawValue = 0;
            }
            print(YawValue);

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
        */

    }

    void YawInput(float delta)
    {
        float eps = 3;//px
        //y=sqrt(x)にしたがってyawValueを決める
        //var th = Screen.width / 80;//閾値
        float absDelta = Mathf.Abs(delta);
        //if (absDelta > th)
        //{
        //    if (delta > 0)
        //        YawValue = 1;
        //    else
        //        YawValue = -1;
        //}
        //else if (absDelta < eps)
        //{
        //    YawValue = 0;
        //}
        //else
        //{
        //    YawValue = Mathf.Sqrt(absDelta / th);
        //    if (YawValue > 1)
        //        YawValue = 1;
        //    else if (YawValue < 0.01)
        //    {
        //        YawValue = 0;
        //    }
        //    if (delta < 0)
        //        YawValue = -YawValue;
        //}

    }

    float GetInputValue(float delta)
    {
        float absDelta = Mathf.Abs(delta);
        float inputValue = 0;
        if (absDelta < 0.5f)
        {
            inputValue = 0;
        }
        else
        {
            inputValue = delta / 50;//y=1/50 x
            if (inputValue > 1)
                inputValue = 1;
            else if (inputValue < -1)
                inputValue = -1;
        }
        return inputValue;
    }

    //円内にpointがあるかどうか
    bool InCircle(float radius, Vector2 point)
    {
        return (point.sqrMagnitude <= radius * radius);
    }
}
