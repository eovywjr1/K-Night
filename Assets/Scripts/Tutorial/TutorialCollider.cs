using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    TutorialMonsterBase tutorialMonsterBase;

    private void Start()
    {
        tutorialMonsterBase = transform.parent.gameObject.GetComponent<TutorialMonsterBase>();
    }

    //�÷��̾� �浹 �� �� ����
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.layer == 6 && collision.CompareTag("Player"))
            tutorialMonsterBase.player.HpDecrease(tutorialMonsterBase.power);
    }
}
