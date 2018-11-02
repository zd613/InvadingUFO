using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bgm : MonoBehaviour
{
    public bool dontDestroy = true;

    public static Bgm Instance { get; private set; } = null;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
