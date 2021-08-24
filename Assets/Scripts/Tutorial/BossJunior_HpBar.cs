using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossJunior_HpBar : MonoBehaviour
{
    public Image image;
    private float startHp;
    void OnEnable()
    {
        startHp = GetComponent<BossJunior>().hp;
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = GetComponent<BossJunior>().hp / startHp;
    }
}
