using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpawner : MonoBehaviour
{
    public bool isActive = true;
    public GameObject ufoPrefab;
    public int spawnCount;
    public float minInterval = 1;
    public float maxInterval = 2;

    public List<UfoRouteInfo> routeInfo;

    int current = 0;
    int routeIndex = 0;
    int routeCount = 0;

    private void Awake()
    {
        //SpawnUfo();
        StartCoroutine(S());
        
    }

    void SpawnUfo()
    {
        var rot = Quaternion.identity;
        var obj = Instantiate(ufoPrefab, transform.position, rot);

        var followPath = obj.GetComponent<FollowPathInputProvider>();
        followPath.targetPath = routeInfo[routeIndex].path;
        routeCount++;
        if (routeCount >= routeInfo[routeIndex].ufoCount)
        {
            routeIndex++;
        }
    }

    IEnumerator S()
    {
        while (true)
        {
            if (!isActive)
                continue;

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

[System.Serializable]
public class UfoRouteInfo
{
    public PathController path;
    public int ufoCount;
}
