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
        //플레이어가 튜토리얼 보스 씬으로 이동 시 보스 생성
        if (boss != null && SceneManager.GetActiveScene().name == "TutorialBoss")
            boss.SetActive(true);
    }

    void Update()
    {
        //aftertutorial에서 beforecombet 씬으로 이동하는 오브젝트 활성화
        if (barricade != null && barricade.activeSelf == false)
            mapTransfer.SetActive(true);

        //보스가 죽었을 때 오브젝트 활성화
        if(boss != null && boss.activeSelf == false)
            mapTransfer.SetActive(true);

        //spider 보스가 죽었을 때 오브젝트 활성화
        if (bossSpider != null && !boss.activeSelf && bossSpider.dead)
            mapTransfer.SetActive(true);
    }

    //저장
    public void Save(Player player)
    {
        PlayerPrefs.SetString("playerName", player.myName);
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("Hp", player.hp);
        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
    }

    //불러오기
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