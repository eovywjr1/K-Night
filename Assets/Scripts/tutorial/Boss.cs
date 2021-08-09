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
    public bool isattack;

    public Vector2 direction;
    public Vector3 playerPosition;

    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;

    public GameObject junior;
    public GameObject childBoxCollider;
    public Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        childBoxCollider = this.gameObject.transform.GetChild(0).gameObject;

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        StartCoroutine(RandomMove());
        StartCoroutine(JuniorCalldelay());
        //StartCoroutine(DashToplayer());
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
        if (direction == Vector2.right)
            spriteRenderer.flipX = true;
        else if (direction == Vector2.left)
            spriteRenderer.flipX = false;
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
    public void Ondamaged(int quantity)
    {
        hp -= quantity;
    }

    void Dash()
    {
        //Move 코루틴 중단
        StopCoroutine(RandomMove());

        //자식 오브젝트 레이어 변경
        childBoxCollider.layer = 9;

        //위치까지 속도 추가
        if ((direction == Vector2.right && playerPosition.x > this.gameObject.transform.position.x) || (direction == Vector2.left && playerPosition.x < this.gameObject.transform.position.x)) {
            //속도 제한
            if ((direction == Vector2.right && rigidBody.velocity.x < dashSpeed) || (direction == Vector2.left && rigidBody.velocity.x > dashSpeed * (-1)))
                rigidBody.AddForce(direction * dashSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        //위치 도착 후
        else
        {
            //속도 제거
            rigidBody.velocity = new Vector2(0, 0);

            //레이어 초기화
            this.gameObject.layer = 7;
            childBoxCollider.layer = 11;

            //딜레이, 공격 플래그 초기화
            isdashToplayerDelay = true;
            isattack = false;

            //딜레이 시작
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
            direction = Vector2.right;
        else
            direction = Vector2.left;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //대쉬 중 처음 플레이어가 공격받았을 때 플레이어 피 감소
        if (collision.gameObject.CompareTag("Player") && !isattack && this.gameObject.layer == 9)
        {
            player.HpDecrease(power);
            isattack = true;
        }
    }
}