using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [Header("設定")]
    public GameObject rootGameObject;
    public CityCellType[,] cellTypes;//grid のcellが何かの情報
    //public Vector3 rootOffset;

    [Header("道路")]
    public string roadParentName = "Roads";
    public GameObject roadParentGameObject;
    public RoadGenerationMode roadGenerationMode;
    public Vector3 roadOffset;//道全体を移動させる
    //あとで複数の道路に変える
    public GameObject roadGameObject;
    public GameObject road2x2Crossroad;
    public GameObject road2x2Straight;
    public GameObject road2x2Edge;

    public int deltaWidth = 4;
    public int deltaHeight = 4;

    //public Vector3 roadOffset;
    List<GameObject> roads = new List<GameObject>();

    public void SetRootAndGridInfo(GameObject rootGameObject, CityCellType[,] cellTypes)
    {
        this.rootGameObject = rootGameObject;
        this.cellTypes = cellTypes;
    }

    public void MakeRoads()
    {
        if (roadParentGameObject == null)
        {
            roadParentGameObject = new GameObject(roadParentName);
            roadParentGameObject.transform.SetParent(rootGameObject.transform);
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
            //2x2 の　セルの表現
            int ur = 0;//右上
            int lr = 0;//右下

            int ul = 1;//左上
            int ll = 1;//左下

            var height = cellTypes.GetLength(0);
            var width = cellTypes.GetLength(1);

            float x = (float)(ll + lr) / 2;
            float z = (float)(ul + ll) / 2;
            var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;
            var obj = Instantiate(road2x2Crossroad, position, Quaternion.identity);
            obj.transform.SetParent(roadParentGameObject.transform);

            CreateRoad2x2(0, 0, RoadDir.X);
            //CreateRoad2x2(0, 0, RoadDir.Z);
        }


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

        int rX = lowerLeftCellX + 1;//右
        int lX = lowerLeftCellX;//左
        int lZ = lowerLeftCellZ;//下
        int uZ = lowerLeftCellZ + 1;//上
        //横
        if (dir == RoadDir.Z)
        {
            //プラス
            while (true)
            {
                if (cellTypes[lZ, lX] == CityCellType.Road)
                    continue;

                float x = (float)(rX + lX) / 2;
                float z = (float)(lZ + uZ) / 2;
                var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;


                cellTypes[lZ, lX] = CityCellType.Road;
                cellTypes[lZ, rX] = CityCellType.Road;
                cellTypes[uZ, lX] = CityCellType.Road;
                cellTypes[uZ, lX] = CityCellType.Road;

                var rot = Quaternion.identity;
                var obj = Instantiate(road2x2Crossroad, position, transform.rotation);
                obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                obj.transform.SetParent(roadParentGameObject.transform);

                //lX = (lX + 2 + deltaWidth);
                lZ += 2;
                uZ = lZ + 1;
                if (lZ >= cellTypes.GetLength(0))
                    break;
            }

            rX = lowerLeftCellX + 1;//右
            lX = lowerLeftCellX;//左
            lZ = lowerLeftCellZ;//下
            uZ = lowerLeftCellZ + 1;//上

            while (lZ >= 0)
            {
                if (cellTypes[lZ, lX] == CityCellType.Road)
                    continue;

                float x = (float)(rX + lX) / 2;
                float z = (float)(lZ + uZ) / 2;
                var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;

                cellTypes[lZ, lX] = CityCellType.Road;
                cellTypes[lZ, rX] = CityCellType.Road;
                cellTypes[uZ, lX] = CityCellType.Road;
                cellTypes[uZ, lX] = CityCellType.Road;

                var rot = Quaternion.identity;
                var obj = Instantiate(road2x2Crossroad, position, transform.rotation);
                obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                obj.transform.SetParent(roadParentGameObject.transform);

                //lX = (lX + 2 + deltaWidth);
                lZ += 2;
                uZ = lZ + 1;

            }

        }
        //縦
        else
        {
            //プラス
            while (true)
            {
                if (cellTypes[lZ, lX] == CityCellType.Road)
                    continue;
                float x = (float)(rX + lX) / 2;
                float z = (float)(lZ + uZ) / 2;
                var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;


                cellTypes[lZ, lX] = CityCellType.Road;
                cellTypes[lZ, rX] = CityCellType.Road;
                cellTypes[uZ, lX] = CityCellType.Road;
                cellTypes[uZ, lX] = CityCellType.Road;

                var rot = Quaternion.identity;
                var obj = Instantiate(road2x2Crossroad, position, transform.rotation);
                //obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                obj.transform.SetParent(roadParentGameObject.transform);

                //lX = (lX + 2 + deltaWidth);
                lX += 2;
                rX = lX + 1;

                if (rX >= cellTypes.GetLength(1))
                    break;
            }

            rX = lowerLeftCellX + 1;//右
            lX = lowerLeftCellX;//左
            lZ = lowerLeftCellZ;//下
            uZ = lowerLeftCellZ + 1;//上

            //マイナス
            while (lX <= 0)
            {
                if (cellTypes[lZ, lX] == CityCellType.Road)
                    continue;

                float x = (float)(rX + lX) / 2;
                float z = (float)(lZ + uZ) / 2;
                var position = (new Vector3(x, 0, z) + roadOffset) * CityGenerator.DefaultDeltaDistance;

                cellTypes[lZ, lX] = CityCellType.Road;
                cellTypes[lZ, rX] = CityCellType.Road;
                cellTypes[uZ, lX] = CityCellType.Road;
                cellTypes[uZ, lX] = CityCellType.Road;

                var rot = Quaternion.identity;
                var obj = Instantiate(road2x2Crossroad, position, transform.rotation);
                //obj.transform.Rotate(new Vector3(0, 90, 0), Space.World);
                obj.transform.SetParent(roadParentGameObject.transform);

                //lX = (lX + 2 + deltaWidth);
                lX -= 2;
                rX = lX + 1;


            }
        }
    }

    enum RoadDir
    {
        X,
        Z,
    }
}


public enum RoadGenerationMode
{
    Cell1x1,
    Cell2x2,
}
