using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHPBar_KimKH : MonoBehaviour
{
    public GameObject bossInThisScene;
    public GameObject bossHpBar;
    private GameObject bossHpBarRed;

    private float initialBossHp;

    private void Awake()
    {
        if (bossInThisScene != null && bossHpBar != null)
        {
            
            bossHpBarRed = bossHpBar.transform.Find("RedHPBar").gameObject;
            initialBossHp = bossInThisScene.GetComponent<LivingEntity>().startingHealth;
            
            





        }
    }


    void Update()
    {
        if (bossInThisScene != null && bossHpBar != null)
        {

            
            bossHpBarRed.GetComponent<Image>().fillAmount = bossInThisScene.GetComponent<LivingEntity>().health / initialBossHp;
            








        }
    }
}
