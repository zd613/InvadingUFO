using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("root")]
    public string rootName = "City";
    public GameObject rootGameObject;



    public static readonly int DefaultDeltaDistance = 10;



    CityCellType[,] cellTypes;

    GridGenerator gridGenerator;
    RoadGenerator roadGenerator;


    private void Awake()
    {
    }

    private void Start()
    {
        gridGenerator = GetComponent<GridGenerator>();

        roadGenerator = GetComponent<RoadGenerator>();
        Generate();
    }


    //road generator


    void Generate()
    {

        //ルートオブジェクト作成
        if (rootGameObject == null)
        {
            rootGameObject = new GameObject(rootName);
        }

        gridGenerator.SetRootAndGridInfo(rootGameObject, cellTypes);
        gridGenerator.MakeGrid();

        cellTypes = gridGenerator.cellTypes;

        roadGenerator.SetRootAndGridInfo(rootGameObject, cellTypes);
        roadGenerator.MakeRoads();
    }

}
