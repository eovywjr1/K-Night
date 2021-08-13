using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : GameManager
{
    public string startPointName;

    public Player player;
    public MapManager mapManager;

    void Awake()
    {
        startPointName = this.gameObject.name;
        player = FindObjectOfType<Player>();
    }

    void start()
    {
        if (startPointName == player.mapName)
            player.transform.position = this.gameObject.transform.position;

        if (mapManager.isSave)
        {
            Save(player);
            mapManager.isSave = false;
        }
    }
}