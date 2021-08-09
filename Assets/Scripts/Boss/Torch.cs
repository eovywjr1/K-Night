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
    public bool on;
    public bool refill;

    private void Awake()
    {
        Boss = GameObject.Find(nameof(Boss));
        child = transform.Find("Light").gameObject;
        on = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 횃불을 공격한다면
        if (collision.gameObject.CompareTag("Player") && on)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = torchOff;
            child.SetActive(false);
            Boss.GetComponent<Boss_form>().numOfTorchOff++;
            on = false;
        }
    }
    private void Update()
    { 
        if (Timer.GetComponent<TimeCountdown>().TimeEnd && !on)
        {
            refill = Boss.GetComponent<Boss_form>().refill;
            if (refill)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = torchOn;
                child.SetActive(true);
                Boss.GetComponent<Boss_form>().numOfTorchOff--;
                on = true;
            }
        }
    }
}
