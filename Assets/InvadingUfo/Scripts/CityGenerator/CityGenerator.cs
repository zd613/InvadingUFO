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
    [Header("Generator")]
    public GridGenerator gridGenerator;
    public RoadGenerator roadGenerator;

    [Header("sidewalk")]
    public string sidewalkParentName = "Sidewalk";
    public GameObject sidewalkParentGameObject;
    public GameObject cube;
    public Vector3 sidewalkOffset;
    public Material sidewalkMaterial;
    public float sidewalkYScale = 1;


    [Header("Ground")]
    public string groundParentName = "Ground";
    public GameObject groundParentGameObject;
    public Vector3 groundOffset;
    public Material groundMaterial;
    public float groundYScale = 1;

    private void Awake()
    {
    }

    private void Start()
    {
        gridGenerator = GetComponent<GridGenerator>();

        roadGenerator = GetComponent<RoadGenerator>();
        //Generate();
    }


    //road generator


    public void Generate()
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

        CreateSidewalkAndGround();



    }

    //四角形前提
    void CreateSidewalkAndGround()
    {
        if (sidewalkParentGameObject == null)
        {
            sidewalkParentGameObject = new GameObject(sidewalkParentName);
            if (rootGameObject != null)
            {
                sidewalkParentGameObject.transform.SetParent(rootGameObject.transform);

            }
        }

        if (groundParentGameObject == null)
        {
            groundParentGameObject = new GameObject(groundParentName);
            if (rootGameObject != null)
            {
                groundParentGameObject.transform.SetParent(rootGameObject.transform);
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

                    var upperRightX = (x + width - 1);
                    var upperRightZ = (z + height - 1);
                    //cell の左下と右上のやつから真ん中をけいさんして　
                    var px = (float)(x + upperRightX) / 2;
                    var pz = (float)(z + upperRightZ) / 2;
                    o.transform.position = (new Vector3(px, 0, pz) + sidewalkOffset) * DefaultDeltaDistance;
                    o.transform.SetParent(sidewalkParentGameObject.transform);

                    if (sidewalkMaterial != null)
                    {
                        o.GetComponent<MeshRenderer>().sharedMaterial = sidewalkMaterial;
                    }


                    //celltypes の設定
                    for (int sz = z; sz <= upperRightZ; sz++)
                    {
                        for (int sx = x; sx <= upperRightX; sx++)
                        {
                            //四角形の周だけsidewalkに設定
                            if (sz == z || sz == upperRightZ || sx == x || sx == upperRightX)
                            {
                                cellTypes[sz, sx] = CityCellType.Sidewalk;
                            }
                        }
                    }


                    //groundの作成
                    if (width - 2 > 0 && height - 2 > 0)
                    {
                        var ground = InstantiateGround(width - 2, height - 2);


                        ground.transform.position = (new Vector3(px, 0, pz) + groundOffset) * DefaultDeltaDistance;
                        ground.transform.SetParent(groundParentGameObject.transform);
                        if (groundMaterial != null)
                        {
                            ground.GetComponent<MeshRenderer>().sharedMaterial = groundMaterial;
                        }
                    }


                    //celltypes の設定
                    for (int sz = z + 1; sz <= upperRightZ - 1; sz++)
                    {
                        for (int sx = x + 1; sx <= upperRightX - 1; sx++)
                        {
                            //四角形の周だけ

                            cellTypes[sz, sx] = CityCellType.Ground;

                        }
                    }
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
        ls.y *= sidewalkYScale;
        ls.z *= height;
        obj.transform.localScale = ls;

        return obj;
    }

    GameObject InstantiateGround(int width, int height)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.localScale = obj.transform.localScale * DefaultDeltaDistance;
        var ls = obj.transform.localScale;
        ls.x *= width;
        ls.y *= groundYScale;
        ls.z *= height;
        obj.transform.localScale = ls;

        return obj;
    }

}
