using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject boss;

    void Start()
    {
        //�÷��̾ Ʃ�丮�� ���� ������ �̵� �� ���� ����
        boss.SetActive(true);
    }
}
