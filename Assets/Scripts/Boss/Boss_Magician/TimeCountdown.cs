using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountdown : MonoBehaviour
{
    public Text TimeCount;
    public float TimeCost;

    public bool TimeEnd;

    private void Awake()
    {
        TimeEnd = false;
    }

    private void Update()
    {
        TimeCost -= Time.deltaTime;
        TimeCount.text = $"{TimeCost:N2}";
        if (TimeCost <= 0.1)
        {
            TimeEnd = true;
            this.gameObject.SetActive(false);
        }
    }
}
