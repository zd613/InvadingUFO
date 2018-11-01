using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UfoSpawner : MonoBehaviour
{
    public bool isActive = true;

    public Wave wave;

    public HouseManager houseManager;
    public UfoManager ufoManager;

    //event
    public event System.Action OnAllUfosSpawned;
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

        //print(spawnQueue.Length);
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

        for (int i = 0; i < spawningCoroutines.Length; i++)
        {
            //print(i + "," + (spawningCoroutines[i] == null ? "null" : "not null"));

            if (spawningCoroutines[i] != null)
            {
                allNull = false;
            }
        }

        //print(allNull ? "all null" : "not all null");


        if (allNull)
        {
            print("all null");

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
        print("wave" + order);
        for (int i = 0; i < spawnQueue.Length; i++)
        {
            if (spawnQueue[i].Count == 0)
                continue;

            var wp = spawnQueue[i].Peek();
            if (wp.order == order)
            {
                print(order + "," + spawningCoroutines[i] == null ? "null" : "no");

                spawningCoroutines[i] = StartCoroutine(SpawningUfoLoop(i, spawnQueue[i].Dequeue()));
                print(spawningCoroutines[i] == null ? "nullll" : "nnnnn");
                print(i);


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
        print("s");

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
            print("spawn");

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
}


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