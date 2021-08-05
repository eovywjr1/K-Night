using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spider_Reprise : Boss_form
{
    /// 설명
    /// 메테오를 주기적으로 사용
    /// 불이 켜져있는 횃불에서 새끼가 나와서 에너지볼 발사

    //보스의 공격 딜레이
    public float attackDelay; //보스의 공격 딜레이
    private float lastAttackTime; //보스의 마지막 공격 시점
    private bool canAttack; //보스의 공격 가능 여부(스킬 쿨타임)

    //메테오 딜레이
    public float meteoDelay = 5f;
    private float lastMeteoTime;
    private bool canMeteo;


    //메테오 관련 변수들
    public int meteoPosY;
    public int meteoPosX_min;
    public int meteoPosX_max;
    public float warningTime;
    public int numOfMeteo;
    public int numOfTorchOff;
    private int[] xList = new int[10]; //numOfMeteo
    private Vector3[] posList = new Vector3[10]; //numOfMeteo

    //데미지
    public int damage_EnergyBall; //에너지 볼 데미지
    public int damage_Meteo; //메테오 데미지

    //스킬의 속도
    public float energyBallSpeed; //에너지 볼 속도
    public float meteoGravity; //메테오 중력


    private int rnd;

    void Start()
    {
        numOfTorchOff = 0;
        lastAttackTime = 0f;
        lastMeteoTime = 0f;
        Meteo();
        Skills();
    }
    ////////////////
    /////메테오/////
    ////////////////
    void Meteo()
    {
        UseMeteo();
        Invoke(nameof(Meteo), meteoDelay);
    }
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
            Meteo(posList[i] + new Vector3(0, meteoWarningPrefab.transform.localScale.y / 2, 0));
        }
    }
    // 메테오 위치 설정
    void makeVec()
    {
        GetRandomInt(numOfMeteo, meteoPosX_min / 3, meteoPosX_max / 3);
        for (int i = 0; i < numOfMeteo; i++)
        {
            posList[i] = new Vector3(xList[i] * 3, meteoPosY, -1);
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
    ///////////////
    ///스킬 사용///
    ///////////////
    void Skills()
    {

    }
}
