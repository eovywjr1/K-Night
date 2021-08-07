using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountdown: MonoBehaviour
{
    public Text TimeCount; // 타이머 텍스트
    public float TimeCost; // 시작 시간

    public bool TimeEnd; // 끝났나?

    private void Awake()
    {
        TimeEnd = false;
        TimeCost = GameObject.Find("Boss").GetComponent<Boss_form>().timerStartTime;
    }

    private void Update()
    {
        TimeCost -= Time.deltaTime;
        TimeCount.text = $"{TimeCost:N2}";
        if (TimeCost <= 0.01)
        {
            TimeEnd = true;
            this.gameObject.SetActive(false);
        }
    }
}
