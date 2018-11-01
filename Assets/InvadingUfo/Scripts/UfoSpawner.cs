using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UfoSpawner : MonoBehaviour
{
    public bool isActive = true;

    public Wave wave;


    public Mission mission;

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

    bool hasWaveFinished = false;



    IEnumerator StartMission(Mission mission)
    {
        for (int i = 0; i < mission.waves.Count; i++)
        {

            var coroutine = StartCoroutine(StartWave(mission.waves[i]));
            yield return coroutine;
            //print("wave ends" + i);
        }
        //print("all waves finished");

        OnAllUfosSpawned?.Invoke();
    }

    IEnumerator StartWave(Wave wave)
    {
        yield return new WaitForSeconds(wave.startDelay);
        //print(wave.startDelay);

        Initialize(wave);
        //print("initialized");

        StartWaveParts(0);

        //print("start wave parts ends");
        //print(hasWaveFinished);

        while (!hasWaveFinished)
        {
            yield return null;
            continue;
        }

        yield return new WaitForSeconds(wave.endDelay);

    }

    //それぞれの位置のスポナーにufo生成を振り分け
    void Initialize(Wave wave)
    {
        order = 0;
        var spawnerTransforms = wave.wavePart.Select(x => x.spawner).Distinct().ToArray();
        int spawnerCount = spawnerTransforms.Length;

        spawningCoroutines = new Coroutine[spawnerCount];
        spawnQueue = new Queue<WavePart>[spawnerCount];//それぞれのスポナーの生成
        for (int i = 0; i < spawnQueue.Length; i++)
        {
            spawnQueue[i] = new Queue<WavePart>();
        }

        for (int i = 0; i < spawnerCount; i++)
        {
            foreach (var item in wave.wavePart)
            {
                if (item == null)
                {
                    print("null");
                    return;
                }

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

        hasWaveFinished = false;
    }


    private void Start()
    {
        ufoHolder = ufoManager.ufoHolder;
        StartCoroutine(StartMission(mission));
    }

    private void Update()
    {
        if (hasWaveFinished)
        {
            return;
        }

        if (order >= maxOrder)
        {
            return;
        }
        bool allNull = true;

        for (int i = 0; i < spawningCoroutines.Length; i++)
        {
            if (spawningCoroutines[i] != null)
            {
                allNull = false;
            }
        }


        if (allNull)
        {

            order++;
            if (order == maxOrder)
            {
                //OnAllUfosSpawned?.Invoke();
                //print("wave finished");

                hasWaveFinished = true;
                OnWaveFinished?.Invoke();
            }


            StartWaveParts(order);

        }
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
}

[System.Serializable]
public class WaveMission
{
    public List<Wave> waves;
}

[System.Serializable]
public class Wave
{
    public float startDelay;
    public float endDelay;

    //上から順に実行される。
    public List<WavePart> wavePart;
}

[System.Serializable]
public class WavePart
{
    //実行される順番
    //TODO:よくわからん
    //0から順に実行されていく(スポナーごと)
    //0から順番に番号を付けていく必要がある
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