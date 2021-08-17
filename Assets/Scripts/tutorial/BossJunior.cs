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
        //�׾��� ��
        if (hp <= 0)
            Destroy(this.gameObject);

        //�ִϸ��̼� ���� ��ȯ
        if (moveDirection == Vector2.left)
            spriteRenderer.flipX = false;
        else if(moveDirection == Vector2.right)
            spriteRenderer.flipX = true;

        FindCollision();
    }

    void FixedUpdate()
    {
        //�÷��̾�� �̵�
        if (!isMoveDelay)
            Movetoplayer();
    }

    //�̵�
    void Movetoplayer()
    {
        //�÷��̾� ��ġ ����
        playerPosition = player.GetTransform().position;

        //���� ����
        if (playerPosition.x > this.gameObject.transform.position.x)
            moveDirection = Vector2.right;
        else
            moveDirection = Vector2.left;

        //�ӵ� �߰�, ����
        if ((moveDirection == Vector2.right && rigidBody.velocity.x < moveSpeed) || (moveDirection == Vector2.left && rigidBody.velocity.x > moveSpeed * (-1)))
            rigidBody.AddForce(moveDirection * 10 * Time.deltaTime, ForceMode2D.Impulse);

        //�÷��̾ ����ĥ ��� ������
        if ((rigidBody.velocity.x > 0 && this.transform.position.x > playerPosition.x) || (rigidBody.velocity.x < 0 && this.transform.position.x < playerPosition.x))
        {
            rigidBody.velocity = new Vector2(0, 0);
            isMoveDelay = true;
            StartCoroutine(MoveDelay());
        }
    }

    //�ǰ� �޾��� �� �� ����
    public void Ondagamaed(int power)
    {
        hp -= power;
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
        Debug.DrawRay(this.gameObject.transform.position, moveDirection * 0.4f, new Color(0, 0, 1), LayerMask.GetMask("Player"));
        rayHit = Physics2D.Raycast(this.gameObject.transform.position, moveDirection, 0.4f, LayerMask.GetMask("Player"));
        Debug.Log(rayHit.collider.name);
        if (rayHit.collider.name == "Player")
            player.HpDecrease(power);
    }
}