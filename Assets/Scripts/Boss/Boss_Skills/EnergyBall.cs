using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    private float damage;
    private float throwSpeed;
    private Vector3 direction;
    private Rigidbody2D rigid;

    private void Awake()
    {
        damage = GameObject.Find("Boss").GetComponent<Boss_form>().damage_EnergyBall;
        throwSpeed = GameObject.Find("Boss").GetComponent<Boss_form>().energyBallSpeed;
        direction = GameObject.Find("Boss").GetComponent<Boss_form>().direction;
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        StartCoroutine(nameof(Throw));
    }
    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.5f);
        rigid.AddForce(direction * throwSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, 3f);
    }

    //플레이어에게 데미지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //damage to player
            Debug.Log("에너지 볼 맞음");
        }
    }
}
