using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : LivingEntity
{
    public int damage_Dash; //대쉬 데미지
    public int damage_Throw; //던지기 데미지
    public float dashSpeed; //대쉬 속도
    public float throwSpeed; //던지기 공격 속도
    public float attackDelay; //공격 딜레이
    public float attackDelay_Dash; //공격 딜레이
    public float attackDelay_Throw; //공격 딜레이
    public float damagedDelay;//플레이어 무적시간
    public string LR = "";//"left" or "right"

    public GameObject player;
    public GameObject stonePrefab;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Rigidbody2D stoneRigid;
    private Animator animator;

    private float rnd; //던지는 공의 위치 랜덤성 부여
    public bool inRange; //범위
    public bool canAttack_Dash; //공격 가능 여부(스킬 쿨타임)
    public bool canAttack_Throw; //공격 가능 여부(스킬 쿨타임)
    private float lastAttackTime_Dash; //마지막 공격 시점
    private float lastAttackTime_Throw; //마지막 공격 시점
    private float lastDamagedTime; //마지막 데미지 받은 시점
    private bool canDamaged; //플레이어가 맞을수 있음?



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }
    private void Start()
    {
        inRange = false;
        lastAttackTime_Dash = 0f;
        lastAttackTime_Throw = 0f;
        FindPlayer();
        Skills();
        Invoke(nameof(Skills),attackDelay);
    }
    void FindPlayer()
    {
        player = GameObject.Find("Player");
        LR = player.transform.position.x <= transform.position.x ? "left" : "right";
    }
    void Skills() //스킬 사용
    {
        //쿨타임 여부(Dash)
        if (lastAttackTime_Dash + attackDelay_Dash <= Time.time)
        {
            canAttack_Dash = true;
            lastAttackTime_Dash = Time.time;
        }
        else canAttack_Dash = false;
        //쿨타임 여부(Throw)
        if (lastAttackTime_Throw + attackDelay_Throw <= Time.time)
        {
            canAttack_Throw = true;
            lastAttackTime_Throw = Time.time;
        }
        else canAttack_Throw = false;

        //좌우 확인
        player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;
        if (playerPos.x < transform.position.x) LR = "left";
        else if (playerPos.x >= transform.position.x) LR = "right";

        //범위 체크
        if (inRange && canAttack_Dash)
        {
            Dash();
            attackDelay = attackDelay_Dash;
        }
        else if(!inRange && canAttack_Throw)
        {
            ThrowStones ();
            Invoke(nameof(ThrowStones), 0.5f);
            Invoke(nameof(ThrowStones), 0.5f);
            attackDelay = attackDelay_Throw;
        }
        Invoke(nameof(Skills),attackDelay);
    }
    void Dash()//스킬1
    {
        if (LR == "left")
            rigid.AddForce(Vector2.left * dashSpeed, ForceMode2D.Impulse);
        else if (LR == "right")
            rigid.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);
    }
    void ThrowStones()//스킬2
    {
        rnd = Random.Range(-1f,1.1f);
        GameObject stoneClone = Instantiate(stonePrefab, transform.position + Vector3.up * 2 + Vector3.up * rnd + Vector3.right * rnd, transform.rotation);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            inRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            inRange = false;
    }
    private void FixedUpdate()
    {//부딪혔을때 데미지
        Debug.DrawRay(rigid.position + Vector2.left/4, Vector3.right/2, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
        {
            
            GetDamage(damage_Dash);
        }
    }
    void GetDamage(float damage)
    {
        //플레이어 무적시간 고려
        if (lastDamagedTime + damagedDelay <= Time.time)
        {
            canDamaged = true;
            lastDamagedTime = Time.time;
        }
        else
        {
            canDamaged = false;
        }
        //데미지
        if (canDamaged)
        {
            Debug.Log("부딪힘");
        }
    }

}
