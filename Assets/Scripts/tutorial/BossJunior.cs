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
        //�÷��̾�� �̵�
        if (!isMoveDelay)
            MoveToPlayer();
    }

    //�̵�
    void MoveToPlayer()
    {
        PlayerPositionSave();

        Move(10, moveSpeed);

        //�÷��̾ ����ĥ ��� ������
        if ((rigidBody.velocity.x > 0 && this.transform.position.x > playerPosition.x) || (rigidBody.velocity.x < 0 && this.transform.position.x < playerPosition.x))
        {
            rigidBody.velocity = new Vector2(0, 0);
            isMoveDelay = true;
            StartCoroutine(MoveDelay());
        }
    }

    //�̵�������
    IEnumerator MoveDelay()
    {
        yield return new WaitForSecondsRealtime(moveDelaytime);

        isMoveDelay = false;
    }
}