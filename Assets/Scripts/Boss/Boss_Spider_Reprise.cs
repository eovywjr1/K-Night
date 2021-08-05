using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spider_Reprise : Boss_form
{
    /// ����
    /// ���׿��� �ֱ������� ���
    /// ���� �����ִ� ȶ�ҿ��� ������ ���ͼ� �������� �߻�

    //������ ���� ������
    public float attackDelay; //������ ���� ������
    private float lastAttackTime; //������ ������ ���� ����
    private bool canAttack; //������ ���� ���� ����(��ų ��Ÿ��)

    //���׿� ������
    public float meteoDelay = 5f;
    private float lastMeteoTime;
    private bool canMeteo;


    //���׿� ���� ������
    public int meteoPosY;
    public int meteoPosX_min;
    public int meteoPosX_max;
    public float warningTime;
    public int numOfMeteo;
    public int numOfTorchOff;
    private int[] xList = new int[10]; //numOfMeteo
    private Vector3[] posList = new Vector3[10]; //numOfMeteo

    //������
    public int damage_EnergyBall; //������ �� ������
    public int damage_Meteo; //���׿� ������

    //��ų�� �ӵ�
    public float energyBallSpeed; //������ �� �ӵ�
    public float meteoGravity; //���׿� �߷�


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
    /////���׿�/////
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
    // ���׿� ��ġ ����
    void makeVec()
    {
        GetRandomInt(numOfMeteo, meteoPosX_min / 3, meteoPosX_max / 3);
        for (int i = 0; i < numOfMeteo; i++)
        {
            posList[i] = new Vector3(xList[i] * 3, meteoPosY, -1);
        }
    }
    //�ߺ����� ���� ����
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
    ///��ų ���///
    ///////////////
    void Skills()
    {

    }
}
