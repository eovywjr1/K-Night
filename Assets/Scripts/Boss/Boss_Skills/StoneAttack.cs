using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//생성되는 프리팹에 적용
public class StoneAttack : MonoBehaviour
{
    private float throwSpeed;
    private bool onGround = false;
    private Vector3 direction;
    private float rnd;//던지는 돌의 속력을 랜덤하게
    private float delay;//좀 있다가 던져!

    private Rigidbody2D rigid;
    private void Awake()
    {
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
        rnd = Random.Range(1f, 2f);
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
            //damage to player
            Debug.Log("돌맞음");
        }
    }
    private void DestroyStone()
    {
        Destroy(gameObject);
    }
}
