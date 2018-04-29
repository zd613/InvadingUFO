using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class UfoRotate : MonoBehaviour
    {
        public UfoRotationMode mode;

        private void Start()
        {
            //var core = GetComponent<UfoCore>();
            //if (mode == UfoRotationMode.Y30)
            //    core.Rotate += Rotate;
            //else if (mode == UfoRotationMode.Y60)
            //    core.Rotate += Rotate60;
        }

        void Rotate()
        {
            transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        }

        void Rotate60()
        {
            transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);

        }

        public enum UfoRotationMode
        {
            Y30,
            Y60,
            Y90,
        }
    }


}
