using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{

    public int houseCount;
    public int activeHouseCount;

    private void Awake()
    {
        houseCount = 0;
        activeHouseCount = 0;
        foreach (Transform item in transform)
        {
            houseCount++;

            if (item.gameObject.activeInHierarchy)
                activeHouseCount++;
        }

        print("house" + activeHouseCount);

    }

}
