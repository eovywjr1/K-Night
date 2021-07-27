using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junior : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public Transform transForm;
    public Transform transformPlayer;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public Vector2 moveDirection;

    //public int defensPower; // �뷱�� �ʿ��� ���
    public int hp;
    public int moveSpeed;

    public bool ismoveDelay;

    void Start()
    {
        transForm = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //player = gameManager.getPlayer(); // �÷��̾� ���� �� �ٽ�

        hp = 2;

        ismoveDelay = false;
    }

    void Update()
    {
        //�÷��̾�� �̵�
        if (!ismoveDelay)
        {
            /*transformPlayer = player.transform;
            Movetoplayer(transformPlayer);*/
        }

        //�׾��� ��
        if (hp <= 0)
            Destroy(this.gameObject);

        //�ִϸ��̼� ���� ��ȯ
        if (rigidBody.velocity.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    //�̵�������
    IEnumerator Changedirection()
    {
        yield return new WaitForSecondsRealtime(2f);

        ismoveDelay = false;
    }

    //�̵�
    public void Movetoplayer(Transform playerTransform)
    {
        if (playerTransform.position.x >= 0)
        {
            moveDirection = Vector2.right;
        }
        else
            moveDirection = Vector2.left;
        
        //�ӵ�����
        if (moveDirection == Vector2.right) {
            if (rigidBody.velocity.x < moveSpeed)
                rigidBody.AddForce(moveDirection * Time.deltaTime, ForceMode2D.Impulse);
        }
        else
        {
            if(rigidBody.velocity.x > moveSpeed)
                rigidBody.AddForce(moveDirection * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    public void Ondagamaed()
    {
        hp--;
    }
}
