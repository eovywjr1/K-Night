using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    static public Player instance;
    /* 
    C: dash 
    X: attack 
    */

    public string myName; // player 이름 (만약 게임 시작시 입력받는다면)

    private int hp; // 체력

    
    public int atkDamage; // player가 가하는 damage

    public string mapName;

    public float moveSpeed = 4f; // 이동속도
    public float jumpSpeed = 4f; // 점프속도
    public float dashSpeed = 30f; // 대쉬속도

    public int jumpCount = 1; // 점프 가능 횟수 

    public bool isJumping = false; // 점프상태
    public bool isdash = false; // 대쉬상태
    public bool isattack = false; // 공격상태

    Rigidbody2D rigid;

    Vector3 movement;
    private Animator animator;

    // npc 대사.
    private Vector3 directionPlayerLooksAt;
    private GameObject scannedTalker;
    public TalkManager talkManager;

    private void Awake()
    {
        
        

    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Village_Present")
        {
            
        }

        if (instance == null) // 씬 이동하면서 캐릭터 복사하지 않기 위해 static 선언 후 대입
        {
            DontDestroyOnLoad(this.gameObject);

            
            rigid = gameObject.GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            jumpCount = 1;

            instance = this;
        }
        else
            Destroy(this.gameObject);
    }


    

    private void Update()
    {
        TalkerFinder();
        



        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount == 1)
            {
                isJumping = true;

                
                jumpCount = 0;


            }
        }


        if (Input.GetKeyDown(KeyCode.C))

        {
            isdash = true;
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            isattack = true;
        }

    }

    void OnCollisionEnter2D(Collision2D col)   // Ground tag에 닿으면 점프횟수 초기화 (다시 점프 가능하도록)
    { 

        if (col.gameObject.CompareTag("Ground"))
        {

            Debug.Log("isGround!");

            jumpCount = 1;

        }



    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        Dash();
        Attack();

        TalkerFinderInAwakeAndFixedUpdate();
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
        {
            
            return;
        }

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

        animator.SetBool("isdash", true);

        isdash = false;

    }

    void Attack() // 공격
    {
        if (!isattack)
            return;

        animator.SetBool("isattack", true);


        isattack = false;
    }

    public Transform GetTransform()
    {
        return this.gameObject.transform;
    }

    public void HpDecrease(int quantity)
    {
        hp -= quantity;
    }










    void TalkerFinder()
    {
        // 플레이어가 발사하는 Ray의 벡터 조절.
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            directionPlayerLooksAt = new Vector3(1, 0, 0);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            directionPlayerLooksAt = new Vector3(-1, 0, 0);
        }

        if ( // npc에게 말 걸기.
            (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") == 1 && scannedTalker != null)
            ||
            (talkManager.talkIndex >= 1 && (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Jump")))
           )
        {
            talkManager.TriggerTalks(scannedTalker);
        }
    }

    // 이 함수를 통해서 Player는 "Talker" 레이어의 오브젝트를 감지합니다.
    void TalkerFinderInAwakeAndFixedUpdate()
    {
        
        float rayLenOfLooking = 0.5f;

        // Ray.
        Debug.DrawRay(rigid.position, directionPlayerLooksAt * rayLenOfLooking, new Color(0, 255 / 255f, 0, 255 / 255f));
        RaycastHit2D rayHit
            = Physics2D.Raycast(rigid.position, directionPlayerLooksAt, rayLenOfLooking, LayerMask.GetMask("Talker"));

        if (rayHit.collider != null)
        {
            scannedTalker = rayHit.collider.gameObject;
        }
        else if(rayHit.collider == null)
        {
            scannedTalker = null;
        }
    }

    
   

}
