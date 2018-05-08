using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshairUI : MonoBehaviour
{
    public GameObject player;
    public Image crosshair;
    public float playerRayLength;

    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = crosshair.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Debug.DrawRay(player.transform.position, player.transform.forward * playerRayLength, Color.black);

        var targetPos = player.transform.forward * playerRayLength;
        //rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos);
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position + player.transform.forward * playerRayLength);

    }

}
