using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public List<GameObject> cloudPrefabs;

    public float forwardLength;
    public float rightLength;
    public float leftLength;

    public float intervalSec = 10;
    public float cloudSpeed;

    public float minSize;
    public float maxSize;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Generate());

    }

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
    }

    IEnumerator Generate()
    {
        while (true)
        {
            GenerateCloud();
            yield return new WaitForSeconds(intervalSec);
        }
    }

    void GenerateCloud()
    {
        var pos = transform.TransformPoint(new Vector3(Random.Range(-leftLength, rightLength), 0, 0));//x横　z縦

        var obj = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Count)],
            pos, Random.rotation);
        var scale = obj.transform.localScale;
        scale = Vector3.one * Random.Range(minSize, maxSize);
        obj.transform.localScale = scale;
        obj.transform.SetParent(transform);
    }
}
