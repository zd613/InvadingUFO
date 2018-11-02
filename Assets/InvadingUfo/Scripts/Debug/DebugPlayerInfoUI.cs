using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPlayerInfoUI : MonoBehaviour
{
    public GameObject player;
    public Image crosshair;
    [Header("テキスト")]
    public Text hpText;
    public Text mousePosText;
    public Text crosshairPosText;
    public Text diffPosText;
    public Text inputText;

    Health health;
    BaseInputProvider inputProvider;
    private void Awake()
    {
        health = player.GetComponent<Health>();
        inputProvider = player.GetComponent<BaseInputProvider>();
        if (health != null)
        {
            health.OnDamageTaken += () => UpdateHpText();
        }
    }

    private void Start()
    {
        if (health != null)
        {
            UpdateHpText();
        }
    }

    private void Update()
    {
        Vector2 mousePos = (Vector2)Input.mousePosition;
        mousePosText.text = "mouse:" + mousePos;

        Vector2 crosshairPos= (Vector2)crosshair.GetComponent<RectTransform>().position;
        crosshairPosText.text = "crosshair:" + crosshairPos;

        Vector2 diff = mousePos - crosshairPos;
        diffPosText.text = "diff pos:" + diff;
        inputText.text = "pitch:" + inputProvider.PitchValue + ",yaw:" + inputProvider.YawValue;
    }

    void UpdateHpText()
    {
        hpText.text = string.Format("hp:{0,4} / {1,4} ", health.hp, health.MaxHp);
    }
}
