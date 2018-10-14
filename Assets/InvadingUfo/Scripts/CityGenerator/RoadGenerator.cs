using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [Header("設定")]
    public GameObject rootGameObject;
    CityCellType[,] cellTypes;//grid のcellが何かの情報
    RoadType[,] roadtypes;

    //public Vector3 rootOffset;

    [Header("道路")]
    public string roadParentName = "Roads";
    public GameObject roadParentGameObject;
    public RoadGenerationMode roadGenerationMode;
    public Vector3 roadOffset;//道全体を移動させる
    //あとで複数の道路に変える
    public GameObject roadGameObject;
    public GameObject road2x2Staright;
    public GameObject road2x2Cross;
    public GameObject road2x2Edge;

    public int deltaWidth = 4;
    public int deltaHeight = 4;

    [Header("Traffic Light")]
    public string trafficLightParentName = "TrafficLight";
    public GameObject trafficLightParentGameObject;
    public GameObject trafficLightPrefab;
    public Vector3 trafficLightOffset;

    //public Vector3 roadOffset;
    List<GameObject> roads = new List<GameObject>();


    public void SetRootAndGridInfo(GameObject rootGameObject, CityCellType[,] cellTypes)
    {
        this.rootGameObject = rootGameObject;
        this.cellTypes = cellTypes;
    }

    //2x2 は一個飛ばしですれば楽そう

    public void MakeRoads()
    {
        if (roadParentGameObject == null)
        {
            roadParentGameObject = new GameObject(roadParentName);
            roadParentGameObject.transform.SetParent(rootGameObject.transform);
        }

        if (roadtypes == null)
        {
            roadtypes = new RoadType[cellTypes.GetLength(0), cellTypes.GetLength(1)];
        }

        if (roadGenerationMode == RoadGenerationMode.Cell1x1)
        {
            //ベース位置
            var rh = 0;
            var rw = 0;


            var height = cellTypes.GetLength(0);
            var width = cellTypes.GetLength(1);


            //x方向移動　width
            for (int w = 0; w < width; w++)
            {
                var diff = w - rw;
                if ((Mathf.Abs(diff) % (deltaWidth + 1)) == 0)//差がdeltaHeight+1の倍数
                {

                    CreateRoadWithCross(w, rh);
                }
            }
            //z方向移動 height
            for (int h = 0; h < height; h++)
            {
                var diff = h - rh;
                if ((Mathf.Abs(diff) % (deltaHeight + 1)) == 0)//差がdeltaHeight+1の倍数
                {

                    CreateRoadWithCross(rw, h);
                }
            }

        }
        else if (roadGenerationMode == RoadGenerationMode.Cell2x2)
        {
            if (deltaWidth % 2 != 0 || deltaHeight % 2 != 0)
            {
                throw new System.Exception("deltaWidth と deltaHeightは2の倍数");

            }
            //2x2 の　セルの表現
            int ur = 0;//右上
            int lr = 0;//右下

            int ul = 1;//左上
            int ll = 1;//左下

            var height = cellTypes.GetLength(0);
            var width = cellTypes.GetLength(1);

            //x,zの順番買えたら交差点などがおかしくなるかも
            //X移動してZ方向に道路
            for (int x = 0; x < width; x += (2 + deltaWidth))
            {
                CreateRoad2x2(x, 0, RoadDir.Z);
            }
            //Z
            for (int z = 0; z < height; z += (2 + deltaHeight))
            {
                CreateRoad2x2(0, z, RoadDir.X);
            }


            ////roadtype を記録
            //for (int z = 0; z < cellTypes.GetLength(0); z++)
            //{
            //    for (int x = 0; x < cellTypes.GetLength(1); x++)
            //    {
            //        if (cellTypes[z, x] == CityCellType.Road)
            //        {
            //            roadtypes[z, x] = RoadType.Straight;
            //        }
            //    }
            //}


        }

    }

    public void MakeTrafficLights()
    {
        if (trafficLightParentGameObject == null)
        {
            trafficLightParentGameObject = new GameObject(trafficLightParentName);
            if (rootGameObject != null)
            {
                trafficLightParentGameObject.transform.SetParent(rootGameObject.transform);

            }
        }

        InstantiateTrafficLight(0, 0);

        for (int z = 0; z < cellTypes.GetLength(0); z++)
        {
            for (int x = 0; x < cellTypes.GetLength(1); x++)
            {

                //if(roadtype
                if (roadtypes[z, x] == RoadType.Crossroad)
                {
                    //斜めを調べてnone なら　作る


                    for (int az = 0; az < naname.Length; az++)
                    {
                        for (int ax = 0; ax < naname.Length; ax++)
                        {
                            int targetX = x + naname[ax];
                            int targetZ = z + naname[az];
                            //範囲内かどうか
                            if (targetZ >= 0 && targetX >= 0 && targetZ < roadtypes.GetLength(0) && targetX < roadtypes.GetLength(1))
                            {
                                if (roadtypes[targetZ, targetX] == RoadType.None)
                                {
                                    var obj = InstantiateTrafficLight(targetX, targetZ);

                                    int nanamex = naname[ax];
                                    int nanamez = naname[az];
                                    if (nanamex == -1 && nanamez == 1)
                                    {
                                        //正しい
                                    }
                                    else if (nanamex == 1 && nanamez == 1)
                                    {
                                        obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                                    }

                                    else if (nanamex == 1 && nanamez == -1)
                                    {

                                        obj.transform.Rotate(new Vector3(0, 180, 0), Space.World);
                                    }

                                    else if (nanamex == -1 && nanamez == -1)
                                    {
                                        obj.transform.Rotate(new Vector3(0, -90, 0), Space.World);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
    int[] naname = new int[] { -1, 1 };


    GameObject InstantiateTrafficLight(int x, int z)
    {
        var obj = Instantiate(trafficLightPrefab);
        obj.transform.position = (new Vector3(x, 0, z) + trafficLightOffset) * CityGenerator.DefaultDeltaDistance;
        obj.transform.SetParent(trafficLightParentGameObject.transform);

        return obj;

    }

    void CreateRoadWithCross(int centerWidth, int centerHeight)
    {
        var height = cellTypes.GetLength(0);
        var width = cellTypes.GetLength(1);

        //クロス方向に作る
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (h == centerHeight || w == centerWidth)
                {
                    if (cellTypes[h, w] == CityCellType.Road)
                        continue;
                    var position = (new Vector3(w, 0, h) + roadOffset) * CityGenerator.DefaultDeltaDistance;
                    var obj = Instantiate(roadGameObject, position, Quaternion.identity);

                    obj.transform.SetParent(roadParentGameObject.transform);
                    cellTypes[h, w] = CityCellType.Road;
                }
            }
        }
    }

    void CreateRoad2x2(int lowerLeftCellX, int lowerLeftCellZ, RoadDir dir)
    {
        //2x2 の　セルの表現
        //int ur;//右上

        int rx = lowerLeftCellX + 1;//右
        int lx = lowerLeftCellX;//左
        int lz = lowerLeftCellZ;//下
        int uz = lowerLeftCellZ + 1;//上
                                    //横
        if (dir == RoadDir.Z)
        {
            //プラス
            while (true)
            {
                if (cellTypes[lz, lx] == CityCellType.Road)
                {
                    lz += 2;
                    uz = lz + 1;
                    continue;

                }

                float x = (float)(rx + lx) / 2;
                float z = (float)(lz + uz) / 2;
                var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;


                SetCityCellTypes(rx, lx, lz, uz);

                var rot = Quaternion.identity;

                //
                GameObject obj = null;
                if (lz % (deltaHeight + 2) == 0)
                {
                    obj = Instantiate(road2x2Cross, position, transform.rotation);
                    SetRoadTypes2x2(lx, lz, RoadType.Crossroad);

                }
                else
                {
                    obj = Instantiate(road2x2Staright, position, transform.rotation);
                    SetRoadTypes2x2(lx, lz, RoadType.Straight);
                }
                //obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                obj.transform.SetParent(roadParentGameObject.transform);

                //lX = (lX + 2 + deltaWidth);
                lz += 2;
                uz = lz + 1;
                if (uz >= cellTypes.GetLength(0))
                    break;
            }

            rx = lowerLeftCellX + 1;//右
            lx = lowerLeftCellX;//左
            lz = lowerLeftCellZ;//下
            uz = lowerLeftCellZ + 1;//上

            //マイナス
            while (lz >= 0)
            {
                if (cellTypes[lz, lx] == CityCellType.Road)
                {
                    lz -= 2;
                    uz = lz + 1;
                    continue;

                }
                float x = (float)(rx + lx) / 2;
                float z = (float)(lz + uz) / 2;
                var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;

                SetCityCellTypes(rx, lx, lz, uz);


                var rot = Quaternion.identity;
                GameObject obj = null;
                if (lz % (deltaHeight + 2) == 0)
                {
                    obj = Instantiate(road2x2Cross, position, transform.rotation);
                    SetRoadTypes2x2(lx, lz, RoadType.Crossroad);

                }
                else
                {
                    obj = Instantiate(road2x2Staright, position, transform.rotation);
                    SetRoadTypes2x2(lx, lz, RoadType.Straight);
                }
                //obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                obj.transform.SetParent(roadParentGameObject.transform);

                //lX = (lX + 2 + deltaWidth);
                lz -= 2;
                uz = lz + 1;

            }

        }
        //縦
        else
        {
            //プラス
            while (true)
            {
                if (cellTypes[lz, lx] == CityCellType.Road)
                {
                    lx += 2;
                    rx = lx + 1;
                    continue;

                }
                float x = (float)(rx + lx) / 2;
                float z = (float)(lz + uz) / 2;
                var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;

                SetCityCellTypes(rx, lx, lz, uz);


                var rot = Quaternion.identity;
                var obj = Instantiate(road2x2Staright, position, transform.rotation);
                SetRoadTypes2x2(lx, lz, RoadType.Straight);

                obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                obj.transform.SetParent(roadParentGameObject.transform);

                //lX = (lX + 2 + deltaWidth);
                lx += 2;
                rx = lx + 1;

                if (rx >= cellTypes.GetLength(1))
                    break;
            }

            rx = lowerLeftCellX + 1;//右
            lx = lowerLeftCellX;//左
            lz = lowerLeftCellZ;//下
            uz = lowerLeftCellZ + 1;//上

            //マイナス
            while (lx >= 0)
            {
                if (cellTypes[lz, lx] == CityCellType.Road)
                {

                    lx -= 2;
                    rx = lx + 1;

                    continue;

                }

                float x = (float)(rx + lx) / 2;
                float z = (float)(lz + uz) / 2;
                var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;

                SetCityCellTypes(rx, lx, lz, uz);

                var rot = Quaternion.identity;
                var obj = Instantiate(road2x2Staright, position, transform.rotation);
                SetRoadTypes2x2(lx, lz, RoadType.Straight);

                //obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                obj.transform.SetParent(roadParentGameObject.transform);

                //lX = (lX + 2 + deltaWidth);
                lx -= 2;
                rx = lx + 1;


            }
        }
    }

    void SetCityCellTypes(int rx, int lx, int lz, int uz, CityCellType cityCellType = CityCellType.Road)
    {
        cellTypes[lz, lx] = cityCellType;
        cellTypes[lz, rx] = cityCellType;
        cellTypes[uz, lx] = cityCellType;
        cellTypes[uz, rx] = cityCellType;
    }

    //４つのうち左下のcellが引数
    //そのcellの上、右、右上の４つのcellのタイプを設定
    void SetRoadTypes2x2(int lx, int lz, RoadType roadType)
    {
        roadtypes[lz, lx] = roadType;
        roadtypes[lz + 1, lx] = roadType;
        roadtypes[lz, lx + 1] = roadType;
        roadtypes[lz + 1, lx + 1] = roadType;

    }

    enum RoadDir
    {
        X,
        Z,
    }

    enum RoadType
    {
        None,
        Crossroad,
        Straight,
        NextToCrossroad,
    }
}


public enum RoadGenerationMode
{
    Cell1x1,
    Cell2x2,
}
