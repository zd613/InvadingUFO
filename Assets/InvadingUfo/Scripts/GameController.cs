using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isPlaying;


    private void Awake()
    {
        isPlaying = true;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayGame();
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0;
        isPlaying = false;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        isPlaying = true;
    }


}
