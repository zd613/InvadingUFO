using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class FpsCounter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Text fpsText;
    [SerializeField]
    private Text averageFpsText;
    public Color normalColor = Color.black;
    public Color middleColor = Color.yellow;
    public Color lowColor = Color.red;

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
            ChangeColor(fpsText, Fps);
            fpsText.text = "fps:" + Fps;
        }
        if (averageFpsText != null)
        {
            ChangeColor(averageFpsText, AverageFps);
            averageFpsText.text = "average fps:" + AverageFps;
        }
    }

    void ChangeColor(Text text, float fps)
    {
        if (fps < 15)
        {
            text.color = lowColor;
        }
        else if (fps < 40)
        {
            text.color = middleColor;
        }
        else
        {
            text.color = normalColor;
        }
    }
}
