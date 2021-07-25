﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spider: Boss_form
{
    public float attackDelay; //공격 딜레이
    public float attackDelay_Dash; //공격 딜레이
    public float attackDelay_Throw; //공격 딜레이

    public bool inRange; //범위 안 or 밖?
    public bool canAttack_Dash; //공격 가능 여부(스킬 쿨타임)
    public bool canAttack_Throw; //공격 가능 여부(스킬 쿨타임)
    private float lastAttackTime_Dash; //마지막 공격 시점
    private float lastAttackTime_Throw; //마지막 공격 시점


    private void Start()
    {
        inRange = false;
        lastAttackTime_Dash = 0f;
        lastAttackTime_Throw = 0f;
        Skills();
        Invoke(nameof(Skills),attackDelay);
    }
    void Skills() //스킬 사용
    {
        //쿨타임 여부(Dash)
        if (lastAttackTime_Dash + attackDelay_Dash <= Time.time)
        {
            canAttack_Dash = true;
            lastAttackTime_Dash = Time.time;
        }
        else canAttack_Dash = false;
        //쿨타임 여부(Throw)
        if (lastAttackTime_Throw + attackDelay_Throw <= Time.time)
        {
            canAttack_Throw = true;
            lastAttackTime_Throw = Time.time;
        }
        else canAttack_Throw = false;

        //좌우 확인
        FindPlayer();

        //범위 체크
        if (inRange && canAttack_Dash)
        {
            Dash();
            attackDelay = attackDelay_Dash;
        }
        else if(!inRange && canAttack_Throw)
        {
            ThrowStones ();
            Invoke(nameof(ThrowStones), 0.5f);
            Invoke(nameof(ThrowStones), 1f);
            attackDelay = attackDelay_Throw;
        }
        Invoke(nameof(Skills),attackDelay);
    }
    
    //플레이어가 범위안에 있는가?
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            inRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            inRange = false;
    }

    private void FixedUpdate()
    {//부딪혔을때 데미지
        //Collision으로 했을 경우 inRange에 영향이 가서 Ray로 했음
        Debug.DrawRay(rigid.position + Vector2.left/4, Vector3.right/2, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
        {
            GetDamage(damage_Dash);
        }
    }
}