using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircleGui : Image, ICanvasRaycastFilter
{

    public float radius;
    [SerializeField] private float r = 10;

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {

        return Vector2.Distance(screenPoint, transform.position) < r;
    }



}
