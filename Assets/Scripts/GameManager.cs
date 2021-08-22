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

    public Player player;
    float x;
    float y;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (player.isSave)
            Save(player);

        //aftertutorial���� beforecombet ������ �̵��ϴ� ������Ʈ Ȱ��ȭ
        if (barricade != null && barricade.activeSelf == false)
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
        if (PlayerPrefs.HasKey("playerX") == true)
        {
            x = PlayerPrefs.GetFloat("playerX");
            y = PlayerPrefs.GetFloat("playerY");
            player.transform.position = new Vector3(x, y, 0);
            SceneManager.LoadScene(PlayerPrefs.GetString("sceneName"));
            player.myName = PlayerPrefs.GetString("playerName");
            player.hp = PlayerPrefs.GetInt("Hp");
        }
    }
}