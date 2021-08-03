using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tutorialBossPrefab;
    public Player player;

    public string playerMapName;

    void Start()
    {
        player = FindObjectOfType<Player>();
        playerMapName = player.mapName.Split('_')[1];
    }

    void Update()
    {
        //플레이어가 보스 씬으로 이동 시 보스 생성
        if (playerMapName == "Tutorial")
        {
            tutorialBossPrefab = Resources.Load<GameObject>("Prefabs/Tutorial/Boss");
            Instantiate(tutorialBossPrefab, new Vector3(2, 2), Quaternion.identity);
        }
    }
}
