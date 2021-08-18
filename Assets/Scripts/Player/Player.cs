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

    public int hp; // 체력

    
    public int atkDamage; // player가 가하는 damage

    public string mapName;

    public float moveSpeed = 4f; // 이동속도
    public float jumpSpeed = 4f; // 점프속도
    public float dashSpeed = 30f; // 대쉬속도

    public int jumpCount = 1; // 점프 가능 횟수 

    public bool isJumping = false; // 점프상태
    public bool isdash = false; // 대쉬상태
    public bool isattack = false; // 공격상태
    public bool isSave;
    public bool isTalking = false;//대화중인가?

    Rigidbody2D rigid;

    Vector3 movement;
    private Animator animator;

    // npc 대사.
    private Vector3 directionPlayerLooksAt;
    public GameObject scannedTalker;
    public TalkManager talkManager;

    // NPC 인식.
    float rayLenOfLooking = 1.5f;
    RaycastHit2D rayHit;

    // 물과 충돌 시.

    // YesNo 관련 변수.
    public bool isYesNoOn;

    // Save Point 관련 변수.
    GameObject savePoint;

    // 엔딩 관련 변수.
    bool alreadyTriggeredFirstEnding = false;
    private GameObject cameraInThisScene;
    private bool passedFirstTalkTriggerInFirstEnding = false;

    private void Awake()
    {
        isYesNoOn = false;
        isTalking = false;
        hp = 100;

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
        FindTalkManager();
        CheckIsInEnding();
        TalkerFinder();
        
        if(SceneManager.GetActiveScene().name == "TitleScreen")
        {
            hp = 100;
        }


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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Water")
        {
            hp = 0;
        }
        if (collider.gameObject.tag == "SavePoint")
        {
            talkManager.SaveGame();
        }
        //Castle_BossRoom_BeforeCombat
        //StartTalk콜리더에 들어갈시 대사 이벤트 발생
        if(collider.gameObject.layer == 15 && collider.GetComponent<ObjTalkData>().talkId == 550)
        {
            talkManager.TriggerTalks(scannedTalker);
        }
    }

    bool once = false;
    bool once2 = false;
    bool once3 = false;
    bool once4 = false;

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Castle_BossRoom_AfterMagician
        //StartTalk콜리더에 들어갈시 대사 이벤트 발생
        if (collider.gameObject.layer == 15 && collider.GetComponent<ObjTalkData>().talkId == 600 && once == false)
        {
            once = true;
            talkManager.TriggerTalks(scannedTalker);
        }
        if (collider.gameObject.layer == 15 && collider.GetComponent<ObjTalkData>().talkId == 601 && once2 == false)
        {
            once2 = true;
            talkManager.TriggerTalks(scannedTalker);
        }
        if (collider.gameObject.layer == 15 && collider.GetComponent<ObjTalkData>().talkId == 700 && once3 == false)
        {
            once3 = true;
            talkManager.TriggerTalks(scannedTalker);
        }
        if (collider.gameObject.layer == 15 && collider.GetComponent<ObjTalkData>().talkId == 800 && once4 == false)
        {
            once4 = true;
            talkManager.TriggerTalks(scannedTalker);
        }
    }


    private void FixedUpdate()
    {
        if (isTalking == false)
        {
            Move();
            Jump();
            Dash();
            Attack();
        }

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

        if (scannedTalker != null)
        {
            if (
                ( // npc에게 말 걸기.
                 (Input.GetKeyDown(KeyCode.UpArrow) && scannedTalker != null)
                 ||
                 (talkManager.talkIndex >= 1 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                ) && (isYesNoOn == false) && passedFirstTalkTriggerInFirstEnding == false)
            {
                talkManager.TriggerTalks(scannedTalker);
                
            }
            else if (
                ( // npc에게 말 걸기.
                 (Input.GetKeyDown(KeyCode.UpArrow) && scannedTalker != null)
                 ||
                 (talkManager.talkIndex >= 1 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                ) && (isYesNoOn == true))
            {

            }
        }
    }

    // 이 함수를 통해서 Player는 "Talker" 레이어의 오브젝트를 감지합니다.
    void TalkerFinderInAwakeAndFixedUpdate()
    {
        // Ray.
        Debug.DrawRay(rigid.position, directionPlayerLooksAt * rayLenOfLooking, new Color(0, 255 / 255f, 0, 255 / 255f));
        rayHit = Physics2D.Raycast(rigid.position, directionPlayerLooksAt, rayLenOfLooking, LayerMask.GetMask("Talker"));

        if (rayHit.collider != null)
        {
            scannedTalker = rayHit.collider.gameObject;
        }
        else if(rayHit.collider == null)
        {
            scannedTalker = null;
        }

    }

    void FindTalkManager()
    {
        if (talkManager == null)
        {
            Debug.Log("asdf");
            talkManager = GameObject.Find("TalkManager").GetComponent<TalkManager>();
        }
    }
   










    // 스토리용 함수. 엔딩.
    void CheckIsInEnding()
    {
        if (SceneManager.GetActiveScene().name == "Village_FirstEnding" || SceneManager.GetActiveScene().name == "Village_SecondEnding")
        {
            if (alreadyTriggeredFirstEnding == false)
            {
                passedFirstTalkTriggerInFirstEnding = true;
            }
            cameraInThisScene = GameObject.Find("Main Camera");
            isTalking = true;
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }


        if( (SceneManager.GetActiveScene().name == "Village_FirstEnding" || SceneManager.GetActiveScene().name == "Village_SecondEnding") && alreadyTriggeredFirstEnding == false && cameraInThisScene.transform.position.x <= 5.0f)
        {
            passedFirstTalkTriggerInFirstEnding = false;
            TriggerFirstEnding();
            alreadyTriggeredFirstEnding = true;
        }



    }



    // 스토리용 함수. 엔딩.
    void TriggerFirstEnding()
    {
        // Ray.
        Debug.DrawRay(rigid.position, directionPlayerLooksAt * rayLenOfLooking, new Color(0, 255 / 255f, 0, 255 / 255f));
        rayHit = Physics2D.Raycast(rigid.position, directionPlayerLooksAt, rayLenOfLooking, LayerMask.GetMask("Talker"));

        if (rayHit.collider != null)
        {
            scannedTalker = rayHit.collider.gameObject;
        }
        else if (rayHit.collider == null)
        {
            scannedTalker = null;
        }


        talkManager.TriggerTalks(scannedTalker); // talkID 350의 대사가 실행되도록 하려는 의도입니다.
    }
}
