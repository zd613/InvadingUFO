using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpawner : MonoBehaviour
{

    public GameObject ufoPrefab;

    private void Awake()
    {
        SpawnUfo();
    }

    void SpawnUfo()
    {
        var rot = Quaternion.identity;
        var obj = Instantiate(ufoPrefab, transform.position, rot);
    }

}
