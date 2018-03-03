using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
	public class UfoMove : MonoBehaviour 
	{
        public float speed;

		private void Start ()
		{
            var core = GetComponent<UfoCore>();
            core.Move += GoStraight;
		}

        public void Move()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void GoStraight()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
