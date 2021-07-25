using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_form : LivingEntity
{
    public GameObject player; //�÷��̾�
    public GameObject stonePrefab; //��ų(��) Prefab
    public GameObject energyBallPrefab; //��ų(��������) Prefab

    protected Rigidbody2D rigid; //���� ������ٵ�
    private Rigidbody2D stoneRigid; //��ų(��)�� ������ �ٵ�
    private SpriteRenderer spriteRenderer; //�¿� ������ ���� // �ӽ�
    private new Transform transform; //������ ��ġ
    private Animator animator; //���� �ִϸ�����


    private float floatRnd; //�Ǽ��� ����
    private int intRnd; //������ ����
    public string LR = ""; // "left" or "right"

    public int damage_Dash; //�뽬 ������
    public int damage_Throw; //������ ������
    public float dashSpeed; //�뽬 �ӵ�
    public float throwSpeed; //������ ���� �ӵ�

    private float lastDamagedTime; //������ ������ ���� ����
    public float damagedDelay;//�÷��̾� �����ð� //���� ������ �ƴѵ�
    private bool canDamaged; //�÷��̾� �ǰ� �����Ѱ�?

    /////////////////////////////////////
    ////////////////SETTING//////////////
    /////////////////////////////////////
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }
    public void FindPlayer() //�÷��̾��� ��ġ �ľ�
    {
        player = GameObject.Find("Player");
        LR = player.transform.position.x <= base.transform.position.x ? "left" : "right";
    }
    /////////////////////////////////////
    ////////////////SKILLS///////////////
    /////////////////////////////////////
    protected void Dash()
    {
        if (LR == "left")
            rigid.AddForce(Vector2.left* dashSpeed, ForceMode2D.Impulse);
        else if (LR == "right")
            rigid.AddForce(Vector2.right* dashSpeed, ForceMode2D.Impulse);
    }
    protected void ThrowStones()
    {
        floatRnd = Random.Range(-1f, 1.1f);
        GameObject stoneClone = Instantiate(stonePrefab, base.transform.position + Vector3.up * 2 + Vector3.up * floatRnd + Vector3.right * floatRnd, base.transform.rotation);
    }
    protected void EnergyBall()
    {
        if (LR == "left")
        {
            GameObject stoneClone = Instantiate(energyBallPrefab, base.transform.position + Vector3.left * 0.5f, base.transform.rotation);
        }
        else if (LR == "right")
        {
            GameObject stoneClone = Instantiate(energyBallPrefab, base.transform.position + Vector3.left * 0.5f, base.transform.rotation);
        }
    }
    protected void Meteo()
    {

    }
    /////////////////////////////////////
    ////////////////OTHERS///////////////
    /////////////////////////////////////
    protected void GetDamage(float damage)
    {
        //�÷��̾� �����ð� ���
        if (lastDamagedTime + damagedDelay <= Time.time)
        {
            canDamaged = true;
            lastDamagedTime = Time.time;
        }
        else
        {
            canDamaged = false;
        }
        //������
        if (canDamaged)
        {
            Debug.Log("�ε���");
        }
    }
}

