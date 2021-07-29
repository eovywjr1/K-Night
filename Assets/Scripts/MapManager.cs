using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string mapName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ĳ���Ͱ� ���̵� ������Ʈ�� ����� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(mapName);
        }
    }
}