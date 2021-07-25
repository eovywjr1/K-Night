using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_form : LivingEntity
{
    public GameObject player; //플레이어
    public GameObject stonePrefab; //스킬(돌) Prefab
    public GameObject energyBallPrefab; //스킬(에너지볼) Prefab

    protected Rigidbody2D rigid; //보스 리지드바디
    private Rigidbody2D stoneRigid; //스킬(돌)의 리지드 바디
    private SpriteRenderer spriteRenderer; //좌우 뒤집기 위해 // 임시
    private new Transform transform; //보스의 위치
    private Animator animator; //보스 애니메이터


    private float floatRnd; //실수형 난수
    private int intRnd; //정수형 난수
    public string LR = ""; // "left" or "right"

    public int damage_Dash; //대쉬 데미지
    public int damage_Throw; //던지기 데미지
    public float dashSpeed; //대쉬 속도
    public float throwSpeed; //던지기 공격 속도

    private float lastDamagedTime; //마지막 데미지 받은 시점
    public float damagedDelay;//플레이어 무적시간 //여기 있을건 아닌듯
    private bool canDamaged; //플레이어 피격 가능한가?

    /////////////////////////////////////
    ////////////////SETTING//////////////
    /////////////////////////////////////
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }
    public void FindPlayer() //플레이어의 위치 파악
    {
        player = GameObject.Find("Player");
        LR = player.transform.position.x <= base.transform.position.x ? "left" : "right";
    }
    /////////////////////////////////////
    ////////////////SKILLS///////////////
    /////////////////////////////////////
    protected void Dash()
    {
        if (LR == "left")
            rigid.AddForce(Vector2.left* dashSpeed, ForceMode2D.Impulse);
        else if (LR == "right")
            rigid.AddForce(Vector2.right* dashSpeed, ForceMode2D.Impulse);
    }
    protected void ThrowStones()
    {
        floatRnd = Random.Range(-1f, 1.1f);
        GameObject stoneClone = Instantiate(stonePrefab, base.transform.position + Vector3.up * 2 + Vector3.up * floatRnd + Vector3.right * floatRnd, base.transform.rotation);
    }
    protected void EnergyBall()
    {
        if (LR == "left")
        {
            GameObject stoneClone = Instantiate(energyBallPrefab, base.transform.position + Vector3.left * 0.5f, base.transform.rotation);
        }
        else if (LR == "right")
        {
            GameObject stoneClone = Instantiate(energyBallPrefab, base.transform.position + Vector3.left * 0.5f, base.transform.rotation);
        }
    }
    protected void Meteo()
    {

    }
    /////////////////////////////////////
    ////////////////OTHERS///////////////
    /////////////////////////////////////
    protected void GetDamage(float damage)
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

