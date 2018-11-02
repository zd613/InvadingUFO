using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public BasePlaneCore player;

    public Text altitudeText;


    private void Update()
    {
        altitudeText.text = "altitude: " + player.Altitude;
    }
}
