using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public int hp;
    public int power;
    public int defensePower;
    public int dashSpeed;
    public int moveSpeed;
    public int moveDelaytime;
    public int juniorcallDelaytime;
    public bool ismoveDelay;
    public bool isjunorcallDelay;
    public Vector2 direction;
    public Rigidbody2D rigidBody;
    public Transform transform;
    public GameObject junior;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        junior = Resources.Load<GameObject>("Prefabs/Junior");
        ismoveDelay = true;
        isjunorcallDelay = true;
        hp = 3;

        StartCoroutine(RandomMove());
    }

    void Update()
    {
        /*if (!isjunorcallDelay)
        {
            Debug.Log("1");
            Instantiate(junior, new Vector2(transform.position.x, transform.position.y),
                Quaternion.identity);
            isjunorcallDelay = true;
        }
        else
            StartCoroutine(callJunior());*/
    }

    void FixedUpdate()
    {
        if (!ismoveDelay)
        {
            //�̵� �� ������
            rigidBody.AddForce(direction * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            ismoveDelay = true;

            StartCoroutine(RandomMove());
        }
    }

    //���� �̵� ����
    IEnumerator RandomMove()
    {
        yield return new WaitForSecondsRealtime(moveDelaytime);

        //�¿� ���� ����
        if (Random.Range(-1, 1) < 0)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        ismoveDelay = false;
    }

    //��� ��ȯ ����
    IEnumerator callJunior()
    {
        yield return new WaitForSecondsRealtime(juniorcallDelaytime);

        isjunorcallDelay = false;
        Debug.Log("2");
    }
}
