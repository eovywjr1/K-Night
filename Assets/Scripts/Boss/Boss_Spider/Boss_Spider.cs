using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spider: Boss_form
{
    public float attackDelay; //공격 딜레이
    public float attackDelay_Dash; //공격 딜레이
    public float attackDelay_Throw; //공격 딜레이
    public float RangeDistance;

    public bool inRange; //범위 안 or 밖?
    public bool canAttack_Dash; //공격 가능 여부(스킬 쿨타임)
    public bool canAttack_Throw; //공격 가능 여부(스킬 쿨타임)
    private float lastAttackTime_Dash; //마지막 공격 시점
    private float lastAttackTime_Throw; //마지막 공격 시점


    private void Start()
    {
        inRange = false;
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
    private void FixedUpdate()
    {
        InRange();
        Physics2D.IgnoreLayerCollision(6, 7);
    }

    //플레이어가 범위안에 있는가?
    private void InRange()
    {
        if (CalculateDistance(transform.position, player.GetComponent<Transform>().position) < RangeDistance)
            inRange = true;
        else inRange = false;
    }
    private float CalculateDistance(Vector2 pos1, Vector2 pos2)
    {
        return Vector2.Distance(pos1,pos2);
    }
    /*private void OnCollisionEnter2D (Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GetDamage(damage_Dash);
        }
    }
    */
}
