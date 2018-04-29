using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class UfoMove : MonoBehaviour
    {
        public float speed;
        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Move()
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
    }
}
