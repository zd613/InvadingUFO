using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("設定")]
    public GameObject rootGameObject;
    public CityCellType[,] cellTypes;//grid のcellが何かの情報

    [Header("")]
    public bool startOnAwake;
    public bool hideAfterCreation;

    [Header("グリッド")]
    public string gridName = "Grid";
    public GameObject gridGameObject;
    public int cellWidth = 1;
    public int cellHeight = 1;
    GameObject[,] grid;

    public bool useCutomGameObject = false;
    public List<GameObject> cellGameObjects;

    public int gridWidthCount = 10;
    public int gridHeightCount = 10;

    public Vector3 gridOffset;
    public Material gridMaterial;

    private void Awake()
    {
        if (startOnAwake)
        {
            MakeGrid();
        }
    }

    public void SetRootAndGridInfo(GameObject rootGameObject, CityCellType[,] cellTypes)
    {
        this.rootGameObject = rootGameObject;
        this.cellTypes = cellTypes;
    }

    public void MakeGrid()
    {
        if (gridGameObject == null)
        {
            gridGameObject = new GameObject(gridName);
            if (rootGameObject != null)
                gridGameObject.transform.SetParent(rootGameObject.transform);
        }

        grid = new GameObject[gridHeightCount, gridWidthCount];
        cellTypes = new CityCellType[gridHeightCount, gridWidthCount];

        for (int h = 0; h < grid.GetLength(0); h++)
        {
            for (int w = 0; w < grid.GetLength(1); w++)
            {
                GameObject plane;
                if (useCutomGameObject || cellGameObjects.Count != 0)
                {
                    plane = Instantiate(cellGameObjects[Random.Range(0, cellGameObjects.Count)]);
                }
                else
                {
                    plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

                }

                if (gridMaterial != null)
                {
                    plane.GetComponent<MeshRenderer>().sharedMaterial = gridMaterial;
                }

                plane.transform.SetParent(gridGameObject.transform);
                plane.transform.localPosition = (new Vector3(w * cellWidth, 0, h * cellHeight) + gridOffset) * CityGenerator.DefaultDeltaDistance;
                grid[h, w] = plane;
                cellTypes[h, w] = CityCellType.None;
            }
        }

        if (hideAfterCreation)
        {
            gridGameObject.SetActive(false);
        }
    }
}
