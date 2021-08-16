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
            //ÀúÀå bool
            player.isSave = true;

            //¸Ê ÀÌµ¿
            MapTransfer();
        }

        //¸ÅÁö¼Ç ¾À ½ºÅÂÇÁ °ü·Ã ¸Ê ÀÌµ¿
        if(staff != null && magician != null && magician.isStaff)
        {
            player.isSave = true;

            magician.isStaff = false;

            //¸Ê ÀÌµ¿
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
        //¸Ê ÀÌµ¿
        player.mapName = SceneManager.GetActiveScene().name + '>' + mapName + "StartPoint";
        SceneManager.LoadScene(mapName);
    }
}