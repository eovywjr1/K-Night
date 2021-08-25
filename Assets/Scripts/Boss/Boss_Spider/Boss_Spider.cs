using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spider: Boss_form
{

    Vector3 startPoint;
    protected override void Start()
    {
        base.Start();
        //dead = true;
        doDash = false;

        Skills();
    }
    //스킬 사용
    void Skills()
    {
        //좌우 확인
        FindPlayer();
        if (inRange)
        {
            startPoint = transform.position;
            //뒤로 잠시 이동 후 대쉬
            StartCoroutine(UseDash());
        }
        else if (!inRange)
        {   //좌우 확인
            FindPlayer();
            ThrowStones();
            Invoke(nameof(ThrowStones), 0.5f);
            Invoke(nameof(ThrowStones), 1f);
            Debug.Log("다 던졌어");
            StartCoroutine(AttackEnd(1.5f));
        }
    }
    IEnumerator UseDash()
    {
        rigid.AddForce(direction * -4, ForceMode2D.Impulse);
        yield return new WaitForSeconds(3f);
        doDash = true;
        Dash(dashSpeed);
        yield return new WaitForSeconds(1f);
        Skills();
    }
    IEnumerator AttackEnd(float time)
    {
        yield return new WaitForSeconds(time);
        Skills();
    }
    private void FixedUpdate()
    {
        InRange();
        Debug.DrawRay(transform.position, direction*RangeDistance, Color.red);
        //대쉬 그만!
        if (doDash)
        {
            if(CalculateDistance(startPoint, transform.position) >= RangeDistance * 2)
            {
                rigid.velocity = Vector2.zero;
                doDash = false;
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && doDash)
        {
            Debug.Log("대쉬 맞음");
            player.HpDecrease(damage_Dash);
        }

        /* 몸빵데미지
        else if (collision.gameObject.CompareTag("Player") && !doDash)
        {
            Debug.Log("몸빵 맞음");
            player.HpDecrease(damage_Touch);
        }*/
    }
}
