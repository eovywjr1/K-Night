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

    //메테오 관련 변수들
    public int meteoPosY;
    public int meteoPosX_min;
    public int meteoPosX_max;
    public int numOfMeteo;
    private int[] xList = new int[10]; //numOfMeteo
    private Vector3[] posList = new Vector3[10]; //numOfMeteo


    //범위
    public float RangeDistance; //범위 거리
    public bool inRange; //범위 안 or 밖?

    private int rnd;

    void Start()
    {
        Skills();
    }
    ///////////////
    ///스킬 사용///
    ///////////////
    void Skills()
    {
        Meteo();
        //좌우 확인
        FindPlayer();

        //범위 체크
        if (!inRange)
        {
            Dash(dashSpeed);
        }
        else if (inRange)
        {
            rnd = Random.Range(1, 3);
            FindPlayer();
            switch (rnd)
            {
                case 1:
                    EnergyBall();
                    break;
                case 2:
                    ThrowStones();
                    Invoke(nameof(ThrowStones), 0.5f);
                    Invoke(nameof(ThrowStones), 1f);
                    break;
            }
        }
        Invoke(nameof(Skills), attackDelay);
    }
    ////////////////
    /////메테오/////
    ////////////////
    void Meteo()
    {
        UseMeteo();
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

    private void FixedUpdate()
    {
        InRange();
        Debug.DrawRay(transform.position + Vector3.left * RangeDistance, Vector3.right *2 * RangeDistance, Color.red);
    }
    //플레이어가 범위안에 있는가?
    private void InRange()
    {
        if (CalculateDistance(transform.position, player.GetComponent<Transform>().position) < RangeDistance)
            inRange = true;
        else inRange = false;
    }
    //거리계산
    private float CalculateDistance(Vector2 pos1, Vector2 pos2)
    {
        return Vector2.Distance(pos1, pos2);
    }
}
