using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI")]
    public Slider bulletSlider;
    public Slider hpSlider;

    public Text missileText;

    [Header("Player")]
    public Health playerHealth;
    public Attack playerBulletAttack;
    public Ame.PlayerMissileAttack playerMissileAttack;


    private void LateUpdate()
    {
        UpdateHealth();
        UpdateMissile();
        UpdateBullet();
    }

    private void UpdateBullet()
    {
        
    }

    private void UpdateMissile()
    {
    }

    void UpdateHealth()
    {
        hpSlider.value = playerHealth.hp / playerHealth.MaxHp;
    }
}
