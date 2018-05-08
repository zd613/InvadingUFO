using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public void ChangeScene(string sceneName)
    {
        print("change");
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        print("hi");
        Application.Quit();
#if UNITY_EDITOR
        print("uni");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
