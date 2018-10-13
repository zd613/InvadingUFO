using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{


    public List<GameObject> gameObjectList;
    bool isCalculated = false;
    float time = -1;

    GameObject sphere;

    private void Awake()
    {
        gameObjectList = new List<GameObject>();
        foreach (Transform item in transform)
        {
            gameObjectList.Add(item.gameObject);
        }

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 10;
        sphere.GetComponent<SphereCollider>().enabled = false;
    }

    private void Update()
    {
        sphere.transform.position = CalculateCenterPosition();
    }

    public Vector3 CalculateCenterPosition()
    {
        Vector3 center = Vector3.zero;
        foreach (var item in gameObjectList)
        {
            center += item.transform.position;
        }
        center /= gameObjectList.Count;

        return center;
    }


}
