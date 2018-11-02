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
    public MouseInputProvider mouseInputProvider;
    public Ame.PlayerInputProvider keybordInputProvider;

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
    public float gameClearButtonAppearTime = 2;
    public GameObject gameClearButtonPanel;

    [Header("カメラ")]
    public GameObject mainCamera;

    [Header("debug")]
    public bool countDownOnStart = true;

    [Header("manager and spawner")]
    public HouseManager houseManager;
    public UfoManager ufoManager;
    public UfoSpawner ufoSpawner;
    public PriceManager priceManager;
    public PlaneManager planeManager;

    [Header("UI")]
    public long priceDeadLine = 1000000000;
    public Text priceDeadLineText;
    public Text currentDamagePriceText;
    public Slider damagePriceSlider;

    [Header("game")]
    public bool canClearGame = false;
    bool isGameOver = false;

    [Header("UI fps")]
    public GameObject fpsCanvas;
    public Toggle fpsToggle;

    public Dropdown playerControlDropdown;
    public GameObject playerMouseUI;

    public Toggle reverseUpDownToggle;

    public PlayingStatus playingStatus = PlayingStatus.PlayingNow;

    public GameObject missionParent;
    public Transform planeAllyParent;
    public bool useGlobalMission = true;
    public static int missionLevel;
    public static bool useAllyPlane = false;

    private void Awake()
    {
        player.OnDeath += () => StartCoroutine(GameOver());

        if (useGlobalMission)
        {
            ufoSpawner.mission = missionParent.transform.GetChild(missionLevel).GetComponent<Mission>();

        }

        if (useAllyPlane)
        {
            foreach (Transform item in planeAllyParent)
            {
                item.gameObject.SetActive(true);
                item.transform.parent = planeManager.holder.transform;
            }
        }
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
            damagePriceSlider.value = ((float)priceManager.damagePrice / priceDeadLine);
        };
        damagePriceSlider.value = 0;

        fpsToggle.onValueChanged.AddListener(b => { fpsCanvas.SetActive(b); });

        playerControlDropdown.onValueChanged.AddListener((v) =>
        {
            if (v == 0)
            {
                player.inputProvider = mouseInputProvider;
                playerMouseUI.SetActive(true);
            }
            else if (v == 1)
            {
                player.inputProvider = keybordInputProvider;
                playerMouseUI.SetActive(false);
            }
        });

        reverseUpDownToggle.onValueChanged.AddListener((b) =>
        {
            print("c");

            if (b)//上下反転
            {
                player.Rotation.reversePitchControl = true;
            }
            else//default
            {
                player.Rotation.reversePitchControl = false;
            }
        });

    }

    string ToDamagePriceText(long price)
    {
        var sb = new System.Text.StringBuilder();
        var tyou = price / 1000000000000;
        if (tyou != 0)
        {
            sb.Append(tyou + "兆");
            price = price % 1000000000000;
        }


        var oku = price / 100000000;
        if (oku != 0)
        {
            sb.Append(oku + "億");
            price = price % 100000000;
        }

        var mann = price / 10000;
        if (mann != 0)
        {
            sb.Append(mann + "万");
            price = price % 10000;
        }

        if (price != 0)
        {
            sb.Append(price);
        }
        sb.Append("円");

        return sb.ToString();
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
            else
            {
                BackToGame();
            }
        }


        if (canClearGame && !isGameOver)
        {
            if (ufoManager.Count <= 0)
            {
                StartCoroutine(GameClear());
                canClearGame = false;
                playingStatus = PlayingStatus.GameClear;
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
        if (playingStatus == PlayingStatus.GameClear)
            yield break;
        if (isGameOver)
            yield break;
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


            alpha += gameOverAppearSpeed * Time.deltaTime;

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

            alpha += gameClearAppearSpeed * Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(gameClearButtonAppearTime);
        gameClearButtonPanel.SetActive(true);

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

public enum PlayingStatus
{
    PlayingNow,
    GameOver,
    GameClear,
}