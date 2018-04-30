using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class TestUfoAi : MonoBehaviour
    {
        //Vector3 start;
        public Vector3 goal;

        float eps = 0.01f;

        private void Awake()
        {
            //transform.rotation = Quaternion.LookRotation(goal - transform.position);
            //start = transform.position;
        }

        System.Action moveAction;

        //private void Start()
        //{
        //    moveAction = GetComponent<UfoMove>().Move;
        //}

        private void Update()
        {
            //if (Vector3.Distance(transform.position, goal) < eps)
            //{
            //    print("ho");

            //    return;

            //}
            //moveAction();

        }
    }
}
