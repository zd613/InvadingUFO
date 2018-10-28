using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UfoManager : MonoBehaviour
{
    public GameObject ufoHolder;
    public List<BaseUfoCore> ufos = new List<BaseUfoCore>();

    public int Count { get { return ufos.Count; } }

    [Header("UI")]
    public Text currentUfoCountText;
    public Text totalUfoCountText;
    public Slider ufoPercenTageSlider;

    int totalUfos = 0;

    private void Awake()
    {
        if (ufoHolder == null)
        {
            ufoHolder = new GameObject("UfoHolder");

        }


    }

    private void Start()
    {
        if (ufoHolder != null)
        {
            foreach (Transform item in ufoHolder.transform)
            {
                var core = item.GetComponent<BaseUfoCore>();
                if (core != null)
                {
                    Add(core);
                }
            }
        }
    }

    public void Add(BaseUfoCore ufo)
    {
        if (!ufos.Contains(ufo))
        {
            ufos.Add(ufo);
            totalUfos++;
        }
    }

    public bool Remove(BaseUfoCore ufo)
    {
        return ufos.Remove(ufo);
    }

    private void Update()
    {
        ufos.RemoveAll(x => x == null || !x.IsAlive);
        UpdateUI();

    }

    void UpdateUI()
    {
        currentUfoCountText.text = ufos.Count.ToString();
        totalUfoCountText.text = totalUfos.ToString();

        ufoPercenTageSlider.value = ((float)ufos.Count / totalUfos);
    }

    public BaseUfoCore GetUfo(int index)
    {
        if (ufos.Count == 0)
            return null;
        return ufos[index];
    }
}
