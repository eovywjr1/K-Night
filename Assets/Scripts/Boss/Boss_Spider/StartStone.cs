using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStone : MonoBehaviour
{
    private Player player;
    private int damage;
    [SerializeField]
    private float throwSpeed;

    private bool onGround = false;

    private Vector3 direction;
    private Rigidbody2D rigid;
    private GameObject boss;
    private void Awake()
    {
        boss = GameObject.Find("Boss_Start");

        player = FindObjectOfType<Player>();
        rigid = GetComponent<Rigidbody2D>();
        damage = boss.GetComponent<Boss_Spider_Start>().damage_Stone;
    }
    void Start()
    {
        direction = new Vector3(1, 1, 0);
        rigid.AddForce(direction * throwSpeed , ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��(Ground�� ���˽� ������Ʈ ����)
        if (collision.CompareTag("Ground"))
        {
            onGround = true;
            rigid.velocity = Vector3.zero;
            rigid.gravityScale = 0;
            Invoke(nameof(DestroyStone), 0.3f);
        }
        //�÷��̾�� ���˽� ������
        else if (collision.CompareTag("Player") && !onGround)
        {
            Debug.Log("������");
            player.HpDecrease(damage);
        }
    }
    private void DestroyStone()
    {
        Destroy(gameObject);
    }
}
