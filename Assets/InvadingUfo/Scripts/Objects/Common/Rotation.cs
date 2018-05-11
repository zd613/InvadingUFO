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
    [SerializeField]
    protected bool reverseRollControl = true;
    [SerializeField]
    protected bool reversePitchControl;

    //ボーダー
    protected float pitchReverseBorder = 2;
    protected float rollReverseBorder = 2;


    protected Vector3 signedEulerAngles;




    protected virtual void Awake()
    {
        if (reverseRollControl)
        {
            power.Roll = -power.Roll;
        }
        if (reversePitchControl)
        {
            power.Pitch = -power.Pitch;
        }
        rb = GetComponent<Rigidbody>();
    }


    //private void Update()
    //{
    //    if (FixRotation)
    //    {
    //        //rootObj.transform.rotation = initialRotation;
    //        return;
    //    }
    //    //1回目　他のクラスから呼び出されてローテーションが変更されているので
    //    UpdateSignedEulerAngles();

    //    //角度制限

    //    var newRotX = Mathf.Clamp(signedEulerAngles.x, pitchLimit.Min, pitchLimit.Max);
    //    var newRotZ = Mathf.Clamp(signedEulerAngles.z, rollLimit.Min, rollLimit.Max);
    //    //rootObj.transform.eulerAngles = new Vector3(newRotX, rootObj.transform.eulerAngles.y, newRotZ);


    //    //2回目　角度制限したので
    //    UpdateSignedEulerAngles();
    //}

    public virtual void Rotate(float pitch, float yaw)
    {
        if (!isActive)
            return;
        Pitch(pitch);
        Yaw(yaw);
    }

    #region Rotations

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
            rb.Rotate(Vector3.right * value * power.Pitch * Time.deltaTime);
            //transform.Rotate(Vector3.right * value * power.Pitch * Time.deltaTime);
            //TODO:角度制限
        }
    }

    protected virtual void Roll(float value)
    {
        if (value == 0)
        {
            ReverseToInitialPosition(Vector3.forward, signedEulerAngles.z, rollReverseBorder, reversePower.Roll);
        }
        else
        {
            rb.Rotate(Vector3.forward, value * power.Roll * Time.deltaTime);
            //TODO:角度制限
        }
        //signedEulerAngles.z = Angle.ToSignedEulerAngle(transform.eulerAngles.z);

    }

    //private void Yaw(float value)
    //{
    //    transform.Rotate(Vector3.up, power.Yaw * Mathf.Deg2Rad * -signedEulerAngles.z * Time.deltaTime, Space.World);
    //    //signedEulerAngles.y = Angle.ToSignedEulerAngle(transform.eulerAngles.y);

    //}

    protected virtual void Yaw(float value)
    {
        if (value == 0)
        {
            //ReverseToInitialPosition(Vector3.up, signedEulerAngleY, pitchReverseBorder, reversePower.Pitch);
        }
        else
        {
            rb.Rotate(Vector3.up, power.Yaw * value * Time.deltaTime, Space.World);

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
