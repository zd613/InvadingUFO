using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelSelect : MonoBehaviour
{
    public ToggleGroup toggleGroup;

    private void Awake()
    {
    }

    int selectedMissionLevel;
    public void SetLevel()
    {
        var activeToggles = toggleGroup.ActiveToggles();

        if (activeToggles.Count() != 1)
        {
            throw new System.Exception("toggleの数がおかしい");
        }

        var activeToggle = activeToggles.First();
        var parent = activeToggle.transform.parent;

        int i = 0;
        foreach (Transform item in parent.transform)
        {
            var t = item.GetComponent<Toggle>();
            if (t == activeToggle)
            {
                if (i < 4)
                {
                    GameManager.useAllyPlane = false;
                }
                else
                {
                    GameManager.useAllyPlane = true;
                }
                GameManager.missionLevel = i;

                break;
            }
            i++;
        }

    }
}
