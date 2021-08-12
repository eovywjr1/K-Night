using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string mapName;

    public Player player;
    public GameObject boss;

    public bool isSave;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(boss != null && boss.activeSelf == false)
        {
            isSave = true;
            player.mapName = SceneManager.GetActiveScene().name + '>' + mapName + "StartPoint";
            SceneManager.LoadScene(mapName);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.mapName = SceneManager.GetActiveScene().name + '>' + mapName + "StartPoint";
            SceneManager.LoadScene(mapName);
        }
    }
}