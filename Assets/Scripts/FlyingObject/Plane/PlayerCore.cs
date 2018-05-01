using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ame
{
    public class PlayerCore : CommonCore
    {
        public Image crosshair;
        public float crosshairForwardLength = 5;

        private RectTransform crosshairRectTransform;

        protected override void Awake()
        {
            base.Awake();
            crosshairRectTransform = crosshair.GetComponent<RectTransform>();
        }

        protected override void Update()
        {

            base.Update();
            UpdateCrosshairPosition();
   
        }

        void UpdateCrosshairPosition()
        {
            var targetPos = transform.position + transform.forward * crosshairForwardLength;
            crosshairRectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos);

        }

    }
}
