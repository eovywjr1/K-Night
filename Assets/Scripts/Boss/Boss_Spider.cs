using UnityEngine;

public class Boss_Spider: Boss_form
{

    //보스의 공격 딜레이
    private float attackDelay; //공격 딜레이
    public float attackDelay_Dash; //대쉬공격 딜레이
    public float attackDelay_Throw; //던지기공격 딜레이
    private bool canAttack_Dash; //공격 가능 여부(스킬 쿨타임)
    private bool canAttack_Throw; //공격 가능 여부(스킬 쿨타임)
    private float lastAttackTime_Dash; //마지막 공격 시점
    private float lastAttackTime_Throw; //마지막 공격 시점

    //상태
    //private bool doDash;

    //레이어
    int playerLayer, bossLayer;
    private void Start()
    {
        /*
        playerLayer = LayerMask.NameToLayer("Player");
        bossLayer = LayerMask.NameToLayer("Boss");
        */
        inRange = false;
        lastAttackTime_Throw = 0f;
        Skills();
        Invoke(nameof(Skills),attackDelay);
    }

    //스킬 사용
    void Skills() 
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
            Dash(dashSpeed);
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
        //if (rigid.velocity == Vector2.zero) doDash = false;
        //else doDash = true;
        InRange();
        //Physics2D.IgnoreLayerCollision(6, 7);
        Debug.DrawRay(transform.position, direction*RangeDistance, Color.red);

        /*
        //충돌 무시
        Physics2D.IgnoreLayerCollision(playerLayer, bossLayer,true);
        */
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.HpDecrease(damage_Dash);
        }
    }
   
}
