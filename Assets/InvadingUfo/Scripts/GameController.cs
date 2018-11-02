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

    //private void Update()
    //{

    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        StopGame();
    //    }

    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        PlayGame();
    //    }
    //}

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

    //public void ChangeSceneAndSendMissionLevel(string sceneName)
    //{
    //    System.Action action = () =>
    //    {
    //        var param = FindObjectOfType<MissionLevel>();
    //        param.number = 1;
    //    };
    //    StartCoroutine(LoadSceneAsync(sceneName, action));
    //}

    public void ChangeScene(string sceneName)
    {
        print("change");
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneAsync(string sceneName, System.Action onLoad)
    {
        var ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return ao;

        onLoad?.Invoke();
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
