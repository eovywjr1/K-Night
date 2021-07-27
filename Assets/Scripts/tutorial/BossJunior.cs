using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJunior : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public Transform transForm;
    public Transform transformPlayer;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public Vector2 moveDirection;
    public int defensPower; // �뷱�� �ʿ��� ���
    public int hp;
    public int moveSpeed;
    public int moveDelaytime;

    public bool ismoveDelay;

    void Start()
    {
        transForm = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //player = gameManager.getPlayer(); // �÷��̾� ���� �� �ٽ�
    }

    void Update()
    {
        //�÷��̾�� �̵�
        /*if (!ismoveDelay) // �÷��̾� ���� �� �ٽ�
        { 
            transformPlayer = player.transform;
            Movetoplayer(transformPlayer);
            ismoveDelay = true;
        }*/

        //�׾��� ��
        if (hp <= 0)
            Destroy(this.gameObject);

        //�ִϸ��̼� ���� ��ȯ
        if (rigidBody.velocity.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    //�̵�
    void Movetoplayer(Transform playerTransform)
    {
        //���� ����
        if (playerTransform.position.x > transForm.position.x)
            moveDirection = Vector2.right;
        else
            moveDirection = Vector2.left;

        //�ӵ�����
        if ((moveDirection == Vector2.right && rigidBody.velocity.x < moveSpeed) || (moveDirection == Vector2.left && rigidBody.velocity.x > moveSpeed))
            rigidBody.AddForce(moveDirection * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void Ondagamaed()
    {
        hp--;
    }

    //�̵�������(�ӽ�)
    IEnumerator Changedirection()
    {
        yield return new WaitForSecondsRealtime(moveDelaytime);
        ismoveDelay = false;
    }
}