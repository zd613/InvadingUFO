using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuUI;
    bool isRunning = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isRunning)
            {
                Time.timeScale = 0;
                menuUI.SetActive(true);
                isRunning = false;
            }
        }
    }


    public void Restart()
    {
        BackToGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();  
#endif
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }


    public void BackToGame()
    {
        Time.timeScale = 1;
        menuUI.SetActive(false);
        isRunning = true;
    }
}
