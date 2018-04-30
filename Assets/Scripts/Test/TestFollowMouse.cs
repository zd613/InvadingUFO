using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollowMouse : MonoBehaviour
{
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //Vector2 pos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, Camera.main, out pos);


        //rectTransform.transform.position = transform.TransformPoint(pos);

        Vector2 mousePos = Input.mousePosition;
        var viewPos = Camera.main.ScreenToViewportPoint(mousePos);
        viewPos.x *= Screen.width;
        viewPos.y *= Screen.height;
        print(viewPos);

        rectTransform.position = viewPos;
    }
}
