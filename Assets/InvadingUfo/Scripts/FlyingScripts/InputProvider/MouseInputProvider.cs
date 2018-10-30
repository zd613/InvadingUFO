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


    public GameObject arrowUI;


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

    //円内にpointがあるかどうか
    bool InCircle(float radius, Vector2 point)
    {
        return (point.sqrMagnitude <= radius * radius);
    }
}
