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
    public LivingEntity BOSS;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(boss != null)
        {
            if (boss.activeSelf == false)
            {
                //저장 bool
                player.isSave = true;

                //맵 이동
                MapTransfer();
            }
        }

        //매지션 씬 스태프 관련 맵 이동
        if (magician != null)
        {
            if (magician.isStaff)
            {
                player.isSave = true;

                magician.isStaff = false;

                //맵 이동
                MapTransfer();
            }
        }

        //나머지 보스들
        if (BOSS != null)
        {
            if (BOSS.dead)
            {
                //저장 bool
                player.isSave = true;
                
                //맵 이동
                MapTransfer();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            MapTransfer();
    }

    void MapTransfer()
    {
        //맵 이동
        player.mapName = SceneManager.GetActiveScene().name + '>' + mapName + "StartPoint";
        SceneManager.LoadScene(mapName);
    }
}