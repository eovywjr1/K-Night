using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Magician : Boss_form
{
    /// 설명
    /// 보스는 가운데에 위치함
    /// 에너지볼 : 플레이어의 방향으로 발사
    /// 메테오 : 랜덤한 위치에 5개 생성
    /// 특정 시간마다 랜덤한 스킬 사용
    /// 멀어질수록 보스의 공격 속도 증가
    /// 시간이 지날수록 보스의 공격 속도 증가

    //보스의 공격 딜레이
    [SerializeField]
    private float attackDelay; //보스의 공격 딜레이
    [SerializeField]
    private float tempDelay; //현재 적용되는 딜레이

    //메테오 관련 변수들
    public int meteoPosY;
    public int meteoPosX_min;
    public int meteoPosX_max;
    public int numOfMeteo;
    private int[] xList = new int[8]; //numOfMeteo
    private Vector3[] posList = new Vector3[8]; //numOfMeteo

    private bool playerCanAttack;// 보스를 때릴수 있다!

    public int numOfTorchOff;

    private int rnd;

    void Start()
    {
        numOfTorchOff = 0;
        Skills();
    }

    /////////////////////////////////////
    /////////////USING SKILLS////////////
    /////////////////////////////////////
    void Skills() // 스킬 사용
    {
        
        rnd = Random.Range(1, 3);
        switch (rnd)
        {
        case 1:
            FindPlayer();//좌우 확인
            EnergyBall();
            break;
        case 2:
            UseMeteo();
            break;
        }
        //멀어질수록 스킬 딜레이 감소
        tempDelay = attackDelay - Mathf.Abs(player.transform.position.x/40*attackDelay);
        if (tempDelay < 0.5f)
            tempDelay = 0.5f;
        Invoke(nameof(Skills), tempDelay);
        if(attackDelay >= 0.5f)
            attackDelay -= 0.1f;
    }
    //메테오
    private void UseMeteo()
    {
        StartCoroutine(CoMeteo(warningTime));

    }
    IEnumerator CoMeteo(float warningTime)
    {
        makeVec();
        for (int i = 0; i < numOfMeteo; i++)
        {
            MeteoWarning(posList[i]);
        }
        yield return new WaitForSeconds(warningTime);
        for (int i = 0; i < numOfMeteo; i++)
        {
            Meteo(posList[i] + new Vector3(0, meteoWarningPrefab.transform.localScale.y/2, 0));
        }
    }
    // 메테오 위치 설정
    void makeVec()
    {
        GetRandomInt(numOfMeteo, meteoPosX_min/3, meteoPosX_max/3);
        for (int i = 0; i < numOfMeteo; i++)
        {
            posList[i] = new Vector3(xList[i]*3, meteoPosY, -1);
        }
    }

    //중복없는 난수 생성
    public void GetRandomInt(int length, int min, int max)
    {
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                xList[i] = Random.Range(min, max);
                isSame = false;

                for (int j = 0; j < i; ++j)
                {
                    if (xList[j] == xList[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
    }
    /////////////////////////////////////
    /////////////보스 특징 관련//////////
    /////////////////////////////////////
    public GameObject Timer;
    private void Update()
    {
        //torch 여부
        if (numOfTorchOff == 6)
        {
            Debug.Log("15초간 보스에게 타격 가능");
            playerCanAttack = true;
            if(Timer.GetComponent<TimeCountdown>().TimeEnd == false) Timer.SetActive(true);
            //타이머가 꺼지면 초기화
            if (numOfTorchOff == 6 && Timer.GetComponent<TimeCountdown>().TimeEnd)
            {
                Timer.GetComponent<TimeCountdown>().TimeEnd = false;
                Timer.GetComponent<TimeCountdown>().TimeCost = timerStartTime;
                numOfTorchOff = 0;
                playerCanAttack = false;
            }
        }
    }
}
