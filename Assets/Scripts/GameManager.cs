using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject boss;
    public GameObject barricade;
    public GameObject mapTransfer;

    float x;
    float y;

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

    public void Save(Player player)
    {
        PlayerPrefs.SetString("playerName", player.myName);
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
        Debug.Log(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("Hp", player.hp);
        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
    }

    public void Load(Player player)
    {
        x = PlayerPrefs.GetFloat("playerX");
        y = PlayerPrefs.GetFloat("playerY");

        player.transform.position = new Vector3(x, y, 0);
    }
}