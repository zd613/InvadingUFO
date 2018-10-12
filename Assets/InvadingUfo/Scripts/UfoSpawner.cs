﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpawner : MonoBehaviour
{
    public bool isActive = true;

    [Header("生成オブジェクト")]
    public List<SpawnInfo> spawnInfo;


    [Header("生成間の時間")]
    public float minInterval = 1;
    public float maxInterval = 2;

    //public List<UfoRouteInfo> routeInfo;

    int spawnInfoListIndex = 0;
    int infoGameObjectCounter = 0;

    int routeIndex = 0;
    int routeCount = 0;

    private void Awake()
    {
        //SpawnUfo();
        StartCoroutine(LoopSpawning());

    }

    void SpawnUfo(SpawnInfo info)
    {
        var rot = Quaternion.identity;
        var obj = Instantiate(info.GameObject, transform.position, rot);

        //var followPath = obj.GetComponent<FollowPathInputProvider>();
        //followPath.targetPath = routeInfo[routeIndex].path;
        //routeCount++;
        //if (routeCount >= routeInfo[routeIndex].ufoCount)
        //{
        //    routeIndex++;
        //}
    }

    IEnumerator LoopSpawning()
    {
        if (spawnInfo.Count == 0)
            yield break;
        while (true)
        {
            if (!isActive)
            {
                yield return null;
                continue;
            }

            SpawnUfo(spawnInfo[spawnInfoListIndex]);
            infoGameObjectCounter++;

            if (infoGameObjectCounter >= spawnInfo[spawnInfoListIndex].Count)
            {
                spawnInfoListIndex++;
                infoGameObjectCounter = 0;
            }

            if (spawnInfoListIndex == spawnInfo.Count)
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
