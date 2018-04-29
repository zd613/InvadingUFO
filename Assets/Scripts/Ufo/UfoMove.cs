using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public class UfoMove : MonoBehaviour
    {
        public float speed;
        public MoveMode currentMode = MoveMode.Straight;

        private UfoCore core;

        private void Start()
        {
            core = GetComponent<UfoCore>();

            if (currentMode == MoveMode.Straight)
                core.Move += GoStraight;
        }

        public void ChangeMode(MoveMode nextMode)
        {
            if (nextMode == this.currentMode)
                return;

            var now = GetAction(this.currentMode);
            core.Move -= now;
            core.Move += GetAction(nextMode);
        }

        public void Move()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void GoStraight()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        void Stop() { }


        System.Action GetAction(MoveMode mode)
        {
            switch (mode)
            {
                case MoveMode.Stop:
                    return Stop;
                case MoveMode.Straight:
                    return GoStraight;
                default:
                    return null;
            }
        }

    }

    public enum MoveMode
    {
        Stop,
        Straight,
    }
}
