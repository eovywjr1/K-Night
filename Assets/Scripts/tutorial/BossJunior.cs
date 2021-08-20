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
}