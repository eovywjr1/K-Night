using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMoving_Player : MonoBehaviour
{
    public GameObject target; // 카메라가 따라갈 대상
    public float moveSpeed; // 카메라가 따라갈 속도
    public float leftEnd;
    public float rightEnd;
    private Vector3 targetPosition; // 대상의 현재 위치

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Village_FirstEnding" || SceneManager.GetActiveScene().name == "Village_SecondEnding")
        {
            // 엔딩 장면 진입이 딱딱한 느낌이 있어서, 엔딩 장면에서는 카메라의 속도를 이전처럼 4로 설정했습니다.
            moveSpeed = 4;
        }
        else
        {
            moveSpeed = 1000;
        }
    }

    void Update()
    {
        if (GameObject.Find("Player") != null)
        {
            target = GameObject.Find("Player");
        }
        else
        {
            target = null;
        }

        // 대상이 있는지 체크
        if (target.gameObject != null)
        {
            // this는 카메라를 의미 (z값은 카메라값을 그대로 유지)
            targetPosition.Set(target.transform.position.x, this.transform.position.y, this.transform.position.z);
            if (targetPosition.x < leftEnd)
                targetPosition.x = leftEnd;
            else if (targetPosition.x > rightEnd)
                targetPosition.x = rightEnd;

            // vectorA -> B까지 T의 속도로 이동
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
        }
    }
}
