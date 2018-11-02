using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StageDamageMode
{
    TakeDamage,
    DieInstantly,
}

public class StageData
{
    //ステージに当たったときに受けるダメージ
    public static float DamageValue = 0.1f;
}