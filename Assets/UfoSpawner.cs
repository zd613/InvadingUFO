using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpawner : MonoBehaviour
{

    public GameObject ufoPrefab;
    public int spawnCount;
    public float minInterval = 1;
    public float maxInterval = 2;

    int current = 0;

    private void Awake()
    {
        //SpawnUfo();
        StartCoroutine(S());
    }

    void SpawnUfo()
    {
        var rot = Quaternion.identity;
        var obj = Instantiate(ufoPrefab, transform.position, rot);
    }

    IEnumerator S()
    {
        while (true)
        {
            SpawnUfo();
            current++;
            if (current >= spawnCount)
            {
                break;
            }
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
    }

}
