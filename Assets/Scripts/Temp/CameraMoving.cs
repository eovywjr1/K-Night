using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public GameObject target; // ī�޶� ���� ���
    public float moveSpeed; // ī�޶� ���� �ӵ�
    private Vector3 targetPosition; // ����� ���� ��ġ

    void Update()
    {
        // ����� �ִ��� üũ
        if (target.gameObject != null)
        {
            // this�� ī�޶� �ǹ� (z���� ī�޶��� �״�� ����)
            targetPosition.Set(target.transform.position.x, this.transform.position.y, this.transform.position.z);
            if (targetPosition.x < -22)
                targetPosition.x = -22;
            else if (targetPosition.x > 22)
                targetPosition.x = 22;

            // vectorA -> B���� T�� �ӵ��� �̵�
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
