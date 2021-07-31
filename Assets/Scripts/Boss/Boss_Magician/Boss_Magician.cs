using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Magician : Boss_form
{
    // Start is called before the first frame update
    /// 설명
    /// 보스는 가운데에 위치함
    /// 에너지볼 : 플레이어의 방향으로 발사
    /// 메테오 : 랜덤한 위치에 5개 생성
    /// 특정 시간마다 랜덤한 스킬 사용
    public float attackDelay; //공격 딜레이
    public int meteoPosY;
    public float warningTime;
    public int numOfMeteo;
    public float meteoGravity;
    public int numOfTorchOff;

    private int[] xList = new int[5];
    private Vector3[] posList = new Vector3[5];

    private bool canAttack; //공격 가능 여부(스킬 쿨타임)
    private float lastAttackTime; //마지막 공격 시점
    private int rnd;

    void Start()
    {
        numOfTorchOff = 0;
        lastAttackTime = 0f;
        Skills();
        Invoke(nameof(Skills), attackDelay);
    }
    void Skills() //스킬 사용
    {
        //쿨타임 여부
        if (lastAttackTime + attackDelay <= Time.time)
        {
            canAttack = true;
            lastAttackTime = Time.time;
        }
        else canAttack = false;

        if (canAttack)
        {
            //랜덤으로 스킬 사용
            // 1 : 에너지볼, 2 : 메테오
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
        }
        Invoke(nameof(Skills), attackDelay);
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
            rnd = Random.Range(-10, -11);
            MeteoWarning(posList[i]);
        }
        yield return new WaitForSeconds(warningTime);
        for (int i = 0; i < numOfMeteo; i++)
        {
            rnd = Random.Range(-10, -11);
            Meteo(posList[i] + new Vector3(0, meteoWarningPrefab.transform.localScale.y/2, 0));
        }
    }
    void makeVec()
    {
        GetRandomInt(numOfMeteo, -8, 9);
        for (int i = 0; i < numOfMeteo; i++)
        {
            posList[i] = new Vector3(xList[i], 0, 0);
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
    //torch 여부
    private void Update()
    {
        if(numOfTorchOff == 6)
        {
            Debug.Log("15초간 보스에게 타격 가능");
            numOfTorchOff = 0;
        }
    }

}
