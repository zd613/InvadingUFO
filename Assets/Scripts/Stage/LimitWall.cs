using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class LimitWall : MonoBehaviour
    {
        public GameObject wall;

        private GameObject obj;//まだ反転していないもの
        private Vector3 enteredPoint;
        private PlayerCore core;//unlock rotation;
        private void OnTriggerEnter(Collider other)
        {
            var rootObj = other.transform.root.gameObject;
            if (rootObj == null)
                return;

            print("enter");
            if (rootObj != obj)
            {
                print("syori");
                //傾き直すpitch,roll
                var rot = rootObj.transform.eulerAngles;
                rot.x = 0;
                rot.z = 0;
                rootObj.transform.eulerAngles = rot;

                enteredPoint = rootObj.transform.position;
                //操作禁止
                core = rootObj.GetComponent<PlayerCore>();

                core.LockRotation();
                //作戦領域外を表示(player)　

                //暗転(player)

                //機体を裏返す
                rootObj.transform.rotation = Quaternion.LookRotation(-rootObj.transform.forward);
                obj = rootObj;
            }

        }


        private void OnTriggerExit(Collider other)
        {
            print("end");
            core.UnlockRotation();
            obj = null;
        }

        class OutOfAreaInfo
        {
            public GameObject RootGameObject { get; set; }
            public PlayerCore ControlRotation { get; set; }
        }
    }
}
