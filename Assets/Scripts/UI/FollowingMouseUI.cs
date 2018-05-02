using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//main cameraとrectTransform必要
public class FollowingMouseUI : MonoBehaviour
{
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        var viewPos = Camera.main.ScreenToViewportPoint(mousePos);
        viewPos.x *= Screen.width;
        viewPos.y *= Screen.height;

        rectTransform.position = viewPos;
    }
}
