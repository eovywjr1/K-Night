using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : TutorialMonsterBase
{
    int dashSpeed = 3;
    int skillDelaytime = 5;

    bool isjunorcallDelay = true;
    bool isdashToplayerDelay = true;
    bool isattack = false;

    public GameObject junior;
    GameObject childBoxCollider;

    void Start()
    {
        childBoxCollider = this.gameObject.transform.GetChild(0).gameObject;

        Initial();

        StartCoroutine(Skilldelay());
    }

    void Update()
    {
        if (!isjunorcallDelay)
            JuniorCall();

        if (!isdashToplayerDelay)
            DashFindCollision();
    }

    void FixedUpdate()
    {
        if (!isMoveDelay)
            RandomMove();

        if (!isdashToplayerDelay)
            Dash();
    }
    
    //보스 이동 함수
    void RandomMove()
    {
        rigidBody.AddForce(direction * 300 * Time.deltaTime, ForceMode2D.Impulse);
        isMoveDelay = true;
        StartCoroutine(Skilldelay());
    }

    //잡몹 소환 함수
    void JuniorCall()
    {
        Instantiate(junior, new Vector2(this.gameObject.transform.position.x + 2, this.gameObject.transform.position.y), Quaternion.identity); 
        isjunorcallDelay = true;
        StartCoroutine(Skilldelay());
    }

    void Dash()
    {
        //자식 오브젝트 레이어 변경
        childBoxCollider.layer = 9;

        //위치까지 속도 추가
        if ((direction == Vector2.right && playerPosition.x > this.gameObject.transform.position.x) || (direction == Vector2.left && playerPosition.x < this.gameObject.transform.position.x))
            Move(10, dashSpeed);

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
        raycastHit = Physics2D.Raycast(this.gameObject.transform.position, direction, 1.6f, LayerMask.GetMask("Player"));

        if (raycastHit.collider != null && raycastHit.collider.name == "Player" && !isattack)
        {
            isattack = true;

            player.HpDecrease(power);
        }
    }

    //잡몹 소환 딜레이
    IEnumerator Skilldelay()
    {
        yield return new WaitForSecondsRealtime(skillDelaytime);

        switch (Random.Range(1, 4))
        {
            //잡몹 소환
            case 1:
                isjunorcallDelay = false;

                break;

            //대쉬
            case 2:
                PlayerPositionSave();

                this.gameObject.layer = 9;

                isdashToplayerDelay = false;

                break;

            //랜덤 이동
            case 3:
                //좌우 방향 결정
                if (Random.Range(-1, 1) < 0)
                {
                    direction = Vector2.left;

                    spriteRenderer.flipX = false;
                }
                else
                {
                    direction = Vector2.right;

                    spriteRenderer.flipX = true;
                }

                isMoveDelay = false;

                break;

            default:
                break;
        }

        //코루틴 중단
        StopAllCoroutines();
    }
}