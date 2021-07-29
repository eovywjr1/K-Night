using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string mapName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //캐릭터가 맵이동 오브젝트에 닿았을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(mapName);
        }
    }
}