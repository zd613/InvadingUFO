using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class UfoRotation : MonoBehaviour
    {
        public bool isActive = true;

        ////インスペクタで設定
        //public PlayerInputProvider inputProvider;

        Rigidbody rb;

        [Space]
        public RotationPower power;
        public RotationPower reversePower;


        //機体を傾けたとき元の位置に自動的に戻すかどうか
        [SerializeField]
        private bool revertToInitialPosition;



        //反転
        [SerializeField]
        private bool reverseRollControl = true;
        [SerializeField]
        private bool reversePitchControl;

        //ボーダー
        private float pitchReverseBorder = 2;
        private float rollReverseBorder = 2;


        private Vector3 signedEulerAngles;


        private Limit pitchLimit = new Limit(-30, 30);//test value
        private Limit rollLimit = new Limit(-30, 30);//test value

        public bool FixRotation { get; set; }
        private Quaternion initialRotation;

        private void Awake()
        {
            if (reverseRollControl)
            {
                power.Yaw = -power.Yaw;
            }
            if (reversePitchControl)
            {
                power.Pitch = -power.Pitch;
            }
            rb = GetComponent<Rigidbody>();
            //if (inputProvider != null)
            //{
            //    inputProvider.OnPitchRotation += (pitchValue) => Pitch(pitchValue);

            //    inputProvider.OnYawRotation += (yawValue) => Yaw(yawValue);
            //}
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

        public void Rotate(float pitch, float yaw)
        {
            if (!isActive)
                return;
            Pitch(pitch);
            Yaw(yaw);
        }

        #region Rotations

        private void Pitch(float value)
        {
            if (value == 0)
            {
                //float signedEulerAngleX = transform.eulerAngles.x > 180 ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
                //ReverseToInitialPosition(Vector3.right, signedEulerAngleX, pitchReverseBorder, reversePower.Pitch);
            }
            else
            {
                rb.Rotate(Vector3.right * value * power.Pitch * Time.deltaTime);
                //TODO:角度制限
            }
        }

        private void Roll(float value)
        {
            if (value == 0)
            {
                ReverseToInitialPosition(Vector3.forward, signedEulerAngles.z, rollReverseBorder, reversePower.Yaw);
            }
            else
            {
                rb.Rotate(Vector3.forward, value * power.Yaw * Time.deltaTime);
                //TODO:角度制限
            }
            //signedEulerAngles.z = Angle.ToSignedEulerAngle(transform.eulerAngles.z);

        }

        //private void Yaw(float value)
        //{
        //    transform.Rotate(Vector3.up, power.Yaw * Mathf.Deg2Rad * -signedEulerAngles.z * Time.deltaTime, Space.World);
        //    //signedEulerAngles.y = Angle.ToSignedEulerAngle(transform.eulerAngles.y);

        //}

        private void Yaw(float value)
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

        private void ReverseToInitialPosition(Vector3 axis, float signedEulerAngle, float reverseBorder, float reversePower)
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
}

