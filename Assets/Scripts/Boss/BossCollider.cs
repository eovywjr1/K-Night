using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    Boss_form bossForm;
    Player player;

    private void Start()
    {
        bossForm = GameObject.Find("Boss").GetComponent<Boss_form>();
        player = FindObjectOfType<Player>();
    }
    /////////////////////////////////////////////
    /////////////플레이어에 의한 피격////////////
    /////////////////////////////////////////////
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword") && player.attackOnce)
        {
            bossForm.OnDamage(player.atkDamage);
            player.attackOnce = false;
        }
    }
}
