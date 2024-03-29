﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_form : LivingEntity
{
    //PLAYER
    protected Player player;

    //스킬 PREFAB
    public GameObject stonePrefab; //스킬(돌)
    public GameObject energyBallPrefab; //스킬(에너지볼)
    public GameObject meteoPrefab; //스킬(메테오)
    public GameObject meteoWarningPrefab; //스킬(메테오 경고)

    private float floatRnd; //실수형 난수
    private Vector3 pos;

    //데미지
    public int damage_EnergyBall; //에너지 볼 데미지
    public int damage_Meteo; //메테오 데미지
    public int damage_Dash; //대쉬 데미지
    public int damage_Stone; //던지기 데미지
    public int damage_Touch; //닿았을때 데미지

    //스킬 관련
    public float dashSpeed; //대쉬 속도
    public float throwSpeed; //던지기 공격 속도
    public float energyBallSpeed; //에너지 볼 속도
    public float meteoGravity; // 메테오 속도
    public float warningTime; // 메테오 경고 시간

    protected bool doDash; //대쉬중인가?

    //메테오 관련 변수들
    protected int meteoPosY;
    protected int meteoPosX_min;
    protected int meteoPosX_max;
    protected int numOfMeteo;

    //범위
    public float RangeDistance; //범위 거리
    public bool inRange; //범위 안 or 밖?

    //보스 특징 관련
    public int numOfTorchOff;

    //TIMER
    public float timerStartTime;
    public GameObject Timer;
    public bool refill = false;

    /////////////////////////////////////
    ////////////////SETTING//////////////
    /////////////////////////////////////
    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
    }
    protected virtual void Start()
    {
        //메테오 관련
        meteoPosY = 2;
        meteoPosX_min = -30;
        meteoPosX_max = 30;

}
    public void FindPlayer() //플레이어의 위치 파악 (좌, 우)
    {
        direction = player.transform.position.x <= transform.position.x ? Vector3.left : Vector3.right;
        Debug.Log(direction);
        if (direction == Vector3.left) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }
    /////////////////////////////////////
    ////////////////SKILLS///////////////
    /////////////////////////////////////
    // 대쉬 특정 위치 도착후 정지하는 로직 필요
    protected void Dash(float dashSpeed)
    {
        rigid.AddForce(direction* dashSpeed, ForceMode2D.Impulse);
    }
    protected void ThrowStones()
    {
        floatRnd = Random.Range(-1f, 1.1f);
        pos = transform.position + Vector3.up * (5 + floatRnd) + Vector3.right * floatRnd;
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
        GameObject MeteoWarningClone = Instantiate(meteoWarningPrefab, pos, this.transform.rotation);
    }
    /////////////////////////////////////
    /////////////MORE DETAILS////////////
    /////////////////////////////////////
    //플레이어가 범위안에 있는가?
    protected void InRange()
    {
        if (CalculateDistance(transform.position, player.GetComponent<Transform>().position) < RangeDistance)
            inRange = true;
        else inRange = false;
    }
    //거리계산
    protected float CalculateDistance(Vector2 pos1, Vector2 pos2)
    {
        return Vector2.Distance(pos1, pos2);
    }
}

