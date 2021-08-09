using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string mapName;
    public string thisSceneName;

    public Player player;
    public GameObject boss;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(boss.activeSelf == false)
        {
            player.mapName = SceneManager.GetActiveScene().name + '>' + mapName + "StartPoint";
            SceneManager.LoadScene(mapName);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.mapName = thisSceneName + '>' + mapName + "StartPoint";
            SceneManager.LoadScene(mapName);
        }
    }
}