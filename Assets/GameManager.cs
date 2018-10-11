using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuUI;
    bool isRunning = true;
    public Text countDownText;
    public CommonCore player;

    [Header("GameOver")]
    public GameObject gameOverUI;
    public Text gameOverText;
    [Range(0, 1)]
    public float speed = 0.02f;
    public GameObject buttonPanel;

    private void Awake()
    {
        //StartCoroutine(CountDown());

        player.OnDeath += () => StartCoroutine(GameOver());
    }

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

    public IEnumerator CountDown()
    {
        if (player != null)
        {
            player.Rotation.isActive = false;
        }

        countDownText.gameObject.SetActive(true);
        countDownText.text = "3";
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        yield return new WaitForSeconds(1);
        countDownText.text = "スタート";
        yield return new WaitForSeconds(1);
        countDownText.gameObject.SetActive(false);
        if (player != null)
        {
            player.Rotation.isActive = true;
        }
    }

    IEnumerator GameOver()
    {
        buttonPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameOverUI.SetActive(true);
        float alpha = 0;
        while (alpha < 1)
        {
            var color = gameOverText.color;
            color.a = alpha;
            gameOverText.color = color;
            gameOverUI.SetActive(true);


            alpha += speed;

            yield return null;
        }
        buttonPanel.SetActive(true);
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
