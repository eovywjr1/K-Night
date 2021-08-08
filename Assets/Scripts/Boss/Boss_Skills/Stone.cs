using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//생성되는 프리팹에 적용
public class Stone : Boss_Skills_Figures
{
    private float rnd;//던지는 돌의 속력을 랜덤하게
    private bool onGround = false;
    private float delay;//좀 있다가 던져!

    private float throwSpeed;
    private Vector3 direction;
    private Rigidbody2D rigid;

    private void Awake()
    {
        damage = GameObject.Find("Boss").GetComponent<Boss_form>().damage_Stone;
        throwSpeed = GameObject.Find("Boss").GetComponent<Boss_form>().throwSpeed;
        direction = GameObject.Find("Boss").GetComponent<Boss_form>().direction;
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(nameof(Throw));
    }
    IEnumerator Throw()
    {
        rnd = Random.Range(0.5f, 1.3f);
        rigid.gravityScale = 0;
        yield return new WaitForSeconds(rnd);
        rigid.gravityScale = 1;
        onGround = false;
        rigid.AddForce(direction * throwSpeed * rnd, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //땅(Platform과 접촉시 오브젝트 제거)
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
            rigid.velocity = Vector2.zero;
            rigid.gravityScale = 0;
            Invoke(nameof(DestroyStone), 1f);
        }
        //플레이어와 접촉시 데미지
        else if (collision.gameObject.tag == "Player" && !onGround)
        {
            Debug.Log("돌맞음");
            player.HpDecrease(damage);
        }
    }
    private void DestroyStone()
    {
        Destroy(gameObject);
    }
}
