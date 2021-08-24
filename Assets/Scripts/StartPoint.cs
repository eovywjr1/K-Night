using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : GameManager
{
    public string startPointName;

    void Awake()
    {
        startPointName = this.gameObject.name;
        player = FindObjectOfType<Player>();

        if (startPointName == player.mapName)
            player.transform.position = this.gameObject.transform.position;
    }

    void Update()
    {
        if (player.isSave)
        {
            Save(player);
            player.isSave = false;
        }
    }
}