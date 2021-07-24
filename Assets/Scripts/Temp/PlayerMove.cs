using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    private bool isJumping;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        isJumping = false;
    }
    //단발적인 키 입력
    void Update()
    {
        //Jump
        if (Input.GetKeyDown(KeyCode.C) && !isJumping)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJumping = true;
        }
        if (rigid.velocity.y == 0) isJumping = false;
        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f,rigid.velocity.y);
        }
        //Direction Sprite
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

    }
    //꾹누르기
    private void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        //Max Speed
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < (-1)*maxSpeed)
            rigid.velocity = new Vector2((-1)*maxSpeed, rigid.velocity.y);
        
        //OnCollsionEnter 2D
        /*
        void OnCollisionEnter2D(Collision2D collsion)
        {
            if (collsion.contacts[0].normal.y > 0.7f)
                isJumping = false;

        }
        void OnCollisionExit2D(Collision2D collision)
        {
            isJumping = true;
        }
        */
        //Landing Platform
        if(rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    isJumping = false;
            }
        }
    }
}
