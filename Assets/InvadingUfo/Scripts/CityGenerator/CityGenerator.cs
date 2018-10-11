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
    //public GameObject cellGameObject;

    public Vector3 gridOffset;

    const int defaultDeltaDistance = 10;

    [Header("道路")]
    public string roadParentName = "Roads";
    public GameObject roadParentGameObject;

    //あとで複数の道路に変える
    public GameObject roadGameObject;

    public int deltaWidth = 4;
    public int deltaHeight = 4;

    public Vector3 roadOffset;
    List<GameObject> roads = new List<GameObject>();


    CityCellType[,] cellTypes;

    private void Awake()
    {
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
                plane.transform.localPosition = new Vector3(w * defaultDeltaDistance, 0, h * defaultDeltaDistance) + gridOffset * defaultDeltaDistance;
                grid[h, w] = plane;
                cellTypes[h, w] = CityCellType.None;
            }
        }
    }

    //road generator

    void MakeRoads()
    {
        if (roadParentGameObject == null)
        {
            roadParentGameObject = new GameObject(roadParentName);
            roadParentGameObject.transform.SetParent(rootGameObject.transform);
        }

        //Transform pt = roadParentGameObject.transform;

        var rh = Random.Range(0, grid.GetLength(0));
        var rw = Random.Range(0, grid.GetLength(1));

        var mat = grid[rh, rw].GetComponent<MeshRenderer>().material;
        mat.color = Color.red;
        grid[rh, rw].GetComponent<MeshRenderer>().material = mat;

        var height = grid.GetLength(0);
        var width = grid.GetLength(1);
        CreateRoadWithCross(rw, rh);


        //z方向移動 height
        for (int h = 0; h < height; h++)
        {
            var diff = h - rh;
            print(diff);
            if ((Mathf.Abs(diff) % (deltaHeight + 1)) == 0)//差がdeltaHeight+1の倍数
            {

                CreateRoadWithCross(rw, h);
            }
        }


        //x方向移動　width
        for (int w = 0; w < width; w++)
        {
            var diff = w - rw;
            if ((Mathf.Abs(diff) % (deltaWidth + 1)) == 0)//差がdeltaHeight+1の倍数
            {

                CreateRoadWithCross(w, rh);
            }
        }



    }

    void CreateRoadWithCross(int centerWidth, int centerHeight)
    {
        var height = grid.GetLength(0);
        var width = grid.GetLength(1);

        //クロス方向に作る
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (h == centerHeight || w == centerWidth)
                {
                    if (cellTypes[h, w] == CityCellType.Road)
                        continue;
                    var position = (new Vector3(w, 0, h) + gridOffset) * defaultDeltaDistance;
                    var obj = Instantiate(roadGameObject, position, Quaternion.identity);

                    obj.transform.SetParent(roadParentGameObject.transform);
                    cellTypes[h, w] = CityCellType.Road;
                }
            }
        }
    }

    void Generate()
    {

        //ルートオブジェクト作成
        if (rootGameObject == null)
        {
            rootGameObject = new GameObject(rootName);
        }


        MakeGrid();

        MakeRoads();
    }

    enum RoadDirection
    {
        X,
        Z,
    }
}
