using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaWave : MonoBehaviour
{
    //波の振幅
    public float amplitude = 0.1f;

    //波の周期
    public float wavePeriod = 3;

    public float v = 2;

    MeshFilter meshFilter;
    Vector3[] baseVertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        baseVertices = meshFilter.mesh.vertices;
        foreach (var item in baseVertices)
        {
            print(item);
        }
        meshFilter.mesh.MarkDynamic();
    }

    // Update is called once per frame
    void Update()
    {
        Wave();
    }



    void Wave()
    {

        var mesh = meshFilter.mesh;
        var vertices = mesh.vertices;
        //var normals = mesh.normals;

        for (int i = 0; i < vertices.Length; i++)
        {
            var dest = baseVertices[i];
            dest.y += amplitude / 2 * Mathf.Sin(2 * Mathf.PI / wavePeriod * (Time.time - baseVertices[i].x / v));
            dest.y += amplitude / 2 * Mathf.Sin(2 * Mathf.PI / wavePeriod * (Time.time - baseVertices[i].z / v));

            vertices[i] = dest;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.UploadMeshData(false);
    }
}
