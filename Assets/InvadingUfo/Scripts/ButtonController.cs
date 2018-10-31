using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    List<Button> buttons = new List<Button>();

    int selectedIndex = 0;

    private void Start()
    {
        foreach (Transform item in transform)
        {
            var button = item.GetComponent<Button>();
            if (button != null)
            {
                buttons.Add(button);
            }
        }

        //buttons[selectedIndex].Select();
    }
}
