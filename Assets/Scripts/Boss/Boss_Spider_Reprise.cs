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
    private int[] xList = new int[10]; //numOfMeteo
    private Vector3[] posList = new Vector3[10]; //numOfMeteo
    private Vector3 endPoint;

    private bool limitMagicSkills;// 보스가 마법공격 못함!

    private int rnd;

    void Start()
    {
        dead = true;
        FindPlayer(); // 플레이어가 왼쪽에 있는지 오른쪽에 있는지 확인
        doDash = true;
        Dash(dashSpeed); // 대쉬

        inRange = true;
        limitMagicSkills = false;
        numOfTorchOff = 0;
        Skills();
    }
    ///////////////
    ///스킬 사용///
    ///////////////
    void Skills(){
        if (!limitMagicSkills){ // 마법스킬 사용이 가능하면
            Meteo(); // 메테오 사용
        }
        FindPlayer(); // 플레이어가 왼쪽에 있는지 오른쪽에 있는지 확인

        //범위 확인
        if (!inRange){ // 범위 안에 없다면
            doDash = true;
            Dash(dashSpeed); // 대쉬
        }
        else if (inRange){ // 범위 안에 있다면
            rnd = Random.Range(1, 3); // 랜덤한 스킬 사용
            if (rnd == 1 && !limitMagicSkills){
                EnergyBall();
            }
            else if(rnd == 2){
                ThrowStones();
                Invoke(nameof(ThrowStones), 0.5f);
                Invoke(nameof(ThrowStones), 1f);
            }
        }
        Invoke(nameof(Skills), attackDelay);
    }
    ////////////////
    /////메테오/////
    ////////////////
    void Meteo(){
        UseMeteo();
    }
    private void UseMeteo(){
        StartCoroutine(CoMeteo(warningTime));
    }
    IEnumerator CoMeteo(float warningTime){
        makeVec();
        for (int i = 0; i < numOfMeteo; i++) {
            MeteoWarning(posList[i]);
        }
        yield return new WaitForSeconds(warningTime);
        for (int i = 0; i < numOfMeteo; i++){
            Meteo(posList[i] + new Vector3(0, meteoWarningPrefab.transform.localScale.y / 2, 0));
        }
    }
    // 메테오 위치 설정
    void makeVec(){
        GetRandomInt(numOfMeteo, meteoPosX_min / 3, meteoPosX_max / 3);
        for (int i = 0; i < numOfMeteo; i++){
            posList[i] = new Vector3(xList[i] * 3, meteoPosY, -1);
        }
    }
    //중복없는 난수 생성
    public void GetRandomInt(int length, int min, int max){
        bool isSame;

        for (int i = 0; i < length; ++i){
            while (true){
                xList[i] = Random.Range(min, max);
                isSame = false;

                for (int j = 0; j < i; ++j){
                    if (xList[j] == xList[i]){
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
    }

    private void FixedUpdate(){
        InRange(); // 범위 확인
        Debug.DrawRay(transform.position + Vector3.left * RangeDistance, Vector3.right *2 * RangeDistance, Color.red);
        if(rigid.velocity == Vector2.zero)
        {
            doDash = false;
        }
    }


    /////////////////////////////////////
    /////////////보스 특징 관련//////////
    /////////////////////////////////////
    private void Update()
    {
        //torch 여부
        if (numOfTorchOff == 6)
        {
            Debug.Log("마법 스킬 사용 불가!");
            limitMagicSkills = true;
            if (Timer.GetComponent<TimeCountdown>().TimeEnd == false)
            {
                Timer.SetActive(true);
            }
            //타이머가 꺼지면 초기화
            if (Timer.GetComponent<TimeCountdown>().TimeEnd)
            {
                refill = true;
            }
        }
        else if (numOfTorchOff == 0)
        {
            refill = false;
            if (refill == false)
            {
                Timer.GetComponent<TimeCountdown>().TimeEnd = false;
                Timer.GetComponent<TimeCountdown>().TimeCost = timerStartTime;
                limitMagicSkills = false;
            }
        }
    }
    /////////////////////////////////////
    //////////////피격 관련//////////////
    /////////////////////////////////////
    //대쉬를 할때 데미지 예를 들어 2이면 그냥 피격데미지는 1
    //돌던지기에 관련된 피격은 따로 있음
    //플레이어가 무적이 아닐동안
    //=> 플레이어가 피격되면 일정시간 동안 무적
    //임시 변수
    private void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && doDash)
        {
            Debug.Log("대쉬 맞음");
            player.HpDecrease(damage_Dash);
        }
        else if (collision.gameObject.CompareTag("Player") && !doDash)
        {
            Debug.Log("몸빵 맞음");
            player.HpDecrease(damage_Touch);
        }
    }
}
