using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public float throwSpeed;
    private Vector3 direction;

    public Rigidbody2D rigid;
    private void Awake()
    {
        throwSpeed = GameObject.Find("Boss").GetComponent<Boss_Magician>().throwSpeed;
        direction = GameObject.Find("Boss").GetComponent<Boss_Magician>().direction;
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
        Destroy(gameObject, 4f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //damage to player
            Debug.Log("에너지 볼 맞음");
        }
    }
}
