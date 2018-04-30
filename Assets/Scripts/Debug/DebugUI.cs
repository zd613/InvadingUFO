using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class DebugUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Text fpsText;
    [SerializeField]
    private Text averageFpsText;

    [Header("設定")]
    [SerializeField]
    private int averageFpsArrayLength = 60;

    public float Fps { get; private set; }
    public float AverageFps { get; private set; }

    private float[] fpsArray;
    private int count = 0;

    private void Awake()
    {
        fpsArray = new float[averageFpsArrayLength];
    }

    private void Update()
    {
        Fps = 1 / Time.unscaledDeltaTime;
        fpsArray[count] = Fps;
        count++;
        if (count >= averageFpsArrayLength)
            count = 0;
        AverageFps = fpsArray.Average();



        if (fpsText != null)
        {
            fpsText.text = "fps:" + Fps;
        }
        if (averageFpsText != null)
        {
            averageFpsText.text = "average fps:" + AverageFps;
        }
    }
}
