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
            int w0 = 0;//右
            int w1 = 1;//左
            int h0 = 0;//下
            int h1 = 1;//上

            var height = cellTypes.GetLength(0);
            var width = cellTypes.GetLength(1);

            var position = (new Vector3(0.5f, 0, 0.5f) + roadOffset) * CityGenerator.DefaultDeltaDistance;
            //var obj = Instantiate(road2x2GameObject, position, Quaternion.identity);


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

}


public enum RoadGenerationMode
{
    Cell1x1,
    Cell2x2,
}
