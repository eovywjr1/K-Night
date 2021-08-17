using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJunior : TutorialMonsterBase
{
    int moveDelaytime = 2;

    void Start()
    {
        Initial();
    }

    void Update()
    { 
        FindCollision();
    }

    void FixedUpdate()
    {
        //플레이어로 이동
        if (!isMoveDelay)
            MoveToPlayer();
    }

    //이동
    void MoveToPlayer()
    {
        PlayerPositionSave();

        Move(10, moveSpeed);

        //플레이어를 지나칠 경우 딜레이
        if ((rigidBody.velocity.x > 0 && this.transform.position.x > playerPosition.x) || (rigidBody.velocity.x < 0 && this.transform.position.x < playerPosition.x))
        {
            rigidBody.velocity = new Vector2(0, 0);
            isMoveDelay = true;
            StartCoroutine(MoveDelay());
        }
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
        Debug.DrawRay(this.gameObject.transform.position, direction * 0.4f, new Color(0, 0, 1), LayerMask.GetMask("Player"));
        raycastHit = Physics2D.Raycast(this.gameObject.transform.position, direction, 0.4f, LayerMask.GetMask("Player"));
        Debug.Log(raycastHit.collider.name);
        if (raycastHit.collider.name == "Player")
            player.HpDecrease(power);
    }
}