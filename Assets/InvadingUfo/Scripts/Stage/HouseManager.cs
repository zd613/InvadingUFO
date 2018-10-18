
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

                activeHouseCount++;
                houses.Add(h);
            }
        }

        //print("house" + activeHouseCount);

    }

    private void Update()
    {
        RemoveDestroyedHouses();
    }

    public House GetRandomHouse()
    {
        RemoveDestroyedHouses();
        //print(houses.Count);
        if (houses.Count == 0)
            return null;
        var index = Random.Range(0, houses.Count);
        return houses[index];
    }

    void RemoveDestroyedHouses()
    {
        for (int i = 0; i < houses.Count; i++)
        {
            if (houses[i] == null || houses[i].gameObject == null)
            {
                houses.RemoveAt(i);
                activeHouseCount--;
            }
        }
    }

    public int GetActiveHouseCount()
    {
        return houses.Count;
    }

}
