using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public Player player;
    public string sceneName;

    void Start()
    {        
        player = FindObjectOfType<Player>();
        sceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (sceneName == "TutorialBoss")
        {
            if (player.transform.position.x > -17.3 && this.gameObject.transform.position.x < 13)
                this.gameObject.transform.position = new Vector3(player.transform.position.x, this.gameObject.transform.position.y, -1);
        }
    }
}
