using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public List<GameObject> cloudPrefabs;

    public float forwardLength;
    public float rightLength;
    public float leftLength;

    [Space]
    public int initialCloud;

    public float minIntervalSec = 5;
    public float maxIntervalSec = 15;
    public float cloudSpeed;

    public float minSize;
    public float maxSize;

    public float minAltitude = 450;
    public float maxAltitude = 600;

    private void Awake()
    {
        for (int i = 0; i < initialCloud; i++)
        {
            var pos = transform.TransformPoint(new Vector3(Random.Range(-leftLength, rightLength),
                Random.Range(minAltitude, maxAltitude), Random.Range(0, forwardLength)));
            GenerateCloud(pos);
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Generate());

    }

    //public List<CloudControlInfo> cloudInfo;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * forwardLength);
        Debug.DrawRay(transform.position, transform.right * rightLength);
        Debug.DrawRay(transform.position, -transform.right * leftLength);


        foreach (Transform item in transform)
        {
            var pos = transform.InverseTransformPoint(item.transform.position);
            if (pos.z > forwardLength)
            {
                Destroy(item.gameObject);
            }
            item.transform.Translate(transform.forward * cloudSpeed * Time.deltaTime, Space.World);
        }

        //foreach (var item in cloudInfo)
        //{
        //    var pos = transform.InverseTransformPoint(item.CloudGameObject.transform.position);
        //    if (pos.z > forwardLength)
        //    {
        //        cloudInfo.Remove()
        //        Destroy(item.CloudGameObject);
        //    }
        //    item.transform.Translate(transform.forward * cloudSpeed * Time.deltaTime, Space.World);
        //}
    }

    IEnumerator Generate()
    {
        while (true)
        {//x横　z縦
            var pos = transform.TransformPoint(new Vector3(Random.Range(-leftLength, rightLength),
               Random.Range(minAltitude, maxAltitude), 0));
            GenerateCloud(pos);
            yield return new WaitForSeconds(Random.Range(minIntervalSec, maxIntervalSec));
        }
    }

    GameObject GenerateCloud(Vector3 localPosition)
    {
        var obj = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Count)],
            localPosition, Random.rotation);
        var scale = obj.transform.localScale;
        scale = Vector3.one * Random.Range(minSize, maxSize);
        obj.transform.localScale = scale;
        obj.transform.SetParent(transform);

        return obj;
    }

    [System.Serializable]
    public class CloudControlInfo
    {
        public GameObject CloudGameObject;
        public float Speed;
        public Vector3 Rotation;
    }
}
