using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class LimitWall : MonoBehaviour
    {
        public GameObject wall;


        private Vector3 enteredPoint;

        OutOfAreaInfos infos = new OutOfAreaInfos();


        private void OnTriggerEnter(Collider other)
        {
            var rootObj = other.transform.root.gameObject;
            if (rootObj == null)
                return;

            print("enter");
            if (!infos.ContainsInfo(rootObj))//infoになかったら
            {
                print("syori");
                //傾き直すpitch,roll

                var rot = rootObj.transform.eulerAngles;
                rot.x = 0;
                rot.z = 0;
                rootObj.transform.eulerAngles = rot;

                //enteredPoint = rootObj.transform.position;


                //操作禁止

                var r = rootObj.GetComponent<IRestrictableRotation>();
                if (r == null)
                {
                    return;
                }
                r.RestrictRotation();

                //info設定
                infos.Add(rootObj, r);

                //作戦領域外を表示(player)　

                //暗転(player)

                //機体を裏返すor 壁に垂直な方向へ向ける

                //rootObj.transform.rotation = Quaternion.LookRotation(-rootObj.transform.forward);
                rootObj.transform.rotation = Quaternion.LookRotation(wall.transform.forward);

            }

        }


        private void OnTriggerExit(Collider other)
        {
            print("end");
            var root = GetRootObject(other);
            if (infos.ContainsInfo(root))
            {
                infos[root].FreeRotation();
                infos.RemoveInfo(root);
            }
        }

        GameObject GetRootObject(Collider other)
        {
            return other.transform.root.gameObject;
        }

        class OutOfAreaInfos
        {
            Dictionary<GameObject, IRestrictableRotation> dict = new Dictionary<GameObject, IRestrictableRotation>();

            public OutOfAreaInfos()
            {
            }

            public bool ContainsInfo(GameObject obj)
            {
                return dict.ContainsKey(obj);
            }

            public void Add(GameObject obj, IRestrictableRotation restrictable)
            {
                dict.Add(obj, restrictable);
            }

            public void RemoveInfo(GameObject obj)
            {
                dict.Remove(obj);
            }

            public IRestrictableRotation this[GameObject obj]
            {
                get { return dict[obj]; }
            }

        }
    }
}
