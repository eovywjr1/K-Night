using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject boss;
    public GameObject barricade;
    public GameObject mapTransfer;

    void Start()
    {
        //플레이어가 튜토리얼 보스 씬으로 이동 시 보스 생성
        if (boss != null)
            boss.SetActive(true);
    }

    void Update()
    {
        //aftertutorial에서 beforecombet 씬으로 이동하는 오브젝트 활성화
        if (barricade != null && mapTransfer != null && barricade.activeSelf == false)
            mapTransfer.SetActive(true);

        //tutorial 보스에서 보스가 죽었을 때 오브젝트 활성화
        if(boss != null && mapTransfer != null && boss.activeSelf == false)
            mapTransfer.SetActive(true);
    }
}
