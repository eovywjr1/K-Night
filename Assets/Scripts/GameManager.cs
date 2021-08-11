using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject boss;
    public GameObject barricade;
    public GameObject mapTransfer;

    void Start()
    {
        //�÷��̾ Ʃ�丮�� ���� ������ �̵� �� ���� ����
        if (boss != null)
            boss.SetActive(true);
    }

    void Update()
    {
        //aftertutorial���� beforecombet ������ �̵��ϴ� ������Ʈ Ȱ��ȭ
        if (barricade != null && mapTransfer != null && barricade.activeSelf == false)
            mapTransfer.SetActive(true);

        //tutorial �������� ������ �׾��� �� ������Ʈ Ȱ��ȭ
        if(boss != null && mapTransfer != null && boss.activeSelf == false)
            mapTransfer.SetActive(true);
    }
}
