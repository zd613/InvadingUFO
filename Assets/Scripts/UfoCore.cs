using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class UfoCore : MonoBehaviour, IDamagable
    {
        [SerializeField]
        float speed = 1;
        [SerializeField]
        Vector3 start;
        [SerializeField]
        Vector3 goal;


        public float Hp { get; private set; }

        private void Start()
        {
            transform.position = start;
            transform.rotation = Quaternion.LookRotation(goal - start);
            Hp = 300;
        }

        private void Update()
        {
            print(Hp);
            if (Input.GetKeyDown(KeyCode.A))
                ApplyDamage(100);
            var diff = Vector3.Distance(transform.position, goal);
            if (diff < 0.001f)
            {
                return;
            }

            GoStraight();
        }

        private void GoStraight()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void ApplyDamage(int damageValue)
        {
            Hp -= damageValue;
            if (Hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
