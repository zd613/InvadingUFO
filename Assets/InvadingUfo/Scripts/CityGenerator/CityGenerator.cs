using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("root")]
    public string rootName = "City";
    public GameObject rootGameObject;

    [Header("グリッド")]
    public string gridName = "Grid";
    public GameObject gridGameObject;
    public int cellWidth = 1;
    public int cellHeight = 1;
    GameObject[,] grid;

    public int gridWidthCount = 10;
    public int gridHeightCount = 10;

    public Vector3 gridOffset;

    public static readonly int DefaultDeltaDistance = 10;



    CityCellType[,] cellTypes;

    RoadGenerator roadGenerator;

    private void Awake()
    {
    }

    private void Start()
    {
        roadGenerator = GetComponent<RoadGenerator>();
        roadGenerator.Initialize(rootGameObject, cellTypes);
        Generate();
    }

    void MakeGrid()
    {
        if (gridGameObject == null)
        {
            gridGameObject = new GameObject(gridName);
            gridGameObject.transform.SetParent(rootGameObject.transform);
        }

        grid = new GameObject[gridHeightCount, gridWidthCount];
        cellTypes = new CityCellType[gridHeightCount, gridWidthCount];

        for (int h = 0; h < grid.GetLength(0); h++)
        {
            for (int w = 0; w < grid.GetLength(1); w++)
            {
                var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.SetParent(gridGameObject.transform);
                plane.transform.localPosition = new Vector3(w * DefaultDeltaDistance, 0, h * DefaultDeltaDistance) + gridOffset * DefaultDeltaDistance;
                grid[h, w] = plane;
                cellTypes[h, w] = CityCellType.None;
            }
        }
    }

    //road generator


    void Generate()
    {

        //ルートオブジェクト作成
        if (rootGameObject == null)
        {
            rootGameObject = new GameObject(rootName);
        }


        MakeGrid();

        roadGenerator.Initialize(rootGameObject, cellTypes);
        roadGenerator.MakeRoads();
    }

}
