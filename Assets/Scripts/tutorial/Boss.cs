using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp;
    public int power;
    public int dashSpeed;
    public int moveSpeed;
    public int skillDelaytime;
    public int skillindex;

    public bool isSkilldelay;
    public bool isjunorcallDelay;
    public bool isdashToplayerDelay;
    public bool ismoveDelay;
    public bool isattack;

    public Vector2 direction;
    Vector3 playerPosition;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    public GameObject junior;
    GameObject childBoxCollider;
    Player player;

    RaycastHit2D rayHit;

    void Start()
    {
        player = FindObjectOfType<Player>();
        childBoxCollider = this.gameObject.transform.GetChild(0).gameObject;

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        StartCoroutine(Skilldelay());
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

        if (!isdashToplayerDelay)
            DashFindCollision();
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
        StartCoroutine(Skilldelay());
    }

    //잡몹 소환 함수
    void JuniorCall()
    {
        Instantiate(junior, new Vector2(this.gameObject.transform.position.x + 2, this.gameObject.transform.position.y), Quaternion.identity); 
        isjunorcallDelay = true;
        StartCoroutine(Skilldelay());
    }

    //공격받았을 때
    public void Ondamaged(int quantity)
    {
        hp -= quantity;
    }

    void Dash()
    {
        //자식 오브젝트 레이어 변경
        childBoxCollider.layer = 9;

        //위치까지 속도 추가
        if ((direction == Vector2.right && playerPosition.x > this.gameObject.transform.position.x) || (direction == Vector2.left && playerPosition.x < this.gameObject.transform.position.x)) {
            //속도 제한
            if ((direction == Vector2.right && rigidBody.velocity.x < dashSpeed) || (direction == Vector2.left && rigidBody.velocity.x > dashSpeed * (-1)))
                rigidBody.AddForce(direction * 10 * Time.deltaTime, ForceMode2D.Impulse);
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

            //모든 코루틴 시작
            StartCoroutine(Skilldelay());
        }
    }

    //대쉬 중 플레이어 충돌 찾기
    void DashFindCollision()
    {
        Debug.DrawRay(this.gameObject.transform.position, direction * 1.6f, new Color(0, 0, 1), LayerMask.GetMask("Player"));
        rayHit = Physics2D.Raycast(this.gameObject.transform.position, direction, 1.6f, LayerMask.GetMask("Player"));

        if (rayHit.collider.name == "Player" && isattack == false)
        {
            isattack = true;

            player.HpDecrease(power);
        }
    }

    //잡몹 소환 딜레이
    IEnumerator Skilldelay()
    {
        yield return new WaitForSecondsRealtime(skillDelaytime);

        skillindex = Random.Range(1, 4);

        switch (skillindex)
        {
            //잡몹 소환
            case 1:
                isjunorcallDelay = false;
                break;

            //대쉬
            case 2:
                //플레이어 위치 및 방향 저장
                playerPosition = new Vector3(player.GetTransform().position.x, this.gameObject.transform.position.y);
                if (playerPosition.x >= this.transform.position.x)
                    direction = Vector2.right;
                else
                    direction = Vector2.left;

                this.gameObject.layer = 9;

                isdashToplayerDelay = false;

                break;

            //랜덤 이동
            case 3:
                //좌우 방향 결정
                if (Random.Range(-1, 1) < 0)
                    direction = Vector2.left;
                else
                    direction = Vector2.right;

                ismoveDelay = false;

                break;

            default:
                break;
        }
    }
}