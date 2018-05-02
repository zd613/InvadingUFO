using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestMousePointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    Color defaultColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        defaultColor = image.color;
        print("start");
    }
    
}
