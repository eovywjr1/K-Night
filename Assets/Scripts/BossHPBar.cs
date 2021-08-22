using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Boss bossInThisScene;
    public GameObject bossHpBar;
    private GameObject bossHpBarRed;

    private float initialBossHp;

    private void Awake()
    {
        if (bossInThisScene != null && bossHpBar != null)
        {
            bossHpBarRed = bossHpBar.transform.Find("RedHPBar").gameObject;
            initialBossHp = bossInThisScene.hp;
        }


    }


    void Update()
    {
        if (bossInThisScene != null && bossHpBar != null)
        {
            bossHpBarRed.GetComponent<Image>().fillAmount = bossInThisScene.hp / initialBossHp;
        }

    }
}
