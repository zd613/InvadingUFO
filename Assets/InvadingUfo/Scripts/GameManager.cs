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
    public BasePlaneCore player;

    [Header("GameOver")]
    public GameObject gameOverUI;
    public Text gameOverText;
    [Range(0, 1)]
    public float gameOverAppearSpeed = 0.02f;
    public GameObject gameOverButtonPanel;

    [Header("GameClear")]
    public GameObject gameClearUI;
    public Text gameClearText;
    [Range(0, 1)]
    public float gameClearAppearSpeed = 0.02f;
    public float sceneTransitionDelaySec = 2;

    [Header("カメラ")]
    public GameObject mainCamera;

    [Header("debug")]
    public bool countDownOnStart = true;

    [Header("manager and spawner")]
    public HouseManager houseManager;
    public UfoManager ufoManager;
    public UfoSpawner ufoSpawner;
    public PriceManager priceManager;

    [Header("UI")]
    public long priceDeadLine = 1000000000;
    public Text priceDeadLineText;
    public Text currentDamagePriceText;
    //public Slider housePercentage;

    [Header("game")]
    public bool canClearGame = false;
    bool isGameOver = false;

    private void Awake()
    {
        player.OnDeath += () => StartCoroutine(GameOver());
    }


    private void Start()
    {
        if (countDownOnStart)
            StartCoroutine(CountDown());

        mainCamera.GetComponent<FollowingCamera>().enabled = false;
        var s = mainCamera.GetComponent<StartCameraAction>();
        s.enabled = true;
        s.OnFinished += () =>
        {
            s.enabled = false;
            mainCamera.GetComponent<FollowingCamera>().enabled = true;
        };

        //UI
        priceDeadLineText.text = ToDamagePriceText(priceDeadLine);
        currentDamagePriceText.text = "0円";

        //Ufo

        ufoSpawner.OnAllUfosSpawned += () => canClearGame = true;

        priceManager.OnDamagePriceChanged += () =>
        {
            if (priceManager.damagePrice > priceDeadLine)
            {
                StartCoroutine(GameOver());
            }
            currentDamagePriceText.text = ToDamagePriceText(priceManager.damagePrice);
        };
    }

    string[] unit = new string[] { "万", "億", "兆", "京", };
    string ToDamagePriceText(long price)
    {
        //int counter = 1;
        //int index = 0;
        //while (price != 0)
        //{
        //    if (counter % 4 == 0)
        //    {

        //    }
        //    price /= 10;
        //    counter++;

        //}
        string text = price + "円";

        return text;
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
        //currentDamagePriceText.text = houseManager.activeHouseCount.ToString();
        //housePercentage.value = houseManager.activeHouseCount / houseManager.houseCount;

        if (canClearGame && !isGameOver)
        {
            if (ufoManager.Count <= 0)
            {
                StartCoroutine(GameClear());
                canClearGame = false;

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
        if (player != null)
        {
            player.Rotation.isActive = true;
        }
        countDownText.text = "スタート";
        yield return new WaitForSeconds(1);
        countDownText.gameObject.SetActive(false);

    }

    IEnumerator GameOver()
    {
        isGameOver = true;
        gameOverButtonPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameOverUI.SetActive(true);
        float alpha = 0;
        while (alpha < 1)
        {
            var color = gameOverText.color;
            color.a = alpha;
            gameOverText.color = color;
            gameOverUI.SetActive(true);


            alpha += gameOverAppearSpeed;

            yield return null;
        }
        gameOverButtonPanel.SetActive(true);
    }

    IEnumerator GameClear()
    {
        yield return null;
        print("clear");

        yield return new WaitForSeconds(0.5f);
        gameClearUI.SetActive(true);
        float alpha = 0;
        while (alpha < 1)
        {
            var color = gameClearText.color;
            color.a = alpha;
            gameClearText.color = color;
            //gameClearUI.SetActive(true);


            alpha += gameClearAppearSpeed;

            yield return null;
        }

        yield return new WaitForSeconds(sceneTransitionDelaySec);
        LoadTitle();
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
