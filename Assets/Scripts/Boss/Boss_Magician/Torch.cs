using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public Sprite torchOff;
    public Sprite torchOn;

    public GameObject Boss;
    public GameObject Timer;
    private GameObject child;

    private bool on;

    private void Awake()
    {
        Boss = GameObject.Find(nameof(Boss));
        child = transform.Find("Light").gameObject;
        on = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 횃불을 공격한다면
        if (collision.gameObject.tag == "Player" && on)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = torchOff;
            child.SetActive(false);
            Boss.GetComponent<Boss_Magician>().numOfTorchOff++;
            on = false;
        }
    }
    private void Update()
    {
        if (Timer != null)
        {
            if (Boss.GetComponent<Boss_Magician>().numOfTorchOff == 6)
            {
                if (Timer.GetComponent<TimeCountdown>().TimeEnd)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = torchOn;
                    child.SetActive(true);
                    on = true;
                }
            }
        }
    }
}
