using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_form : LivingEntity
{
    public GameObject player; //플레이어
    public GameObject stonePrefab; //스킬(돌) Prefab
    public GameObject energyBallPrefab; //스킬(에너지볼) Prefab
    public GameObject meteoPrefab; //스킬(메테오) prefab
    public GameObject meteoWarningPrefab; //스킬(메테오 경고) prefab

    protected Rigidbody2D rigid; //보스 리지드바디
    private SpriteRenderer spriteRenderer; //좌우 뒤집기 위해 // 임시
    private new Transform transform; //보스의 위치
    private Animator animator; //보스 애니메이터


    private float floatRnd; //실수형 난수
    public Vector3 direction;

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
    public void FindPlayer() //플레이어의 위치 파악 (좌, 우)
    {
        player = GameObject.Find("Player");
        direction = player.transform.position.x <= base.transform.position.x ? Vector3.left : Vector3.right;
    }
    /////////////////////////////////////
    ////////////////SKILLS///////////////
    /////////////////////////////////////
    protected void Dash()
    {
        rigid.AddForce(direction* dashSpeed, ForceMode2D.Impulse);
    }
    protected void ThrowStones()
    {
        floatRnd = Random.Range(-1f, 1.1f);
        GameObject stoneClone = Instantiate(stonePrefab, transform.position + Vector3.up * (2 + floatRnd) + Vector3.right * floatRnd, base.transform.rotation);
    }
    protected void EnergyBall()
    {
        GameObject energyBallClone = Instantiate(energyBallPrefab, transform.position + direction * 0.5f, transform.rotation);
    }
    protected void Meteo(Vector3 pos)
    {
        GameObject MeteoClone = Instantiate(meteoPrefab, pos, transform.rotation);
    }
    protected void MeteoWarning(Vector3 pos)
    {
        GameObject MeteoWarningClone = Instantiate(meteoWarningPrefab, pos, transform.rotation);
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

