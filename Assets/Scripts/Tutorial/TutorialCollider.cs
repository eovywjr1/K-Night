using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    TutorialMonsterBase tutorialMonsterBase;
    Player player;

    private void Start()
    {
        tutorialMonsterBase = transform.parent.gameObject.GetComponent<TutorialMonsterBase>();
        player = FindObjectOfType<Player>();
    }

    //�÷��̾� �浹 �� �� ����
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
            tutorialMonsterBase.Ondamaged(player.atkDamage);

        if (this.gameObject.layer == 6 && collision.CompareTag("Player"))
            tutorialMonsterBase.player.HpDecrease(tutorialMonsterBase.power);

    }
}
