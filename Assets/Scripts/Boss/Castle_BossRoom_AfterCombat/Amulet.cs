using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : MonoBehaviour
{
    public bool getAmulet = false;
    Vector3 pos = new Vector3(0, -1, 0);

    private void Update()
    {
        Debug.Log("�۵���");
        if (getAmulet)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, pos, 1.5f * Time.deltaTime) ;
        }
        if (this.transform.position.x < 0.1)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("������");
        if (collision.CompareTag("Player"))
        {
            getAmulet = true;
        }
    }
}
