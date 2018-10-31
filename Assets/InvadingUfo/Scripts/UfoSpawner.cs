using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UfoSpawner : MonoBehaviour
{
    public bool isActive = true;

    public Wave wave;

    [Header("生成オブジェクト")]
    public List<SpawnInfo> spawnInfo;


    [Header("生成間の時間")]
    public float minInterval = 1;
    public float maxInterval = 2;

    int spawnInfoListIndex = 0;
    int infoGameObjectCounter = 0;

    public HouseManager houseManager;
    public UfoManager ufoManager;

    //event
    public event System.Action OnAllUfosSpawned;
    //bool hasAllUfosSpawned = false;
    public event System.Action OnWaveFinished;

    GameObject ufoHolder;

    Coroutine[] spawningCoroutines;
    int order = 0;
    int maxOrder;
    Queue<WavePart>[] spawnQueue;

    private void Awake()
    {
        var spawnerTransforms = wave.wavePart.Select(x => x.spawner).Distinct().ToArray();
        int spawnerCount = spawnerTransforms.Length;

        spawningCoroutines = new Coroutine[spawnerCount];
        spawnQueue = new Queue<WavePart>[spawnerCount];//それぞれのスポナーの生成
        for (int i = 0; i < spawnQueue.Length; i++)
        {
            spawnQueue[i] = new Queue<WavePart>();
        }

        print(spawnQueue.Length);
        for (int i = 0; i < spawnerCount; i++)
        {
            foreach (var item in wave.wavePart)
            {
                if (item == null)
                    print("null");

                if (item.spawner == spawnerTransforms[i])
                {
                    spawnQueue[i].Enqueue(item);
                }
            }
        }

        foreach (var item in spawnQueue)
        {
            maxOrder = Mathf.Max(item.Count, maxOrder);
        }
    }




    private void Start()
    {
        ufoHolder = ufoManager.ufoHolder;
        StartToSpawn();
    }

    private void Update()
    {
        if (order >= maxOrder)
            return;
        bool allNull = true;
        foreach (var item in spawningCoroutines)
        {
            if (item == null)
            {
                allNull = false;
            }
        }

        if (allNull)
        {
            order++;
            StartWaveParts(order);

            if (order == maxOrder)
            {
                OnAllUfosSpawned?.Invoke();
            }
        }
    }

    void StartToSpawn()
    {
        StartWaveParts(0);
    }

    void StartWaveParts(int order)
    {
        for (int i = 0; i < spawnQueue.Length; i++)
        {
            if (spawnQueue[i].Count == 0)
                continue;

            var wp = spawnQueue[i].Peek();
            if (wp.order == order)
            {
                spawningCoroutines[i] = StartCoroutine(SpawningUfoLoop(i, spawnQueue[i].Dequeue()));
            }
        }
    }

    void SpawnUfo(SpawnInfo info)
    {
        var rot = Quaternion.identity;
        var obj = Instantiate(info.GameObject, transform.position, rot);
        obj.transform.SetParent(ufoHolder.transform);

        //
        var ai = obj.GetComponent<AIMagnetUfoInputProvider>();
        if (ai != null)
        {
            ai.houseManager = houseManager;
            if (ufoManager != null)
            {
                ufoManager.Add(obj.GetComponent<BaseUfoCore>());
            }
        }
    }

    void SpawnUfo(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var obj = Instantiate(prefab, ufoHolder.transform);

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        //magnet 
        var ai = obj.GetComponent<AIMagnetUfoInputProvider>();
        if (ai != null)
        {
            ai.houseManager = houseManager;
        }

        if (ufoManager != null)
        {
            ufoManager.Add(obj.GetComponent<BaseUfoCore>());
        }

    }

    IEnumerator SpawningUfoLoop(int index, WavePart wavePart)
    {
        yield return new WaitForSeconds(wavePart.startDelaySec);

        if (wavePart.count == 0)
            yield break;
        int counter = 0;

        while (true)
        {
            if (!isActive)
            {
                yield return null;
                continue;
            }

            SpawnUfo(wavePart.ufo, wavePart.spawner.position, Quaternion.identity);
            counter++;

            if (counter >= wavePart.count)
            {
                break;
            }
            yield return new WaitForSeconds(Random.Range(wavePart.minIntervalSec, wavePart.maxIntervalSec));
        }

        spawningCoroutines[index] = null;
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
                OnWaveFinished?.Invoke();
            }

            if (spawnInfoListIndex == spawnInfo.Count)
            {
                break;
            }
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
        OnAllUfosSpawned?.Invoke();
        //print("invoke");

    }

}

[System.Serializable]
public class UfoRouteInfo
{
    public PathController path;
    public int ufoCount;
}

//[System.Serializable]
//public class Mission
//{
//    public List<Wave> WaveList;
//}


[System.Serializable]
public class Wave
{
    public List<WavePart> wavePart;
}

[System.Serializable]
public class WavePart
{
    //実行される順番
    public int order;
    [Space]
    public GameObject ufo;
    public int count;
    public Transform spawner;
    [Space]
    public float minIntervalSec;
    public float maxIntervalSec;
    [Space]
    public float startDelaySec;
}