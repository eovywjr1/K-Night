using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//생명체로 동작할 게임 오브젝트들의 뼈대를 제공
//체력, 피해받음, 사망 기능, 사망 이벤트 제공

public class LivingEntity : MonoBehaviour
{
    public float startingHealth; //시작 체력
    public float health;//현재 체력
    public bool dead;//사망 상태

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid; //보스 리지드바디
    protected Transform transform; //보스의 위치

    //플레이어 위치(방향)
    public Vector3 direction;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    //생명체가 활성화될 떄 상태를 리셋
    protected virtual void OnEnable()
    {
        //사망하지 않은 상태로 시작
        dead = false;
        //체력을 시작 체력으로 초기화
        health = startingHealth;
    }

    //피해를 받는 기능
    public virtual void OnDamage(float damage)
    {
        //데미지만큼 체력 감소
        health -= damage; // health = health - damage;
        //체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (health <= 0 && !dead)
        {
            Die();
        }
        else // 안죽었을경우
        {
            if (SceneManager.GetActiveScene().name != "Boss_Magician")
                KnockBack();
            StartCoroutine(blink());
        }
    }
    void KnockBack()
    {
        transform.position += direction * -0.1f;
        //rigid.AddForce(direction * -0.5f, ForceMode2D.Impulse);
    }
    IEnumerator blink()
    {
        yield return new WaitForSeconds(0.7f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.7f);
        spriteRenderer.color = Color.white;
    }
    //사망 처리
    public virtual void Die()
    {
        dead = true;
    }

}