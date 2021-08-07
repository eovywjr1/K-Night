using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : MonoBehaviour
{
    private float damage;
    private bool onGround = false;
    private Rigidbody2D rigid;

    private void Awake()
    {
        damage = GameObject.Find("Boss").GetComponent<Boss_form>().damage_Meteo;
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = GameObject.Find(nameof(Boss)).GetComponent<Boss_form>().meteoGravity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //땅(Ground과 접촉시 오브젝트 제거)
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
            rigid.velocity = Vector3.zero;
            rigid.gravityScale = 0;
            Invoke(nameof(DestroyMeteo), 0.3f);
        }
        //플레이어와 접촉시 데미지
        else if (collision.gameObject.tag == "Player" && !onGround)
        {
            //damage to player
            Debug.Log("메테오맞음");
        }
    }
    private void DestroyMeteo()
    {
        Destroy(gameObject);
    }
}
