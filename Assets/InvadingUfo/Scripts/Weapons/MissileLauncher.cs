using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ame
{
    public class MissileLauncher : MonoBehaviour
    {
        public GameObject missilePrefab;
        private float rotationSpeed = 20;

        [Tooltip("test")]
        public GameObject testTarget;

        private void Update()
        {
            RotateLauncher();
            if (Input.GetKeyDown(KeyCode.M))
            {
                LaunchMissile();
            }
        }

        void RotateLauncher()
        {
            int rotLR = 0;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rotLR = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                rotLR = 1;
            }
            int rotUD = 0;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rotUD = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                rotUD = -1;
            }
            transform.RotateAround(transform.position, transform.up, rotationSpeed * rotLR * Time.deltaTime);
            transform.RotateAround(transform.position, transform.right, rotationSpeed * rotUD * Time.deltaTime);
        }

        Missile LaunchMissile()
        {
            var obj = Instantiate(missilePrefab, transform.position, Quaternion.LookRotation(transform.forward));
            var missile = obj.GetComponent<Missile>();
            missile.Target = testTarget;
            return missile;
        }

    }
}