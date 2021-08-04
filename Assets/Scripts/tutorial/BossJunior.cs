using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJunior : MonoBehaviour
{
    public Player player;

    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;

    public Vector2 moveDirection;
    public Vector2 playerPosition;

    public int hp;
    public int power;
    public int moveSpeed;
    public int moveDelaytime;

    public bool isMoveDelay;

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
        if (rigidBody.velocity.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
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
            rigidBody.AddForce(moveDirection * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);

        //�÷��̾ ����ĥ ��� ������
        if ((rigidBody.velocity.x > 0 && this.transform.position.x > playerPosition.x) || (rigidBody.velocity.x < 0 && this.transform.position.x < playerPosition.x))
        {
            rigidBody.velocity = new Vector2(0, 0);
            isMoveDelay = true;
            StartCoroutine(MoveDelay());
        }
    }

    //�ǰ� �޾��� �� �� ����
    public void Ondagamaed(int quantitiy)
    {
        hp -= quantitiy;
    }

    //�̵�������
    IEnumerator MoveDelay()
    {
        yield return new WaitForSecondsRealtime(moveDelaytime);

        isMoveDelay = false;
    }

    //�÷��̾� �浹 �� �÷��̾� �� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //�÷��̾� �� ����
            player.HpDecrease(power);

            //�浹 �� ����
            rigidBody.velocity = new Vector2(0, 0);
            isMoveDelay = true;
            StartCoroutine(MoveDelay());
        }
    }
}