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

    //public int defensPower; // 밸런스 필요할 경우
    public int hp;
    public int moveSpeed;

    public bool ismoveDelay;

    void Start()
    {
        transForm = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //player = gameManager.getPlayer(); // 플레이어 구현 후 다시

        hp = 2;

        ismoveDelay = false;
    }

    void Update()
    {
        //플레이어로 이동
        if (!ismoveDelay)
        {
            /*transformPlayer = player.transform;
            Movetoplayer(transformPlayer);*/
        }

        //죽었을 때
        if (hp <= 0)
            Destroy(this.gameObject);

        //애니메이션 방향 전환
        if (rigidBody.velocity.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    //이동딜레이
    IEnumerator Changedirection()
    {
        yield return new WaitForSecondsRealtime(2f);

        ismoveDelay = false;
    }

    //이동
    public void Movetoplayer(Transform playerTransform)
    {
        if (playerTransform.position.x >= 0)
        {
            moveDirection = Vector2.right;
        }
        else
            moveDirection = Vector2.left;
        
        //속도제한
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
