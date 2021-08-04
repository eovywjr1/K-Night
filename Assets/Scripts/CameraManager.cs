using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        this.gameObject.transform.position = new Vector3(player.transform.position.x, this.gameObject.transform.position.y, -1);
    }

    void Update()
    {
        if (player.transform.position.x > -22 && this.gameObject.transform.position.x < 18)
            this.gameObject.transform.position = new Vector3(player.transform.position.x, this.gameObject.transform.position.y, -1);
    }
}
