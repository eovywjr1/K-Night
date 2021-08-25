using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMonsterBase : MonoBehaviour
{
    public int hp;
    public int power;
    public int moveSpeed;

    public bool isMoveDelay;

    public Player player;

    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    protected Transform transform; //������ ��ġ

    public Vector2 direction;
    public Vector2 playerPosition;

    public RaycastHit2D raycastHit;

    //�ʿ��� ���� ����
    public void Initial()
    {
        player = FindObjectOfType<Player>();

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }

    //�ǰ� �޾��� �� �� ����
    public void Ondamaged(int power)
    {
        hp -= power;

        if (hp <= 0)//�׾��� ��
            this.gameObject.SetActive(false);
        else // ���׾������
        {
            rigidBody.AddForce(direction * -1, ForceMode2D.Impulse);
            StopCoroutine(blink());
            StartCoroutine(blink());
        }
    }
    IEnumerator blink()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    //�÷��̾� ��ġ ���� �� ���� ����
    public void PlayerPositionSave()
    {
        playerPosition = player.GetTransform().position;

        if (playerPosition.x > this.gameObject.transform.position.x)
        {
            direction = Vector2.right;
            spriteRenderer.flipX = true;
        }
        else
        {
            direction = Vector2.left;
            spriteRenderer.flipX = false;
        }
    }
    public void FindPlayer()
    {
        if (player.transform.position.x > this.gameObject.transform.position.x)
        {
            direction = Vector2.right;
            spriteRenderer.flipX = true;
        }
        else
        {
            direction = Vector2.left;
            spriteRenderer.flipX = false;
        }
    }
    //�ִ�ӵ����� �߰�
    public void Move(int speed, int maxSpeed)
    {
        if ((direction == Vector2.right && rigidBody.velocity.x < maxSpeed) || (direction == Vector2.left && rigidBody.velocity.x > maxSpeed * (-1)))
            rigidBody.AddForce(direction * speed * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword") && player.attackOnce)
        {
            Ondamaged(player.atkDamage);
            player.attackOnce = false;
        }
    }
}
