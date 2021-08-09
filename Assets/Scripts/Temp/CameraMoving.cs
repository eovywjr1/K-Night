using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public GameObject target; // ī�޶� ���� ���
    public float moveSpeed; // ī�޶� ���� �ӵ�
    public float leftEnd;
    public float rightEnd;
    private Vector3 targetPosition; // ����� ���� ��ġ



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

        // ����� �ִ��� üũ
        if (target.gameObject != null)
        {
            // this�� ī�޶� �ǹ� (z���� ī�޶��� �״�� ����)
            targetPosition.Set(target.transform.position.x, this.transform.position.y, this.transform.position.z);
            if (targetPosition.x < leftEnd)
                targetPosition.x = leftEnd;
            else if (targetPosition.x > rightEnd)
                targetPosition.x = rightEnd;

            // vectorA -> B���� T�� �ӵ��� �̵�
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
        }
    }
}
