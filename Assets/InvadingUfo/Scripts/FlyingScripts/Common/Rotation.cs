using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotation : MonoBehaviour
{
    public bool isActive = true;

    protected Rigidbody rb;

    [Space]
    public RotationPower power;
    public RotationPower reversePower;


    //機体を傾けたとき元の位置に自動的に戻すかどうか
    [SerializeField]
    private bool revertToInitialPosition;



    //反転
    public bool reverseYawControl = true;
    public bool reversePitchControl;



    //ボーダー
    protected float pitchReverseBorder = 2;
    protected float rollReverseBorder = 2;


    protected Vector3 signedEulerAngles;

    //pitch角度　制限
    //-90から90度
    //上側へ向くのが-
    //上へ向く方
    [Range(-90, 0)]
    public float minPitchAngle = 45;
    //下へ向く方
    [Range(0, 90)]
    public float maxPitchAngle = 45;

    public bool canTurnByAreaWall = false;

    //plane animation
    public bool useAnimation = false;



    protected virtual void Awake()
    {
        //if (reverseYawControl)
        //{
        //    power.Yaw = -power.Yaw;
        //}
        //if (reversePitchControl)
        //{
        //    power.Pitch = -power.Pitch;
        //}
        rb = GetComponent<Rigidbody>();
    }

    //衝突時にダメージ受ける場合　回転速度などをなくす
    public void CancelRotationByCollisionHit()
    {
        var v = rb.velocity;
        if (!Mathf.Approximately(v.x, 0) || !Mathf.Approximately(v.y, 0) || !Mathf.Approximately(v.z, 0))
        {
            rb.velocity = Vector3.zero;
        }

        v = rb.angularVelocity;
        if (!Mathf.Approximately(v.x, 0) || !Mathf.Approximately(v.y, 0) || !Mathf.Approximately(v.z, 0))
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    //pitch,yaw が1となっていたら、それぞれのpowerの値　degree/sec 回転する
    //pitch yawの値を0-1で調整することで回転を制御する
    public virtual void Rotate(float pitch, float yaw)
    {
        if (!isActive)
            return;
        Pitch(pitch);
        Yaw(yaw);
    }

    public void SetRotation(Vector3 eulerAngles)
    {
        transform.eulerAngles = eulerAngles;
    }

    #region Rotations

    //value が　0の時回転しない　1の時power.Yaw の値 degree/sec 回転する
    //-1 の時　上側へ向く　1 は下側へ向く
    protected virtual void Pitch(float value)
    {
        //TODO:float で0と比較してる
        if (value == 0)
        {
            //float signedEulerAngleX = transform.eulerAngles.x > 180 ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
            //ReverseToInitialPosition(Vector3.right, signedEulerAngleX, pitchReverseBorder, reversePower.Pitch);
        }
        else
        {
            //pitch 制限
            var rotX = transform.eulerAngles.x > 180 ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
            //上方向の回転角度は-
            if (rotX < minPitchAngle)
            {
                if (value < 0)
                {
                    value = 0;
                }
            }
            //下方向への回転角度は+
            else if (rotX > maxPitchAngle)
            {
                if (value > 0)
                {
                    value = 0;
                }
            }
            //回転

            int sign = 1;
            if (reversePitchControl)
            {
                sign = -1;
            }

            rb.Rotate(Vector3.right * value * sign * power.Pitch * Time.deltaTime);
            //transform.Rotate(Vector3.right * value * power.Pitch * Time.deltaTime);
        }
    }

    //protected virtual void Roll(float value)
    //{
    //    if (value == 0)
    //    {
    //        ReverseToInitialPosition(Vector3.forward, signedEulerAngles.z, rollReverseBorder, reversePower.Yaw);
    //    }
    //    else
    //    {
    //        rb.Rotate(Vector3.forward, value * power.Roll * Time.deltaTime);
    //        //TODO:角度制限
    //    }
    //    //signedEulerAngles.z = Angle.ToSignedEulerAngle(transform.eulerAngles.z);

    //}



    protected virtual void Yaw(float value)
    {
        if (value == 0)
        {
            //ReverseToInitialPosition(Vector3.up, signedEulerAngleY, pitchReverseBorder, reversePower.Pitch);
        }
        else
        {
            int sign = 1;
            if (reverseYawControl)
            {
                sign = -1;
            }
            rb.Rotate(Vector3.up, sign * power.Yaw * value * Time.deltaTime, Space.World);

            //TODO:角度制限
        }
    }

    protected virtual void ReverseToInitialPosition(Vector3 axis, float signedEulerAngle, float reverseBorder, float reversePower)
    {
        if (!revertToInitialPosition)
            return;

        if (Mathf.Abs(signedEulerAngle) > reverseBorder)
        {
            int sign = signedEulerAngle > 0 ? -1 : 1;
            transform.Rotate(axis * sign * reversePower * Time.deltaTime);
        }
        //TODO:角度が0になるようにする
    }


    #endregion




}
