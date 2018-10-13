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

    [Header("sidewalk")]
    public string sidewalkParentName = "Sidewalk";
    public GameObject sidewalkParentGameObject;
    public GameObject cube;
    public Vector3 sidewalkOffset;
    public Material sidewalkMaterial;
    public float yScale = 1;



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

        CreateSidewalk();



    }

    //四角形前提
    void CreateSidewalk()
    {
        if (sidewalkParentGameObject == null)
        {
            sidewalkParentGameObject = new GameObject(sidewalkParentName);
            if (rootGameObject != null)
            {
                sidewalkParentGameObject.transform.SetParent(rootGameObject.transform);

            }
        }

        //左下から
        for (int z = 0; z < cellTypes.GetLength(0); z++)
        {
            for (int x = 0; x < cellTypes.GetLength(1); x++)
            {
                //左下
                if (cellTypes[z, x] == CityCellType.None)
                {
                    //get width
                    int width = 0;
                    while (true)
                    {
                        width++;
                        if ((x + width) >= cellTypes.GetLength(1) || cellTypes[z, x + width] != CityCellType.None)
                        {
                            break;
                        }
                    }

                    //get height
                    int height = 0;
                    while (true)
                    {
                        height++;
                        if ((z + height) >= cellTypes.GetLength(0) || cellTypes[z + height, x] != CityCellType.None)
                        {
                            break;
                        }
                    }

                    //print("w,h" + width + "," + height);
                    //print("x,z" + x + "," + z);
                    var o = InstantiateSidewalk(width, height);
                    //cell の左下と右上のやつから真ん中をけいさんして　
                    var px = (float)(x + (x + width - 1)) / 2;
                    var pz = (float)(z + (z + height - 1)) / 2;
                    o.transform.position = (new Vector3(px, 0, pz) + sidewalkOffset) * DefaultDeltaDistance;
                    o.transform.SetParent(sidewalkParentGameObject.transform);

                    if (sidewalkMaterial != null)
                    {
                        o.GetComponent<MeshRenderer>().sharedMaterial = sidewalkMaterial;
                    }
                    //return;
                }
            }
        }



    }

    GameObject InstantiateSidewalk(int width, int height)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.localScale = obj.transform.localScale * DefaultDeltaDistance;
        var ls = obj.transform.localScale;
        ls.x *= width;
        ls.y *= yScale;
        ls.z *= height;
        obj.transform.localScale = ls;

        return obj;
    }

}
