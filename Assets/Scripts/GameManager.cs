using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject boss;
    public GameObject barricade;
    public GameObject mapTransfer;

    public Boss_Spider bossSpider;

    float x;
    float y;

    void Start()
    {
        //�÷��̾ Ʃ�丮�� ���� ������ �̵� �� ���� ����
        if (boss != null && SceneManager.GetActiveScene().name == "TutorialBoss")
            boss.SetActive(true);
    }

    void Update()
    {
        //aftertutorial���� beforecombet ������ �̵��ϴ� ������Ʈ Ȱ��ȭ
        if (barricade != null && barricade.activeSelf == false)
            mapTransfer.SetActive(true);

        //������ �׾��� �� ������Ʈ Ȱ��ȭ
        if(boss != null && boss.activeSelf == false)
            mapTransfer.SetActive(true);

        //spider ������ �׾��� �� ������Ʈ Ȱ��ȭ
        if (bossSpider != null && !boss.activeSelf && bossSpider.dead)
            mapTransfer.SetActive(true);
    }

    //����
    public void Save(Player player)
    {
        PlayerPrefs.SetString("playerName", player.myName);
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("Hp", player.hp);
        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
    }

    //�ҷ�����
    public void Load(Player player)
    {
        x = PlayerPrefs.GetFloat("playerX");
        y = PlayerPrefs.GetFloat("playerY");
        player.transform.position = new Vector3(x, y, 0);
        SceneManager.LoadScene(PlayerPrefs.GetString("sceneName"));
        player.myName = PlayerPrefs.GetString("playerName");
        player.hp = PlayerPrefs.GetInt("Hp");
    }
}