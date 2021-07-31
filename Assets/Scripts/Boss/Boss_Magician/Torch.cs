using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public Sprite torchOff;
    public Sprite torchOn;

    public GameObject Boss;

    private bool on;

    private void Awake()
    {
        Boss = GameObject.Find(nameof(Boss));
        on = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 횃불을 공격한다면
        if (collision.gameObject.tag == "Player" && on)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = torchOff;
            Boss.GetComponent<Boss_Magician>().numOfTorchOff++;
            on = false;
        }
    }
    private void Update()
    {
        if(Boss.GetComponent<Boss_Magician>().numOfTorchOff++ == 6)
        {
            //15초 후 횃불 켜짐
            this.gameObject.GetComponent<SpriteRenderer>().sprite = torchOn;
            on = true;

        }
    }
}
