using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string mapName;

    public Player player;
    public Boss_Magician magician;
    public GameObject boss;
    public GameObject staff;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(boss != null && boss.activeSelf == false)
        {
            //���� bool
            player.isSave = true;

            //�� �̵�
            MapTransfer();
        }

        //������ �� ������ ���� �� �̵�
        if(staff != null && magician != null && magician.isStaff)
        {
            player.isSave = true;

            magician.isStaff = false;

            //�� �̵�
            MapTransfer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            MapTransfer();
    }

    void MapTransfer()
    {
        //�� �̵�
        player.mapName = SceneManager.GetActiveScene().name + '>' + mapName + "StartPoint";
        SceneManager.LoadScene(mapName);
    }
}