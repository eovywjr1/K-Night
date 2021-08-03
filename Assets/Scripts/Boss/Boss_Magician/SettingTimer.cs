using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingTimer : MonoBehaviour
{
    public GameObject Timer;
    public float time_Max;

    private Text text_Timer;
    private float time_start;
    private float time_current;
    public bool isEnded;

    private void Awake()
    {
        text_Timer = Timer.GetComponent<Text>();
    }
    private void Start()
    {
        Reset_Timer();
    }
    void Update()
    {
        if (isEnded)
            Timer.SetActive(false);

        Check_Timer();
    }

    private void Check_Timer()
    {
        time_current = Time.time - time_start;
        if (time_current < time_Max)
        {
            text_Timer.text = $"{time_current:N2}";
            Debug.Log(time_current);
        }
        else if (!isEnded)
        {
            End_Timer();
        }
    }

    private void End_Timer()
    {
        Debug.Log("End");
        time_current = time_Max;
        text_Timer.text = $"{time_current:N2}";
        isEnded = true;
    }


    private void Reset_Timer()
    {
        time_start = Time.time;
        time_current = 0;
        text_Timer.text = $"{time_current:N2}";
        isEnded = false;
        Debug.Log("Start");
    }
}
