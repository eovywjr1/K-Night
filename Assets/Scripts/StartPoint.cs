using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPointName;

    public Player player;

    void Start()
    {
        startPointName = this.gameObject.name;
        player = FindObjectOfType<Player>();

        if (startPointName == player.mapName)
            player.transform.position = this.gameObject.transform.position;
    }
}