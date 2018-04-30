using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPlayerInfoUI : MonoBehaviour
{
    public GameObject player;

    [Header("テキスト")]
    public Text hpText;

    Health health;
    private void Awake()
    {
        health = player.GetComponent<Health>();

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

    }

    void UpdateHpText()
    {
        hpText.text = string.Format("hp:{0,4} / {1,4} ", health.hp, health.MaxHp);
    }
}
