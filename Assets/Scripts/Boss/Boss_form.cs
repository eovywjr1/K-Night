﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_form : LivingEntity
{
    //PLAYER
    public GameObject player;

    //스킬 PREFAB
    public GameObject stonePrefab; //스킬(돌)
    public GameObject energyBallPrefab; //스킬(에너지볼)
    public GameObject meteoPrefab; //스킬(메테오)
    public GameObject meteoWarningPrefab; //스킬(메테오 경고)

    protected Rigidbody2D rigid; //보스 리지드바디
    private SpriteRenderer spriteRenderer; //좌우 뒤집기 위해 // 임시
    private new Transform transform; //보스의 위치

    public Vector3 direction;

    private float floatRnd; //실수형 난수
    private float lastDamagedTime; //마지막 데미지 받은 시점
    public float damagedDelay;//플레이어 무적시간 //여기 있을건 아닌듯
    private bool canDamaged; //플레이어 피격 가능한가?
    private Vector3 pos;

    //데미지
    public int damage_EnergyBall; //에너지 볼 데미지
    public int damage_Meteo; //메테오 데미지
    public int damage_Dash; //대쉬 데미지
    public int damage_Stone; //던지기 데미지

    //스킬 관련
    public float dashSpeed; //대쉬 속도
    public float throwSpeed; //던지기 공격 속도
    public float energyBallSpeed; //에너지 볼 속도
    public float meteoGravity; // 메테오 속도
    public float warningTime; // 메테오 경고 시간

    //TIMER
    public float timerStartTime;

    /////////////////////////////////////
    ////////////////SETTING//////////////
    /////////////////////////////////////
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = gameObject.GetComponent<Transform>();
    }
    public void FindPlayer() //플레이어의 위치 파악 (좌, 우)
    {
        player = GameObject.Find("Player");
        direction = player.transform.position.x <= base.transform.position.x ? Vector3.left : Vector3.right;
    }
    /////////////////////////////////////
    ////////////////SKILLS///////////////
    /////////////////////////////////////
    protected void Dash(float dashSpeed)
    {
        rigid.AddForce(direction* dashSpeed, ForceMode2D.Impulse);
    }
    protected void ThrowStones()
    {
        floatRnd = Random.Range(-1f, 1.1f);
        pos = transform.position + Vector3.up * (4 + floatRnd) + Vector3.right * floatRnd;
        GameObject stoneClone = Instantiate(stonePrefab, pos, base.transform.rotation);
    }
    protected void EnergyBall()
    {
        GameObject energyBallClone = Instantiate(energyBallPrefab, transform.position + (direction * 0.5f), transform.rotation);
    }
    protected void Meteo(Vector3 pos)
    {
        GameObject MeteoClone = Instantiate(meteoPrefab, pos, transform.rotation);
    }
    protected void MeteoWarning(Vector3 pos)
    {
        GameObject MeteoWarningClone = Instantiate(meteoWarningPrefab, pos, transform.rotation);
    }


    /*
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
    }*/
}

