using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp;
    public int power;
    public int dashSpeed;
    public int moveSpeed;
    public int moveDelaytime;
    public int juniorcallDelaytime;
    public int dashToplayerDelaytime;

    public bool ismoveDelay;
    public bool isjunorcallDelay;
    public bool isdashToplayerDelay;

    public Vector2 direction;
    public Vector2 dashDirection;
    public Vector3 playerPosition;

    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;

    public GameObject junior;
    public Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(RandomMove());
        StartCoroutine(JuniorCalldelay());
        StartCoroutine(DashToplayer());
    }

    void Update()
    {
        //잡몹 소환
        if (!isjunorcallDelay)
            JuniorCall();

        //죽었을 때
        if (hp <= 0)
            this.gameObject.SetActive(false);

        //애니메이션 방향 전환
        if (rigidBody.velocity.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        //보스 이동
        if (!ismoveDelay)
            Move();

        //보스 돌진
        if (!isdashToplayerDelay)
            Dash();
    }
    
    //보스 이동 함수
    void Move()
    {
        rigidBody.AddForce(direction * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        ismoveDelay = true;
        StartCoroutine(RandomMove());
    }

    //잡몹 소환 함수
    void JuniorCall()
    {
        Instantiate(junior, new Vector2(this.gameObject.transform.position.x + 2, this.gameObject.transform.position.y), Quaternion.identity); 
        isjunorcallDelay = true;
        StartCoroutine(JuniorCalldelay());
    }

    //공격받았을 때
    public void Ondamaged()
    {
        hp--;
    }

    void Dash()
    {
        //Move 코루틴 중단
        StopCoroutine(RandomMove());

        //위치까지 속도 추가
        if ((dashDirection == Vector2.right && playerPosition.x > this.gameObject.transform.position.x) || (dashDirection == Vector2.left && playerPosition.x < this.gameObject.transform.position.x)) {
            //속도 제한
            if ((dashDirection == Vector2.right && rigidBody.velocity.x < dashSpeed) || (dashDirection == Vector2.left && rigidBody.velocity.x > dashSpeed * (-1)))
                rigidBody.AddForce(dashDirection * dashSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        //위치 도착 후
        else
        {
            rigidBody.velocity = new Vector2(0, 0);
            this.gameObject.layer = 7;

            isdashToplayerDelay = true;
            StartCoroutine(DashToplayer());
            StartCoroutine(RandomMove());
        }
    }

    //잡몹 소환 딜레이
    IEnumerator JuniorCalldelay()
    {
        yield return new WaitForSecondsRealtime(juniorcallDelaytime);

        isjunorcallDelay = false;
    }

    //플레이어로 돌진 딜레이
    IEnumerator DashToplayer()
    {
        yield return new WaitForSecondsRealtime(dashToplayerDelaytime);

        //플레이어 위치 및 방향 저장
        playerPosition = new Vector3(player.GetTransform().position.x, this.gameObject.transform.position.y);
        if (playerPosition.x >= this.transform.position.x)
            dashDirection = Vector2.right;
        else
            dashDirection = Vector2.left;

        this.gameObject.layer = 9;

        isdashToplayerDelay = false;
    }

    //보스 이동 딜레이
    IEnumerator RandomMove()
    {
        yield return new WaitForSecondsRealtime(moveDelaytime);

        //좌우 방향 결정
        if (Random.Range(-1, 1) < 0)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        ismoveDelay = false;
    }
}