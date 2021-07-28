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

    private Vector3 directionPlayerLooksAt;
    private GameObject scannedTalker;
    public TalkManager talkManager;

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

        // NPC 대화 (임시).
        TalkerFinder();

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

        FindingTalker();
    }





















    
    void TalkerFinder()
    {
        
        // 플레이어가 발사하는 Ray의 벡터 조절.
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            directionPlayerLooksAt = new Vector3(1, 0, 0);
        }
        else if(Input.GetAxisRaw("Horizontal") == -1)
        {
            directionPlayerLooksAt = new Vector3(-1, 0, 0);
        }

        if  (
            (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") == 1 && scannedTalker != null)
            ||
            ( talkManager.talkIndex >= 1 && (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Jump")) )
            )
        {
            talkManager.TriggerTalks(scannedTalker);
        }

    }

    void FindingTalker()
    {
        // 이 함수를 통해서 Player는 "Talker" 레이어의 오브젝트를 감지합니다.
        float rayLenOfLooking = 0.5f;

        // Ray.
        Debug.DrawRay(rigid.position, directionPlayerLooksAt * rayLenOfLooking, new Color(0, 255/255f, 0, 255/255f));
        RaycastHit2D rayHit
            = Physics2D.Raycast(rigid.position, directionPlayerLooksAt, rayLenOfLooking, LayerMask.GetMask("Talker"));

        if(rayHit.collider != null)
        {
            scannedTalker = rayHit.collider.gameObject;
        }
        else
        {
            scannedTalker = null;
        }


    }






}
