using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject boss;

    void Start()
    {
        //플레이어가 튜토리얼 보스 씬으로 이동 시 보스 생성
        boss.SetActive(true);
    }
}
