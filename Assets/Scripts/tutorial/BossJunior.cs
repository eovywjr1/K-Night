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

    //�÷��̾� �浹 �� �� ����
    void FindCollision()
    {
        Debug.DrawRay(this.gameObject.transform.position, direction * 0.4f, new Color(0, 0, 1), LayerMask.GetMask("Player"));
        raycastHit = Physics2D.Raycast(this.gameObject.transform.position, direction, 0.4f, LayerMask.GetMask("Player"));
        Debug.Log(raycastHit.collider.name);
        if (raycastHit.collider.name == "Player")
            player.HpDecrease(power);
    }
}