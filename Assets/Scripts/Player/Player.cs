using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* 
    C: dash 
    X: attack 
    */

    public string myName; // player 이름 (만약 게임 시작시 입력받는다면)

    public int hp; // 체력
    public int power; // attack power


    public float moveSpeed = 4f; // 이동속도
    public float jumpSpeed = 4f; // 점프속도
    public float dashSpeed = 30f; // 대쉬속도


    public int jumpCount = 1; // 점프 횟수 (2단점프x)

    /*private bool isGrounded = false; */
    private bool isJumping = false; // 점프상태
    public bool isdash = false; // 대쉬상태

    Rigidbody2D rigid;

    Vector3 movement;



    void Start()
    {

        rigid = gameObject.GetComponent<Rigidbody2D>();
        jumpCount = 0;
    }


    /* private void OnCollsionEnter2D(Collider2D col)   // Ground tag에 닿으면 점프횟수 초기화 (다시 점프 가능하도록)
    {
        

        if (col.gameObject.tag == "Ground")
        {

            Debug.Log("isGrounded!");


            isGrounded = true;
            jumpCount--;
            
        }
    } */

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount == 0)
            {
                if (isJumping == false)
                {
                    isJumping = true;
                    /*jumpCount++;*/


                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))

        {
            isdash = true;
        }

    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        Dash();

    }

    void Move() // 좌우 움직임 
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false; 
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true; 
        }

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }

    void Jump() // 점프 
    {
        if (!isJumping)
            return;

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpSpeed);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;


    }

    void Dash() // 대쉬
    {
        Vector3 moveVelocity = Vector3.zero;


        if (!isdash)
            return;
        // 캐릭터가 좌측을 보고 있을 때
        if (this.gameObject.GetComponent<SpriteRenderer>().flipX == false)
        {
            moveVelocity = Vector3.left;
        }
        // 캐릭터가 우측을 보고 있을 때
        if (this.gameObject.GetComponent<SpriteRenderer>().flipX == true)
        {
            moveVelocity = Vector3.right;
        }

        transform.position += moveVelocity * dashSpeed * Time.deltaTime;

        isdash = false;

    }

    public Transform GetTransform()
    {
        return this.gameObject.transform;
    }

    public void SetHp()
    {
        hp--;
    }


}
