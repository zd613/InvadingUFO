using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneManager : MonoBehaviour
{
    public GameObject holder;
    [Space]
    public List<BasePlaneCore> planes = new List<BasePlaneCore>();

    public int Count { get { return planes.Count; } }

    [Header("UI")]
    public Text currentPlaneCountText;
    public Text totalPlaneCountText;
    public Slider planePercenTageSlider;

    int totalPlanes = 0;

    private void Start()
    {
        foreach (Transform item in holder.transform)
        {
            var t = item.GetComponent<BasePlaneCore>();
            if (t != null && t.gameObject.activeInHierarchy)
            {
                Add(t);
            }
        }
    }


    public void Add(BasePlaneCore ufo)
    {
        if (!planes.Contains(ufo))
        {
            planes.Add(ufo);
            totalPlanes++;
        }
    }

    public bool Remove(BasePlaneCore ufo)
    {
        return planes.Remove(ufo);
    }

    private void Update()
    {
        planes.RemoveAll(x => x == null || !x.IsAlive);


        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    foreach (var item in planes)
        //    {
        //        item.Health.KillInstantly(gameObject);
        //    }
        //}

        UpdateUI();

    }

    void UpdateUI()
    {
        if (currentPlaneCountText != null)
            currentPlaneCountText.text = planes.Count.ToString();

        if (totalPlaneCountText != null)
            totalPlaneCountText.text = totalPlanes.ToString();

        if (planePercenTageSlider != null)
            planePercenTageSlider.value = ((float)planes.Count / totalPlanes);
    }

    public BasePlaneCore GetPlane(int index)
    {
        if (planes.Count == 0)
            return null;
        return planes[index];
    }

}
