using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJunior : MonoBehaviour
{
    Player player;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    Vector2 moveDirection;
    Vector2 playerPosition;

    public int hp;
    public int power;
    int moveSpeed = 1;
    int moveDelaytime = 2;

    bool isMoveDelay;

    RaycastHit2D rayHit;

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
        if (moveDirection == Vector2.left)
            spriteRenderer.flipX = false;
        else if(moveDirection == Vector2.right)
            spriteRenderer.flipX = true;

        FindCollision();
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
            rigidBody.AddForce(moveDirection * 10 * Time.deltaTime, ForceMode2D.Impulse);

        //플레이어를 지나칠 경우 딜레이
        if ((rigidBody.velocity.x > 0 && this.transform.position.x > playerPosition.x) || (rigidBody.velocity.x < 0 && this.transform.position.x < playerPosition.x))
        {
            rigidBody.velocity = new Vector2(0, 0);
            isMoveDelay = true;
            StartCoroutine(MoveDelay());
        }
    }

    //피격 받았을 때 피 감소
    public void Ondagamaed(int power)
    {
        hp -= power;
    }

    //이동딜레이
    IEnumerator MoveDelay()
    {
        yield return new WaitForSecondsRealtime(moveDelaytime);

        isMoveDelay = false;
    }

    //플레이어 충돌 시 피 감소
    void FindCollision()
    {
        Debug.DrawRay(this.gameObject.transform.position, moveDirection * 0.4f, new Color(0, 0, 1), LayerMask.GetMask("Player"));
        rayHit = Physics2D.Raycast(this.gameObject.transform.position, moveDirection, 0.4f, LayerMask.GetMask("Player"));
        Debug.Log(rayHit.collider.name);
        if (rayHit.collider.name == "Player")
            player.HpDecrease(power);
    }
}