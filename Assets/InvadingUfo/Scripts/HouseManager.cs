using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{

    public int houseCount;
    public int activeHouseCount;

    public List<House> houses = new List<House>();



    private void Awake()
    {
        houseCount = 0;
        activeHouseCount = 0;
        foreach (Transform item in transform)
        {
            houseCount++;

            if (item.gameObject.activeInHierarchy)
            {
                var h = item.GetComponent<House>();
                if (h == null)
                    continue;

                houses.Add(h);
            }
        }

        print("house" + activeHouseCount);

    }

    private void Update()
    {
        
    }

    void UpdateHouseCount()
    {

    }

    public int GetActiveHouseCount()
    {
        return houses.Count;
    }

}
