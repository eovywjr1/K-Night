using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJunior : MonoBehaviour
{
    public Player player;

    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;

    public Vector2 moveDirection;
    public Vector2 playerPosition;

    public int hp;
    public int power;
    public int moveSpeed;
    public int moveDelaytime;

    public bool isMoveDelay;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        //죽었을 때
        if (hp <= 0)
            Destroy(this.gameObject);

        //애니메이션 방향 전환
        if (rigidBody.velocity.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        //플레이어로 이동
        if (!isMoveDelay)
            Movetoplayer();
    }

    //이동
    void Movetoplayer()
    {
        //플레이어 위치 저장
        playerPosition = player.GetTransform().position;

        //방향 설정
        if (playerPosition.x > this.gameObject.transform.position.x)
            moveDirection = Vector2.right;
        else
            moveDirection = Vector2.left;

        //속도 추가, 제한
        if ((moveDirection == Vector2.right && rigidBody.velocity.x < moveSpeed) || (moveDirection == Vector2.left && rigidBody.velocity.x > moveSpeed * (-1)))
            rigidBody.AddForce(moveDirection * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);

        //플레이어를 지나칠 경우 딜레이
        if ((rigidBody.velocity.x > 0 && this.transform.position.x > playerPosition.x) || (rigidBody.velocity.x < 0 && this.transform.position.x < playerPosition.x))
        {
            rigidBody.velocity = new Vector2(0, 0);
            isMoveDelay = true;
            StartCoroutine(MoveDelay());
        }
    }

    //피격 받았을 때 피 감소
    public void Ondagamaed(int quantitiy)
    {
        hp -= quantitiy;
    }

    //이동딜레이
    IEnumerator MoveDelay()
    {
        yield return new WaitForSecondsRealtime(moveDelaytime);

        isMoveDelay = false;
    }

    //플레이어 충돌 시 플레이어 피 감소
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어 피 감소
            player.HpDecrease(power);

            //충돌 시 멈춤
            rigidBody.velocity = new Vector2(0, 0);
            isMoveDelay = true;
            StartCoroutine(MoveDelay());
        }
    }
}