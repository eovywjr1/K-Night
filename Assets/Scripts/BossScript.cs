using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public int hp;
    public int power;
    public int defensePower;
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
    public Rigidbody2D rigidBody;
    public Transform transForm;
    public Transform playerTrasnform; //플레이어 위치 저장 변수(임시)
    public GameObject junior;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        transForm = GetComponent<Transform>();

        junior = Resources.Load<GameObject>("Prefabs/Junior");

        ismoveDelay = true;
        isjunorcallDelay = true;
        isdashToplayerDelay = true;

        hp = 3;

        StartCoroutine(RandomMove());
        StartCoroutine(callJunior());
        StartCoroutine(dashToplayer());

        
    }

    void Update()
    {
        //잡몹 소환
        if (!isjunorcallDelay)
        {
            Instantiate(junior, new Vector2(transForm.position.x, transForm.position.y),
                Quaternion.identity);

            isjunorcallDelay = true;

            StartCoroutine(callJunior());
        }
    }

    void FixedUpdate()
    {
        //보스 이동
        if (!ismoveDelay)
        {
            rigidBody.AddForce(direction * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);

            ismoveDelay = true;

            StartCoroutine(RandomMove());
        }

        //보스 돌진
        if (!isdashToplayerDelay)
        {
            playerTrasnform.position = new Vector2(3, 2);

            if (playerTrasnform.position.x >= 0)
                dashDirection = Vector2.right;
            else
                dashDirection = Vector2.left;

            while (transForm.position == playerTrasnform.position)
            {
                if (rigidBody.velocity.x < dashSpeed && rigidBody.velocity.x > dashSpeed * (-1))
                    rigidBody.AddForce(dashDirection * dashSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }

            rigidBody.velocity = new Vector2(0,0);

            isdashToplayerDelay = true;

            StartCoroutine(dashToplayer());
        }
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

    //잡몹 소환 딜레이
    IEnumerator callJunior()
    {
        yield return new WaitForSecondsRealtime(juniorcallDelaytime);

        isjunorcallDelay = false;
    }

    //플레이어로 돌진 딜레이
    IEnumerator dashToplayer()
    {
        yield return new WaitForSecondsRealtime(dashToplayerDelaytime);

        isdashToplayerDelay = false;
    }
}
